using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class DoorPrivilige : PriviligeBase
    {
        public DoorPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Door");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridDoorList");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");

            IsEnableAuthorizationList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserDoorAuthorizationList");
            IsEnableAuthorizationManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDoorAuthorizationManage");
            IsEnableAuthorizationEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDoorAuthorizationEdit");
            IsEnableAuthorizationDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDoorAuthorizationDelete");
            IsEnableAuthorizationSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDoorAuthorizationSave");

            IsEnableContinuedAuthorizationList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserDoorContinuedAuthorizationList");
            IsEnableContinuedAuthorizationManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDoorContinuedAuthorizationManage");
            IsEnableContinuedAuthorizationEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDoorContinuedAuthorizationEdit");
            IsEnableContinuedAuthorizationDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDoorContinuedAuthorizationDelete");
            IsEnableContinuedAuthorizationSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDoorContinuedAuthorizationSave");

            IsEnableDoorAccessRecordExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "DoorAccessRecordExportExcel");
            IsEnableUpdateOfflineRecord = GetIsEnableOpearteExtend(viewModuleFunctionList, "UpdateOfflineRecord");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }

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

        public bool IsEnableDoorAccessRecordExportExcel { get; private set; }
        public bool IsEnableUpdateOfflineRecord { get; private set; }
    }
}
