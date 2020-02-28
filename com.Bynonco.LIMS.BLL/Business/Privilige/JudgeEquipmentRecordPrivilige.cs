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
    public class JudgeEquipmentRecordPrivilige : PriviligeBase
    {
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewJudgeEquipmentRecordBLL __viewJudgeEquipmentRecordBLL;
        private UserWorkScope __userWorkScope = null;
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        public bool IsAuthorizedPower { get; private set; }
        public Guid? JudgeEquipmentRecordId { get; private set; }

        public JudgeEquipmentRecordPrivilige(Guid userId)
            : base(userId) {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewJudgeEquipmentRecordBLL = BLLFactory.CreateViewJudgeEquipmentRecordBLL();
        }
        public JudgeEquipmentRecordPrivilige(Guid userId, Guid judgeEquipmentRecordId)
            : this(userId)
        {
            JudgeEquipmentRecordId = judgeEquipmentRecordId;
            __isEquipmentOrgAuthorizedPower = false;
            var viewJudgeEquipmentRecord = __viewJudgeEquipmentRecordBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"", judgeEquipmentRecordId), null, "", true);
            if (viewJudgeEquipmentRecord != null)
            {
                __userWorkScope = __userWorkScopeBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", viewJudgeEquipmentRecord.EquipmentId, userId.ToString()));
                if (viewJudgeEquipmentRecord.EquipmentOrgId.HasValue)
                    __isEquipmentOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewJudgeEquipmentRecord.EquipmentOrgId.Value, LabOrganizationAuthorizedType.Equipment);
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "JudgeEquipmentRecord");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !JudgeEquipmentRecordId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add", true);
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save", true);
            IsEnableExport = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel", true);
            IsEnableViewRecord = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewRecord", true);
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (__userWorkScope != null && viewModuleFunctionList.Any(p => p.ActionName == actionName))
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, false, __isEquipmentOrgAuthorizedPower));
        }

        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableExport { get; private set; }
        public bool IsEnableViewRecord { get; private set; }
    }
}
