using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class JudgeProjectPrivilige : PriviligeBase
    {
        public JudgeProjectPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "JudgeProject");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetViewJudgeProjectList");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableAdjustXPath = GetIsEnableOpearteExtend(viewModuleFunctionList, "AdjustXPath");

            IsEnableContentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetViewJudgeProjectContentList");
            IsEnableContentAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddContent");
            IsEnableContentEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditContent");
            IsEnableContentDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteContent");
            IsEnableContentSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveContent");
            IsEnableContentAdjustXPath = GetIsEnableOpearteExtend(viewModuleFunctionList, "AdjustXPathContent");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableAdjustXPath { get; private set; }

        public bool IsEnableContentList { get; private set; }
        public bool IsEnableContentAdd { get; private set; }
        public bool IsEnableContentEdit { get; private set; }
        public bool IsEnableContentDelete { get; private set; }
        public bool IsEnableContentSave { get; private set; }
        public bool IsEnableContentAdjustXPath { get; private set; }
    }
}
