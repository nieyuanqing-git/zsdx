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
    public class  NMPAppointmentMoneyValidate :  NMPAppointmentValidateBase
    {
        private IMoneyValidateBLL __appointmentMoneyValidateBLL;
        public NMPAppointmentMoneyValidate(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments)
            : base(nmpEquipmentId, nmpId, userId, nmpAppointments) { }
        //private bool Validates(NMPAppointment nmpAppointment, IEnumerable<NMPAppointment> lstNMPAppointments, out string errorMessage)
        //{
        //    //return __appointmentMoneyValidateBLL.AppointmentMoneyValidate(nmpAppointment.UserId.Value, nmpAppointment.SubjectId.Value, nmpAppointment.PaymentMethodEnum, nmpAppointment, lstNMPAppointments,_equipment.AppointmentMinAccountBalance ,out errorMessage);
        //}
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            ////预约校验
            //if (_appointmentMidiator.NewAppointments != null && _appointmentMidiator.NewAppointments.Count() > 0)
            //    return Validates(_appointmentMidiator.NewAppointments.First(), _appointmentMidiator.NewAppointments, out errorMessage);
            return true;
        }
    }
}
