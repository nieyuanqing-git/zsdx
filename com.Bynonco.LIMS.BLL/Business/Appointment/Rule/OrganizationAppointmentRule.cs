using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class OrganizationAppointmentRule : AppointmentRuleBase
    {
        private Guid? __userId;
        public OrganizationAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid? userId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            __userId = userId;
        }
        protected override void Prepare() { }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (appointmentTime.Status != EquipmentAppointmentTimeStatus.Valid) return;
            if (_user == null || _user.Organization == null) return;
            if (_user.Organization.IsUnAppointment.HasValue && _user.Organization.IsUnAppointment.Value)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = _user.Organization.WhyUnAppointment;
            }
        }
    }
}
