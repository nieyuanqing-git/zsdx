using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.BLL.Business.Privilige;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewDepositRecordOpenFundBLL : BLLBase<ViewDepositRecordOpenFund>, IViewDepositRecordOpenFundBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IDictCodeBLL __dictCodeBLL;
        public ViewDepositRecordOpenFundBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewDepositRecordOpenFund> viewDepositRecordOpenFundList, bool isSupressLazyLoad)
        {
            if (viewDepositRecordOpenFundList == null || viewDepositRecordOpenFundList.Count == 0) return;
            var isDepositRecordOpenFundNeedPreAudit = __dictCodeBLL.GetDictCodeBoolValueByCode("DepositRecord", "IsOpenFundDepositRecordNeedPreAudit");
            foreach (var item in viewDepositRecordOpenFundList)
            {
                item.InitOperate();
                OperateDecorate(userId, item, isDepositRecordOpenFundNeedPreAudit);
                item.BuildOperate();
                item.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewDepositRecordOpenFund viewDepositRecordOpenFund, bool? isDepositRecordOpenFundNeedPreAudit)
        {
            if (viewDepositRecordOpenFund == null) throw new ArgumentException("用户信息为空");
            var depositRecordOpenFundPrivilige = userId.HasValue ? PriviligeFactory.GetDepositRecordOpenFundPrivilige(userId.Value) : null;
            if (depositRecordOpenFundPrivilige == null)
            {
                viewDepositRecordOpenFund.ModifyOperate = "";
                viewDepositRecordOpenFund.DeleteOperate = "";
                viewDepositRecordOpenFund.PreAuditOperate = "";
                viewDepositRecordOpenFund.ResetPreAuditOperate = "";
                viewDepositRecordOpenFund.DepositMoneyOperate = "";
                return;
            }
            if (!depositRecordOpenFundPrivilige.IsEnableEdit) viewDepositRecordOpenFund.ModifyOperate = "";
            if (!depositRecordOpenFundPrivilige.IsEnableDelete) viewDepositRecordOpenFund.DeleteOperate = "";
            if (!depositRecordOpenFundPrivilige.IsEnablePreAudit || !isDepositRecordOpenFundNeedPreAudit.HasValue || !isDepositRecordOpenFundNeedPreAudit.Value) viewDepositRecordOpenFund.PreAuditOperate = "";
            if (!depositRecordOpenFundPrivilige.IsEnableResetPreAudit || !isDepositRecordOpenFundNeedPreAudit.HasValue || !isDepositRecordOpenFundNeedPreAudit.Value || viewDepositRecordOpenFund.StatusEnum != OpenFundDepositRecordStatus.AuditReject) viewDepositRecordOpenFund.ResetPreAuditOperate = "";
            if (viewDepositRecordOpenFund.StatusEnum == OpenFundDepositRecordStatus.AuditPass
                || viewDepositRecordOpenFund.StatusEnum == OpenFundDepositRecordStatus.AuditReject)
            {
                viewDepositRecordOpenFund.PreAuditOperate = "";
                viewDepositRecordOpenFund.ModifyOperate = "";
                viewDepositRecordOpenFund.DeleteOperate = "";
            }
            if(!viewDepositRecordOpenFund.OpenFundApplyId.HasValue || viewDepositRecordOpenFund.StatusEnum != OpenFundDepositRecordStatus.AuditPass)
                viewDepositRecordOpenFund.DepositMoneyOperate = "";
            if (!depositRecordOpenFundPrivilige.IsEnablePreAudit && viewDepositRecordOpenFund.UserId != userId)
                viewDepositRecordOpenFund.DepositMoneyOperate = "";
        }
        public IList<ViewDepositRecordOpenFund> GetViewDepositRecordOpenFundListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDepositRecordOpenFund(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDepositRecordOpenFundList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordOpenFundList, isSupressLazyLoad);
            return viewDepositRecordOpenFundList;
        }
        public IList<ViewDepositRecordOpenFund> GetViewDepositRecordOpenFundListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDepositRecordOpenFund(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDepositRecordOpenFundList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordOpenFundList, isSupressLazyLoad);
            return viewDepositRecordOpenFundList;
        }
        public IList<ViewDepositRecordOpenFund> GetViewDepositRecordOpenFundListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDepositRecordOpenFund(userId, ref dataGridSettings)) return null;
            var viewDepositRecordOpenFundList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordOpenFundList, isSupressLazyLoad);
            return viewDepositRecordOpenFundList;
        }
        public GridData<ViewDepositRecordOpenFund> GetGridViewDepositRecordOpenFundListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDepositRecordOpenFund(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDepositRecordOpenFundList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordOpenFundList == null ? null : viewDepositRecordOpenFundList.Data, isSupressLazyLoad);
            return viewDepositRecordOpenFundList;
        }
        public GridData<ViewDepositRecordOpenFund> GetPayerGridViewDepositRecordOpenFundListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "")
        {
            var viewDepositRecordOpenFundList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, true);
            ExcuteOperateDecorate(userId, viewDepositRecordOpenFundList == null ? null : viewDepositRecordOpenFundList.Data, true);
            return viewDepositRecordOpenFundList;
        }
        public GridData<ViewDepositRecordOpenFund> GetGridViewDepositRecordOpenFundListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDepositRecordOpenFund(userId, ref dataGridSettings)) return null;
            var viewDepositRecordOpenFundList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordOpenFundList == null ? null : viewDepositRecordOpenFundList.Data, isSupressLazyLoad);
            return viewDepositRecordOpenFundList;
        }
        public int GetViewDepositRecordOpenFundCountByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GetViewDepositRecordOpenFundCountByExpression(userId, dataGridSettings,mapping, isDistinct, outDistinctMapping);
        }
        public int GetViewDepositRecordOpenFundCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDepositRecordOpenFund(userId, ref dataGridSettings)) return 0;
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }
        private bool IsManageDepositRecordOpenFund(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
                dataGridSettings.AppendAndQueryExpression(__workGroupModuleBLL.GetQueryManagerUserOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.DepositRecordOpenFund)));
            return true;
        }
    }
}
