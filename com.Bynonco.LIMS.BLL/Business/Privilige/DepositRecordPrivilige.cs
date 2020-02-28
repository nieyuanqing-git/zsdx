using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class DepositRecordPrivilige : PriviligeBase
    {
        public DepositRecordPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("((ControllerName=UserAccountRecord*(ActionName=MyUserAccountRecordContainer+ActionName=UserAccountRecordPayerContainer)))");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "DepositRecord", additionQueryExpression);
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableMyManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyManage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableMyEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyEdit");
            IsEnableRealEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "RealEdit");
            IsEnableVirtualEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "VirtualEdit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Audit");
            IsEnablePreAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "PreAudit");
            IsEnableResetPreAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ResetPreAudit");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
            IsEnableTesterExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "TesterExportExcel");
            IsEnableDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportDoc");
            IsEnableMyDepositContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyDepositContainer");
            IsEnableMyUserAccountRecordContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyUserAccountRecordContainer");
            IsEnableUserAccountRecordPayerContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserAccountRecordPayerContainer");
            IsEnableDepositContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "DepositContainer");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableMyManage { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableMyEdit { get; private set; }
        public bool IsEnableRealEdit { get; private set; }
        public bool IsEnableVirtualEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableAudit { get; private set; }
        public bool IsEnablePreAudit { get; private set; }
        public bool IsEnableResetPreAudit { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
        public bool IsEnableTesterExportExcel { get; private set; }
        public bool IsEnableDoc { get; private set; }
        public bool IsEnableMyUserAccountRecordContainer { get; private set; }
        public bool IsEnableUserAccountRecordPayerContainer { get; private set; }
        public bool IsEnableMyDepositContainer { get; private set; }
        public bool IsEnableDepositContainer { get; private set; }
    }
}
