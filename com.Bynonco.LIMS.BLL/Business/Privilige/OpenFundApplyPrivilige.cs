using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class OpenFundApplyPrivilige : PriviligeBase
    {
        public OpenFundApplyPrivilige(Guid userId)
            : base(userId)
        {}
        
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "OpenFundApply");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;

            IsEnableMyManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyManage");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Audit");
            IsEnableBalance = GetIsEnableOpearteExtend(viewModuleFunctionList, "Balance");
            IsEnableImportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ImportExcel");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");

            IsEnableMyDepositContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyDepositContainer");
            IsEnableMyDepositAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyDepositAdd");
            IsEnableMyDepositEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyDepositEdit");

            IsEnableDepositContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "DepositContainer");
            IsEnableDepositAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "DepositAdd");
            IsEnableDepositEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "DepositEdit");
            
            IsEnableDepositDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "DepositDelete");
            IsEnableDepositAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "DepositAudit");
            IsEnableDepositPreAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "DepositPreAudit");

        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }

        public bool IsEnableMyManage { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableAudit { get; private set; }
        public bool IsEnableBalance { get; private set; }
        public bool IsEnableImportExcel { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
        
        public bool IsEnableMyDepositContainer { get; private set; }
        public bool IsEnableMyDepositAdd { get; private set; }
        public bool IsEnableMyDepositEdit { get; private set; }
        public bool IsEnableDepositContainer { get; private set; }
        public bool IsEnableDepositAdd { get; private set; }
        public bool IsEnableDepositEdit { get; private set; }
        public bool IsEnableDepositDelete { get; private set; }
        public bool IsEnableDepositAudit { get; private set; }
        public bool IsEnableDepositPreAudit { get; private set; }
    }
}
