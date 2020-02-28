using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class EquipmentPrivilige:PriviligeBase
    {
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private IEquipmentBLL __equipmentBLL;
        private int __userWorkScopeCount = 0;
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        public Guid? EquipmentId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public EquipmentPrivilige(Guid userId)
            : base(userId) 
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
        }
        public EquipmentPrivilige(Guid userId, Guid equipmentId)
            : this(userId) 
        {
            EquipmentId = equipmentId;
            __userWorkScopeCount = __userWorkScopeBLL.CountModelListByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", equipmentId.ToString(), userId.ToString()));
            var equipment = __equipmentBLL.GetEntityById(equipmentId);
            if (equipment != null && equipment.OrgId.HasValue)
                __isEquipmentOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, equipment.OrgId.Value, LabOrganizationAuthorizedType.Equipment);
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("((ControllerName=EquipmentTrainning*ActionName=ApplyTrainning)+(ControllerName=UserExamination*ActionName=Examination)");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Equipment", additionQueryExpression);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !EquipmentId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index",true);

            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "Index");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSaveBasic = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBasic");
            IsEnableEditIcon = GetIsEnableOpearteExtend(viewModuleFunctionList, "IconContainer");
            IsEnableSaveIcon = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveIcon");
            IsEnableDeleteIcon = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteIcon");
            IsEnableUseControlSettingContrainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "UseControlSettingContrainer");
            IsEnableSaveUseControlSetting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveUseControlSetting");
            IsEnableAppointmentSettingContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "AppointmentSettingContainer");
            IsEnableSaveAppointmentSetting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAppointmentSetting");
            IsEnableSampleSettingContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "SampleSettingContainer");
            IsEnableSaveSampleSetting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveSampleSetting");
            IsEnableStatusSettingContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "StatusSettingContainer");
            IsEnableSaveStatusSetting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveStatusSetting");
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
            IsEnableAddEquipmentLabelChargeStand = GetIsEnableOpearteExtend(viewModuleFunctionList, "AttachmentContainer");
            IsEnableAddEquipmentLabelChargeStand = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditAttachment");
            IsEnableAddEquipmentLabelChargeStand = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAttachment");
            IsEnableAddEquipmentLabelChargeStand = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteAttachment");
            IsEnableAddAttachment = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddAttachment");
            IsEnableEditAttachment = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditAttachment");
            IsEnableSaveAttachment = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAttachment");
            IsEnableDeleteAttachment = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteAttachment");
            IsEnableBlackListContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "BlackListContainer");
            IsEnableEditBlackListMessageTemplate = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditBlackListMessageTemplate");
            IsEnableSaveBlackListMessageTemplate = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBlackListMessageTemplate");
            IsEnableAddBlackList = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddBlackList");
            IsEnableEditBlackList = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditBlackList");
            IsEnableSaveBlackList = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBlackList");
            IsEnableDeleteBlackList = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteBlackList");
            IsEnableNoticeContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "NoticeContainer");
            IsEnableAddNotice = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddNotice");
            IsEnableEditNotice = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditNotice");
            IsEnableSaveNotice = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveNotice");
            IsEnableDeleteNotice = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteNotice");
            IsEnableDeleteNoticeAttachment = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteNoticeAttachmen");
            IsEnableUploadNoticeAttachment = GetIsEnableOpearteExtend(viewModuleFunctionList, "UploadNoticeAttachment");
            IsEnableTrainningContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "TrainningContainer");
            IsEnableAddTrainning = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddTrainning");
            IsEnableEditTrainning = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditTrainning");
            IsEnableDeleteTrainning = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteTrainning");
            IsEnableSaveTrainning = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveTrainning");


            IsEnableAddUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddUnAppointmentPeriodTime");
            IsEnableEditUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditUnAppointmentPeriodTime"); 
            IsEnableSaveUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveUnAppointmentPeriodTime"); 
            IsEnableDeleteUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteUnAppointmentPeriodTime");
            IsEnableAddUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddUnAppointmentPeriodTime");

            IsEnableAddSampleUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddSampleUnAppointmentPeriodTime");
            IsEnableEditSampleUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditSampleUnAppointmentPeriodTime");
            IsEnableSaveSampleUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveSampleUnAppointmentPeriodTime");
            IsEnableDeleteSampleUnAppointmentPeriodTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteSampleUnAppointmentPeriodTime");
            
            IsEnableAddAppointmentPriority = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddAppointmentPriority");
            IsEnableEditAppointmentPriority = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditAppointmentPriority");
            IsEnableSaveAppointmentPriority = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveAppointmentPriority");
            IsEnableDeleteAppointmentPriority = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteAppointmentPriority");

            IsEnableAddUserEquipmentTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddUserEquipmentTime"); 
            IsEnableEditUserEquipmentTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditUserEquipmentTime"); 
            IsEnableSaveUserEquipmentTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveUserEquipmentTime"); 
            IsEnableDeleteUserEquipmentTime = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteUserEquipmentTime"); 

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


            IsEnableGetGridSubjectAppointmentTimeLimitList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridSubjectAppointmentTimeLimitList", true);
            IsEnableAddSubjectAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddSubjectAppointmentTimeLimit", true);
            IsEnableEditSubjectAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditSubjectAppointmentTimeLimit", true);
            IsEnableSaveSubjectAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveSubjectAppointmentTimeLimit", true);
            IsEnableDeleteSubjectAppointmentTimeLimit = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteSubjectAppointmentTimeLimit", true);

            IsEnableGetGridMarkingList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridMarkingList");
            IsEnableAddMarking = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddEquipmentMarking", true);
            IsEnableEditMarking = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditEquipmentMarking");
            IsEnableSaveMarking = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveEquipmentMarking");
            IsEnableDeleteMarking = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteEquipmentMarking");

            IsEnableGetGridPartList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridEquipmentPartList");
            IsEnableAddPart = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddPart", true);
            IsEnableEditPart = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditPart");
            IsEnableSavePart = GetIsEnableOpearteExtend(viewModuleFunctionList, "SavePart");
            IsEnableDeletePart = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeletePart");

            IsEnableBrokenReportManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "BrokenReportManage");
            IsEnableGetGridBrokenReportList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridBrokenReportList");
            IsEnableAddBrokenReport = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddBrokenReport",true);
            IsEnableEditBrokenReport = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditBrokenReport");
            IsEnableSaveBrokenReport = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBrokenReport");
            IsEnableDeleteBrokenReport = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteBrokenReport");

            IsEnableGetGridRepairList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridRepairList");
            IsEnableAddRepair = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddRepair");
            IsEnableEditRepair = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditRepair");
            IsEnableSaveRepair = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveRepair");
            IsEnableDeleteRepair = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteRepair");

            IsEnableStatusLogContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "StatusLogContainer");
            IsEnableGetGridEquipmentStatusLogList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridEquipmentStatusLogList");
            IsEnableDeleteStatusLog = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteStatusLog");

            IsEnableExaminationManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExaminationManage");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");

            IsEnableAuthorizationList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserEquipmentAuthorizationList"); 
            IsEnableAuthorizationManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentAuthorizationManage"); 
            IsEnableAuthorizationEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentAuthorizationEdit");
            IsEnableAuthorizationDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentAuthorizationDelete");
            IsEnableAuthorizationSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentAuthorizationSave");

            IsEnableContinuedAuthorizationList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewUserEquipmentContinuedAuthorizationList");
            IsEnableContinuedAuthorizationManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentContinuedAuthorizationManage");
            IsEnableContinuedAuthorizationEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentContinuedAuthorizationEdit");
            IsEnableContinuedAuthorizationDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentContinuedAuthorizationDelete");
            IsEnableContinuedAuthorizationSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "UserEquipmentContinuedAuthorizationSave");

            IsEnableRemoteControl = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteControl");
            IsEnableRemoteOpen = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteOpen");
            IsEnableRemoteClose = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteClose");
            IsEnableRemoteReloadControllerStatus = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteReloadControllerStatus");
            IsEnableRemoteUpdateAdminCard = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteUpdateAdminCard");
            IsEnableRemoteUpdateEquipmentInfo = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteUpdateEquipmentInfo");
            IsEnableRemoteUpdateOfflineNum = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteUpdateOfflineNum");
            IsEnableRemoteUpdateOfflineRecord = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteUpdateOfflineRecord");
            IsEnableRemoteDeleteOfflineRecord = GetIsEnableOpearteExtend(viewModuleFunctionList, "RemoteDeleteOfflineRecord");

            IsEnableEditFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditFeedBack");
            IsEnableSaveFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveFeedBack");
            IsEnableDeleteFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteFeedBack");

            IsEnableCameraPlay = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentRelationCameraPlay");
            IsEnableCameraVideoRecord = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentVideoRecord");

            IsEnableApplyTrainning = GetIsEnableOpearteExtend(viewModuleFunctionList, "ApplyTrainning" ,true);
            IsEnableExamination = GetIsEnableOpearteExtend(viewModuleFunctionList, "Examination", true);
            IsEnableReadingMaterial = GetIsEnableOpearteExtend(viewModuleFunctionList, "Examination", true);
            
            IsEnableAuthorizationEquipmentEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit" ,true);
            IsEnableAuthorizationEquipmentDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete", true);
            IsEnableAuthorizationEquipmentCameraPlay = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentRelationCameraPlay", true);
            IsEnableAuthorizationEquipmentCameraVideoRecord = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentVideoRecord", true);

            IsEnableGetGridEquipmentUseConditionList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridEquipmentUseConditionList", true);
            IsEnableAddEquipmentUseCondition = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddEquipmentUseCondition", true);
            IsEnableEditEquipmentUseCondition = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditEquipmentUseCondition", true);
            IsEnableSaveEquipmentUseCondition = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveEquipmentUseCondition", true);
            IsEnableDeleteEquipmentUseCondition = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteEquipmentUseCondition", true);

            IsEnableApplyEquipment = GetIsEnableOpearteExtend(viewModuleFunctionList, "ApplyEquipment", true);
            IsEanbleCancelApplyEquipment = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelApplyEquipment", true);
            IsEnableSaveExaminationSetting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveExaminationSetting", true);

            IsEnableExportDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportDoc");

            IsEnableAlarmManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "AlarmManage");
            IsEnableAlarmDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteEquipmentAlarm");
            IsEnableAlarmOpen = GetIsEnableOpearteExtend(viewModuleFunctionList, "SetEquipmentAlarmOpen");
            IsEnableAlarmClosed = GetIsEnableOpearteExtend(viewModuleFunctionList, "SetEquipmentAlarmCloesd");

            IsEnableOpen = GetIsEnableOpearteExtend(viewModuleFunctionList, "Open", true);
            IsEnableClose = GetIsEnableOpearteExtend(viewModuleFunctionList, "Close", true);

            IsEnableDutyFreeExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentDutyFreeSearch");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (__userWorkScopeCount > 0 && viewModuleFunctionList.Any(p => p.ActionName == actionName))
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, false, __isEquipmentOrgAuthorizedPower));
        }
        
        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSaveBasic { get; private set; }
        public bool IsEnableEditIcon { get; private set; }
        public bool IsEnableSaveIcon { get; private set; }
        public bool IsEnableDeleteIcon { get; private set; }
        public bool IsEnableUseControlSettingContrainer { get; private set; }
        public bool IsEnableSaveUseControlSetting { get; private set; }
        public bool IsEnableAppointmentSettingContainer { get; private set; }
        public bool IsEnableSaveAppointmentSetting { get; private set; }
        public bool IsEnableSampleSettingContainer { get; private set; }
        public bool IsEnableSaveSampleSetting { get; private set; }
        public bool IsEnableStatusSettingContainer { get; private set; }
        public bool IsEnableSaveStatusSetting { get; private set; }
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
        public bool IsEnableAddEquipmentLabelChargeStand { get; private set; }
        public bool IsEnableAddAttachment { get; private set; }
        public bool IsEnableEditAttachment { get; private set; }
        public bool IsEnableAttachmentContainer { get; private set; }
        public bool IsEnableSaveAttachment { get; private set; }
        public bool IsEnableDeleteAttachment { get; private set; }
        public bool IsEnableBlackListContainer { get; private set; }
        public bool IsEnableEditBlackListMessageTemplate { get; private set; }
        public bool IsEnableSaveBlackListMessageTemplate { get; private set; }
        public bool IsEnableAddBlackList { get; private set; }
        public bool IsEnableEditBlackList { get; private set; }
        public bool IsEnableSaveBlackList { get; private set; }
        public bool IsEnableDeleteBlackList { get; private set; }
        public bool IsEnableNoticeContainer { get; private set; }
        public bool IsEnableAddNotice { get; private set; }
        public bool IsEnableEditNotice { get; private set; }
        public bool IsEnableSaveNotice { get; private set; }
        public bool IsEnableDeleteNotice { get; private set; }
        public bool IsEnableDeleteNoticeAttachment { get; private set; }
        public bool IsEnableUploadNoticeAttachment { get; private set; }
        public bool IsEnableTrainningContainer { get; private set; }
        public bool IsEnableAddTrainning { get; private set; }
        public bool IsEnableEditTrainning { get; private set; }
        public bool IsEnableDeleteTrainning { get; private set; }
        public bool IsEnableSaveTrainning { get; private set; }


        public bool IsEnableAddUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableEditUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableSaveUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableDeleteUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableAddSampleUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableEditSampleUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableSaveSampleUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableDeleteSampleUnAppointmentPeriodTime { get; private set; }
        public bool IsEnableAddAppointmentPriority { get; private set; }
        public bool IsEnableEditAppointmentPriority { get; private set; }
        public bool IsEnableSaveAppointmentPriority { get; private set; }
        public bool IsEnableDeleteAppointmentPriority { get; private set; }
        public bool IsEnableAddUserEquipmentTime { get; private set; }
        public bool IsEnableEditUserEquipmentTime { get; private set; }
        public bool IsEnableSaveUserEquipmentTime { get; private set; }
        public bool IsEnableDeleteUserEquipmentTime { get; private set; }
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



        public bool IsEnableGetGridMarkingList { get; private set; }
        public bool IsEnableAddMarking { get; private set; }
        public bool IsEnableEditMarking { get; private set; }
        public bool IsEnableSaveMarking { get; private set; }
        public bool IsEnableDeleteMarking { get; private set; }
        public bool IsEnableGetGridPartList { get; private set; }
        public bool IsEnableAddPart { get; private set; }
        public bool IsEnableEditPart { get; private set; }
        public bool IsEnableSavePart { get; private set; }
        public bool IsEnableDeletePart { get; private set; }
        public bool IsEnableBrokenReportManage { get; private set; }
        public bool IsEnableGetGridBrokenReportList { get; private set; }
        public bool IsEnableAddBrokenReport { get; private set; }
        public bool IsEnableEditBrokenReport { get; private set; }
        public bool IsEnableSaveBrokenReport { get; private set; }
        public bool IsEnableDeleteBrokenReport { get; private set; }
        public bool IsEnableGetGridRepairList { get; private set; }
        public bool IsEnableAddRepair { get; private set; }
        public bool IsEnableEditRepair { get; private set; }
        public bool IsEnableSaveRepair { get; private set; }
        public bool IsEnableDeleteRepair { get; private set; }
        public bool IsEnableStatusLogContainer { get; private set; }
        public bool IsEnableGetGridEquipmentStatusLogList { get; private set; }
        public bool IsEnableDeleteStatusLog { get; private set; }
        public bool IsEnableExaminationManage { get; private set; }
        public bool IsEnableExportExcel { get; private set; }

        public bool IsEnableAuthorizationList { get; private set; }
        public bool IsEnableAuthorizationManage { get; private set; }
        public bool IsEnableAuthorizationEdit { get; private set; }
        public bool IsEnableAuthorizationDelete { get; private set; }
        public bool IsEnableAuthorizationSave { get; private set; }

        public bool IsEnableContinuedAuthorizationList { get; private set; }
        public bool IsEnableContinuedAuthorizationManage { get; private set; }
        public bool IsEnableContinuedAuthorizationEdit { get; private set; }
        public bool IsEnableContinuedAuthorizationDelete { get; private set; }
        public bool IsEnableContinuedAuthorizationSave { get; private set; }

        public bool IsEnableRemoteControl { get; private set; }
        public bool IsEnableRemoteOpen { get; private set; }
        public bool IsEnableRemoteClose { get; private set; }
        public bool IsEnableRemoteReloadControllerStatus { get; private set; }
        public bool IsEnableRemoteUpdateAdminCard { get; private set; }
        public bool IsEnableRemoteUpdateEquipmentInfo { get; private set; }
        public bool IsEnableRemoteUpdateOfflineNum { get; private set; }
        public bool IsEnableRemoteUpdateOfflineRecord { get; private set; }
        public bool IsEnableRemoteDeleteOfflineRecord { get; private set; }

        public bool IsEnableEditFeedBack { get; private set; }
        public bool IsEnableSaveFeedBack { get; set; }
        public bool IsEnableDeleteFeedBack { get; set; }

        public bool IsEnableCameraPlay { get; set; }
        public bool IsEnableCameraVideoRecord { get; set; }

        public bool IsEnableApplyTrainning { get; set; }
        public bool IsEnableExamination { get; set; }
        public bool IsEnableReadingMaterial { get; set; }
        
        public bool IsEnableAuthorizationEquipmentEdit { get; private set; }
        public bool IsEnableAuthorizationEquipmentDelete { get; private set; }
        public bool IsEnableAuthorizationEquipmentCameraPlay { get; private set; }
        public bool IsEnableAuthorizationEquipmentCameraVideoRecord { get; private set; }

        public bool IsEnableGetGridEquipmentUseConditionList { get; private set; }
        public bool IsEnableAddEquipmentUseCondition { get; private set; }
        public bool IsEnableEditEquipmentUseCondition { get; private set; }
        public bool IsEnableSaveEquipmentUseCondition { get; private set; }
        public bool IsEnableDeleteEquipmentUseCondition { get; private set; }

        public bool IsEnableApplyEquipment { get; set; }
        public bool IsEanbleCancelApplyEquipment { get; set; }

        public bool IsEnableSaveExaminationSetting { get; set; }

        public bool IsEnableExportDoc { get; set; }

        public bool IsEnableAlarmManage { get; private set; }
        public bool IsEnableAlarmDelete { get; private set; }
        public bool IsEnableAlarmOpen { get; private set; }
        public bool IsEnableAlarmClosed { get; private set; }


        public bool IsEnableOpen { get; private set; }
        public bool IsEnableClose { get; private set; }

        public bool IsEnableDutyFreeExportExcel { get; private set; }
    }
}
