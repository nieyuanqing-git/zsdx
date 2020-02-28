using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.View;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;

namespace com.Bynonco.LIMS.BLL
{
    public class StatisticsInfoBLL : IStatisticsInfoBLL
    {
        private IDictCodeBLL __dictCodeBLL;
        private IUserBLL __userBLL;
        private IUserSystemSettingBLL __userSystemSettingBLL;
        private IViewUserBLL __viewUserBLL;
        private IUserTypeBLL __userTypeBLL;
        private ISubjectBLL __subjectBLL;
        private IExperimenterSubjectBLL __experimenterSubjectBLL;
        private IViewAppointmentBLL __viewAppointmentBLL;
        private IViewUserEquipmentBLL __viewUserEquipmentBLL;
        private IViewUsedConfirmBLL __viewUsedConfirmBLL;
        private IViewMessageReceiveBLL __viewMessageReceiveBLL;
        private IViewDepositRecordBLL __viewDepositRecordBLL;
        private IViewOpenFundDepositRecordBLL __viewOpenFundDepositRecordBLL;
        private IViewCurrentUsingEquipmentBLL __viewCurrentUsingEquipmentBLL;
        private IViewEquipmentTrainningBLL __viewEquipmentTrainningBLL;
        private IViewMaterialInfoBLL __viewMaterialInfoBLL;
        private IViewMaterialPurchaseBLL __viewMaterialPurchaseBLL;
        private IViewMaterialRecipientBLL __viewMaterialRecipientBLL;
        private IViewMaterialOutputBLL __viewMaterialOutputBLL;
        private IViewEquipmentAlarmBLL __viewEquipmentAlarmBLL;
        private IViewEquipmentBrokenReportBLL __viewEquipmentBrokenReportBLL;
        private IViewSampleApplyBLL __viewSampleApplyBLL;    
        
