using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using System.Data;
using com.Bynonco.LIMS.DAL;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewSJ1BLL : BLLBase<ViewSJ1>, IViewSJ1BLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewSJ1BLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        public IList<ViewSJ1> GetViewSJ1ListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageSJ1(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewSJ1List = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewSJ1List;
        }
        public IList<ViewSJ1> GetViewSJ1ListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageSJ1(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewSJ1List = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewSJ1List;
        }
        public IList<ViewSJ1> GetViewSJ1ListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageSJ1(userId, ref dataGridSettings)) return null;
            var viewSJ1List = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewSJ1List;
        }
        public GridData<ViewSJ1> GetGridViewSJ1ListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageSJ1(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewSJ1List = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewSJ1List;
        }
        public GridData<ViewSJ1> GetGridViewSJ1ListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageSJ1(userId, ref dataGridSettings)) return null;
            var viewSJ1List = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewSJ1List;
        }
        private bool IsManageSJ1(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.SJ1, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
