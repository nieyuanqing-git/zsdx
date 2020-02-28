using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Business;
using com.august.Core.XQuery;
using System.Data;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;
namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewAppointmentBLL : BLLBase<ViewAppointment>, IViewAppointmentBLL
    {
        private IEquipmentBLL __equipmentBLL;
        private IAppointmentBLL __appointmentBLL;
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IDictCodeBLL __dictCodeBLL;
        private Dictionary<string, string> __mapping = new Dictionary<string, string>();
        private readonly string[] __outMapping = new string[] { "EquipmentAdminId" };
        public ViewAppointmentBLL()
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __appointmentBLL = BLLFactory.CreateAppointmentBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __mapping["Id"] = "AppointmentId";
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewAppointment> viewAppointmentList, bool isSupressLazyLoad)
        {
            if (viewAppointmentList == null || viewAppointmentList.Count == 0) return;
            IList<AppointmentPrivilige> lstAppointmentPriviliges = (IList<AppointmentPrivilige>)__appointmentBLL.GetAppointmentPriviliges(userId, viewAppointmentList);
            var equipmentList = __equipmentBLL.GetEntitiesByExpression(viewAppointmentList.Select(p => p.EquipmentId.Value).Distinct().ToFormatStr());
            foreach (var viewAppointment in viewAppointmentList)
            {
                AppointmentPrivilige appointmentPrivilige = lstAppointmentPriviliges.FirstOrDefault(p => p.AppointmentId.HasValue && p.AppointmentId.Value == viewAppointment.Id);
                viewAppointment.IsSupressLazyLoad = false;
                viewAppointment.InitOperate();
                OperateDecorate(userId, viewAppointment, appointmentPrivilige, equipmentList.First(p => p.Id == viewAppointment.EquipmentId.Value));
                viewAppointment.BuildOperate();
                viewAppointment.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewAppointment viewAppointment, AppointmentPrivilige appointmentPrivilige, Equipment equipment)
        {
            string errorMessage = "";
            if (viewAppointment == null) throw new ArgumentException("设备信息为空");
            if (appointmentPrivilige == null)
            {
                viewAppointment.CancelOperate = "";
                viewAppointment.ChangeOperate = "";
                viewAppointment.RegisterBreakAppointmentOperate = "";
                viewAppointment.CancelRegisterBreakAppointmentOperate = "";
                viewAppointment.TutorAuditAppointmentOperate = "";
                viewAppointment.AuditAppointmentOperate = "";
                viewAppointment.ViewAppointmentInfoOperate = "";
                viewAppointment.AddUsedConfirmOperate = "";
                viewAppointment.SelfAddUsedConfirmOperate = "";
                viewAppointment.PrintOperate = "";
                return;
            }
            if (!appointmentPrivilige.IsEnableAppiontmentDetailInfo)
            {
                viewAppointment.ViewAppointmentInfoOperate = "";
            }
            var viewAppointments = new ViewAppointment[] { viewAppointment };
            var lstAppointmentPrivilige = new List<AppointmentPrivilige> { appointmentPrivilige };
            var equipments = new Equipment[] { equipment };
            if (!__appointmentBLL.JudgeIsEnableCancel(viewAppointments, lstAppointmentPrivilige, equipments, userId.Value, out errorMessage))
            {
                viewAppointment.CancelOperate = "";
            }
            if (!__appointmentBLL.JudgeIsEnableChange(viewAppointments, lstAppointmentPrivilige, equipments, userId.Value, out errorMessage))
            {
                viewAppointment.ChangeOperate = "";
            }
            if (!__appointmentBLL.JudgeIsEnableAudit(false, null, viewAppointments, lstAppointmentPrivilige, equipments, userId.Value, out errorMessage))
            {
                viewAppointment.AuditAppointmentOperate = "";
            }
            if (!__appointmentBLL.JudgeIsEnableTutorAudit(false, null, viewAppointments, equipments, userId.Value, out errorMessage))
            {
                viewAppointment.TutorAuditAppointmentOperate = "";
            }
            if (!__appointmentBLL.JudgeIsEnableRegisterBreakAppointment(false, viewAppointments, lstAppointmentPrivilige, userId.Value, out errorMessage))
            {
                viewAppointment.RegisterBreakAppointmentOperate = "";
            }
            if (!__appointmentBLL.JudgeIsEnableCancelRegisterBreakAppointment(viewAppointments, lstAppointmentPrivilige, userId.Value, out errorMessage))
            {
                viewAppointment.CancelRegisterBreakAppointmentOperate = "";
            }
            if (!__appointmentBLL.JudgeIsEnableAddUsedConfirm(viewAppointments, lstAppointmentPrivilige, userId.Value, out errorMessage))
            {
                viewAppointment.AddUsedConfirmOperate = "";
            }

            if (!appointmentPrivilige.IsEnablePrint)
            {
                viewAppointment.PrintOperate = "";
            }
            if (!__appointmentBLL.JudgeIsEnableSelfAddUsedConfirm(viewAppointments, lstAppointmentPrivilige, userId.Value, out errorMessage))
            {
                viewAppointment.SelfAddUsedConfirmOperate = "";
            }
            if (!string.IsNullOrWhiteSpace(viewAppointment.AddUsedConfirmOperate) || viewAppointment.UserId != userId)
            {
                viewAppointment.SelfAddUsedConfirmOperate = "";
            }
        }
        public IList<ViewAppointment> GetViewAppointmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageAppointment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewAppointmentList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewAppointmentList, isSupressLazyLoad);
            return viewAppointmentList;
        }
        public IList<ViewAppointment> GetViewAppointmentListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageAppointment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewAppointmentList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewAppointmentList, isSupressLazyLoad);
            return viewAppointmentList;
        }
        public IList<ViewAppointment> GetViewAppointmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageAppointment(userId, ref dataGridSettings)) return null;
            var viewAppointmentList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewAppointmentList, isSupressLazyLoad);
            return viewAppointmentList;
        }
        public GridData<ViewAppointment> GetGridViewAppointmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageAppointment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewAppointmentList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewAppointmentList == null ? null : viewAppointmentList.Data, isSupressLazyLoad);
            return viewAppointmentList;
        }
        public GridData<ViewAppointment> GetGridViewAppointmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageAppointment(userId, ref dataGridSettings)) return null;
            var viewAppointmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewAppointmentList == null ? null : viewAppointmentList.Data, isSupressLazyLoad);
            return viewAppointmentList;
        }
        private bool IsManageAppointment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var userIdValue = userId.Value;
            var viewUserSystemSettingBLL = BLLFactory.CreateViewUserSystemSettingBLL();
            var viewUserSystemSetting = viewUserSystemSettingBLL.GetEntityByUserId(userIdValue);
            if (viewUserSystemSetting != null && !viewUserSystemSettingBLL.IsSuperAdminWorkGroup(viewUserSystemSetting))
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", userIdValue);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(userIdValue, new ManagerUserOrgId(ModuleBusinessType.Appointment, "", "", "UserOrgId"), new ManagerEquipmentOrgId(ModuleBusinessType.Appointment, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            //var user = __userBLL.GetEntityById(userId.Value);
            //if (user == null) return false;
            //if (!user.IsSuperAdmin)
            //{
            //    var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
            //    var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.Appointment, "", "", "UserOrgId"), new ManagerEquipmentOrgId(ModuleBusinessType.Appointment, "", "", "EquipmentOrgId"), null);
            //    if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
            //    dataGridSettings.AppendAndQueryExpression(queryExpression);
            //}
            return true;
        }
        /// <summary> 获取待支付自动取消预约列表 </summary>
        /// <returns></returns>
        public IList<ViewAppointment> GetWillAutoCancleAppointmentList()
        {
            return GetEntitiesByExpression(string.Format("BeginTime<\"{0}\"*EndTime>\"{1}\"*IsAutoCancelAppoinment=true*Status={2}*AuditingStatus=-{3}*TutorAuditStatus=-{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentStatus.Waitting, (int)AppointmentAuditStatus.NotPass, (int)TutorAuditStatus.Refused), null, "", false, false, true, __outMapping);
        }
        /// <summary> 获取待支付预约查询条件 </summary>
        /// <returns></returns>
        private string GetWillPayAppointmentQueryExpression()
        {
            /*添加IsPredictCostDeduct=0，不使用预扣费的预约将作为待支付预约*/
            //return string.Format("BeginTime>\"{0}\"*Status={1}*((IsNeedAudit=true*AuditingStatus={2}*IsNeedTutorAudit=true*TutorAuditStatus={3})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus={2})+(IsNeedAudit=false*IsNeedTutorAudit=true*TutorAuditStatus={3})+(IsNeedTutorAudit=false*IsNeedAudit=false))", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentStatus.Waitting, (int)AppointmentAuditStatus.Pass, (int)TutorAuditStatus.Passed);
            return string.Format("BeginTime>\"{0}\"*Status={1}*((IsNeedAudit=true*AuditingStatus={2}*IsNeedTutorAudit=true*TutorAuditStatus={3})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus={2})+(IsNeedAudit=false*IsNeedTutorAudit=true*TutorAuditStatus={3})+(IsNeedTutorAudit=false*IsNeedAudit=false))*IsPredictCostDeduct=false", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentStatus.Waitting, (int)AppointmentAuditStatus.Pass, (int)TutorAuditStatus.Passed);
        }
        /// <summary> 获取用户待支付预约列表 </summary>
        /// <param name="payerId"></param>
        /// <returns></returns>
        public IList<ViewAppointment> GetUserWillPayAppointmentList(Guid payerId)
        {
            var willPayAppointmentQueryExpression = GetWillPayAppointmentQueryExpression();
            willPayAppointmentQueryExpression = string.Format("{0}*{1}", willPayAppointmentQueryExpression, string.Format("SubjectDirectorId=\"{0}\"", payerId));
            return GetEntitiesByExpression(willPayAppointmentQueryExpression, null, "", false, false, true, __outMapping);
        }
        /// <summary> 获取用户与课题组待支付预约列表 </summary>
        /// <param name="userId"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public IList<ViewAppointment> GetUserWillPayAppointmentList(Guid userId, Guid subjectId)
        {
            var willPayAppointmentQueryExpression = GetWillPayAppointmentQueryExpression();
            willPayAppointmentQueryExpression = string.Format("{0}*{1}*{2}", willPayAppointmentQueryExpression, string.Format("UserId=\"{0}\"", userId), string.Format("SubjectId=\"{0}\"", subjectId));
            return GetEntitiesByExpression(willPayAppointmentQueryExpression, null, "", false, false, true, __outMapping);
        }
        public IList<ViewAppointment> GetViewAppointmentListByEquipmentLabel(string equipmentLabel, DateTime beginDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(equipmentLabel)) return null;
            return GetEntitiesByExpression(string.Format("EquipmentLabel=\"{0}\"*Status={1}*BeginTime<\"{2}\"*EndTime>\"{3}\"*((IsNeedAudit=true*AuditingStatus={4}*IsNeedTutorAudit=true*TutorAuditStatus={5})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus={4})+(IsNeedAudit=false*IsNeedTutorAudit=true*TutorAuditStatus={5})+(IsNeedTutorAudit=false*IsNeedAudit=false))*IsUseable=true", equipmentLabel, (int)AppointmentStatus.Waitting, endDate.ToString("yyyy-MM-dd HH:mm:ss"), beginDate.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentAuditStatus.Pass, (int)AppointmentAuditStatus.Pass, (int)TutorAuditStatus.Passed), null, "", false, false, true, __outMapping);
        }
        public IList<ViewAppointment> GetViewAppointmentList(User user, Guid? equipmentId, IList<int> totalTypeList, string queryExpression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (user == null) return null;
            GenerateViewAppointmentListQueryExpression(user, equipmentId, totalTypeList, ref queryExpression, isManageList);
            var viewAppointmentList = GetEntitiesByExpression(queryExpression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(user.Id, viewAppointmentList, isSupressLazyLoad);
            return viewAppointmentList;
        }
        public GridData<ViewAppointment> GetGridViewAppointmentList(User user, Guid? equipmentId, int? totalType, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (user == null) return null;
            IList<int> totalTypeList = new List<int>();
            string queryExpression = dataGridSettings.QueryExpression;
            if (totalType.HasValue) totalTypeList.Add(totalType.Value);
            GenerateViewAppointmentListQueryExpression(user, equipmentId, totalTypeList, ref queryExpression, isManageList);
            dataGridSettings.QueryExpression = queryExpression;
            var viewAppointmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(user.Id, viewAppointmentList == null ? null : viewAppointmentList.Data, isSupressLazyLoad);
            return viewAppointmentList;
        }
        private void GenerateViewAppointmentListQueryExpression(User user, Guid? equipmentId, IList<int> totalTypeList, ref string queryExpression, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = queryExpression };
            if (isManageList && !IsManageAppointment(user.Id, ref dataGridSettings))
            {
                queryExpression = "Id=null";
                return;
            }
            queryExpression = dataGridSettings.QueryExpression;
            if (equipmentId.HasValue)
            {
                queryExpression += (queryExpression == "" ? "" : "*") + string.Format("EquipmentId=\"{0}\"", equipmentId.ToString());
            }
            if (totalTypeList == null || totalTypeList.Count() == 0) return;
            string queryStatus = "(";
            foreach (var totalType in totalTypeList)
            {
                var totalTypeEnum = (EquipmentAppointmentTotalType)System.Enum.ToObject(typeof(EquipmentAppointmentTotalType), totalType);
                queryStatus += (queryStatus == "(" ? "" : "+") + AppointmentRelativeBLL.GenerateAppointmentTotalTypeQueryExpression(totalTypeEnum);
            }
            queryStatus += ")";
            if (queryStatus != "()") queryExpression += (queryExpression == "" ? "" : "*") + queryStatus;
            else queryStatus = "";
        }
        private List<IDataParameter> GenerateAppoitmentTotalDbDataParameter(string queryExpression)
        {
            queryExpression = !string.IsNullOrWhiteSpace(queryExpression) && queryExpression.IndexOf("+Id=-null") != -1 ? queryExpression.Replace("+Id=-null", "+AppointmentId=-null") : queryExpression;
            var sqlStr = ConverExpressionToSql(queryExpression, __mapping);
            var queryExpressionDataParam = DataParameterFactory.CreateDataParameter("queryExpression", sqlStr);
            var todayTotalQueryExpression = AppointmentRelativeBLL.GenerateAppointmentTotalTypeQueryExpression(EquipmentAppointmentTotalType.TodayTotal);
            var todayTotalDataParam = DataParameterFactory.CreateDataParameter("todayTotalQueryExpression", ConverExpressionToSql(todayTotalQueryExpression, __mapping));
            var doingTotalQueryExpression = AppointmentRelativeBLL.GenerateAppointmentTotalTypeQueryExpression(EquipmentAppointmentTotalType.DoingTotal);
            var doingTotalDataParam = DataParameterFactory.CreateDataParameter("doingTotalQueryExpression", ConverExpressionToSql(doingTotalQueryExpression, __mapping));
            var doneTotalQueryExpression = AppointmentRelativeBLL.GenerateAppointmentTotalTypeQueryExpression(EquipmentAppointmentTotalType.DoneTotal);
            var doneTotalDataParam = DataParameterFactory.CreateDataParameter("doneTotalQueryExpression", ConverExpressionToSql(doneTotalQueryExpression, __mapping));
            var missTotalQueryExpression = AppointmentRelativeBLL.GenerateAppointmentTotalTypeQueryExpression(EquipmentAppointmentTotalType.MissTotal);
            var missTotalDataParam = DataParameterFactory.CreateDataParameter("missTotalQueryExpression", ConverExpressionToSql(missTotalQueryExpression, __mapping));
            var changedTotalQueryExpression = AppointmentRelativeBLL.GenerateAppointmentTotalTypeQueryExpression(EquipmentAppointmentTotalType.ChangeTotal);
            var changedTotalDataParam = DataParameterFactory.CreateDataParameter("changedTotalQueryExpression", ConverExpressionToSql(changedTotalQueryExpression, __mapping));
            var cancelTotalQueryExpression = AppointmentRelativeBLL.GenerateAppointmentTotalTypeQueryExpression(EquipmentAppointmentTotalType.CancelTotal);
            var cancelTotalDataParam = DataParameterFactory.CreateDataParameter("cancelTotalQueryExpression", ConverExpressionToSql(cancelTotalQueryExpression, __mapping));
            var waitingAuditTotalQueryExpression = AppointmentRelativeBLL.GenerateAppointmentTotalTypeQueryExpression(EquipmentAppointmentTotalType.WaitingAudit);
            var waitingAuditTotalDataParam = DataParameterFactory.CreateDataParameter("waitingAuditTotalQueryExpression", ConverExpressionToSql(waitingAuditTotalQueryExpression, __mapping));
            var auditTotalQueryExpression = AppointmentRelativeBLL.GenerateAppointmentTotalTypeQueryExpression(EquipmentAppointmentTotalType.Audited);
            var auditTotalDataParam = DataParameterFactory.CreateDataParameter("auditTotalQueryExpression", ConverExpressionToSql(auditTotalQueryExpression, __mapping));
            List<IDataParameter> lstDataParam = new List<IDataParameter>()
             {
                 queryExpressionDataParam,
                 todayTotalDataParam,
                 doingTotalDataParam,
                 doneTotalDataParam,
                 missTotalDataParam,
                 changedTotalDataParam,
                 cancelTotalDataParam,
                 waitingAuditTotalDataParam,
                 auditTotalDataParam
             };
            return lstDataParam;
        }
        public AppoitmentTotalInfo GetAppoitmentTotalInfo(User user)
        {
            DataGridSettings dataGridSettings = new DataGridSettings();
            IsManageAppointment(user.Id, ref dataGridSettings);

            var dataParams = GenerateAppoitmentTotalDbDataParameter(dataGridSettings.QueryExpression);
            var result = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetAppointmentTotal", dataParams).Result;
            var totalCount = Convert.ToInt32(result[0][0]);
            var todayTotalCount = Convert.ToInt32(result[1][0]);
            var doingTotalCount = Convert.ToInt32(result[2][0]);
            var doneTotalCount = Convert.ToInt32(result[3][0]);
            var missTotalCount = Convert.ToInt32(result[4][0]);
            var changedTotalCount = Convert.ToInt32(result[5][0]);
            var cancelTotalCount = Convert.ToInt32(result[6][0]);
            var waitingAuditTotalCount = Convert.ToInt32(result[7][0]);
            var auditTotalCount = Convert.ToInt32(result[8][0]);
            AppoitmentTotalInfo appoitmentTotalInfo = new AppoitmentTotalInfo
            (
                totalCount,
                todayTotalCount,
                doingTotalCount,
                doneTotalCount,
                missTotalCount,
                changedTotalCount,
                cancelTotalCount,
                waitingAuditTotalCount,
                auditTotalCount
            );
            return appoitmentTotalInfo;
        }
        public GridData<EquipmentAppointmentTotalInfo> GetEquipmentAppoitmentTotalInfo(User user, DataGridSettings dataGridSettings)
        {
            int returnValue = 0;
            IList<EquipmentAppointmentTotalInfo> lstEquipmentAppointmentTotalInfo = new List<EquipmentAppointmentTotalInfo>();
            GridData<EquipmentAppointmentTotalInfo> gridData = new GridData<EquipmentAppointmentTotalInfo>();
            IsManageAppointment(user.Id, ref dataGridSettings);
            var dataParams = GenerateAppoitmentTotalDbDataParameter(dataGridSettings.QueryExpression);
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageIndex", dataGridSettings.PageIndex));
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageCount", dataGridSettings.PageSize));
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", returnValue, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetEquipmentAppointmentTotal", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    var equipmentId = new Guid(result[0].ToString());
                    var equipmentName = result[1] != null ? result[1].ToString() : "";
                    var equipmentIcon = result[2] != null ? result[2].ToString() : "";
                    var roomName = result[3] != null ? result[3].ToString() : "";
                    var totalCount = result[4] != null && !string.IsNullOrWhiteSpace(result[4].ToString()) ? Convert.ToInt32(result[4]) : 0;
                    var todayTotalCount = result[5] != null && !string.IsNullOrWhiteSpace(result[5].ToString()) ? Convert.ToInt32(result[5]) : 0;
                    var doingTotalCount = result[6] != null && !string.IsNullOrWhiteSpace(result[6].ToString()) ? Convert.ToInt32(result[6]) : 0;
                    var doneTotalCount = result[7] != null && !string.IsNullOrWhiteSpace(result[7].ToString()) ? Convert.ToInt32(result[7]) : 0;
                    var missTotalCount = result[8] != null && !string.IsNullOrWhiteSpace(result[8].ToString()) ? Convert.ToInt32(result[8]) : 0;
                    var changedTotalCount = result[9] != null && !string.IsNullOrWhiteSpace(result[9].ToString()) ? Convert.ToInt32(result[9]) : 0;
                    var cancelTotalCount = result[10] != null && !string.IsNullOrWhiteSpace(result[10].ToString()) ? Convert.ToInt32(result[10]) : 0;
                    var waitingAuditTotalCount = result[11] != null && !string.IsNullOrWhiteSpace(result[11].ToString()) ? Convert.ToInt32(result[11]) : 0;
                    var auditTotalCount = result[12] != null && !string.IsNullOrWhiteSpace(result[12].ToString()) ? Convert.ToInt32(result[12]) : 0;
                    EquipmentAppointmentTotalInfo equipmentAppointmentTotalInfo = new EquipmentAppointmentTotalInfo()
                    {
                        AuditedCount = auditTotalCount,
                        CancelCount = cancelTotalCount,
                        ChangedCount = changedTotalCount,
                        DoingCount = doingTotalCount,
                        DoneCount = doneTotalCount,
                        EquipmentIcon = equipmentIcon,
                        EquipmentId = equipmentId,
                        EquipmentName = equipmentName,
                        MissCount = missTotalCount,
                        RoomName = roomName,
                        TodayCount = todayTotalCount,
                        TotalCount = totalCount,
                        WaitingAuditCount = waitingAuditTotalCount
                    };
                    lstEquipmentAppointmentTotalInfo.Add(equipmentAppointmentTotalInfo);
                }
                gridData.Data = lstEquipmentAppointmentTotalInfo;
                gridData.Count = Convert.ToInt32(returnValueDataParameter.Value);
            }
            return gridData;
        }
        public AppointmentStatistics GetAppointmentStatisticsSum(User user, string year, DataGridSettings dataGridSettings)
        {
            dataGridSettings.AppendAndQueryExpression(string.Format("Status=-\"{0}\"*Status=-\"{1}\"*EndTime>\"{2}\"*EndTime<\"{3}\"", (int)AppointmentStatus.Changed, (int)AppointmentStatus.Cancel, new DateTime(Int32.Parse(year), 1, 1), new DateTime(Int32.Parse(year) + 1, 1, 1)));
            IsManageAppointment(user.Id, ref dataGridSettings);
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", sqlStr));
            int equipmentCount = 0;
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", equipmentCount, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);

            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetEquipmentAppointmentStatisticsSum", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                var result = results[0];
                var JanuaryHour = Convert.ToDouble(result[0] == DBNull.Value ? 0 : result[0]);
                var FebruaryHour = Convert.ToDouble(result[1] == DBNull.Value ? 0 : result[1]);
                var MarchHour = Convert.ToDouble(result[2] == DBNull.Value ? 0 : result[2]);
                var AprilHour = Convert.ToDouble(result[3] == DBNull.Value ? 0 : result[3]);
                var MayHour = Convert.ToDouble(result[4] == DBNull.Value ? 0 : result[4]);
                var JuneHour = Convert.ToDouble(result[5] == DBNull.Value ? 0 : result[5]);
                var JulyHour = Convert.ToDouble(result[6] == DBNull.Value ? 0 : result[6]);
                var AugustHour = Convert.ToDouble(result[7] == DBNull.Value ? 0 : result[7]);
                var SeptemberHour = Convert.ToDouble(result[8] == DBNull.Value ? 0 : result[8]);
                var OctoberHour = Convert.ToDouble(result[9] == DBNull.Value ? 0 : result[9]);
                var NovemberHour = Convert.ToDouble(result[10] == DBNull.Value ? 0 : result[10]);
                var DecemberHour = Convert.ToDouble(result[11] == DBNull.Value ? 0 : result[11]);
                equipmentCount = Convert.ToInt32(returnValueDataParameter.Value);
                AppointmentStatistics appointmentStatistics = new AppointmentStatistics()
                {
                    EquipmentId = Guid.Empty,
                    EquipmentName = equipmentCount.ToString(),
                    EquipmentNameStr = equipmentCount.ToString(),
                    JanuaryHour = JanuaryHour,
                    FebruaryHour = FebruaryHour,
                    MarchHour = MarchHour,
                    AprilHour = AprilHour,
                    MayHour = MayHour,
                    JuneHour = JuneHour,
                    JulyHour = JulyHour,
                    AugustHour = AugustHour,
                    SeptemberHour = SeptemberHour,
                    OctoberHour = OctoberHour,
                    NovemberHour = NovemberHour,
                    DecemberHour = DecemberHour
                };
                return appointmentStatistics;
            }
            return null;
        }
        public IList<AppointmentStatistics> GetAppointmentStatistics(User user, string year, DataGridSettings dataGridSettings, out int returnCount)
        {
            returnCount = 0;
            IList<AppointmentStatistics> lstAppointmentStatistics = new List<AppointmentStatistics>();
            dataGridSettings.AppendAndQueryExpression(string.Format("Status=-\"{0}\"*Status=-\"{1}\"*EndTime>\"{2}\"*EndTime<\"{3}\"", (int)AppointmentStatus.Changed, (int)AppointmentStatus.Cancel, new DateTime(Int32.Parse(year), 1, 1), new DateTime(Int32.Parse(year) + 1, 1, 1)));
            IsManageAppointment(user.Id, ref dataGridSettings);
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", sqlStr));
            if (!string.IsNullOrWhiteSpace(dataGridSettings.SortColumn))
            {
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderName", dataGridSettings.SortColumn));
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr() == "desc" ? 1 : 0));
            }
            else
            {
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderName", DBNull.Value));
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderType", DBNull.Value));
            }
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageIndex", dataGridSettings.PageIndex));
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageCount", dataGridSettings.PageSize));
            int returnValue = 0;
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", returnValue, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetEquipmentAppointmentStatistics", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    var equipmentId = new Guid(result[0].ToString());
                    var equipmentName = result[1] != null ? result[1].ToString() : "";
                    var equipmentIcon = result[2] != null ? result[2].ToString() : "";
                    var roomName = result[3] != null ? result[3].ToString() : "";
                    var JanuaryHour = Convert.ToDouble(result[4] == DBNull.Value ? 0 : result[4]);
                    var FebruaryHour = Convert.ToDouble(result[5] == DBNull.Value ? 0 : result[5]);
                    var MarchHour = Convert.ToDouble(result[6] == DBNull.Value ? 0 : result[6]);
                    var AprilHour = Convert.ToDouble(result[7] == DBNull.Value ? 0 : result[7]);
                    var MayHour = Convert.ToDouble(result[8] == DBNull.Value ? 0 : result[8]);
                    var JuneHour = Convert.ToDouble(result[9] == DBNull.Value ? 0 : result[9]);
                    var JulyHour = Convert.ToDouble(result[10] == DBNull.Value ? 0 : result[10]);
                    var AugustHour = Convert.ToDouble(result[11] == DBNull.Value ? 0 : result[11]);
                    var SeptemberHour = Convert.ToDouble(result[12] == DBNull.Value ? 0 : result[12]);
                    var OctoberHour = Convert.ToDouble(result[13] == DBNull.Value ? 0 : result[13]);
                    var NovemberHour = Convert.ToDouble(result[14] == DBNull.Value ? 0 : result[14]);
                    var DecemberHour = Convert.ToDouble(result[15] == DBNull.Value ? 0 : result[15]);
                    var equipmentOrgName = result[16] != null ? result[16].ToString() : "";
                    AppointmentStatistics appointmentStatistics = new AppointmentStatistics()
                    {
                        EquipmentId = equipmentId,
                        EquipmentName = equipmentName,
                        EquipmentRelativePic = equipmentIcon,
                        RoomName = roomName,
                        JanuaryHour = JanuaryHour,
                        FebruaryHour = FebruaryHour,
                        MarchHour = MarchHour,
                        AprilHour = AprilHour,
                        MayHour = MayHour,
                        JuneHour = JuneHour,
                        JulyHour = JulyHour,
                        AugustHour = AugustHour,
                        SeptemberHour = SeptemberHour,
                        OctoberHour = OctoberHour,
                        NovemberHour = NovemberHour,
                        DecemberHour = DecemberHour,
                        EquipmentOrgName = equipmentOrgName
                    };
                    appointmentStatistics.InitOperate();
                    appointmentStatistics.BuildOperate();
                    lstAppointmentStatistics.Add(appointmentStatistics);
                    returnCount = Convert.ToInt32(returnValueDataParameter.Value);
                }
            }

            return lstAppointmentStatistics;
        }
        public GridData<AppointmentStatistics> GetGridAppointmentStatistics(User user, string year, DataGridSettings dataGridSettings)
        {
            int count = 0;
            var appointmentStatisticsList = GetAppointmentStatistics(user, year, dataGridSettings, out count);
            return new GridData<AppointmentStatistics>()
            {
                Data = appointmentStatisticsList,
                Count = count
            };
        }
        public IList<string> GetAppointmentStatisticsYearList()
        {
            IList<string> yearList = new List<string>();
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetAppointmentStatisticsYear", null);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results) yearList.Add(result[0].ToString());
            }
            return yearList;
        }
        public UserAppointmentCount GetViewAppointmentCountByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            User user = null;
            if (userId.HasValue) user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return null;
            UserAppointmentCount userAppointmentCount = new UserAppointmentCount();
            GenerateViewAppointmentListQueryExpression(user, null, null, ref expression, isManageList);
            var viewAppointmentList = CountModelListByExpression(expression, mapping, isDistinct, outDistinctMapping);
            var todayExpression = expression + (expression == "" ? "" : "*") + string.Format("(BeginTime=-null*BeginTime>\"{0}\")", DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            var doingExpression = expression + (expression == "" ? "" : "*") + string.Format("(Status=\"{0}\"*BeginTime=-null*BeginTime>\"{1}\")", (int)AppointmentStatus.Waitting, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var mayMissExpression = expression + (expression == "" ? "" : "*") + string.Format("(Status=\"{0}\"*BeginTime=-null*BeginTime<\"{1}\")", (int)AppointmentStatus.Waitting, DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            userAppointmentCount.DoingAppointmentToday = CountModelListByExpression(doingExpression, mapping, isDistinct, outDistinctMapping);
            userAppointmentCount.MayMissAppointmentToday = CountModelListByExpression(mayMissExpression, mapping, isDistinct, outDistinctMapping);
            userAppointmentCount.AppointmentToday = CountModelListByExpression(todayExpression, mapping, isDistinct, outDistinctMapping);
            return userAppointmentCount;
        }
        public UserAppointmentCount GetViewAppointmentCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true)
        {
            return GetViewAppointmentCountByExpression(userId, dataGridSettings.QueryExpression, mapping = null, isDistinct, outDistinctMapping, isManageList);
        }
        public override GridData<ViewAppointment> GetGridEntitiesByExpression(string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (mapping == null)
                mapping = new Dictionary<string, string>();
            mapping["Id"] = "AppointmentId";
            var dataGridSettings = GenerateDataGridSetting(expression, orderExpression, pageindex, pageSize, mapping);
            GridData<ViewAppointment> gridData = new GridData<ViewAppointment>();
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, mapping);
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
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetAppointmentList", dataParams).Result;
            if (results != null && results.Count > 0)
            {
                IList<ViewAppointment> viewAppointments = new List<ViewAppointment>();
                foreach (var result in results)
                {
                    ViewAppointment viewAppointment = new ViewAppointment();
                    viewAppointment.Id = new Guid(result[0].ToString());
                    viewAppointment.UserId = new Guid(result[1].ToString());
                    viewAppointment.UserName = result[2] != null ? result[2].ToString() : null;
                    viewAppointment.UserOrgId = null;
                    if (result[3] != null && !string.IsNullOrWhiteSpace(result[3].ToString())) viewAppointment.UserOrgId = new Guid(result[3].ToString());
                    viewAppointment.Card = null;
                    if (result[4] != null && !string.IsNullOrWhiteSpace(result[4].ToString())) viewAppointment.Card = int.Parse(result[4].ToString());
                    viewAppointment.UserLabel = result[5] != null ? result[5].ToString() : null;
                    viewAppointment.UserPhone = result[6] != null ? result[6].ToString() : null;
                    viewAppointment.EquipmentId = null;
                    if (result[7] != null && !string.IsNullOrWhiteSpace(result[7].ToString())) viewAppointment.EquipmentId = new Guid(result[7].ToString());
                    viewAppointment.RelativePic = result[8] != null ? result[8].ToString() : null;
                    viewAppointment.EquipmentName = result[9] != null ? result[9].ToString() : null;
                    viewAppointment.EquipmentModel = result[10] != null ? result[10].ToString() : null;
                    viewAppointment.EquipmentOrgId = null;
                    if (result[11] != null && !string.IsNullOrWhiteSpace(result[11].ToString())) viewAppointment.EquipmentOrgId = new Guid(result[11].ToString());
                    viewAppointment.EquipmentOrgName = result[12] != null ? result[12].ToString() : null;
                    viewAppointment.EquipmentOrgXPath = result[13] != null ? result[13].ToString() : null;
                    viewAppointment.IsAutoCancelAppoinment = bool.Parse(result[14].ToString());
                    viewAppointment.AutoCancelAppoinmentMinutes = int.Parse(result[15].ToString());
                    viewAppointment.EquipmentLabel = result[16] != null ? result[16].ToString() : null;
                    viewAppointment.RoomName = result[17] != null ? result[17].ToString() : null;
                    viewAppointment.Status = null;
                    if (result[18] != null && !string.IsNullOrWhiteSpace(result[18].ToString())) viewAppointment.Status = int.Parse(result[18].ToString());
                    viewAppointment.SubjectId = null;
                    if (result[19] != null && !string.IsNullOrWhiteSpace(result[19].ToString())) viewAppointment.SubjectId = new Guid(result[19].ToString());
                    viewAppointment.PaymentMethod = null;
                    if (result[20] != null && !string.IsNullOrWhiteSpace(result[20].ToString())) viewAppointment.PaymentMethod = int.Parse(result[20].ToString());
                    viewAppointment.ApplyTime = null;
                    if (result[21] != null && !string.IsNullOrWhiteSpace(result[21].ToString())) viewAppointment.ApplyTime = DateTime.Parse(result[21].ToString());
                    viewAppointment.BeginTime = null;
                    if (result[22] != null && !string.IsNullOrWhiteSpace(result[22].ToString())) viewAppointment.BeginTime = DateTime.Parse(result[22].ToString());
                    viewAppointment.EndTime = null;
                    if (result[23] != null && !string.IsNullOrWhiteSpace(result[23].ToString())) viewAppointment.EndTime = DateTime.Parse(result[23].ToString());
                    viewAppointment.IsUseable = null;
                    if (result[24] != null && !string.IsNullOrWhiteSpace(result[24].ToString())) viewAppointment.IsUseable = bool.Parse(result[24].ToString());
                    viewAppointment.WhyUnuseable = result[25] != null ? result[25].ToString() : null;
                    viewAppointment.PayerId = null;
                    if (result[26] != null && !string.IsNullOrWhiteSpace(result[26].ToString())) viewAppointment.PayerId = new Guid(result[26].ToString());
                    viewAppointment.PayerName = result[27] != null ? result[27].ToString() : null;
                    viewAppointment.DesOptions = result[28] != null ? result[28].ToString() : null;
                    viewAppointment.ChangerId = null;
                    if (result[29] != null && !string.IsNullOrWhiteSpace(result[29].ToString())) viewAppointment.ChangerId = new Guid(result[29].ToString());
                    viewAppointment.ChangerName = result[30] != null ? result[30].ToString() : null;
                    viewAppointment.ChangedTime = null;
                    if (result[31] != null && !string.IsNullOrWhiteSpace(result[31].ToString())) viewAppointment.ChangedTime = DateTime.Parse(result[31].ToString());
                    viewAppointment.ChangedApppointmentId = null;
                    if (result[32] != null && !string.IsNullOrWhiteSpace(result[32].ToString())) viewAppointment.ChangedApppointmentId = new Guid(result[32].ToString());
                    viewAppointment.CreaterId = null;
                    if (result[33] != null && !string.IsNullOrWhiteSpace(result[33].ToString())) viewAppointment.CreaterId = new Guid(result[33].ToString());
                    viewAppointment.CreatorName = result[34] != null ? result[34].ToString() : null;
                    viewAppointment.IsNeedAudit = null;
                    if (result[35] != null && !string.IsNullOrWhiteSpace(result[35].ToString())) viewAppointment.IsNeedAudit = bool.Parse(result[35].ToString());
                    viewAppointment.AuditingStatus = null;
                    if (result[36] != null && !string.IsNullOrWhiteSpace(result[36].ToString())) viewAppointment.AuditingStatus = int.Parse(result[36].ToString());
                    viewAppointment.AuditingUserId = null;
                    if (result[37] != null && !string.IsNullOrWhiteSpace(result[37].ToString())) viewAppointment.AuditingUserId = new Guid(result[37].ToString());
                    viewAppointment.AuditingUserName = result[38] != null ? result[38].ToString() : null;
                    viewAppointment.AuditingNoPassWhy = result[39] != null ? result[39].ToString() : null;
                    viewAppointment.IsPredictCostDeduct = null;
                    if (result[40] != null && !string.IsNullOrWhiteSpace(result[40].ToString())) viewAppointment.IsPredictCostDeduct = bool.Parse(result[40].ToString());
                    viewAppointment.Fee = null;
                    if (result[41] != null && !string.IsNullOrWhiteSpace(result[41].ToString())) viewAppointment.Fee = double.Parse(result[41].ToString());
                    viewAppointment.RealCurrency = null;
                    if (result[42] != null && !string.IsNullOrWhiteSpace(result[42].ToString())) viewAppointment.RealCurrency = double.Parse(result[42].ToString());
                    viewAppointment.VirtualCurrency = null;
                    if (result[43] != null && !string.IsNullOrWhiteSpace(result[43].ToString())) viewAppointment.VirtualCurrency = double.Parse(result[43].ToString());
                    viewAppointment.CalDuration = null;
                    if (result[44] != null && !string.IsNullOrWhiteSpace(result[44].ToString())) viewAppointment.CalDuration = double.Parse(result[44].ToString());
                    viewAppointment.HasClossingAccount = bool.Parse(result[45].ToString());
                    viewAppointment.UseNature = null;
                    if (result[46] != null && !string.IsNullOrWhiteSpace(result[46].ToString())) viewAppointment.UseNature = int.Parse(result[46].ToString());
                    viewAppointment.SubjectProjectId = null;
                    if (result[47] != null && !string.IsNullOrWhiteSpace(result[47].ToString())) viewAppointment.SubjectProjectId = new Guid(result[47].ToString());
                    viewAppointment.SubjectName = result[48] != null ? result[48].ToString() : null;
                    viewAppointment.SubjectDirectorId = null;
                    if (result[49] != null && !string.IsNullOrWhiteSpace(result[49].ToString())) viewAppointment.SubjectDirectorId = new Guid(result[49].ToString());
                    viewAppointment.SubjectProjectName = result[50] != null ? result[50].ToString() : null;
                    viewAppointment.SampleCount = null;
                    if (result[51] != null && !string.IsNullOrWhiteSpace(result[51].ToString())) viewAppointment.SampleCount = int.Parse(result[51].ToString());
                    viewAppointment.UsedConfirmId = null;
                    if (result[52] != null && !string.IsNullOrWhiteSpace(result[52].ToString())) viewAppointment.UsedConfirmId = new Guid(result[52].ToString());
                    viewAppointment.TutorName = result[53] != null ? result[53].ToString() : null;
                    viewAppointment.IsNeedTutorAudit = result[54] != null && !string.IsNullOrWhiteSpace(result[54].ToString()) ? (bool)result[54] : false;
                    viewAppointment.TutorAuditStatus = result[55] != null && !string.IsNullOrWhiteSpace(result[55].ToString()) ? (int)result[55] : (int)TutorAuditStatus.WaitingAudit;
                    viewAppointment.TutorAuditRemark = result[56] != null ? result[56].ToString() : null;
                    viewAppointment.TutorAuditTime = null;
                    if (result[57] != null && !string.IsNullOrWhiteSpace(result[57].ToString())) viewAppointment.TutorAuditTime = DateTime.Parse(result[57].ToString());
                    viewAppointment.TutorId = null;
                    if (result[58] != null && !string.IsNullOrWhiteSpace(result[58].ToString())) viewAppointment.TutorId = new Guid(result[58].ToString());
                    viewAppointment.UserOrgName = result[59] != null ? result[59].ToString() : null;
                    viewAppointment.IsSupressLazyLoad = isSupressLazyLoad;
                    viewAppointments.Add(viewAppointment);
                }
                gridData.Data = viewAppointments;
            }
            gridData.Count = returnValueDataParam.Value == null ? 0 : int.Parse(returnValueDataParam.Value.ToString());
            return gridData;
        }
        public string GenerateRecentAppointmentQueryExpression(out DateTime beginDate, out DateTime endDate, out int days)
        {
            days = 1;
            var showAppointmentDays = __dictCodeBLL.GetDictCodeValueByCode("Appointment", "ShowAppointmentDays");
            if (!string.IsNullOrWhiteSpace(showAppointmentDays) && showAppointmentDays.IsInt()) days = int.Parse(showAppointmentDays);
            beginDate = DateTime.Now.Date;
            endDate = DateTime.Now.Date.AddDays(days).AddSeconds(-1);
            string todayQueryExpression = string.Format("BeginTime>\"{0}\"*BeginTime<\"{1}\"",
                beginDate.ToString("yyyy-MM-dd HH:mm:ss"),
                endDate.ToString("yyyy-MM-dd HH:mm:ss"));
            return todayQueryExpression;
        }

    }
}
