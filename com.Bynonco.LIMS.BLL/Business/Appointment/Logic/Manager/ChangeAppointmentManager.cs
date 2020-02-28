using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class ChangeAppointmentManager:AppointmentManagerBase
    {
        public ChangeAppointmentManager(AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP)
            : base(appointmentMidiator, operatorId, operateIP, new ChangeAppointmentManagerFactory(appointmentMidiator.ChangeNewAppointment.EquipmentId.Value, appointmentMidiator.ChangeNewAppointment.UserId.Value, appointmentMidiator, operatorId, operateIP)) { }
    }
}
