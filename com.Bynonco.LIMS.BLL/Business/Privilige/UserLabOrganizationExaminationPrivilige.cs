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
    public class UserLabOrganizationExaminationPrivilige : PriviligeBase
    {
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewUserLabOrganizationExaminationBLL __viewUserLabOrganizationExaminationBLL;

        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        public Guid? UserExaminationId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public UserLabOrganizationExaminationPrivilige(Guid userId)
            : base(userId) 
        {
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewUserLabOrganizationExaminationBLL = BLLFactory.CreateViewUserLabOrganizationExaminationBLL();
        }
        public UserLabOrganizationExaminationPrivilige(Guid userId, Guid userExaminationId)
            : this(userId) 
        {
            __isUserOrgAuthorizedPower = false;
            UserExaminationId = userExaminationId;
            var viewUserLabOrganizationExaminationList = __viewUserLabOrganizationExaminationBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", userExaminationId), null, "", true);
            if (viewUserLabOrganizationExaminationList != null && viewUserLabOrganizationExaminationList.Count() > 0)
            {
                var viewUserLabOrganizationExamination = viewUserLabOrganizationExaminationList.First();
                if (viewUserLabOrganizationExamination.UserOrgId.HasValue)
                    __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewUserLabOrganizationExamination.UserOrgId.Value, LabOrganizationAuthorizedType.User);
                if (viewUserLabOrganizationExamination.OrgId.HasValue)
                    __isEquipmentOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewUserLabOrganizationExamination.OrgId.Value, LabOrganizationAuthorizedType.Equipment);
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
            IsEnableManageLabOrganizationExamination = viewModuleFunctionList.Any(p => p.ActionName == "LabOrganizationExaminationContainer");
            IsEnableManageMyLabOrganizationExamination = viewModuleFunctionList.Any(p => p.ActionName == "MyLabOrganizationExaminationContainer");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, __isEquipmentOrgAuthorizedPower));
        }
        public bool IsEnableList　{ get; private set; }
        public bool IsEnableManage　{ get; private set; }
        public bool IsEnableView　{ get; private set; }
        public bool IsEnableEdit　{ get; private set; }
        public bool IsEnableDelete　{ get; private set; }
        public bool IsEnableSave　{ get; private set; }
        public bool IsEnableExamination　{ get; private set; }
        public bool IsEnablePrint { get; private set; }
        public bool IsEnableManageLabOrganizationExamination{ get; private set; }
        public bool IsEnableManageMyLabOrganizationExamination{ get; private set; }
       
    }
}
