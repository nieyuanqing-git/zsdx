using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPPublicHolidaysBLL : INMPPublicHolidaysBLL
    {
        public IList<IPublicHolidays> GetValidatePublicHolidaysListForAppointment(bool isEffectOnSample) { return null; }
        public void ValidateForAppointment(Guid nmpId, bool isEffectOnSample, EquipmentAppointmentTime appointmentTime, IList<IPublicHolidays> publicHolidaysList) { }
    }
}
