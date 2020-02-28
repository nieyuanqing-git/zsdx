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
    public class EquipmentNoticeAppointmentRule:AppointmentRuleBase
    {
        private IEquipmentNoticeBLL __equipmentNoticeBLL;
        private Guid __equipmentId;
        private Guid? __userId;
        private bool validateEquipmentNotice;
        string errorMessage = "";
        public EquipmentNoticeAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Guid? userId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            this.__userId = userId;
            __equipmentNoticeBLL = BLLFactory.CreateEquipmentNoticeBLL();
            __equipmentId = equipmentId;
            validateEquipmentNotice = __equipmentNoticeBLL.Validates(__equipmentId, __userId, out errorMessage);
        }
        protected override void Prepare() { }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (!__userId.HasValue || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid ) return;
            
            var result = validateEquipmentNotice;
            if (!result)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = errorMessage;
            }
        }
    }
}
