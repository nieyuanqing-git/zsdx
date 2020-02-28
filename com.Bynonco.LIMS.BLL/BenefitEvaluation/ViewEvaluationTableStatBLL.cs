using System;
using System.Collections.Generic;
using System.Linq;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.BenefitEvaluation;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.Logic.Model;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary> 效益评价表视图业务层 </summary>
    public class ViewEvaluationTableStatBLL : BLLBase<ViewEvaluationTableStat>, IViewEvaluationTableStatBLL
    {
        public GridData<ViewEvaluationTableStat> GetViewList(Guid loginUserId, DataGridSettings dataGridSettings, bool isManageList, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipment(loginUserId, ref dataGridSettings)) return null;
            var viewList = GetGridEntitiesByExpression(dataGridSettings, null, string.Empty, true);
            if (isIniOperate) ExcuteOperateDecorate(loginUserId, viewList == null ? null : viewList.Data, true);
            return viewList;
        }

        public IList<ViewEvaluationTableStat> GetExportViewList(Guid loginUserId, DataGridSettings dataGridSettings, bool isManageList)
        {
            if (isManageList && !IsManageEquipment(loginUserId, ref dataGridSettings)) return null;
            var viewList = GetEntitiesByExpression(dataGridSettings, null, string.Empty, true);
            return viewList;
        }

        private void ExcuteOperateDecorate(Guid userId, IList<ViewEvaluationTableStat> viewList, bool isSupressLazyLoad)
        {
            if (viewList == null || viewList.Count == 0) return;
            var privilege = PriviligeFactory.GetEvaluationTablePrivilige(userId);
            foreach (var view in viewList)
            {
                view.IsSupressLazyLoad = false;
                view.InitOperate();
                OperateDecorate(userId, view, privilege);
                view.BuildOperate();
                view.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        public void OperateDecorate(Guid? userId, ViewEvaluationTableStat view, EvaluationTablePrivilige privilege)
        {
            if (view.AuditStatusEnum.HasValue)
            {
                view.TeachingOperate = string.Empty;
                view.ScientificResearchOperate = string.Empty;
                view.PublishServiceOperate = string.Empty;
                view.EquipmentDevelopmentOperate = string.Empty;
                if (view.AuditStatusEnum.Value != EvaluationAuditStatus.Draft) view.ModifyOperate = string.Empty;
                if (view.AuditStatusEnum.Value != EvaluationAuditStatus.Submit) view.AuditOperate = string.Empty;

                if (!privilege.IsEnableView) view.ViewOperate = string.Empty;
                if (!privilege.IsEnableAdd)
                {
                    view.TeachingOperate = string.Empty;
                    view.ScientificResearchOperate = string.Empty;
                    view.PublishServiceOperate = string.Empty;
                    view.EquipmentDevelopmentOperate = string.Empty;
                    view.ModifyOperate = string.Empty;
                }
                if (!privilege.IsEnableAudit) view.AuditOperate = string.Empty;
                if (!privilege.IsEnableDownLoad) view.DownLoadOperate = string.Empty;
            }
            else
            {
                view.ViewOperate = string.Empty;
                view.ModifyOperate = string.Empty;
                view.AuditOperate = string.Empty;
                view.DownLoadOperate = string.Empty;
            }
        }
        private bool IsManageEquipment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = BLLFactory.CreateUserBLL().GetEntityById(userId.Value);
            if (user == null) return false;
            var queryExpression = "";
            if (!user.IsSuperAdmin || CustomerFactory.GetCustomer().GetIsSuperAdminOnlyShowAdminEqList())
            {
                var userWorkScopeList = BLLFactory.CreateUserWorkScopeBLL().GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id));
                if (userWorkScopeList != null && userWorkScopeList.Any())
                    queryExpression = userWorkScopeList.Select(p => p.EquipmentId).ToFormatStr();
            }
            if (!user.IsSuperAdmin)
            {
                var queryOrgId = BLLFactory.CreateWorkGroupModuleBLL().GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.Equipment, "", "", "EquipmentOrganizationId"), null);
                if ((queryOrgId == "Id=null" || queryOrgId == "") && queryExpression == "") queryExpression = "Id=null";
                else if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression += (queryExpression == "" ? "" : "+") + queryOrgId;
            }
            if (!string.IsNullOrWhiteSpace(queryExpression)) dataGridSettings.AppendAndQueryExpression(queryExpression);
            return true;
        }

    }
}