using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class OutOfTimeValidate: AppointmentValidateBase
    {
        public OutOfTimeValidate(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator)
            : base(equipmentId, userId, appointmentMidiator) { }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            //改约校验
            if (_appointmentMidiator.ChangeOldAppointment != null && _appointmentMidiator.ChangeNewAppointment != null)
                return Validates(_appointmentMidiator.ChangeOldAppointment, out errorMessage);
            //取消预约校验
            if (_appointmentMidiator.CanceledAppointment != null)
                return Validates(_appointmentMidiator.CanceledAppointment, out errorMessage);
            return true;
        }
        private bool Validates(Appointment appointment, out string errorMessage)
        {
            errorMessage = "";
            var equipment = _equipmentBLL.GetEntityById(appointment.EquipmentId.Value);
            if (!equipment.IsEnableCancelNotOverAppointment && appointment.EndTime.Value > DateTime.Now)
            {
                if (appointment.BeginTime.Value < DateTime.Now.AddMinutes(!equipment.MinAppointmentCancelTime.HasValue ? 0 : equipment.MinAppointmentCancelTime.Value))
                {
                    errorMessage = "过了提前取消的时间,不能取消预约";
                    return false;
                }
            }
            return true;
        }
    }
}
