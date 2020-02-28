using System;
using System.Collections.Generic;
using System.Linq;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary> 效益评价表权限 </summary>
    public class EvaluationTablePrivilige : PriviligeBase
    {
        public EvaluationTablePrivilige(Guid userId)
            : base(userId)
        { }

        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "BenefitEvaluation");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;

            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "View");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableModify = GetIsEnableOpearteExtend(viewModuleFunctionList, "Modify");
            IsEnableAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Audit");
            IsEnableDownLoad = GetIsEnableOpearteExtend(viewModuleFunctionList, "DownLoad");
        }

        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }

        public bool IsEnableView { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableModify { get; private set; }
        public bool IsEnableAudit { get; private set; }
        public bool IsEnableDownLoad { get; private set; }
    }
}