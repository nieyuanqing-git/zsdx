using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewNMPAppointmentBLL : BLLBase<ViewNMPAppointment>, IViewNMPAppointmentBLL
    {
        private IUserBLL __userBLL;
        private INMPBLL __nmpBLL;
        private INMPAppointmentBLL __nmpAppointmentBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL; 
        private Dictionary<string, string> __mapping = new Dictionary<string, string>();
        private readonly string[] __outMapping = new string[] { "NMPAdminId" };
        public ViewNMPAppointmentBLL()
        {
            __mapping["Id"] = "NMPAppointmentId";
            __userBLL = BLLFactory.CreateUserBLL();
            __nmpBLL = BLLFactory.CreateNMPBLL();
            __nmpAppointmentBLL = BLLFactory.CreateNMPAppointmentBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewNMPAppointment> viewNMPAppointmentList, bool isSupressLazyLoad)
        {
            if (viewNMPAppointmentList == null || viewNMPAppointmentList.Count == 0) return;
            IList<NMPAppointmentPrivilige> lstNMPAppointmentPriviliges = (IList<NMPAppointmentPrivilige>)__nmpAppointmentBLL.GetNMPAppointmentPriviliges(userId, viewNMPAppointmentList);
            var nmpList = __nmpBLL.GetEntitiesByExpression(viewNMPAppointmentList.Select(p => p.NMPId).Distinct().ToFormatStr());
            foreach (var viewNMPAppointment in viewNMPAppointmentList)
            {
                NMPAppointmentPrivilige nmpAppointmentPrivilige = lstNMPAppointmentPriviliges.FirstOrDefault(p => p.NMPAppointmentId.HasValue && p.NMPAppointmentId.Value == viewNMPAppointment.Id);
                viewNMPAppointment.IsSupressLazyLoad = false;
                viewNMPAppointment.InitOperate();
                OperateDecorate(userId, viewNMPAppointment, nmpAppointmentPrivilige, nmpList.First(p => p.Id == viewNMPAppointment.NMPId));
                viewNMPAppointment.BuildOperate();
                viewNMPAppointment.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewNMPAppointment viewNMPAppointment, NMPAppointmentPrivilige nmpAppointmentPrivilige, NMP nmp)
        {
            string errorMessage = "";
            if (viewNMPAppointment == null) throw new ArgumentException("工位设备信息为空");
            if (nmpAppointmentPrivilige == null)
            {
                viewNMPAppointment.CancelNMPAppointmentOperate = "";
                viewNMPAppointment.RegisterBreakNMPAppointmentOperate = "";
                viewNMPAppointment.CancelRegisterBreakNMPAppointmentOperate = "";
                viewNMPAppointment.TutorAuditNMPAppointmentOperate = "";
                viewNMPAppointment.AuditNMPAppointmentOperate = "";
                viewNMPAppointment.ViewNMPAppointmentInfoOperate = "";
                viewNMPAppointment.AddNMPAppointmentUsedConfirmOperate = "";
                return;
            }
            if (!nmpAppointmentPrivilige.IsEnableAppiontmentDetailInfo)
            {
                viewNMPAppointment.ViewNMPAppointmentInfoOperate = "";
            }
            var viewAppointments = new ViewNMPAppointment[] { viewNMPAppointment };
            var lstNMPAppointmentPrivilige = new List<NMPAppointmentPrivilige> { nmpAppointmentPrivilige };
            var nmps = new NMP[] { nmp };
            if (!__nmpAppointmentBLL.JudgeIsEnableCancel(viewAppointments, lstNMPAppointmentPrivilige, nmps, userId.Value, out errorMessage))
            {
                viewNMPAppointment.CancelNMPAppointmentOperate = "";
            }
            if (!__nmpAppointmentBLL.JudgeIsEnableAudit(false, null, viewAppointments, lstNMPAppointmentPrivilige, nmps, userId.Value, out errorMessage))
            {
                viewNMPAppointment.AuditNMPAppointmentOperate = "";
            }
            if (!__nmpAppointmentBLL.JudgeIsEnableTutorAudit(false, null, viewAppointments, nmps, userId.Value, out errorMessage))
            {
                viewNMPAppointment.TutorAuditNMPAppointmentOperate = "";
            }
            if (!__nmpAppointmentBLL.JudgeIsEnableRegisterBreakAppointment(false, viewAppointments, lstNMPAppointmentPrivilige, userId.Value, out errorMessage))
            {
                viewNMPAppointment.RegisterBreakNMPAppointmentOperate = "";
            }
            if (!__nmpAppointmentBLL.JudgeIsEnableCancelRegisterBreakAppointment(viewAppointments, lstNMPAppointmentPrivilige, userId.Value, out errorMessage))
            {
                viewNMPAppointment.CancelRegisterBreakNMPAppointmentOperate = "";
            }
            if (!__nmpAppointmentBLL.JudgeIsEnableAddUsedConfirm(viewAppointments, lstNMPAppointmentPrivilige, userId.Value, out errorMessage))
            {
                viewNMPAppointment.AddNMPAppointmentUsedConfirmOperate = "";
            }
        }
        private bool IsManageNMPAppointment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("NMPAdminId=\"{0}\"", user.Id);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.NMPAppointment, "", "", "UserOrgId"), null, new ManagerNMPOrgId(ModuleBusinessType.NMPAppointment, "", "", "NMPOrgId"));
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
        private List<IDataParameter> GenerateNMPAppoitmentTotalDbDataParameter(string queryExpression)
        {
            queryExpression = !string.IsNullOrWhiteSpace(queryExpression) && queryExpression.IndexOf("+Id=-null") != -1 ? queryExpression.Replace("+Id=-null", "+NMPAppointmentId=-null") : queryExpression;
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
                 cancelTotalDataParam,
                 waitingAuditTotalDataParam,
                 auditTotalDataParam
             };
            return lstDataParam;
        }
        private void GenerateViewNMPAppointmentListQueryExpression(User user, Guid? nmpEquipmentId, IList<int> totalTypeList, ref string queryExpression, bool isManageList = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = queryExpression };
            if (isManageList && !IsManageNMPAppointment(user.Id, ref dataGridSettings))
            {
                queryExpression = "Id=null";
                return;
            }
            queryExpression = dataGridSettings.QueryExpression;
            if (nmpEquipmentId.HasValue)
            {
                queryExpression += (queryExpression == "" ? "" : "*") + string.Format("NMPEquipmentId=\"{0}\"", nmpEquipmentId.ToString());
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
        public IList<ViewNMPAppointment> GetViewNMPAppointmentListByNMPEquipmentLabel(string nmpEquipmentLabel, DateTime beginDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(nmpEquipmentLabel)) return null;
            return GetEntitiesByExpression(string.Format("NMPEquipmentLabel=\"{0}\"*Status={1}*BeginTime<\"{2}\"*EndTime>\"{3}\"*((IsNeedAudit=true*AuditingStatus={4}*IsNeedTutorAudit=true*TutorAuditStatus={5})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus={4})+(IsNeedAudit=false*IsNeedTutorAudit=true*TutorAuditStatus={5})+(IsNeedTutorAudit=false*IsNeedAudit=false))*IsUseable=true", nmpEquipmentLabel, (int)AppointmentStatus.Waitting, endDate.ToString("yyyy-MM-dd HH:mm:ss"), beginDate.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentAuditStatus.Pass, (int)AppointmentAuditStatus.Pass, (int)TutorAuditStatus.Passed), null, "", false, false, true, __outMapping);
        }
        public GridData<NMPEquipmentAppointmentTotalInfo> GetNMPEquipmentAppoitmentTotalInfo(User user, DataGridSettings dataGridSettings)
        {
            int returnValue = 0;
            IList<NMPEquipmentAppointmentTotalInfo> lstNMPAppoitmentTotalInfo = new List<NMPEquipmentAppointmentTotalInfo>();
            GridData<NMPEquipmentAppointmentTotalInfo> gridData = new GridData<NMPEquipmentAppointmentTotalInfo>();
            IsManageNMPAppointment(user.Id, ref dataGridSettings);
            var dataParams = GenerateNMPAppoitmentTotalDbDataParameter(dataGridSettings.QueryExpression);
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageIndex", dataGridSettings.PageIndex));
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageCount", dataGridSettings.PageSize));
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", returnValue, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetNMPEquipmentAppointmentTotal", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    var nmpId = new Guid(result[0].ToString());
                    var nmpName = result[1] != null ? result[1].ToString() : "";
                    var nmpEquipmentId = new Guid(result[2].ToString());
                    var nmpEquipmentName = result[3] != null ? result[3].ToString() : "";
                    var nmpEquipmentIcon = result[4] != null ? result[4].ToString() : "";
                    var roomName = result[5] != null ? result[5].ToString() : "";
                    var totalCount = result[6] != null && !string.IsNullOrWhiteSpace(result[6].ToString()) ? Convert.ToInt32(result[6]) : 0;
                    var todayTotalCount = result[7] != null && !string.IsNullOrWhiteSpace(result[7].ToString()) ? Convert.ToInt32(result[7]) : 0;
                    var doingTotalCount = result[8] != null && !string.IsNullOrWhiteSpace(result[8].ToString()) ? Convert.ToInt32(result[8]) : 0;
                    var doneTotalCount = result[9] != null && !string.IsNullOrWhiteSpace(result[9].ToString()) ? Convert.ToInt32(result[9]) : 0;
                    var missTotalCount = result[10] != null && !string.IsNullOrWhiteSpace(result[10].ToString()) ? Convert.ToInt32(result[10]) : 0;
                    var cancelTotalCount = result[11] != null && !string.IsNullOrWhiteSpace(result[11].ToString()) ? Convert.ToInt32(result[11]) : 0;
                    var waitingAuditTotalCount = result[12] != null && !string.IsNullOrWhiteSpace(result[12].ToString()) ? Convert.ToInt32(result[12]) : 0;
                    var auditTotalCount = result[13] != null && !string.IsNullOrWhiteSpace(result[13].ToString()) ? Convert.ToInt32(result[13]) : 0;
                    NMPEquipmentAppointmentTotalInfo nmpEquipmentAppointmentTotalInfo = new NMPEquipmentAppointmentTotalInfo()
                    {
                        NMPId = nmpId,
                        NMPName = nmpName,
                        NMPEquipmentIcon = nmpEquipmentIcon,
                        NMPEquipmentId = nmpEquipmentId,
                        NMPEquipmentName = nmpEquipmentName,
                        AuditedCount = auditTotalCount,
                        CancelCount = cancelTotalCount,
                        DoingCount = doingTotalCount,
                        DoneCount = doneTotalCount,
                        MissCount = missTotalCount,
                        RoomName = roomName,
                        TodayCount = todayTotalCount,
                        TotalCount = totalCount,
                        WaitingAuditCount = waitingAuditTotalCount
                    };
                    lstNMPAppoitmentTotalInfo.Add(nmpEquipmentAppointmentTotalInfo);
                }
                gridData.Data = lstNMPAppoitmentTotalInfo;
                gridData.Count = Convert.ToInt32(returnValueDataParameter.Value);
            }
            return gridData;
        }
        public NMPAppoitmentTotalInfo GetNMPAppoitmentTotalInfo(User user)
        {
            DataGridSettings dataGridSettings = new DataGridSettings();
            IsManageNMPAppointment(user.Id, ref dataGridSettings);

            var dataParams = GenerateNMPAppoitmentTotalDbDataParameter(dataGridSettings.QueryExpression);
            var result = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetNMPAppointmentTotal", dataParams).Result;
            var totalCount = Convert.ToInt32(result[0][0]);
            var todayTotalCount = Convert.ToInt32(result[1][0]);
            var doingTotalCount = Convert.ToInt32(result[2][0]);
            var doneTotalCount = Convert.ToInt32(result[3][0]);
            var missTotalCount = Convert.ToInt32(result[4][0]);
            var cancelTotalCount = Convert.ToInt32(result[5][0]);
            var waitingAuditTotalCount = Convert.ToInt32(result[6][0]);
            var auditTotalCount = Convert.ToInt32(result[7][0]);
            NMPAppoitmentTotalInfo nmpAppoitmentTotalInfo = new NMPAppoitmentTotalInfo
            (
                totalCount,
                todayTotalCount,
                doingTotalCount,
                doneTotalCount,
                missTotalCount,
                cancelTotalCount,
                waitingAuditTotalCount,
                auditTotalCount
            );
            return nmpAppoitmentTotalInfo;
        }
        public IList<ViewNMPAppointment> GetViewNMPAppointmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageNMPAppointment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewNMPAppointmentList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewNMPAppointmentList, isSupressLazyLoad);
            return viewNMPAppointmentList;
        }
        public IList<ViewNMPAppointment> GetViewNMPAppointmentListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageNMPAppointment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewNMPAppointmentList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewNMPAppointmentList, isSupressLazyLoad);
            return viewNMPAppointmentList;
        }
        public IList<ViewNMPAppointment> GetViewNMPAppointmentList(User user, Guid? nmpEquipmentId, IList<int> totalTypeList, string queryExpression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (user == null) return null;
            GenerateViewNMPAppointmentListQueryExpression(user, nmpEquipmentId, totalTypeList, ref queryExpression, isManageList);
            var viewNMPAppointmentList = GetEntitiesByExpression(queryExpression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(user.Id, viewNMPAppointmentList, isSupressLazyLoad);
            return viewNMPAppointmentList;
        }
        public IList<ViewNMPAppointment> GetViewNMPAppointmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageNMPAppointment(userId, ref dataGridSettings)) return null;
            var viewNMPAppointmentList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewNMPAppointmentList, isSupressLazyLoad);
            return viewNMPAppointmentList;
        }
        public GridData<ViewNMPAppointment> GetGridViewNMPAppointmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageNMPAppointment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewNMPAppointmentList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewNMPAppointmentList == null ? null : viewNMPAppointmentList.Data, isSupressLazyLoad);
            return viewNMPAppointmentList;
        }
        public GridData<ViewNMPAppointment> GetGridViewNMPAppointmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageNMPAppointment(userId, ref dataGridSettings)) return null;
            var viewNMPAppointmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewNMPAppointmentList == null ? null : viewNMPAppointmentList.Data, isSupressLazyLoad);
            return viewNMPAppointmentList;
        }
        public GridData<ViewNMPAppointment> GetGridViewNMPAppointmentList(User user, Guid? equipmentId, int? totalType, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (user == null) return null;
            IList<int> totalTypeList = new List<int>();
            string queryExpression = dataGridSettings.QueryExpression;
            if (totalType.HasValue) totalTypeList.Add(totalType.Value);
            GenerateViewNMPAppointmentListQueryExpression(user, equipmentId, totalTypeList, ref queryExpression, isManageList);
            dataGridSettings.QueryExpression = queryExpression;
            var viewNMPAppointmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(user.Id, viewNMPAppointmentList == null ? null : viewNMPAppointmentList.Data, isSupressLazyLoad);
            return viewNMPAppointmentList;
        }
        public override GridData<ViewNMPAppointment> GetGridEntitiesByExpression(string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var dataGridSettings = GenerateDataGridSetting(expression, orderExpression, pageindex, pageSize, mapping);
            GridData<ViewNMPAppointment> gridData = new GridData<ViewNMPAppointment>();
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
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetNMPAppointmentList", dataParams).Result;
            if (results != null && results.Count > 0)
            {
                IList<ViewNMPAppointment> viewNMPAppointments = new List<ViewNMPAppointment>();
                foreach (var result in results)
                {
                    ViewNMPAppointment viewNMPAppointment = new ViewNMPAppointment();
                    viewNMPAppointment.Id = new Guid(result[0].ToString());
                    if (result[1] != null && !string.IsNullOrWhiteSpace(result[1].ToString()))
                        viewNMPAppointment.AppointmentId = result[1].ToString();
                    viewNMPAppointment.UserId = new Guid(result[2].ToString());
                    viewNMPAppointment.UserName = result[3] != null ? result[3].ToString() : null;
                    viewNMPAppointment.UserOrgId = null;
                    if (result[4] != null && !string.IsNullOrWhiteSpace(result[4].ToString())) viewNMPAppointment.UserOrgId = new Guid(result[4].ToString());
                    viewNMPAppointment.TutorId = null;
                    if (result[5] != null && !string.IsNullOrWhiteSpace(result[5].ToString())) viewNMPAppointment.TutorId = new Guid(result[5].ToString());
                    viewNMPAppointment.TutorName = result[6] != null ? result[6].ToString() : null;
                    viewNMPAppointment.UserPhone = result[7] != null ? result[7].ToString() : null;
                    viewNMPAppointment.TutorPhone = result[8] != null ? result[8].ToString() : null;
                    viewNMPAppointment.NMPEquipmentId = new Guid(result[9].ToString());
                    viewNMPAppointment.NMPEquipmentName = result[10] != null ? result[10].ToString() : null;
                    viewNMPAppointment.NMPEquipmentLabel = result[11] != null ? result[11].ToString() : null;
                    viewNMPAppointment.NMPId = new Guid(result[12].ToString());
                    viewNMPAppointment.NMPName = result[13] != null ? result[13].ToString() : null;
                    viewNMPAppointment.NMPPicPath = result[14] != null ? result[14].ToString() : null;
                    viewNMPAppointment.NMPOrgId = null;
                    if (result[15] != null && !string.IsNullOrWhiteSpace(result[15].ToString())) viewNMPAppointment.NMPOrgId = new Guid(result[15].ToString());
                    viewNMPAppointment.NMPOrgName = result[16] != null ? result[16].ToString() : null;
                    viewNMPAppointment.NMPOrgXPath = result[17] != null ? result[17].ToString() : null;
                    viewNMPAppointment.NMPRoomId = null;
                    if (result[18] != null && !string.IsNullOrWhiteSpace(result[18].ToString())) viewNMPAppointment.NMPRoomId = new Guid(result[18].ToString());
                    viewNMPAppointment.NMPRoomName = result[19] != null ? result[19].ToString() : null;
                    viewNMPAppointment.NMPRoomXPath = result[20] != null ? result[20].ToString() : null;
                    viewNMPAppointment.BeginTime = DateTime.Parse(result[21].ToString());
                    viewNMPAppointment.EndTime = DateTime.Parse(result[22].ToString());
                    viewNMPAppointment.IsUseable = bool.Parse(result[23].ToString());
                    viewNMPAppointment.WhyUnuseable = result[24] != null ? result[24].ToString() : null;
                    viewNMPAppointment.Status = int.Parse(result[25].ToString());
                    viewNMPAppointment.UseNature = int.Parse(result[26].ToString());
                    viewNMPAppointment.SubjectId = null;
                    if (result[27] != null && !string.IsNullOrWhiteSpace(result[27].ToString())) viewNMPAppointment.SubjectId = new Guid(result[27].ToString());
                    viewNMPAppointment.SubjectName = result[28] != null ? result[28].ToString() : null;
                    viewNMPAppointment.SubjectDirectorId = null;
                    if (result[29] != null && !string.IsNullOrWhiteSpace(result[29].ToString())) viewNMPAppointment.SubjectDirectorId = new Guid(result[29].ToString());
                    viewNMPAppointment.ApplyTime = DateTime.Parse(result[30].ToString());
                    viewNMPAppointment.CreaterId = null;
                    if (result[31] != null && !string.IsNullOrWhiteSpace(result[31].ToString())) viewNMPAppointment.CreaterId = new Guid(result[31].ToString());
                    viewNMPAppointment.CreatorName = result[32] != null ? result[32].ToString() : null;
                    viewNMPAppointment.IsNeedAudit = bool.Parse(result[33].ToString());
                    viewNMPAppointment.AuditingStatus = int.Parse(result[34].ToString());
                    viewNMPAppointment.AuditingUserId = null;
                    if (result[35] != null && !string.IsNullOrWhiteSpace(result[35].ToString())) viewNMPAppointment.AuditingUserId = new Guid(result[35].ToString());
                    viewNMPAppointment.AuditingUserName = result[36] != null ? result[36].ToString() : null;
                    if (result[37] != null && !string.IsNullOrWhiteSpace(result[37].ToString())) viewNMPAppointment.CalDuration = double.Parse(result[37].ToString());
                    if (result[38] != null && !string.IsNullOrWhiteSpace(result[38].ToString())) viewNMPAppointment.Fee = double.Parse(result[38].ToString());
                    viewNMPAppointment.HasClossingAccount = bool.Parse(result[39].ToString());
                    viewNMPAppointment.SubjectProjectId = null;
                    if (result[40] != null && !string.IsNullOrWhiteSpace(result[40].ToString())) viewNMPAppointment.SubjectProjectId = new Guid(result[40].ToString());
                    viewNMPAppointment.SubjectProjectName = result[41] != null ? result[41].ToString() : null;
                    viewNMPAppointment.NMPAdminId = null;
                    if (result[42] != null && !string.IsNullOrWhiteSpace(result[42].ToString())) viewNMPAppointment.NMPAdminId = new Guid(result[42].ToString());
                    viewNMPAppointment.IsNeedTutorAudit = bool.Parse(result[43].ToString());
                    viewNMPAppointment.TutorAuditStatus = int.Parse(result[44].ToString());
                    viewNMPAppointment.TutorAuditRemark = result[45] != null ? result[45].ToString() : null;
                    viewNMPAppointment.TutorAuditTime = null;
                    if (result[46] != null && !string.IsNullOrWhiteSpace(result[46].ToString())) viewNMPAppointment.TutorAuditTime = DateTime.Parse(result[46].ToString());
                    viewNMPAppointment.ExperimentationContent = result[47] != null ? result[47].ToString() : null;
                    viewNMPAppointment.AuditingNoPassWhy = result[48] != null ? result[48].ToString() : null;
                    viewNMPAppointment.PayerId = null;
                    if (result[49] != null && !string.IsNullOrWhiteSpace(result[49].ToString())) viewNMPAppointment.PayerId = new Guid(result[49].ToString());
                    viewNMPAppointment.PaymentMethod = int.Parse(result[50].ToString());
                    viewNMPAppointment.UserLabel = result[51] != null ? result[51].ToString() : null;
                    viewNMPAppointment.Card = null;
                    if (result[51] != null && !string.IsNullOrWhiteSpace(result[52].ToString())) viewNMPAppointment.Card = int.Parse(result[52].ToString());
                    
                    viewNMPAppointment.IsSupressLazyLoad = isSupressLazyLoad;
                    viewNMPAppointments.Add(viewNMPAppointment);
                }
                gridData.Data = viewNMPAppointments;
            }
            gridData.Count = returnValueDataParam.Value == null ? 0 : int.Parse(returnValueDataParam.Value.ToString());
            return gridData;
        }
    }
}
