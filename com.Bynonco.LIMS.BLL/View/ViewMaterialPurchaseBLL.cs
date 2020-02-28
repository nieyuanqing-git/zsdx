using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewMaterialPurchaseBLL : BLLBase<ViewMaterialPurchase>, IViewMaterialPurchaseBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialPurchaseItemBLL __materialPurchaseItemBLL;
        private IViewMaterialInfoBLL __viewMaterialInfoBLL;

        public ViewMaterialPurchaseBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialPurchaseItemBLL = BLLFactory.CreateMaterialPurchaseItemBLL();
            __viewMaterialInfoBLL = BLLFactory.CreateViewMaterialInfoBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewMaterialPurchase> viewMaterialPurchaseList, bool isSupressLazyLoad = false)
        {
            if (viewMaterialPurchaseList == null || viewMaterialPurchaseList.Count == 0) return;
            foreach (var viewMaterialPurchase in viewMaterialPurchaseList)
            {
                viewMaterialPurchase.IsSupressLazyLoad = false;
                viewMaterialPurchase.InitOperate();
                OperateDecorate(userId, viewMaterialPurchase);
                viewMaterialPurchase.BuildOperate();
                viewMaterialPurchase.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewMaterialPurchase viewMaterialPurchase)
        {
            if (viewMaterialPurchase == null) throw new ArgumentException("耗材采购单为空");
            var materialPurchasePrivilige = userId.HasValue ? PriviligeFactory.GetMaterialPurchasePrivilige(userId.Value, viewMaterialPurchase.Id) : null;
            if (materialPurchasePrivilige == null)
            {
                viewMaterialPurchase.ModifyOperate = "";
                viewMaterialPurchase.DeleteOperate = "";
                viewMaterialPurchase.GroupAdminAuditOperate = "";
                viewMaterialPurchase.DirectorAuditOperate = "";
                viewMaterialPurchase.OperatorAuditOperate = "";
                viewMaterialPurchase.InputorAuditPassOperate = "";
                viewMaterialPurchase.InputorAuditRejectOperate = "";
                viewMaterialPurchase.BalanceAuditPassOperate = "";
                viewMaterialPurchase.BalanceAuditRejectOperate = "";
                viewMaterialPurchase.NoteOperate = "";
                viewMaterialPurchase.ExportOperate = "";
                return;
            }
            if (!materialPurchasePrivilige.IsEnableEdit) viewMaterialPurchase.ModifyOperate = "";
            if (!materialPurchasePrivilige.IsEnableDelete) viewMaterialPurchase.DeleteOperate = "";
            if (!materialPurchasePrivilige.IsEnableGroupAdminAudit) viewMaterialPurchase.GroupAdminAuditOperate = "";
            if (!materialPurchasePrivilige.IsEnableDirectorAudit) viewMaterialPurchase.DirectorAuditOperate = "";
            if (!materialPurchasePrivilige.IsEnableOperatorAudit) viewMaterialPurchase.OperatorAuditOperate = "";
            if (!materialPurchasePrivilige.IsEnableInputorAudit)
            {
                viewMaterialPurchase.InputorAuditPassOperate = "";
                viewMaterialPurchase.InputorAuditRejectOperate = "";
            }
            if (!materialPurchasePrivilige.IsEnableBalanceAudit)
            {
                viewMaterialPurchase.BalanceAuditPassOperate = "";
                viewMaterialPurchase.BalanceAuditRejectOperate = "";
            }
            if (!materialPurchasePrivilige.IsEnableNote) viewMaterialPurchase.NoteOperate = "";
            var isGroupAdminAudit = false;
            var isDirectorAudit = false;
            var isOperatorAudit = false;
            var isInputorAudit = false;
            var isBalanceAudit = false;
            switch (viewMaterialPurchase.StatusEnum)
            {
                case MaterialPurchaseStatus.WaittingAudit:
                case MaterialPurchaseStatus.GroupAdminAuditReject:
                    isGroupAdminAudit = true;
                    break;
                case MaterialPurchaseStatus.GroupAdminAuditPass:
                    isGroupAdminAudit = true;
                    isDirectorAudit = true;
                    break;
                case MaterialPurchaseStatus.DirectorAuditReject:
                    isDirectorAudit = true;
                    break;
                case MaterialPurchaseStatus.DirectorAuditPass:
                    isDirectorAudit = true;
                    isOperatorAudit = true;
                    break;
                case MaterialPurchaseStatus.OperatorAuditReject:
                    isOperatorAudit = true;
                    break;
                case MaterialPurchaseStatus.OperatorAuditPass:
                    isOperatorAudit = true;
                    isInputorAudit = true;
                    break;
                case MaterialPurchaseStatus.InputReject:
                    isInputorAudit = true;
                    break;
                case MaterialPurchaseStatus.InputPass:
                    isInputorAudit = true;
                    isBalanceAudit = true;
                    break;
                case MaterialPurchaseStatus.BalanceReject:
                    isBalanceAudit = true;
                    break;
            }
            if (!isGroupAdminAudit) viewMaterialPurchase.GroupAdminAuditOperate = "";
            if (!isDirectorAudit) viewMaterialPurchase.DirectorAuditOperate = "";
            if (!isOperatorAudit) viewMaterialPurchase.OperatorAuditOperate = "";
            if (!isInputorAudit)
            {
                viewMaterialPurchase.InputorAuditPassOperate = "";
                viewMaterialPurchase.InputorAuditRejectOperate = "";
            }
            if (!isBalanceAudit)
            {
                viewMaterialPurchase.BalanceAuditPassOperate = "";
                viewMaterialPurchase.BalanceAuditRejectOperate = "";
            }
            if (viewMaterialPurchase.StatusEnum != MaterialPurchaseStatus.WaittingAudit)
            {
                viewMaterialPurchase.ModifyOperate = "";
                viewMaterialPurchase.DeleteOperate = "";
            }
            if (viewMaterialPurchase.GroupAdminAuditOperate != "")
            {
                if (viewMaterialPurchase.EquipmentGroupAdminList == null
                    || viewMaterialPurchase.EquipmentGroupAdminList.Count() == 0
                    || viewMaterialPurchase.EquipmentGroupAdminList.Where(p => p.AdminId == userId.Value).Count() == 0)
                {
                    viewMaterialPurchase.GroupAdminAuditOperate = "";
                }
            }
            //申请人验收入库
            if (viewMaterialPurchase.ApplyUserId != userId.Value)
            {
                viewMaterialPurchase.InputorAuditPassOperate = "";
                viewMaterialPurchase.InputorAuditRejectOperate = "";
            }
            if (viewMaterialPurchase.StatusEnum == MaterialPurchaseStatus.InputPass)
            {
                viewMaterialPurchase.InputorAuditRejectOperate = "";
            }
            if (viewMaterialPurchase.StatusEnum == MaterialPurchaseStatus.BalancePass)
            {
                viewMaterialPurchase.BalanceAuditRejectOperate = "";
            }
            if (!materialPurchasePrivilige.IsEnableExport) viewMaterialPurchase.ExportOperate = "";
        }
        public IList<ViewMaterialPurchase> GetViewMaterialPurchaseListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialPurchaseList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialPurchaseList, isSupressLazyLoad);
            return viewMaterialPurchaseList;
        }
        public IList<ViewMaterialPurchase> GetViewMaterialPurchaseListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialPurchaseList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialPurchaseList, isSupressLazyLoad);
            return viewMaterialPurchaseList;
        }
        public IList<ViewMaterialPurchase> GetViewMaterialPurchaseListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialPurchaseList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialPurchaseList, isSupressLazyLoad);
            return viewMaterialPurchaseList;
        }
        public GridData<ViewMaterialPurchase> GetGridViewMaterialPurchaseListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialPurchaseList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialPurchaseList == null ? null : viewMaterialPurchaseList.Data, isSupressLazyLoad);
            return viewMaterialPurchaseList;
        }
        public GridData<ViewMaterialPurchase> GetGridViewMaterialPurchaseListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialPurchaseList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialPurchaseList == null ? null : viewMaterialPurchaseList.Data, isSupressLazyLoad);
            return viewMaterialPurchaseList;
        }
      
        public IList<ViewMaterialPurchase> GetManageViewMaterialPurchaseList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialPurchase(userId, ref dataGridSettings)) return null;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }

        public int GetManageViewMaterialPurchaseCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialPurchase(userId, ref dataGridSettings)) return 0;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }

        private bool IsManageMaterialPurchase(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = "";
                var materialInfoIds = __viewMaterialInfoBLL.GetManageMaterialInfoIds(userId, new DataGridSettings() { QueryExpression = "" });
                if (materialInfoIds != null && materialInfoIds.Count() > 0)
                {
                    var materialPurchaseItemList = __materialPurchaseItemBLL.GetEntitiesByExpression(string.Format("{0}*IsDelete=false", materialInfoIds.ToFormatStr("MaterialInfoId")), null, "", true);
                    if (materialPurchaseItemList != null && materialPurchaseItemList.Count() > 0)
                        queryExpression = materialPurchaseItemList.Select(p => p.MaterialPurchaseId).Distinct().ToFormatStr();
                    else return false;
                }
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }

        
    }
}
