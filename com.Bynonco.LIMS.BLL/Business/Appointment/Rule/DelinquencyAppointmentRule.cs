using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class DelinquencyAppointmentRule:AppointmentRuleBase
    {
        private IPunishActionBLL __punishActionBLL;
        private Guid __equipmentId;
        private Guid? __userId;
        private bool isPassValidate = false;
        private string errorMessage = "";
        public DelinquencyAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Guid? userId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            this.__userId = userId;
            __punishActionBLL = BLLFactory.CreatePunishActionBLL();
            __equipmentId = equipmentId;
        }
        protected override void Prepare()
        {
            isPassValidate = __punishActionBLL.Validates(__userId, out errorMessage);
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (!__userId.HasValue || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid ) return;
            if (!isPassValidate)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = errorMessage;
            }
        }
    }
}
