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
    public class MaterialBrokenPrivilige : PriviligeBase
    {
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialBrokenBLL __materialBrokenBLL;
        private MaterialAdmin __materialAdmin = null;
        private bool __isMaterialBrokenOrgAuthorizedPower { get; set; }
        public Guid? MaterialBrokenId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public MaterialBrokenPrivilige(Guid userId)
            : base(userId)
        {
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialBrokenBLL = BLLFactory.CreateMaterialBrokenBLL();
        }
        public MaterialBrokenPrivilige(Guid userId, Guid materialBrokenId)
            : this(userId)
        {
            MaterialBrokenId = materialBrokenId;
            var materialBroken = __materialBrokenBLL.GetEntityById(materialBrokenId);
            if (materialBroken != null && materialBroken.MaterialBrokenItemList != null && materialBroken.MaterialBrokenItemList.Count() > 0)
            {
                var materialAdminList = __materialAdminBLL.GetEntitiesByExpression(string.Format("{0}*UserId=\"{1}\"", materialBroken.MaterialBrokenItemList.Select(p => p.MaterialInfoId).ToFormatStr("MaterialInfoId"), userId.ToString()));
                if (materialAdminList != null && materialAdminList.Count > 0)
                    __materialAdmin = materialAdminList.First();
                foreach (var item in materialBroken.MaterialBrokenItemList)
                {
                    __isMaterialBrokenOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, item.MaterialInfo.OrganizationId, LabOrganizationAuthorizedType.Equipment);
                    if(__isMaterialBrokenOrgAuthorizedPower) break;
                }
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "MaterialBroken");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !MaterialBrokenId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);

            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "Index");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Audit");
            IsEnableOutput = GetIsEnableOpearteExtend(viewModuleFunctionList, "Output");
            IsEnableNote = GetIsEnableOpearteExtend(viewModuleFunctionList, "Note");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || __materialAdmin != null || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, false, __isMaterialBrokenOrgAuthorizedPower));
        }

        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableAudit { get; private set; }
        public bool IsEnableOutput { get; private set; }
        public bool IsEnableNote { get; private set; }
    }
}
