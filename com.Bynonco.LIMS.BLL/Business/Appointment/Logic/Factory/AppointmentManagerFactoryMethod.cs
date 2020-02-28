using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class AppointmentManagerFactoryMethod
    {
        public static AppointmentManagerBase CreateAppointmentManager(AppointmentMidiator appointmentMidiator, AppointmentBusinessType appointmentBusinessType, Guid operatorId,string operateIP)
        {
            switch (appointmentBusinessType)
            {
                case AppointmentBusinessType.Appointment:
                    return new AppointmentManager(appointmentMidiator, operatorId,operateIP);
                case AppointmentBusinessType.CancelAppointment:
                    return new CancelAppointmentManager(appointmentMidiator, operatorId,operateIP);
                case AppointmentBusinessType.ChangeAppointment:
                    return new ChangeAppointmentManager(appointmentMidiator, operatorId,operateIP);
            }
            return null;
        }
    }
}
