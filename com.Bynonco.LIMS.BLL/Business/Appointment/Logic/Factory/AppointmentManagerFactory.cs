using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class AppointmentManagerFactory : AppointmentManagerFactoryBase
    {
        public AppointmentManagerFactory(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator, Guid operatorId, string operateIP)
            : base(equipmentId, userId, appointmentMidiator, operatorId, operateIP) { }
        public override AppointmentValidateManagerBase CreateAppointmentValidate()
        {
            return new AppointmentValidateManager(_equipmentId, _userId, _appointmentMidiator);
        }

        public override AppointmentHandlerBase CreateAppointmentHandler()
        {
            return new AppointmentHandler(_appointmentMidiator, _operatorId, _operateIP);
        }
    }
}
