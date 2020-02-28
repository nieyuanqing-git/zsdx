using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //华东师范大学
    public class HDSFDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "HDSFDX";
        public override string GetSystemName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SystemName");
        }
        public override bool GetIsLoginShowRememberMe()
        {
            return false;
        }
        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index";
        }
        public override string GetEquipmentManageSearch()
        {
            return __CODE + "PaginationSearch";
        }
        public override string GetEquipmentShowList()
        {
            return "SearchAdvanced";
        }
        public override string GetLabOrganizationName()
        {
            return "机构名称";
        }
        public override bool GetIsShowAppointmentSampleInfo()
        {
            return false;
        }
        public override bool JudgeIsNeedInputSamplePredictResultTimeWhenAudit()
        {
            return true;
        }

        public override bool JudgeIsNeedSelectSampleTesterWhenAudit()
        {
            return true;
        }
        public override string GetSampleInnerOrOuterApplyViewName()
        {
            return "HDSFDXSampleApplyInnerOrOuter";
        }
        public override bool GetIsDepositAutoPrintDoc()
        {
            return true;
        }
        public override bool GetIsDepositPhoto()
        {
            return true;
        }
        public override bool GetIsDepositPhotoAutoPreAuditPass()
        {
            return true;
        }
        public override bool GetIsDepositPhotoAfterPreAudit()
        {
            return true;
        }
        public override bool GetIsDepositPhotoRequired()
        {
            return true;
        }
        public override bool GetIsOpenFundCostDeduct()
        {
            return true;
        }
        public override bool GetIsDepositRecordEquipmentRequired()
        {
            return true;
        }

        public override bool GetIsShowSampleSubmitOpWhenAuditing()
        {
            return false;
        }
        public override bool GetIsHidePredictPriceWhenInputSampleApply()
        {
            return true;
        }
        public override bool GetIsHideChargeInfoWhenViewSampleApplyDetail()
        {
            return true;
        }
        public override string GetAchievementSubjectComeCode()
        {
            return "SubjectProjectComeFrom";
        }
    }
}
