using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class NMPAppointmentManagerFactoryBase
    {
        protected Guid _nmpEquipmentId;
        protected Guid _nmpId;
        protected Guid _userId;
        protected IEnumerable<NMPAppointment> _nmpAppointments;
        protected Guid _operatorId;
        protected string _operateIP;
        public NMPAppointmentManagerFactoryBase(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments, Guid operatorId, string operateIP)
        {
            this._nmpEquipmentId = nmpEquipmentId;
            this._nmpId = nmpId;
            this._userId = userId;
            this._operatorId = operatorId;
            this._nmpAppointments = nmpAppointments;
            this._operateIP = operateIP;
        }
        public abstract NMPAppointmentValidateManagerBase CreateNMPAppointmentValidate();
        public abstract NMPAppointmentHandlerBase CreateNMPAppointmentHandler();
    } 
}
