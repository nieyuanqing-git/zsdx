using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class EquipmentOpenTrainingPrivilige : PriviligeBase
    {
        public EquipmentOpenTrainingPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "EquipmentOpenTraining");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewInfo");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableCollegeAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "CollegeAudit");
            IsEnableManageAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ManageAudit");
            IsEnableDirectorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "DirectorAudit");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
            IsEnableExportWord = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportWord");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableView { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableCollegeAudit { get; private set; }
        public bool IsEnableManageAudit { get; private set; }
        public bool IsEnableDirectorAudit { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
        public bool IsEnableExportWord { get; private set; }
    }
}
