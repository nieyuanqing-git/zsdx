using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class AppointmentManagerBase
    {
        protected AppointmentMidiator _appointmentMidiator;
        protected Guid _operatorId;
        protected string _operateIP;
        protected AppointmentManagerFactoryBase _appointmentManagerFactory;
        public AppointmentManagerBase(AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP, AppointmentManagerFactoryBase appointmentManagerFactory)
        {
            this._appointmentMidiator = appointmentMidiator;
            this._operatorId = operatorId;
            this._operateIP = operateIP;
            _appointmentManagerFactory = appointmentManagerFactory;
        }
        public bool Handler(out string errorMessage)
        {
            if (!_appointmentManagerFactory.CreateAppointmentValidate().Validate(out errorMessage)) return false;
            if (!_appointmentManagerFactory.CreateAppointmentValidate().ValidateAppointmentTime(out errorMessage)) return false;
            return _appointmentManagerFactory.CreateAppointmentHandler().Handle(out errorMessage);
        }
    }
}
