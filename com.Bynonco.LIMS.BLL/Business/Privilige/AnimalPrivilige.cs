using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class AnimalPrivilige : PriviligeBase
    {
        public AnimalPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Animal");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableGetGridSampleStatusList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewAnimalList");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "View");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditContainer");
            IsEnableBatchStop = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchStop");
            IsEnableBatchStart = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchStart");
            IsEnableRegisterDeath = GetIsEnableOpearteExtend(viewModuleFunctionList, "RegisterDeath");
            IsEnableConfirmRegistingDeath = GetIsEnableOpearteExtend(viewModuleFunctionList, "ConfirmRegistingDeath");
            IsEnableIn = GetIsEnableOpearteExtend(viewModuleFunctionList, "In");
            IsEnableBatchIn = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchIn");
            IsEnableCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "CostDeduct");
            IsEnableBatchCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchCostDeduct");
            IsEnableAutoCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "AutoCostDeduct");
            IsEnbaleLogContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "LogContainer");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableGetGridSampleStatusList { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableView { get; private set; }
        public bool IsEnableIn { get; private set; }
        public bool IsEnableBatchIn { get; private set; }
        public bool IsEnableBatchStop { get; private set; }
        public bool IsEnableBatchStart { get; private set; }
        public bool IsEnableRegisterDeath { get; private set; }
        public bool IsEnableConfirmRegistingDeath { get; private set; }
        public bool IsEnableCostDeduct { get; private set; }
        public bool IsEnableBatchCostDeduct { get; private set; }
        public bool IsEnableAutoCostDeduct { get; private set; }
        public bool IsEnbaleLogContainer { get; private set; }
    }
}
