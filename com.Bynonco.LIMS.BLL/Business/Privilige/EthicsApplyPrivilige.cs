using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class EthicsApplyPrivilige : PriviligeBase
    {
        public EthicsApplyPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "EthicsApply");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableGetGridViewEthicsApplyList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewEthicsApplyList");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableCancel = GetIsEnableOpearteExtend(viewModuleFunctionList, "Cancel");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "View");
            IsEnableApply = GetIsEnableOpearteExtend(viewModuleFunctionList, "Apply");
            IsEnablePrint = GetIsEnableOpearteExtend(viewModuleFunctionList, "Print");
            IsEnableAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditPass");
            IsEnableAuditNotPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditNotPass");

            IsEnableGetEthicsApplyAnimalExperimenterList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetEthicsApplyAnimalExperimenterList");
            IsEnableEditEthicsApplyAnimalExperimenter = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditEthicsApplyAnimalExperimenter");
            IsEnableAddEthicsApplyAnimalExperimenter = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddEthicsApplyAnimalExperimenter");
            IsEnableSaveEthicsApplyAnimalExperimenter = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveEthicsApplyAnimalExperimenter");
            IsEnableDeleteEthicsApplyAnimalExperimenter = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteEthicsApplyAnimalExperimenter");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableGetGridViewEthicsApplyList { get; private set; }
        public bool IsEnableCancel { get; private set; }
        public bool IsEnableView { get; private set; }
        public bool IsEnableApply { get; private set; }
        public bool IsEnablePrint { get; private set; }
        public bool IsEnableAuditPass { get; private set; }
        public bool IsEnableAuditNotPass { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableGetEthicsApplyAnimalExperimenterList { get; private set; }
        public bool IsEnableEditEthicsApplyAnimalExperimenter { get; private set; }
        public bool IsEnableAddEthicsApplyAnimalExperimenter { get; private set; }
        public bool IsEnableSaveEthicsApplyAnimalExperimenter { get; private set; }
        public bool IsEnableDeleteEthicsApplyAnimalExperimenter { get; private set; }
    }
}
