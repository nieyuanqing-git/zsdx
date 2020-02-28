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
    public class ViewUserEquipmentExaminationBLL : BLLBase<ViewUserEquipmentExamination>, IViewUserEquipmentExaminationBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IUserExaminationBLL __userExaminationBLL;
        public ViewUserEquipmentExaminationBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __userExaminationBLL = BLLFactory.CreateUserExaminationBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewUserEquipmentExamination> viewUserEquipmentExaminationList, bool isSupressLazyLoad)
        {
            if (viewUserEquipmentExaminationList == null || viewUserEquipmentExaminationList.Count == 0) return;
            var userEquipmentExaminationPriviligeList = (IList<UserEquipmentExaminationPrivilige>)__userExaminationBLL.GetUserEquipmentExaminationPriviliges(userId, viewUserEquipmentExaminationList);
            foreach (var viewUserEquipmentExamination in viewUserEquipmentExaminationList)
            {
                UserEquipmentExaminationPrivilige userEquipmentExaminationPrivilige = userEquipmentExaminationPriviligeList.FirstOrDefault(P => P.UserExaminationId.HasValue && P.UserExaminationId == viewUserEquipmentExamination.Id);
                viewUserEquipmentExamination.IsSupressLazyLoad = false;
                viewUserEquipmentExamination.InitOperate();
                OperateDecorate(userId, viewUserEquipmentExamination, userEquipmentExaminationPrivilige);
                viewUserEquipmentExamination.BuildOperate();
                viewUserEquipmentExamination.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewUserEquipmentExamination viewUserEquipmentExamination, UserEquipmentExaminationPrivilige userEquipmentExaminationPrivilige)
        {
            if (viewUserEquipmentExamination == null) throw new ArgumentException("考试记录为空");
            if (userEquipmentExaminationPrivilige == null)
            {
                viewUserEquipmentExamination.ViewDetailOperate = "";
                viewUserEquipmentExamination.EditOperate = "";
                viewUserEquipmentExamination.DeleteOperate = "";
                viewUserEquipmentExamination.PrintOperate = "";
                return;
            }
            if (!userEquipmentExaminationPrivilige.IsEnableView)
            {
                viewUserEquipmentExamination.ViewDetailOperate = "";
            }
            var viewUserEquipmentExaminations = new ViewUserEquipmentExamination[] { viewUserEquipmentExamination };
            if (!userEquipmentExaminationPrivilige.IsEnableEdit)
            {
                viewUserEquipmentExamination.EditOperate = "";
            }
            if (!userEquipmentExaminationPrivilige.IsEnableDelete)
            {
                viewUserEquipmentExamination.DeleteOperate = "";
            }
            if (!userEquipmentExaminationPrivilige.IsEnablePrint || !viewUserEquipmentExamination.IsPass.HasValue || !viewUserEquipmentExamination.IsPass.Value)
            {
                viewUserEquipmentExamination.PrintOperate = "";
            }
        }
        public IList<ViewUserEquipmentExamination> GetViewUserEquipmentExaminationListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUserEquipmentExamination(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserEquipmentExaminationList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentExaminationList, isSupressLazyLoad);
            return viewUserEquipmentExaminationList;
        }
        public IList<ViewUserEquipmentExamination> GetViewUserEquipmentExaminationListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUserEquipmentExamination(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserEquipmentExaminationList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentExaminationList, isSupressLazyLoad);
            return viewUserEquipmentExaminationList;
        }
        public IList<ViewUserEquipmentExamination> GetViewUserEquipmentExaminationListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageUserEquipmentExamination(userId, ref dataGridSettings)) return null;
            var viewUserEquipmentExaminationList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentExaminationList, isSupressLazyLoad);
            return viewUserEquipmentExaminationList;
        }
        public GridData<ViewUserEquipmentExamination> GetGridViewUserEquipmentExaminationListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageUserEquipmentExamination(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewUserEquipmentExaminationList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentExaminationList == null ? null : viewUserEquipmentExaminationList.Data, isSupressLazyLoad);
            return viewUserEquipmentExaminationList;
        }
        public GridData<ViewUserEquipmentExamination> GetGridViewUserEquipmentExaminationListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageUserEquipmentExamination(userId, ref dataGridSettings)) return null;
            var viewUserEquipmentExaminationList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUserEquipmentExaminationList == null ? null : viewUserEquipmentExaminationList.Data, isSupressLazyLoad);
            return viewUserEquipmentExaminationList;
        }
        private bool IsManageUserEquipmentExamination(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.UserEquipmentExamination, "", "", "TrainningTypeOrgId"), new ManagerEquipmentOrgId(ModuleBusinessType.UserEquipmentExamination, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
