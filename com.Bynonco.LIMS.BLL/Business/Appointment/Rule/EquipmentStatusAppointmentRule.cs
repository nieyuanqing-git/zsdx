using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class EquipmentStatusAppointmentRule:AppointmentRuleBase
    {
        private Guid __equipmentId;
        public EquipmentStatusAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            __equipmentId = equipmentId;
        }
        protected override void Prepare() { }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (_equipment.StatusEnum != Model.Enum.EquipmentStatus.Normal || _equipment.UseTypeEnum != Model.Enum.EquipmentUseType.NeedBook)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = _equipment.StatusDescription;
            }
        }
    }
}
