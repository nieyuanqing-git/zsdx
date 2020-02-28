using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPOutOfTimeAppointmentRule : NMPAppointmentRuleBase
    {
        public NMPOutOfTimeAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, NMP nmp, NMPEquipment nmpEquipment, User user)
            : base(equipmentAppoitmentTimes, next, nmp, nmpEquipment, user) { }
        protected override void Prepare() { }

        public override void Superimposing(EquipmentAppointmentTime appointmentTime)
        {
            if (appointmentTime.Status != EquipmentAppointmentTimeStatus.Valid) return;
            if ((appointmentTime.BeginTime - DateTime.Now.AddMinutes(_equipmentAppoitmentTimes.MaxAdvanceTime)).TotalMilliseconds <= 0)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = string.Format("该时间段已经超过提前预约时间,须提前【{0}】分钟预约", _equipmentAppoitmentTimes.MaxAdvanceTime);
            }
        }
    }
}
