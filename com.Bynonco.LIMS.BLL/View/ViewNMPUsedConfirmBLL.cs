using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.Logic.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewNMPUsedConfirmBLL : BLLBase<ViewNMPUsedConfirm>, IViewNMPUsedConfirmBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private INMPUsedConfirmBLL __nmpUsedConfirmBLL;
        private Dictionary<string, string> __mapping = new Dictionary<string, string>();
        public ViewNMPUsedConfirmBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __nmpUsedConfirmBLL = BLLFactory.CreateNMPUsedConfirmBLL();
            __mapping["Id"] = "NMPUsedConfirmId";
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewNMPUsedConfirm> viewNMPUsedConfirmList, bool isSupressLazyLoad)
        {
            if (viewNMPUsedConfirmList == null || viewNMPUsedConfirmList.Count == 0) return;
            var nmpUsedCofirmPriviligeList = (IList<NMPUsedConfirmPrivilige>)__nmpUsedConfirmBLL.GetNMPUsedConfirmPriviliges(userId, viewNMPUsedConfirmList);
            foreach (var viewNMPUsedConfirm in viewNMPUsedConfirmList)
            {
                NMPUsedConfirmPrivilige nmpUsedConfirmPrivilige = nmpUsedCofirmPriviligeList.FirstOrDefault(P => P.NMPUsedConfirmId.HasValue && P.NMPUsedConfirmId == viewNMPUsedConfirm.Id);
                viewNMPUsedConfirm.IsSupressLazyLoad = false;
                viewNMPUsedConfirm.InitOperate();
                OperateDecorate(userId, viewNMPUsedConfirm, nmpUsedConfirmPrivilige);
                viewNMPUsedConfirm.BuildOperate();
                viewNMPUsedConfirm.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewNMPUsedConfirm viewNMPUsedConfirm, NMPUsedConfirmPrivilige nmpUsedConfirmPrivilige)
        {
            string errorMessage = " ";
            if (viewNMPUsedConfirm == null) throw new ArgumentException("工位信息为空");
            if (nmpUsedConfirmPrivilige == null)
            {
                viewNMPUsedConfirm.ViewDetailOperate = "";
                viewNMPUsedConfirm.EditOperate = "";
                viewNMPUsedConfirm.DeleteOperate = "";
                viewNMPUsedConfirm.AddFeedBackOperate = "";
                viewNMPUsedConfirm.ChangeUserOperate = "";
                return;
            }
            if (!nmpUsedConfirmPrivilige.IsEnableViewNMPUsedConfirmDetail)
            {
                viewNMPUsedConfirm.ViewDetailOperate = "";
            }
            if (!nmpUsedConfirmPrivilige.IsEnableEditNMPFeedBack || viewNMPUsedConfirm.StatusEnum == UsedConfirmStatus.UnComplete)
            {
                viewNMPUsedConfirm.AddFeedBackOperate = "";
            }
            var lstNMPUsedConfirmPrivilige = new List<NMPUsedConfirmPrivilige>() { nmpUsedConfirmPrivilige };
            var viewNMPUsedConfirms = new ViewNMPUsedConfirm[] { viewNMPUsedConfirm };
            if (!__nmpUsedConfirmBLL.JudgeIsEnableEditNMPUsedConfirm(userId, viewNMPUsedConfirms, lstNMPUsedConfirmPrivilige, out errorMessage))
            {
                viewNMPUsedConfirm.EditOperate = "";
            }
            if (!__nmpUsedConfirmBLL.JudgeIsEnableDeleteNMPUsedConfirm(userId, viewNMPUsedConfirms, lstNMPUsedConfirmPrivilige, out errorMessage))
            {
                viewNMPUsedConfirm.DeleteOperate = "";
            }
            if (!nmpUsedConfirmPrivilige.IsEnableChangeNMPUsedConfirmUser) viewNMPUsedConfirm.ChangeUserOperate = "";
            if (viewNMPUsedConfirm.StatusEnum != UsedConfirmStatus.Complete && viewNMPUsedConfirm.StatusEnum != UsedConfirmStatus.Deduct) viewNMPUsedConfirm.ChangeUserOperate = "";
        }
        private DataGridSettings GeneratePersonalQueryExpression(DataGridSettings dataGridSettings, Guid userId)
        {
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            var subjectList = subjectBLL.GetSubjectListByDirectorId(userId);
            if (subjectList != null && subjectList.Count > 0)
            {
                dataGridSettings.AppendAndQueryExpression(string.Format("({0}+UserId=\"{1}\")", subjectList.Select(p => p.Id).ToFormatStr("SubjectId"), userId));
            }
            else
            {
                dataGridSettings.AppendAndQueryExpression(string.Format("UserId=\"{0}\"", userId));
            }
            return dataGridSettings;
        }
        public string GenerateQueryExpression(Guid? userId, DataGridSettings dataGridSettings, bool isManageList, string controllerName, string actionName)
        {
            if (isManageList && !IsManageNMPUsedConfirm(userId, ref dataGridSettings, controllerName, actionName)) return "Id=null";
            return dataGridSettings.QueryExpression;
        }
        public string GenerateQueryExpression(Guid? userId, string expression, bool isManageList, string controllerName, string actionName)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GenerateQueryExpression(userId, dataGridSettings, isManageList, controllerName, actionName);
        }
        public IList<ViewNMPUsedConfirm> GetViewNMPUsedConfirmListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            expression = GenerateQueryExpression(userId, expression, isManageList, "", "");
            var viewUsedConfirmList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUsedConfirmList, isSupressLazyLoad);
            return viewUsedConfirmList;
        }
        public IList<ViewNMPUsedConfirm> GetViewNMPUsedConfirmListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            expression = GenerateQueryExpression(userId, expression, isManageList, "", "");
            var viewNMPUsedConfirmList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewNMPUsedConfirmList, isSupressLazyLoad);
            return viewNMPUsedConfirmList;
        }
        public IList<ViewNMPUsedConfirm> GetViewNMPUsedConfirmListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            GenerateQueryExpression(userId, dataGridSettings, isManageList, "", "");
            var viewNMPUsedConfirmList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewNMPUsedConfirmList, isSupressLazyLoad);
            return viewNMPUsedConfirmList;
        }
        public GridData<ViewNMPUsedConfirm> GetGridViewNMPUsedConfirmListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            expression = GenerateQueryExpression(userId, expression, isManageList, "", "");
            var viewUsedConfirmList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUsedConfirmList == null ? null : viewUsedConfirmList.Data, isSupressLazyLoad);
            return viewUsedConfirmList;
        }
        public GridData<ViewNMPUsedConfirm> GetGridViewNMPUsedConfirmListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            GenerateQueryExpression(userId, dataGridSettings, isManageList, "", "");
            var viewNMPUsedConfirmList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewNMPUsedConfirmList == null ? null : viewNMPUsedConfirmList.Data, isSupressLazyLoad);
            return viewNMPUsedConfirmList;
        }
        public int GetViewNMPUsedConfirmCountByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GetViewNMPUsedConfirmCountByExpression(userId, dataGridSettings, mapping, isDistinct, outDistinctMapping, isManageList);
        }
        public int GetViewNMPUsedConfirmCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            if (isManageList && !IsManageNMPUsedConfirm(userId, ref dataGridSettings, "", "")) return 0;
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }
        public IList<ViewNMPUsedConfirm> GetMyViewNMPUsedConfirmListByExpression(Guid userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = false, bool isIniOperate = true)
        {
            dataGridSettings = GeneratePersonalQueryExpression(dataGridSettings, userId);
            return GetViewNMPUsedConfirmListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isManageList, isIniOperate);
        }
        public GridData<ViewNMPUsedConfirm> GetMyGridViewNMPUsedConfirmListByExpression(Guid userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = false, bool isIniOperate = true)
        {
            dataGridSettings = GeneratePersonalQueryExpression(dataGridSettings, userId);
            var viewNMPUsedConfirmList = GetGridViewNMPUsedConfirmListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isManageList, isIniOperate);
            return viewNMPUsedConfirmList;
        }
        private bool IsManageNMPUsedConfirm(Guid? userId, ref DataGridSettings dataGridSettings, string controllerName, string actionName)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("NMPAdminId=\"{0}\"", user.Id);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.NMPUsedConfirm, controllerName, actionName, "UserOrgId"), null, new ManagerNMPOrgId(ModuleBusinessType.NMPUsedConfirm, controllerName, actionName, "NMPOrgId"));
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
        public override GridData<ViewNMPUsedConfirm> GetGridEntitiesByExpression(string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var dataGridSettings = GenerateDataGridSetting(expression, orderExpression, pageindex, pageSize, __mapping);
            GridData<ViewNMPUsedConfirm> gridData = new GridData<ViewNMPUsedConfirm>();
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            var queryExpressionDataParam = DataParameterFactory.CreateDataParameter("queryExpression", sqlStr);
            var orderClomnNameDataParam = DataParameterFactory.CreateDataParameter("orderClomnName", dataGridSettings.SortColumn);
            var orderTypeDataParam = DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr());
            var pageIndexDataParam = DataParameterFactory.CreateDataParameter("pageIndex", dataGridSettings.PageIndex);
            var pageCountDataParam = DataParameterFactory.CreateDataParameter("pageCount", dataGridSettings.PageSize);
            var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
            List<IDataParameter> dataParams = new List<IDataParameter>()
             {
                 queryExpressionDataParam,
                 orderClomnNameDataParam,
                 orderTypeDataParam,
                 pageIndexDataParam,
                 pageCountDataParam,
                 returnValueDataParam
             };
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetNMPUsedConfirmList", dataParams).Result;
            if (results != null && results.Count > 0)
            {
                IList<ViewNMPUsedConfirm> viewNMPUsedConfirms = new List<ViewNMPUsedConfirm>();
                foreach (var result in results)
                {
                    ViewNMPUsedConfirm viewNMPUsedConfirm = new ViewNMPUsedConfirm();
                    viewNMPUsedConfirm.Id = new Guid(result[0].ToString());
                    viewNMPUsedConfirm.UserId = new Guid(result[1].ToString());
                    viewNMPUsedConfirm.UserName = result[2] != null ? result[2].ToString() : null;
                    viewNMPUsedConfirm.PhoneNumber = result[3] != null ? result[3].ToString() : null;
                    viewNMPUsedConfirm.UserOrgId = null;
                    if (result[4] != null && !string.IsNullOrWhiteSpace(result[4].ToString())) viewNMPUsedConfirm.UserOrgId = new Guid(result[4].ToString());
                    viewNMPUsedConfirm.TutorId = null;
                    if (result[5] != null && !string.IsNullOrWhiteSpace(result[5].ToString())) viewNMPUsedConfirm.TutorId = new Guid(result[5].ToString());
                    viewNMPUsedConfirm.TutorName = result[6] != null ? result[6].ToString() : null;
                    viewNMPUsedConfirm.NMPAppointmentId = null;
                    if (result[7] != null && !string.IsNullOrWhiteSpace(result[7].ToString())) viewNMPUsedConfirm.NMPAppointmentId = new Guid(result[7].ToString());
                    viewNMPUsedConfirm.SubjectId = null;
                    if (result[8] != null && !string.IsNullOrWhiteSpace(result[8].ToString())) viewNMPUsedConfirm.SubjectId = new Guid(result[8].ToString());
                    viewNMPUsedConfirm.SubjectName = result[9] != null ? result[9].ToString() : null;
                    viewNMPUsedConfirm.NMPId = new Guid(result[10].ToString());
                    viewNMPUsedConfirm.NMPName = result[11] != null ? result[11].ToString() : null;
                    viewNMPUsedConfirm.NMPEquipmentId = new Guid(result[12].ToString());
                    viewNMPUsedConfirm.NMPEquipmentName = result[13] != null ? result[13].ToString() : null;
                    viewNMPUsedConfirm.NMPRelativePic = result[14] != null ? result[14].ToString() : null;
                    viewNMPUsedConfirm.NMPOrgId = null;
                    if (result[15] != null && !string.IsNullOrWhiteSpace(result[15].ToString())) viewNMPUsedConfirm.NMPOrgId = new Guid(result[15].ToString());
                    viewNMPUsedConfirm.LabRoomName = result[16] != null ? result[16].ToString() : null;
                    viewNMPUsedConfirm.LabRoomXPath = result[17] != null ? result[17].ToString() : null;
                    viewNMPUsedConfirm.Status = int.Parse(result[18].ToString());
                    viewNMPUsedConfirm.BeginAt = null;
                    if (result[19] != null && !string.IsNullOrWhiteSpace(result[19].ToString())) viewNMPUsedConfirm.BeginAt = DateTime.Parse(result[19].ToString());
                    viewNMPUsedConfirm.EndAt = null;
                    if (result[20] != null && !string.IsNullOrWhiteSpace(result[20].ToString())) viewNMPUsedConfirm.EndAt = DateTime.Parse(result[20].ToString());
                    viewNMPUsedConfirm.RealFee = double.Parse(result[21].ToString());
                    viewNMPUsedConfirm.CalcFee = double.Parse(result[22].ToString());
                    viewNMPUsedConfirm.CalcDuration = double.Parse(result[23].ToString());
                    viewNMPUsedConfirm.CreateAt = DateTime.Parse(result[24].ToString());
                    viewNMPUsedConfirm.CreaterId = null;
                    if (result[25] != null && !string.IsNullOrWhiteSpace(result[25].ToString())) viewNMPUsedConfirm.CreaterId = new Guid(result[25].ToString());
                    viewNMPUsedConfirm.CreaterName = result[26] != null ? result[26].ToString() : null;
                    viewNMPUsedConfirm.PaymentMethod = int.Parse(result[27].ToString());
                    viewNMPUsedConfirm.UnitPrice = double.Parse(result[28].ToString());
                    viewNMPUsedConfirm.ChargeType = null;
                    if (result[29] != null && !string.IsNullOrWhiteSpace(result[29].ToString())) viewNMPUsedConfirm.ChargeType = int.Parse(result[29].ToString());
                    viewNMPUsedConfirm.RoundFator = 0;
                    if (result[30] != null && !string.IsNullOrWhiteSpace(result[30].ToString())) viewNMPUsedConfirm.RoundFator = int.Parse(result[30].ToString());
                    viewNMPUsedConfirm.RoundDigits = 0;
                    if (result[31] != null && !string.IsNullOrWhiteSpace(result[31].ToString())) viewNMPUsedConfirm.RoundFator = int.Parse(result[31].ToString());
                    viewNMPUsedConfirm.IsFeedBack = false;
                    if (result[32] != null && !string.IsNullOrWhiteSpace(result[32].ToString())) viewNMPUsedConfirm.IsFeedBack = bool.Parse(result[32].ToString());
                    viewNMPUsedConfirm.NMPAdminId = null;
                    if (result[33] != null && !string.IsNullOrWhiteSpace(result[33].ToString())) viewNMPUsedConfirm.NMPAdminId = new Guid(result[33].ToString());
                    viewNMPUsedConfirm.SubjectProjectId = null;
                    if (result[34] != null && !string.IsNullOrWhiteSpace(result[34].ToString())) viewNMPUsedConfirm.SubjectProjectId = new Guid(result[34].ToString());
                    viewNMPUsedConfirm.SubjectProjectName = result[35] != null ? result[35].ToString() : null;
                    viewNMPUsedConfirm.Remark = result[36] != null ? result[36].ToString() : null;
                    viewNMPUsedConfirm.SubjectRelativePic = result[37] != null ? result[37].ToString() : null;
                    viewNMPUsedConfirm.IsSupressLazyLoad = isSupressLazyLoad;
                    viewNMPUsedConfirms.Add(viewNMPUsedConfirm);
                }
                gridData.Data = viewNMPUsedConfirms;
            }
            gridData.Count = returnValueDataParam.Value == null ? 0 : int.Parse(returnValueDataParam.Value.ToString());
            return gridData;
        }
    }
}
