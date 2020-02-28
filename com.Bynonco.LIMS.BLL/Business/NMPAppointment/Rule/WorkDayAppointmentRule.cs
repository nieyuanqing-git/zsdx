using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPWorkDayAppointmentRule : NMPAppointmentRuleBase
    {
        private Guid __nmpId;
        private Guid __nmpEquipmentId;
        private IEnumerable<DayOfWeek> __workDays;
        private IWorkdaysValidateBLL __workdaysValidateBLL;
        public NMPWorkDayAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, Guid nmpId, Guid nmpEquipmentId, NMP nmp, NMPEquipment nmpEquipment, User user)
            : base(equipmentAppoitmentTimes, next, nmp,nmpEquipment, user)
        {
            __workdaysValidateBLL = new WorkdaysValidateBLL();
            __nmpId = nmpId;
            __nmpEquipmentId = nmpEquipmentId;
        }
        protected override void Prepare()
        {
            __workDays = WeekDayUtility.ConvertToWordDays(_nmp.WorkDays);
        }

        public override void Superimposing(EquipmentAppointmentTime appointmentTime)
        {
            string errorMessage = "";
            __workdaysValidateBLL.Validates(out errorMessage, __workDays, appointmentTime, null, null);
        }
    }
}
