using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewSampleItemLabelLabelBLL : BLLBase<ViewSampleItemLabel>, IViewSampleItemLabelBLL
    {
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewSampleItemLabel> viewSampleItemLabelList, bool isSupressLazyLoad)
        {
            SampleItemLabelPrivilige sampleItemLabelPrivilige = null;
            if (userId.HasValue) sampleItemLabelPrivilige = PriviligeFactory.GetSampleItemLabelPrivilige(userId.Value);
            if (viewSampleItemLabelList != null && viewSampleItemLabelList.Count > 0)
            {
                foreach (var viewSampleItemLabel in viewSampleItemLabelList)
                {
                    viewSampleItemLabel.IsSupressLazyLoad = false;
                    viewSampleItemLabel.InitOperate();
                    OperateDecorate(userId, viewSampleItemLabel, sampleItemLabelPrivilige);
                    viewSampleItemLabel.IsSupressLazyLoad = isSupressLazyLoad;
                }
            }
        }
        private void OperateDecorate(Guid? userId, ViewSampleItemLabel viewSampleItemLabel, SampleItemLabelPrivilige SampleItemLabelPrivilige)
        {
            if (viewSampleItemLabel == null) throw new ArgumentException("检测项目标签为空");
            if (SampleItemLabelPrivilige == null)
            {
                viewSampleItemLabel.EditOperate = "";
                return;
            }
            if (!SampleItemLabelPrivilige.IsEnableEdit)
            {
                viewSampleItemLabel.EditOperate = "";
            }
        }
        public IList<ViewSampleItemLabel> GetViewSampleItemLabelListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageSampleItemLabel(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var ViewSampleItemLabelList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemLabelList, isSupressLazyLoad);
            return ViewSampleItemLabelList;
        }
        public IList<ViewSampleItemLabel> GetViewSampleItemLabelListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageSampleItemLabel(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var ViewSampleItemLabelList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemLabelList, isSupressLazyLoad);
            return ViewSampleItemLabelList;
        }
        public IList<ViewSampleItemLabel> GetViewSampleItemLabelListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageSampleItemLabel(userId, ref dataGridSettings)) return null;
            var ViewSampleItemLabelList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemLabelList, isSupressLazyLoad);
            return ViewSampleItemLabelList;
        }
        public GridData<ViewSampleItemLabel> GetGridViewSampleItemLabelListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageSampleItemLabel(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var ViewSampleItemLabelList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemLabelList == null ? null : ViewSampleItemLabelList.Data, isSupressLazyLoad);
            return ViewSampleItemLabelList;
        }
        public GridData<ViewSampleItemLabel> GetGridViewSampleItemLabelListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageSampleItemLabel(userId, ref dataGridSettings)) return null;
            var ViewSampleItemLabelList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemLabelList == null ? null : ViewSampleItemLabelList.Data, isSupressLazyLoad);
            return ViewSampleItemLabelList;
        }
        private bool IsManageSampleItemLabel(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            IWorkGroupModuleBLL workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            if (!userId.HasValue) return false;
            var user = userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
                var queryOrgId = workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.SampleItemLabel, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
