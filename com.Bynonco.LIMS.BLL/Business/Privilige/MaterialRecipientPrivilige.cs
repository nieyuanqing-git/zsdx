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
    public class MaterialRecipientPrivilige : PriviligeBase
    {
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialRecipientBLL __materialRecipientBLL;
        private MaterialAdmin __materialAdmin = null;
        private bool __isMaterialRecipientOrgAuthorizedPower { get; set; }
        public Guid? MaterialRecipientId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public MaterialRecipientPrivilige(Guid userId)
            : base(userId)
        {
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialRecipientBLL = BLLFactory.CreateMaterialRecipientBLL();
        }
        public MaterialRecipientPrivilige(Guid userId, Guid materialRecipientId)
            : this(userId)
        {
            MaterialRecipientId = materialRecipientId;
            var materialRecipient = __materialRecipientBLL.GetEntityById(materialRecipientId);
            if (materialRecipient != null && materialRecipient.MaterialRecipientItemList != null && materialRecipient.MaterialRecipientItemList.Count() > 0)
            {
                var materialAdminList = __materialAdminBLL.GetEntitiesByExpression(string.Format("{0}*UserId=\"{1}\"", materialRecipient.MaterialRecipientItemList.Select(p => p.MaterialInfoId).ToFormatStr("MaterialInfoId"), userId.ToString()));
                if (materialAdminList != null && materialAdminList.Count > 0)
                    __materialAdmin = materialAdminList.First();
                foreach (var item in materialRecipient.MaterialRecipientItemList)
                {
                    __isMaterialRecipientOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, item.MaterialInfo.OrganizationId, LabOrganizationAuthorizedType.Equipment);
                    if(__isMaterialRecipientOrgAuthorizedPower) break;
                }
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("(ControllerName=System*ActionName=MaterialSettingIndex)");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "MaterialRecipient", additionQueryExpression);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !MaterialRecipientId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "MaterialSettingIndex", true);

            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "Index");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Audit");
            IsEnableRecipientOutput = GetIsEnableOpearteExtend(viewModuleFunctionList, "RecipientOutput");
            IsEnableRecipientDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "RecipientDeduct");
            IsEnableNote = GetIsEnableOpearteExtend(viewModuleFunctionList, "Note");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || __materialAdmin != null || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, false, __isMaterialRecipientOrgAuthorizedPower));
        }

        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableAudit { get; private set; }
        public bool IsEnableRecipientOutput { get; private set; }
        public bool IsEnableRecipientDeduct { get; private set; }
        public bool IsEnableNote { get; private set; }
    }
}
