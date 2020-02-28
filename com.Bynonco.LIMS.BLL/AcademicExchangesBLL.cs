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
    public class AcademicExchangesBLL : BLLBase<AcademicExchanges>, IAcademicExchangesBLL
    {
        private IAcademicExchangesDepBLL __academicExchangesDepBLL;
        private IAcademicExchangesUserBLL __academicExchangesUserBLL;
        private IAttachmentBLL __attachmentBLL;
        public AcademicExchangesBLL()
        {
            __academicExchangesDepBLL = BLLFactory.CreateAcademicExchangesDepBLL();
            __academicExchangesUserBLL = BLLFactory.CreateAcademicExchangesUserBLL();
            __attachmentBLL = BLLFactory.CreateAttachmentBLL();
            
        }
        public bool DeleteAcademicExchanges(AcademicExchanges academicExchanges)
        {
            if (academicExchanges == null) return false;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var academicExchangesDepartmentList = __academicExchangesDepBLL.GetEntitiesByExpression(string.Format("AcademicExchangesId=\"{0}\"", academicExchanges.Id));
                if (academicExchangesDepartmentList != null && academicExchangesDepartmentList.Count() > 0)
                {
                    __academicExchangesDepBLL.Delete(academicExchangesDepartmentList.Select(m => m.id), ref tran, true);
                }
                var academicExchangesUserList = __academicExchangesUserBLL.GetEntitiesByExpression(string.Format("AcademicExchangesId=\"{0}\"", academicExchanges.Id));
                if (academicExchangesUserList != null && academicExchangesUserList.Count() > 0)
                {
                    __academicExchangesUserBLL.Delete(academicExchangesUserList.Select(m => m.id), ref tran, true);
                }
                var attachmentList = __attachmentBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", academicExchanges.Id));
                if (attachmentList != null && attachmentList.Count() > 0)
                {
                    __attachmentBLL.Delete(attachmentList.Select(m => m.id), ref tran, true);
                }
                Delete(new Guid[] { academicExchanges.Id }, ref tran, true);
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