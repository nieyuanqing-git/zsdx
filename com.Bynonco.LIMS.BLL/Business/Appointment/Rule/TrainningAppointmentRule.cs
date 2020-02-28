using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class TrainningAppointmentRule:AppointmentRuleBase
    {
        private IEquipmentTranningBLL __equipmentTranningBLL;
        private Guid __equipmentId;
        private Guid? __userId;
        private bool isPassValidate = false;
        private string errorMessage = "";
        public TrainningAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Guid? userId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            this.__userId = userId;
            __equipmentTranningBLL = BLLFactory.CreateEquipmentTranningBLL();
            __equipmentId = equipmentId;
        }
        protected override void Prepare()
        {
            isPassValidate = __equipmentTranningBLL.Validates(__equipmentId, __userId, out errorMessage);
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (!__userId.HasValue || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid || (!_equipment.IsNeedTranningBeforeAppointment.HasValue || !_equipment.IsNeedTranningBeforeAppointment.Value) && (!_equipment.IsNeedTranningBeforeUse.HasValue || !_equipment.IsNeedTranningBeforeUse.Value)) return;
            if (!isPassValidate)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = errorMessage;
            }
        }
    }
}
