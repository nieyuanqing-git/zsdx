using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class NMPUsedConfirmPrivilige:PriviligeBase
    {
        private INMPAdminBLL __nmpAdminBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewNMPUsedConfirmBLL __viewNMPUsedConfirmBLL;
        private int __nmpAdminCount = 0;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isNMPOrgAuthorizedPower { get; set; }
        public Guid? NMPUsedConfirmId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public NMPUsedConfirmPrivilige(Guid userId)
            : base(userId) 
        {
            __nmpAdminBLL = BLLFactory.CreateNMPAdminBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewNMPUsedConfirmBLL = BLLFactory.CreateViewNMPUsedConfirmBLL();
        }
        public NMPUsedConfirmPrivilige(Guid userId, Guid nmpUsedConfirmId)
            : this(userId)
        {
            NMPUsedConfirmId = nmpUsedConfirmId;
            __isUserOrgAuthorizedPower = false;
            __isNMPOrgAuthorizedPower = false;
            Guid? userOrgId = null;
            Guid? nmpId = null;
            Guid? nmpOrgId = null;

            var viewNMPUsedConfirm = __viewNMPUsedConfirmBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"", nmpUsedConfirmId), null, "", true);
            if (viewNMPUsedConfirm != null)
            {
                userOrgId = viewNMPUsedConfirm.UserOrgId;
                nmpId = viewNMPUsedConfirm.NMPId;
                nmpOrgId = viewNMPUsedConfirm.NMPOrgId;
            }
            if (nmpId.HasValue)
            {
                 __nmpAdminCount = __nmpAdminBLL.CountModelListByExpression(string.Format("NMPId=\"{0}\"*AdminId=\"{1}\"", nmpId.Value, userId));
            }
            if (userOrgId.HasValue)
                __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, userOrgId.Value, LabOrganizationAuthorizedType.User);
            if (nmpOrgId.HasValue)
                __isNMPOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, nmpOrgId.Value, LabOrganizationAuthorizedType.NMP);
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "NMPUsedConfirm");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !NMPUsedConfirmId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableMyManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "MyNMPUsedConfirmContainer");
            IsEnableGetGridViewCurrentUsingNMPEquipmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewCurrentUsingNMPEquipmentList");
            IsEnableGetGridTodayNMPUsedConfirmList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridTodayNMPUsedConfirmList");
            IsEnableViewNMPUsedConfirmDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewNMPUsedConfirmDetail", true);
            IsEnableAddNMPUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddNMPUsedConfirm");
            IsEnableEditNMPUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditNMPUsedConfirm");
            IsEnableDeleteNMPUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteNMPUsedConfirm");
            IsEnableEditNMPFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditNMPFeedBack");
            IsEnableSaveNMPFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveNMPFeedBack");
            IsEnableDeleteNMPFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteNMPFeedBack");
            IsEnableSaveNMPUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveNMPUsedConfirm");
            IsEnableCalcFeeNMPUsedConfirm = GetIsEnableOpearteExtend(viewModuleFunctionList, "CalcFeeNMPUsedConfirm");
            IsEnableDirectNMPCostDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "DirectNMPCostDeduct");
            IsEnableExportUnDeductNMPUsedConfirmList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportUnDeductNMPUsedConfirmList");
            IsEnableExportNMPCostDeductList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportNMPCostDeductList");
            IsEnableChangeNMPUsedConfirmUser = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveChangeNMPUser");
            IsEnableNMPUsedConfirmExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "NMPUsedConfirmExportExcel");
            IsEnableNMPRunConditionSumTotalManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "NMPRunConditionSumTotalManage");
            IsEnableExpertNMPRunConditionSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertNMPRunConditionSumTotal");
            IsEnableGetNMPRunConditionSumTotal = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetNMPRunConditionSumTotal");
            IsEnableExpertNMPRunConditionDetail = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExpertNMPRunConditionDetail");
            IsEnableNMPUsedConfirmUseConditionExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "NMPUsedConfirmUseConditionExportExcel");
            IsEnableSelfSaveAppointmentNMPUsedConfirm = viewModuleFunctionList.Any(p => p.ActionName == "SelfSaveAppointmentNMPUsedConfirm");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (__nmpAdminCount > 0  && viewModuleFunctionList.Any(p => p.ActionName == actionName))
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, __isNMPOrgAuthorizedPower));
        }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableMyManage { get; private set; }
        public bool IsEnableGetGridViewCurrentUsingNMPEquipmentList { get; private set; }
        public bool IsEnableGetGridTodayNMPUsedConfirmList { get; private set; }
        public bool IsEnableViewNMPUsedConfirmDetail { get; private set; }
        public bool IsEnableAddNMPUsedConfirm { get; private set; }
        public bool IsEnableEditNMPUsedConfirm { get; private set; }
        public bool IsEnableDeleteNMPUsedConfirm { get; private set; }
        public bool IsEnableSaveNMPUsedConfirm { get; private set; }
        public bool IsEnableCalcFeeNMPUsedConfirm { get; private set; }
        public bool IsEnableDirectNMPCostDeduct { get; private set; }
        public bool IsEnableExportUnDeductNMPUsedConfirmList { get; private set; }
        public bool IsEnableExportNMPCostDeductList { get; private set; }
        public bool IsEnableEditNMPFeedBack { get; private set; }
        public bool IsEnableSaveNMPFeedBack { get; private set; }
        public bool IsEnableDeleteNMPFeedBack { get; private set; }
        public bool IsEnableChangeNMPUsedConfirmUser { get; private set; }
        public bool IsEnableNMPUsedConfirmExportExcel { get; private set; }
        public bool IsEnableNMPRunConditionSumTotalManage { get; private set; }
        public bool IsEnableExpertNMPRunConditionSumTotal { get; private set; }
        public bool IsEnableGetNMPRunConditionSumTotal { get; private set; }
        public bool IsEnableExpertNMPRunConditionDetail { get; private set; }
        public bool IsEnableNMPUsedConfirmUseConditionExportExcel { get; private set; }
        public bool IsEnableSelfSaveAppointmentNMPUsedConfirm { get; set; }
    }
}
