using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class AppointmentMoneyValidate : AppointmentValidateBase
    {
        private IMoneyValidateBLL __appointmentMoneyValidateBLL;
        public AppointmentMoneyValidate(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator)
            : base(equipmentId, userId, appointmentMidiator)
        {
            __appointmentMoneyValidateBLL = BLLFactory.CreateMoneyValidateBLL();
        }
        private bool Validates(Appointment appointment, IEnumerable<Appointment> lstAppointments, out string errorMessage)
        {
            return __appointmentMoneyValidateBLL.AppointmentMoneyValidate(appointment.UserId.Value, appointment.SubjectId.Value, appointment.PaymentMethodEnum, appointment, lstAppointments,_equipment.AppointmentMinAccountBalance ,out errorMessage);
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            //预约校验
            if (_appointmentMidiator.NewAppointments != null && _appointmentMidiator.NewAppointments.Count() > 0)
                return Validates(_appointmentMidiator.NewAppointments.First(), _appointmentMidiator.NewAppointments, out errorMessage);
            //改约校验
            if (_appointmentMidiator.ChangeOldAppointment != null && _appointmentMidiator.ChangeNewAppointment != null)
                return Validates(_appointmentMidiator.ChangeOldAppointment, new Appointment[] { _appointmentMidiator.ChangeNewAppointment }, out errorMessage);
            return true;
        }
    }
}
