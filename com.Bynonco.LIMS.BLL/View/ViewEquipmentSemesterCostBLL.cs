using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewEquipmentSemesterCostBLL : BLLBase<ViewEquipmentSemesterCost>, IViewEquipmentSemesterCostBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewEquipmentSemesterCostBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentSemesterCost> viewEquipmentSemesterCostList, bool isSupressLazyLoad)
        {
            if (!userId.HasValue || viewEquipmentSemesterCostList == null || viewEquipmentSemesterCostList.Count == 0) return;
            var equipmentSemesterCostPrivilige = PriviligeFactory.GetEquipmentSemesterCostPrivilige(userId.Value);
            foreach (var viewEquipmentSemesterCost in viewEquipmentSemesterCostList)
            {
                viewEquipmentSemesterCost.IsSupressLazyLoad = false;
                viewEquipmentSemesterCost.InitOperate();
                OperateDecorate(userId, viewEquipmentSemesterCost, equipmentSemesterCostPrivilige);
                viewEquipmentSemesterCost.BuildOperate();
                viewEquipmentSemesterCost.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewEquipmentSemesterCost viewEquipmentSemesterCost, EquipmentSemesterCostPrivilige equipmentSemesterCostPrivilige)
        {
            string errorMessage = "";
            if (viewEquipmentSemesterCost == null) throw new ArgumentException("仪器学期费用信息为空");
            if (equipmentSemesterCostPrivilige == null)
            {
                viewEquipmentSemesterCost.ViewOperate = "";
                viewEquipmentSemesterCost.EditOperate = "";
                viewEquipmentSemesterCost.AuditOperate = "";
                viewEquipmentSemesterCost.DeleteOperate = "";
                return;
            }
            if (!equipmentSemesterCostPrivilige.IsEnableView) viewEquipmentSemesterCost.ViewOperate = "";
            if (!equipmentSemesterCostPrivilige.IsEnableEdit) viewEquipmentSemesterCost.EditOperate = "";
            if (!equipmentSemesterCostPrivilige.IsEnableAudit) viewEquipmentSemesterCost.AuditOperate = "";
            if (!equipmentSemesterCostPrivilige.IsEnableDelete) viewEquipmentSemesterCost.DeleteOperate = "";
            if (viewEquipmentSemesterCost.StatusEnum != EquipmentSemesterCostStatus.WaittingAudit)
            {
                viewEquipmentSemesterCost.EditOperate = "";
                viewEquipmentSemesterCost.DeleteOperate = "";
            }
            if(viewEquipmentSemesterCost.EquipmentGroupAdminIdList == null 
                || viewEquipmentSemesterCost.EquipmentGroupAdminIdList.Count() == 0
                || !viewEquipmentSemesterCost.EquipmentGroupAdminIdList.Contains(userId.Value)
                )
                viewEquipmentSemesterCost.AuditOperate = "";
        }

        public IList<ViewEquipmentSemesterCost> GetViewEquipmentSemesterCostListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipmentSemesterCost(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewEquipmentSemesterCostList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentSemesterCostList, isSupressLazyLoad);
            return viewEquipmentSemesterCostList;
        }
        public IList<ViewEquipmentSemesterCost> GetViewEquipmentSemesterCostListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipmentSemesterCost(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewEquipmentSemesterCostList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentSemesterCostList, isSupressLazyLoad);
            return viewEquipmentSemesterCostList;
        }
        public IList<ViewEquipmentSemesterCost> GetViewEquipmentSemesterCostListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            if (isManageList && !IsManageEquipmentSemesterCost(userId, ref dataGridSettings)) return null;
            var viewEquipmentSemesterCostList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentSemesterCostList, isSupressLazyLoad);
            return viewEquipmentSemesterCostList;
        }
        public GridData<ViewEquipmentSemesterCost> GetGridViewEquipmentSemesterCostListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipmentSemesterCost(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewEquipmentSemesterCostList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentSemesterCostList == null ? null : viewEquipmentSemesterCostList.Data, isSupressLazyLoad);
            return viewEquipmentSemesterCostList;
        }
        public GridData<ViewEquipmentSemesterCost> GetGridViewEquipmentSemesterCostListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            if (isManageList && !IsManageEquipmentSemesterCost(userId, ref dataGridSettings)) return null;
            var viewEquipmentSemesterCostList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentSemesterCostList == null ? null : viewEquipmentSemesterCostList.Data, isSupressLazyLoad);
            return viewEquipmentSemesterCostList;
        }
        private bool IsManageEquipmentSemesterCost(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
                dataGridSettings.AppendAndQueryExpression(__workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.EquipmentSemesterCost), null));
            return true;
        }
    }
}
