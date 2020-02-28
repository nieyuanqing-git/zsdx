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

namespace com.Bynonco.LIMS.BLL
{
    public class ViewThesisBLL : BLLBase<ViewThesis>, IViewThesisBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewThesisBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        public IList<ViewThesis> GetViewThesisListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageThesis(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewThesisList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewThesisList;
        }
        public IList<ViewThesis> GetViewThesisListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageThesis(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewThesisList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewThesisList;
        }
        public IList<ViewThesis> GetViewThesisListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageThesis(userId, ref dataGridSettings)) return null;
            var viewThesisList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewThesisList;
        }
        public GridData<ViewThesis> GetGridViewThesisListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageThesis(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewThesisList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewThesisList;
        }
        public GridData<ViewThesis> GetGridViewThesisListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageThesis(userId, ref dataGridSettings)) return null;
            var viewThesisList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewThesisList;
        }
        private bool IsManageThesis(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                string queryExpression = string.Format("CreateUserId=\"{0}\"", user.Id);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerUserOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.Thesis, "", "", "UserOrgId"));
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
