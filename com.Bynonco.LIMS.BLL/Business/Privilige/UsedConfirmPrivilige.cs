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
    public class UsedConfirmPrivilige:PriviligeBase
    {
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewUsedConfirmBLL __viewUsedConfirmBLL;
        private IViewCurrentUsingEquipmentBLL __viewCurrentUsingEquipmentBLL;
        private UserWorkScope __userWorkScope = null;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        public Guid? UsedConfirmId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public UsedConfirmPrivilige(Guid userId)
            : base(userId) 
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewUsedConfirmBLL = BLLFactory.CreateViewUsedConfirmBLL();
            __viewCurrentUsingEquipmentBLL = BLLFactory.CreateViewCurrentUsingEquipmentBLL();
        }
        public UsedConfirmPrivilige(Guid userId, Guid usedConfirmId)
            : this(userId)
        {
            UsedConfirmId = usedConfirmId;
            __isUserOrgAuthorizedPower = false;
            __isEquipmentOrgAuthorizedPower = false;
            Guid? userOrgId = null;
            Guid? equipmentId = null;
            Guid? equipmentOrgId = null;

            var viewUsedConfirmList = __viewUsedConfirmBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", usedConfirmId), null, "", true);
            if (viewUsedConfirmList != null && viewUsedConfirmList.Count() > 0)
            {
                var viewUsedConfirm = viewUsedConfirmList.First();
                userOrgId = viewUsedConfirm.UserOrgId;
                equipmentId = viewUsedConfirm.EquipmentId;
                equipmentOrgId = viewUsedConfirm.EquipmentOrgId;
            }
            else
            {
                var viewCurrentUsingEquipmentList = __viewCurrentUsingEquipmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", usedConfirmId), null, "", true);
                if (viewCurrentUsingEquipmentList != null && viewCurrentUsingEquipmentList.Count() > 0)
                {
                    var viewCurrentUsingEquipment = viewCurrentUsingEquipmentList.First();
                    userOrgId = viewCurrentUsingEquipment.UserOrgId;
                    equipmentId = viewCurrentUsingEquipment.EquipmentId;
                    equipmentOrgId = viewCurrentUsingEquipment.EquipmentOrgId;
                }
            }
            if (equipmentId.HasValue)
            {
                var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", equipmentId.Value, userId.ToString()));
                if (userWorkScopeList != null && userWorkScopeList.Count > 0) __userWorkScope = userWorkScopeList.First();
            }
            if (userOrgId.HasValue)
                __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, userOrgId.Value, LabOrganizationAuthorizedType.User);
            if (equipmentOrgId.HasValue)
                __isEquipmentOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, equipmentOrgId.Value, LabOrganizationAuthorizedType.Equipment);
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "UsedConfirm");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !UsedConfirmId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableMyManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyUsedConfirmContainer");
            IsEnableGetGridViewCurrentUsingEquipmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewCurrentUsingEquipmentList");
            IsEnableGetGridTodayUsedConfirmList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridTodayUsedConfirmList");
            IsEnableViewUsedConfirmDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewUsedConfirmDetail", true);
            IsEnableAddUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddUsedConfirm");
            IsEnableEditUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditUsedConfirm");
            IsEnableDeleteUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteUsedConfirm");
            IsEnableEditFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditFeedBack");
            IsEnableSaveFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveFeedBack");
            IsEnableDeleteFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteFeedBack");
            IsEnableSaveUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveUsedConfirm");
            IsEnableCalcFeeUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "CalcFeeUsedConfirm");
            IsEnableDirectCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "DirectCostDeduct");
            IsEnableExportUnDeductUsedConfirmList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportUnDeductUsedConfirmList");
            IsEnableExportCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportCostDeductList");
            IsEnableChangeUsedConfirmUser = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveChangeUser");
            IsEnableUsedConfirmExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "UsedConfirmExportExcel");
            IsEnableEquipmentRunConditionSumTotalManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentRunConditionSumTotalManage");
            IsEnableExpertEquipmentRunConditionSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertEquipmentRunConditionSumTotal");
            IsEnableGetEquipmentRunConditionSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetEquipmentRunConditionSumTotal");
            IsEnableExpertEquipmentRunConditionDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertEquipmentRunConditionDetail");
            IsEnableUsedConfirmUseConditionExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "UsedConfirmUseConditionExportExcel");
            IsEnableSelfSaveAppointmentUsedConfirm = viewModuleFunctionList.Any(p => p.ActionName == "SelfSaveAppointmentUsedConfirm");
            IsEnableCurrentUsingEquipment = viewModuleFunctionList.Any(p => p.ActionName == "CurrentUsingEquipmentListIndex");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (__userWorkScope != null && viewModuleFunctionList.Any(p => p.ActionName == actionName))
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, __isEquipmentOrgAuthorizedPower));
        }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableMyManage { get; private set; }
        public bool IsEnableGetGridViewCurrentUsingEquipmentList { get; private set; }
        public bool IsEnableGetGridTodayUsedConfirmList { get; private set; }
        public bool IsEnableViewUsedConfirmDetail { get; private set; }
        public bool IsEnableAddUsedConfirm { get; private set; }
        public bool IsEnableEditUsedConfirm { get; private set; }
        public bool IsEnableDeleteUsedConfirm { get; private set; }
        public bool IsEnableSaveUsedConfirm { get; private set; }
        public bool IsEnableCalcFeeUsedConfirm { get; private set; }
        public bool IsEnableDirectCostDeduct { get; private set; }
        public bool IsEnableExportUnDeductUsedConfirmList { get; private set; }
        public bool IsEnableExportCostDeductList { get; private set; }
        public bool IsEnableEditFeedBack { get; private set; }
        public bool IsEnableSaveFeedBack { get; private set; }
        public bool IsEnableDeleteFeedBack { get; private set; }
        public bool IsEnableChangeUsedConfirmUser { get; private set; }
        public bool IsEnableUsedConfirmExportExcel { get; private set; }
        public bool IsEnableEquipmentRunConditionSumTotalManage { get; private set; }
        public bool IsEnableExpertEquipmentRunConditionSumTotal { get; private set; }
        public bool IsEnableGetEquipmentRunConditionSumTotal { get; private set; }
        public bool IsEnableExpertEquipmentRunConditionDetail { get; private set; }
        public bool IsEnableUsedConfirmUseConditionExportExcel { get; private set; }
        public bool IsEnableSelfSaveAppointmentUsedConfirm { get; set; }
        public bool IsEnableCurrentUsingEquipment { get; private set; }
    }
}
