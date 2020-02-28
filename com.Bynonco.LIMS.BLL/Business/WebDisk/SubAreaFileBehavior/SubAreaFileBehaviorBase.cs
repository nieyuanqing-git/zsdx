using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.DAL;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL.SubAreaFileBehavior
{
    public abstract class SubAreaFileBehaviorBase : ISubAreaFileBehaviorBase
    {
        XTransaction tran = null;
        protected ISubAreaBLL subAreaBLL = BLLFactory.CreateSubAreaBLL();
        protected ISubAreaCategoryBLL subAreaCategoryBLL = BLLFactory.CreateSubAreaCategoryBLL();
        protected ISubAreaFileBLL subAreaFileBLL = BLLFactory.CreateSubAreaFileBLL();
        protected ISubAreaTagPowerBLL subAreaTagPowerBLL = BLLFactory.CreateSubAreaTagPowerBLL();
        protected ISubAreaUerPowerBLL subAreaUerPowerBLL = BLLFactory.CreateSubAreaUerPowerBLL();
        protected ISubAreaUserWithoutPowerBLL subAreaUserWithoutPowerBLL = BLLFactory.CreateSubAreaUserWithoutPowerBLL();
        protected User operateUser = null;
        public SubAreaFileBehaviorBase(User operateUser)
        {
            this.operateUser = operateUser;
        }

        public virtual bool Add(IEnumerable<SubAreaFile> subAreaFiles, ref string message)
        {
            XTransaction tran = null;
            try
            {
                foreach (var subAreaFile in subAreaFiles)
                {
                    if (!AddValidates(subAreaFile, ref message)) return false;
                    if (!Validates(subAreaFile, ref message)) return false;
                }
                return subAreaFileBLL.Add(subAreaFiles, ref tran);
            }
            catch (Exception ex)
            {
                message = string.Format("出错,出错信息:{0}", ex.Message);
                return false;
            }
        }

        public virtual bool Edit(IEnumerable<SubAreaFile> subAreaFiles, ref string message)
        {
            XTransaction tran = null;
            try
            {
                foreach (var subAreaFile in subAreaFiles)
                {
                    if (!EditValidates(subAreaFile, ref message)) return false;
                    if (!Validates(subAreaFile, ref message)) return false;
                }
                return subAreaFileBLL.Save(subAreaFiles, ref tran);
            }
            catch (Exception ex)
            {
                message = string.Format("出错,出错信息:{0}", ex.Message);
                return false;
            }
        }

        private bool DeleteSubAreaFile(SubAreaFile subAreaFile, IList<Guid> lstDeteledId,ref XTransaction tran)
        {
            if (subAreaFile.Childrens != null && subAreaFile.Childrens.Count > 0)
            {
                foreach (var children in subAreaFile.Childrens)
                {
                    DeleteSubAreaFile(children,lstDeteledId, ref tran);
                    if (!lstDeteledId.Any(p => p == children.Id))
                    {
                        subAreaFileBLL.Delete(new Guid[] { children.Id }, ref tran, true);
                        lstDeteledId.Add(children.Id);
                    }
                }
            }
            if (!lstDeteledId.Any(p => p == subAreaFile.Id))
            {
                subAreaFileBLL.Delete(new Guid[] { subAreaFile.Id }, ref tran, true);
                lstDeteledId.Add(subAreaFile.Id);
            }
           
            return true;
        }

        public virtual bool Delete(IEnumerable<Guid> ids, ref string message)
        {
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            IList<SubAreaFile> lstSubAreaFiles = new List<SubAreaFile>();
            IList<Guid> lstDeteledId = new List<Guid>();
            try
            {
                foreach (var id in ids)
                {
                    var subAreaFile = subAreaFileBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).FirstOrDefault();
                    if (!DeleteValidates(subAreaFile, ref message)) return false;
                    lstSubAreaFiles.Add(subAreaFile);
                }
                foreach (var subAreaFile in lstSubAreaFiles)
                    DeleteSubAreaFile(subAreaFile, lstDeteledId,ref tran);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                message = string.Format("出错,出错信息:{0}", ex.Message);
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
        }

        public virtual bool Move(IEnumerable<SubAreaFile> sourceSubAreaFiles, Model.SubAreaFile targetSubAreaFile, ref string message)
        {
             XTransaction tran = null;
            try
            {
                foreach (var sourceSubAreaFile in sourceSubAreaFiles)
                {
                    if (!Validates(sourceSubAreaFile, ref message)) return false;
                    if (!Validates(targetSubAreaFile, ref message)) return false;
                    sourceSubAreaFile.ParentId = targetSubAreaFile.Id;
                }
                return subAreaFileBLL.Save(sourceSubAreaFiles, ref tran);
            }
            catch (Exception ex)
            {
                message = string.Format("出错,出错信息:{0}", ex.Message);
                return false;
            }
        }

        public abstract bool AddValidates(Model.SubAreaFile subAreaFile, ref string message);

        public abstract bool EditValidates(Model.SubAreaFile subAreaFile, ref string message);

        public abstract bool DeleteValidates(SubAreaFile subAreaFile, ref string message);

        public virtual bool MoveValidates(Model.SubAreaFile sourceSubAreaFile, Model.SubAreaFile targetSubAreaFile, ref string message)
        {
            try
            {
                if (targetSubAreaFile.IsFold)
                {
                    message = string.Format("目标【{0}】不是文件夹", targetSubAreaFile.FileName);
                    return false;
                }
                if (!sourceSubAreaFile.SubArea.IsOwner(operateUser))
                {
                    if (!sourceSubAreaFile.SubArea.IsHasPower(operateUser))
                    {
                        message = string.Format("对【{0}】无操作权限", sourceSubAreaFile.FileName);
                        return false;
                    }
                }
                if (!targetSubAreaFile.SubArea.IsOwner(operateUser))
                {
                    if (!targetSubAreaFile.SubArea.IsHasPower(operateUser))
                    {
                        message = string.Format("对【{0}】无操作权限", targetSubAreaFile.FileName);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                message = string.Format("出错,出错信息:{0}", ex.Message);
                return false;
            }
            return true;
        }

        public virtual bool Validates(Model.SubAreaFile subAreaFile, ref string message)
        {
            if (subAreaFile.CreateTime == DateTime.MinValue || subAreaFile.CreateTime == DateTime.MaxValue)
            {
                message = "创建时间不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(subAreaFile.FileName) || subAreaFile.FileName.Trim() == "")
            {
                message = "文件名不能为空";
                return false;
            }
            if (!subAreaFile.IsFold )
            {
                if (subAreaFile.FileSize > subAreaFile.SubArea.LeftSize)
                {
                    message = "文件大小大于分区文件所在分区可用容量";
                    return false;
                }
            }
            if (subAreaFile.FileSize == 0d && !subAreaFile.IsFold)
            {
                message = "文件路径不能为空";
                return false;
            }
            if (subAreaFile.OwerId == null || subAreaFile.OwerId == new Guid())
            {
                message = "创建人编码不能为空";
                return false;
            }
            if (subAreaFile.SubAreaId == null || subAreaFile.SubAreaId == new Guid())
            {
                message = "编码不能为空";
                return false;
            }
            if (subAreaFile.SubArea == null)
            {
                message = "所在文件分区不能为空";
                return false;
            }
            if (subAreaFile.SubArea.IsStop)
            {
                message = "所在文件分区已停用";
                return false;
            }
            if (!subAreaFile.SubArea.IsHasPower(operateUser))
            {
                message = "无操作权限";
                return false;
            }
            if (subAreaFile.SubArea.SubAreaCategory == null)
            {
                message = "所在文件分区分类不能为空";
                return false;
            }
            if (subAreaFile.SubArea.SubAreaCategory.IsStop)
            {
                message = "所在文件分区分类已停用";
                return false;
            }
            return true;
        }

        protected bool IsSubAreaFileOwner(SubAreaFile subAreaFile)
        {
            bool isOwner = false;
            isOwner = subAreaFile.IsOwner(operateUser);
            if (!isOwner) return false;
            if (subAreaFile.Parent != null)
            {
                isOwner = subAreaFile.Parent.IsOwner(operateUser);
                if (!isOwner) return false;
                IsSubAreaFileOwner(subAreaFile.Parent);
            }
            return isOwner;
        }
        
    }
}
