using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class QAPrivilige : PriviligeBase
    {
        public QAPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "QA");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewQuestionList");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableAnswer = GetIsEnableOpearteExtend(viewModuleFunctionList, "Answer");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAnswer");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "QAView");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAnswer { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableView { get; private set; }
    }
}
