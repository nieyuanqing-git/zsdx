using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class NMPPrivilige:PriviligeBase
    {
        private INMPAdminBLL __nmpAdminBLL;
        private INMPBLL __nmpBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private int __nmpAdminCount = 0;
        private bool __isNMPOrgAuthorizedPower { get; set; }
        public Guid? NMPId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public NMPPrivilige(Guid userId)
            : base(userId)
        {
            __nmpAdminBLL = BLLFactory.CreateNMPAdminBLL();
            __nmpBLL = BLLFactory.CreateNMPBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
        }
        public NMPPrivilige(Guid userId, Guid nmpId)
            : this(userId) 
        {
            NMPId = nmpId;
            var __nmpAdminCount = __nmpAdminBLL.CountModelListByExpression(string.Format("NMPId=\"{0}\"*AdminId=\"{1}\"", nmpId,userId));
            var nmp = __nmpBLL.GetEntityById(nmpId);
            if (nmp != null && nmp.OrganizationId.HasValue)
                __isNMPOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, nmp.OrganizationId.Value, LabOrganizationAuthorizedType.NMP);
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "NMP");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !NMPId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "List");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSaveBasic = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBasic");
            IsEnableAddIcon = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddIcon");
            IsEnableDeleteIcon = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteIcon");
            IsEnableSaveIcon = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveIcon");
            IsEnableAddNMPEquipment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddNMPEquipment");
            IsEnableEditNMPEquipment = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditNMPEquipment");
            IsEnableDeleteNMPEquipment = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteNMPEquipment");
            IsEnableSaveNMPEquipment = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveNMPEquipment");
            IsEnableSaveAppointmentSetting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAppointmentSetting");
            IsEnableSaveLabel = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveLabel");
            IsEnableEditLabel = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditLabel");
            IsEnableAddLabel = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddLabel");
            IsEnableDeleteLabel = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteLabel");
            IsEnableLabelContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "LabelContainer");
            IsEnableChargeSettingContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "ChargeSettingContainer");
            IsEnableSaveCalculateTimeRule = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveCalculateTimeRule");
            IsEnableAddAddtionChargeItem = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddAdditionChargeItem");
            IsEnableEditAddtionChargeItem = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditAdditionChargeItem");
            IsEnableSaveAddtionChargeItem = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAdditionChargeItem");
            IsEnableDeleteAddtionChargeItem = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteAdditionChargeItem");
            IsEnableSaveBasicChargeSetting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBasicChargeSetting");
            IsEnableEditDurationChargeStandard = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditDurationChargeStandard");
            IsEnableAddDurationChargeStandard = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddDurationChargeStandard");
            IsEnableSaveDurationChargeStandard = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveDurationChargeStandard");
            IsEnableDeleteDurationChargeStandard = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteDurationChargeStandard");
            IsEnableAddNMPLabelChargeStand = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddNMPLabelChargeStand");
            IsEnableAddUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddUnAppointmentPeriodTime");
            IsEnableEditUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditUnAppointmentPeriodTime");
            IsEnableSaveUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveUnAppointmentPeriodTime");
            IsEnableDeleteUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteUnAppointmentPeriodTime");
            IsEnableAddUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddUnAppointmentPeriodTime");

            IsEnableAddAppointmentPriority = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddAppointmentPriority");
            IsEnableEditAppointmentPriority = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditAppointmentPriority");
            IsEnableSaveAppointmentPriority = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAppointmentPriority");
            IsEnableDeleteAppointmentPriority = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteAppointmentPriority");

            IsEnableAddUserNMPTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddUserNMPTime");
            IsEnableEditUserNMPTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditUserNMPTime");
            IsEnableSaveUserNMPTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveUserNMPTime");
            IsEnableDeleteUserNMPTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteUserNMPTime");

            IsEnableAddAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddAppointmentTimeLimit");
            IsEnableEditAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditAppointmentTimeLimit");
            IsEnableSaveAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAppointmentTimeLimit");
            IsEnableDeleteAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteAppointmentTimeLimit");

            IsEnableAddTagEquipmentFun = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddTagEquipmentFun");
            IsEnableEditTagEquipmentFun = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditTagEquipmentFun");
            IsEnableSaveTagEquipmentFun = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveTagEquipmentFun");
            IsEnableDeleteTagEquipmentFun = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteTagEquipmentFun");
            IsEnableSaveAppointmentSpeciaRule = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAppointmentSpeciaRule");
            IsEnableDeleteAppointmentSpeciaRule = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteAppointmentSpeciaRule");


            IsEnableGetGridSubjectAppointmentTimeLimitList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridSubjectAppointmentTimeLimitList");
            IsEnableAddSubjectAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddSubjectAppointmentTimeLimit");
            IsEnableEditSubjectAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditSubjectAppointmentTimeLimit");
            IsEnableSaveSubjectAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveSubjectAppointmentTimeLimit");
            IsEnableDeleteSubjectAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteSubjectAppointmentTimeLimit");

            IsEnableRemoteOpen = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteOpen");
            IsEnableRemoteClose = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteClose");
            IsEnableRemoteReloadControllerStatus = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteReloadControllerStatus");
            IsEnableNMPEquipmentAuthorization = GetIsEnableOpearteExtend(viewModuleFunctionList, "NMPEquipmentAuthorization");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (__nmpAdminCount > 0 && viewModuleFunctionList.Any(p => p.ActionName == actionName))
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, false, __isNMPOrgAuthorizedPower));
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSaveBasic { get; private set; }
        public bool IsEnableAddIcon { get; private set; }
        public bool IsEnableDeleteIcon { get; private set; }
        public bool IsEnableSaveIcon { get; private set; }
        public bool IsEnableAddNMPEquipment { get; private set; }
        public bool IsEnableEditNMPEquipment { get; private set; }
        public bool IsEnableDeleteNMPEquipment { get; private set; }
        public bool IsEnableSaveNMPEquipment { get; private set; }
        public bool IsEnableSaveAppointmentSetting { get; private set; }
        public bool IsEnableLabelContainer { get; private set; }
        public bool IsEnableAddLabel { get; private set; }
        public bool IsEnableEditLabel { get; private set; }
        public bool IsEnableSaveLabel { get; private set; }
        public bool IsEnableDeleteLabel { get; private set; }
        public bool IsEnableChargeSettingContainer { get; private set; }
        public bool IsEnableSaveBasicChargeSetting { get; private set; }
        public bool IsEnableSaveCalculateTimeRule { get; private set; }
        public bool IsEnableAddAddtionChargeItem { get; private set; }
        public bool IsEnableEditAddtionChargeItem { get; private set; }
        public bool IsEnableSaveAddtionChargeItem { get; private set; }
        public bool IsEnableDeleteAddtionChargeItem { get; private set; }
        public bool IsEnableEditDurationChargeStandard { get; private set; }
        public bool IsEnableAddDurationChargeStandard { get; private set; }
        public bool IsEnableSaveDurationChargeStandard { get; private set; }
        public bool IsEnableDeleteDurationChargeStandard { get; private set; }
        public bool IsEnableAddNMPLabelChargeStand { get; private set; }
        public bool IsEnableAddUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableEditUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableSaveUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableDeleteUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableAddAppointmentPriority { get; private set; }
        public bool IsEnableEditAppointmentPriority { get; private set; }
        public bool IsEnableSaveAppointmentPriority { get; private set; }
        public bool IsEnableDeleteAppointmentPriority { get; private set; }
        public bool IsEnableAddUserNMPTime { get; private set; }
        public bool IsEnableEditUserNMPTime { get; private set; }
        public bool IsEnableSaveUserNMPTime { get; private set; }
        public bool IsEnableDeleteUserNMPTime { get; private set; }
        public bool IsEnableAddAppointmentTimeLimit { get; private set; }
        public bool IsEnableEditAppointmentTimeLimit { get; private set; }
        public bool IsEnableSaveAppointmentTimeLimit { get; private set; }
        public bool IsEnableDeleteAppointmentTimeLimit { get; private set; }
        public bool IsEnableAddTagEquipmentFun { get; private set; }
        public bool IsEnableEditTagEquipmentFun { get; private set; }
        public bool IsEnableSaveTagEquipmentFun { get; private set; }
        public bool IsEnableDeleteTagEquipmentFun { get; private set; }
        public bool IsEnableSaveAppointmentSpeciaRule { get; private set; }
        public bool IsEnableDeleteAppointmentSpeciaRule { get; private set; }
        public bool IsEnableGetGridSubjectAppointmentTimeLimitList { get; private set; }
        public bool IsEnableAddSubjectAppointmentTimeLimit { get; private set; }
        public bool IsEnableEditSubjectAppointmentTimeLimit { get; private set; }
        public bool IsEnableSaveSubjectAppointmentTimeLimit { get; private set; }
        public bool IsEnableDeleteSubjectAppointmentTimeLimit { get; private set; }

        public bool IsEnableRemoteOpen { get; private set; }
        public bool IsEnableRemoteClose { get; private set; }
        public bool IsEnableRemoteReloadControllerStatus { get; private set; }
        public bool IsEnableNMPEquipmentAuthorization { get; private set; }
    }
}
