using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class CancelAppointmentManager:AppointmentManagerBase
    {
        public CancelAppointmentManager(AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP)
            : base(appointmentMidiator, operatorId, operateIP,new CancelAppointmentManagerFactory(appointmentMidiator.CanceledAppointment.EquipmentId.Value, appointmentMidiator.CanceledAppointment.UserId.Value, appointmentMidiator, operatorId,operateIP)) { }

    }
}
