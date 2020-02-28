using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.JqueryEasyUI.Core;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentBLL : BLLBase<Equipment>, IEquipmentBLL
    {
        private IUserBLL __userBLL;
        private ISystemLogBLL __systemLogBLL;
        private IEquipmentLogBLL __equipmentLogBLL;
        private IPublicHolidaysBLL __publicHolidaysBLL;
        private AppointmentRelativeBLL __appointmentTimeRelativeBLL;
        public EquipmentBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __systemLogBLL = BLLFactory.CreateSystemLogBLL();
            __equipmentLogBLL = BLLFactory.CreateEquipmentLogBLL();
            __publicHolidaysBLL = BLLFactory.CreatePublicHolidaysBLL();
            __appointmentTimeRelativeBLL = new AppointmentRelativeBLL(__publicHolidaysBLL.GetValidatePublicHolidaysListForAppointment, __publicHolidaysBLL.ValidateForAppointment);
        }
        private bool DelEquipment(Guid equipmentId, ref XTransaction tran,Guid? operatorId,string operateIP, out string errorMessage)
        {
            IEquipmentLinkmanBLL equipmentLinkmanBLL = BLLFactory.CreateEquipmentLinkmanBLL();
            IEquipmentCategoryBindBLL equipmentCategoryBindBLL = BLLFactory.CreateEquipmentCategoryBindBLL();
            IUserWorkScopeBLL userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            IUserEquipmentBLL userEquipmentBLL = BLLFactory.CreateUserEquipmentBLL();
            ICalcFeeTimeRuleBLL calcFeeTimeRuleBLL = BLLFactory.CreateCalcFeeTimeRuleBLL();
            IChargeStandardBLL chargeStandardBLL = BLLFactory.CreateChargeStandardBLL();
            IEquipmentLabelAddtionChargeItemBLL equipmentLabelAddtionChargeItemBLL = BLLFactory.CreateEquipmentLabelAddtionChargeItemBLL();
            IEquipmentAdditionChargeItemBLL equipmentAdditionChargeItemBLL = BLLFactory.CreateEquipmentAdditionChargeItemBLL();
            IEquipmentBlackListMessageTemplateBLL equipmentBlackListMessageTemplateBLL = BLLFactory.CreateEquipmentBlackListMessageTemplateBLL();
            IEquipmentLabelChargeStandardBLL equipmentLabelChargeStandardBLL = BLLFactory.CreateEquipmentLabelChargeStandardBLL();
            IEquipmentBlackListBLL equipmentBlackListBLL = BLLFactory.CreateEquipmentBlackListBLL();
            IEquipmentTranningBLL equipmentTrainningBLL = BLLFactory.CreateEquipmentTranningBLL();
            IUnAppointmentPeriodTimeBLL unAppointmentPeriodTimeBLL = BLLFactory.CreateUnAppointmentPeriodTimeBLL();
            ITagEquipmentFunBLL tagEquipmentFunBLL = BLLFactory.CreateTagEquipmentFunBLL();
            IEquipmentRepairBLL equipmentRepairBLL = BLLFactory.CreateEquipmentRepairBLL();
            IEquipmentStatusLogBLL equipmentStatusLogBLL = BLLFactory.CreateEquipmentStatusLogBLL();
            IEquipmentMarkingBLL equipmentMarkingBLL = BLLFactory.CreateEuipmentMarkingBLL();
            IUserEquipmentTimeUserBLL userEquipmentTimeUserBLL = BLLFactory.CreateUserEquipmentTimeUserBLL();
            IUserEquipmentTimeBLL userEquipmentTimeBLL = BLLFactory.CreateUserEquipmentTimeBLL();
            IEquipmentNoticeBLL equipmentNoticeBLL = BLLFactory.CreateEquipmentNoticeBLL();
            IEquipmentNoticeReadBLL equipmentNoticeReadBLL = BLLFactory.CreateEquipmentNoticeReadBLL();
            IEquipmentNoticeAttachmentBLL equipmentNoticeAttachmentBLL = BLLFactory.CreateEquipmentNoticeAttachmentBLL();
            IEquipmentLabelItemBLL equipmentLabelItemBLL = BLLFactory.CreateEquipmentLabelItemBLL();
            IEquipmentLabelBLL equipmentLabelBLL = BLLFactory.CreateEquipmentLabelBLL();
            IHolidaysExcludeBLL holidaysExcludeBLL = BLLFactory.CreateHolidaysExcludeBLL();
            IHolidaysIncludeBLL holidaysIncludeBLL = BLLFactory.CreateHolidaysIncludeBLL();
            ISampleItemBLL sampleItemBLL = BLLFactory.CreateSampleItemBLL();
            IEquipmentRelationBLL equipmentRelationBLL = BLLFactory.CreateEquipmentRelationBLL();
            IDoorOfflineAuthorizationBLL doorOfflineAuthorizationBLL = BLLFactory.CreateDoorOfflineAuthorizationBLL();
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var equipment = GetEntityById(equipmentId);
                if (equipment == null) string.Format("出错,找不到编码为【{0}】的设备信息", equipmentId);
                if (equipment.Linkmen != null && equipment.Linkmen.Count > 0)
                    equipmentLinkmanBLL.Delete(equipment.Linkmen.Select(p => p.Id), ref tran, true);
                if (equipment.CategoryBinds != null && equipment.CategoryBinds.Count > 0)
                    equipmentCategoryBindBLL.Delete(equipment.CategoryBinds.Select(p => p.Id), ref tran, true);
                if (equipment.Directors != null && equipment.Directors.Count > 0)
                    userWorkScopeBLL.Delete(equipment.Directors.Select(p => p.Id), ref tran, true);
                if (equipment.UserEquipments != null && equipment.UserEquipments.Count > 0)
                    userEquipmentBLL.Delete(equipment.UserEquipments.Select(p => p.Id), ref tran, true);
                if (equipment.CalcFeeTimeRule != null)
                    calcFeeTimeRuleBLL.Delete(new Guid[] { equipment.CalcFeeTimeRule.Id }, ref tran, true);
                if (equipment.ChargeStandard != null)
                    chargeStandardBLL.Delete(new Guid[] { equipment.ChargeStandard.Id }, ref tran, true);
                if (equipment.AdditionChargeItems != null && equipment.AdditionChargeItems.Count > 0)
                {
                    foreach (var equipmentAdditionChargeItem in equipment.AdditionChargeItems)
                        equipmentAdditionChargeItem.IsDelete = true;
                    equipmentAdditionChargeItemBLL.Save(equipment.AdditionChargeItems, ref tran, true);
                }
                if (equipment.LabelAddtionChargeItems != null && equipment.LabelAddtionChargeItems.Count > 0)
                    equipmentLabelAddtionChargeItemBLL.Delete(equipment.LabelAddtionChargeItems.Select(p => p.Id), ref tran, true);
                if (equipment.LabelChargeStandards != null && equipment.LabelChargeStandards.Count > 0)
                    equipmentLabelChargeStandardBLL.Delete(equipment.LabelChargeStandards.Select(p => p.Id), ref tran, true);
                if (equipment.BlackListMessageTemplate != null)
                    equipmentBlackListMessageTemplateBLL.Delete(new Guid[] { equipment.BlackListMessageTemplate.Id }, ref tran, true);
                if (equipment.BlackList != null && equipment.BlackList.Count > 0)
                    equipmentBlackListBLL.Delete(equipment.BlackList.Select(p => p.Id), ref tran, true);
                if (equipment.Trainnins != null && equipment.Trainnins.Count > 0)
                    equipmentTrainningBLL.Delete(equipment.Trainnins.Select(p => p.Id), ref tran, true);
                if (equipment.UnAppointmentPeriodTimes != null && equipment.UnAppointmentPeriodTimes.Count > 0)
                    unAppointmentPeriodTimeBLL.Delete(equipment.UnAppointmentPeriodTimes.Select(p => p.Id), ref tran, true);
                if (equipment.TagEquipmentFuns != null && equipment.TagEquipmentFuns.Count > 0)
                    tagEquipmentFunBLL.Delete(equipment.TagEquipmentFuns.Select(p => p.Id), ref tran, true);
                if (equipment.Repairs != null && equipment.Repairs.Count > 0)
                    equipmentRepairBLL.Delete(equipment.Repairs.Select(p => p.Id), ref tran, true);
                if (equipment.StatusLogs != null && equipment.StatusLogs.Count > 0)
                    equipmentStatusLogBLL.Delete(equipment.StatusLogs.Select(p => p.Id), ref tran, true);
                if (equipment.Markings != null && equipment.Markings.Count > 0)
                    equipmentMarkingBLL.Delete(equipment.Markings.Select(p => p.Id), ref tran, true);
                var sampleItems = sampleItemBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*IsDelete=false", equipmentId));
                if (sampleItems != null && sampleItems.Count > 0)
                {
                    foreach (var sampleItem in sampleItems)
                        sampleItem.IsDelete = true;
                    sampleItemBLL.Save(sampleItems, ref tran, true);
                }
                if (equipment.UserEquipmentTimes != null && equipment.UserEquipmentTimes.Count > 0)
                {
                    foreach (var userEquipmentTime in equipment.UserEquipmentTimes)
                    {
                        if (userEquipmentTime.Users != null && userEquipmentTime.Users.Count > 0)
                            userEquipmentTimeUserBLL.Delete(userEquipmentTime.Users.Select(p => p.Id), ref tran, true);
                    }
                    userEquipmentTimeBLL.Delete(equipment.UserEquipmentTimes.Select(p => p.Id), ref tran, true);
                }
                if (equipment.Notices != null && equipment.Notices.Count > 0)
                {
                    foreach (var equipmentNotice in equipment.Notices)
                    {
                        var equipmentNoticeReadList = equipmentNoticeReadBLL.GetEntitiesByExpression(string.Format("EquipmentNoticeId=\"{0}\"", equipmentNotice.Id));
                        if (equipmentNoticeReadList != null && equipmentNoticeReadList.Count > 0)
                            equipmentNoticeReadBLL.Delete(equipmentNoticeReadList.Select(p => p.Id), ref tran, true);
                        if (equipmentNotice.Attachments != null && equipmentNotice.Attachments.Count > 0)
                            equipmentNoticeAttachmentBLL.Delete(equipmentNotice.Attachments.Select(p => p.Id), ref tran, true);
                    }
                    equipmentNoticeBLL.Delete(equipment.Notices.Select(p => p.Id), ref tran, true);
                }

                if (equipment.Labels != null && equipment.Labels.Count > 0)
                {
                    foreach (var label in equipment.Labels)
                    {
                        if (label.LabelItems != null && label.LabelItems.Count > 0)
                            equipmentLabelItemBLL.Delete(label.LabelItems.Select(p => p.Id), ref tran, true);
                    }
                    equipmentLabelBLL.Delete(equipment.Labels.Select(p => p.Id), ref tran, true);
                }
                var holidaysExcludeList = holidaysExcludeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", equipment.Id.ToString()));
                if (holidaysExcludeList != null && holidaysExcludeList.Count > 0)
                    holidaysExcludeBLL.Delete(holidaysExcludeList.Select(p => p.Id), ref tran, true);
                var holidaysIncludeList = holidaysIncludeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"",equipment.Id.ToString()));
                if (holidaysIncludeList != null && holidaysIncludeList.Count > 0)
                    holidaysIncludeBLL.Delete(holidaysIncludeList.Select(p => p.Id), ref tran, true);
                if (equipment.EquipmentRelationList != null && equipment.EquipmentRelationList.Count() > 0)
                    equipmentRelationBLL.Delete(equipment.EquipmentRelationList.Select(p => p.Id), ref tran, true);
                var doorOfflineAuthorizationList = doorOfflineAuthorizationBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*IsDel=false", equipment.Id));
                if(doorOfflineAuthorizationList != null && doorOfflineAuthorizationList.Count() > 0)
                {
                    foreach (var item in doorOfflineAuthorizationList)
                    {
                        item.IsDel = true;
                        item.Updated = false;
                        item.LeamsUpdated = false;
                    }
                    doorOfflineAuthorizationBLL.Save(doorOfflineAuthorizationList, ref tran, true);
                }
                GernateEquipmentOperateBasicLog(OperateType.Deleted, equipment, equipment, operatorId, operateIP, ref tran);
                equipment.IsDelete = true;
                equipment.IP = "";
                equipment.Label = "";
                equipment.LeamsUpdated = false;
                Save(new Equipment[] { equipment }, ref tran, true);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool DelEquipment(Guid equipmentId,Guid? operatorId,string operateIP, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelEquipment(equipmentId, ref tran, operatorId, operateIP, out errorMessage);
        }
        public bool DelEquipments(IList<Equipment> equipments,Guid? operatorId,string operateIP, out  string errorMessage)
        {
            errorMessage = "";
            if (equipments != null && equipments.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var equipment in equipments)
                    {
                        var result = DelEquipment(equipment.Id, ref tran, operatorId, operateIP, out errorMessage);
                        if (!result)
                            throw new Exception(errorMessage);
                    }
                    if (tran.HasTransaction) tran.CommitTransaction();
                }
                catch (Exception ex)
                {
                    if (tran.HasTransaction) tran.CommitTransaction();
                    errorMessage = ex.Message;
                    return false;
                }
                finally { tran.Dispose(); }
            }
            return true;
        }
        public int GetUserEquipmentAppoitmentDaysBeforeBeginDate(User user, int appointmentTimeDays, int aHaedOfTime, IEnumerable<DayOfWeek> equipmentWorkDays, DateTime beginDate)
        {
            return __appointmentTimeRelativeBLL.GetUserAppoitmentDaysBeforeBeginDate(user, appointmentTimeDays, aHaedOfTime, equipmentWorkDays, beginDate);
        }

        public EquipmentAppointmentTimes GetEquipmentAppoitmentTimes(Guid equipmentId, EquipmentTimeAppointmemtMode equipmentTimeAppointmemtMode, Guid? userId, int aHaedOfTime, int maxAdvanceTime, int aHeadOfCancelTime, ref int appointmentTimeDays, int appointmentTimeStep, string workDays, string workHours, DateTime beginDate, DateTime endDate)
        {
            return __appointmentTimeRelativeBLL.GetAppoitmentTimes(equipmentId, equipmentTimeAppointmemtMode, userId, aHaedOfTime, maxAdvanceTime, aHeadOfCancelTime, ref appointmentTimeDays, appointmentTimeStep, workDays, workHours, beginDate, endDate);
        }
        public EquipmentAppointmentTimes GetEquipmentAppoitmentTimes(EquipmentTimeAppointmemtMode equipmentTimeAppointmemtMode,Guid? userId, Guid equipmentId, DateTime beginDate, DateTime endDate, out int appointmentTimeDays)
        {
            var equipemt = GetEntityById(equipmentId);
            if (equipemt == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的设备信息", equipmentId.ToString()));
            var aHeadOfTime = equipemt.OpenAdvanceTime;
            var maxAdvanceTime = equipemt.MaxAppointmentAdvanceTime ?? 0;
            var aHeadOfCancelTime = equipemt.MinAppointmentCancelTime ?? 5;
            appointmentTimeDays = equipemt.AppointmentDays ?? 0;
            var appointmentTimeStep = equipemt.AppointmentTimeStep ?? 30;
            var workDays = equipemt.WorkDays;
            var workHours = equipemt.WorkHours;
            return GetEquipmentAppoitmentTimes(equipmentId,equipmentTimeAppointmemtMode, userId, aHeadOfTime, maxAdvanceTime, aHeadOfCancelTime, ref appointmentTimeDays, appointmentTimeStep, workDays, workHours, beginDate, endDate);
        }
        public EquipmentAppointmentTimes GetSampleSendTimes(Guid? userId, Guid equipmentId, DateTime beginDate, DateTime endDate)
        {
            IEquipmentBLL equipmentBLL = BLLFactory.CreateEquipmentBLL();
            var equipment = equipmentBLL.GetEntityById(equipmentId);
            if (equipment == null) throw new Exception(string.Format("找不到编码为【{0}】的设备信息", equipmentId));
            int aHaedOfTime = 0, maxAdvanceTime = 0,aHeadOfCancelTime = 0, appointmentTimeDays = 0, appointmentTimeStep = 0;
            appointmentTimeDays = equipment.SampleEnableSendDays;
            appointmentTimeStep = equipment.SampleSendTimeInerval;
            string workDays = equipment.SampleEnableSendWeekDays;
            string workHours = equipment.SampleSendTimes;
            return GetEquipmentAppoitmentTimes(equipmentId,EquipmentTimeAppointmemtMode.CommondCalendar,userId, aHaedOfTime, maxAdvanceTime, aHeadOfCancelTime, ref appointmentTimeDays, appointmentTimeStep, workDays, workHours, beginDate, endDate);
        }
        public EquipmentCurStatusInfo GetEquipmentStatus(Guid equipmentId)
        {
            var usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
            var viewSampleTestRecordBLL = BLLFactory.CreateViewSampleTestRecordBLL();
            var viewCurrentUsingEquipmentByPowerOperationBLL = BLLFactory.CreateViewCurrentUsingEquipmentByPowerOperationBLL();
            var appointmentBLL = BLLFactory.CreateAppointmentBLL();
            EquipmentCurStatusInfo equipmentCurStatusInfo = new EquipmentCurStatusInfo() { EquipmentStatus = EquipmentCurStatus.Free, Remark = EquipmentCurStatus.Free.ToCnName() };
            var maxStartUsingRecord = usedConfirmBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*EndAt=null", equipmentId.ToString()), null, "BeginAt desc");//获取设备最新的开机记录
            var maxUsedConfirmRecord = usedConfirmBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*EndAt=-null", equipmentId.ToString()), null, "EndAt desc");//获取设备最新的使用记录
            var maxStartTestRecord = viewSampleTestRecordBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*Status={1}*BeginTime=-null", equipmentId.ToString(), (int)SampleApplyStatus.BeginTest), null, "EndTime desc");
            var maxTestRecord = viewSampleTestRecordBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*Status={1}*BeginTime=-null", equipmentId.ToString(), (int)SampleApplyStatus.FinishTest), null, "EndTime desc");
            var maxPowerOperation = viewCurrentUsingEquipmentByPowerOperationBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentID=\"{0}\"", equipmentId.ToString()), null, "OperationTime D");
            if (maxStartUsingRecord == null && maxStartTestRecord == null && maxPowerOperation == null) return equipmentCurStatusInfo;
            //-----获取最大开机时时开始-----
            DateTime maxBeginDate = DateTime.MinValue;
            if (maxStartUsingRecord != null) maxBeginDate = maxStartUsingRecord.BeginAt.Value;
            if (maxStartTestRecord != null && maxStartTestRecord.BeginTime.Value > maxBeginDate) maxBeginDate = maxStartTestRecord.BeginTime.Value;
            if (maxUsedConfirmRecord != null && maxUsedConfirmRecord.BeginAt.Value > maxBeginDate) maxBeginDate = maxUsedConfirmRecord.BeginAt.Value;
            if (maxTestRecord != null && maxTestRecord.BeginTime.Value > maxBeginDate) maxBeginDate = maxTestRecord.BeginTime.Value;
            if (maxPowerOperation != null && maxPowerOperation.OperationTime.Value > maxBeginDate) maxBeginDate = maxPowerOperation.OperationTime.Value;
            //-----获取最大开机时时结束-----
            if (maxUsedConfirmRecord != null && maxUsedConfirmRecord.BeginAt.Value == maxBeginDate) return equipmentCurStatusInfo;//最大开机时间等于已完成使用记录的最大开始时间直接返回
            if (maxTestRecord != null && maxTestRecord.BeginTime.Value == maxBeginDate) return equipmentCurStatusInfo;//最大开机时间等于已完成送样检测的最大开始时间直接返回
            if (maxStartTestRecord != null && maxStartTestRecord.BeginTime >= maxBeginDate)
            {
                equipmentCurStatusInfo.EquipmentStatus = EquipmentCurStatus.Testing;
                equipmentCurStatusInfo.SampleItemName = maxStartTestRecord.SampleItemName;
                equipmentCurStatusInfo.SampleNo = maxStartTestRecord.SampleNo;
                equipmentCurStatusInfo.TestBeginTime = maxStartTestRecord.BeginTime;
                equipmentCurStatusInfo.TestEndTime = maxStartTestRecord.EndTime;
                equipmentCurStatusInfo.UserId = maxStartTestRecord.TesterId;
                equipmentCurStatusInfo.UserName = maxStartTestRecord.TesterName;
                equipmentCurStatusInfo.Remark = string.Format("当前测试人:{0},测试项目:{1},样品编号:{2}", equipmentCurStatusInfo.UserName, equipmentCurStatusInfo.SampleItemName, equipmentCurStatusInfo.SampleNo);
            }
            if (maxStartUsingRecord != null && maxStartUsingRecord.BeginAt.Value >= maxBeginDate)
            {
                equipmentCurStatusInfo.EquipmentStatus = EquipmentCurStatus.Using;
                equipmentCurStatusInfo.UserId = maxStartUsingRecord.UserId;
                equipmentCurStatusInfo.UserName = maxStartUsingRecord.User.UserName;
                equipmentCurStatusInfo.Remark = string.Format("当前使用者:{0}", equipmentCurStatusInfo.UserName);
                if (!string.IsNullOrWhiteSpace(maxStartUsingRecord.User.PhoneNumber) || !string.IsNullOrWhiteSpace(maxStartUsingRecord.User.FixedPhone))
                    equipmentCurStatusInfo.Remark += string.Format(", 联系电话:{0}", !string.IsNullOrWhiteSpace(maxStartUsingRecord.User.PhoneNumber) ? maxStartUsingRecord.User.PhoneNumber : maxStartUsingRecord.User.FixedPhone);
                if (maxStartUsingRecord.AppointmentId.HasValue)
                {
                    var appointments = appointmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", maxStartUsingRecord.AppointmentId.ToString()));
                    if (appointments != null && appointments.Count > 0)
                    {
                        var appointment = appointments.First();
                        equipmentCurStatusInfo.AppointmentBeginTime = appointment.BeginTime;
                        equipmentCurStatusInfo.AppointmentEndTime = appointment.EndTime;
                        equipmentCurStatusInfo.Remark += string.Format(",预约时间:{0}-{1}", equipmentCurStatusInfo.AppointmentBeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), equipmentCurStatusInfo.AppointmentEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                return equipmentCurStatusInfo;
            }

            if (maxPowerOperation != null && maxPowerOperation.OperationTime.Value >= maxBeginDate)
            {
                equipmentCurStatusInfo.EquipmentStatus = EquipmentCurStatus.Using;
                equipmentCurStatusInfo.UserId = maxPowerOperation.UserID.Value;
                equipmentCurStatusInfo.UserName = maxPowerOperation.UserName;
                equipmentCurStatusInfo.Remark = string.Format("开机用户:{0},开机时间:{1}", maxPowerOperation.UserName, maxPowerOperation.OperationTime.Value.ToString("yyyy-MM-dd HH:mm"));
            }
            return equipmentCurStatusInfo;
        }
        public EquipmentTotalInfo GetEquipmentTotalInfo(Guid equipmentId)
        {
            var equipment = GetEntityById(equipmentId);
            EquipmentTotalInfo equipmentTotalInfo = new EquipmentTotalInfo(equipment.Id, equipment.Name);
            equipmentTotalInfo.TotalFocus = (int)GetSingleResult(string.Format("SELECT COUNT(*) FROM UserEquipment WHERE EquipmentId='{0}'", equipment.Id.ToString()));
            equipmentTotalInfo.TotalHour = Convert.ToDouble(GetSingleResult(string.Format("SELECT IsNull(sum(IsNull(datediff(Hour,BeginAt,EndAt),0)),0) FROM UsedConfirm WHERE BeginAt IS NOT NULL AND EndAt IS NOT NULL AND EquipmentId='{0}'", equipment.Id.ToString())));
            equipmentTotalInfo.TotalUsedTimes = (int)GetSingleResult(string.Format("SELECT COUNT(*) FROM UsedConfirm WHERE BeginAt IS NOT NULL AND EndAt IS NOT NULL AND EquipmentId='{0}'", equipment.Id.ToString()));
            equipmentTotalInfo.TotalUsers = (int)GetSingleResult(string.Format("SELECT IsNull(sum(t.quatity),0) FROM (SELECT COUNT(*) quatity FROM UsedConfirm WHERE BeginAt IS NOT NULL AND EndAt IS NOT NULL AND EquipmentId='{0}' GROUP BY UserId) t", equipment.Id.ToString()));
            return equipmentTotalInfo;
        }
        public IList<Equipment> GetUserManageEquipmentList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false)
        {
            if (!userId.HasValue) return null;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return null;
            if (!user.IsSuperAdmin)
            {
                var userWorkScopeList = BLLFactory.CreateUserWorkScopeBLL().GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id));
                var queryExpression = "";
                if (userWorkScopeList != null && userWorkScopeList.Count() > 0)
                    queryExpression = userWorkScopeList.Select(p => p.EquipmentId).ToFormatStr();

                var queryOrgId = BLLFactory.CreateWorkGroupModuleBLL().GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.Equipment, "", "", "OrgId"), null);
                if ((queryOrgId == "Id=null" || queryOrgId == "") && queryExpression == "") queryExpression = "Id=null";
                else if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression += (queryExpression == "" ? "" : "+") + queryOrgId;
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct);
        }
        public IList<Equipment> GetEquipmentListByLabels(string[] labels)
        {
            if (labels == null || labels.Length == 0) return null;
            string queryExpression = "";
            foreach (var label in labels)
            {
                if(string.IsNullOrWhiteSpace(label)) continue;
                queryExpression += string.IsNullOrWhiteSpace(queryExpression) ? string.Format("Label=\"{0}\"", label.Trim()) : string.Format("+Label=\"{0}\"", label.Trim());
            }
            if (queryExpression == "") return null;
            return GetEntitiesByExpression(queryExpression);
        }
        public string GetTutorUnAuditMessage()
        {
            return __appointmentTimeRelativeBLL.GetTutorUnAuditMessage();
        }
        public string GetUnTrainningMessage()
        {
            return __appointmentTimeRelativeBLL.GetUnTrainningMessage();
        }
        public string GetUnAuditMessage()
        {
            return __appointmentTimeRelativeBLL.GetUnAuditMessage();
        }
        public bool SaveImportEquipment(IList<ImportEquipmentLog> importEquipmentLogList, out string errorMessage)
        {
            errorMessage = "";
            if (importEquipmentLogList != null && importEquipmentLogList.Count() == 0)
            {
                errorMessage = "导入数据为空";
                return false;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var equipmentLinkmanBLL = BLLFactory.CreateEquipmentLinkmanBLL();
                var equipmentCategoryBindBLL = BLLFactory.CreateEquipmentCategoryBindBLL();
                var calcFeeTimeRuleBLL = BLLFactory.CreateCalcFeeTimeRuleBLL();
                var chargeStandardBLL = BLLFactory.CreateChargeStandardBLL();

                IList<Equipment> equipmentList = new List<Equipment>();
                foreach (var item in importEquipmentLogList)
                {
                    var equipment = new Equipment();
                    bool isNew = false;
                    var equipmentLabelList = GetEquipmentListByLabels(new string[] { item.Label });
                    if (equipmentLabelList != null && equipmentLabelList.Count() > 0)
                    {
                        equipment = equipmentLabelList.First();
                        isNew = true;
                    }
                    else equipment.Id = Guid.NewGuid();
                    if (equipmentList.Where(p => p.Label == equipment.Label).FirstOrDefault() != null) continue;
                    if (!string.IsNullOrWhiteSpace(item.Status)) equipment.Status = int.Parse(item.Status);
                    equipment.Name = item.Name;
                    equipment.Label = item.Label;
                    equipment.UnitPrice = item.UnitPrice;
                    equipment.InputStr = ShortcutStringUtility.GetFirstPYLetter(equipment.Name);
                    equipment.Model = item.Model;
                    equipment.Specifications = item.Specifications;
                    equipment.SpecimenNoticeNoHTML = item.Specifications;
                    equipment.ProductionArea = item.ProductionArea;
                    equipment.Factory = item.Factory;
                    equipment.Brand = item.Brand;
                    equipment.ProductionNo = item.ProductionNo;
                    equipment.Supplier = item.Supplier;
                    equipment.ProductionDate = item.ProductionDate;
                    equipment.BuyDate = item.BuyDate;
                    //equipment.ComeFrom = item.ComeFrom;
                    equipment.OrgId = item.OrgId;
                    equipment.RoomId = item.RoomId;
                    equipment.LabRoomName = item.RoomName;

                    if (!string.IsNullOrWhiteSpace(item.UseDirection)) equipment.UseDirection = int.Parse(item.UseDirection);
                    equipment.IsLargeScaleEquipment = item.IsLargeScaleEquipment.HasValue ? item.IsLargeScaleEquipment.Value : false;
                    equipment.LinkTelNo = item.LinkTelNo;
                    equipment.AdminEmail = item.AdminEmail;
                    equipment.Qualification = item.Qualification;
                    equipment.QualificationNoHTML = item.Qualification;
                    equipment.ScopeOfApplication = item.ScopeOfApplication;
                    equipment.ScopeOfApplicationNoHTML = item.ScopeOfApplication;
                    equipment.SpecimenNotice = item.SpecimenNotice;
                    equipment.SpecimenNoticeNoHTML = item.SpecimenNotice;
                    equipment.UseNotice = item.UseNotice;
                    equipment.UseNoticeNoHTML = item.UseNotice;
                    equipment.IsDelete = false;
                    if (!equipment.IsLargeScaleEquipment.HasValue) equipment.IsLargeScaleEquipment = false;
                    if (!equipment.Status.HasValue || equipment.Status.Value == 0) equipment.Status = (int)EquipmentStatus.Normal;
                    if (isNew) Save(new Equipment[] { equipment }, ref tran, true);
                    else Add(new Equipment[] { equipment }, ref tran, true);

                    if(isNew && equipment.CategoryBinds != null && equipment.CategoryBinds.Count() > 0)
                    {
                        equipmentCategoryBindBLL.Delete(equipment.CategoryBinds.Select(p => p.Id), ref tran, true);
                    }
                    if (item.CategorieId.HasValue)
                    {
                        var equipmentCategoryBind = new EquipmentCategoryBind();
                        equipmentCategoryBind.Id = Guid.NewGuid();
                        equipmentCategoryBind.EquipmentId = equipment.Id;
                        equipmentCategoryBind.EquipmentCategoryId = item.CategorieId.Value;
                        equipmentCategoryBindBLL.Add(new EquipmentCategoryBind[] { equipmentCategoryBind }, ref tran, true);
                    }
                    //equipment.UserEquipments = new List<UserEquipment>();
                    //equipment.Directors
                    //equipment.Linkmen
                    if (equipment.CalcFeeTimeRule == null)
                    {
                        equipment.CalcFeeTimeRule = new CalcFeeTimeRule();
                        equipment.CalcFeeTimeRule.Id = Guid.NewGuid();
                        equipment.CalcFeeTimeRule.EquipmentId = equipment.Id;
                        equipment.CalcFeeTimeRule.Expression = "return t2-t1;";
                        equipment.CalcFeeTimeRule.ReadableExpression = "使用结束时间-开始使用时间";
                        equipment.CalcFeeTimeRule.RoundDigits = 2;
                        equipment.CalcFeeTimeRule.RoundFator = 0;
                        calcFeeTimeRuleBLL.Add(new CalcFeeTimeRule[] { equipment.CalcFeeTimeRule }, ref tran, true);
                    }
                    if (equipment.ChargeStandard == null)
                    {
                        equipment.ChargeStandard = new ChargeStandard();
                        equipment.ChargeStandard.ChargeTypeEnum = com.Bynonco.LIMS.Model.Enum.ChargeType.ByHour;
                        equipment.ChargeStandard.EquipmentId = equipment.Id;
                        equipment.ChargeStandard.Id = Guid.NewGuid();
                        equipment.ChargeStandard.StandardPrice = 0d;
                        chargeStandardBLL.Add(new ChargeStandard[] { equipment.ChargeStandard }, ref tran, true);
                    }
                    equipmentList.Add(equipment);
                }
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
        public override bool Save(IEnumerable<Equipment> entities, ref XTransaction tran, bool isSupress = false)
        {
            if (entities != null && entities.Count() > 0)
            {
                foreach (var item in entities) item.LeamsUpdated = false;
            }
            return base.Save(entities, ref tran, isSupress);
        }
        public void GernateEquipmentOperateBasicLog(OperateType operateType, Equipment originalEquipment, Equipment curEquipment, Guid? operatorId, string operateIP, ref XTransaction tran)
        {
            if (originalEquipment != null) originalEquipment.IsSupressLazyLoad = true;
            if (curEquipment != null) curEquipment.IsSupressLazyLoad = true;
            SystemLog systemLog = new SystemLog() { Id = Guid.NewGuid(), BusinessId = curEquipment.Id, OperateEntityCnName = "设备基本信息", OperateEntityName = "Equipment", OperateTypeEnum = operateType, OperateIP = operateIP, OperateTime = DateTime.Now, OperatorId = operatorId };
            StringBuilder sbLogContent = new StringBuilder();
            if (operateType == OperateType.New || operateType == OperateType.Deleted)
            {
                sbLogContent.AppendFormat("设备名称:【{0}】", curEquipment.Name).Append("\r\n");
                sbLogContent.AppendFormat("资产编号:【{0}】", curEquipment.Label).Append("\r\n");
                sbLogContent.AppendFormat("型号:【{0}】", curEquipment.Model).Append("\r\n");
                if (curEquipment.Organization != null)
                {
                    sbLogContent.AppendFormat("所属单位:【{0}】", curEquipment.Organization.Name).Append("\r\n");
                }
                if (curEquipment.LabRoom != null)
                {
                    sbLogContent.AppendFormat("放置地点:【{0}】", curEquipment.LabRoom.Name).Append("\r\n");
                }
            }
            if (operateType == OperateType.Modified)
            {
                sbLogContent.AppendFormat("设备名称:【{0}】→【{1}】", originalEquipment.Name, curEquipment.Name).Append("\r\n");
                sbLogContent.AppendFormat("助记符:【{0}】→【{1}】", originalEquipment.InputStr, curEquipment.InputStr).Append("\r\n");
                sbLogContent.AppendFormat("资产编号:【{0}】→【{1}】", originalEquipment.Label, curEquipment.Label).Append("\r\n");
                sbLogContent.AppendFormat("型号:【{0}】→【{1}】", originalEquipment.Model, curEquipment.Model).Append("\r\n");
                sbLogContent.AppendFormat("规格:【{0}】→【{1}】", originalEquipment.Specifications, curEquipment.Specifications).Append("\r\n");
                sbLogContent.AppendFormat("产地:【{0}】→【{1}】", originalEquipment.ProductionArea, curEquipment.ProductionArea).Append("\r\n");
                sbLogContent.AppendFormat("厂家:【{0}】→【{1}】", originalEquipment.Factory, curEquipment.Factory).Append("\r\n");
                sbLogContent.AppendFormat("所属品牌:【{0}】→【{1}】", originalEquipment.Brand, curEquipment.Brand).Append("\r\n");
                sbLogContent.AppendFormat("出产编号:【{0}】→【{1}】", originalEquipment.ProductionNo, curEquipment.ProductionNo);
                sbLogContent.AppendFormat("出产日期:【{0}】→【{1}】", originalEquipment.ProductionDate.HasValue ? originalEquipment.ProductionDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curEquipment.ProductionDate.HasValue ? curEquipment.ProductionDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                sbLogContent.AppendFormat("设备来源:【{0}】→【{1}】", originalEquipment.ComeFrom, curEquipment.ComeFrom).Append("\r\n");
                sbLogContent.AppendFormat("供应商:【{0}】→【{1}】", originalEquipment.Supplier, curEquipment.Supplier).Append("\r\n");
                sbLogContent.AppendFormat("价格:【{0}】→【{1}", originalEquipment.UnitPrice.HasValue ? originalEquipment.UnitPrice.Value.ToString("0.00") : "", curEquipment.UnitPrice.HasValue ? curEquipment.UnitPrice.Value.ToString("0.00") : "").Append("\r\n");
                sbLogContent.AppendFormat("出产日期:【{0}】→【{1}】", originalEquipment.BuyDate.HasValue ? originalEquipment.BuyDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curEquipment.BuyDate.HasValue ? curEquipment.BuyDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                sbLogContent.AppendFormat("所属单位:【{0}】→【{1}】", originalEquipment.Organization != null ? originalEquipment.Organization.Name : "", curEquipment.Organization != null ? curEquipment.Organization.Name : "").Append("\r\n");
                sbLogContent.AppendFormat("放置地点:【{0}】→【{1}】", originalEquipment.LabRoom != null ? originalEquipment.LabRoom.Name : "", curEquipment.LabRoom != null ? curEquipment.LabRoom.Name : "").Append("\r\n");
                sbLogContent.AppendFormat("关联单位:【{0}】→【{1}】", originalEquipment.EquipmentRelationList != null && originalEquipment.EquipmentRelationList.Count > 0 ? string.Join(",", originalEquipment.EquipmentRelationList.Select(p => p.GetOrganization().Name)) : "", curEquipment.EquipmentRelationList != null && curEquipment.EquipmentRelationList.Count > 0 ? string.Join(",", curEquipment.EquipmentRelationList.Select(p => p.GetOrganization().Name)) : "").Append("\r\n");
                sbLogContent.AppendFormat("使用性质:【{0}】→【{1}】", originalEquipment.UseType.HasValue ? originalEquipment.UseTypeEnum.ToCnName() : "", curEquipment.UseType.HasValue ? curEquipment.UseTypeEnum.ToCnName() : "").Append("\r\n");
                sbLogContent.AppendFormat("是否停用:【{0}】→【{1}】", originalEquipment.IsStop.HasValue && originalEquipment.IsStop.Value ? "是" : "否", curEquipment.IsStop.HasValue && curEquipment.IsStop.Value ? "是" : "否").Append("\r\n");
                sbLogContent.AppendFormat("是否纳入管理系统:【{0}】→【{1}】", originalEquipment.IsOpen ? "是" : "否", curEquipment.IsOpen ? "是" : "否").Append("\r\n");
                sbLogContent.AppendFormat("是否大型设备:【{0}】→【{1}】", originalEquipment.IsLargeScaleEquipment.HasValue && originalEquipment.IsLargeScaleEquipment.Value ? "是" : "否", curEquipment.IsLargeScaleEquipment.HasValue && curEquipment.IsLargeScaleEquipment.Value ? "是" : "否").Append("\r\n");
                sbLogContent.AppendFormat("所属分类:【{0}】→【{1}】", originalEquipment.CategoryNames, curEquipment.CategoryNames).Append("\r\n");
                sbLogContent.AppendFormat("联系人:【{0}】→【{1}】", originalEquipment.LinkManNames, curEquipment.LinkManNames).Append("\r\n");
                sbLogContent.AppendFormat("联系电话:【{0}】→【{1}】", originalEquipment.LinkTelNo, curEquipment.LinkTelNo).Append("\r\n");
                sbLogContent.AppendFormat("联系邮箱:【{0}】→【{1}】", originalEquipment.AdminEmail, curEquipment.AdminEmail).Append("\r\n");
                sbLogContent.AppendFormat("主要规格及技术指标:【{0}】→【{1}】", originalEquipment.QualificationNoHTML, curEquipment.QualificationNoHTML).Append("\r\n");
                sbLogContent.AppendFormat("主要功能及特色:【{0}】→【{1}】", originalEquipment.ScopeOfApplicationNoHTML, curEquipment.ScopeOfApplicationNoHTML).Append("\r\n");
                sbLogContent.AppendFormat("样本检测注意事项:【{0}】→【{1}】", originalEquipment.SpecimenNoticeNoHTML, curEquipment.SpecimenNoticeNoHTML).Append("\r\n");
                sbLogContent.AppendFormat("设备使用相关说明:【{0}】→【{1}】", originalEquipment.UseNoticeNoHTML, curEquipment.UseNoticeNoHTML).Append("\r\n");
            }
            systemLog.LogContent = sbLogContent.ToString();
            systemLog.LogContentHTML = sbLogContent.ToString().Replace("\r\n", "<br />");
            __systemLogBLL.Add(new SystemLog[] { systemLog }, ref tran, true);

        }
        public void GenerateEquipmentLog(Equipment originalEquipment, Equipment curEquipment, Guid? operatorId,string URL ,string operateIP, ref XTransaction tran)
        {
            if (originalEquipment != null && curEquipment != null)
            {
                URL = URL.ToLower().IndexOf("http://") == -1 ? "http://" + URL : URL;
                if (
                        originalEquipment.Label != curEquipment.Label ||
                        originalEquipment.Name != curEquipment.Name ||
                        originalEquipment.Model != curEquipment.Model ||
                        originalEquipment.Specifications != curEquipment.Specifications ||
                        originalEquipment.Qualification != curEquipment.Qualification ||
                        originalEquipment.ProductionArea != curEquipment.ProductionArea ||
                        originalEquipment.ProductionNo != curEquipment.ProductionNo ||
                        originalEquipment.ScopeOfApplication != curEquipment.ScopeOfApplication ||
                        originalEquipment.RoomId != curEquipment.RoomId ||
                        originalEquipment.RelativePic != curEquipment.RelativePic 
                   )
                {
                    var equipmentLogList = __equipmentLogBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", curEquipment.Id));
                    if (equipmentLogList != null && equipmentLogList.Count > 0)
                    {
                        __equipmentLogBLL.Delete(equipmentLogList.Select(p => p.Id), ref tran, true);
                    }
                    var picURLs = "";
                    if (!string.IsNullOrWhiteSpace(curEquipment.RelativePic))
                    {
                        var picURLArray = curEquipment.RelativePic.Split(';').SkipWhile(p => string.IsNullOrWhiteSpace(p));
                        if (picURLArray != null && picURLArray.Count() > 0)
                        {
                            picURLs = string.Join(",", picURLArray.Select(p =>URL  + p));
                        }
                    }
                    var isSupressLazyLoad = curEquipment.IsSupressLazyLoad;
                    curEquipment.IsSupressLazyLoad = false;
                    var equipmentLog = new EquipmentLog()
                    {
                        Id = Guid.NewGuid(),
                        EquipmentId = curEquipment.Id,
                        Label = curEquipment.Label,
                        Model = curEquipment.Model,
                        Name = curEquipment.Name,
                        OperateTime = DateTime.Now,
                        OperatorId = operatorId,
                        ProductionArea = curEquipment.ProductionArea,
                        ProductionNo = curEquipment.ProductionNo,
                        Qualification = curEquipment.Qualification,
                        ScopeOfApplication = curEquipment.ScopeOfApplication,
                        Specifications = curEquipment.Specifications,
                        RoomId = curEquipment.LabRoom != null ? curEquipment.LabRoom.Code : "",
                        RoomName = curEquipment.LabRoomName,
                        PicURL = picURLs
                    };
                    curEquipment.IsSupressLazyLoad = isSupressLazyLoad;
                    __equipmentLogBLL.Add(new EquipmentLog[] { equipmentLog }, ref tran, true);
                }
            }
        }
        #region OriginalCode
        //public int GetUserEquipmentAppoitmentDaysBeforeBeginDate(User user, int appointmentTimeDays, int aHaedOfTime, IEnumerable<DayOfWeek> equipmentWorkDays, DateTime beginDate)
        //{
        //    var now = DateTime.Now;
        //    var curDate = now.Date;
        //    if (user != null && user.Tag != null && user.Tag.EnableAppointmentDays.HasValue && user.Tag.EnableAppointmentDays < appointmentTimeDays)
        //    {
        //        appointmentTimeDays = user.Tag.EnableAppointmentDays.Value;
        //    }
        //    var days = appointmentTimeDays;
        //    int aHaedOfDay = (int)(DateTime.Now.AddMinutes(aHaedOfTime).Date - curDate.Date).TotalDays;
        //    for (int j = 1; j <= aHaedOfDay; j++)
        //    {
        //        //if (equipmentWorkDays.Any(p => p == curDate.Date.AddDays(j - 1).DayOfWeek) || (!equipmentWorkDays.Any(p => p == curDate.Date.AddDays(j - 1).DayOfWeek) && equipmentWorkDays.Any(p => p == curDate.Date.AddDays(j).DayOfWeek) && days == 1))
        //        if (equipmentWorkDays.Any(p => p == curDate.Date.AddDays(j - 1).DayOfWeek))
        //        {
        //            appointmentTimeDays++;
        //        }
        //    }
        //    if (curDate < beginDate.AddDays(-1))
        //    {
        //        var diffDays = (int)(beginDate.AddDays(-1) - curDate).TotalDays;
        //        for (int i = 0; i <= diffDays; i++)
        //        {
        //            if (!equipmentWorkDays.Any(p => p == curDate.AddDays(i).DayOfWeek))
        //                appointmentTimeDays++;
        //        }
        //    }
        //    return appointmentTimeDays;
        //}
        //public EquipmentAppointmentTimes GetEquipmentAppoitmentTimes(Guid equipmentId,EquipmentTimeAppointmemtMode equipmentTimeAppointmemtMode, Guid? userId, int aHaedOfTime, int maxAdvanceTime, int aHeadOfCancelTime, ref int appointmentTimeDays, int appointmentTimeStep, string workDays, string workHours, DateTime beginDate, DateTime endDate)
        //{
        //    var days = 7;
        //    var now = DateTime.Now.Date;
        //    User user = null;
        //    if (userId.HasValue) user = __userBLL.GetEntityById(userId.Value);
        //    var equipmentWorkDays = WeekDayUtility.ConvertToWordDays(workDays);
        //    double beginHour = 0d, endHour = 0d;
        //    TimeUtility.GetBeginAndEndHourByTimeFormatStr(workHours, out beginHour, out endHour);
        //    IList<EquipmentAppointmentTime> lstEquipmentAppointmentTime = new List<EquipmentAppointmentTime>();
        //    IList<PublicHolidays> publiHolidayList = equipmentTimeAppointmemtMode == EquipmentTimeAppointmemtMode.Classic ? __publicHolidaysBLL.GetValidatePublicHolidaysList(false) : null;
        //    appointmentTimeDays = GetUserEquipmentAppoitmentDaysBeforeBeginDate(user, appointmentTimeDays, aHaedOfTime, equipmentWorkDays, beginDate);
        //    if (equipmentTimeAppointmemtMode == EquipmentTimeAppointmemtMode.Classic) days = appointmentTimeDays;
        //    for (int i = 1; i <= days; i++)
        //    {
        //        var date = beginDate.Date.AddDays(i - 1);
        //        var dayGaps = (date - now).TotalDays;
        //        if (!equipmentWorkDays.Any(p => p == date.DayOfWeek) && date >= now.Date)
        //        {
        //            appointmentTimeDays++;
        //            if (equipmentTimeAppointmemtMode == EquipmentTimeAppointmemtMode.Classic) days++;
        //        }
        //        else if (equipmentTimeAppointmemtMode == EquipmentTimeAppointmemtMode.Classic)
        //        {
        //            var equipmentAppointmentTime = new EquipmentAppointmentTime()
        //            {
        //                BeginTime = date,
        //                EndTime = date.AddDays(1).AddSeconds(-1),
        //                Status = EquipmentAppointmentTimeStatus.Valid
        //            };
        //            __publicHolidaysBLL.Validate(equipmentId, false, equipmentAppointmentTime, publiHolidayList);
        //            if (equipmentAppointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid)
        //            {
        //                appointmentTimeDays++;
        //                days++;
        //            }
        //        }
        //        var timeScopes = TimeUtility.GetTimeScopersByTimeFormatStr(workHours, appointmentTimeStep, date);
        //        foreach (var timeScope in timeScopes)
        //        {
        //            EquipmentAppointmentTime equipmentAppointmentTime = new EquipmentAppointmentTime();
        //            equipmentAppointmentTime.BeginTime = timeScope.BeginTime;
        //            equipmentAppointmentTime.EndTime = timeScope.EndTime;
        //            equipmentAppointmentTime.GenerateDefaultRemark();
        //            if (dayGaps >= appointmentTimeDays)
        //            {
        //                equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
        //                equipmentAppointmentTime.Remark = "该时间段还未到提前开放预约时间,暂时不可预约";
        //            }
        //            if (user == null)
        //            {
        //                equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
        //                equipmentAppointmentTime.Remark = "还没登录,暂时不可预约";
        //            }
        //            lstEquipmentAppointmentTime.Add(equipmentAppointmentTime);
        //        }
        //    }
        //    EquipmentAppointmentTimes equipmentAppointmentTimes = new EquipmentAppointmentTimes(aHaedOfTime, maxAdvanceTime,aHeadOfCancelTime, appointmentTimeStep, beginHour, endHour, lstEquipmentAppointmentTime);
        //    return equipmentAppointmentTimes;
        //}

        #endregion
    }
}