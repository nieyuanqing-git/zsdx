using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using log4net;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.SubAreaBehavior
{
    public abstract class SubAreaBehaviorBase : ISubAreaBehavior
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(SubAreaBLL));
        protected IUserBLL userBLL = BLLFactory.CreateUserBLL();
        protected ISubAreaBLL subAreaBLL = BLLFactory.CreateSubAreaBLL();
        protected ISubAreaTagPowerBLL subAreaTagPowerBLL = BLLFactory.CreateSubAreaTagPowerBLL();
        protected ISubAreaUerPowerBLL subAreaUerPowerBLL = BLLFactory.CreateSubAreaUerPowerBLL();
        protected ISubAreaUserWithoutPowerBLL subAreaUserWithoutPowerBLL = BLLFactory.CreateSubAreaUserWithoutPowerBLL();
        protected ISubAreaFileBLL subAreaFileBLL = BLLFactory.CreateSubAreaFileBLL();
        protected ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
        protected IExperimenterSubjectBLL experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
        protected ITagBLL tagBLL = BLLFactory.CreateTagBLL();

        public virtual double GetTotalSizeBySubAreaId(Guid subAreaId)
        {
            return subAreaBLL.GetTotalSizeBySubAreaId(subAreaId);
        }

        public virtual IList<Model.SubArea> GetSubAreasByUserId(Guid userId)
        {
            return subAreaBLL.GetSubAreasByUserId(userId);
        }

        public virtual IList<Model.SubArea> GetSubAreasByUserId(Guid userId, Guid subAreaCategoryId)
        {
            return subAreaBLL.GetSubAreasByUserId(userId, subAreaCategoryId);
        }

        public virtual IList<Model.SubArea> GetSubAreasByCondition(string ownerName, string subAreaCategeryName, string subAreaName, string subAreaCategeryId)
        {
            return subAreaBLL.GetSubAreasByCondition(ownerName, subAreaCategeryName, subAreaName, subAreaCategeryId);
        }

        public abstract bool Add(IEnumerable<SubArea> entities, ref august.DataLink.XTransaction tran, bool isSupress = false);


        public virtual bool Save(IEnumerable<SubArea> entities, ref august.DataLink.XTransaction tran, bool isSupress = false)
        {
            return subAreaBLL.Save(entities, ref tran, false); 
        }

        public virtual bool Delete(IEnumerable<Guid> ids, ref august.DataLink.XTransaction tran, bool isSupress = false)
         {
             tran = august.DataLink.SessionManager.Instance.BeginTransaction();
             try
             {
                 List<Guid> lstSubAreaIds = new List<Guid>();
                 List<Guid> lstSubAreaTagPowerIds = new List<Guid>();
                 List<Guid> lstSubAreaUserPowerIds = new List<Guid>();
                 List<Guid> lstSubAreaWithoutPowerIds = new List<Guid>();
                 List<Guid> lstSubAreaFileIds = new List<Guid>();
                 foreach (var id in ids)
                 {
                     var subArea = subAreaBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).FirstOrDefault();
                     if (subArea == null) continue;
                     lstSubAreaIds.Add(id);
                     if (subArea.SubAreaUerPowers != null && subArea.SubAreaUerPowers.Count > 0)
                         lstSubAreaUserPowerIds.AddRange(subArea.SubAreaUerPowers.Select(p => p.Id));
                     if (subArea.SubAreaUserWithoutPowers != null && subArea.SubAreaUserWithoutPowers.Count > 0)
                         lstSubAreaWithoutPowerIds.AddRange(subArea.SubAreaUserWithoutPowers.Select(p => p.Id));
                     if (subArea.SubAreaTagPowers != null && subArea.SubAreaTagPowers.Count > 0)
                         lstSubAreaTagPowerIds.AddRange(subArea.SubAreaTagPowers.Select(p => p.Id));
                     var subAreaFile = subAreaFileBLL.GetEntitiesByExpression(string.Format("SubAreaId=\"{0}\"", subArea.Id.ToString()));
                     if (subAreaFile != null && subAreaFile.Count > 0)
                         lstSubAreaFileIds.AddRange(subAreaFile.Select(p => p.Id));
                 }
                 if (lstSubAreaTagPowerIds.Count > 0)
                     subAreaTagPowerBLL.Delete(lstSubAreaTagPowerIds.ToArray(), ref tran, true);
                 if (lstSubAreaWithoutPowerIds.Count > 0)
                     subAreaUserWithoutPowerBLL.Delete(lstSubAreaWithoutPowerIds.ToArray(), ref tran, true);
                 if (lstSubAreaUserPowerIds.Count > 0)
                     subAreaUerPowerBLL.Delete(lstSubAreaUserPowerIds.ToArray(), ref tran, true);
                 if (lstSubAreaFileIds.Count > 0)
                     subAreaFileBLL.Delete(lstSubAreaFileIds.ToArray(), ref tran, true);
                 if (lstSubAreaIds.Count > 0) subAreaBLL.Delete(lstSubAreaIds.ToArray(), ref tran, true);
                 tran.CommitTransaction();
                 return true;
             }
             catch (Exception ex)
             {
                 log.Error(string.Format("{0}:{1}", "Delete", ex.Message));
                 tran.RollbackTransaction();
                 return false;
             }
             finally { tran.Dispose(); }
         }
    }
}
