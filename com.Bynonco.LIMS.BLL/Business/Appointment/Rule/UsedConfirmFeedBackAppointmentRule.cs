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
    public class UsedConfirmFeedBackAppointmentRule:AppointmentRuleBase
    {
        private IUsedConfirmBLL __usedConfirmBLL;
        private IEquipmentBLL __equipmentBLL;
        private Guid __equipmentId;
        private Guid? __userId;
        private bool isPassValidate = false;
        private string errorMessage = "";
        public UsedConfirmFeedBackAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Guid? userId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            this.__userId = userId;
            __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __equipmentId = equipmentId;
        }
        protected override void Prepare()
        {
            isPassValidate = __usedConfirmBLL.Validates(__equipmentId, __userId, out errorMessage);
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (!__userId.HasValue || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid || !_equipment.IsEnableAppoitmentWithFeedBack.HasValue || !_equipment.IsEnableAppoitmentWithFeedBack.Value) return;
            if (!isPassValidate)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = errorMessage;
            }
        }
    }
}
