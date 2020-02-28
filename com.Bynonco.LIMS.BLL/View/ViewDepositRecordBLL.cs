using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;
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
    public class ViewDepositRecordBLL : BLLBase<ViewDepositRecord>, IViewDepositRecordBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IDictCodeBLL __dictCodeBLL;
        public ViewDepositRecordBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewDepositRecord> viewDepositRecordList, bool isSupressLazyLoad)
        {
            if (viewDepositRecordList == null || viewDepositRecordList.Count == 0) return;
            var customer = CustomerFactory.GetCustomer();
            var customerIsDepositPhotoRequired = customer != null && customer.GetIsDepositPhotoRequired();
            var isDepositRecordNeedPreAudit = __dictCodeBLL.GetDictCodeBoolValueByCode("DepositRecord", "IsDepositRecordNeedPreAudit");
            foreach (var item in viewDepositRecordList)
            {
                item.InitOperate();
                OperateDecorate(userId, item, isDepositRecordNeedPreAudit.HasValue && isDepositRecordNeedPreAudit.Value, customerIsDepositPhotoRequired);
                item.BuildOperate();
                item.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewDepositRecord viewDepositRecord, bool isDepositRecordNeedPreAudit, bool isDepositPhotoRequired)
        {
            if (viewDepositRecord == null) throw new ArgumentException("用户信息为空");
            var depositRecordPrivilige = userId.HasValue ? PriviligeFactory.GetDepositRecordPrivilige(userId.Value) : null;
            if (depositRecordPrivilige == null)
            {
                viewDepositRecord.ModifyOperate = "";
                viewDepositRecord.DeleteOperate = "";
                viewDepositRecord.AuditOperate = "";
                viewDepositRecord.PreAuditOperate = "";
                viewDepositRecord.ResetPreAuditOperate = "";
                return;
            }
            if (!depositRecordPrivilige.IsEnableEdit) viewDepositRecord.ModifyOperate = "";
            if (!depositRecordPrivilige.IsEnableDelete) viewDepositRecord.DeleteOperate = "";
            if (!depositRecordPrivilige.IsEnableAudit) viewDepositRecord.AuditOperate = "";
            if (isDepositRecordNeedPreAudit)
            {
                if (viewDepositRecord.DepositRecordStatusEnum != DepositRecordStatus.AuditReject) viewDepositRecord.ResetPreAuditOperate = "";
                if (viewDepositRecord.DepositRecordStatusEnum != DepositRecordStatus.AuditPass) viewDepositRecord.AuditOperate = "";
                if (viewDepositRecord.HasChecked || viewDepositRecord.DepositRecordStatusEnum != DepositRecordStatus.AuditWaitting) viewDepositRecord.PreAuditOperate = "";
            }
            else
            {
                if (!depositRecordPrivilige.IsEnablePreAudit) viewDepositRecord.PreAuditOperate = "";
                if (!depositRecordPrivilige.IsEnableResetPreAudit || viewDepositRecord.DepositRecordStatusEnum != DepositRecordStatus.AuditReject) viewDepositRecord.ResetPreAuditOperate = "";
                //if (viewDepositRecord.HasChecked || viewDepositRecord.DepositRecordStatusEnum != DepositRecordStatus.Draft) 
                viewDepositRecord.PreAuditOperate = "";
            }
            if (isDepositPhotoRequired && string.IsNullOrEmpty(viewDepositRecord.DepositRecordPhoto)) viewDepositRecord.AuditOperate = "";
            //if (!depositRecordPrivilige.IsEnablePreAudit || !isDepositRecordNeedPreAudit.HasValue || !isDepositRecordNeedPreAudit.Value) viewDepositRecord.PreAuditOperate = "";
            //if (!depositRecordPrivilige.IsEnableResetPreAudit || !isDepositRecordNeedPreAudit.HasValue || !isDepositRecordNeedPreAudit.Value || viewDepositRecord.DepositRecordStatusEnum != DepositRecordStatus.AuditReject) viewDepositRecord.ResetPreAuditOperate = "";
            //if (isDepositRecordNeedPreAudit.HasValue && isDepositRecordNeedPreAudit.Value && viewDepositRecord.DepositRecordStatusEnum != DepositRecordStatus.AuditPass) viewDepositRecord.AuditOperate = "";
            //if (string.IsNullOrEmpty(viewDepositRecord.DepositRecordPhoto)) viewDepositRecord.AuditOperate = "";
            //if (viewDepositRecord.HasChecked || viewDepositRecord.DepositRecordStatusEnum != DepositRecordStatus.AuditWaitting) viewDepositRecord.PreAuditOperate = "";
        }
        public IList<ViewDepositRecord> GetViewDepositRecordListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDepositRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDepositRecordList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordList, isSupressLazyLoad);
            return viewDepositRecordList;
        }
        public IList<ViewDepositRecord> GetViewDepositRecordListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDepositRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDepositRecordList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordList, isSupressLazyLoad);
            return viewDepositRecordList;
        }
        public IList<ViewDepositRecord> GetViewDepositRecordListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDepositRecord(userId, ref dataGridSettings)) return null;
            var viewDepositRecordList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordList, isSupressLazyLoad);
            return viewDepositRecordList;
        }
        public GridData<ViewDepositRecord> GetGridViewDepositRecordListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDepositRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDepositRecordList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordList == null ? null : viewDepositRecordList.Data, isSupressLazyLoad);
            return viewDepositRecordList;
        }
        public GridData<ViewDepositRecord> GetPayerGridViewDepositRecordListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "")
        {
            var viewDepositRecordList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, true);
            ExcuteOperateDecorate(userId, viewDepositRecordList == null ? null : viewDepositRecordList.Data, true);
            return viewDepositRecordList;
        }
        public GridData<ViewDepositRecord> GetGridViewDepositRecordListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDepositRecord(userId, ref dataGridSettings)) return null;
            var viewDepositRecordList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewDepositRecordList == null ? null : viewDepositRecordList.Data, isSupressLazyLoad);
            return viewDepositRecordList;
        }
        public int GetViewDepositRecordCountByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GetViewDepositRecordCountByExpression(userId, dataGridSettings, mapping, isDistinct, outDistinctMapping);
        }
        public int GetViewDepositRecordCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDepositRecord(userId, ref dataGridSettings)) return 0;
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }
        private bool IsManageDepositRecord(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
                dataGridSettings.AppendAndQueryExpression(__workGroupModuleBLL.GetQueryManagerUserOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.DepositRecord)));
            return true;
        }
    }
}
