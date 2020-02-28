using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Business.Privilige;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewWaterControlCostBLL : BLLBase<ViewWaterControlCost>, IViewWaterControlCostBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private Dictionary<string, string> __mapping = new Dictionary<string, string>();
        public ViewWaterControlCostBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();      
        }
        public string GenerateQueryExpression(Guid? userId, DataGridSettings dataGridSettings, bool isManageList, string controllerName, string actionName)
        {
            if (isManageList) return "Id=null";
            return dataGridSettings.QueryExpression;
        }
        public string GenerateQueryExpression(Guid? userId, string expression, bool isManageList, string controllerName, string actionName)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GenerateQueryExpression(userId, dataGridSettings, isManageList, controllerName, actionName);
        }
        public IList<ViewWaterControlCost> GetViewWaterControlListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            GenerateQueryExpression(userId, dataGridSettings, false, "", "");
            var viewWaterControlCostList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            //if (isIniOperate) ExcuteOperateDecorate(userId, viewUsedConfirmList, isSupressLazyLoad);
            return viewWaterControlCostList;
        }
        //public IList<ViewWaterControlCost> GetViewWaterControlListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        //{
        //    expression = GenerateQueryExpression(userId, expression, isManageList, "", "");
        //    var viewWaterControlCostList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        //    //if (isIniOperate) ExcuteOperateDecorate(userId, viewUsedConfirmList, isSupressLazyLoad);
        //    return viewWaterControlCostList;
        //}
    }
}
