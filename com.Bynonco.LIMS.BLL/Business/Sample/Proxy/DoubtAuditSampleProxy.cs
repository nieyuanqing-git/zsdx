using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class DoubtAuditSampleProxy: SampleProxy
    {
        public DoubtAuditSampleProxy(Guid userId)
            : base(userId, new DoubtAuditSampleFactory(userId)) { }
        public override void DoPriviligeFacade(Model.View.ViewSampleApply viewSampleApply)
        {
            string errorMessage = "";
            if (!_viewSampleApplyBLL.JudgeIsEnableAuditDoubts(_user.Id, viewSampleApply.SampleStatus, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.AuditDoubtsOperate = "";
            }
        }
        public override string BuildOperate(Model.View.ViewSampleApply viewSampleApply)
        {
            StringBuilder sbOperate = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ViewSampleApplyInfoOperate))
                sbOperate.Append(viewSampleApply.ViewSampleApplyInfoOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.SendMessageOperate))
                sbOperate.Append(viewSampleApply.SendMessageOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.AuditDoubtsOperate))
                sbOperate.Append(viewSampleApply.AuditDoubtsOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ChargeOperate))
                sbOperate.Append(viewSampleApply.ChargeOperate);
            return sbOperate.ToString();
        }
    }
}
