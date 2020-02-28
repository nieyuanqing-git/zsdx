using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class UploadSampleProxy: SampleProxy
    {
        public UploadSampleProxy(Guid userId)
            : base(userId, new UploadSampleFactory(userId)) { }

        public override void DoPriviligeFacade(Model.View.ViewSampleApply viewSampleApply)
        {
            string errorMessage = "";
            if (!_viewSampleApplyBLL.JudgeIsEnableAnalysis(_user.Id, viewSampleApply.SampleStatus, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.AnalysisOperate = "";
            }
            if (!_viewSampleApplyBLL.JudgeIsEnableRegister(_user.Id, viewSampleApply.SampleStatus, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.UploadOperate = "";
            }
        }
        public override string BuildOperate(Model.View.ViewSampleApply viewSampleApply)
        {
            StringBuilder sbOperate = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ViewSampleApplyInfoOperate))
                sbOperate.Append(viewSampleApply.ViewSampleApplyInfoOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.SendMessageOperate))
                sbOperate.Append(viewSampleApply.SendMessageOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.UploadOperate))
                sbOperate.Append(viewSampleApply.UploadOperate);
                sbOperate.Append(viewSampleApply.ChargeOperate);
            return sbOperate.ToString();
        }
    }
}
