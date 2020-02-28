using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class EthicsSettingPrivilige : PriviligeBase
    {
        public EthicsSettingPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "EthicsSetting");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableGetEthicsNeedPassTrainningTypeList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetNeedPassTrainningTypeList");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");

            IsEnableDeleteNeedPassTrainningType = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteNeedPassTrainningType");
            IsEnableAddNeedPassTrainningType = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddNeedPassTrainningType");
            IsEnableEditNeedPassTrainningType = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditNeedPassTrainningType");
            IsEnableSaveNeedPassTrainningType = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveNeedPassTrainningType");

            IsEnableGetShowLinksWhenApplyPassList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetShowLinksWhenApplyPassList");
            IsEnableDeleteShowLinksWhenApplyPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteShowLinksWhenApplyPass");
            IsEnableAddShowLinksWhenApplyPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddShowLinksWhenApplyPass");
            IsEnableEditShowLinksWhenApplyPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditShowLinksWhenApplyPass");
            IsEnableSaveShowLinksWhenApplyPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveShowLinksWhenApplyPass");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableGetEthicsNeedPassTrainningTypeList { get; private set; }
        public bool IsEnableDeleteNeedPassTrainningType { get; private set; }
        public bool IsEnableAddNeedPassTrainningType { get; private set; }
        public bool IsEnableEditNeedPassTrainningType { get; private set; }
        public bool IsEnableSaveNeedPassTrainningType { get; private set; }

        public bool IsEnableGetShowLinksWhenApplyPassList { get; private set; }
        public bool IsEnableDeleteShowLinksWhenApplyPass { get; private set; }
        public bool IsEnableAddShowLinksWhenApplyPass { get; private set; }
        public bool IsEnableEditShowLinksWhenApplyPass { get; private set; }
        public bool IsEnableSaveShowLinksWhenApplyPass { get; private set; }
    }
}
