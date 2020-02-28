using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class SampleChargeItemPrivilige : PriviligeBase
    {
        public SampleChargeItemPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "SampleChargeItem");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableGetSampleChargeItemList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetSampleChargeItemList");
            IsEnableGetGridSampleChargeItemList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridSampleChargeItemList");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableBatchStop = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchStop");
            IsEnableBatchStart = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchStart");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableGetSampleChargeItemList { get; private set; }
        public bool IsEnableGetGridSampleChargeItemList { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableBatchStop { get; private set; }
        public bool IsEnableBatchStart { get; private set; }
    }
}
