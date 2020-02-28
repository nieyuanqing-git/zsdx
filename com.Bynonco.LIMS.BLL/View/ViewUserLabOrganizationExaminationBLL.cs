using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewUserLabOrganizationExaminationBLL : BLLBase<ViewUserLabOrganizationExamination>, IViewUserLabOrganizationExaminationBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IUserExaminationBLL __userExaminationBLL;
        public ViewUserLabOrganizationExaminationBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __userExaminationBLL = BLLFactory.CreateUserExaminationBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewUserLabOrganizationExamination> viewUserLabOrganizationExaminationList, bool isSupressLazyLoad)
        {
            if (viewUserLabOrganizationExaminationList == null || viewUserLabOrganizationExaminationList.Count == 0) return;
            var userLabOrganizationExaminationPriviligeList = (IList<UserLabOrganizationExaminationPrivilige>)__userExaminationBLL.GetUserLabOrganizationExaminationPriviliges(userId, viewUserLabOrganizationExaminationList);
            foreach (var viewUserLabOrganizationExamination in viewUserLabOrganizationExaminationList)
            {
                UserLabOrganizationExaminationPrivilige userLabOrganizationExaminationPrivilige = userLabOrganizationExaminationPriviligeList.FirstOrDefault(P => P.UserExaminationId.HasValue && P.UserExaminationId == viewUserLabOrganizationExamination.Id);
                viewUserLabOrganizationExamination.IsSupressLazyLoad = false;
                viewUserLabOrganizationExamination.InitOperate();
                OperateDecorate(userId, viewUserLabOrganizationExamination, userLabOrganizationExaminationPrivilige);
                viewUserLabOrganizationExamination.BuildOperate();
                viewUserLabOrganizationExamination.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewUserLabOrganizationExamination viewUserLabOrganizationExamination, UserLabOrganizationExaminationPrivilige userLabOrganizationExaminationPrivilige)
        {
            if (viewUserLabOrganizationExamination == null) throw new ArgumentException("考试记录为空");
            if (userLabOrganizationExaminationPrivilige == null)
            {
                viewUserLabOrganizationExamination.ViewDetailOperate = "";
                viewUserLabOrganizationExamination.EditOperate = "";
                viewUserLabOrganizationExamination.DeleteOperate = "";
                viewUserLabOrganizationExamination.PrintOperate = "";
                return;
            }
            if (!userLabOrganizationExaminationPrivilige.IsEnableView)
            {
                viewUserLabOrganizationExamination.ViewDetailOperate = "";
            }
            var viewUserLabOrganizationExaminations = new ViewUserLabOrganizationExamination[] { viewUserLabOrganizationExamination };
            if (!userLabOrganizationExaminationPrivilige.IsEnableEdit)
            {
                viewUserLabOrganizationExamination.EditOperate = "";
            }
            if (!userLabOrganizationExaminationPrivilige.IsEnableDelete)
            {
                viewUserLabOrganizationExamination.DeleteOperate = "";
            }
            if (!userLabOrganizationExaminationPrivilige.IsEnablePrint || !viewUserLabOrganizationExamination.IsPass.HasValue || !viewUserLabOrganizationExamination.IsPass.Value)
            {
                viewUserLabOrganizationExamination.PrintOperate = "";
            }
        }
        public IList<ViewUserLabOrganizationExamination> GetViewUserLabOrganizationExaminationListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUserLabOrganizationExamination(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserLabOrganizationExaminationList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserLabOrganizationExaminationList, isSupressLazyLoad);
            return viewUserLabOrganizationExaminationList;
        }
        public IList<ViewUserLabOrganizationExamination> GetViewUserLabOrganizationExaminationListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUserLabOrganizationExamination(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserLabOrganizationExaminationList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserLabOrganizationExaminationList, isSupressLazyLoad);
            return viewUserLabOrganizationExaminationList;
        }
        public IList<ViewUserLabOrganizationExamination> GetViewUserLabOrganizationExaminationListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageUserLabOrganizationExamination(userId, ref dataGridSettings)) return null;
            var viewUserLabOrganizationExaminationList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserLabOrganizationExaminationList, isSupressLazyLoad);
            return viewUserLabOrganizationExaminationList;
        }
        public GridData<ViewUserLabOrganizationExamination> GetGridViewUserLabOrganizationExaminationListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUserLabOrganizationExamination(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserLabOrganizationExaminationList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserLabOrganizationExaminationList == null ? null : viewUserLabOrganizationExaminationList.Data, isSupressLazyLoad);
            return viewUserLabOrganizationExaminationList;
        }
        public GridData<ViewUserLabOrganizationExamination> GetGridViewUserLabOrganizationExaminationListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageUserLabOrganizationExamination(userId, ref dataGridSettings)) return null;
            var viewUserLabOrganizationExaminationList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserLabOrganizationExaminationList == null ? null : viewUserLabOrganizationExaminationList.Data, isSupressLazyLoad);
            return viewUserLabOrganizationExaminationList;
        }
        private bool IsManageUserLabOrganizationExamination(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
                dataGridSettings.AppendAndQueryExpression(__workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.UserLabOrganizationExamination, "UserExamination", "LabOrganizationExaminationContainer", "TrainningTypeOrgId"), null, null));//new ManagerEquipmentOrgId(ModuleBusinessType.UserExamination, "", "", "EquipmentOrgId"), null)
            return true;
        }
    }
}
