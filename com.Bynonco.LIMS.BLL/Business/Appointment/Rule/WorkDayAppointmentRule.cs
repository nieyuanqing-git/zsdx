using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class WorkDayAppointmentRule : AppointmentRuleBase
    {
        private IPublicHolidaysBLL __publicHolidaysBLL;
        private IHolidaysExcludeBLL __holidaysExcludeBLL;
        private IWorkdaysValidateBLL __workdaysValidateBLL = new WorkdaysValidateBLL();
        private IList<PublicHolidays> __pubHolidayList;
        private IList<HolidaysExclude> __excludeList;
        private Guid __equipmentId;
        private IEnumerable<DayOfWeek> __workDays;
        public WorkDayAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            __equipmentId = equipmentId;
            __publicHolidaysBLL = BLLFactory.CreatePublicHolidaysBLL();
            __holidaysExcludeBLL = BLLFactory.CreateHolidaysExcludeBLL();
            __pubHolidayList = __publicHolidaysBLL.GetEntitiesByExpression(string.Format("WorkDays=-null*EndAt>\"{0}\"", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            if (__pubHolidayList != null && __pubHolidayList.Count > 0)
            {
                __excludeList = __holidaysExcludeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", _equipment.Id) + "*" + __pubHolidayList.Select(p => p.Id).ToFormatStr("HolidaysId"));
            }
        }
        protected override void Prepare()
        {
            __workDays = WeekDayUtility.ConvertToWordDays(_equipment.WorkDays);
        }

        public override void Superimposing(EquipmentAppointmentTime appointmentTime)
        {
            string errorMessage = "";
            __workdaysValidateBLL.Validates(out errorMessage, __workDays, appointmentTime, __pubHolidayList, __excludeList);
        }
    }
}
