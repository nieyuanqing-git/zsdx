using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewSampleItemBLL : BLLBase<ViewSampleItem>, IViewSampleItemBLL
    {

        private void ExcuteOperateDecorate(Guid? userId, IList<ViewSampleItem> viewSampleItemList, bool isSupressLazyLoad)
        {
            SampleItemPrivilige sampleItemPrivilige = null;
            if (userId.HasValue) sampleItemPrivilige = PriviligeFactory.GetSampleItemPrivilige(userId.Value);
            if (viewSampleItemList != null && viewSampleItemList.Count > 0)
            {
                foreach (var viewSampleItem in viewSampleItemList)
                {
                    viewSampleItem.IsSupressLazyLoad = false;
                    viewSampleItem.InitOperate();
                    OperateDecorate(userId, viewSampleItem, sampleItemPrivilige);
                    viewSampleItem.IsSupressLazyLoad = isSupressLazyLoad;
                }
            }
        }
        private void OperateDecorate(Guid? userId, ViewSampleItem ViewSampleItem, SampleItemPrivilige sampleItemPrivilige)
        {
            if (ViewSampleItem == null) throw new ArgumentException("设备信息为空");
            if (sampleItemPrivilige == null)
            {
                ViewSampleItem.EditOperate = "";
                return;
            }
            if (!sampleItemPrivilige.IsEnableEdit)
            {
                ViewSampleItem.EditOperate = "";
            }
            
        }
        public IList<ViewSampleItem> GetViewSampleItemListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageSampleItem(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var ViewSampleItemList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemList, isSupressLazyLoad);
            return ViewSampleItemList;
        }
        public IList<ViewSampleItem> GetViewSampleItemListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageSampleItem(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var ViewSampleItemList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemList, isSupressLazyLoad);
            return ViewSampleItemList;
        }
        public IList<ViewSampleItem> GetViewSampleItemListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageSampleItem(userId, ref dataGridSettings)) return null;
            var ViewSampleItemList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemList, isSupressLazyLoad);
            return ViewSampleItemList;
        }
        public GridData<ViewSampleItem> GetGridViewSampleItemListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageSampleItem(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var ViewSampleItemList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemList == null ? null : ViewSampleItemList.Data, isSupressLazyLoad);
            return ViewSampleItemList;
        }
        public GridData<ViewSampleItem> GetGridViewSampleItemListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageSampleItem(userId, ref dataGridSettings)) return null;
            var ViewSampleItemList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewSampleItemList == null ? null : ViewSampleItemList.Data, isSupressLazyLoad);
            return ViewSampleItemList;
        }
        private bool IsManageSampleItem(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            IWorkGroupModuleBLL workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL(); 
            if (!userId.HasValue) return false;
            var user = userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
                var queryOrgId = workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.SampleItem, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
