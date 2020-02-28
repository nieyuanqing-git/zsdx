using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewEquipmentSampleFeedBackSettingBLL : BLLBase<ViewEquipmentSampleFeedBackSetting>, IViewEquipmentSampleFeedBackSettingBLL
    {
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentSampleFeedBackSetting> viewEquipmentSampleFeedBackSettingList, bool isSupressLazyLoad)
        {
            EquipmentSampleFeedBackSettingPrivilige equipmentSampleFeedBackSettingPrivilige = null;
            if (userId.HasValue) equipmentSampleFeedBackSettingPrivilige = PriviligeFactory.GetEquipmentSampleFeedBackSettingPrivilige(userId.Value);
            if (viewEquipmentSampleFeedBackSettingList != null && viewEquipmentSampleFeedBackSettingList.Count > 0)
            {
                foreach (var ViewEquipmentSampleFeedBackSetting in viewEquipmentSampleFeedBackSettingList)
                {
                    ViewEquipmentSampleFeedBackSetting.IsSupressLazyLoad = false;
                    ViewEquipmentSampleFeedBackSetting.InitOperate();
                    OperateDecorate(userId, ViewEquipmentSampleFeedBackSetting, equipmentSampleFeedBackSettingPrivilige);
                    ViewEquipmentSampleFeedBackSetting.IsSupressLazyLoad = isSupressLazyLoad;
                }
            }
        }
        private void OperateDecorate(Guid? userId, ViewEquipmentSampleFeedBackSetting ViewEquipmentSampleFeedBackSetting, EquipmentSampleFeedBackSettingPrivilige EquipmentSampleFeedBackSettingPrivilige)
        {
            if (ViewEquipmentSampleFeedBackSetting == null) throw new ArgumentException("设备送样反馈设置信息为空");
            if (EquipmentSampleFeedBackSettingPrivilige == null)
            {
                ViewEquipmentSampleFeedBackSetting.EditOperate = "";
                return;
            }
            if (!EquipmentSampleFeedBackSettingPrivilige.IsEnableEdit)
            {
                ViewEquipmentSampleFeedBackSetting.EditOperate = "";
            }

        }
        public IList<ViewEquipmentSampleFeedBackSetting> GetViewEquipmentSampleFeedBackSettingListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewEquipmentSampleFeedBackSettingList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewEquipmentSampleFeedBackSettingList, isSupressLazyLoad);
            return ViewEquipmentSampleFeedBackSettingList;
        }
        public IList<ViewEquipmentSampleFeedBackSetting> GetViewEquipmentSampleFeedBackSettingListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewEquipmentSampleFeedBackSettingList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewEquipmentSampleFeedBackSettingList, isSupressLazyLoad);
            return ViewEquipmentSampleFeedBackSettingList;
        }
        public IList<ViewEquipmentSampleFeedBackSetting> GetViewEquipmentSampleFeedBackSettingListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewEquipmentSampleFeedBackSettingList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewEquipmentSampleFeedBackSettingList, isSupressLazyLoad);
            return ViewEquipmentSampleFeedBackSettingList;
        }
        public GridData<ViewEquipmentSampleFeedBackSetting> GetGridViewEquipmentSampleFeedBackSettingListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewEquipmentSampleFeedBackSettingList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewEquipmentSampleFeedBackSettingList == null ? null : ViewEquipmentSampleFeedBackSettingList.Data, isSupressLazyLoad);
            return ViewEquipmentSampleFeedBackSettingList;
        }
        public GridData<ViewEquipmentSampleFeedBackSetting> GetGridViewEquipmentSampleFeedBackSettingListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewEquipmentSampleFeedBackSettingList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewEquipmentSampleFeedBackSettingList == null ? null : ViewEquipmentSampleFeedBackSettingList.Data, isSupressLazyLoad);
            return ViewEquipmentSampleFeedBackSettingList;
        }
    }
}
