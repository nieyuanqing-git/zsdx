using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class AwardsBLL : BLLBase<Awards>, IAwardsBLL
    {
        private IAwardsDepartmentBLL __awardsDepartmentBLL;
        private IAwardsAuthorBLL __awardsAuthorBLL;
        private IAwardsEquipmentBLL __awardsEquipmentBLL;
        private IAttachmentBLL __attachmentBLL;
        public AwardsBLL()
        {
            __awardsDepartmentBLL = BLLFactory.CreateAwardsDepartmentBLL();
            __awardsAuthorBLL = BLLFactory.CreateAwardsAuthorBLL();
            __awardsEquipmentBLL = BLLFactory.CreateAwardsEquipmentBLL();
            __attachmentBLL = BLLFactory.CreateAttachmentBLL();
            
        }
        public bool DeleteAwards(Awards awards)
        {
            if (awards == null) return false;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var awardsDepartmentList = __awardsDepartmentBLL.GetEntitiesByExpression(string.Format("AwardsId=\"{0}\"", awards.Id));
                if (awardsDepartmentList != null && awardsDepartmentList.Count() > 0)
                {
                    __awardsDepartmentBLL.Delete(awardsDepartmentList.Select(m => m.id), ref tran, true);
                }
                var awardsAuthorList = __awardsAuthorBLL.GetEntitiesByExpression(string.Format("AwardsId=\"{0}\"", awards.Id));
                if (awardsAuthorList != null && awardsAuthorList.Count() > 0)
                {
                    __awardsAuthorBLL.Delete(awardsAuthorList.Select(m => m.id), ref tran, true);
                }
                var awardsEquipmentList = __awardsEquipmentBLL.GetEntitiesByExpression(string.Format("AwardsId=\"{0}\"", awards.Id));
                if (awardsEquipmentList != null && awardsEquipmentList.Count() > 0)
                {
                    __awardsEquipmentBLL.Delete(awardsEquipmentList.Select(m => m.id), ref tran, true);
                }
                var attachmentList = __attachmentBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", awards.Id));
                if (attachmentList != null && attachmentList.Count() > 0)
                {
                    __attachmentBLL.Delete(attachmentList.Select(m => m.id), ref tran, true);
                }
                Delete(new Guid[] { awards.Id }, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
    }
}