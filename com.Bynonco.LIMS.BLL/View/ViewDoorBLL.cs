using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewDoorBLL : BLLBase<ViewDoor>, IViewDoorBLL
    {
        private IUserBLL __userBLL;
        public ViewDoorBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
        }
        public IList<ViewDoor> GetViewDoorListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDoor(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDoorList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorList;
        }
        public IList<ViewDoor> GetViewDoorListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDoor(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDoorList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorList;
        }
        public IList<ViewDoor> GetViewDoorListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDoor(userId, ref dataGridSettings)) return null;
            var viewDoorList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorList;
        }
        public GridData<ViewDoor> GetGridViewDoorListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDoor(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDoorList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorList;
        }
        public GridData<ViewDoor> GetGridViewDoorListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDoor(userId, ref dataGridSettings)) return null;
            var viewDoorList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorList;
        }
        private bool IsManageDoor(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var viewLabOrganizationAdminBLL = BLLFactory.CreateViewLabOrganizationAdminBLL();
                var viewLabOrganizationAdminList = viewLabOrganizationAdminBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id));
                if (viewLabOrganizationAdminList == null || viewLabOrganizationAdminList.Count() == 0) return false;
                string querySql = "";
                foreach (var item in viewLabOrganizationAdminList) querySql += (querySql == "" ? "" : "+") + string.Format("OrgXPath→\"{0}\"", item.OrgXPath);
                querySql = "(" + querySql + ")";
                dataGridSettings.AppendAndQueryExpression(querySql);
            }
            return true;
        }
    }
}
