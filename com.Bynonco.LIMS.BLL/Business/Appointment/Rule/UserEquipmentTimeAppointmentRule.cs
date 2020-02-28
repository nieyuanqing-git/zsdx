using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.august.Core.XQuery;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class UserEquipmentTimeAppointmentRule : AppointmentRuleBase
    {
        private IUserEquipmentTimeBLL __userEquipmentTimeBLL;
        private IUserEquipmentTimeUserBLL __userEquipmentTimeUserBLL;
        private IList<UserEquipmentTime> __userEquipmentTimeList;
        private IList<UserEquipmentTimeUser> __userEquipmentTimeUserList;
        private Guid __equipmentId;
        private Guid? __userId;
        public UserEquipmentTimeAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Guid? userId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            this.__userId = userId;
            __userEquipmentTimeBLL = BLLFactory.CreateUserEquipmentTimeBLL();
            __userEquipmentTimeUserBLL = BLLFactory.CreateUserEquipmentTimeUserBLL();
            __equipmentId = equipmentId;
        }
        protected override void Prepare()
        {
            if (__userId.HasValue)
            {
                __userEquipmentTimeList = __userEquipmentTimeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", __equipmentId.ToString()));
                if (__userEquipmentTimeList != null && __userEquipmentTimeList.Count > 0)
                {
                    __userEquipmentTimeUserList = __userEquipmentTimeUserBLL.GetEntitiesByExpression(__userEquipmentTimeList.Select(p => p.Id).ToFormatStr("UserEquipmentTimeId"));
                }
            }
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (!__userId.HasValue || __userEquipmentTimeList == null || __userEquipmentTimeList.Count == 0 ||
                __userEquipmentTimeUserList == null || __userEquipmentTimeUserList .Count==0 || 
                appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid) return;
            
            foreach (var userEquipmentTime in __userEquipmentTimeList)
            {
                var userEquipmentTimeUserList = __userEquipmentTimeUserList.Where(string.Format("UserEquipmentTimeId=\"{0}\"*UserId=\"{1}\"", userEquipmentTime.Id.ToString(), __userId.Value), null);

                if (userEquipmentTime.IsEffectOnStudent && _user.TutorId.HasValue &&
                    (userEquipmentTimeUserList == null || userEquipmentTimeUserList.Count() == 0) &&
                    !__userEquipmentTimeUserList.Any(p => p.UserId == __userId.Value))
                {
                    userEquipmentTimeUserList = __userEquipmentTimeUserList.Where(string.Format("UserEquipmentTimeId=\"{0}\"*UserId=\"{1}\"", userEquipmentTime.Id.ToString(), _user.TutorId.Value), null);
                }
                if (userEquipmentTimeUserList == null || userEquipmentTimeUserList.Count() == 0) continue;
                var workdays = !string.IsNullOrWhiteSpace(userEquipmentTime.WorkDays) ? WeekDayUtility.ConvertToWordDays(userEquipmentTime.WorkDays) : null;
                if (workdays != null && workdays.Count() > 0)
                {
                    if (!workdays.Any(p => p == appointmentTime.BeginTime.DayOfWeek))
                    {
                        appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                        appointmentTime.Remark = userEquipmentTime.Remark;
                        continue;
                    }
                }
                var workTimes = !string.IsNullOrWhiteSpace(userEquipmentTime.WorkHours) ? 
                                        TimeUtility.GetTimeScopersByTimeFormatStr(userEquipmentTime.WorkHours,_equipmentAppoitmentTimes.AppointmentStep,appointmentTime.BeginTime.Date) : 
                                        null;
                if (workTimes != null && workTimes.Count() > 0)
                {
                    if (!workTimes.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
                    {
                        appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                        appointmentTime.Remark = userEquipmentTime.Remark;
                        continue;
                    }
                }
            }
        }
    }
}

