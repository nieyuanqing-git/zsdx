using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class MaterialPurchasePrivilige : PriviligeBase
    {
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialPurchaseBLL __materialPurchaseBLL;
        private MaterialAdmin __materialAdmin = null;
        private bool __isMaterialPurchaseOrgAuthorizedPower { get; set; }
        public Guid? MaterialPurchaseId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public MaterialPurchasePrivilige(Guid userId)
            : base(userId)
        {
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialPurchaseBLL = BLLFactory.CreateMaterialPurchaseBLL();
        }
        public MaterialPurchasePrivilige(Guid userId, Guid materialPurchaseId)
            : this(userId)
        {
            MaterialPurchaseId = materialPurchaseId;
            var materialPurchase = __materialPurchaseBLL.GetEntityById(materialPurchaseId);
            if (materialPurchase != null && materialPurchase.MaterialPurchaseItemList != null && materialPurchase.MaterialPurchaseItemList.Count() > 0)
            {
                var materialAdminList = __materialAdminBLL.GetEntitiesByExpression(string.Format("{0}*UserId=\"{1}\"", materialPurchase.MaterialPurchaseItemList.Select(p => p.MaterialInfoId).ToFormatStr("MaterialInfoId"), userId.ToString()));
                if (materialAdminList != null && materialAdminList.Count > 0)
                    __materialAdmin = materialAdminList.First();
                foreach (var item in materialPurchase.MaterialPurchaseItemList)
                {
                    __isMaterialPurchaseOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, item.MaterialInfo.OrganizationId, LabOrganizationAuthorizedType.Equipment);
                    if(__isMaterialPurchaseOrgAuthorizedPower) break;
                }
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("(ControllerName=System*ActionName=MaterialSettingIndex)");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "MaterialPurchase", additionQueryExpression);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !MaterialPurchaseId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);

            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "MaterialSettingIndex");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableNote = GetIsEnableOpearteExtend(viewModuleFunctionList, "Note");
            IsEnableExport = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
            IsEnableGroupAdminAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "GroupAdminAudit");
            IsEnableDirectorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "DirectorAudit");
            IsEnableOperatorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "OperatorAudit");
            IsEnableInputorAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "InputorAudit", true);
            IsEnableBalanceAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "BalanceAudit");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || __materialAdmin != null || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, false, __isMaterialPurchaseOrgAuthorizedPower));
        }

        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableNote { get; private set; }
        public bool IsEnableExport { get; private set; }
        public bool IsEnableGroupAdminAudit { get; private set; }
        public bool IsEnableDirectorAudit { get; private set; }
        public bool IsEnableOperatorAudit { get; private set; }
        public bool IsEnableInputorAudit { get; private set; }
        public bool IsEnableBalanceAudit { get; private set; }
    }
}
