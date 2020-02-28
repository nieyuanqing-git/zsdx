using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class WorkdaysValidateBLL : IWorkdaysValidateBLL
    {
        public bool Validates(out string errorMessage, IEnumerable<DayOfWeek> workDays, EquipmentAppointmentTime equipmentAppointmentTime, IList<PublicHolidays> pubHolidayList, IList<HolidaysExclude> excludeList)
        {
            errorMessage = "";
            if (workDays == null || workDays.Count() == 0 || !workDays.Any(p => p == equipmentAppointmentTime.BeginTime.DayOfWeek))
            {
                equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                equipmentAppointmentTime.Remark = "该工作日尚未开放预约";
            }
            if (equipmentAppointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid)
            {
                if (pubHolidayList != null && pubHolidayList.Count > 0)
                {
                    foreach (var pubHoliday in pubHolidayList)
                    {
                        if (string.IsNullOrWhiteSpace(pubHoliday.WorkDays)) continue;
                        var exclude = excludeList != null && excludeList.Count > 0 ? excludeList.FirstOrDefault(p => p.HolidaysId == pubHoliday.Id) : null;
                        if (exclude != null) continue;
                        if (pubHoliday.ExchangeWorkDays != null && pubHoliday.ExchangeWorkDays.Count > 0)
                        {
                            foreach (var exchangeWorkDay in pubHoliday.ExchangeWorkDays)
                            {
                                if (DateTimeUtility.IsContain(equipmentAppointmentTime.BeginTime, equipmentAppointmentTime.EndTime, exchangeWorkDay.BeginTime, exchangeWorkDay.EndTime))
                                {
                                    equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Valid;
                                    equipmentAppointmentTime.GenerateDefaultRemark();
                                }
                            }
                        }
                    }
                }
            }
            return equipmentAppointmentTime.Status == EquipmentAppointmentTimeStatus.Valid || equipmentAppointmentTime.Status== EquipmentAppointmentTimeStatus.SelfAppointmented;
        }
    }
}
