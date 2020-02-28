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
using com.Bynonco.LIMS.Utility;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewEquipmentOpenTrainingCarryOutBLL : BLLBase<ViewEquipmentOpenTrainingCarryOut>, IViewEquipmentOpenTrainingCarryOutBLL
    {
        private IUserBLL __userBLL;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewEquipmentOpenTrainingCarryOutBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentOpenTrainingCarryOut> viewEquipmentOpenTrainingCarryOutList, bool isSupressLazyLoad = false)
        {
            if (viewEquipmentOpenTrainingCarryOutList == null || viewEquipmentOpenTrainingCarryOutList.Count == 0) return;
            foreach (var viewEquipmentOpenTrainingCarryOut in viewEquipmentOpenTrainingCarryOutList)
            {
                viewEquipmentOpenTrainingCarryOut.IsSupressLazyLoad = false;
                viewEquipmentOpenTrainingCarryOut.InitOperate();
                OperateDecorate(userId, viewEquipmentOpenTrainingCarryOut);
                viewEquipmentOpenTrainingCarryOut.BuildOperate();
                viewEquipmentOpenTrainingCarryOut.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewEquipmentOpenTrainingCarryOut viewEquipmentOpenTrainingCarryOut)
        {
            if (viewEquipmentOpenTrainingCarryOut == null) throw new ArgumentException("设备培训计划为空");
            var equipmentPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentPrivilige(userId.Value, viewEquipmentOpenTrainingCarryOut.EquipmentId) : null;
            var equipmentOpenTrainingCarryOutPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentOpenTrainingCarryOutPrivilige(userId.Value) : null;
            if (equipmentOpenTrainingCarryOutPrivilige == null)
            {
                viewEquipmentOpenTrainingCarryOut.ViewOperate = "";
                viewEquipmentOpenTrainingCarryOut.ModifyOperate = "";
                viewEquipmentOpenTrainingCarryOut.AuditOperate = "";
                viewEquipmentOpenTrainingCarryOut.ExportWordOperate = "";
                viewEquipmentOpenTrainingCarryOut.DeleteOperate = "";
                return;
            }
            if (!equipmentOpenTrainingCarryOutPrivilige.IsEnableView) viewEquipmentOpenTrainingCarryOut.ViewOperate = "";
            if (!equipmentOpenTrainingCarryOutPrivilige.IsEnableAudit) viewEquipmentOpenTrainingCarryOut.AuditOperate = "";
            if (!equipmentOpenTrainingCarryOutPrivilige.IsEnableEdit || (!equipmentOpenTrainingCarryOutPrivilige.IsEnableAudit && !equipmentPrivilige.IsEnableEdit)) viewEquipmentOpenTrainingCarryOut.ModifyOperate = "";
            if (!equipmentOpenTrainingCarryOutPrivilige.IsEnableExportWord) viewEquipmentOpenTrainingCarryOut.ExportWordOperate = "";
            if (!equipmentOpenTrainingCarryOutPrivilige.IsEnableDelete || (!equipmentOpenTrainingCarryOutPrivilige.IsEnableAudit && !equipmentPrivilige.IsEnableDelete)) viewEquipmentOpenTrainingCarryOut.DeleteOperate = "";
            if (viewEquipmentOpenTrainingCarryOut.StatusEnum == EquipmentOpenTrainingCarryOutStatus.AuditPass && !equipmentOpenTrainingCarryOutPrivilige.IsEnableAudit)
            {
                viewEquipmentOpenTrainingCarryOut.ModifyOperate = "";
                viewEquipmentOpenTrainingCarryOut.DeleteOperate = "";
            }
        }
        public IList<ViewEquipmentOpenTrainingCarryOut> GetViewEquipmentOpenTrainingCarryOutListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutList = GetEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutList, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutList;
        }
        public IList<ViewEquipmentOpenTrainingCarryOut> GetViewEquipmentOpenTrainingCarryOutListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutList = GetEntitiesByExpression(dataGridSettings.QueryExpression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutList, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutList;
        }
        public IList<ViewEquipmentOpenTrainingCarryOut> GetViewEquipmentOpenTrainingCarryOutListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutList, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutList;
        }
        public GridData<ViewEquipmentOpenTrainingCarryOut> GetGridViewEquipmentOpenTrainingCarryOutListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutList = GetGridEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutList == null ? null : viewEquipmentOpenTrainingCarryOutList.Data, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutList;
        }
        public GridData<ViewEquipmentOpenTrainingCarryOut> GetGridViewEquipmentOpenTrainingCarryOutListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutList == null ? null : viewEquipmentOpenTrainingCarryOutList.Data, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutList;
        }
        private bool IsManageEquipment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            var equipmentOpenTrainingCarryOutPrivilige = PriviligeFactory.GetEquipmentOpenTrainingCarryOutPrivilige(userId.Value);
            if (!user.IsSuperAdmin && (equipmentOpenTrainingCarryOutPrivilige == null || !equipmentOpenTrainingCarryOutPrivilige.IsEnableAudit))
            {
                var queryExpression = string.Format("CreaterId=\"{0}\"",userId.Value);
                //var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id));
                //var queryExpression = "";
                //if (userWorkScopeList != null && userWorkScopeList.Count() > 0)
                //    queryExpression = userWorkScopeList.Select(p => p.EquipmentId).ToFormatStr("EquipmentId");

                //var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.Equipment, "", "", "EquipmentOrgId"), null);
                //if ((queryOrgId == "Id=null" || queryOrgId == "") && queryExpression == "") queryExpression = "Id=null";
                //else if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression += (queryExpression == "" ? "" : "+") + queryOrgId;
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
