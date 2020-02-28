using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPDelinquencyAppointmentRule : NMPAppointmentRuleBase
    {
        private IPunishActionBLL __punishActionBLL;
        private Guid __nmpId;
        private Guid __nmpEquipmentId;
        private Guid? __userId;
        private bool isPassValidate = false;
        private string errorMessage = "";
        public NMPDelinquencyAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, Guid nmpId, Guid nmpEquipmentId, Guid? userId, NMP nmp, NMPEquipment nmpEquipment, User user)
            : base(equipmentAppoitmentTimes, next, nmp, nmpEquipment, user)
        {
            this.__userId = userId;
            __punishActionBLL = BLLFactory.CreatePunishActionBLL();
            __nmpId = nmpId;
            __nmpEquipmentId = nmpEquipmentId;
        }
        protected override void Prepare()
        {
            isPassValidate = __punishActionBLL.Validates(__userId, out errorMessage);
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (!__userId.HasValue || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid) return;
            if (!isPassValidate)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = errorMessage;
            }
        }
    }
}
