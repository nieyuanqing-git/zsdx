using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class AnimalCategoryPrivilige:PriviligeBase
    {
        public AnimalCategoryPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "AnimalCategory");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetTreeGridAnimalCategoryList");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableAdjustXPath = GetIsEnableOpearteExtend(viewModuleFunctionList, "AdjustXPath");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableList{ get; private set; }
        public bool IsEnableManage{ get; private set; }
        public bool IsEnableEdit{ get; private set; }
        public bool IsEnableDelete{ get; private set; }
        public bool IsEnableAdjustXPath{ get; private set; }
        public bool IsEnableSave{ get; private set; }
    }
}
