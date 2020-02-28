using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleStatusBLL : BLLBase<SampleStatus>, ISampleStatusBLL
    {

        private void ExcuteOperateDecorate(Guid? userId, IList<SampleStatus> sampleStatusList, bool isSupressLazyLoad)
        {
            SampleStatusPrivilige sampleStatusPrivilige = null;
            if (userId.HasValue) sampleStatusPrivilige = PriviligeFactory.GetSampleStatusPrivilige(userId.Value);
            if (sampleStatusList == null || sampleStatusList.Count == 0) return;
            foreach (var SampleStatus in sampleStatusList)
            {
                SampleStatus.IsSupressLazyLoad = false;
                SampleStatus.InitOperate();
                OperateDecorate(userId, SampleStatus, sampleStatusPrivilige);
                SampleStatus.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, SampleStatus sampleStatus, SampleStatusPrivilige sampleStatusPrivilige)
        {
            if (sampleStatus == null) throw new ArgumentException("样品状态为空");
            if (sampleStatusPrivilige == null)
            {
                sampleStatus.EditOperate = "";
                return;
            }
            if (!sampleStatusPrivilige.IsEnableEdit)
            {
                sampleStatus.EditOperate = "";
            }

        }
        public IList<SampleStatus> GetSampleStatusListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var sampleStatusList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, sampleStatusList, isSupressLazyLoad);
            return sampleStatusList;
        }
        public IList<SampleStatus> GetSampleStatusListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var sampleStatusList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, sampleStatusList, isSupressLazyLoad);
            return sampleStatusList;
        }
        public IList<SampleStatus> GetSampleStatusListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var sampleStatusList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, sampleStatusList, isSupressLazyLoad);
            return sampleStatusList;
        }
        public GridData<SampleStatus> GetGridSampleStatusListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var sampleStatusList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, sampleStatusList == null ? null : sampleStatusList.Data, isSupressLazyLoad);
            return sampleStatusList;
        }
        public GridData<SampleStatus> GetGridSampleStatusListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var sampleStatusList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, sampleStatusList == null ? null : sampleStatusList.Data, isSupressLazyLoad);
            return sampleStatusList;
        }
       
    }
}