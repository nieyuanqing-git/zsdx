using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class EquipmentOpenPracticePrivilige : PriviligeBase
    {
        public EquipmentOpenPracticePrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "EquipmentOpenPractice");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewInfo");
            IsEnableViewClosed = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewClosedInfo");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableExportWord = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportWord");
            IsEnableExportClosedWord = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportClosedWord");
            IsEnableSaveBasic = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBasic");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableTeacherAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "TeacherAudit");
            IsEnableCollegeAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "CollegeAudit");
            IsEnableManageAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ManageAudit");
            IsEnableManagePreAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ManagePreAudit");
            IsEnableAllocatingFund = GetIsEnableOpearteExtend(viewModuleFunctionList, "AllocatingFundAuditSubmit");
            IsEnableClosed = GetIsEnableOpearteExtend(viewModuleFunctionList, "Closed");
            IsEnableSaveBasicClosed = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBasicClosed");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
            IsEnableClosedTeacherAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ClosedTeacherAudit");
            IsEnableClosedCollegeAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ClosedCollegeAudit");
            IsEnableClosedManageAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ClosedManageAudit");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableView { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableExportWord { get; private set; }
        public bool IsEnableExportClosedWord { get; private set; }
        public bool IsEnableSaveBasic { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableTeacherAudit { get; private set; }
        public bool IsEnableCollegeAudit { get; private set; }
        public bool IsEnableManageAudit { get; private set; }
        public bool IsEnableManagePreAudit { get; private set; }
        public bool IsEnableAllocatingFund { get; private set; }
        public bool IsEnableClosed { get; private set; }
        public bool IsEnableSaveBasicClosed { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
        public bool IsEnableViewClosed { get; private set; }
        public bool IsEnableClosedTeacherAudit { get; private set; }
        public bool IsEnableClosedCollegeAudit { get; private set; }
        public bool IsEnableClosedManageAudit { get; private set; }
    }
}
