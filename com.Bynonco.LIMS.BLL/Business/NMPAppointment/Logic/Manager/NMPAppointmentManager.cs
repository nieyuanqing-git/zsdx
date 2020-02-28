using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPAppointmentManager:NMPAppointmentManagerBase
    {
        public NMPAppointmentManager(Guid nmpId, IEnumerable<NMPAppointment> nmpAppointments, Guid operatorId, string operateIP)
            : base(nmpId, nmpAppointments, operatorId, operateIP, new NMPAppointmentManagerFactory(nmpAppointments.First().NMPEquipmentId.Value,nmpId, nmpAppointments.First().UserId.Value, nmpAppointments, operatorId, operateIP)) { }
    }
}
