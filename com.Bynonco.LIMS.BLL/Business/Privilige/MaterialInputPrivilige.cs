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
    public class MaterialInputPrivilige : PriviligeBase
    {
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialInputBLL __materialInputBLL;
        private MaterialAdmin __materialAdmin = null;
        private bool __isMaterialInputOrgAuthorizedPower { get; set; }
        public Guid? MaterialInputId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public MaterialInputPrivilige(Guid userId)
            : base(userId)
        {
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialInputBLL = BLLFactory.CreateMaterialInputBLL();
        }
        public MaterialInputPrivilige(Guid userId, Guid materialInputId)
            : this(userId)
        {
            MaterialInputId = materialInputId;
            var materialInput = __materialInputBLL.GetEntityById(materialInputId);
            if (materialInput != null && materialInput.MaterialInputItemList != null && materialInput.MaterialInputItemList.Count() > 0)
            {
                var materialAdminList = __materialAdminBLL.GetEntitiesByExpression(string.Format("{0}*UserId=\"{1}\"", materialInput.MaterialInputItemList.Select(p => p.MaterialInfoId).ToFormatStr("MaterialInfoId"), userId.ToString()));
                if (materialAdminList != null && materialAdminList.Count > 0)
                    __materialAdmin = materialAdminList.First();
                foreach (var item in materialInput.MaterialInputItemList)
                {
                    __isMaterialInputOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, item.MaterialInfo.OrganizationId, LabOrganizationAuthorizedType.Equipment);
                    if(__isMaterialInputOrgAuthorizedPower) break;
                }
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("(ControllerName=System*ActionName=MaterialSettingIndex)");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "MaterialInput", additionQueryExpression);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !MaterialInputId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "MaterialSettingIndex", true);

            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "Index");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableNote = GetIsEnableOpearteExtend(viewModuleFunctionList, "Note");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || __materialAdmin != null || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, false, __isMaterialInputOrgAuthorizedPower));
        }

        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableNote { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
    }
}
