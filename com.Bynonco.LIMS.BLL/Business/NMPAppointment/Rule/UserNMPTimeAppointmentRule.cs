using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.august.Core.XQuery;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class UserNMPTimeAppointmentRule : NMPAppointmentRuleBase
    {
        private IUserNMPTimeBLL __userNMPTimeBLL;
        private IUserNMPTimeUserBLL __userNMPTimeUserBLL;
        private IList<UserNMPTime> __userNMPTimeList;
        private IList<UserNMPTimeUser> __userNMPTimeUserList;
        private Guid __nmpId;
        private Guid __nmpEquipmentId;
        private Guid? __userId;
        public UserNMPTimeAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, Guid nmpId, Guid nmpEquipmentId, Guid? userId, NMP nmp,NMPEquipment nmpEquipment, User user)
            : base(equipmentAppoitmentTimes, next, nmp,nmpEquipment, user)
        {
            this.__userId = userId;
            __userNMPTimeBLL = BLLFactory.CreateUserNMPTimeBLL();
            __userNMPTimeUserBLL = BLLFactory.CreateUserNMPTimeUserBLL();
            __nmpId = nmpId;
            __nmpEquipmentId = nmpEquipmentId;
        }
        protected override void Prepare()
        {
            if (__userId.HasValue)
            {
                __userNMPTimeList = __userNMPTimeBLL.GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", __nmpId.ToString()));
                if (__userNMPTimeList != null && __userNMPTimeList.Count > 0)
                {
                    __userNMPTimeUserList = __userNMPTimeUserBLL.GetEntitiesByExpression(__userNMPTimeList.Select(p => p.Id).ToFormatStr("UserNMPTimeId"));
                }
            }
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (!__userId.HasValue || __userNMPTimeList == null || __userNMPTimeList.Count == 0 ||
                __userNMPTimeUserList == null || __userNMPTimeUserList .Count==0 || 
                appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid) return;
            
            foreach (var userNMPTime in __userNMPTimeList)
            {
                var userEquipmentTimeUserList = __userNMPTimeUserList.Where(string.Format("UserNMPTimeId=\"{0}\"*UserId=\"{1}\"", userNMPTime.Id.ToString(), __userId.Value), null);

                if (userNMPTime.IsEffectOnStudent && _user.TutorId.HasValue &&
                    (userEquipmentTimeUserList == null || userEquipmentTimeUserList.Count() == 0) &&
                    !__userNMPTimeUserList.Any(p => p.UserId == __userId.Value))
                {
                    userEquipmentTimeUserList = __userNMPTimeUserList.Where(string.Format("UserNMPTimeId=\"{0}\"*UserId=\"{1}\"", userNMPTime.Id.ToString(), _user.TutorId.Value), null);
                }
                if (userEquipmentTimeUserList == null || userEquipmentTimeUserList.Count() == 0) continue;
                var workdays = !string.IsNullOrWhiteSpace(userNMPTime.WorkDays) ? WeekDayUtility.ConvertToWordDays(userNMPTime.WorkDays) : null;
                if (workdays != null && workdays.Count() > 0)
                {
                    if (!workdays.Any(p => p == appointmentTime.BeginTime.DayOfWeek))
                    {
                        appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                        appointmentTime.Remark = userNMPTime.Remark;
                        continue;
                    }
                }
                var workTimes = !string.IsNullOrWhiteSpace(userNMPTime.WorkHours) ? 
                                        TimeUtility.GetTimeScopersByTimeFormatStr(userNMPTime.WorkHours,_equipmentAppoitmentTimes.AppointmentStep,appointmentTime.BeginTime.Date) : 
                                        null;
                if (workTimes != null && workTimes.Count() > 0)
                {
                    if (!workTimes.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
                    {
                        appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                        appointmentTime.Remark = userNMPTime.Remark;
                        continue;
                    }
                }
            }
        }
    }
}

