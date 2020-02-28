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

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewEquipmentOpenTrainingBLL : BLLBase<ViewEquipmentOpenTraining>, IViewEquipmentOpenTrainingBLL
    {
        private IUserBLL __userBLL;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewEquipmentOpenTrainingBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentOpenTraining> viewEquipmentOpenTrainingList, bool isSupressLazyLoad = false)
        {
            if (viewEquipmentOpenTrainingList == null || viewEquipmentOpenTrainingList.Count == 0) return;
            foreach (var viewEquipmentOpenTraining in viewEquipmentOpenTrainingList)
            {
                viewEquipmentOpenTraining.IsSupressLazyLoad = false;
                viewEquipmentOpenTraining.InitOperate();
                OperateDecorate(userId, viewEquipmentOpenTraining);
                viewEquipmentOpenTraining.BuildOperate();
                viewEquipmentOpenTraining.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewEquipmentOpenTraining viewEquipmentOpenTraining)
        {
            if (viewEquipmentOpenTraining == null) throw new ArgumentException("设备应用知识培训申请信息为空");
            var equipmentPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentPrivilige(userId.Value, viewEquipmentOpenTraining.EquipmentId) : null;
            var equipmentOpenTrainingPrivilige = userId.HasValue ?  PriviligeFactory.GetEquipmentOpenTrainingPrivilige(userId.Value) : null;
            if (equipmentOpenTrainingPrivilige == null)
            {
                viewEquipmentOpenTraining.ViewOperate = "";
                viewEquipmentOpenTraining.ModifyOperate = "";
                viewEquipmentOpenTraining.CollegeAuditOperate = "";
                viewEquipmentOpenTraining.ManageAuditOperate = "";
                viewEquipmentOpenTraining.DirectorAuditOperate = "";
                viewEquipmentOpenTraining.DeleteOperate = "";
                return;
            }
            if (!equipmentOpenTrainingPrivilige.IsEnableView) viewEquipmentOpenTraining.ViewOperate = "";
            if (!equipmentOpenTrainingPrivilige.IsEnableEdit) viewEquipmentOpenTraining.ModifyOperate = "";
            if (!equipmentOpenTrainingPrivilige.IsEnableDelete) viewEquipmentOpenTraining.DeleteOperate = "";
            if (!equipmentOpenTrainingPrivilige.IsEnableCollegeAudit) viewEquipmentOpenTraining.CollegeAuditOperate = "";
            if (!equipmentOpenTrainingPrivilige.IsEnableManageAudit) viewEquipmentOpenTraining.ManageAuditOperate = "";
            if (!equipmentOpenTrainingPrivilige.IsEnableDirectorAudit) viewEquipmentOpenTraining.DirectorAuditOperate = "";
            if (!equipmentOpenTrainingPrivilige.IsEnableExportWord) viewEquipmentOpenTraining.ExportWord = "";
            bool isAdminEditPower = true;
            bool isAdminDeletePower = true;
            if (!equipmentOpenTrainingPrivilige.IsEnableManageAudit && !equipmentOpenTrainingPrivilige.IsEnableDirectorAudit && !equipmentPrivilige.IsEnableEdit)
                isAdminEditPower = false;
            if (!equipmentOpenTrainingPrivilige.IsEnableManageAudit && !equipmentOpenTrainingPrivilige.IsEnableDirectorAudit && !equipmentPrivilige.IsEnableDelete)
                isAdminDeletePower = false;

            //if (!equipmentOpenTrainingPrivilige.IsEnableEdit || (!equipmentOpenTrainingPrivilige.IsEnableManageAudit && !equipmentOpenTrainingPrivilige.IsEnableDirectorAudit && !equipmentPrivilige.IsEnableEdit)) viewEquipmentOpenTraining.ModifyOperate = "";

            //switch (viewEquipmentOpenTraining.StatusEnum)
            //{
            //    case EquipmentOpenTrainingStatus.WaitingAudit:
            //        if (!isAdminEditPower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.ModifyOperate = "";
            //        if (!isAdminDeletePower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.DeleteOperate = "";
            //        viewEquipmentOpenTraining.ManageAuditOperate = "";
            //        viewEquipmentOpenTraining.DirectorAuditOperate = "";
            //        break;
            //    case EquipmentOpenTrainingStatus.CollegeAuditPass:
            //        viewEquipmentOpenTraining.DirectorAuditOperate = "";
            //        if (!isAdminEditPower) viewEquipmentOpenTraining.ModifyOperate = "";
            //        if (!isAdminDeletePower) viewEquipmentOpenTraining.DeleteOperate = "";
            //        break;
            //    case EquipmentOpenTrainingStatus.CollegeAuditReject:
            //        viewEquipmentOpenTraining.ManageAuditOperate = "";
            //        viewEquipmentOpenTraining.DirectorAuditOperate = "";
            //        if (!isAdminEditPower) viewEquipmentOpenTraining.ModifyOperate = "";
            //        if (!isAdminDeletePower) viewEquipmentOpenTraining.DeleteOperate = "";
            //        break;
            //    case EquipmentOpenTrainingStatus.ManageAuditPass:
            //        viewEquipmentOpenTraining.CollegeAuditOperate = "";
            //        if (!equipmentOpenTrainingPrivilige.IsEnableManageAudit && !equipmentOpenTrainingPrivilige.IsEnableDirectorAudit)
            //        {
            //            viewEquipmentOpenTraining.ModifyOperate = "";
            //            viewEquipmentOpenTraining.DeleteOperate = "";
            //        }
            //        break;
            //    case EquipmentOpenTrainingStatus.ManageAuditReject:
            //        viewEquipmentOpenTraining.CollegeAuditOperate = "";
            //        viewEquipmentOpenTraining.DirectorAuditOperate = "";
            //        if (!equipmentOpenTrainingPrivilige.IsEnableManageAudit)
            //        {
            //            viewEquipmentOpenTraining.ModifyOperate = "";
            //            viewEquipmentOpenTraining.DeleteOperate = "";
            //        }
            //        break;
            //    case EquipmentOpenTrainingStatus.DirectorAuditPass:
            //    case EquipmentOpenTrainingStatus.DirectorAuditReject:
            //        viewEquipmentOpenTraining.CollegeAuditOperate = "";
            //        viewEquipmentOpenTraining.ManageAuditOperate = "";
            //        if (!equipmentOpenTrainingPrivilige.IsEnableDirectorAudit)
            //        {
            //            viewEquipmentOpenTraining.ModifyOperate = "";
            //            viewEquipmentOpenTraining.DeleteOperate = "";
            //        }
            //        break;
            //}
 
            switch (viewEquipmentOpenTraining.StatusEnum)
            {
                case EquipmentOpenTrainingStatus.WaitingAudit:
                    if (!isAdminEditPower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.ModifyOperate = "";
                    if (!isAdminDeletePower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.DeleteOperate = "";
                    viewEquipmentOpenTraining.ManageAuditOperate = "";
                    viewEquipmentOpenTraining.DirectorAuditOperate = "";
                    break;
                case EquipmentOpenTrainingStatus.CollegeAuditPass:
                    viewEquipmentOpenTraining.DirectorAuditOperate = "";
                    viewEquipmentOpenTraining.ModifyOperate = "";
                    viewEquipmentOpenTraining.DeleteOperate = "";
                    break;
                case EquipmentOpenTrainingStatus.CollegeAuditReject:
                    viewEquipmentOpenTraining.ManageAuditOperate = "";
                    viewEquipmentOpenTraining.DirectorAuditOperate = "";
                    if (!isAdminEditPower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.ModifyOperate = "";
                    if (!isAdminDeletePower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.DeleteOperate = "";
                    break;
                case EquipmentOpenTrainingStatus.ManageAuditPass:
                    viewEquipmentOpenTraining.CollegeAuditOperate = "";
                    viewEquipmentOpenTraining.ModifyOperate = "";
                    viewEquipmentOpenTraining.DeleteOperate = "";
                    break;
                case EquipmentOpenTrainingStatus.ManageAuditReject:
                    viewEquipmentOpenTraining.CollegeAuditOperate = "";
                    viewEquipmentOpenTraining.DirectorAuditOperate = "";
                    if (!isAdminEditPower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.ModifyOperate = "";
                    if (!isAdminDeletePower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.DeleteOperate = "";

                    break;
                case EquipmentOpenTrainingStatus.DirectorAuditPass:
                    viewEquipmentOpenTraining.CollegeAuditOperate = "";
                    viewEquipmentOpenTraining.ManageAuditOperate = "";
                    viewEquipmentOpenTraining.ModifyOperate = "";
                    viewEquipmentOpenTraining.DeleteOperate = "";
                    break;
                case EquipmentOpenTrainingStatus.DirectorAuditReject:
                    viewEquipmentOpenTraining.CollegeAuditOperate = "";
                    viewEquipmentOpenTraining.ManageAuditOperate = "";
                    if (!isAdminEditPower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.ModifyOperate = "";
                    if (!isAdminDeletePower && viewEquipmentOpenTraining.CreaterId != userId.Value) viewEquipmentOpenTraining.DeleteOperate = "";
                    break;
            }
        }
        public IList<ViewEquipmentOpenTraining> GetViewEquipmentOpenTrainingListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingList = GetEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingList, isSupressLazyLoad);
            return viewEquipmentOpenTrainingList;
        }
        public IList<ViewEquipmentOpenTraining> GetViewEquipmentOpenTrainingListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingList = GetEntitiesByExpression(dataGridSettings.QueryExpression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingList, isSupressLazyLoad);
            return viewEquipmentOpenTrainingList;
        }
        public IList<ViewEquipmentOpenTraining> GetViewEquipmentOpenTrainingListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingList, isSupressLazyLoad);
            return viewEquipmentOpenTrainingList;
        }
        public GridData<ViewEquipmentOpenTraining> GetGridViewEquipmentOpenTrainingListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingList = GetGridEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingList == null ? null : viewEquipmentOpenTrainingList.Data, isSupressLazyLoad);
            return viewEquipmentOpenTrainingList;
        }
        public GridData<ViewEquipmentOpenTraining> GetGridViewEquipmentOpenTrainingListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenTrainingList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenTrainingList == null ? null : viewEquipmentOpenTrainingList.Data, isSupressLazyLoad);
            return viewEquipmentOpenTrainingList;
        }
        private bool IsManageEquipment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            var equipmentOpenTrainingPrivilige = PriviligeFactory.GetEquipmentOpenTrainingPrivilige(userId.Value);
            if (!user.IsSuperAdmin && (equipmentOpenTrainingPrivilige == null || !equipmentOpenTrainingPrivilige.IsEnableManageAudit || !equipmentOpenTrainingPrivilige.IsEnableDirectorAudit))
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
