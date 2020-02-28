using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class UserSampleItemDiscountPrivilige:PriviligeBase
    {
        private IUserWorkScopeBLL __userWorkScopeBLL;
        public UserSampleItemDiscountPrivilige(Guid userId)
            : base(userId) 
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "SampleItem");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableUserDiscountEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDiscountEdit");
            IsEnableGetViewUserSampleItemDiscountList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetViewUserSampleItemDiscountList");
            IsEnableDeleteUserSampleItemDiscount = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteUserSampleItemDiscount");
            IsEnableSaveUserSampleItemDiscount = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveUserSampleItemDiscount");
            IsEnableBatchStop = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchStopUserSampleItemDiscount");
            IsEnableBatchStart = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchStartUserSampleItemDiscount");
            IsEnableUserDiscountAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserDiscountAdd");
            if (!IsEnableUserDiscountAdd)
            {
                var uerWorkScopeCount = __userWorkScopeBLL.GetSingleResult(string.Format("SELECT count(*) FROM UserWorkScope WHERE UserId='{0}'", _userId.ToString()));
                IsEnableUserDiscountAdd = IsEnableUserDiscountAdd | (int)uerWorkScopeCount > 0;
               
            }
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableUserDiscountEdit { get; private set; }
        public bool IsEnableGetViewUserSampleItemDiscountList { get; private set; }
        public bool IsEnableDeleteUserSampleItemDiscount { get; private set; }
        public bool IsEnableSaveUserSampleItemDiscount { get; private set; }
        public bool IsEnableUserDiscountAdd { get; private set; }
        public bool IsEnableBatchStop { get; private set; }
        public bool IsEnableBatchStart { get; private set; }
    }
}
