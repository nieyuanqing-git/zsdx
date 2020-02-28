using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class EquipmentRepairFundsApplyPrivilige:PriviligeBase
    {
        public EquipmentRepairFundsApplyPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "EquipmentRepairFundsApply");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableApply = GetIsEnableOpearteExtend(viewModuleFunctionList, "Apply");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "View");
            IsEnablePrint = GetIsEnableOpearteExtend(viewModuleFunctionList, "Print");
            IsEnableCancel = GetIsEnableOpearteExtend(viewModuleFunctionList, "Cancel");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableFinishRepairRegister = GetIsEnableOpearteExtend(viewModuleFunctionList, "FinishRepairRegister");
            IsEnableFundsProvide = GetIsEnableOpearteExtend(viewModuleFunctionList, "FundsProvide");

            IsEnableLabRoomAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "LabRoomAudit");
            IsEnableLabRoomAuditNoPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "LabRoomAuditNoPass");
            IsEnableLabRoomAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "LabRoomAuditPass");

            IsEnableCollegeAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "CollegeAudit");
            IsEnableCollegeAuditAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "CollegeAuditPass");
            IsEnableCollegeAuditNoPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "CollegeAuditNoPass");

            IsEnableEquipmentDeptAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentDeptAudit");
            IsEnableEquipmentDeptAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentDeptAuditPass");
            IsEnableEquipmentDeptAuditNoPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "EquipmentDeptAuditNoPass");

            IsEnableShareEDirectorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ShareEAudit");
            IsEnableShareEDirectorAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "ShareEAuditPass");
            IsEnableShareEDirectorAuditNoPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "ShareEAuditNoPass");


            IsEnableSchoolAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SchoolAudit");
            IsEnableSchoolAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SchoolAuditPass");
            IsEnableSchoolAuditNoPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SchoolAuditNoPass");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableApply { get; private set; }
        public bool IsEnableView { get; private set; }
        public bool IsEnablePrint { get; private set; }
        public bool IsEnableCancel { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableDelete { get; private set; }

        public bool IsEnableLabRoomAudit{ get; private set; }
        public bool IsEnableLabRoomAuditNoPass { get; private set; }
        public bool IsEnableLabRoomAuditPass { get; private set; }

        public bool IsEnableCollegeAudit{ get; private set; }
        public bool IsEnableCollegeAuditAuditPass { get; private set; }
        public bool IsEnableCollegeAuditNoPass { get; private set; }

        public bool IsEnableEquipmentDeptAudit { get; private set; }
        public bool IsEnableEquipmentDeptAuditPass { get; private set; }
        public bool IsEnableEquipmentDeptAuditNoPass { get; private set; }

        public bool IsEnableShareEDirectorAudit { get; private set; }
        public bool IsEnableShareEDirectorAuditPass { get; private set; }
        public bool IsEnableShareEDirectorAuditNoPass { get; private set; }

        public bool IsEnableFinishRepairRegister { get; private set; }
        public bool IsEnableFundsProvide { get; private set; }

        public bool IsEnableSchoolAudit { get; private set; }
        public bool IsEnableSchoolAuditPass { get; private set; }
        public bool IsEnableSchoolAuditNoPass { get; private set; }
    }
}
