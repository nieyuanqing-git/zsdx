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
    public class ViewEquipmentOpenTrainingCarryOutSignUpBLL : BLLBase<ViewEquipmentOpenTrainingCarryOutSignUp>, IViewEquipmentOpenTrainingCarryOutSignUpBLL
    {
        private IUserBLL __userBLL;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewEquipmentOpenTrainingCarryOutSignUpBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentOpenTrainingCarryOutSignUp> viewEquipmentOpenTrainingCarryOutSignUpList, bool isSupressLazyLoad = false)
        {
            if (viewEquipmentOpenTrainingCarryOutSignUpList == null || viewEquipmentOpenTrainingCarryOutSignUpList.Count == 0) return;
            foreach (var viewEquipmentOpenTrainingCarryOutSignUp in viewEquipmentOpenTrainingCarryOutSignUpList)
            {
                viewEquipmentOpenTrainingCarryOutSignUp.IsSupressLazyLoad = false;
                viewEquipmentOpenTrainingCarryOutSignUp.InitOperate();
                OperateDecorate(userId, viewEquipmentOpenTrainingCarryOutSignUp);
                viewEquipmentOpenTrainingCarryOutSignUp.BuildOperate();
                viewEquipmentOpenTrainingCarryOutSignUp.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewEquipmentOpenTrainingCarryOutSignUp viewEquipmentOpenTrainingCarryOutSignUp)
        {
            if (viewEquipmentOpenTrainingCarryOutSignUp == null) throw new ArgumentException("设备培训报名为空");
            var equipmentPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentPrivilige(userId.Value, viewEquipmentOpenTrainingCarryOutSignUp.EquipmentId) : null;
            var equipmentOpenTrainingCarryOutSignUpPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentOpenTrainingCarryOutSignUpPrivilige(userId.Value) : null;
            if (equipmentOpenTrainingCarryOutSignUpPrivilige == null)
            {
                viewEquipmentOpenTrainingCarryOutSignUp.ViewOperate = "";
                viewEquipmentOpenTrainingCarryOutSignUp.AuditOperate = "";
                viewEquipmentOpenTrainingCarryOutSignUp.ResultOperate = "";
                viewEquipmentOpenTrainingCarryOutSignUp.DeleteOperate = "";
                return;
            }
            if (!equipmentOpenTrainingCarryOutSignUpPrivilige.IsEnableView) viewEquipmentOpenTrainingCarryOutSignUp.ViewOperate = "";
            if (!equipmentOpenTrainingCarryOutSignUpPrivilige.IsEnableAudit || !equipmentPrivilige.IsEnableEdit) viewEquipmentOpenTrainingCarryOutSignUp.AuditOperate = "";
            if (!equipmentOpenTrainingCarryOutSignUpPrivilige.IsEnableResult || !equipmentPrivilige.IsEnableEdit) viewEquipmentOpenTrainingCarryOutSignUp.ResultOperate = "";
            if (!equipmentOpenTrainingCarryOutSignUpPrivilige.IsEnableDelete || !equipmentPrivilige.IsEnableEdit) viewEquipmentOpenTrainingCarryOutSignUp.DeleteOperate = "";
            if (viewEquipmentOpenTrainingCarryOutSignUp.StatusEnum == EquipmentOpenTrainingCarryOutSignUpStatus.Complete) viewEquipmentOpenTrainingCarryOutSignUp.AuditOperate = "";
            if (viewEquipmentOpenTrainingCarryOutSignUp.StatusEnum != EquipmentOpenTrainingCarryOutSignUpStatus.AuditPass && viewEquipmentOpenTrainingCarryOutSignUp.StatusEnum != EquipmentOpenTrainingCarryOutSignUpStatus.Complete) viewEquipmentOpenTrainingCarryOutSignUp.ResultOperate = "";
        }
        public IList<ViewEquipmentOpenTrainingCarryOutSignUp> GetViewEquipmentOpenTrainingCarryOutSignUpListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutSignUpList = GetEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutSignUpList, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutSignUpList;
        }
        public IList<ViewEquipmentOpenTrainingCarryOutSignUp> GetViewEquipmentOpenTrainingCarryOutSignUpListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutSignUpList = GetEntitiesByExpression(dataGridSettings.QueryExpression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutSignUpList, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutSignUpList;
        }
        public IList<ViewEquipmentOpenTrainingCarryOutSignUp> GetViewEquipmentOpenTrainingCarryOutSignUpListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutSignUpList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutSignUpList, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutSignUpList;
        }
        public GridData<ViewEquipmentOpenTrainingCarryOutSignUp> GetGridViewEquipmentOpenTrainingCarryOutSignUpListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutSignUpList = GetGridEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutSignUpList == null ? null : viewEquipmentOpenTrainingCarryOutSignUpList.Data, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutSignUpList;
        }
        public GridData<ViewEquipmentOpenTrainingCarryOutSignUp> GetGridViewEquipmentOpenTrainingCarryOutSignUpListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingCarryOutSignUpList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingCarryOutSignUpList == null ? null : viewEquipmentOpenTrainingCarryOutSignUpList.Data, isSupressLazyLoad);
            return viewEquipmentOpenTrainingCarryOutSignUpList;
        }
        private bool IsManageEquipment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
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
    }
}
