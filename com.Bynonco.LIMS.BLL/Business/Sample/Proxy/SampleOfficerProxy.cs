using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleOfficerProxy: SampleProxy
    {
        public SampleOfficerProxy(Guid userId)
            : base(userId, new SampleOfficerFactory(userId)) { }
        public override Model.UserEquipmentPrivilige GetUserEquipmentPrivilige(Model.View.ViewSampleApply viewSampleApply)
        {
            var userSamplePrivilige = viewSampleApply == null ?
               _userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserId(_user.Id) :
               _userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserItemId(_user.Id, viewSampleApply.SampleItemId);
            if(userSamplePrivilige == null) userSamplePrivilige =  _samplePrivilige.ToUserEquipmentPrivilige();
            userSamplePrivilige.IsEnableHandleInner = false;
            userSamplePrivilige.IsEnableHandleOuter = true;
            return  userSamplePrivilige;
        }
        public override void DoPriviligeFacade(Model.View.ViewSampleApply viewSampleApply)
        {
            string errorMessage = "";
            if (!viewSampleApply.IsOuter) viewSampleApply.EditOperate = "";
            if (!_viewSampleApplyBLL.JudgeIsEnableReportConfirm(_user.Id, viewSampleApply.SampleStatus, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.ReportConfirmOperate = "";
            }
            if (!_viewSampleApplyBLL.JudgeIsEnableAccept(_user.Id, viewSampleApply.IsNeedConfirmAcceptSample, viewSampleApply.IsNeedTutorAudit, (AuditStatus)viewSampleApply.TutorAuditStatus, viewSampleApply.SampleStatus, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.AcceptOperate = "";
            }
            if (!_viewSampleApplyBLL.JudgeIsEnableRefuseAccept(_user.Id, viewSampleApply.IsNeedConfirmAcceptSample, viewSampleApply.IsNeedTutorAudit, (AuditStatus)viewSampleApply.TutorAuditStatus, viewSampleApply.SampleStatus, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.RefuseAcceptOperate = "";
            }
            //if (!viewSampleApply.IsNeedConfirmAcceptSample)
            //{
            //    viewSampleApply.AcceptOperate = "";
            //    viewSampleApply.RefuseOperate = "";
            //    if (viewSampleApply.SampleStatus != Model.Enum.SampleApplyStatus.UnAudit)
            //    {
            //        viewSampleApply.AuditOperate = "";
            //    }
            //}
            //else if (viewSampleApply.SampleStatus != Model.Enum.SampleApplyStatus.Accepted)
            //{
            //     viewSampleApply.AuditOperate = "";
            //}
            //if (viewSampleApply.SampleStatus !=  Model.Enum.SampleApplyStatus.UnAudit)
            //{
            //    viewSampleApply.EditOperate = "";
            //    viewSampleApply.CancelOperate = "";
            //    viewSampleApply.AcceptOperate = "";
            //    viewSampleApply.RefuseOperate = "";
            //}
            //if (viewSampleApply.IsNeedTutorAudit && viewSampleApply.TutorAuditStatus != (int)AuditStatus.Passed)
            //{
            //    viewSampleApply.AcceptOperate = "";
            //    viewSampleApply.RefuseOperate = "";
            //    viewSampleApply.ReportConfirmOperate = "";
            //}
            //if (viewSampleApply.SampleStatus != Model.Enum.SampleApplyStatus.Obtained && viewSampleApply.SampleStatus != Model.Enum.SampleApplyStatus.Released)
            //{
            //    viewSampleApply.ReportConfirmOperate = "";
            //} 
        }
        protected override bool ApplyHook { get { return true; } }
        protected override bool CopyHook { get { return true; } }
        protected override bool EditHook { get { return true; } }

        public override string BuildOperate(Model.View.ViewSampleApply viewSampleApply)
        {
            StringBuilder sbOperate = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ViewSampleApplyInfoOperate))
                sbOperate.Append(viewSampleApply.ViewSampleApplyInfoOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.SendMessageOperate))
                sbOperate.Append(viewSampleApply.SendMessageOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.EditOperate))
                sbOperate.Append(viewSampleApply.EditOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.AcceptOperate))
                sbOperate.Append(viewSampleApply.AcceptOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.RefuseAcceptOperate))
                sbOperate.Append(viewSampleApply.RefuseAcceptOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ReportConfirmOperate))
                sbOperate.Append(viewSampleApply.ReportConfirmOperate);
            return sbOperate.ToString();
        }
    }
}
