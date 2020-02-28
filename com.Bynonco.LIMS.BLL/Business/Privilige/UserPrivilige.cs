using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class UserPrivilige : PriviligeBase
    {
        private IUserBLL __userBLL;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        private Guid? TargetUserId { get; set; }
        public bool IsAuthorizedPower { get; private set; }
        public UserPrivilige(Guid userId)
            : base(userId) 
        {
            __userBLL = BLLFactory.CreateUserBLL();
        }
        public UserPrivilige(Guid userId, Guid targetUserId)
            : this(userId) 
        {
            __isEquipmentOrgAuthorizedPower = false;
            TargetUserId = targetUserId;
            var targetUser = __userBLL.GetEntityById(targetUserId);
            if (targetUser != null && targetUser.OrganizationId.HasValue)
                __isUserOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, targetUser.OrganizationId.Value, LabOrganizationAuthorizedType.User);
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "User");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !TargetUserId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserList");
            IsEnableTutorList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserTutorList");
            IsEnableStudentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserStudentList");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit", true);
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete", true);
            IsEnableSaveBasic = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBasic", true);
            IsEnableAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAuditPass", true);
            IsEnableSetInvalid = GetIsEnableOpearteExtend(viewModuleFunctionList, "SetInvalid", true);
            IsEnableSetBlacklist = GetIsEnableOpearteExtend(viewModuleFunctionList, "SetBlacklist", true);
            IsEnableSetWorkGroup = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserWorkGroupList", true);
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
            IsEnableImportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ImportExcel");

            IsEnableAuthorizationUserEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit", true);
            IsEnableAuthorizationUserDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete", true);
            IsEnableAuthorizationExportSlwUser = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportSlwUser", true);
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> moduleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (isAuthorizedPower.Value && GetIsEnableOpearte(moduleFunctionList, actionName, __isUserOrgAuthorizedPower, __isEquipmentOrgAuthorizedPower));
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableTutorList { get; private set; }
        public bool IsEnableStudentList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSaveBasic { get; private set; }
        public bool IsEnableAudit { get; private set; }
        public bool IsEnableSetInvalid { get; private set; }
        public bool IsEnableSetBlacklist { get; private set; }
        public bool IsEnableSetWorkGroup { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
        public bool IsEnableImportExcel { get; private set; }

        public bool IsEnableAuthorizationUserEdit { get; private set; }
        public bool IsEnableAuthorizationUserDelete { get; private set; }
        public bool IsEnableAuthorizationExportSlwUser { get; private set; }

    }
}
