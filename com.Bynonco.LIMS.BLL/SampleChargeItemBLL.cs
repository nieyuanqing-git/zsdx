using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleChargeItemBLL : BLLBase<SampleChargeItem>, ISampleChargeItemBLL
    {
        public SampleChargeItem GetDefaultSampleChargeItem()
        {
            return GetFirstOrDefaultEntityByExpression("IsDefault=true*IsDelete=false");
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<SampleChargeItem> SampleChargeItemList, bool isSupressLazyLoad)
        {
            SampleChargeItemPrivilige SampleChargeItemPrivilige = null;
            if (userId.HasValue) SampleChargeItemPrivilige = PriviligeFactory.GetSampleChargeItemPrivilige(userId.Value);
            foreach (var SampleChargeItem in SampleChargeItemList)
            {
                SampleChargeItem.IsSupressLazyLoad = false;
                SampleChargeItem.InitOperate();
                OperateDecorate(userId, SampleChargeItem, SampleChargeItemPrivilige);
                SampleChargeItem.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, SampleChargeItem SampleChargeItem, SampleChargeItemPrivilige SampleChargeItemPrivilige)
        {
            if (SampleChargeItem == null) throw new ArgumentException("收费项目为空");
            if (SampleChargeItemPrivilige == null)
            {
                SampleChargeItem.EditOperate = "";
                return;
            }
            if (!SampleChargeItemPrivilige.IsEnableEdit)
            {
                SampleChargeItem.EditOperate = "";
            }

        }
        public IList<SampleChargeItem> GetSampleChargeItemListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var SampleChargeItemList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, SampleChargeItemList, isSupressLazyLoad);
            return SampleChargeItemList;
        }
        public IList<SampleChargeItem> GetSampleChargeItemListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var SampleChargeItemList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, SampleChargeItemList, isSupressLazyLoad);
            return SampleChargeItemList;
        }
        public IList<SampleChargeItem> GetSampleChargeItemListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var SampleChargeItemList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, SampleChargeItemList, isSupressLazyLoad);
            return SampleChargeItemList;
        }
        public GridData<SampleChargeItem> GetGridSampleChargeItemListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var SampleChargeItemList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, SampleChargeItemList == null ? null : SampleChargeItemList.Data, isSupressLazyLoad);
            return SampleChargeItemList;
        }
        public GridData<SampleChargeItem> GetGridSampleChargeItemListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var SampleChargeItemList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, SampleChargeItemList == null ? null : SampleChargeItemList.Data, isSupressLazyLoad);
            return SampleChargeItemList;
        }
    }
}