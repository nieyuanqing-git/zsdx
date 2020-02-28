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
    public class ViewEquipmentUndergraduateOpenBLL : BLLBase<ViewEquipmentUndergraduateOpen>, IViewEquipmentUndergraduateOpenBLL
    {
        private IUserBLL __userBLL;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IChargeStandardBLL __chargeStandardBLL;
        public ViewEquipmentUndergraduateOpenBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __chargeStandardBLL = BLLFactory.CreateChargeStandardBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentUndergraduateOpen> viewEquipmentUndergraduateOpenList, bool isSupressLazyLoad = false)
        {
            if (viewEquipmentUndergraduateOpenList == null || viewEquipmentUndergraduateOpenList.Count == 0) return;
            foreach (var viewEquipmentUndergraduateOpen in viewEquipmentUndergraduateOpenList)
            {
                viewEquipmentUndergraduateOpen.IsSupressLazyLoad = false;
                viewEquipmentUndergraduateOpen.InitOperate();
                OperateDecorate(userId, viewEquipmentUndergraduateOpen);
                viewEquipmentUndergraduateOpen.BuildOperate();
                viewEquipmentUndergraduateOpen.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewEquipmentUndergraduateOpen viewEquipmentUndergraduateOpen)
        {
            if (viewEquipmentUndergraduateOpen == null) throw new ArgumentException("设备开放信息为空");
            var equipmentPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentPrivilige(userId.Value, viewEquipmentUndergraduateOpen.EquipmentId) : null;
            var equipmentUndergraduateOpenPrivilige = userId.HasValue ?  PriviligeFactory.GetEquipmentUndergraduateOpenPrivilige(userId.Value) : null;
            if (equipmentUndergraduateOpenPrivilige == null)
            {
                viewEquipmentUndergraduateOpen.ViewOperate = "";
                viewEquipmentUndergraduateOpen.ModifyOperate = "";
                viewEquipmentUndergraduateOpen.AuditOperate = "";
                viewEquipmentUndergraduateOpen.DeleteOperate = "";
                return;
            }
            if (!equipmentUndergraduateOpenPrivilige.IsEnableView) viewEquipmentUndergraduateOpen.ViewOperate = "";
            if (!equipmentUndergraduateOpenPrivilige.IsEnableAudit) viewEquipmentUndergraduateOpen.AuditOperate = "";
            if (!equipmentUndergraduateOpenPrivilige.IsEnableEdit || (!equipmentUndergraduateOpenPrivilige.IsEnableAudit && !equipmentPrivilige.IsEnableEdit)) viewEquipmentUndergraduateOpen.ModifyOperate = "";
            if (!equipmentUndergraduateOpenPrivilige.IsEnableDelete || (!equipmentUndergraduateOpenPrivilige.IsEnableAudit && !equipmentPrivilige.IsEnableDelete)) viewEquipmentUndergraduateOpen.DeleteOperate = "";
            if (viewEquipmentUndergraduateOpen.StatusEnum == EquipmentUndergraduateOpenStatus.AuditPass && !equipmentUndergraduateOpenPrivilige.IsEnableAudit)
            {
                viewEquipmentUndergraduateOpen.ModifyOperate = "";
                viewEquipmentUndergraduateOpen.DeleteOperate = "";
            }
        }
        public IList<ViewEquipmentUndergraduateOpen> GetViewEquipmentUndergraduateOpenListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentUndergraduateOpenList = GetEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentUndergraduateOpenList, isSupressLazyLoad);
            return viewEquipmentUndergraduateOpenList;
        }
        public IList<ViewEquipmentUndergraduateOpen> GetViewEquipmentUndergraduateOpenListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentUndergraduateOpenList = GetEntitiesByExpression(dataGridSettings.QueryExpression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentUndergraduateOpenList, isSupressLazyLoad);
            return viewEquipmentUndergraduateOpenList;
        }
        public IList<ViewEquipmentUndergraduateOpen> GetViewEquipmentUndergraduateOpenListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentUndergraduateOpenList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentUndergraduateOpenList, isSupressLazyLoad);
            return viewEquipmentUndergraduateOpenList;
        }
        public GridData<ViewEquipmentUndergraduateOpen> GetGridViewEquipmentUndergraduateOpenListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentUndergraduateOpenList = GetGridEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentUndergraduateOpenList == null ? null : viewEquipmentUndergraduateOpenList.Data, isSupressLazyLoad);
            return viewEquipmentUndergraduateOpenList;
        }
        public GridData<ViewEquipmentUndergraduateOpen> GetGridViewEquipmentUndergraduateOpenListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentUndergraduateOpenList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentUndergraduateOpenList == null ? null : viewEquipmentUndergraduateOpenList.Data, isSupressLazyLoad);
            return viewEquipmentUndergraduateOpenList;
        }
        private bool IsManageEquipment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            var equipmentUndergraduateOpenPrivilige = PriviligeFactory.GetEquipmentUndergraduateOpenPrivilige(userId.Value);
            if (!user.IsSuperAdmin && (equipmentUndergraduateOpenPrivilige == null || !equipmentUndergraduateOpenPrivilige.IsEnableAudit))
            {
                var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id));
                var queryExpression = "";
                if (userWorkScopeList != null && userWorkScopeList.Count() > 0)
                    queryExpression = userWorkScopeList.Select(p => p.EquipmentId).ToFormatStr("EquipmentId");

                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.Equipment, "", "", "EquipmentOrgId"), null);
                if ((queryOrgId == "Id=null" || queryOrgId == "") && queryExpression == "") queryExpression = "Id=null";
                else if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression += (queryExpression == "" ? "" : "+") + queryOrgId;
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
        public void FillEquipmentUndergraduateOpenStandardPrice(IList<ViewEquipmentUndergraduateOpen> viewEquipmentUndergraduateOpenList, Guid? userId, Guid? subjectId)
        {
            if (viewEquipmentUndergraduateOpenList != null && viewEquipmentUndergraduateOpenList.Count() > 0)
            {
                double discount;
                foreach (var item in viewEquipmentUndergraduateOpenList)
                {
                    var chargeStandard = __chargeStandardBLL.GetEquipmentChargeStandardByUserId(item.EquipmentId, userId,subjectId, out discount);
                    if (chargeStandard != null)
                    {
                        item.StandardPrice = chargeStandard.StandardPrice;
                        item.ChargeTypeEnum = chargeStandard.ChargeTypeEnum;
                    }
                }
            }
        }
    }
}
