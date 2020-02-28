using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NJYKDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "NJYKDX";
        public override string GetUnAuditSampleListSortName()
        {
            return "ExpectSendDate";
        }
        public override int GetUnAuditSampleListPageSize()
        {
            return 50;
        }
        public override bool GetIsNJYKDRegistNoteInfo()
        {
            return true;
        }
        public override bool GetIsShowEquipmentSampleItemSearch()
        {
            return false;
        }
        public override bool GetIsSampleItemTestConditionRequired()
        {
            return false;
        }
        public override string GetSampleApplyStatusName(Model.Enum.SampleApplyStatus sampleApplyStatus)
        {
            if (sampleApplyStatus == SampleApplyStatus.Audited)
            {
                return "已完成";
            }
            if (sampleApplyStatus == SampleApplyStatus.UnAudit)
            {
                return "待检测";
            }
            return sampleApplyStatus.ToCnName();
        }

        public override string GetSampleChargeStatusName(Model.Enum.SampleApplyStatus sampleApplyStatus, Model.Enum.SampleChargeStatus sampleChargeStatus)
        {
            return sampleChargeStatus.ToCnName();
        }
    }
}
