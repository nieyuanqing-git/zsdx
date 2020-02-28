using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Sample
{
    public class SampleSendTimeWorkDaysValidate:SampleSendTimeValidateBase
    {
        private IPublicHolidaysBLL __publicHolidaysBLL;
        private IHolidaysExcludeBLL __holidaysExcludeBLL;
        private IList<PublicHolidays> __pubHolidayList;
        private IList<HolidaysExclude> __excludeList;
        private IWorkdaysValidateBLL __workdaysValidateBLL = new WorkdaysValidateBLL();
        private IEnumerable<DayOfWeek> __workDays;
        public SampleSendTimeWorkDaysValidate(Equipment equipment)
            : base(equipment) 
        {
            __publicHolidaysBLL = BLLFactory.CreatePublicHolidaysBLL();
            __holidaysExcludeBLL = BLLFactory.CreateHolidaysExcludeBLL();
            __workDays = WeekDayUtility.ConvertToWordDays(equipment.SampleEnableSendWeekDays);
            __pubHolidayList = __publicHolidaysBLL.GetEntitiesByExpression(string.Format("IsEffectOnSample=true*WorkDays=-null*EndAt>\"{0}\"", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            if (__pubHolidayList != null && __pubHolidayList.Count > 0)
            {
                __excludeList = __holidaysExcludeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", _equipment.Id) + "*" + __pubHolidayList.Select(p => p.Id).ToFormatStr("HolidaysId"));
            }
        }

        protected override bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime)
        {
            return __workdaysValidateBLL.Validates(out errorMessage, __workDays, equipmentAppointmentTime, __pubHolidayList, __excludeList);
        }
    }
}
