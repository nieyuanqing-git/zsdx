using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class AppointmentManager:AppointmentManagerBase
    {
        public AppointmentManager(AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP)
            : base(appointmentMidiator, operatorId,operateIP, new AppointmentManagerFactory(appointmentMidiator.NewAppointments.First().EquipmentId.Value, appointmentMidiator.NewAppointments.First().UserId.Value, appointmentMidiator,operatorId,operateIP)) { }
    }
}
