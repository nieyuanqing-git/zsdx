using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class ActivityApplyPrivilige : PriviligeBase
    {
        public ActivityApplyPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "ActivityApply");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewInfo");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableGroupAdminAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "GroupAdminAudit");
            IsEnableDirectorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "DirectorAudit");
            IsEnableOperatorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "OperatorAudit");
            IsEnableExportDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportDoc");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableView { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableGroupAdminAudit { get; private set; }
        public bool IsEnableDirectorAudit { get; private set; }
        public bool IsEnableOperatorAudit { get; private set; }
        public bool IsEnableExportDoc { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
    }
}
