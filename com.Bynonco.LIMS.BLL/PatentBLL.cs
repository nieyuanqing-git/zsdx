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
    public class PatentBLL : BLLBase<Patent>, IPatentBLL
    {
        private IAttachmentBLL __attachmentBLL;
        private IPatentUserBLL __patentUserBLL;
        private IPatentEquipmentBLL __patentEquipmentBLL;
        public PatentBLL()
        {
            __attachmentBLL = BLLFactory.CreateAttachmentBLL();
            __patentUserBLL = BLLFactory.CreatePatentUserBLL();
            __patentEquipmentBLL = BLLFactory.CreatePatentEquipmentBLL();
        }
        public bool DeletePatent(Patent patent)
        {
            if (patent == null) return false;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var patentUserList = __patentUserBLL.GetEntitiesByExpression(string.Format("PatentId=\"{0}\"", patent.Id));
                if (patentUserList != null && patentUserList.Count() > 0)
                    __patentUserBLL.Delete(patentUserList.Select(m => m.id), ref tran, true);
                var patentEquipmentList = __patentEquipmentBLL.GetEntitiesByExpression(string.Format("PatentId=\"{0}\"", patent.Id));
                if (patentEquipmentList != null && patentEquipmentList.Count() > 0)
                {
                    __patentEquipmentBLL.Delete(patentEquipmentList.Select(m => m.id), ref tran, true);
                }
                var attachmentList = __attachmentBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", patent.Id));
                if (attachmentList != null && attachmentList.Count() > 0)
                    __attachmentBLL.Delete(attachmentList.Select(m => m.id), ref tran, true);
                Delete(new Guid[] { patent.Id }, ref tran, true);
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