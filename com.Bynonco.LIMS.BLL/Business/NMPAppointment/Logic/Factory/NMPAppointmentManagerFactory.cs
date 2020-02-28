using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPAppointmentManagerFactory : NMPAppointmentManagerFactoryBase
    {
        public NMPAppointmentManagerFactory(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments, Guid operatorId, string operateIP)
            : base(nmpEquipmentId, nmpId, userId, nmpAppointments, operatorId, operateIP) { }
        public override NMPAppointmentValidateManagerBase CreateNMPAppointmentValidate()
        {
            return new NMPAppointmentValidateManager(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments);
        }

        public override NMPAppointmentHandlerBase CreateNMPAppointmentHandler()
        {
            return new NMPAppointmentHandler(_nmpId,_nmpAppointments, _operatorId, _operateIP);
        }
    }
}
