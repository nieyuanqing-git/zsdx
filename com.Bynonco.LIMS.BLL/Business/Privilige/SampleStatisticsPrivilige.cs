using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class SampleStatisticsPrivilige: PriviligeBase
    {
        public SampleStatisticsPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "SampleStatistics");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableApplicantSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ApplicantSumTotal");
            IsEnableExpertApplicantSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertApplicantSumTotal");
            IsEnableGetApplicantSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetApplicantSumTotal");

            IsEnableSampleItemSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "SampleItemSumTotal");
            IsEnableGetSampleItemSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetSampleItemSumTotal");
            IsEnableExpertSampleItemSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertSampleItemSumTotal");

            IsEnableEquipmentSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentSumTotal");
            IsEnabelGetEquipmentSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetEquipmentSumTotal");
            IsEnableExpertEquipmentSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertEquipmentSumTotal");

            IsEnableTesterSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "TesterSumTotal");
            IsEnableGetTesterSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetTesterSumTotal");
            IsEnableExpertTesterSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertTesterSumTotal");

            IsEnablePayerSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "PayerSumTotal");
            IsEnableGetPayerSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetPayerSumTotal");
            IsEnableExpertPayerumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertPayerumTotal");

            IsEnableEquipmentPayerSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentPayerSumTotal");
            IsEnabelGetEquipmentPayerSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetEquipmentPayerSumTotal");
            IsEnableExpertEquipmentPayerSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertEquipmentPayerSumTotal");



            IsEnableExpertStatisticsDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertStatisticsDetail");
            IsEnableGetSampleApplyListInfo = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetSampleApplyListInfo");
        }

        public bool IsEnableApplicantSumTotal { get; private set; }
        public bool IsEnableExpertApplicantSumTotal { get; private set; }
        public bool IsEnableGetApplicantSumTotal { get; private set; }

        public bool IsEnableSampleItemSumTotal { get; private set; }
        public bool IsEnableGetSampleItemSumTotal { get; private set; }
        public bool IsEnableExpertSampleItemSumTotal { get; private set; }

        public bool IsEnableEquipmentSumTotal { get; private set; }
        public bool IsEnabelGetEquipmentSumTotal { get; private set; }
        public bool IsEnableExpertEquipmentSumTotal { get; private set; }

        public bool IsEnableTesterSumTotal { get; private set; }
        public bool IsEnableGetTesterSumTotal { get; private set; }
        public bool IsEnableExpertTesterSumTotal { get; private set; }


        public bool IsEnableEquipmentPayerSumTotal { get; private set; }
        public bool IsEnabelGetEquipmentPayerSumTotal { get; private set; }
        public bool IsEnableExpertEquipmentPayerSumTotal { get; private set; }


        public bool IsEnableExportExcelDetail { get; private set; }

        public bool IsEnablePayerSumTotal { get; private set; }
        public bool IsEnableGetPayerSumTotal { get; private set; }
        public bool IsEnableExpertPayerumTotal { get; private set; }

        public bool IsEnableExpertStatisticsDetail { get; private set; }
        public bool IsEnableGetSampleApplyListInfo { get; private set; }
    }
}
