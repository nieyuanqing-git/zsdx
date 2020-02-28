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
    public class AcademicPositionBLL : BLLBase<AcademicPosition>, IAcademicPositionBLL
    {
        private IAttachmentBLL __attachmentBLL;
        public AcademicPositionBLL()
        {
            __attachmentBLL = BLLFactory.CreateAttachmentBLL();
        }
        public bool DeleteAcademicPosition(AcademicPosition academicPosition)
        {
            if (academicPosition == null) return false;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var attachmentList = __attachmentBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", academicPosition.Id));
                if (attachmentList != null && attachmentList.Count() > 0)
                {
                    __attachmentBLL.Delete(attachmentList.Select(m => m.id), ref tran, true);
                }
                Delete(new Guid[] { academicPosition.Id }, ref tran, true);
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