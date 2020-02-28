using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPCancelAppointmentManagerFactory : NMPAppointmentManagerFactoryBase
    {
        public NMPCancelAppointmentManagerFactory(Guid nmpEquipmentId, Guid nmpId, Guid userId, NMPAppointment nmpAppointment, Guid operatorId, string operateIP)
            : base(nmpEquipmentId, nmpId, userId, new List<NMPAppointment>() { nmpAppointment }, operatorId, operateIP) { }
        public override NMPAppointmentValidateManagerBase CreateNMPAppointmentValidate()
        {
            return new NMPCancelAppointmentValidateManager(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments.First());
        }

        public override NMPAppointmentHandlerBase CreateNMPAppointmentHandler()
        {
            return new NMPCancelAppointmentHandler(_nmpId,_nmpAppointments.First(), _operatorId, _operateIP);
        }
    }
}
