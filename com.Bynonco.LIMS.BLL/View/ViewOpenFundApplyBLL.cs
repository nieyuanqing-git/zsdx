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
    public class ViewOpenFundApplyBLL : BLLBase<ViewOpenFundApply>, IViewOpenFundApplyBLL
    {
        private IUserBLL __userBLL;
        private IViewOpenFundApplyEquipmentBLL __viewOpenFundApplyEquipmentBLL;
        public ViewOpenFundApplyBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __viewOpenFundApplyEquipmentBLL = BLLFactory.CreateViewOpenFundApplyEquipmentBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewOpenFundApply> viewOpenFundApplyList, bool isSupressLazyLoad = false)
        {
            if (viewOpenFundApplyList == null || viewOpenFundApplyList.Count == 0) return;
            var equipmentOpenFundApplyPrivilige = PriviligeFactory.GetOpenFundApplyPrivilige(userId.Value);
            foreach (var viewOpenFundApply in viewOpenFundApplyList)
            {
                viewOpenFundApply.IsSupressLazyLoad = false;
                viewOpenFundApply.InitOperate();
                OperateDecorate(userId, viewOpenFundApply, equipmentOpenFundApplyPrivilige);
                viewOpenFundApply.BuildOperate();
                viewOpenFundApply.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewOpenFundApply viewOpenFundApply, OpenFundApplyPrivilige equipmentOpenFundApplyPrivilige)
        {
            if (viewOpenFundApply == null) throw new ArgumentException("开放基金申请单为空");
            if (equipmentOpenFundApplyPrivilige == null)
            {
                viewOpenFundApply.ModifyOperate = "";
                viewOpenFundApply.AuditOperate = "";
                viewOpenFundApply.BalanceOperate = "";
                viewOpenFundApply.DeleteOperate = "";
                return;
            }
            if (!equipmentOpenFundApplyPrivilige.IsEnableEdit || viewOpenFundApply.StatusEnum != OpenFundApplyStatus.AuditWaitting) viewOpenFundApply.ModifyOperate = "";
            if (!equipmentOpenFundApplyPrivilige.IsEnableAudit || viewOpenFundApply.StatusEnum == OpenFundApplyStatus.BalanceClosed) viewOpenFundApply.AuditOperate = "";
            if (!equipmentOpenFundApplyPrivilige.IsEnableBalance || viewOpenFundApply.StatusEnum != OpenFundApplyStatus.AuditPass) viewOpenFundApply.BalanceOperate = "";
            if (!equipmentOpenFundApplyPrivilige.IsEnableDelete || viewOpenFundApply.StatusEnum != OpenFundApplyStatus.AuditWaitting) viewOpenFundApply.DeleteOperate = "";
        }
        public IList<ViewOpenFundApply> GetViewOpenFundApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewOpenFundApplyList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewOpenFundApplyList, isSupressLazyLoad);
            return viewOpenFundApplyList;
        }
        public IList<ViewOpenFundApply> GetViewOpenFundApplyListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewOpenFundApplyList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewOpenFundApplyList, isSupressLazyLoad);
            return viewOpenFundApplyList;
        }
        public IList<ViewOpenFundApply> GetViewOpenFundApplyListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewOpenFundApplyList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewOpenFundApplyList, isSupressLazyLoad);
            return viewOpenFundApplyList;
        }
        public GridData<ViewOpenFundApply> GetGridViewOpenFundApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewOpenFundApplyList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewOpenFundApplyList == null ? null : viewOpenFundApplyList.Data, isSupressLazyLoad);
            return viewOpenFundApplyList;
        }
        public GridData<ViewOpenFundApply> GetGridViewOpenFundApplyListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewOpenFundApplyList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewOpenFundApplyList == null ? null : viewOpenFundApplyList.Data, isSupressLazyLoad);
            return viewOpenFundApplyList;
        }

        public IList<ViewOpenFundApply> GetManageViewOpenFundApplyListScopeEntitiesByExpression(Guid? userId, string expression, int beginIndex, int takeCount, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings();
            dataGridSettings.QueryExpression = expression;
            if (!IsManageOpenFundApply(userId, ref dataGridSettings)) return null;
            return GetScopeEntitiesByExpression(expression, beginIndex, takeCount, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }

        private bool IsManageOpenFundApply(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var dataGridSettings_Apply = new DataGridSettings() { QueryExpression = "" };
                var applyIdList = __viewOpenFundApplyEquipmentBLL.GetManageOpenFundApplyIdByExpression(userId, dataGridSettings_Apply, null, "", true, false, true, new string[] { "EquipmentAdminId", "OpenFundApplyEquipmentId", "EquipmentOrgId" });
                if (applyIdList != null && applyIdList.Count() > 0) dataGridSettings.AppendAndQueryExpression(applyIdList.ToFormatStr());
                else return false;
            }
            return true;
        }

    }
}
