using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class CancelAppointmentManagerFactory : AppointmentManagerFactoryBase
    {
        public CancelAppointmentManagerFactory(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator, Guid operatorId, string operateIP)
            : base(equipmentId, userId, appointmentMidiator, operatorId, operateIP) { }
        public override AppointmentValidateManagerBase CreateAppointmentValidate()
        {
            return new CancelAppointmentValidateManager(_equipmentId, _userId,_appointmentMidiator);
        }

        public override AppointmentHandlerBase CreateAppointmentHandler()
        {
            return new CancelAppointmentHandler(_appointmentMidiator, _operatorId, _operateIP);
        }
    }
}
