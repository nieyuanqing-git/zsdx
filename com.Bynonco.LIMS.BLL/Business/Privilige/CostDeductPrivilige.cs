using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class CostDeductPrivilige : PriviligeBase
    {

        private IUserWorkScopeBLL __userWorkScopeBLL;
        private INMPAdminBLL __nmpAdminBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewCostDeductBLL __viewCostDeductBLL;
        private int __userWorkScopeCount = 0;
        private int __nmpAdminCount = 0;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        private bool __isNMPOrgAuthorizedPower { get; set; }
        public bool IsAuthorizedPower { get; private set; }
        public Guid? CostDeductId { get; private set; }

        public CostDeductPrivilige(Guid userId)
            : base(userId)
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewCostDeductBLL = BLLFactory.CreateViewCostDeductBLL();
            __nmpAdminBLL = BLLFactory.CreateNMPAdminBLL();
        }
        public CostDeductPrivilige(Guid userId, Guid costDeductId)
            : this(userId)
        {
            CostDeductId = costDeductId;
            __isUserOrgAuthorizedPower = false;
            __isEquipmentOrgAuthorizedPower = false;
            var viewCostDeduct = __viewCostDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"", costDeductId), null, "", true);
            if (viewCostDeduct != null)
            {
                if (viewCostDeduct.EquipmentId.HasValue)
                {
                    __userWorkScopeCount = __userWorkScopeBLL.CountModelListByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", viewCostDeduct.EquipmentId.Value, userId.ToString()));
                }
                if (viewCostDeduct.NMPId.HasValue)
                {
                    __nmpAdminCount = __nmpAdminBLL.CountModelListByExpression(string.Format("NMPId=\"{0}\"*AdminId=\"{1}\"", viewCostDeduct.NMPId.Value, userId.ToString()));
                }
                if (viewCostDeduct.PayerOrgId.HasValue)
                    __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewCostDeduct.PayerOrgId.Value, LabOrganizationAuthorizedType.User);
                if (viewCostDeduct.EquipmentOrgId.HasValue)
                    __isEquipmentOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewCostDeduct.EquipmentOrgId.Value, LabOrganizationAuthorizedType.Equipment);
                if (viewCostDeduct.NMPOrgId.HasValue)
                    __isNMPOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewCostDeduct.NMPOrgId.Value, LabOrganizationAuthorizedType.NMP);
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("((ControllerName=UsedConfirm*(ActionName=UnDeductUsedConfirmListContainer+ActionName=SaveUsedConfirm+ActionName=DirectCostDeduct+ActionName=ViewUsedConfirmDetail+ActionName=Index+ActionName=EditFeedBack+ActionName=ExpertEquipmentRunConditionDetail))+(ControllerName=NMPUsedConfirm*(ActionName=UnDeductNMPUsedConfirmListContainer+ActionName=SaveNMPUsedConfirm+ActionName=DirectNMPCostDeduct+ActionName=ViewNMPUsedConfirmDetail+ActionName=Index+ActionName=EditNMPFeedBack)))");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "CostDeduct", additionQueryExpression);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            var list = viewModuleFunctionList.Where(p => p.ControllerName == "NMPUsedConfirm").ToList();
            IsAuthorizedPower = !CostDeductId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableViewCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewUsedConfirmDetail", true);
            IsEnableViewAppointmentPreCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewAppointmentPreCostDeduct", true);
            IsEnbaleViewManualCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewManualCostDeduct", true);
            IsEnableViewSampleApplyCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewSampleApplyCostDeduct", true);
            IsEnableSaveCosetDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveUsedConfirm");
            IsEnableAddFeedBackOperate = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditFeedBack", true);
            IsEnableReCosetDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "DirectCostDeduct");
            IsEnabelDeleteCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelCostDeduct");
            IsEnableAddManualCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddManualCostDeduct");
            IsEnableEditManualCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditManualCostDeduct");
            IsEnableSaveManualCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveManualCostDeduct");
            IsEnableDeleteManualCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteManualCostDeduct");
            IsEnableExportManualCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportManualCostDeductList");
            IsEnableExportSampleApplyCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportSampleApplyCostDeductList");
            IsEnableExportAppointmentPredictCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportAppointmentPredictCostDeductList");
            IsEnableExportCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportCostDeductList");
            IsEnablePayerUseConditionSumTotalManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "PayerUseConditionSumTotalManage");
            IsEnableGetPayerUseConditionSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetPayerUseConditionSumTotal");
            IsEnableExpertPayerUseConditionSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertPayerUseConditionSumTotal");
            IsEnableExportAnimalCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportAnimalCostDeductList");
            IsEnablePrint = GetIsEnableOpearteExtend(viewModuleFunctionList, "Print");
            IsEnableUsedConfirmUseConditionExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertEquipmentRunConditionDetail");
            IsEnableAddWaterControlCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddWaterControlCostDeduct");
            IsEnableExportWaterControlCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportWaterControlCostDeductList");
            IsEnbaleViewWaterControlCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewWaterControlCostDeduct", true);
            IsEnableSaveWaterControlCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveWaterControlCostDeduct");
            IsEnableEditWaterControlCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditWaterControlCostDeduct");
            IsEnableDeleteWaterControlCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteWaterControlCostDeduct");
            IsEnableAddManualTimeCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddManualTimeCostDeduct");
            IsEnableEditManualTimeCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditManualTimeCostDeduct");
            IsEnableSaveManualTimeCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveManualTimeCostDeduct");
            IsEnableDeleteManualTimeCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteManualTimeCostDeduct");
            IsEnableExportExcelManualTimeCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
            IsEnableViewNMPCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewNMPUsedConfirmDetail", true);
            IsEnableSaveNMPCosetDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveNMPUsedConfirm");
            IsEnableAddNMPFeedBackOperate = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditNMPFeedBack");
            IsEnableReNMPCosetDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "DirectNMPCostDeduct");
            IsEnabelDeleteNMPCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelNMPCostDeduct");
            IsEnableExportNMPCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportNMPCostDeductList");

            IsEnableUnDeductUsedConfirmListContainer = viewModuleFunctionList.Any(p => p.ActionName == "UnDeductUsedConfirmListContainer"); 
            IsEnableCostDeductContainer = viewModuleFunctionList.Any(p => p.ActionName == "CostDeductContainer"); GetIsEnableOpearteExtend(viewModuleFunctionList, "");
            IsEnableManualCostDeductListContainer = viewModuleFunctionList.Any(p => p.ActionName == "ManualCostDeductListContainer"); 
            IsEnableAppointmentPredictCostDeductContainer = viewModuleFunctionList.Any(p => p.ActionName == "AppointmentPredictCostDeductContainer");
            IsEnableSampleApplyCostDeductContainer = viewModuleFunctionList.Any(p => p.ActionName == "SampleApplyCostDeductContainer"); 
            IsEnableMaterialCostDeductListContainer = viewModuleFunctionList.Any(p => p.ActionName == "MaterialCostDeductListContainer"); 
            IsEnableAnimalCostDeductListContainer = viewModuleFunctionList.Any(p => p.ActionName == "AnimalCostDeductListContainer"); 
            IsEnableWaterControlCostDeductListContainer = viewModuleFunctionList.Any(p => p.ActionName == "WaterControlCostDeductListContainer"); 
            IsEnableUnDeductNMPUsedConfirmListContainer = viewModuleFunctionList.Any(p => p.ActionName == "UnDeductNMPUsedConfirmListContainer");
            IsEnableNMPCostDeductListContainer = viewModuleFunctionList.Any(p => p.ActionName == "NMPCostDeductListContainer");
            IsEnableCostDeductSynListContainer = viewModuleFunctionList.Any(p => p.ActionName == "CostDeductSynListContainer");
            IsEnableManualTimeCostDeductListContainer = viewModuleFunctionList.Any(p => p.ActionName == "ManualTimeCostDeductListContainer"); 
            
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || 
                (CostDeductId.HasValue &&  (__userWorkScopeCount > 0  || __nmpAdminCount > 0) && viewModuleFunctionList.Any(p => p.ActionName == actionName)) 
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, __isEquipmentOrgAuthorizedPower,__isNMPOrgAuthorizedPower));
        }
        public bool IsEnableViewCostDeduct { get; private set; }
        public bool IsEnableViewAppointmentPreCostDeduct { get; private set; }
        public bool IsEnbaleViewManualCostDeduct { get; private set; }
        public bool IsEnableViewSampleApplyCostDeduct { get; private set; }
        public bool IsEnableSaveCosetDeduct { get; private set; }
        public bool IsEnableReCosetDeduct { get; private set; }
        public bool IsEnabelDeleteCostDeduct { get; private set; }
        public bool IsEnableExportManualCostDeductList { get; private set; }
        public bool IsEnableExportSampleApplyCostDeductList { get; private set; }
        public bool IsEnableExportAppointmentPredictCostDeductList { get; private set; }
        public bool IsEnableAddManualCostDeduct { get; private set; }
        public bool IsEnableEditManualCostDeduct { get; private set; }
        public bool IsEnableSaveManualCostDeduct { get; private set; }
        public bool IsEnableDeleteManualCostDeduct { get; private set; }
        public bool IsEnableAddFeedBackOperate { get; private set; }
        public bool IsEnableExportCostDeductList { get; private set; }
        public bool IsEnablePayerUseConditionSumTotalManage { get; private set; }
        public bool IsEnableGetPayerUseConditionSumTotal { get; private set; }
        public bool IsEnableExpertPayerUseConditionSumTotal { get; private set; }
        public bool IsEnableExportAnimalCostDeductList { get; private set; }
        public bool IsEnablePrint { get; private set; }
        public bool IsEnableUsedConfirmUseConditionExportExcel { get; private set; }
        public bool IsEnbaleViewWaterControlCostDeduct { get; private set; }
        public bool IsEnableAddWaterControlCostDeduct { get; private set; }
        public bool IsEnableExportWaterControlCostDeductList { get; private set; }
        public bool IsEnableEditWaterControlCostDeduct { get; private set; }
        public bool IsEnableSaveWaterControlCostDeduct { get; private set; }
        public bool IsEnableDeleteWaterControlCostDeduct { get; private set; }
        public bool IsEnableAddManualTimeCostDeduct { get; private set; }
        public bool IsEnableEditManualTimeCostDeduct { get; private set; }
        public bool IsEnableSaveManualTimeCostDeduct { get; private set; }
        public bool IsEnableDeleteManualTimeCostDeduct { get; private set; }
        public bool IsEnableExportExcelManualTimeCostDeduct { get; private set; }
        public bool IsEnableViewNMPCostDeduct { get; private set; }
        public bool IsEnableSaveNMPCosetDeduct { get; private set; }
        public bool IsEnableAddNMPFeedBackOperate { get; private set; }
        public bool IsEnableReNMPCosetDeduct { get; private set; }
        public bool IsEnabelDeleteNMPCostDeduct { get; private set; }
        public bool IsEnableExportNMPCostDeductList { get; private set; }

        public bool IsEnableUnDeductUsedConfirmListContainer { get; private set; }
        public bool IsEnableCostDeductContainer { get; private set; }
        public bool IsEnableManualCostDeductListContainer { get; private set; }
        public bool IsEnableAppointmentPredictCostDeductContainer { get; private set; }
        public bool IsEnableSampleApplyCostDeductContainer { get; private set; }
        public bool IsEnableMaterialCostDeductListContainer { get; private set; }
        public bool IsEnableAnimalCostDeductListContainer { get; private set; }
        public bool IsEnableWaterControlCostDeductListContainer { get; private set; }
        public bool IsEnableUnDeductNMPUsedConfirmListContainer { get; private set; }
        public bool IsEnableNMPCostDeductListContainer { get; private set; }
        public bool IsEnableManualTimeCostDeductListContainer { get; private set; }
        public bool IsEnableCostDeductSynListContainer { get; private set; }
    }
}
