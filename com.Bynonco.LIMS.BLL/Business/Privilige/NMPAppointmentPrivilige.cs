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
    public class NMPAppointmentPrivilige : PriviligeBase
    {
        private INMPAdminBLL __nmpAdminBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewNMPAppointmentBLL __viewNMPAppointmentBLL;
        private int __nmpAdminCount = 0;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isNMPOrgAuthorizedPower { get; set; }
        private ViewNMPAppointment __viewNMPAppointment;
        public Guid? NMPAppointmentId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public NMPAppointmentPrivilige(Guid userId)
            : base(userId)
        {
            __nmpAdminBLL = BLLFactory.CreateNMPAdminBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewNMPAppointmentBLL = BLLFactory.CreateViewNMPAppointmentBLL();
        }
        public NMPAppointmentPrivilige(Guid userId, ViewNMPAppointment viewNMPAppointment)
            : this(userId)
        {
            __viewNMPAppointment = viewNMPAppointment;
            __isUserOrgAuthorizedPower = false;
            __isNMPOrgAuthorizedPower = false;
            if (viewNMPAppointment != null )
            {
                NMPAppointmentId = viewNMPAppointment.Id;
                __nmpAdminCount = __nmpAdminBLL.CountModelListByExpression(string.Format("NMPId=\"{0}\"*AdminId=\"{1}\"", viewNMPAppointment.NMPId, userId));
                if (viewNMPAppointment.UserOrgId.HasValue)
                    __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewNMPAppointment.UserOrgId.Value, LabOrganizationAuthorizedType.User);
                if (viewNMPAppointment.NMPOrgId.HasValue)
                    __isNMPOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewNMPAppointment.NMPOrgId.Value, LabOrganizationAuthorizedType.NMP);
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("(ControllerName=NMPUsedConfirm*(ActionName=SaveUsedConfirm+ActionName=DirectCostDeduct+ActionName=ViewNMPUsedConfirmDetail+ActionName=SelfAddNMPAppointmentUsedConfirm))");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "NMPAppointment", additionQueryExpression);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !NMPAppointmentId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableAppiontmentDetailInfo = GetIsEnableOpearteExtend(viewModuleFunctionList, "AppointmentDetailInfo", true);
            IsEnableGetGridViewAppointmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewNMPAppointmentList");
            IsEnableAppointmentUserRelativeInfo = GetIsEnableOpearteExtend(viewModuleFunctionList, "AppointmentUserRelativeInfo");
            IsEnableAppointmentTotalInfo = GetIsEnableOpearteExtend(viewModuleFunctionList, "AppointmentTotalInfo");
            IsEnableAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "Appointment");
            IsEnableCancelAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelAppointment", true);
            IsEnablePrint = viewModuleFunctionList.Any(p => p.ActionName == "Print");
            IsEnableRegisterBreakAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "RegisterBreakAppointment");
            IsEnableCancelRegisterBreakAppointmentOperate = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelRegisterBreakAppointment");
            IsEnableAddAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddAppointment");
            IsEnableAuditAppointment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AuditAppointment");
            IsEnableAddAppointmentUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddAppointmentUsedConfirm");
            IsEnableExportExcelTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcelTotal");
            IsEnableExportExcelDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcelDetail");
            IsEnableExportExcelSearch = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcelSearch");
            IsEnableAppointmentTodayContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "AppointmentTodayContainer");
            IsEnableSelfAddAppointmentUsedConfirm = viewModuleFunctionList.Any(p => p.ActionName == "SelfAddAppointmentUsedConfirm");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            bool isExist = viewModuleFunctionList.Any(p => p.ActionName == actionName);
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (__nmpAdminCount > 0 && isExist)
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, __isNMPOrgAuthorizedPower));
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
