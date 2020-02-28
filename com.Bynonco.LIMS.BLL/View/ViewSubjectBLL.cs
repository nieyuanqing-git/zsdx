using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewSubjectBLL : BLLBase<ViewSubject>, IViewSubjectBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewSubjectBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewSubject> viewSubjectList,bool isCooperative, bool isSupressLazyLoad)
        {
            if (viewSubjectList == null || viewSubjectList.Count == 0) return;
            foreach (var viewSubject in viewSubjectList)
            {
                viewSubject.IsSupressLazyLoad = false;
                viewSubject.IsCoopertive = isCooperative;
                viewSubject.InitOperate();
                OperateDecorate(userId, viewSubject,isCooperative);
                viewSubject.BuildOperate();
                viewSubject.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewSubject viewSubject,bool isCooperative)
        {
            if (viewSubject == null) throw new ArgumentException("用户信息为空");
            var userPrivilige = userId.HasValue ? PriviligeFactory.GetSubjectPrivilige(userId.Value, viewSubject.Id, isCooperative) : null;
            if (userPrivilige == null)
            {
                viewSubject.ModifyOperate = "";
                viewSubject.DeleteOperate = "";
                return;
            }
            if (!userPrivilige.IsEnableEdit)
                viewSubject.ModifyOperate = "";
            if (!userPrivilige.IsEnableDelete) 
                viewSubject.DeleteOperate = "";
        }
        public IList<ViewSubject> GetViewCooperativeSubjectList(Guid userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            IExperimenterSubjectBLL experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            List<ViewSubject> viewSubjectList = null;
            var subjectList = subjectBLL.GetUserCooperativeSubjects(userId);
            if (subjectList != null && subjectList.Count > 0)
            {
                viewSubjectList = new List<ViewSubject>();
                viewSubjectList.AddRange(subjectList.Select(p => p.GetViewSubject()));
                ExcuteOperateDecorate(userId, viewSubjectList,true, isSupressLazyLoad);
                foreach (var viewSubject in viewSubjectList)
                {
                    if ((!string.IsNullOrWhiteSpace(viewSubject.ModifyOperate) || !string.IsNullOrWhiteSpace(viewSubject.DeleteOperate)) && userId != viewSubject.SubjectDirectorId.Value)
                    {
                        var experimenterSubject = experimenterSubjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectId=\"{0}\"*ExperimenterId=\"{1}\"*IsDelete=false*IsAdmin=true*OwnerId=\"{2}\"",
                                                                                                             viewSubject.Id, userId, viewSubject.SubjectDirectorId.Value));
                        if (experimenterSubject == null)
                        {
                            viewSubject.ModifyOperate = "";
                            viewSubject.DeleteOperate = "";
                            viewSubject.BuildOperate();
                        }
                    }
                }
            }
            return viewSubjectList;
        }
        public IList<ViewSubject> GetViewSubjectListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageSubject(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewSubjectList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewSubjectList, false, isSupressLazyLoad);
            return viewSubjectList;
        }
        public IList<ViewSubject> GetViewSubjectListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageSubject(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewSubjectList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewSubjectList,false, isSupressLazyLoad);
            return viewSubjectList;
        }
        public IList<ViewSubject> GetViewSubjectListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageSubject(userId, ref dataGridSettings)) return null;
            var viewSubjectList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewSubjectList,false, isSupressLazyLoad);
            return viewSubjectList;
        }
        public GridData<ViewSubject> GetGridViewSubjectListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageSubject(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewSubjectList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewSubjectList == null ? null : viewSubjectList.Data,false, isSupressLazyLoad);
            return viewSubjectList;
        }
        public GridData<ViewSubject> GetGridViewSubjectListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageSubject(userId, ref dataGridSettings)) return null;
            var viewSubjectList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewSubjectList == null ? null : viewSubjectList.Data,false, isSupressLazyLoad);
            return viewSubjectList;
        }
        private bool IsManageSubject(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerUserOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.Subject, "", "", "OrgId"));
                var __subjectBLL = BLLFactory.CreateSubjectBLL();
                var userRelevantList = __subjectBLL.GetUserRelevantSubjects(user.Id);
                string queryExpression = "";
                if (userRelevantList != null && userRelevantList.Count() > 0)
                    queryExpression = string.Format("{1}", user.Id, userRelevantList.Select(p => p.Id).ToFormatStr());
                else
                    queryExpression = string.Format("SubjectDirectorId=\"{0}\"", user.Id);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
