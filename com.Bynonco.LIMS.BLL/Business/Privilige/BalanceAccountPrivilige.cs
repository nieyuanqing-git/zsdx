using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class BalanceAccountPrivilige : PriviligeBase
    {
        public BalanceAccountPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "BalanceAccount");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableSettle = GetIsEnableOpearteExtend(viewModuleFunctionList, "Settle");
            IsEnableUnSettle = GetIsEnableOpearteExtend(viewModuleFunctionList, "UnSettle");
            IsEnableSettleConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "SettleConfirm");
            IsEnableGetGridPayerUnCloseBalanceTotalList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridPayerUnCloseBalanceTotalList");
            IsEnableGetGridPayerUnCloseViewCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridPayerUnCloseViewCostDeductList");
            IsEnableViewUnCloseBalanceTotalDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewUnCloseBalanceTotalDetail");
            IsEnableExportPayerUnCloseBalanceTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportPayerUnCloseBalanceTotal");
            IsEnabelExportPayerUnCloseBalanceTotalDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportPayerUnCloseBalanceTotalDetail");
            IsEnableGetGridBalanceAccountList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridBalanceAccountList");
            IsEnableViewBalanceAccountDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewBalanceAccountDetail");
            IsEnableGetGridPayerCloseBalanceTotalList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridPayerCloseBalanceTotalList");
            IsEnableGetGridPayerCloseViewCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridPayerCloseViewCostDeductList");
            IsEnableViewCloseBalanceTotalDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewCloseBalanceTotalDetail");
            IsEnableExportBalanceAccountTotalDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportBalanceAccountTotalDoc");
            IsEnableExportBalanceAccountDetailDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportBalanceAccountDetailDoc");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableSettle { get; private set; }
        public bool IsEnableUnSettle { get; private set; }
        public bool IsEnableSettleConfirm { get; private set; }
        public bool IsEnableGetGridPayerUnCloseBalanceTotalList { get; private set; }
        public bool IsEnableGetGridPayerUnCloseViewCostDeductList { get; private set; }
        public bool IsEnableViewUnCloseBalanceTotalDetail { get; private set; }
        public bool IsEnableExportPayerUnCloseBalanceTotal { get; private set; }
        public bool IsEnabelExportPayerUnCloseBalanceTotalDetail { get; private set; }
        public bool IsEnableGetGridBalanceAccountList { get; private set; }
        public bool IsEnableViewBalanceAccountDetail { get; private set; }
        public bool IsEnableGetGridPayerCloseBalanceTotalList { get; private set; }
        public bool IsEnableGetGridPayerCloseViewCostDeductList { get; private set; }
        public bool IsEnableViewCloseBalanceTotalDetail { get; private set; }
        public bool IsEnableExportBalanceAccountTotalDoc { get; private set; }
        public bool IsEnableExportBalanceAccountDetailDoc { get; private set; }
    }
}
