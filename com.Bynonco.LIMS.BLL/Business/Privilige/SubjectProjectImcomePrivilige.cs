using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class SubjectProjectImcomePrivilige : PriviligeBase
    {
        public SubjectProjectImcomePrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "SubjectProjectImcome");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewSubjectProjectImcomeLogList");
            IsEnableErrorList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridSubjectProjectImcomeErrorLogList");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableErrorManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "ErrorManage");
            IsEnableViewLog = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewLog");
            IsEnableViewErrorLog = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewErrorLog");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableErrorList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableErrorManage { get; private set; }
        public bool IsEnableViewLog { get; private set; }
        public bool IsEnableViewErrorLog { get; private set; }
    }
}
