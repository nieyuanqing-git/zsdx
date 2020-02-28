using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class UserAuthorizationPrivilige : PriviligeBase
    {
        public UserAuthorizationPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "UserAuthorization");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserList");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableUserAuthorization = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserAuthorizationContainer");
            IsEnableUserBaseAuthorization = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserBaseAuthorization");
            IsEnableSaveUserBaseAuthorization = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveUserBaseAuthorization");
            IsEnableUpdateAdmins = GetIsEnableOpearteExtend(viewModuleFunctionList, "UpdateAdmins");
            IsEnableUpdateOfflineRecord = GetIsEnableOpearteExtend(viewModuleFunctionList, "UpdateOfflineRecord");
            
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableUserAuthorization { get; private set; }
        public bool IsEnableUserBaseAuthorization { get; private set; }
        public bool IsEnableSaveUserBaseAuthorization { get; private set; }
        public bool IsEnableUpdateAdmins { get; private set; }
        public bool IsEnableUpdateOfflineRecord { get; private set; }
    }
}
