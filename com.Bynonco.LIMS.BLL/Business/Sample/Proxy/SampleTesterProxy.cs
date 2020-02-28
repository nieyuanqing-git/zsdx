using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class TesterSampleProxy : SampleProxy
    {
        public TesterSampleProxy(Guid userId)
            : base(userId, new TesterSampleFactory(userId)) { }
        private IList<DictCodeType> __dictCodeTypes = null;
        public override void DoPriviligeFacade(Model.View.ViewSampleApply viewSampleApply) 
        {
            string errorMessage = "";
            var sampleTesters = _sampleTesterBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"",viewSampleApply.Id.ToString()));
            var sampleTestRecords = _sampleTestRecordBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"",viewSampleApply.Id.ToString()));
            if(!_viewSampleApplyBLL.JudgeIsEnableRelativeTestOperate(_user.Id,viewSampleApply,sampleTesters,sampleTestRecords, SampleTestOption.BeginHandle,_samplePrivilige,out errorMessage))
            {
                viewSampleApply.BeginHandleOperate = "";
            }
            if (!_viewSampleApplyBLL.JudgeIsEnableRelativeTestOperate(_user.Id, viewSampleApply, sampleTesters, sampleTestRecords, SampleTestOption.EndHandle, _samplePrivilige,out errorMessage))
            {
                viewSampleApply.EndHandleOperate = "";
            }
            if (!_viewSampleApplyBLL.JudgeIsEnableRelativeTestOperate(_user.Id, viewSampleApply, sampleTesters, sampleTestRecords, SampleTestOption.BeginTest,_samplePrivilige, out errorMessage))
            {
                viewSampleApply.BeginTestOperate = "";
            }
            if (!_viewSampleApplyBLL.JudgeIsEnableRelativeTestOperate(_user.Id, viewSampleApply, sampleTesters, sampleTestRecords, SampleTestOption.EndTest,_samplePrivilige, out errorMessage))
            {
                viewSampleApply.EndTestOperate = "";
            }
            if (!_viewSampleApplyBLL.JudgeIsEnableInputFinishedQuatity(_user.Id, viewSampleApply.SampleStatus, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.ModifyAmountOperate = "";
            }
        }

        public override string BuildOperate(Model.View.ViewSampleApply viewSampleApply)
        {
            StringBuilder sbOperate = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ViewSampleApplyInfoOperate))
                sbOperate.Append(viewSampleApply.ViewSampleApplyInfoOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.SendMessageOperate))
                sbOperate.Append(viewSampleApply.SendMessageOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.BeginHandleOperate))
                sbOperate.Append(viewSampleApply.BeginHandleOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.EndHandleOperate))
                sbOperate.Append(viewSampleApply.EndHandleOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.TestRecordLogOperate))
                sbOperate.Append(viewSampleApply.TestRecordLogOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ChargeOperate))
                sbOperate.Append(viewSampleApply.ChargeOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.BeginTestOperate))
                sbOperate.Append(viewSampleApply.BeginTestOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.EndTestOperate))
                sbOperate.Append(viewSampleApply.EndTestOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ModifyAmountOperate))
                sbOperate.Append(viewSampleApply.ModifyAmountOperate);
            //if (!string.IsNullOrWhiteSpace(viewSampleApply.PringQrCodeOperate))
            //    sbOperate.Append(viewSampleApply.PringQrCodeOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.PrintOperate))
                sbOperate.Append(viewSampleApply.PrintOperate);
            return sbOperate.ToString();
        }
    }
}
