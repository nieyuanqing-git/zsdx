using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class ShareFundApplyBLL : BLLBase<ShareFundApply>, IShareFundApplyBLL
    {
        private IShareFundApplyEquipmentBLL __shareFundApplyEquipmentBLL;
        public ShareFundApplyBLL()
        {
            __shareFundApplyEquipmentBLL = BLLFactory.CreateShareFundApplyEquipmentBLL();
        }
        private bool DelShareFundApply(Guid userId, Guid shareFundApplyId, ref XTransaction tran, out string errorMessage)
        {
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var shareFundApply = GetEntityById(shareFundApplyId);
                if (shareFundApply == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的共享基金信息", shareFundApplyId));
                shareFundApply.IsDelete = true;
                var itemList = __shareFundApplyEquipmentBLL.GetEntitiesByExpression(string.Format("ShareFundApplyId=\"{0}\"*IsDelete=false", shareFundApplyId), null, "", true);
                if (itemList != null && itemList.Count() > 0)
                {
                    foreach (var item in itemList)
                        item.IsDelete = true;
                    __shareFundApplyEquipmentBLL.Save(itemList, ref tran, true);
                }
                Save(new ShareFundApply[] { shareFundApply }, ref tran, true);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool DelShareFundApply(Guid userId, Guid shareFundApplyId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelShareFundApply(userId, shareFundApplyId, ref tran, out errorMessage);
        }
        public bool DelShareFundApplys(Guid userId, IList<ShareFundApply> shareFundApplys, out  string errorMessage)
        {
            errorMessage = "";
            if (shareFundApplys != null && shareFundApplys.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var shareFundApply in shareFundApplys)
                    {
                        var result = DelShareFundApply(userId, shareFundApply.Id, ref tran, out errorMessage);
                        if (!result)
                            throw new Exception(errorMessage);
                    }
                    if (tran.HasTransaction) tran.CommitTransaction();
                }
                catch (Exception ex)
                {
                    if (tran.HasTransaction) tran.CommitTransaction();
                    errorMessage = ex.Message;
                    return false;
                }
                finally { tran.Dispose(); }
            }
            return true;
        }
        public bool SaveShareFundApply(ShareFundApply shareFundApply, IList<ShareFundApplyEquipment> shareFundApplyEquipmentList, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                if (shareFundApply == null || shareFundApplyEquipmentList == null || shareFundApplyEquipmentList.Count() == 0) throw new Exception("共享基金和设备信息不完整");
                tran = SessionManager.Instance.BeginTransaction();
                var oldShareFundApply = GetEntityById(shareFundApply.Id);
                if (oldShareFundApply != null)
                {
                    var delShareFundApplyEquipmentList = __shareFundApplyEquipmentBLL.GetEntitiesByExpression(
                        string.Format("ShareFundApplyId=\"{0}\"*IsDelete=false*{1}", oldShareFundApply.Id, shareFundApplyEquipmentList.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot)));
                    if (delShareFundApplyEquipmentList != null && delShareFundApplyEquipmentList.Count() > 0)
                    {
                        foreach (var item in delShareFundApplyEquipmentList)
                            item.IsDelete = true;
                        __shareFundApplyEquipmentBLL.Save(delShareFundApplyEquipmentList, ref tran, true);
                    }
                    Save(new ShareFundApply[] { shareFundApply }, ref tran, true);
                }
                else
                    Add(new ShareFundApply[] { shareFundApply }, ref tran, true);

                foreach (var item in shareFundApplyEquipmentList)
                {
                    var oldItem = __shareFundApplyEquipmentBLL.GetEntityById(item.Id) ;
                    if (oldItem != null)
                        __shareFundApplyEquipmentBLL.Save(new ShareFundApplyEquipment[] { item }, ref tran, true);
                    else
                        __shareFundApplyEquipmentBLL.Add(new ShareFundApplyEquipment[] { item }, ref tran, true);
                }
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null) tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错" + ex.Message : ex.Message;
                return false;
            }
            finally { if (tran != null)tran.Dispose(); }
        }
    }
}
