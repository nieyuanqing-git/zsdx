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
    public class DelinquencyPrivilige : PriviligeBase
    {
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IUserBLL __userBLL;
        private bool __isUserOrgAuthorizedPower { get; set; }
        public Guid? PunisherId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public DelinquencyPrivilige(Guid userId)
            : base(userId) 
        {
                __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
                __userBLL = BLLFactory.CreateUserBLL();
        }
        public DelinquencyPrivilige(Guid userId, Guid punisherId)
            : this(userId)
        {
            PunisherId = punisherId;
            __isUserOrgAuthorizedPower = false;
            var punisher = __userBLL.GetEntityById(punisherId);
            if (punisher != null)
            {
                if (punisher.OrganizationId.HasValue)
                    __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, punisher.OrganizationId.Value, LabOrganizationAuthorizedType.User);
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Delinquency");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !PunisherId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableGetGridDelinquencyCategoryList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridDelinquencyCategoryList");
            IsEnableAddDelinquencyCategory = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddDelinquencyCategory");
            IsEnableEditDelinquencyCategory = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditDelinquencyCategory");
            IsEnableSaveDelinquencyCategory = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveDelinquencyCategory");
            IsEnableDeleteDelinquencyCategory = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteDelinquencyCategory");

            IsEnabelGetGridDelinquencyRuleList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridDelinquencyRuleList");
            IsEnableAddDelinquencyRule = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddDelinquencyRule");
            IsEnableEditDelinquencyRule = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditDelinquencyRule");
            IsEnableSaveDelinquencyRule = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveDelinquencyRule");
            IsEnableDeleteDelinquencyRule = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteDelinquencyRule");
            IsEnableSendDelinquencyMsgNotice = GetIsEnableOpearteExtend(viewModuleFunctionList, "SendDelinquencyMsgNotice");

            IsEnableAddDelinquencyConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddDelinquencyConfirm");
            IsEnableSaveDelinquencyConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveDelinquencyConfirm");
            IsEnableDeleteDelinquencyConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteDelinquencyConfirm");
            IsEnableBatchDeleteDelinquencyConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchDeleteDelinquencyConfirm");
            IsEnablePunish = GetIsEnableOpearteExtend(viewModuleFunctionList, "Punish");

            IsEnableGetGridViewPunishActionList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewPunishActionList");
            IsEnablePunishDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "PunishDetail", true);
            IsEnableResend = GetIsEnableOpearteExtend(viewModuleFunctionList, "Resend");
            IsEnableSend = GetIsEnableOpearteExtend(viewModuleFunctionList, "Send");
            IsEnableStopPunish = GetIsEnableOpearteExtend(viewModuleFunctionList, "StopPunish");
            IsEnableBatchStopPunish = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchStopPunish");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, false));
        }        
        public bool IsEnableGetGridDelinquencyCategoryList { get; private set; }
        public bool IsEnableAddDelinquencyCategory { get; private set; }
        public bool IsEnableEditDelinquencyCategory { get; private set; }
        public bool IsEnableSaveDelinquencyCategory { get; private set; }
        public bool IsEnableDeleteDelinquencyCategory { get; private set; }
        public bool IsEnabelGetGridDelinquencyRuleList { get; private set; }
        public bool IsEnableAddDelinquencyRule { get; private set; }
        public bool IsEnableEditDelinquencyRule { get; private set; }
        public bool IsEnableSaveDelinquencyRule { get; private set; }
        public bool IsEnableDeleteDelinquencyRule { get; private set; }
        public bool IsEnableAddDelinquencyConfirm { get; private set; }
        public bool IsEnableSaveDelinquencyConfirm { get; private set; }
        public bool IsEnableDeleteDelinquencyConfirm { get; private set; }
        public bool IsEnableBatchDeleteDelinquencyConfirm { get; private set; }
        public bool IsEnableSendDelinquencyMsgNotice { get; private set; }
        public bool IsEnablePunish { get; private set; }
        public bool IsEnableGetGridViewPunishActionList { get; private set; }
        public bool IsEnablePunishDetail { get; private set; }
        public bool IsEnableResend { get; private set; }
        public bool IsEnableSend { get; private set; }
        public bool IsEnableStopPunish { get; private set; }
        public bool IsEnableBatchStopPunish { get; private set; }
    }
}
