using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewDoorAccessRecordBLL : BLLBase<ViewDoorAccessRecord>, IViewDoorAccessRecordBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewDoorAccessRecordBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        public IList<ViewDoorAccessRecord> GetViewDoorAccessRecordListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDoorAccessRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDoorAccessRecordList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorAccessRecordList;
        }
        public IList<ViewDoorAccessRecord> GetViewDoorAccessRecordListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDoorAccessRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDoorAccessRecordList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorAccessRecordList;
        }
        public IList<ViewDoorAccessRecord> GetViewDoorAccessRecordListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDoorAccessRecord(userId, ref dataGridSettings)) return null;
            var viewDoorAccessRecordList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorAccessRecordList;
        }
        public GridData<ViewDoorAccessRecord> GetGridViewDoorAccessRecordListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageDoorAccessRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDoorAccessRecordList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorAccessRecordList;
        }
        public GridData<ViewDoorAccessRecord> GetGridViewDoorAccessRecordListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageDoorAccessRecord(userId, ref dataGridSettings)) return null;
            var viewDoorAccessRecordList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return viewDoorAccessRecordList;
        }
        private bool IsManageDoorAccessRecord(Guid? userId, ref DataGridSettings dataGridSettings)
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
                foreach (var item in viewLabOrganizationAdminList) querySql += (querySql == "" ? "" : "+") + string.Format("DoorOrgXPath→\"{0}\"", item.OrgXPath);
                querySql = "(" + querySql + ")";
                dataGridSettings.AppendAndQueryExpression(querySql);
            }
            return true;
        }
    }
}
