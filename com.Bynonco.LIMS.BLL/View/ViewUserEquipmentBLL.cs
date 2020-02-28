using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewUserEquipmentBLL : BLLBase<ViewUserEquipment>, IViewUserEquipmentBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IUserEquipmentBLL __userEquipmentBLL;
        public ViewUserEquipmentBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __userEquipmentBLL = BLLFactory.CreateUserEquipmentBLL();
        }
        public void ExcuteOperateDecorate(Guid? userId, IList<ViewUserEquipment> viewUserEquipmentList, bool isSupressLazyLoad)
        {
            if (viewUserEquipmentList == null || viewUserEquipmentList.Count == 0) return;
            IList<com.Bynonco.LIMS.BLL.Business.Privilige.UserEquipmentPrivilige> lstUserEquipmentPriviliges = (IList<com.Bynonco.LIMS.BLL.Business.Privilige.UserEquipmentPrivilige>)__userEquipmentBLL.GetUserEquipmentPriviliges(userId, viewUserEquipmentList);
            foreach (var viewUserEquipment in viewUserEquipmentList)
            {
                var userEquipmentPrivilige = lstUserEquipmentPriviliges.FirstOrDefault(p => p.UserEquipmentId.HasValue && p.UserEquipmentId.Value == viewUserEquipment.Id);
                viewUserEquipment.IsSupressLazyLoad = false;
                viewUserEquipment.InitOperate();
                OperateDecorate(userId, viewUserEquipment, userEquipmentPrivilige);
                viewUserEquipment.BuildOperate();
                viewUserEquipment.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewUserEquipment viewUserEquipment, com.Bynonco.LIMS.BLL.Business.Privilige.UserEquipmentPrivilige userEquipmentPrivilige)
        {
            if (viewUserEquipment == null) throw new ArgumentException("用户设备信息为空");
            if (userEquipmentPrivilige == null)
            {
                viewUserEquipment.AuditOperate = "";
                viewUserEquipment.RejectOperate = "";
                viewUserEquipment.DeleteOperate = "";
                return;
            }
            if (!userEquipmentPrivilige.IsEnablePass)  viewUserEquipment.AuditOperate = "";
            if (!userEquipmentPrivilige.IsEnableReject) viewUserEquipment.RejectOperate = "";
            if (!userEquipmentPrivilige.IsEnableDelete) viewUserEquipment.DeleteOperate = "";
        }
        public IList<ViewUserEquipment> GetViewUserEquipmentListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsUserEquipment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserEquipmentList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentList, isSupressLazyLoad);
            return viewUserEquipmentList;
        }
        public IList<ViewUserEquipment> GetViewUserEquipmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsUserEquipment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserEquipmentList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentList, isSupressLazyLoad);
            return viewUserEquipmentList;
        }
        public IList<ViewUserEquipment> GetViewUserEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsUserEquipment(userId, ref dataGridSettings)) return null;
            var viewUserEquipmentList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentList, isSupressLazyLoad);
            return viewUserEquipmentList;
        }
        public GridData<ViewUserEquipment> GetGridViewUserEquipmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsUserEquipment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserEquipmentList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentList == null ? null : viewUserEquipmentList.Data, isSupressLazyLoad);
            return viewUserEquipmentList;
        }
        public GridData<ViewUserEquipment> GetGridViewUserEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsUserEquipment(userId, ref dataGridSettings)) return null;
            var viewUserEquipmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentList == null ? null : viewUserEquipmentList.Data, isSupressLazyLoad);
            return viewUserEquipmentList;
        }
        public int GetViewUserEquipmentCountByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GetViewUserEquipmentCountByExpression(userId, dataGridSettings, mapping, isDistinct, outDistinctMapping, isManageList);
        }
        public int GetViewUserEquipmentCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            if (isManageList && !IsUserEquipment(userId, ref dataGridSettings)) return 0;
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }
        private bool IsUserEquipment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.UserEquipment, "", "", "UserOrgId"), new ManagerEquipmentOrgId(ModuleBusinessType.UserEquipment, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
