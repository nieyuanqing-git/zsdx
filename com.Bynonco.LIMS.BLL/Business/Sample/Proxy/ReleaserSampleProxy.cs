using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class ReleaserSampleProxy: SampleProxy
    {
        public ReleaserSampleProxy(Guid userId)
            : base(userId, new ReleaserSampleFactory(userId)) { }
        public override void DoPriviligeFacade(Model.View.ViewSampleApply viewSampleApply)
        {
            string errorMessage = "";
            if (!_viewSampleApplyBLL.JudgeIsEnableReleaseResult(_user.Id, viewSampleApply.SampleStatus, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.ReleaseOperate = "";
            }
        }
         
        public override string BuildOperate(Model.View.ViewSampleApply viewSampleApply)
        {
            StringBuilder sbOperate = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ViewSampleApplyInfoOperate))
                sbOperate.Append(viewSampleApply.ViewSampleApplyInfoOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.SendMessageOperate))
                sbOperate.Append(viewSampleApply.SendMessageOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ReleaseOperate))
                sbOperate.Append(viewSampleApply.ReleaseOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.SendResultOperate))
                sbOperate.Append(viewSampleApply.SendResultOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ReportNumberOperate))
                sbOperate.Append(viewSampleApply.ReportNumberOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ChargeOperate))
                sbOperate.Append(viewSampleApply.ChargeOperate);
            return sbOperate.ToString();
        }
    }
}
