using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class AnimalAppointmentPrivilige : PriviligeBase
    {
        public AnimalAppointmentPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "AnimalAppointment");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableGetMyDelegateGridViewAnimalAppointmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetMyDelegateGridViewAnimalAppointmentList");
            IsEnableGetDelegateGridViewAnimalAppointmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetDelegateGridViewAnimalAppointmentList");
            IsEnableViewDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewDelegateAppointment");
            IsEnableAddDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddDelegateAppointment");
            IsEnablePrintDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "PrintDelegateAppointment");
            IsEnableEditDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditDelegateAppointment");
            IsEnableSaveDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveDelegateAppointment");
            IsEnableCancelDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelDelegateAppointment");
            IsEnableAuditPassDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditPassDelegateAppointment");
            IsEnableAuditNotPassDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditNotPassDelegateAppointment");
            IsEnablePurchaseDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "PurchaseDelegateAppointment");
            IsEnableExpertDelegateAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertDelegateAppointment");

            IsEnableGetMySelfGridViewAnimalAppointmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetMySelfGridViewAnimalAppointmentList");
            IsEnableGetSelfGridViewAnimalAppointmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetSelfGridViewAnimalAppointmentList");
            IsEnableViewSelfAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewSelfAppointment");
            IsEnableAddSelfAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddSelfAppointment");
            IsEnablePrintSelfAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "PrintSelfAppointment");
            IsEnableEditSelfAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditSelfAppointment");
            IsEnableSaveSelfAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveSelfAppointment");
            IsEnableCancelSelfAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelSelfAppointment");
            IsEnableAuditPassSelfAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditPassSelfAppointment");
            IsEnableAuditNotPassSelfAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditNotPassSelfAppointment");
            IsEnablePurchaseSelfAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "PurchaseSelfAppointment");
            IsEnableInputSelfAppointmentAnimalQualifiedNo = GetIsEnableOpearteExtend(viewModuleFunctionList, "InputSelfAppointmentAnimalQualifiedNo");
            IsEnableInputDelegateAppointmentAnimalQualifiedNo = GetIsEnableOpearteExtend(viewModuleFunctionList, "InputDelegateAppointmentAnimalQualifiedNo");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableGetMyDelegateGridViewAnimalAppointmentList { get; private set; }
        public bool IsEnableGetDelegateGridViewAnimalAppointmentList { get; private set; }
        public bool IsEnableViewDelegateAppointment { get; private set; }
        public bool IsEnableAddDelegateAppointment { get; private set; }
        public bool IsEnablePrintDelegateAppointment { get; private set; }
        public bool IsEnableEditDelegateAppointment { get; private set; }
        public bool IsEnableSaveDelegateAppointment { get; private set; }
        public bool IsEnableCancelDelegateAppointment { get; private set; }
        public bool IsEnableAuditPassDelegateAppointment { get; private set; }
        public bool IsEnableAuditNotPassDelegateAppointment { get; private set; }
        public bool IsEnablePurchaseDelegateAppointment { get; private set; }
        public bool IsEnableExpertDelegateAppointment { get; private set; }

        public bool IsEnableGetMySelfGridViewAnimalAppointmentList { get; private set; }
        public bool IsEnableGetSelfGridViewAnimalAppointmentList { get; private set; }
        public bool IsEnableViewSelfAppointment { get; private set; }
        public bool IsEnableAddSelfAppointment { get; private set; }
        public bool IsEnablePrintSelfAppointment { get; private set; }
        public bool IsEnableEditSelfAppointment { get; private set; }
        public bool IsEnableSaveSelfAppointment { get; private set; }
        public bool IsEnableCancelSelfAppointment { get; private set; }
        public bool IsEnableAuditPassSelfAppointment { get; private set; }
        public bool IsEnableAuditNotPassSelfAppointment { get; private set; }
        public bool IsEnablePurchaseSelfAppointment { get; private set; }
        public bool IsEnableInputSelfAppointmentAnimalQualifiedNo { get; private set; }
        public bool IsEnableInputDelegateAppointmentAnimalQualifiedNo { get; private set; }

    }
}
