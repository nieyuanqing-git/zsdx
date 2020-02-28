using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public abstract class PriviligeBase
    {
        protected Guid _userId;
        protected IEnumerable<Guid> _workGroupIdList;
        protected IViewUserWorkGroupModuleFunctionBLL _viewUserWorkGroupModuleFunctionBLL;
        protected IUserWorkGroupBLL _userWorkGroupBLL;
        protected IUserBLL _userBLL;
        protected IViewUserSystemSettingBLL _viewUserSystemSettingBLL;
        private PriviligeBase()
        {
            _userWorkGroupBLL = BLLFactory.CreateUserWorkGroupBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            _viewUserWorkGroupModuleFunctionBLL = BLLFactory.CreateViewUserWorkGroupModuleFunctionBLL();
            _viewUserSystemSettingBLL = BLLFactory.CreateViewUserSystemSettingBLL();
        }
        public PriviligeBase(Guid userId)
            : this()
        {
            _userId = userId;
            var userSystemSettingList = _viewUserSystemSettingBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.ToString()));
            ViewUserSystemSetting userSystemSetting = null;
            if (userSystemSettingList != null && userSystemSettingList.Count > 0)
            {
                userSystemSetting = userSystemSettingList.First();
                if (userSystemSetting.WorkGroupId.HasValue)
                {
                    _workGroupIdList = new Guid[] { userSystemSetting.WorkGroupId.Value };
                    return;
                }
            }
            var userWorkGroupList = _userWorkGroupBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.ToString()));
            if (userWorkGroupList != null && userWorkGroupList.Count > 0) _workGroupIdList = userWorkGroupList.Select(p => p.WorkGroupId);
        }
        protected abstract IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList();
        protected abstract void BuildPrivilige();
        public PriviligeBase GetPrivilige()
        {
            BuildPrivilige();
            return this;
        }
        /// <summary>
        /// 超级管理的逻辑在这里实现
        /// </summary>
        public bool IsSuperAmin
        {
            get 
            {
                return false;
            }
        }

        /// <summary>
        /// 强权和机构限制权限判断( 机构限制 > 强权限制) 
        /// </summary>
        /// <param name="moduleFunctionList"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool GetIsEnableOpearte(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool IsUserOrgAuthorizedPower = false, bool IsEquipmentOrgAuthorizedPower = false, bool IsNMPOrgAuthorizedPower=false)
        {
            bool isAllowRuleLimit = viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsAllowRuleLimit);
            bool isAllowUserOrgLimit = viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsAllowUserOrgLimit);
            bool isAllowEquipmentOrgLimit = viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsAllowEquipmentOrgLimit);
            bool isAllowNMPOrgLimit = viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsAllowNMPOrgLimit);
            bool isNoRulePass = isAllowRuleLimit 
                    && viewModuleFunctionList.Any(p => p.ActionName == actionName 
                            && p.IsNoRuleLimit.HasValue && p.IsNoRuleLimit.Value
                            && (!p.IsUserOrgLimit.HasValue || !p.IsUserOrgLimit.Value)
                            && (!p.IsEquipmentOrgLimit.HasValue || !p.IsEquipmentOrgLimit.Value)
                            && (!p.IsNMPOrgLimit.HasValue || !p.IsNMPOrgLimit.Value)
                        );
            bool isUserOrgPass = isAllowUserOrgLimit && IsUserOrgAuthorizedPower
                && viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsUserOrgLimit.HasValue && p.IsUserOrgLimit.Value);
            bool isEquipmentOrgPass = isAllowEquipmentOrgLimit && IsEquipmentOrgAuthorizedPower
                && viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsEquipmentOrgLimit.HasValue && p.IsEquipmentOrgLimit.Value);

            bool isNMPOrgPass = isAllowNMPOrgLimit && IsNMPOrgAuthorizedPower
               && viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsNMPOrgLimit.HasValue && p.IsNMPOrgLimit.Value);

            bool isNormalPass = !isAllowRuleLimit && !isAllowUserOrgLimit && !isAllowEquipmentOrgLimit && !isAllowNMPOrgLimit && viewModuleFunctionList.Any(p => p.ActionName == actionName);
            return isNoRulePass || isUserOrgPass || isEquipmentOrgPass || isNormalPass || isNMPOrgPass;
        }
    }
}
