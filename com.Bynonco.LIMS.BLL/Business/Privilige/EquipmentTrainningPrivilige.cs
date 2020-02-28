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
    public class EquipmentTrainningPrivilige : PriviligeBase
    {
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private IViewEquipmentTrainningBLL __viewEquipmentTrainningBLL;

        private UserWorkScope __userWorkScope = null;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        public Guid? EquipmentTrainningId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public EquipmentTrainningPrivilige(Guid userId)
            : base(userId) 
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __viewEquipmentTrainningBLL = BLLFactory.CreateViewEquipmentTrainningBLL();
        }
        public EquipmentTrainningPrivilige(Guid userId, Guid userExaminationId)
            : this(userId) 
        {
            __isUserOrgAuthorizedPower = false;
            EquipmentTrainningId = userExaminationId;
            var viewEquipmentTrainningList = __viewEquipmentTrainningBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", userExaminationId), null, "", true);
            if (viewEquipmentTrainningList != null && viewEquipmentTrainningList.Count() > 0)
            {
                var viewEquipmentTrainning = viewEquipmentTrainningList.First();
                var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", viewEquipmentTrainning.EquipmentId, userId.ToString()));
                if (userWorkScopeList != null && userWorkScopeList.Count > 0) __userWorkScope = userWorkScopeList.First();
                if (viewEquipmentTrainning.CreatorOrgId.HasValue)
                    __isUserOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewEquipmentTrainning.CreatorOrgId.Value, LabOrganizationAuthorizedType.User);
                if (viewEquipmentTrainning.EquipmentOrgId.HasValue)
                    __isEquipmentOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, viewEquipmentTrainning.EquipmentOrgId.Value, LabOrganizationAuthorizedType.Equipment);
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("(ControllerName=Equipment*(ActionName=EditTrainning+ActionName=DeleteTrainning+ActionName=SaveTrainning))");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "EquipmentTrainning", additionQueryExpression);
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !EquipmentTrainningId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewEquipmentTrainningList"); 
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditTrainning");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteTrainning");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveTrainning");
            IsEnableApply = GetIsEnableOpearteExtend(viewModuleFunctionList, "ApplyTrainning", true); 
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
        public bool IsEnableApply { get; private set; }
    }
}
