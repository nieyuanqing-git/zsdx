using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class MaterialInfoPrivilige : PriviligeBase
    {
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialInfoBLL __materialInfoBLL;
        private MaterialAdmin __materialAdmin = null;
        private bool __isMaterialInfoOrgAuthorizedPower { get; set; }
        public Guid? MaterialInfoId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public MaterialInfoPrivilige(Guid userId)
            : base(userId)
        {
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialInfoBLL = BLLFactory.CreateMaterialInfoBLL();
        }
        public MaterialInfoPrivilige(Guid userId, Guid materialInfoId)
            : this(userId)
        {
            MaterialInfoId = materialInfoId;
            var materialAdminList = __materialAdminBLL.GetEntitiesByExpression(string.Format("MaterialInfoId=\"{0}\"*UserId=\"{1}\"", materialInfoId.ToString(), userId.ToString()));
            if (materialAdminList != null && materialAdminList.Count > 0)
                __materialAdmin = materialAdminList.First();
            var materialInfo = __materialInfoBLL.GetEntityById(materialInfoId);
            if (materialInfo != null)
                __isMaterialInfoOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, materialInfo.OrganizationId, LabOrganizationAuthorizedType.Equipment);
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("(ControllerName=System*ActionName=MaterialSettingIndex)");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "MaterialInfo", additionQueryExpression);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !MaterialInfoId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "MaterialSettingIndex", true);

            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "Index");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableNote = GetIsEnableOpearteExtend(viewModuleFunctionList, "MaterialInfoLog");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || __materialAdmin != null || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, false, __isMaterialInfoOrgAuthorizedPower));
        }

        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableNote { get; private set; }
    }
}
