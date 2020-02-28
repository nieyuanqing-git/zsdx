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
    public class UnAppointmentPeriodTimeAppointmentRule:AppointmentRuleBase
    {
        private IUnAppointmentPeriodTimeBLL __unAppointmentPeriodTimeBLL;
        private Guid __equipmentId;
        private IList<UnAppointmentPeriodTime> __unAppointmentPeriodTimeList;
        public UnAppointmentPeriodTimeAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            __unAppointmentPeriodTimeBLL = BLLFactory.CreateUnAppointmentPeriodTimeBLL();
            __equipmentId = equipmentId;
            __unAppointmentPeriodTimeList = __unAppointmentPeriodTimeBLL.GetValidateUnAppointmentPeriodTimeListByEquipmentId(equipmentId,false);
        }
        protected override void Prepare() { }

        public override void Superimposing(EquipmentAppointmentTime appointmentTime)
        {
            string errorMessage = "";
            if (appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid || __unAppointmentPeriodTimeList == null || __unAppointmentPeriodTimeList.Count == 0) return;
            __unAppointmentPeriodTimeBLL.Validates(out errorMessage, appointmentTime, __unAppointmentPeriodTimeList);
        }
    }
}
