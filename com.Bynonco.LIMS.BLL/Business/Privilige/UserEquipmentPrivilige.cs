using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class UserEquipmentPrivilige:PriviligeBase
    {
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewUserEquipmentBLL __viewUserEquipmentBLL;
        private UserWorkScope __userWorkScope = null;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        public Guid? UserEquipmentId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public UserEquipmentPrivilige(Guid userId)
            : base(userId)
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewUserEquipmentBLL = BLLFactory.CreateViewUserEquipmentBLL();
        }
        public UserEquipmentPrivilige(Guid userId, Guid userEquipmentId)
            : this(userId)
        {
            UserEquipmentId = userEquipmentId;
            __isUserOrgAuthorizedPower = false;
            __isEquipmentOrgAuthorizedPower = false;
            var viewUserEquipmentList = __viewUserEquipmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", userEquipmentId), null, "", true);
            if (viewUserEquipmentList != null && viewUserEquipmentList.Count() > 0)
            {
                var viewUserEquipment = viewUserEquipmentList.First();
                var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", viewUserEquipment.EquipmentId, userId.ToString()));
                if (userWorkScopeList != null && userWorkScopeList.Count > 0) __userWorkScope = userWorkScopeList.First();
                if (viewUserEquipment.UserOrgId.HasValue)
                    __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewUserEquipment.UserOrgId.Value, LabOrganizationAuthorizedType.User);
                if (viewUserEquipment.EquipmentOrgId.HasValue)
                    __isEquipmentOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewUserEquipment.EquipmentOrgId.Value, LabOrganizationAuthorizedType.Equipment);
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "UserEquipment");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !UserEquipmentId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableFocusEquipment = GetIsEnableOpearteExtend(viewModuleFunctionList, "FocusEquipment",true);
            IsEnableCancelFocus = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelFocus", true);
            IsEnabelGetGridSelfViewUserEquipmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridSelfViewUserEquipmentList");
            IsEnabelGetGridViewUserEquipmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserEquipmentList");
            IsEnablePass = GetIsEnableOpearteExtend(viewModuleFunctionList, "Pass");
            IsEnableReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "Reject");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableApply = GetIsEnableOpearteExtend(viewModuleFunctionList, "Apply", true);
            IsEnableCancelApply = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelApply", true);
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete", true);
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage") && viewModuleFunctionList.Any(p => p.ActionName == "Manage" && p.Name == "申请审核");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (__userWorkScope != null && viewModuleFunctionList.Any(p => p.ActionName == actionName))
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, __isEquipmentOrgAuthorizedPower));
        }
        public bool IsEnabelGetGridSelfViewUserEquipmentList { get; private set; }
        public bool IsEnabelGetGridViewUserEquipmentList { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableApply { get; private set; }
        public bool IsEnableCancelApply { get; private set; }
        public bool IsEnableFocusEquipment { get; private set; }
        public bool IsEnableCancelFocus { get; private set; }
        public bool IsEnablePass { get; private set; }
        public bool IsEnableReject { get; private set; }
        public bool IsEnableManage { get; private set; }

    }
}
