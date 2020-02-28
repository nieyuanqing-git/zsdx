using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewEquipmentTrainningBLL : BLLBase<ViewEquipmentTrainning>, IViewEquipmentTrainningBLL
    {
        private IEquipmentTranningBLL __equipmentTrainningBLL;
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewEquipmentTrainningBLL()
        {
            __equipmentTrainningBLL = BLLFactory.CreateEquipmentTranningBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentTrainning> viewEquipmentTrainningList, bool isSupressLazyLoad)
        {
            if (viewEquipmentTrainningList == null || viewEquipmentTrainningList.Count == 0) return;
            IList<EquipmentTrainningPrivilige> lstEquipmentTrainningPriviliges = (IList<EquipmentTrainningPrivilige>)__equipmentTrainningBLL.GetEquipmentTrainningPriviliges(userId, viewEquipmentTrainningList);
            foreach (var viewEquipmentTrainning in viewEquipmentTrainningList)
            {
                EquipmentTrainningPrivilige equipmentTrainningPrivilige = lstEquipmentTrainningPriviliges.FirstOrDefault(p => p.EquipmentTrainningId.HasValue && p.EquipmentTrainningId.Value == viewEquipmentTrainning.Id);
                viewEquipmentTrainning.IsSupressLazyLoad = false;
                viewEquipmentTrainning.InitOperate();
                OperateDecorate(userId, viewEquipmentTrainning, equipmentTrainningPrivilige);
                viewEquipmentTrainning.BuildOperate();
                viewEquipmentTrainning.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewEquipmentTrainning viewEquipmentTrainning, EquipmentTrainningPrivilige equipmentTrainningPrivilige)
        {
            if (viewEquipmentTrainning == null) throw new ArgumentException("设备信息为空");
            if (equipmentTrainningPrivilige == null)
            {
                viewEquipmentTrainning.ModifyOperate = "";
                viewEquipmentTrainning.DeleteOperate = "";
                return;
            }
            if (!equipmentTrainningPrivilige.IsEnableEdit) viewEquipmentTrainning.ModifyOperate = "";
            if (!equipmentTrainningPrivilige.IsEnableDelete) viewEquipmentTrainning.DeleteOperate = "";
        }
        public IList<ViewEquipmentTrainning> GetViewEquipmentTrainningListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipmentTrainning(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewEquipmentTrainningList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if(isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentTrainningList, isSupressLazyLoad);
            return viewEquipmentTrainningList;
        }
        public IList<ViewEquipmentTrainning> GetViewEquipmentTrainningListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipmentTrainning(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewEquipmentTrainningList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentTrainningList, isSupressLazyLoad);
            return viewEquipmentTrainningList;
        }
        public IList<ViewEquipmentTrainning> GetViewEquipmentTrainningListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipmentTrainning(userId, ref dataGridSettings)) return null;
            var viewEquipmentTrainningList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentTrainningList, isSupressLazyLoad);
            return viewEquipmentTrainningList;
        }
        public GridData<ViewEquipmentTrainning> GetGridViewEquipmentTrainningListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipmentTrainning(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewEquipmentTrainningList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentTrainningList == null ? null : viewEquipmentTrainningList.Data, isSupressLazyLoad);
            return viewEquipmentTrainningList;
        }
        public GridData<ViewEquipmentTrainning> GetGridViewEquipmentTrainningListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipmentTrainning(userId, ref dataGridSettings)) return null;
            var viewEquipmentTrainningList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentTrainningList == null ? null : viewEquipmentTrainningList.Data, isSupressLazyLoad);
            return viewEquipmentTrainningList;
        }
        public int GetGridViewEquipmentTrainningCountByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GetGridViewEquipmentTrainningCountByExpression(userId, dataGridSettings, mapping, isDistinct, outDistinctMapping, isManageList);
        }
        public int GetGridViewEquipmentTrainningCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            if (isManageList && !IsManageEquipmentTrainning(userId, ref dataGridSettings)) return 0;
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }
        private bool IsManageEquipmentTrainning(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.EquipmentTrainning, "", "", "CreatorOrgId"), new ManagerEquipmentOrgId(ModuleBusinessType.EquipmentTrainning, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
