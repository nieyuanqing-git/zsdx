using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business.AppointmentRule;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class WhichWeekDayStopBookOfNextWeekAppointmentRule: AppointmentRuleBase
    {
        private DateTime __openForBookDate;
        public WhichWeekDayStopBookOfNextWeekAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user) { }

        protected override void Prepare() 
        {
            if (_equipment.WhichWeekDayStopBookOfNextWeek.HasValue)
            {
                var dateScope = DateTimeUtility.GetWeekFirstAndLastDate(DateTime.Now);
                __openForBookDate = dateScope.BeginTime.Date.AddDays(_equipment.WhichWeekDayStopBookOfNextWeek.Value).AddDays(7).AddDays(1);
            }

        }

        public override void Superimposing(EquipmentAppointmentTime appointmentTime)
        {
            if (appointmentTime.Status != EquipmentAppointmentTimeStatus.Valid || !_equipment.WhichWeekDayStopBookOfNextWeek.HasValue) return;
            if (appointmentTime.BeginTime >= __openForBookDate)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = "该时间段尚未开放";
            }
        }
    }
}
