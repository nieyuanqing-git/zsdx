using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class ApplicantSampleProxy : SampleProxy
    {
        public ApplicantSampleProxy(Guid userId)
            : base(userId, new ApplicantSampleFactory(userId)) { }

        public override void DoPriviligeFacade(Model.View.ViewSampleApply viewSampleApply)
        {
            string errorMessage = "";
            if (!_viewSampleApplyBLL.JudgeIsEnableDownLoad(_user.Id, viewSampleApply.ReportObtainerId, viewSampleApply.ApplicantId, viewSampleApply.SampleStatus, DownLoadHook, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.DownLoadOperate = "";
            }
            if (!_viewSampleApplyBLL.JudgeIsEnableDoubts(_user.Id, viewSampleApply.ApplicantId, viewSampleApply.SampleStatus, DoubtsHook, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.DoubtsOperate = "";
            }
            if (viewSampleApply.AuditTutorId.HasValue || !viewSampleApply.IsNeedTutorAudit || _user.Id != viewSampleApply.SubjectDirectorId.Value || viewSampleApply.SampleStatus != SampleApplyStatus.UnAudit)
            {
                viewSampleApply.TutorAuditNotPassOperate = "";
                viewSampleApply.TutorAuditPassOperate = "";
            }
            if (!_viewSampleApplyBLL.JudgeIsEnableFeedBack(_user.Id, viewSampleApply.IsNeedFeedBack, viewSampleApply.ApplicantId, viewSampleApply.SampleStatus, out errorMessage))
            {
                viewSampleApply.FeedBackOperate = "";
            }
        }
        public override string BuildOperate(Model.View.ViewSampleApply viewSampleApply)
        {
            StringBuilder sbOperate = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ViewSampleApplyInfoOperate)) sbOperate.Append(viewSampleApply.ViewSampleApplyInfoOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.EditOperate)) sbOperate.Append(viewSampleApply.EditOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.TutorAuditPassOperate)) sbOperate.Append(viewSampleApply.TutorAuditPassOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.TutorAuditNotPassOperate)) sbOperate.Append(viewSampleApply.TutorAuditNotPassOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.CopyOperate)) sbOperate.Append(viewSampleApply.CopyOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.CancelOperate)) sbOperate.Append(viewSampleApply.CancelOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.DownLoadOperate)) sbOperate.Append(viewSampleApply.DownLoadOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.DoubtsOperate)) sbOperate.Append(viewSampleApply.DoubtsOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.PrintOperate)) sbOperate.Append(viewSampleApply.PrintOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.FeedBackOperate)) sbOperate.Append(viewSampleApply.FeedBackOperate);
            return sbOperate.ToString();
        }
        protected override bool ApplyHook { get { return true; } }
        protected override bool CopyHook { get { return true; } }
        protected override bool EditHook { get { return true; } }
        protected override bool CancelHook { get { return true; } }
        protected override bool DownLoadHook { get { return true; } }
        protected override bool DoubtsHook { get { return true; } }
        protected override bool ViewHook { get { return true; } } 
    }
}
