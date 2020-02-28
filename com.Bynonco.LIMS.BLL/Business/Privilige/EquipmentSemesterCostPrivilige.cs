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
    public class EquipmentSemesterCostPrivilige : PriviligeBase
    {
        public EquipmentSemesterCostPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "EquipmentSemesterCost");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewInfo");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Audit");
            IsEnableAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditPass");
            IsEnableAuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditReject");
            IsEnableWaittingAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "WaittingAudit");
            IsEnableMutiAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "MutiAdd");
            IsEnableMutiSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "MutiSave");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");

        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableView { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableAudit { get; private set; }
        public bool IsEnableAuditPass { get; private set; }
        public bool IsEnableAuditReject { get; private set; }
        public bool IsEnableWaittingAudit { get; private set; }
        public bool IsEnableMutiAdd { get; private set; }
        public bool IsEnableMutiSave { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
    }
}
