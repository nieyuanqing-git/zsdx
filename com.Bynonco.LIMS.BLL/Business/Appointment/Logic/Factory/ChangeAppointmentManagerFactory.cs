using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class ChangeAppointmentManagerFactory: AppointmentManagerFactoryBase
    {
        public ChangeAppointmentManagerFactory(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP)
            : base(equipmentId, userId, appointmentMidiator, operatorId,operateIP) { }
        public override AppointmentValidateManagerBase CreateAppointmentValidate()
        {
            return new ChangeAppointmentValidateManager(_equipmentId, _userId, _appointmentMidiator);
        }

        public override AppointmentHandlerBase CreateAppointmentHandler()
        {
            return new ChangeAppointmentHandler(_appointmentMidiator, _operatorId,_operateIP);
        }
    }
}
