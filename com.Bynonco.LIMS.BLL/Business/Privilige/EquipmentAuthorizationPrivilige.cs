using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class EquipmentAuthorizationPrivilige : PriviligeBase
    {
        public EquipmentAuthorizationPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "EquipmentAuthorization");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridEquipmentAuthorizationList");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");

            IsEnableAuthorizationList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserEquipmentAuthorizationList");
            IsEnableAuthorizationManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentAuthorizationManage");
            IsEnableAuthorizationEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentAuthorizationEdit");
            IsEnableAuthorizationDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentAuthorizationDelete");
            IsEnableAuthorizationSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentAuthorizationSave");

            IsEnableContinuedAuthorizationList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserEquipmentContinuedAuthorizationList");
            IsEnableContinuedAuthorizationManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentContinuedAuthorizationManage");
            IsEnableContinuedAuthorizationEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentContinuedAuthorizationEdit");
            IsEnableContinuedAuthorizationDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentContinuedAuthorizationDelete");
            IsEnableContinuedAuthorizationSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentContinuedAuthorizationSave");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }

        public bool IsEnableAuthorizationList { get; private set; }
        public bool IsEnableAuthorizationManage { get; private set; }
        public bool IsEnableAuthorizationEdit { get; private set; }
        public bool IsEnableAuthorizationDelete { get; private set; }
        public bool IsEnableAuthorizationSave { get; private set; }

        public bool IsEnableContinuedAuthorizationList { get; private set; }
        public bool IsEnableContinuedAuthorizationManage { get; private set; }
        public bool IsEnableContinuedAuthorizationEdit { get; private set; }
        public bool IsEnableContinuedAuthorizationDelete { get; private set; }
        public bool IsEnableContinuedAuthorizationSave { get; private set; }
    }
}
