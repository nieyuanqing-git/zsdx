using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewBalanceAccountBLL : BLLBase<ViewBalanceAccount>, IViewBalanceAccountBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewBalanceAccountBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        public IList<ViewBalanceAccount> GetViewBalanceAccountListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageBalanceAccount(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewBalanceAccountList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewBalanceAccountList;
        }
        public IList<ViewBalanceAccount> GetViewBalanceAccountListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageBalanceAccount(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewBalanceAccountList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewBalanceAccountList;
        }
        public IList<ViewBalanceAccount> GetViewBalanceAccountListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageBalanceAccount(userId, ref dataGridSettings)) return null;
            var viewBalanceAccountList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewBalanceAccountList;
        }
        public GridData<ViewBalanceAccount> GetGridViewBalanceAccountListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageBalanceAccount(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewBalanceAccountList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewBalanceAccountList;
        }
        public GridData<ViewBalanceAccount> GetGridViewBalanceAccountListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageBalanceAccount(userId, ref dataGridSettings)) return null;
            var viewBalanceAccountList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewBalanceAccountList;
        }
        private bool IsManageBalanceAccount(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
                dataGridSettings.AppendAndQueryExpression(__workGroupModuleBLL.GetQueryManagerUserOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.BalanceAccount)));
            return true;
        }
    }
}
