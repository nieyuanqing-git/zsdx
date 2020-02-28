using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewUserBLL : BLLBase<ViewUser>, IViewUserBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewUserBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewUser> viewUserList, bool isSupressLazyLoad)
        {
            if (viewUserList == null || viewUserList.Count == 0) return;
            foreach (var viewUser in viewUserList)
            {
                viewUser.IsSupressLazyLoad = false;
                viewUser.InitOperate();
                OperateDecorate(userId, viewUser);
                viewUser.BuildOperate();
                viewUser.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewUser viewUser)
        {
            if (viewUser == null) throw new ArgumentException("用户信息为空");
            var userPrivilige = userId.HasValue ? PriviligeFactory.GetUserPrivilige(userId.Value, viewUser.Id) : null;
            if (userPrivilige == null)
            {
                viewUser.ModifyOperate = "";
                viewUser.DeleteOperate = "";
                viewUser.WorkGroupOperate = "";
                return;
            }
            if (!userPrivilige.IsEnableEdit) viewUser.ModifyOperate = "";
            if (!userPrivilige.IsEnableDelete) viewUser.DeleteOperate = "";
            if (!userPrivilige.IsEnableSetWorkGroup) viewUser.WorkGroupOperate = "";
        }
        public IList<ViewUser> GetViewUserListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUser(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserList, isSupressLazyLoad);
            return viewUserList;
        }
        public IList<ViewUser> GetViewUserListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUser(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserList, isSupressLazyLoad);
            return viewUserList;
        }
        public IList<ViewUser> GetViewUserListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageUser(userId, ref dataGridSettings)) return null;
            var viewUserList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserList, isSupressLazyLoad);
            return viewUserList;
        }
        public GridData<ViewUser> GetGridViewUserListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUser(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserList == null ? null : viewUserList.Data, isSupressLazyLoad);
            return viewUserList;
        }
        public GridData<ViewUser> GetGridViewUserListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageUser(userId, ref dataGridSettings)) return null;
            var viewUserList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserList == null ? null : viewUserList.Data, isSupressLazyLoad);
            return viewUserList;
        }
        public IList<UserStatistCount> GetUserManageUserStatistCountList(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GetUserManageUserStatistCountList(userId, dataGridSettings, mapping, isDistinct, outDistinctMapping, isManageList);
        }
        public IList<UserStatistCount> GetUserManageUserStatistCountList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            if (isManageList && !IsManageUser(userId, ref dataGridSettings)) return null;
            IList<UserStatistCount> userStatistCount = new List<UserStatistCount>();
            var expression = dataGridSettings.QueryExpression;
            foreach (var item in Enum.GetValues(typeof(UserStatus)))
            {
                var queryExpression = expression + (expression == "" ? "" : "*") + string.Format("(UserStatus=\"{0}\")", (int)item);
                userStatistCount.Add(new UserStatistCount()
                {
                    UserStatus = (UserStatus)item,
                    UserCount = CountModelListByExpression(queryExpression, mapping, isDistinct, outDistinctMapping)
                });
            }
            return userStatistCount;
        }
        private bool IsManageUser(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
                dataGridSettings.AppendAndQueryExpression(__workGroupModuleBLL.GetQueryManagerUserOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.User, "", "", "OrganizationId")));
            return true;
        }

        public ViewUser GetViewUserByLabel(string label)
        {
            if (string.IsNullOrEmpty(label)) return null;
            return GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsDel=false", label));
        }
        public IList<ViewUser> GetUserManageUserList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageUser(userId, ref dataGridSettings)) return null;
            return  GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }
        private void ExcuteAuthorizationOperateDecorate(Guid? userId, IList<ViewUser> viewUserList, bool isSupressLazyLoad)
        {
            if (viewUserList == null || viewUserList.Count == 0) return;
            foreach (var viewUser in viewUserList)
            {
                viewUser.IsSupressLazyLoad = false;
                viewUser.InitAuthorizationOperate();
                OperateDecorate(userId, viewUser);
                viewUser.BuildAuthorizationOperate();
                viewUser.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void AuthorizationOperateDecorate(Guid? userId, ViewUser viewUser)
        {
            if (viewUser == null) throw new ArgumentException("用户信息为空");
            var userPrivilige = userId.HasValue ? PriviligeFactory.GetUserPrivilige(userId.Value, viewUser.Id) : null;
            if (userPrivilige == null)
            {
                viewUser.ModifyOperate = "";
                viewUser.AuditOperate = "";
                viewUser.DeleteOperate = "";
                return;
            }
            if (!userPrivilige.IsEnableAuthorizationUserEdit) viewUser.ModifyOperate = "";
            if (!userPrivilige.IsEnableAudit && !userPrivilige.IsEnableSetInvalid && !userPrivilige.IsEnableSetBlacklist) viewUser.AuditOperate = "";
            if (!userPrivilige.IsEnableAuthorizationUserDelete) viewUser.DeleteOperate = "";
        }
        public GridData<ViewUser> GetGridAuthorizationViewUserListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            if (!userId.HasValue || (isManageList && dataGridSettings.QueryExpression.IndexOf("OrganizationXPath→") == -1)) return null;
            //dataGridSettings.AppendAndQueryExpression("IsDel=false");
            var viewUserList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteAuthorizationOperateDecorate(userId, viewUserList == null ? null : viewUserList.Data, isSupressLazyLoad);
            return viewUserList;
        }
    }
}
