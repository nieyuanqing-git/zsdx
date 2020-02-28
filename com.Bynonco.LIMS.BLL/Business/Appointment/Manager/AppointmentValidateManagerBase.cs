using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class AppointmentValidateManagerBase
    {
        protected IEquipmentBLL _equipmentBLL;
        protected IDictCodeTypeBLL _dictCodeTypeBLL;
        protected Guid _equipmentId;
        protected Guid _userId;
        protected AppointmentMidiator _appointmentMidiator;
        protected EquipmentTimeAppointmemtMode _equipmentTimeAppointmemtMode;
        public AppointmentValidateManagerBase(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator)
        {
            _equipmentBLL = BLLFactory.CreateEquipmentBLL();
            _dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            this._equipmentId = equipmentId;
            this._userId = userId;
            _appointmentMidiator = appointmentMidiator;
            _equipmentTimeAppointmemtMode = _dictCodeTypeBLL.GetEquipmentTimeAppointmemtMode();
        }

        public virtual bool ValidateAppointmentTime(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        public virtual bool Validate(out string errorMessage)
        {
            errorMessage = "";
            var appointmentValidates = CreateAppointmentValidates();
            if (appointmentValidates == null) return true;
            foreach (var appointmentValidate in appointmentValidates)
            {
                if (!appointmentValidate.Validates(out errorMessage)) return false;
            }
            return true;
        }
        protected abstract IList<AppointmentValidateBase> CreateAppointmentValidates();
    }
}
