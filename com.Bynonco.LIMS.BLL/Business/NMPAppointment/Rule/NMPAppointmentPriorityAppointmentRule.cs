using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPAppointmentPriorityAppointmentRule : NMPAppointmentRuleBase
    {
        private INMPAppointmentPriorityBLL __appointmentPriorityBLL;
        private IList<NMPAppointmentPriority> __appointmentPriorityList;
        private Guid __nmpId;
        private Guid? __userId;
        private Guid __nmpEquipmentId;
        public NMPAppointmentPriorityAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, Guid nmpEquipmentId, Guid nmpId, Guid? userId, NMP nmp, NMPEquipment nmpEquipment, User user)
            : base(equipmentAppoitmentTimes, next, nmp, nmpEquipment, user)
        {
            this.__userId = userId;
            __nmpId = nmpId;
            __nmpEquipmentId = nmpEquipmentId;
            __appointmentPriorityBLL = BLLFactory.CreateNMPAppointmentPriorityBLL();
        }
        protected override void Prepare()
        {
            __appointmentPriorityList = __appointmentPriorityBLL.GetValidateNMPAppointmentPriorityListByNMPId(__nmpId);
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (__appointmentPriorityList == null || __appointmentPriorityList.Count == 0 || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid) return;
            foreach (var appointmentPriority in __appointmentPriorityList)
            {
                if (appointmentPriority.Users != null && appointmentPriority.Users.Count > 0 && appointmentPriority.Users.Any(p => p.UserId == __userId)) continue;
                if (!appointmentPriority.StartAt.HasValue) appointmentPriority.StartAt = DateTime.MinValue;
                if (!appointmentPriority.EndAt.HasValue) appointmentPriority.EndAt = DateTime.MaxValue;
                if ((appointmentTime.BeginTime - DateTime.Now).TotalDays >= appointmentPriority.PriorityDays)
                {
                    var appointmentPriorityUserList = appointmentPriority.Users == null || !__userId.HasValue ? null : appointmentPriority.Users.Where(p => p.UserId == __userId.Value);
                    if ((appointmentTime.BeginTime - appointmentPriority.StartAt.Value).TotalMilliseconds >= 0 &&
                        (appointmentPriorityUserList == null || appointmentPriorityUserList.Count() == 0) && appointmentPriority.Users != null)
                    {
                        if (__userId.HasValue)
                        {
                            var appointmentPriorityListFind = __appointmentPriorityList.Where(p => (!p.EndAt.HasValue || p.EndAt.Value >= appointmentTime.BeginTime) && p.Id != appointmentPriority.Id);
                            if (appointmentPriorityListFind != null && appointmentPriorityListFind.Count() > 0)
                            {
                                foreach (var appointmentPriorityFind in appointmentPriorityListFind)
                                {
                                    if (appointmentPriorityFind.Users != null && appointmentPriorityFind.Users.Count > 0 && appointmentPriorityFind.Users.Any(p => p.UserId == __userId))
                                    {
                                        if (WeekDayUtility.ConvertToWordDays(appointmentPriorityFind.WorkDays).Any(p => p == appointmentTime.BeginTime.DayOfWeek))
                                        {
                                            var priorityimeScopers = TimeUtility.GetTimeScopersByTimeFormatStr(appointmentPriorityFind.PriorityHours, _nmp.AppointmentTimeStep, appointmentTime.BeginTime.Date);
                                            if (priorityimeScopers.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
                                            {
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(appointmentPriority.WorkDays))
                        {
                            if (!WeekDayUtility.ConvertToWordDays(appointmentPriority.WorkDays).Any(p => p == appointmentTime.BeginTime.DayOfWeek)) continue;
                        }
                        if (!string.IsNullOrWhiteSpace(appointmentPriority.PriorityHours))
                        {
                            var priorityimeScopers = TimeUtility.GetTimeScopersByTimeFormatStr(appointmentPriority.PriorityHours, _nmp.AppointmentTimeStep, appointmentTime.BeginTime.Date);
                            if (priorityimeScopers.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
                            {
                                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                                appointmentTime.Remark = appointmentPriority.Remark;
                            }
                        }
                    }
                }
            }
        }
    }
}
