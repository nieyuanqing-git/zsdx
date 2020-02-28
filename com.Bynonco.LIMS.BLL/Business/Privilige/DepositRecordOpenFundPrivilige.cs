using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class DepositRecordOpenFundPrivilige : PriviligeBase
    {
        public DepositRecordOpenFundPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "DepositRecordOpenFund");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableMyManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyManage");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableMyAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyAdd");
            IsEnableMyEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyEdit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnablePreAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "PreAudit");
            IsEnableResetPreAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ResetPreAudit");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
            IsEnableMyDepositContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyDepositContainer");
            IsEnableDepositContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "DepositContainer");
            IsEnableDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportDoc");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableMyManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableMyAdd { get; private set; }
        public bool IsEnableMyEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnablePreAudit { get; private set; }
        public bool IsEnableResetPreAudit { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
        public bool IsEnableDepositContainer { get; private set; }
        public bool IsEnableMyDepositContainer { get; private set; }
        public bool IsEnableDoc { get; private set; }
    }
}
