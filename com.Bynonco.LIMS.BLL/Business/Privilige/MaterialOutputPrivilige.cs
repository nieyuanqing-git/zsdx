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
    public class MaterialOutputPrivilige : PriviligeBase
    {
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialOutputBLL __materialOutputBLL;
        private MaterialAdmin __materialAdmin = null;
        private bool __isMaterialOutputOrgAuthorizedPower { get; set; }
        public Guid? MaterialOutputId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public MaterialOutputPrivilige(Guid userId)
            : base(userId)
        {
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialOutputBLL = BLLFactory.CreateMaterialOutputBLL();
        }
        public MaterialOutputPrivilige(Guid userId, Guid materialOutputId)
            : this(userId)
        {
            MaterialOutputId = materialOutputId;
            var materialOutput = __materialOutputBLL.GetEntityById(materialOutputId);
            if (materialOutput != null && materialOutput.MaterialOutputItemList != null && materialOutput.MaterialOutputItemList.Count() > 0)
            {
                var materialAdminList = __materialAdminBLL.GetEntitiesByExpression(string.Format("{0}*UserId=\"{1}\"", materialOutput.MaterialOutputItemList.Select(p => p.MaterialInfoId).ToFormatStr("MaterialInfoId"), userId.ToString()));
                if (materialAdminList != null && materialAdminList.Count > 0)
                    __materialAdmin = materialAdminList.First();
                foreach (var item in materialOutput.MaterialOutputItemList)
                {
                    __isMaterialOutputOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, item.MaterialInfo.OrganizationId, LabOrganizationAuthorizedType.Equipment);
                    if(__isMaterialOutputOrgAuthorizedPower) break;
                }
            }
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            string additionQueryExpression = string.Format("(ControllerName=System*ActionName=MaterialSettingIndex)");
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "MaterialOutput", additionQueryExpression);
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !MaterialOutputId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "MaterialSettingIndex", true);

            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "Index");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "Deduct");
            IsEnableCancelDeduct = GetIsEnableOpearteExtend(viewModuleFunctionList, "CancelDeduct");
            IsEnableNote = GetIsEnableOpearteExtend(viewModuleFunctionList, "Note");
            IsEnableExportStatList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportMaterialOutputStatList");
            IsEnableExportDetailStatList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportMaterialOutputStatDetailList");
            IsEnableDetailStatList = GetIsEnableOpearteExtend(viewModuleFunctionList, "MaterialOutputStatDetailList");
            IsEnableExportList = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || __materialAdmin != null || (isAuthorizedPower.Value && GetIsEnableOpearte(viewModuleFunctionList, actionName, false, __isMaterialOutputOrgAuthorizedPower));
        }

        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableDeduct { get; private set; }
        public bool IsEnableCancelDeduct { get; private set; }
        public bool IsEnableNote { get; private set; }
        public bool IsEnableExportStatList { get; private set; }
        public bool IsEnableExportDetailStatList { get; private set; }
        public bool IsEnableDetailStatList { get; private set; }
        public bool IsEnableExportList { get; private set; }
    }
}
