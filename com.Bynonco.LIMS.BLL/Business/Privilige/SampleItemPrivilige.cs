using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class SampleItemPrivilige : PriviligeBase
    {
        private UserWorkScope __userWorkScope = null;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private IEquipmentBLL __equipmentBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private bool __isEquipmentOrgAuthorizedPower { get; set; }
        public SampleItemPrivilige(Guid userId)
            : base(userId)
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
        }
        public SampleItemPrivilige(Guid userId, Guid equipmentId)
            : this(userId)
        {
            __isUserOrgAuthorizedPower = false;
            var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", equipmentId.ToString(), userId.ToString()));
            if (userWorkScopeList != null && userWorkScopeList.Count > 0) __userWorkScope = userWorkScopeList.First();
            var equipment = __equipmentBLL.GetEntityById(equipmentId);
            if (equipment != null && equipment.OrgId.HasValue)
                __isEquipmentOrgAuthorizedPower = __labOrganizationAuthorizedBLL.CheckOrganizationControlPower(userId, equipment.OrgId.Value, LabOrganizationAuthorizedType.Equipment);
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "SampleItem");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableGetGridViewSampleItemList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewSampleItemList");
            IsEnableGetSampleChargeCatogory = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetSampleChargeCatogory");
            IsEnableGetSampleItemPrice = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetSampleItemPrice");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableBatchStop = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchStop");
            IsEnableBatchStart = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchStart");
            IsEnableBatchUpdateInfo = GetIsEnableOpearteExtend(viewModuleFunctionList, "BatchUpdateInfo");
            IsEnableImportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ImportExcel");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || __userWorkScope != null || GetIsEnableOpearte(viewModuleFunctionList, actionName, __isUserOrgAuthorizedPower, __isEquipmentOrgAuthorizedPower);
        }
        public bool IsEnableGetGridViewSampleItemList { get; private set; }
        public bool IsEnableGetSampleChargeCatogory { get; private set; }
        public bool IsEnableGetSampleItemPrice { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableBatchStop { get; private set; }
        public bool IsEnableBatchStart { get; private set; }
        public bool IsEnableBatchUpdateInfo { get; private set; }
        public bool IsEnableImportExcel { get; private set; }
    }
}
