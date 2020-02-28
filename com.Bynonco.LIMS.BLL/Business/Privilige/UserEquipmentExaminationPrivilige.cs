using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;


namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class UserEquipmentExaminationPrivilige : PriviligeBase
    {
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewUserEquipmentExaminationBLL __viewUserEquipmentExaminationBLL;

        private UserWorkScope __userWorkScope = null;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        public Guid? UserExaminationId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public UserEquipmentExaminationPrivilige(Guid userId)
            : base(userId) 
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewUserEquipmentExaminationBLL = BLLFactory.CreateViewUserEquipmentExaminationBLL();
        }
        public UserEquipmentExaminationPrivilige(Guid userId, Guid userExaminationId)
            : this(userId) 
        {
            __isUserOrgAuthorizedPower = false;
            UserExaminationId = userExaminationId;
            var viewUserEquipmentExaminationList = __viewUserEquipmentExaminationBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", userExaminationId), null, "", true);
            if (viewUserEquipmentExaminationList != null && viewUserEquipmentExaminationList.Count() > 0)
            {
                var viewUserEquipmentExamination = viewUserEquipmentExaminationList.First();
                if (viewUserEquipmentExamination.EquipmentId.HasValue)
                {
                    var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", viewUserEquipmentExamination.EquipmentId.Value, userId.ToString()));
                    if (userWorkScopeList != null && userWorkScopeList.Count > 0) __userWorkScope = userWorkScopeList.First();
                }
                if (viewUserEquipmentExamination.UserOrgId.HasValue)
                    __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewUserEquipmentExamination.UserOrgId.Value, LabOrganizationAuthorizedType.User);
                if (viewUserEquipmentExamination.EquipmentOrgId.HasValue)
                    __isEquipmentOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewUserEquipmentExamination.EquipmentOrgId.Value, LabOrganizationAuthorizedType.Equipment);
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "UserExamination");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !UserExaminationId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "List"); 
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewUserExamination", true); 
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit"); 
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete"); 
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnablePrint = GetIsEnableOpearteExtend(viewModuleFunctionList, "PrintUserExamination", true);
            IsEnableExamination = GetIsEnableOpearteExtend(viewModuleFunctionList, "Examination", true);
            IssEnableManageEquipmentExamination = viewModuleFunctionList.Any(p=>p.ActionName=="EquipmentExaminationContainer") ;
            IssEnableManageMyEquipmentExamination = viewModuleFunctionList.Any(p => p.ActionName == "MyEquipmentExaminationContainer"); 
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (__userWorkScope != null && viewModuleFunctionList.Any(p => p.ActionName == actionName))
                || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, __isEquipmentOrgAuthorizedPower));
        }
        public bool IsEnableList　{ get; private set; }
        public bool IsEnableManage　{ get; private set; }
        public bool IsEnableView　{ get; private set; }
        public bool IsEnableEdit　{ get; private set; }
        public bool IsEnableDelete　{ get; private set; }
        public bool IsEnableSave　{ get; private set; }
        public bool IsEnablePrint { get; private set; }
        public bool IsEnableExamination　{ get; private set; }
        public bool IssEnableManageEquipmentExamination { get; private set; }
        public bool IssEnableManageMyEquipmentExamination { get; private set; }
    }
}