        public StatisticsInfoBLL()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __viewUserBLL = BLLFactory.CreateViewUserBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __userSystemSettingBLL = BLLFactory.CreateUserSystemSettingBLL();
            __userTypeBLL = BLLFactory.CreateUserTypeBLL();
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            __viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
            __viewUserEquipmentBLL = BLLFactory.CreateViewUserEquipmentBLL();
            __viewUsedConfirmBLL = BLLFactory.CreateViewUsedConfirmBLL();
            __viewMessageReceiveBLL = BLLFactory.CreateViewMessageReceiveBLL();
            __viewDepositRecordBLL = BLLFactory.CreateViewDepositRecordBLL();
            __viewOpenFundDepositRecordBLL = BLLFactory.CreateViewOpenFundDepositRecordBLL();
            __viewCurrentUsingEquipmentBLL = BLLFactory.CreateViewCurrentUsingEquipmentBLL();
            __viewEquipmentTrainningBLL = BLLFactory.CreateViewEquipmentTrainningBLL();
            __viewMaterialInfoBLL = BLLFactory.CreateViewMaterialInfoBLL();
            __viewMaterialPurchaseBLL = BLLFactory.CreateViewMaterialPurchaseBLL();
            __viewMaterialRecipientBLL = BLLFactory.CreateViewMaterialRecipientBLL();
            __viewMaterialOutputBLL = BLLFactory.CreateViewMaterialOutputBLL();
            __viewEquipmentAlarmBLL = BLLFactory.CreateViewEquipmentAlarmBLL();
            __viewEquipmentBrokenReportBLL = BLLFactory.CreateViewEquipmentBrokenReportBLL();
            __viewSampleApplyBLL = BLLFactory.CreateViewSampleApplyBLL();
        }
        public StatisticsInfo GetUserStatisticsInfo(Guid? userId)
        {
            var statisticsInfo = new StatisticsInfo();
            if (!userId.HasValue) return statisticsInfo;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return statisticsInfo;
            SetStatisticsInfoIsShowZero(user, ref statisticsInfo);
            SetStatisticsInfoAuditUser(user, ref statisticsInfo);
            SetStatisticsInfoAuditExperimenterSubject(user, ref statisticsInfo);
            SetStatisticsInfoAuditDepositRecord(user, ref statisticsInfo);
            SetStatisticsInfoAuditOpenFundDepositRecord(user, ref statisticsInfo);
            SetStatisticsInfoUsedConfirm(user, ref statisticsInfo);
            SetStatisticsInfoAuditUserEquipment(user, ref statisticsInfo);
            SetStatisticsInfoUnReadMessage(user, ref statisticsInfo);
            SetStatisticsInfoAppointment(user, ref statisticsInfo);
            SetStatisticsInfoEquipmentTrainning(user, ref statisticsInfo);
            SetStatisticsInfoMaterial(user, ref statisticsInfo);
            SetStatisticsInfoEquipmentAlarm(user, ref statisticsInfo);
            SetStatisticsInfoEquipmentBrokenReport(user, ref statisticsInfo);
            return statisticsInfo;
        }
        public void SetStatisticsInfoIsShowZero(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            var userSystemSettingList = __userSystemSettingBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id));
            if (userSystemSettingList != null && userSystemSettingList.Count() > 0)
            {
                var userSystemSetting = userSystemSettingList.First();
                statisticsInfo.IsShowZero = userSystemSetting.IsShowZeroStatisticsInfo.HasValue ? userSystemSetting.IsShowZeroStatisticsInfo.Value : false;
            }
        }
        public void SetStatisticsInfoAuditUser(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            var userPrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetUserPrivilige(user.Label);
            if (userPrivilige.IsEnableAudit || userPrivilige.IsEnableSetInvalid || userPrivilige.IsEnableSetBlacklist)
            {
                statisticsInfo.IsAuditUser = true;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                var isEnableList = userPrivilige.IsEnableList;
                var isEnableTutorList = userPrivilige.IsEnableTutorList;
                var isEnableStudentList = userPrivilige.IsEnableStudentList;
                var isEmpty = false;
                var isManageList = true;
                if(!isEnableList && isEnableTutorList)
                    dataGridSettings.AppendAndQueryExpression(string.Format("UserIdentity=-\"2\""));
                else if (!isEnableList && isEnableStudentList)
                {
                    isManageList = false;
                    var subject = __subjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", user.Id));
                    if (subject == null) isEmpty = true;
                    else
                    {
                        var experimenterSubjectList = __experimenterSubjectBLL.GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"", subject.Id));
                        if (experimenterSubjectList == null || experimenterSubjectList.Count() == 0) isEmpty = true;
                        else
                        {
                            dataGridSettings.AppendAndQueryExpression(experimenterSubjectList.Select(p => p.ExperimenterId.Value).ToFormatStr());
                            dataGridSettings.AppendAndQueryExpression(string.Format("UserIdentity=\"2\""));
                        }
                    }
                }
                if (!isEmpty)
                {
                    var customer = CustomerFactory.GetCustomer();
                    var isShowImportAuditUser = customer.GetIsShowImportAuditUser();
                    if (isShowImportAuditUser) 
                        dataGridSettings.AppendAndQueryExpression("IsDel=false");
                    else
                        dataGridSettings.AppendAndQueryExpression("IsDel=false*IsImported=false");
                    var userStatistCount = __viewUserBLL.GetUserManageUserStatistCountList(user.Id, dataGridSettings, null, false, null, isManageList);
                    if (userStatistCount != null && userStatistCount.Count() > 0)
                    {
                        statisticsInfo.AuditWaittingUser = userStatistCount.FirstOrDefault(p => p.UserStatus == UserStatus.AuditWaitting).UserCount;
                        statisticsInfo.AuditRejectUser = userStatistCount.FirstOrDefault(p => p.UserStatus == UserStatus.AuditReject).UserCount;
                    }
                }
            }
        }
        public void SetStatisticsInfoAuditExperimenterSubject(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            bool isTutor = user.UserTypeId.HasValue ? __userTypeBLL.IsTutorUserType(user.UserTypeId.Value) : false;
            if (isTutor)
            {
                var subject = __subjectBLL.GetSubjectByTurtorId(user.Id);
                if (subject != null)
                {
                    statisticsInfo.IsAuditExperimenterSubject = true;
                    if (subject.Experiments != null && subject.Experiments.Count() > 0)
                    {
                        statisticsInfo.AuditWaittingExperimenterSubject = subject.Experiments.Where(p => p.StatusEnum == ExperimenterSubjectStatus.UnAuthorized).Count();
                        statisticsInfo.AuditRejectExperimenterSubject = subject.Experiments.Where(p => p.StatusEnum == ExperimenterSubjectStatus.Stoped).Count();
                    }
                }
            }
        }
        public void SetStatisticsInfoAuditDepositRecord(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            var depositRecordPrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetDepositRecordPrivilige(user.Label);
            if (depositRecordPrivilige.IsEnableManage)
            {
                statisticsInfo.IsManageAuditWaittingDepositRecord = true;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                statisticsInfo.AuditWaittingDepositRecord = __viewDepositRecordBLL.GetViewDepositRecordCountByExpression(user.Id, dataGridSettings.AppendAndQueryExpression("HasChecked=false"));
            }
            if (depositRecordPrivilige.IsEnableMyManage)
            {
                statisticsInfo.IsPersonalAuditWaittingDepositRecord = true;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                statisticsInfo.PersonalAuditWaittingDepositRecord = __viewDepositRecordBLL.GetViewDepositRecordCountByExpression(user.Id, dataGridSettings.AppendAndQueryExpression(string.Format("UserId=\"{0}\"*HasChecked=false", user.Id)));
            }
        }
        public void SetStatisticsInfoAuditOpenFundDepositRecord(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            var depositRecordPrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetOpenFundApplyPrivilige(user.Label);
            if (depositRecordPrivilige.IsEnableDepositContainer)
            {
                statisticsInfo.IsManageAuditWaittingOpenFundDepositRecord = true;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                statisticsInfo.AuditWaittingOpenFundDepositRecord = __viewOpenFundDepositRecordBLL.GetViewOpenFundDepositRecordCountByExpression(user.Id, dataGridSettings.AppendAndQueryExpression("HasChecked=false"));
            }
            if (depositRecordPrivilige.IsEnableMyDepositContainer)
            {
                statisticsInfo.IsPersonalAuditWaittingOpenFundDepositRecord = true;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                statisticsInfo.PersonalAuditWaittingOpenFundDepositRecord = __viewOpenFundDepositRecordBLL.GetViewOpenFundDepositRecordCountByExpression(user.Id, dataGridSettings.AppendAndQueryExpression(string.Format("UserId=\"{0}\"*HasChecked=false", user.Id)));
            }
        }
        public void SetStatisticsInfoUsedConfirm(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            string todayQueryExpression = string.Format("EndAt>\"{0}\"*EndAt<\"{1}\"", DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var viewCurrentUsingEquipmentBLL = __viewCurrentUsingEquipmentBLL;
            var viewUsedConfirmBLL = __viewUsedConfirmBLL;
            
            var usedConfirmPrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetUsedConfirmPrivilige(user.Label);
            if (usedConfirmPrivilige.IsEnableGetGridViewCurrentUsingEquipmentList)
            {
                statisticsInfo.IsManageUsedConfirm = true;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                Dictionary<string, string> mapping = new Dictionary<string, string>();
                mapping["Id"] = "Id";
                statisticsInfo.UsingRecord = viewCurrentUsingEquipmentBLL.GetViewCurrentUsingEquipmentCountByExpression(user.Id, dataGridSettings, mapping, true, new string[] { "EquipmentAdminId" });
            }
            if (usedConfirmPrivilige.IsEnableGetGridTodayUsedConfirmList)
            {
                statisticsInfo.IsManageUsedConfirm = true;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                dataGridSettings.AppendAndQueryExpression(todayQueryExpression);
                statisticsInfo.TodayUsedRecord = viewUsedConfirmBLL.GetViewUsedConfirmCountByExpression(user.Id, dataGridSettings, null, true, new string[] { "EquipmentAdminId" });
            }
            if (usedConfirmPrivilige.IsEnableMyManage)
            {
                bool isTutor = user.UserTypeId.HasValue ? __userTypeBLL.IsTutorUserType(user.UserTypeId.Value) : false;
                if (isTutor)
                {
                    var subject = __subjectBLL.GetSubjectByTurtorId(user.Id);
                    if (subject != null)
                    {
                        statisticsInfo.IsExperimenterSubjectUsedConfirm = true;
                        DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                        Dictionary<string, string> mapping = new Dictionary<string, string>();
                        mapping["Id"] = "Id";
                        dataGridSettings.AppendAndQueryExpression(string.Format("SubjectId=\"{0}\"", subject.Id));
                        statisticsInfo.ExperimenterSubjectUsingRecord = viewCurrentUsingEquipmentBLL.CountModelListByExpression(dataGridSettings.QueryExpression, mapping, true, new string[] { "EquipmentAdminId" });
                        dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                        dataGridSettings.AppendAndQueryExpression(todayQueryExpression);
                        dataGridSettings.AppendAndQueryExpression(string.Format("SubjectId=\"{0}\"", subject.Id));
                        statisticsInfo.ExperimenterSubjectTodayUsedRecord = viewUsedConfirmBLL.CountModelListByExpression(dataGridSettings.QueryExpression, null, true, new string[] { "EquipmentAdminId" });
                    }
                }
                statisticsInfo.IsPersonalUsedConfirm = true;
                DataGridSettings dataGridSettingsPersonal = new DataGridSettings() { QueryExpression = "" };
                Dictionary<string, string> mappingPersonal = new Dictionary<string, string>();
                mappingPersonal["Id"] = "Id";
                dataGridSettingsPersonal.AppendAndQueryExpression(string.Format("UserId=\"{0}\"", user.Id));
                statisticsInfo.PersonalUsingRecord = viewCurrentUsingEquipmentBLL.CountModelListByExpression(dataGridSettingsPersonal.QueryExpression, mappingPersonal, true, new string[] { "EquipmentAdminId" });
                statisticsInfo.IsPersonalUsedConfirm = true;
                dataGridSettingsPersonal = new DataGridSettings() { QueryExpression = "" };
                dataGridSettingsPersonal.AppendAndQueryExpression(todayQueryExpression);
                dataGridSettingsPersonal.AppendAndQueryExpression(string.Format("UserId=\"{0}\"", user.Id));
                statisticsInfo.PersonalTodayUsedRecord = viewUsedConfirmBLL.CountModelListByExpression(dataGridSettingsPersonal.QueryExpression, null, true, new string[] { "EquipmentAdminId" });
            }
            //反馈记录var usedConfirmList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"*IsEnableAppoitmentWithFeedBack=true*IsFeedBack=false", equipmentId.ToString(), userId.Value.ToString()));
        }
        public void SetStatisticsInfoAuditUserEquipment(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            var userEquipmentPrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetUserEquipmentPrivilige(user.Label);
            if (userEquipmentPrivilige.IsEnablePass)
            {
                statisticsInfo.IsManageAuditWaittingUserEquipment = true;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                dataGridSettings.AppendAndQueryExpression(string.Format("Status=\"{0}\"", (int)UserEquipmentStatus.UnAudit));
                statisticsInfo.AuditWaittingUserEquipment = __viewUserEquipmentBLL.GetViewUserEquipmentCountByExpression(user.Id, dataGridSettings, null, true, new string[] { "EquipmentAdminId" });
            }
            statisticsInfo.IsPersonalAuditWaittingUserEquipment = true;
            DataGridSettings dataGridSettingsPersonal = new DataGridSettings() { QueryExpression = "" };
            dataGridSettingsPersonal.AppendAndQueryExpression(string.Format("Status=\"{0}\"", (int)UserEquipmentStatus.UnAudit));
            dataGridSettingsPersonal.AppendAndQueryExpression(string.Format("UserId=\"{0}\"", user.Id));
            statisticsInfo.PersonalAuditWaittingUserEquipment = __viewUserEquipmentBLL.CountModelListByExpression(dataGridSettingsPersonal.QueryExpression, null, true, new string[] { "EquipmentAdminId" });
        }
        public void SetStatisticsInfoUnReadMessage(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            statisticsInfo.IsUnReadMessage = true;
            statisticsInfo.UnReadMessage = __viewMessageReceiveBLL.CountModelListByExpression(string.Format("IsDeleted=false*ReceiveUserId=\"{0}\"*HasReaded=false*MsgType=-\"{1}\"", user.Id.ToString(), (int)MsgType.Draft));
        }
        public void SetStatisticsInfoAppointment(User user, ref StatisticsInfo statisticsInfo)
        {
            int days = 1;
            DateTime beginDate = DateTime.MinValue, endDate = DateTime.MaxValue;
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            var todayQueryExpression = __viewAppointmentBLL.GenerateRecentAppointmentQueryExpression(out beginDate, out endDate, out days);
            var appointmentPrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetAppointmentPrivilige(user.Label);
            if (appointmentPrivilige.IsEnableManage && appointmentPrivilige.IsEnableAppointmentTodayContainer)
            {
                statisticsInfo.IsManageAppointment = true;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };   
                dataGridSettings.AppendAndQueryExpression(todayQueryExpression);
                var userAppointmentCount = __viewAppointmentBLL.GetViewAppointmentCountByExpression(user.Id, dataGridSettings, null, true, new string[] { "EquipmentAdminId", "ExperimentationContent" }, true);
                if (userAppointmentCount != null)
                {
                    statisticsInfo.AppointmentToday = userAppointmentCount.AppointmentToday;
                    statisticsInfo.DoingAppointmentToday = userAppointmentCount.DoingAppointmentToday;
                    statisticsInfo.MayMissAppointmentToday = userAppointmentCount.MayMissAppointmentToday;     
                }
            }
            if (appointmentPrivilige.IsEnableAppointmentTodayContainer)
            {
                bool isTutor = user.UserTypeId.HasValue ? __userTypeBLL.IsTutorUserType(user.UserTypeId.Value) : false;
                if (isTutor)
                {
                    var subject = __subjectBLL.GetSubjectByTurtorId(user.Id);
                    if (subject != null)
                    {
                        statisticsInfo.IsExperimenterSubjectAppointment = true;
                        DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                        dataGridSettings.AppendAndQueryExpression(todayQueryExpression);
                        dataGridSettings.AppendAndQueryExpression(string.Format("SubjectId=\"{0}\"", subject.Id));
                        var userAppointmentCount = __viewAppointmentBLL.GetViewAppointmentCountByExpression(user.Id, dataGridSettings, null, true, new string[] { "EquipmentAdminId", "ExperimentationContent" }, false);
                        if (userAppointmentCount != null)
                        {
                            statisticsInfo.ExperimenterSubjectAppointmentToday = userAppointmentCount.AppointmentToday;
                            statisticsInfo.ExperimenterSubjectDoingAppointmentToday = userAppointmentCount.DoingAppointmentToday;
                            statisticsInfo.ExperimenterSubjectMayMissAppointmentToday = userAppointmentCount.MayMissAppointmentToday;
                        }
                    }
                }
                statisticsInfo.IsPersonalAppointment = true;
                DataGridSettings dataGridSettingsPersonal = new DataGridSettings() { QueryExpression = "" };
                dataGridSettingsPersonal.AppendAndQueryExpression(todayQueryExpression);
                dataGridSettingsPersonal.AppendAndQueryExpression(string.Format("UserId=\"{0}\"", user.Id));
                var userAppointmentCountPersonal = __viewAppointmentBLL.GetViewAppointmentCountByExpression(user.Id, dataGridSettingsPersonal, null, true, new string[] { "EquipmentAdminId", "ExperimentationContent" }, false);
                if (userAppointmentCountPersonal != null)
                {
                    statisticsInfo.PersonalAppointmentToday = userAppointmentCountPersonal.AppointmentToday;
                    statisticsInfo.PersonalDoingAppointmentToday = userAppointmentCountPersonal.DoingAppointmentToday;
                    statisticsInfo.PersonalMayMissAppointmentToday = userAppointmentCountPersonal.MayMissAppointmentToday;
                }
            }
        }
        public void SetStatisticsInfoEquipmentTrainning(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            statisticsInfo.IsAppliedEquipmentTrainning = true;
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
            dataGridSettings.AppendAndQueryExpression(string.Format("Status=\"{0}\"",(int)EquipmentTrainningStatus.Applied));
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            mapping["Id"] = "EquipmentTranningId";
            statisticsInfo.AppliedEquipmentTrainning = __viewEquipmentTrainningBLL.GetGridViewEquipmentTrainningCountByExpression(user.Id, dataGridSettings, mapping, true, new string[] { "EquipmentAdminId" });
        }
        public void SetStatisticsInfoMaterial(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            var materialInfoPrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetMaterialInfoPrivilige(user.Label);
            statisticsInfo.IsMaterialInfoWaittingPurchase = materialInfoPrivilige.IsEnableManage;
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
            dataGridSettings.AppendAndQueryExpression("DifValue<0+DifValue=0");
            statisticsInfo.MaterialInfoWaittingPurchase = __viewMaterialInfoBLL.GetManageViewMaterialInfoCountByExpression(user.Id, dataGridSettings);

            var materialPurchasePrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetMaterialPurchasePrivilige(user.Label);
            statisticsInfo.IsMaterialPurchaseWaittingInput = materialPurchasePrivilige.IsEnableManage;
            dataGridSettings = new DataGridSettings() { QueryExpression = "" };
            dataGridSettings.AppendAndQueryExpression(string.Format("Status=\"{0}\"",(int)MaterialPurchaseStatus.WaittingAudit));
            statisticsInfo.MaterialPurchaseWaittingInput = __viewMaterialPurchaseBLL.GetManageViewMaterialPurchaseCountByExpression(user.Id, dataGridSettings);

            var materialRecipientPrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetMaterialRecipientPrivilige(user.Label);
            statisticsInfo.IsMaterialRecipientWaittingAudit = materialRecipientPrivilige.IsEnableAudit;
            dataGridSettings = new DataGridSettings() { QueryExpression = "" };
            dataGridSettings.AppendAndQueryExpression(string.Format("Status=\"{0}\"", (int)MaterialRecipientStatus.WaittingAudit));
            statisticsInfo.MaterialRecipientWaittingAudit = __viewMaterialRecipientBLL.GetManageViewMaterialRecipientCountByExpression(user.Id, dataGridSettings);

            statisticsInfo.IsMaterialRecipientWaittingOutput = materialRecipientPrivilige.IsEnableRecipientOutput;
            dataGridSettings = new DataGridSettings() { QueryExpression = "" };
            dataGridSettings.AppendAndQueryExpression(string.Format("Status=\"{0}\"", (int)MaterialRecipientStatus.AuditPass));
            statisticsInfo.MaterialRecipientWaittingOutput = __viewMaterialRecipientBLL.GetManageViewMaterialRecipientCountByExpression(user.Id, dataGridSettings);


            var materialOutputPrivilige = com.Bynonco.LIMS.BLL.Business.Privilige.PriviligeFactory.GetMaterialOutputPrivilige(user.Label);
            statisticsInfo.IsMaterialOutputWaittingDeduct = materialOutputPrivilige.IsEnableDeduct;
            dataGridSettings = new DataGridSettings() { QueryExpression = "" };
            dataGridSettings.AppendAndQueryExpression(string.Format("Status=\"{0}\"", (int)MaterialOutputStatus.UnDeduct));
            statisticsInfo.MaterialOutputWaittingDeduct = __viewMaterialOutputBLL.GetManageViewMaterialOutputCountByExpression(user.Id, dataGridSettings);
        }
        public void SetStatisticsInfoEquipmentAlarm(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            statisticsInfo.IsManageAlarmEquipment = true;
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
            dataGridSettings.AppendAndQueryExpression(string.Format("Status=\"{0}\"*IsDelete=false", (int)EquipmentAlarmStatus.AlarmOpen));
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            mapping["Id"] = "EquipmentAlarmId";
            statisticsInfo.AlarmEquipment = __viewEquipmentAlarmBLL.GetViewEquipmentAlarmCountByExpression(user.Id, dataGridSettings, mapping, true, new string[] { "EquipmentAdminId" });
        }
        public void SetStatisticsInfoEquipmentBrokenReport(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            statisticsInfo.IsManageBrokenEquipment = true;
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            mapping["Id"] = "EquipmentBrokenReportId";
            statisticsInfo.BrokenEquipment = __viewEquipmentBrokenReportBLL.GetViewEquipmentBrokenReportCountByExpression(user.Id, dataGridSettings, mapping, true, new string[] { "EquipmentAdminId" });
        }
        public void SetStatisticsInfoSampleApply(User user, ref StatisticsInfo statisticsInfo)
        {
            if (statisticsInfo == null) statisticsInfo = new StatisticsInfo();
            if (user == null) return;
            statisticsInfo.UnAuditSampleApplyCount = __viewSampleApplyBLL.GetUnAuditSampleApplyCount(user.Id);
        }
    }
}
