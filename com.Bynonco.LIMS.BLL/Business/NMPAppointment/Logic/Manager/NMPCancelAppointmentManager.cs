using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPCancelAppointmentManager:NMPAppointmentManagerBase
    {
        public NMPCancelAppointmentManager(Guid nmpId, NMPAppointment nmpAppointment, Guid operatorId, string operateIP)
            : base(nmpId, new List<NMPAppointment>() { nmpAppointment }, operatorId, operateIP, new NMPCancelAppointmentManagerFactory(nmpAppointment.NMPEquipmentId.Value, nmpId, nmpAppointment.UserId.Value, nmpAppointment, operatorId, operateIP)) { }

    }
}
