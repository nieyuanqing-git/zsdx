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
    public class AppointmentPrivilige : PriviligeBase
    {
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewAppointmentBLL __viewAppointmentBLL;
        private UserWorkScope __userWorkScope = null;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        private ViewAppointment __viewAppointment;
        public Guid? AppointmentId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public AppointmentPrivilige(Guid userId)
            : base(userId)
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
        }
        public AppointmentPrivilige(Guid userId, ViewAppointment viewAppointment)
            : this(userId)
        {
            __viewAppointment = viewAppointment;
            __isUserOrgAuthorizedPower = false;
            __isEquipmentOrgAuthorizedPower = false;
            if (viewAppointment != null )
            {
                AppointmentId = viewAppointment.Id;
                if (viewAppointment.EquipmentId.HasValue)
                {
                    var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", viewAppointment.EquipmentId.Value, userId.ToString()));
                    if (userWorkScopeList != null && userWorkScopeList.Count > 0) __userWorkScope = userWorkScopeList.First();
                }
                if (viewAppointment.UserOrgId.HasValue)
                    __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewAppointment.UserOrgId.Value, LabOrganizationAuthorizedType.User);
                if (viewAppointment.EquipmentOrgId.HasValue)
                    __isEquipmentOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewAppointment.EquipmentOrgId.Value, LabOrganizationAuthorizedType.Equipment);
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("(ControllerName=UsedConfirm*(ActionName=SaveUsedConfirm+ActionName=DirectCostDeduct+ActionName=ViewUsedConfirmDetail+ActionName=SelfAddAppointmentUsedConfirm))");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Appointment", additionQueryExpression);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !AppointmentId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableAppiontmentDetailInfo = GetIsEnableOpearteExtend(viewModuleFunctionList, "AppointmentDetailInfo", true);
            IsEnableGetGridViewAppointmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewAppointmentList");
            IsEnableAppointmentUserRelativeInfo = GetIsEnableOpearteExtend(viewModuleFunctionList, "AppointmentUserRelativeInfo");
            IsEnableAppointmentTotalInfo = GetIsEnableOpearteExtend(viewModuleFunctionList, "AppointmentTotalInfo");
            IsEnableAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "Appointment");
            IsEnableCancelAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelAppointment", true);
            IsEnabelChangeAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "Print", true);
            IsEnablePrint = viewModuleFunctionList.Any(p => p.ActionName == "SelfAddAppointmentUsedConfirm");

            IsEnableRegisterBreakAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "RegisterBreakAppointment");
            IsEnableCancelRegisterBreakAppointmentOperate = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelRegisterBreakAppointment");
            IsEnableAddAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddAppointment");
            IsEnableAuditAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditAppointment");
            IsEnableAddAppointmentUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddAppointmentUsedConfirm");
            IsEnableExportExcelTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcelTotal");
            IsEnableExportExcelDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcelDetail");
            IsEnableExportExcelSearch = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcelSearch");
            IsEnableAppointmentTodayContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "AppointmentTodayContainer");
            //IsEnableSelfAddAppointmentUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "SelfAddAppointmentUsedConfirm");
            IsEnableSelfAddAppointmentUsedConfirm = viewModuleFunctionList.Any(p => p.ActionName == "SelfAddAppointmentUsedConfirm");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (__userWorkScope != null && viewModuleFunctionList.Any(p => p.ActionName == actionName))
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, __isEquipmentOrgAuthorizedPower));
        }

        public bool IsEnableManage { get; private set; }
        public bool IsEnableAppiontmentDetailInfo { get; private set; }
        public bool IsEnableGetGridViewAppointmentList { get; private set; }
        public bool IsEnableAppointmentUserRelativeInfo { get; private set; }
        public bool IsEnableAppointmentTotalInfo { get; private set; }
        public bool IsEnableAppointment { get; private set; }
        public bool IsEnableCancelAppointment { get; private set; }
        public bool IsEnabelChangeAppointment { get; private set; }
        public bool IsEnableRegisterBreakAppointment { get; private set; }
        public bool IsEnableCancelRegisterBreakAppointmentOperate { get; private set; }
        public bool IsEnableAddAppointment { get; private set; }
        public bool IsEnableExportExcelTotal { get; private set; }
        public bool IsEnableExportExcelDetail { get; private set; }
        public bool IsEnableExportExcelSearch { get; private set; }
        public bool IsEnableAuditAppointment { get; private set; }
        public bool IsEnableAppointmentDetailInfo { get; private set; }
        public bool IsEnableAddAppointmentUsedConfirm { get; private set; }
        public bool IsEnableSelfAddAppointmentUsedConfirm { get; private set; }
        public bool IsEnableAppointmentTodayContainer { get; private set; }
        public bool IsEnablePrint { get; private set; }
    }
}
