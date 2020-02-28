using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class EquipmentApplyPrivilige:PriviligeBase
    { 
        public EquipmentApplyPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "EquipmentApply");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableApply = GetIsEnableOpearteExtend(viewModuleFunctionList, "Apply");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "View");
            IsEnableExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "Expert");
            IsEnableCancel = GetIsEnableOpearteExtend(viewModuleFunctionList, "Cancel");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableGetIconData = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSaveIcon = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveIcon");
            IsEnableDeleteIcon = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteIcon");

            IsEnableLabRoomDirectorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "LabRoomDirectorAudit");
            IsEnableLabRoomDirectorAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "LabRoomDirectorAuditPass");
            IsEnableLabRoomDirectorAuditNoPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "LabRoomDirectorAuditNoPass");

            IsEnableSchoolDirectorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SchoolDirectorAudit");
            IsEnableSchoolDirectorAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SchoolDirectorAuditPass");
            IsEnableSchoolDirectorAuditNoPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SchoolDirectorAuditNoPass");

            IsEnableShareEDirectorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ShareEDirectorAudit");
            IsEnableShareEDirectorAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "ShareEDirectorAuditPass");
            IsEnableShareEDirectorAuditNoPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "ShareEDirectorAuditNoPass");


            IsEnableGetEquipmentGroupServerList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetEquipmentGroupServerList");
            IsEnableAddEquipmentGroupServer = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddEquipmentGroupServer");
            IsEnableEditEquipmentGroupServer = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditEquipmentGroupServer");
            IsEnableSaveEquipmentGroupServer = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveEquipmentGroupServer");
            IsEnableDeleteEquipmentGroupServer = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteEquipmentGroupServer");

            IsEnableGetEquipmentServiceHoursStatList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetEquipmentServiceHoursStatList");
            IsEnableAddEquipmentServiceHoursStat = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddEquipmentServiceHoursStat");
            IsEnableEditEquipmentServiceHoursStat = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditEquipmentServiceHoursStat");
            IsEnableSaveEquipmentServiceHoursStat = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveEquipmentServiceHoursStat");
            IsEnableDeleteEquipmentServiceHoursStat = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteEquipmentServiceHoursStat");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableApply { get; private set; }
        public bool IsEnableView { get; private set; }
        public bool IsEnableExpert { get; private set; }
        public bool IsEnableCancel { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableDelete { get; private set; }

        public bool IsEnableGetIconData { get; private set; }
        public bool IsEnableSaveIcon { get; private set; }
        public bool IsEnableDeleteIcon { get; private set; }

        public bool IsEnableGetEquipmentGroupServerList { get; private set; }
        public bool IsEnableAddEquipmentGroupServer { get; private set; }
        public bool IsEnableEditEquipmentGroupServer { get; private set; }
        public bool IsEnableSaveEquipmentGroupServer { get; private set; }
        public bool IsEnableDeleteEquipmentGroupServer { get; private set; }

        public bool IsEnableGetEquipmentServiceHoursStatList { get; private set; }
        public bool IsEnableAddEquipmentServiceHoursStat { get; private set; }
        public bool IsEnableEditEquipmentServiceHoursStat { get; private set; }
        public bool IsEnableSaveEquipmentServiceHoursStat { get; private set; }
        public bool IsEnableDeleteEquipmentServiceHoursStat { get; private set; }

        public bool IsEnableLabRoomDirectorAudit{ get; private set; }
        public bool IsEnableLabRoomDirectorAuditPass { get; private set; }
        public bool IsEnableLabRoomDirectorAuditNoPass { get; private set; }

        public bool IsEnableSchoolDirectorAudit { get; private set; }
        public bool IsEnableSchoolDirectorAuditPass { get; private set; }
        public bool IsEnableSchoolDirectorAuditNoPass { get; private set; }

        public bool IsEnableShareEDirectorAudit { get; private set; }
        public bool IsEnableShareEDirectorAuditPass { get; private set; }
        public bool IsEnableShareEDirectorAuditNoPass { get; private set; }
    }
}
