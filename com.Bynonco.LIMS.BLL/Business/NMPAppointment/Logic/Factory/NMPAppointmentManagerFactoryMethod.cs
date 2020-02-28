using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPAppointmentManagerFactoryMethod
    {
        public static NMPAppointmentManagerBase CreateNMPAppointmentManager(Guid nmpId,AppointmentBusinessType appointmentBusinessType, IEnumerable<NMPAppointment> nmpAppointments, Guid operatorId, string operateIP)
        {
            switch (appointmentBusinessType)
            {
                case AppointmentBusinessType.Appointment:
                    return new NMPAppointmentManager(nmpId, nmpAppointments, operatorId, operateIP);
                case AppointmentBusinessType.CancelAppointment:
                    return new NMPCancelAppointmentManager(nmpId, nmpAppointments != null && nmpAppointments.Count() > 0 ? nmpAppointments.First() : null, operatorId, operateIP);
            }
            return null;
        }
    }
}
