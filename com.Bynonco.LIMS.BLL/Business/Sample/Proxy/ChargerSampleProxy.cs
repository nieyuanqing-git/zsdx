using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class ChargerSampleProxy : SampleProxy
    {
        public ChargerSampleProxy(Guid userId)
            : base(userId, new ChargerSampleFactory(userId)) { }

        public override void DoPriviligeFacade(Model.View.ViewSampleApply viewSampleApply)
        {
            //if (viewSampleApply.Status != (int)Model.Enum.SampleApplyStatus.UnAudit)
            //{
            //    viewSampleApply.EditOperate = "";
            //    viewSampleApply.CancelOperate = "";
            //}
        }
        public override string BuildOperate(Model.View.ViewSampleApply viewSampleApply)
        {
            StringBuilder sbOperate = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ViewSampleApplyInfoOperate)) sbOperate.Append(viewSampleApply.ViewSampleApplyInfoOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.DepositOperate)) sbOperate.Append(viewSampleApply.DepositOperate);
            return sbOperate.ToString();
        }
    }
}
