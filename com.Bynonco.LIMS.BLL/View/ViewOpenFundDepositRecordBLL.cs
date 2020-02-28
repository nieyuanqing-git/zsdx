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
    public class ViewOpenFundDepositRecordBLL : BLLBase<ViewOpenFundDepositRecord>, IViewOpenFundDepositRecordBLL
    {
        private IUserBLL __userBLL;
        private IViewOpenFundApplyEquipmentBLL __viewOpenFundApplyEquipmentBLL;
        private IDictCodeBLL __dictCodeBLL;
        public ViewOpenFundDepositRecordBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __viewOpenFundApplyEquipmentBLL = BLLFactory.CreateViewOpenFundApplyEquipmentBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewOpenFundDepositRecord> viewOpenFundDepositRecordList, bool isSupressLazyLoad)
        {
            if (viewOpenFundDepositRecordList == null || viewOpenFundDepositRecordList.Count == 0) return;
            var isDepositRecordNeedPreAudit = __dictCodeBLL.GetDictCodeBoolValueByCode("DepositRecord", "IsOpenFundDepositRecordNeedPreAudit");
            foreach (var item in viewOpenFundDepositRecordList)
            {
                item.InitOperate();
                OperateDecorate(userId, item, isDepositRecordNeedPreAudit);
                item.BuildOperate();
                item.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewOpenFundDepositRecord viewOpenFundDepositRecord, bool? isDepositRecordNeedPreAudit)
        {
            if (viewOpenFundDepositRecord == null) throw new ArgumentException("用户信息为空");
            var openFundApplyPrivilige = userId.HasValue ? PriviligeFactory.GetOpenFundApplyPrivilige(userId.Value) : null;
            if (openFundApplyPrivilige == null)
            {
                viewOpenFundDepositRecord.ModifyOperate = "";
                viewOpenFundDepositRecord.DeleteOperate = "";
                viewOpenFundDepositRecord.AuditOperate = "";
                viewOpenFundDepositRecord.PreAuditOperate = "";
                return;
            }
            if (!openFundApplyPrivilige.IsEnableDepositEdit) viewOpenFundDepositRecord.ModifyOperate = "";
            if (!openFundApplyPrivilige.IsEnableDepositDelete) viewOpenFundDepositRecord.DeleteOperate = "";
            if (!openFundApplyPrivilige.IsEnableDepositAudit) viewOpenFundDepositRecord.AuditOperate = "";
            if (!openFundApplyPrivilige.IsEnableDepositPreAudit || !isDepositRecordNeedPreAudit.HasValue || !isDepositRecordNeedPreAudit.Value) viewOpenFundDepositRecord.PreAuditOperate = "";
            if (isDepositRecordNeedPreAudit.HasValue && isDepositRecordNeedPreAudit.Value && viewOpenFundDepositRecord.DepositRecordStatusEnum != OpenFundDepositRecordStatus.AuditPass) viewOpenFundDepositRecord.AuditOperate = "";
            if (viewOpenFundDepositRecord.HasChecked) viewOpenFundDepositRecord.PreAuditOperate = "";
        }
        public IList<ViewOpenFundDepositRecord> GetViewOpenFundDepositRecordListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageOpenFundDepositRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewOpenFundDepositRecordList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewOpenFundDepositRecordList, isSupressLazyLoad);
            return viewOpenFundDepositRecordList;
        }
        public IList<ViewOpenFundDepositRecord> GetViewOpenFundDepositRecordListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageOpenFundDepositRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewOpenFundDepositRecordList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewOpenFundDepositRecordList, isSupressLazyLoad);
            return viewOpenFundDepositRecordList;
        }
        public IList<ViewOpenFundDepositRecord> GetViewOpenFundDepositRecordListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageOpenFundDepositRecord(userId, ref dataGridSettings)) return null;
            var viewOpenFundDepositRecordList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewOpenFundDepositRecordList, isSupressLazyLoad);
            return viewOpenFundDepositRecordList;
        }
        public GridData<ViewOpenFundDepositRecord> GetGridViewOpenFundDepositRecordListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageOpenFundDepositRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewOpenFundDepositRecordList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewOpenFundDepositRecordList == null ? null : viewOpenFundDepositRecordList.Data, isSupressLazyLoad);
            return viewOpenFundDepositRecordList;
        }
        public GridData<ViewOpenFundDepositRecord> GetGridViewOpenFundDepositRecordListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageOpenFundDepositRecord(userId, ref dataGridSettings)) return null;
            var viewOpenFundDepositRecordList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewOpenFundDepositRecordList == null ? null : viewOpenFundDepositRecordList.Data, isSupressLazyLoad);
            return viewOpenFundDepositRecordList;
        }
        public GridData<ViewOpenFundDepositRecord> GetPayerGridViewOpenFundDepositRecordListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewOpenFundDepositRecordList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewOpenFundDepositRecordList == null ? null : viewOpenFundDepositRecordList.Data, isSupressLazyLoad);
            return viewOpenFundDepositRecordList;
        }
        public int GetViewOpenFundDepositRecordCountByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GetViewOpenFundDepositRecordCountByExpression(userId, dataGridSettings,mapping, isDistinct, outDistinctMapping);
        }
        public int GetViewOpenFundDepositRecordCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageOpenFundDepositRecord(userId, ref dataGridSettings)) return 0;
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }
        private bool IsManageOpenFundDepositRecord(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var  dataGridSettings_Apply = new DataGridSettings(){QueryExpression = ""};
                var applyIdList = __viewOpenFundApplyEquipmentBLL.GetManageOpenFundApplyIdByExpression(userId, dataGridSettings_Apply, null, "", true, false, true, new string[] { "EquipmentAdminId", "OpenFundApplyEquipmentId", "EquipmentOrgId" });
                if (applyIdList != null && applyIdList.Count() > 0) dataGridSettings.AppendAndQueryExpression(applyIdList.ToFormatStr("OpenFundApplyId"));
                else return false;
            }
            return true;
        }
    }
}
