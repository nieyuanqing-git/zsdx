using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class HolidayAppointmentRule : AppointmentRuleBase
    {
        private IPublicHolidaysBLL __publicHolidayBLL;
        private IList<PublicHolidays> __publicHolidaysList;
        private Guid __equipmentId;
        public HolidayAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            __equipmentId = equipmentId;
            __publicHolidayBLL = BLLFactory.CreatePublicHolidaysBLL();
            __publicHolidaysList = __publicHolidayBLL.GetValidatePublicHolidaysList(false);
        }
        protected override void Prepare() { }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            __publicHolidayBLL.Validate(__equipmentId, false, appointmentTime, __publicHolidaysList);
        }
    }
}
