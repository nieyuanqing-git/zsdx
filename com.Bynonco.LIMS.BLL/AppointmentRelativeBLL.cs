using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public delegate IList<IPublicHolidays> FunGetValidatePublicHolidaysList(bool isEffectOnSample);
    public delegate void FunDoPublicHolidaysValidate(Guid businessId, bool isEffectOnSample,EquipmentAppointmentTime equipmentAppointmentTime, IList<IPublicHolidays> publiHolidayList);
    public class AppointmentRelativeBLL
    {
        private IUserBLL __userBLL;
        private static IDictCodeTypeBLL __dictCodeTypeBLL;
        private FunGetValidatePublicHolidaysList __funGetValidatePublicHolidaysList;
        private FunDoPublicHolidaysValidate __funDoPublicHolidaysValidate;
        private static DictCodeType __appointmentInputSettingDictCodeType;
        static AppointmentRelativeBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
        }
        public AppointmentRelativeBLL(FunGetValidatePublicHolidaysList funGetValidatePublicHolidaysList, FunDoPublicHolidaysValidate funDoPublicHolidaysValidate)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __funGetValidatePublicHolidaysList = funGetValidatePublicHolidaysList;
            __funDoPublicHolidaysValidate = funDoPublicHolidaysValidate;
        }
        public int GetUserAppoitmentDaysBeforeBeginDate(User user, int appointmentTimeDays, int aHaedOfTime, IEnumerable<DayOfWeek> workDays, DateTime beginDate)
        {
            var now = DateTime.Now;
            var curDate = now.Date;
            if (user != null && user.Tag != null && user.Tag.EnableAppointmentDays.HasValue && user.Tag.EnableAppointmentDays < appointmentTimeDays)
            {
                appointmentTimeDays = user.Tag.EnableAppointmentDays.Value;
            }
            var days = appointmentTimeDays;
            int aHaedOfDay = (int)(DateTime.Now.AddMinutes(aHaedOfTime).Date - curDate.Date).TotalDays;
            for (int j = 1; j <= aHaedOfDay; j++)
            {
                if (workDays.Any(p => p == curDate.Date.AddDays(j - 1).DayOfWeek)) appointmentTimeDays++;
            }
            if (curDate < beginDate.AddDays(-1))
            {
                var diffDays = (int)(beginDate.AddDays(-1) - curDate).TotalDays;
                for (int i = 0; i <= diffDays; i++)
                {
                    if (!workDays.Any(p => p == curDate.AddDays(i).DayOfWeek)) appointmentTimeDays++;  
                }
            }
            return appointmentTimeDays;
        }
        public EquipmentAppointmentTimes GetAppoitmentTimes(Guid businessId, EquipmentTimeAppointmemtMode equipmentTimeAppointmemtMode, Guid? userId, int aHaedOfTime, int maxAdvanceTime, int aHeadOfCancelTime, ref int appointmentTimeDays, int appointmentTimeStep, string workDays, string workHours, DateTime beginDate, DateTime endDate)
        {
            var days = 7;
            var now = DateTime.Now.Date;
            User user = null;
            if (userId.HasValue) user = __userBLL.GetEntityById(userId.Value);
            var equipmentWorkDays = WeekDayUtility.ConvertToWordDays(workDays);
            double beginHour = 0d, endHour = 0d;
            TimeUtility.GetBeginAndEndHourByTimeFormatStr(workHours, out beginHour, out endHour);
            IList<EquipmentAppointmentTime> lstEquipmentAppointmentTime = new List<EquipmentAppointmentTime>();
            IList<IPublicHolidays> publiHolidayList = equipmentTimeAppointmemtMode == EquipmentTimeAppointmemtMode.Classic ? __funGetValidatePublicHolidaysList(false) : null;
            appointmentTimeDays = GetUserAppoitmentDaysBeforeBeginDate(user, appointmentTimeDays, aHaedOfTime, equipmentWorkDays, beginDate);
            if (equipmentTimeAppointmemtMode == EquipmentTimeAppointmemtMode.Classic) days = appointmentTimeDays;
            for (int i = 1; i <= days; i++)
            {
                var date = beginDate.Date.AddDays(i - 1);
                var dayGaps = (date - now).TotalDays;
                if (!equipmentWorkDays.Any(p => p == date.DayOfWeek) && date >= now.Date)
                {
                    appointmentTimeDays++;
                    if (equipmentTimeAppointmemtMode == EquipmentTimeAppointmemtMode.Classic) days++;
                }
                else if (equipmentTimeAppointmemtMode == EquipmentTimeAppointmemtMode.Classic)
                {
                    var equipmentAppointmentTime = new EquipmentAppointmentTime()
                    {
                        BeginTime = date,
                        EndTime = date.AddDays(1).AddSeconds(-1),
                        Status = EquipmentAppointmentTimeStatus.Valid
                    };
                    __funDoPublicHolidaysValidate(businessId, false, equipmentAppointmentTime, publiHolidayList);
                    if (equipmentAppointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid)
                    {
                        appointmentTimeDays++;
                        days++;
                    }
                }
                var timeScopes = TimeUtility.GetTimeScopersByTimeFormatStr(workHours, appointmentTimeStep, date);
                foreach (var timeScope in timeScopes)
                {
                    EquipmentAppointmentTime equipmentAppointmentTime = new EquipmentAppointmentTime();
                    equipmentAppointmentTime.BeginTime = timeScope.BeginTime;
                    equipmentAppointmentTime.EndTime = timeScope.EndTime;
                    equipmentAppointmentTime.GenerateDefaultRemark();
                    if (dayGaps >= appointmentTimeDays)
                    {
                        equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                        equipmentAppointmentTime.Remark = "该时间段还未到提前开放预约时间,暂时不可预约";
                    }
                    //if (user == null)
                    //{
                    //    equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                    //    equipmentAppointmentTime.Remark = "还没登录,暂时不可预约";
                    //}
                    lstEquipmentAppointmentTime.Add(equipmentAppointmentTime);
                }
            }
            EquipmentAppointmentTimes equipmentAppointmentTimes = new EquipmentAppointmentTimes(aHaedOfTime, maxAdvanceTime, aHeadOfCancelTime, appointmentTimeStep, beginHour, endHour, lstEquipmentAppointmentTime);
            return equipmentAppointmentTimes;
        }
        public static AppointmentInputSetting GetAppointmentInputSetting()
        {
            bool isUseNatureRequired = true, isExperimentContentRequired = true;
            UseNature? useNatureDefaultValue = null;
            var dictCodeType = __appointmentInputSettingDictCodeType == null ? __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=Appointment") : __appointmentInputSettingDictCodeType;
            if (dictCodeType != null && dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
            {
                __appointmentInputSettingDictCodeType = dictCodeType;
                var isUseNatureRequiredDictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "IsUseNatureRequired");
                if (isUseNatureRequiredDictCode != null && !string.IsNullOrWhiteSpace(isUseNatureRequiredDictCode.Value))
                {
                    isUseNatureRequired = isUseNatureRequiredDictCode.Value.Trim() == "1";
                }

                var isExperimentContentRequiredDictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "IsExperimentContentRequired");
                if (isExperimentContentRequiredDictCode != null && !string.IsNullOrWhiteSpace(isExperimentContentRequiredDictCode.Value))
                {
                    isExperimentContentRequired = isExperimentContentRequiredDictCode.Value.Trim() == "1";
                }

                var useNatureDefaultValueDictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "UseNatureDefaultValue");
                if (useNatureDefaultValueDictCode != null && !string.IsNullOrWhiteSpace(useNatureDefaultValueDictCode.Value))
                {
                    useNatureDefaultValue = (UseNature)int.Parse(useNatureDefaultValueDictCode.Value.Trim());
                }
            }
            return new AppointmentInputSetting(isUseNatureRequired, isExperimentContentRequired, useNatureDefaultValue); ;
        }
        public string GetTutorUnAuditMessage()
        {
            return "需要导师审核通过才能使用该设备";
        }
        public string GetUnTrainningMessage()
        {
            return "需要完成培训才能使用该设备";
        }
        public string GetUnAuditMessage()
        {
            return "需要审核通过才能使用该设备";
        }
        public static string GenerateAppointmentTotalTypeQueryExpression(EquipmentAppointmentTotalType equipmentAppointmentTotalType)
        {
            string quresyExpression = "";
            switch (equipmentAppointmentTotalType)
            {
                case EquipmentAppointmentTotalType.ChangeTotal:
                    quresyExpression = string.Format("Status={0}", ((int)AppointmentStatus.Changed)).ToString();
                    break;
                case EquipmentAppointmentTotalType.DoingTotal:
                    quresyExpression = string.Format("Status={0}*BeginTime>\"{1}\"",
                                                                        ((int)AppointmentStatus.Waitting),
                                                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).ToString();
                    break;
                case EquipmentAppointmentTotalType.DoneTotal:
                    quresyExpression = string.Format("BeginTime<\"{0}\"*Status=-{1}*Status=-{2}*Status=-{3}",
                                                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                                            ((int)AppointmentStatus.Fail).ToString(),
                                                                            ((int)AppointmentStatus.Changed).ToString(),
                                                                            ((int)AppointmentStatus.Cancel).ToString());
                    break;
                case EquipmentAppointmentTotalType.MissTotal:
                    quresyExpression = string.Format("Status={0}", ((int)AppointmentStatus.Fail).ToString());
                    break;
                case EquipmentAppointmentTotalType.TodayTotal:
                    quresyExpression = string.Format("BeginTime>\"{0}\"*BeginTime<\"{1}\"*Status=-{2}*Status=-{3}*Status=-{4}",
                                                                          DateTime.Now.Date.ToString("yyyy-MM-dd  HH:mm:ss"),
                                                                          DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd  HH:mm:ss"),
                                                                          ((int)AppointmentStatus.Fail).ToString(),
                                                                          ((int)AppointmentStatus.Changed).ToString(),
                                                                          ((int)AppointmentStatus.Cancel).ToString());
                    break;
                case EquipmentAppointmentTotalType.CancelTotal:
                    quresyExpression = string.Format("Status={0}", ((int)AppointmentStatus.Cancel).ToString());
                    break;
                case EquipmentAppointmentTotalType.WaitingAudit:
                    quresyExpression = string.Format("Status={0}*IsNeedAudit=true*AuditingStatus={1}*BeginTime>\"{2}\"",
                        ((int)AppointmentStatus.Waitting).ToString(),
                        ((int)AppointmentAuditStatus.UnAudit).ToString(),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                case EquipmentAppointmentTotalType.Audited:
                    quresyExpression = string.Format("IsNeedAudit=true*AuditingStatus=-{0}", ((int)AppointmentAuditStatus.UnAudit).ToString());
                    break;
                case EquipmentAppointmentTotalType.StatisticsTotal:
                    quresyExpression = string.Format("(Status=-{0}*Status=-{1})", ((int)AppointmentStatus.Changed), ((int)AppointmentStatus.Cancel)).ToString();
                    break;
            }
            return quresyExpression;
        }
    }
}
