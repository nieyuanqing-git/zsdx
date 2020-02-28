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
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Business.Privilige;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewMaterialUserAccountBLL : BLLBase<ViewMaterialUserAccount>, IViewMaterialUserAccountBLL
    {

        public ViewMaterialUserAccountBLL()
        {
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewMaterialUserAccount> viewMaterialUserAccountList, bool isSupressLazyLoad = false)
        {
            if (viewMaterialUserAccountList == null || viewMaterialUserAccountList.Count == 0) return;
            var materialUserAccountPrivilige = userId.HasValue ? PriviligeFactory.GetMaterialUserAccountPrivilige(userId.Value) : null;
            foreach (var viewMaterialUserAccount in viewMaterialUserAccountList)
            {
                viewMaterialUserAccount.IsSupressLazyLoad = false;
                viewMaterialUserAccount.InitOperate();
                OperateDecorate(userId, viewMaterialUserAccount, materialUserAccountPrivilige);
                viewMaterialUserAccount.BuildOperate();
                viewMaterialUserAccount.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewMaterialUserAccount viewMaterialUserAccount, MaterialUserAccountPrivilige materialUserAccountPrivilige)
        {
            if (viewMaterialUserAccount == null) throw new ArgumentException("耗材领用单为空");
            if (materialUserAccountPrivilige == null)
            {
                viewMaterialUserAccount.ModifyOperate = "";
                viewMaterialUserAccount.DeleteOperate = "";
                viewMaterialUserAccount.InputOperate = "";
                viewMaterialUserAccount.OutputOperate = "";
                return;
            }
            if (!materialUserAccountPrivilige.IsEnableEdit) viewMaterialUserAccount.ModifyOperate = "";
            if (!materialUserAccountPrivilige.IsEnableDelete) viewMaterialUserAccount.DeleteOperate = "";
            if (!materialUserAccountPrivilige.IsEnableInputMoney) viewMaterialUserAccount.InputOperate = "";
            if (!materialUserAccountPrivilige.IsEnableOutputMoney) viewMaterialUserAccount.OutputOperate = "";
        }
        public IList<ViewMaterialUserAccount> GetViewMaterialUserAccountListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialUserAccountList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialUserAccountList, isSupressLazyLoad);
            return viewMaterialUserAccountList;
        }
        public IList<ViewMaterialUserAccount> GetViewMaterialUserAccountListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialUserAccountList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialUserAccountList, isSupressLazyLoad);
            return viewMaterialUserAccountList;
        }
        public IList<ViewMaterialUserAccount> GetViewMaterialUserAccountListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialUserAccountList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialUserAccountList, isSupressLazyLoad);
            return viewMaterialUserAccountList;
        }
        public GridData<ViewMaterialUserAccount> GetGridViewMaterialUserAccountListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialUserAccountList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialUserAccountList == null ? null : viewMaterialUserAccountList.Data, isSupressLazyLoad);
            return viewMaterialUserAccountList;
        }
        public GridData<ViewMaterialUserAccount> GetGridViewMaterialUserAccountListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialUserAccountList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialUserAccountList == null ? null : viewMaterialUserAccountList.Data, isSupressLazyLoad);
            return viewMaterialUserAccountList;
        }
    }
}
