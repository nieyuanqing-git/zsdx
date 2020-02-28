using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewUserEquipmentPriviligeBLL : BLLBase<ViewUserEquipmentPrivilige>, IViewUserEquipmentPriviligeBLL
    {
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewUserEquipmentPrivilige> viewUserEquipmentPriviligeList, bool isSupressLazyLoad)
        {
            UserEquipmentPriviligePrivilige userEquipmentPrivilige = null;
            if (userId.HasValue) userEquipmentPrivilige = PriviligeFactory.GetUserEquipmentPriviligePrivilige(userId.Value);
            foreach (var viewUserEquipmentPrivilige in viewUserEquipmentPriviligeList)
            {
                viewUserEquipmentPrivilige.IsSupressLazyLoad = false;
                viewUserEquipmentPrivilige.InitOperate();
                OperateDecorate(userId, viewUserEquipmentPrivilige, userEquipmentPrivilige);
                viewUserEquipmentPrivilige.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewUserEquipmentPrivilige viewUserEquipmentPrivilige, UserEquipmentPriviligePrivilige uerEquipmentPrivilige)
        {
            if (viewUserEquipmentPrivilige == null) throw new ArgumentException("送样权限为空");
            if (uerEquipmentPrivilige == null)
            {
                viewUserEquipmentPrivilige.EditOperate = "";
                return;
            }
            if (!uerEquipmentPrivilige.IsEnableEdit)
            {
                viewUserEquipmentPrivilige.EditOperate = "";
            }

        }
        public IList<ViewUserEquipmentPrivilige> GetViewUserEquipmentPriviligeListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUserEquipmentPrivilige(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserEquipmentPriviligeList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentPriviligeList, isSupressLazyLoad);
            return viewUserEquipmentPriviligeList;
        }
        public IList<ViewUserEquipmentPrivilige> GetViewUserEquipmentPriviligeListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUserEquipmentPrivilige(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserEquipmentPriviligeList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentPriviligeList, isSupressLazyLoad);
            return viewUserEquipmentPriviligeList;
        }
        public IList<ViewUserEquipmentPrivilige> GetViewUserEquipmentPriviligeListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageUserEquipmentPrivilige(userId, ref dataGridSettings)) return null;
            var viewUserEquipmentPriviligeList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentPriviligeList, isSupressLazyLoad);
            return viewUserEquipmentPriviligeList;
        }
        public GridData<ViewUserEquipmentPrivilige> GetGridViewUserEquipmentPriviligeListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUserEquipmentPrivilige(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserEquipmentPriviligeList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentPriviligeList == null ? null : viewUserEquipmentPriviligeList.Data, isSupressLazyLoad);
            return viewUserEquipmentPriviligeList;
        }
        public GridData<ViewUserEquipmentPrivilige> GetGridViewUserEquipmentPriviligeListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageUserEquipmentPrivilige(userId, ref dataGridSettings)) return null;
            var viewUserEquipmentPriviligeList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentPriviligeList == null ? null : viewUserEquipmentPriviligeList.Data, isSupressLazyLoad);
            return viewUserEquipmentPriviligeList;
        }
        private bool IsManageUserEquipmentPrivilige(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            IWorkGroupModuleBLL workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            if (!userId.HasValue) return false;
            var user = userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
                var queryOrgId = workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.SampleItem, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
