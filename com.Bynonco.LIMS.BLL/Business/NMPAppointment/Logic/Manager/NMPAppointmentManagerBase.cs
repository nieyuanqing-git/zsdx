using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class NMPAppointmentManagerBase
    {
        protected IEnumerable<NMPAppointment> _nmpAppointments;
        protected Guid _nmpId;
        protected Guid _operatorId;
        protected string _operateIP;
        protected NMPAppointmentManagerFactoryBase _nmpAppointmentManagerFactory;
        public NMPAppointmentManagerBase(Guid nmpId,IEnumerable<NMPAppointment> nmpAppointments, Guid operatorId, string operateIP, NMPAppointmentManagerFactoryBase nmpAppointmentManagerFactory)
        {
            this._nmpAppointments = nmpAppointments;
            this._nmpId = nmpId;
            this._operatorId = operatorId;
            this._operateIP = operateIP;
            _nmpAppointmentManagerFactory = nmpAppointmentManagerFactory;
        }
        public bool Handler(out string errorMessage)
        {
            if (!_nmpAppointmentManagerFactory.CreateNMPAppointmentValidate().Validate(out errorMessage)) return false;
            return _nmpAppointmentManagerFactory.CreateNMPAppointmentHandler().Handle(out errorMessage);
        }
    }
}
