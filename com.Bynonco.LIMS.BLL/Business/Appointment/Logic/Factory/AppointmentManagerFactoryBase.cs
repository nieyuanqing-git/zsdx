using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class AppointmentManagerFactoryBase
    {
        protected Guid _equipmentId;
        protected Guid _userId;
        protected AppointmentMidiator _appointmentMidiator;
        protected Guid _operatorId;
        protected string _operateIP;
        public AppointmentManagerFactoryBase(Guid equipmentId, Guid userId,AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP)
        {
            this._equipmentId = equipmentId;
            this._userId = userId;
            this._operatorId = operatorId;
            this._appointmentMidiator = appointmentMidiator;
            this._operateIP = operateIP;
        }
        public abstract AppointmentValidateManagerBase CreateAppointmentValidate();
        public abstract AppointmentHandlerBase CreateAppointmentHandler();
    } 
}
