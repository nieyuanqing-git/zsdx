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
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using System.Data;
using com.Bynonco.LIMS.DAL;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewUsedConfirmBLL : BLLBase<ViewUsedConfirm>, IViewUsedConfirmBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IUsedConfirmBLL __usedConfirmBLL;
        private Dictionary<string, string> __mapping = new Dictionary<string, string>();
        public ViewUsedConfirmBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
            __mapping["Id"] = "UsedConfirmId";
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewUsedConfirm> viewUsedConfirmList, bool isSupressLazyLoad)
        {
            if (viewUsedConfirmList == null || viewUsedConfirmList.Count == 0) return;
            var usedCofirmPriviligeList = (IList<UsedConfirmPrivilige>)__usedConfirmBLL.GetUsedConfirmPriviliges(userId, viewUsedConfirmList);
            foreach (var viewUsedConfirm in viewUsedConfirmList)
            {
                UsedConfirmPrivilige usedConfirmPrivilige = usedCofirmPriviligeList.FirstOrDefault(P => P.UsedConfirmId.HasValue && P.UsedConfirmId == viewUsedConfirm.Id);
                viewUsedConfirm.IsSupressLazyLoad = false;
                viewUsedConfirm.InitOperate();
                OperateDecorate(userId, viewUsedConfirm, usedConfirmPrivilige);
                viewUsedConfirm.BuildOperate();
                viewUsedConfirm.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewUsedConfirm viewUsedConfirm, UsedConfirmPrivilige usedConfirmPrivilige)
        {
            string errorMessage = " ";
            if (viewUsedConfirm == null) throw new ArgumentException("设备信息为空");
            if (usedConfirmPrivilige == null)
            {
                viewUsedConfirm.ViewDetailOperate = "";
                viewUsedConfirm.EditOperate = "";
                viewUsedConfirm.DeleteOperate = "";
                viewUsedConfirm.AddFeedBackOperate = "";
                viewUsedConfirm.ChangeUserOperate = "";
                return;
            }
            if (!usedConfirmPrivilige.IsEnableViewUsedConfirmDetail)
            {
                viewUsedConfirm.ViewDetailOperate = "";
            }
            if (!usedConfirmPrivilige.IsEnableEditFeedBack || viewUsedConfirm.StatusEnum == UsedConfirmStatus.UnComplete)
            {
                viewUsedConfirm.AddFeedBackOperate = "";
            }
            var lstUsedConfirmPrivilige = new List<UsedConfirmPrivilige>() { usedConfirmPrivilige };
            var viewUsedConfirms = new ViewUsedConfirm[] { viewUsedConfirm };
            if (!__usedConfirmBLL.JudgeIsEnableEditUsedConfirm(userId, viewUsedConfirms, lstUsedConfirmPrivilige, out errorMessage))
            {
                viewUsedConfirm.EditOperate = "";
            }
            if (!__usedConfirmBLL.JudgeIsEnableDeleteUsedConfirm(userId, viewUsedConfirms, lstUsedConfirmPrivilige, out errorMessage))
            {
                viewUsedConfirm.DeleteOperate = "";
            }
            if (!usedConfirmPrivilige.IsEnableChangeUsedConfirmUser) viewUsedConfirm.ChangeUserOperate = "";
            if (viewUsedConfirm.StatusEnum != UsedConfirmStatus.Complete && viewUsedConfirm.StatusEnum != UsedConfirmStatus.Deduct) viewUsedConfirm.ChangeUserOperate = "";
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
            if (isManageList && !IsManageUsedConfirm(userId, ref dataGridSettings, controllerName, actionName)) return "Id=null";
            return dataGridSettings.QueryExpression;
        }
        public string GenerateQueryExpression(Guid? userId, string expression, bool isManageList, string controllerName, string actionName)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GenerateQueryExpression(userId, dataGridSettings, isManageList, controllerName, actionName);
        }
        public IList<ViewUsedConfirm> GetViewUsedConfirmListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            expression = GenerateQueryExpression(userId, expression, isManageList, "", "");
            var viewUsedConfirmList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUsedConfirmList, isSupressLazyLoad);
            return viewUsedConfirmList;
        }
        public IList<ViewUsedConfirm> GetViewUsedConfirmListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            expression = GenerateQueryExpression(userId, expression, isManageList, "", "");
            var viewUsedConfirmList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUsedConfirmList, isSupressLazyLoad);
            return viewUsedConfirmList;
        }
        public IList<ViewUsedConfirm> GetViewUsedConfirmListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            GenerateQueryExpression(userId, dataGridSettings, isManageList, "", "");
            var viewUsedConfirmList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUsedConfirmList, isSupressLazyLoad);
            return viewUsedConfirmList;
        }
        public GridData<ViewUsedConfirm> GetGridViewUsedConfirmListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            expression = GenerateQueryExpression(userId, expression, isManageList, "", "");
            var viewUsedConfirmList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUsedConfirmList == null ? null : viewUsedConfirmList.Data, isSupressLazyLoad);
            return viewUsedConfirmList;
        }
        public GridData<ViewUsedConfirm> GetGridViewUsedConfirmListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            GenerateQueryExpression(userId, dataGridSettings, isManageList, "", "");
            var viewUsedConfirmList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewUsedConfirmList == null ? null : viewUsedConfirmList.Data, isSupressLazyLoad);
            return viewUsedConfirmList;
        }
        public int GetViewUsedConfirmCountByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GetViewUsedConfirmCountByExpression(userId, dataGridSettings, mapping, isDistinct, outDistinctMapping, isManageList);
        }
        public int GetViewUsedConfirmCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            if (isManageList && !IsManageUsedConfirm(userId, ref dataGridSettings, "", "")) return 0;
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }
        public IList<ViewUsedConfirm> GetMyViewUsedConfirmListByExpression(Guid userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = false, bool isIniOperate = true)
        {
            dataGridSettings = GeneratePersonalQueryExpression(dataGridSettings, userId);
            return GetViewUsedConfirmListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isManageList, isIniOperate);
        }
        public GridData<ViewUsedConfirm> GetMyGridViewUsedConfirmListByExpression(Guid userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = false, bool isIniOperate = true)
        {
            dataGridSettings = GeneratePersonalQueryExpression(dataGridSettings, userId);
            var viewUsedConfirmList = GetGridViewUsedConfirmListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isManageList, isIniOperate);
            return viewUsedConfirmList;
        }
        private bool IsManageUsedConfirm(Guid? userId, ref DataGridSettings dataGridSettings, string controllerName, string actionName)
        {
            if (!userId.HasValue) return false;
            var userIdValue = userId.Value;
            var viewUserSystemSettingBLL = BLLFactory.CreateViewUserSystemSettingBLL();
            var viewUserSystemSetting = viewUserSystemSettingBLL.GetEntityByUserId(userIdValue);
            if (viewUserSystemSetting != null && !viewUserSystemSettingBLL.IsSuperAdminWorkGroup(viewUserSystemSetting))
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", userIdValue);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(userIdValue, new ManagerUserOrgId(ModuleBusinessType.UsedConfirm, controllerName, actionName, "UserOrgId"), new ManagerEquipmentOrgId(ModuleBusinessType.UsedConfirm, controllerName, actionName, "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            //var user = __userBLL.GetEntityById(userId.Value);
            //if (user == null) return false;
            //if (!user.IsSuperAdmin)
            //{
            //    var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
            //    var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.UsedConfirm, controllerName, actionName, "UserOrgId"), new ManagerEquipmentOrgId(ModuleBusinessType.UsedConfirm, controllerName, actionName, "EquipmentOrgId"), null);
            //    if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
            //    dataGridSettings.AppendAndQueryExpression(queryExpression);
            //}
            return true;
        }
        public override GridData<ViewUsedConfirm> GetGridEntitiesByExpression(string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var dataGridSettings = GenerateDataGridSetting(expression, orderExpression, pageindex, pageSize, __mapping);
            GridData<ViewUsedConfirm> gridData = new GridData<ViewUsedConfirm>();
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
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetUsedConfirmList", dataParams).Result;
            if (results != null && results.Count > 0)
            {
                IList<ViewUsedConfirm> viewUsedConfirms = new List<ViewUsedConfirm>();
                foreach (var result in results)
                {
                    ViewUsedConfirm viewUsedConfirm = new ViewUsedConfirm();
                    viewUsedConfirm.Id = new Guid(result[0].ToString());
                    viewUsedConfirm.UserId = new Guid(result[1].ToString());
                    viewUsedConfirm.UserName = result[2] != null ? result[2].ToString() : null;
                    viewUsedConfirm.PhoneNumber = result[3] != null ? result[3].ToString() : null;
                    viewUsedConfirm.UserOrgId = null;
                    if (result[4] != null && !string.IsNullOrWhiteSpace(result[4].ToString())) viewUsedConfirm.UserOrgId = new Guid(result[4].ToString());
                    viewUsedConfirm.TutorId = null;
                    if (result[5] != null && !string.IsNullOrWhiteSpace(result[5].ToString())) viewUsedConfirm.TutorId = new Guid(result[5].ToString());
                    viewUsedConfirm.TutorName = result[6] != null ? result[6].ToString() : null;
                    viewUsedConfirm.AppointmentId = null;
                    if (result[7] != null && !string.IsNullOrWhiteSpace(result[7].ToString())) viewUsedConfirm.AppointmentId = new Guid(result[7].ToString());
                    viewUsedConfirm.SubjectId = null;
                    if (result[8] != null && !string.IsNullOrWhiteSpace(result[8].ToString())) viewUsedConfirm.SubjectId = new Guid(result[8].ToString());
                    viewUsedConfirm.SubjectName = result[9] != null ? result[9].ToString() : null;
                    viewUsedConfirm.EquipmentId = new Guid(result[10].ToString());
                    viewUsedConfirm.EquipmentName = result[11] != null ? result[11].ToString() : null;
                    viewUsedConfirm.RelativePic = result[12] != null ? result[12].ToString() : null;
                    viewUsedConfirm.EquipmentOrgId = null;
                    if (result[13] != null && !string.IsNullOrWhiteSpace(result[13].ToString())) viewUsedConfirm.EquipmentOrgId = new Guid(result[13].ToString());
                    viewUsedConfirm.LabRoomName = result[14] != null ? result[14].ToString() : null;
                    viewUsedConfirm.LabRoomXPath = result[15] != null ? result[15].ToString() : null;
                    viewUsedConfirm.Status = int.Parse(result[16].ToString());
                    viewUsedConfirm.BeginAt = null;
                    if (result[17] != null && !string.IsNullOrWhiteSpace(result[17].ToString())) viewUsedConfirm.BeginAt = DateTime.Parse(result[17].ToString());
                    viewUsedConfirm.EndAt = null;
                    if (result[18] != null && !string.IsNullOrWhiteSpace(result[18].ToString())) viewUsedConfirm.EndAt = DateTime.Parse(result[18].ToString());
                    viewUsedConfirm.RealFee = double.Parse(result[19].ToString());
                    viewUsedConfirm.CalcFee = double.Parse(result[20].ToString());
                    viewUsedConfirm.CalcDuration = double.Parse(result[21].ToString());
                    viewUsedConfirm.CreateAt = DateTime.Parse(result[22].ToString());
                    viewUsedConfirm.CreaterId = null;
                    if (result[23] != null && !string.IsNullOrWhiteSpace(result[23].ToString())) viewUsedConfirm.CreaterId = new Guid(result[23].ToString());
                    viewUsedConfirm.CreaterName = result[24] != null ? result[24].ToString() : null;
                    viewUsedConfirm.PaymentMethod = int.Parse(result[25].ToString());
                    viewUsedConfirm.UnitPrice = double.Parse(result[26].ToString());
                    viewUsedConfirm.ChargeType = null;
                    if (result[27] != null && !string.IsNullOrWhiteSpace(result[27].ToString())) viewUsedConfirm.ChargeType = int.Parse(result[27].ToString());
                    viewUsedConfirm.IsFeedBack = null;
                    if (result[28] != null && !string.IsNullOrWhiteSpace(result[28].ToString())) viewUsedConfirm.IsFeedBack = bool.Parse(result[28].ToString());
                    viewUsedConfirm.SubjectProjectId = null;
                    if (result[29] != null && !string.IsNullOrWhiteSpace(result[29].ToString())) viewUsedConfirm.SubjectProjectId = new Guid(result[29].ToString());
                    viewUsedConfirm.SubjectProjectName = result[30] != null ? result[30].ToString() : null;
                    viewUsedConfirm.SampleCount = null;
                    if (result[31] != null && !string.IsNullOrWhiteSpace(result[31].ToString())) viewUsedConfirm.SampleCount = int.Parse(result[31].ToString());
                    viewUsedConfirm.Remark = result[32] != null ? result[32].ToString() : null;
                    viewUsedConfirm.SubjectRelativePic = result[33] != null ? result[33].ToString() : null;
                    viewUsedConfirm.UserTypeName = result[35] != null ? result[35].ToString() : null;
                    viewUsedConfirm.IsSupressLazyLoad = isSupressLazyLoad;
                    viewUsedConfirms.Add(viewUsedConfirm);
                }
                gridData.Data = viewUsedConfirms;
            }
            gridData.Count = returnValueDataParam.Value == null ? 0 : int.Parse(returnValueDataParam.Value.ToString());
            return gridData;
        }

        public IDictionary<EquipmentRunConditionSumTotal, IList<EquipmentRunConditionSumTotal>> GenerateEquipmentRunConditionSumTotalData(Guid userId, DataGridSettings dataGridSettings, bool isManageList = true)
        {
            IList<EquipmentRunConditionSumTotal> totals = new List<EquipmentRunConditionSumTotal>();
            IDictionary<EquipmentRunConditionSumTotal, IList<EquipmentRunConditionSumTotal>> datas = new Dictionary<EquipmentRunConditionSumTotal, IList<EquipmentRunConditionSumTotal>>();
            dataGridSettings.QueryExpression = GenerateQueryExpression(userId, dataGridSettings, isManageList, "UsedConfirm", "EquipmentRunConditionSumTotalIndex");
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            var queryExpressionDataParam = DataParameterFactory.CreateDataParameter("queryExpression", sqlStr);
            var orderClomnNameDataParam = DataParameterFactory.CreateDataParameter("orderClomnName", dataGridSettings.SortColumn);
            var orderTypeDataParam = DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr());
            var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
            List<IDataParameter> dataParams = new List<IDataParameter>()
             {
                 queryExpressionDataParam,
                 orderClomnNameDataParam,
                 orderTypeDataParam,
                 returnValueDataParam
             };
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetEquipmentRunConditionSumTotal", dataParams).Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    EquipmentRunConditionSumTotal equipmentRunConditionSumTotal = new EquipmentRunConditionSumTotal();
                    equipmentRunConditionSumTotal.EquipmentId = new Guid(result[0].ToString());
                    equipmentRunConditionSumTotal.EquipmentName = result[1].ToString();
                    equipmentRunConditionSumTotal.RelativePic = result[2].ToString();
                    if (result[2] != null && !string.IsNullOrWhiteSpace(result[3].ToString())) equipmentRunConditionSumTotal.TotalUseTimes = int.Parse(result[3].ToString());
                    if (result[3] != null && !string.IsNullOrWhiteSpace(result[4].ToString())) equipmentRunConditionSumTotal.TotalDuration = Math.Round(double.Parse(result[4].ToString()), 2);
                    if (result[4] != null && !string.IsNullOrWhiteSpace(result[5].ToString())) equipmentRunConditionSumTotal.TotalCalcDuration = Math.Round(double.Parse(result[5].ToString()));
                    if (result[5] != null && !string.IsNullOrWhiteSpace(result[6].ToString())) equipmentRunConditionSumTotal.TotalUserCount = int.Parse(result[6].ToString());
                    totals.Add(equipmentRunConditionSumTotal);
                }

            }
            var footer = new EquipmentRunConditionSumTotal
            {
                EquipmentId = Guid.Empty,
                EquipmentName = totals.Count.ToString(),
                TotalUseTimes = totals.Sum(p => p.TotalUseTimes),
                TotalDuration = Math.Round(totals.Sum(p => p.TotalDuration), 2),
                TotalUserCount = (int)returnValueDataParam.Value,
                TotalCalcDuration = Math.Round(totals.Sum(p => p.TotalCalcDuration), 2),
            };
            datas[footer] = totals;
            return datas;
        }
        public IDictionary<EquipmentSubjectRunConditionSumTotal, IList<EquipmentSubjectRunConditionSumTotal>> GenerateEquipmentSubjectRunConditionSumTotalData(Guid userId, DataGridSettings dataGridSettings, bool isManageList = true)
        {
            IList<EquipmentSubjectRunConditionSumTotal> totals = new List<EquipmentSubjectRunConditionSumTotal>();
            IDictionary<EquipmentSubjectRunConditionSumTotal, IList<EquipmentSubjectRunConditionSumTotal>> datas = new Dictionary<EquipmentSubjectRunConditionSumTotal, IList<EquipmentSubjectRunConditionSumTotal>>();
            dataGridSettings.QueryExpression = GenerateQueryExpression(userId, dataGridSettings, isManageList, "UsedConfirm", "EquipmentRunConditionSumTotalIndex");
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            var queryExpressionDataParam = DataParameterFactory.CreateDataParameter("queryExpression", sqlStr);
            var orderClomnNameDataParam = DataParameterFactory.CreateDataParameter("orderClomnName", dataGridSettings.SortColumn);
            var orderTypeDataParam = DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr());
            var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
            List<IDataParameter> dataParams = new List<IDataParameter>()
             {
                 queryExpressionDataParam,
                 orderClomnNameDataParam,
                 orderTypeDataParam,
                 returnValueDataParam
             };
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetEquipmentSubjectRunConditionSumTotal", dataParams).Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    EquipmentSubjectRunConditionSumTotal equipmentSubjectRunConditionSumTotal = new EquipmentSubjectRunConditionSumTotal();
                    equipmentSubjectRunConditionSumTotal.SubjectId = new Guid(result[0].ToString());
                    equipmentSubjectRunConditionSumTotal.SubjectName = result[1].ToString();
                    equipmentSubjectRunConditionSumTotal.RelativePic = result[2].ToString();
                    if (result[2] != null && !string.IsNullOrWhiteSpace(result[3].ToString())) equipmentSubjectRunConditionSumTotal.TotalUseTimes = int.Parse(result[3].ToString());
                    if (result[3] != null && !string.IsNullOrWhiteSpace(result[4].ToString())) equipmentSubjectRunConditionSumTotal.TotalDuration = Math.Round(double.Parse(result[4].ToString()), 2);
                    if (result[4] != null && !string.IsNullOrWhiteSpace(result[5].ToString())) equipmentSubjectRunConditionSumTotal.TotalCalcDuration = Math.Round(double.Parse(result[5].ToString()));
                    if (result[5] != null && !string.IsNullOrWhiteSpace(result[6].ToString())) equipmentSubjectRunConditionSumTotal.TotalUserCount = int.Parse(result[6].ToString());
                    totals.Add(equipmentSubjectRunConditionSumTotal);
                }

            }
            var footer = new EquipmentSubjectRunConditionSumTotal
            {
                SubjectId = Guid.Empty,
                SubjectName = totals.Count.ToString(),
                TotalUseTimes = totals.Sum(p => p.TotalUseTimes),
                TotalDuration = Math.Round(totals.Sum(p => p.TotalDuration), 2),
                TotalUserCount = (int)returnValueDataParam.Value,
                TotalCalcDuration = Math.Round(totals.Sum(p => p.TotalCalcDuration), 2),
            };
            datas[footer] = totals;
            return datas;
        }

    }
}
