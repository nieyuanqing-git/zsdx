using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class AppointmentValidateBase
    {
        protected IViewAppointmentBLL _viewAppointmentBLL;
        protected IAppointmentBLL _appointmentBLL;
        protected IEquipmentBLL _equipmentBLL;
        protected IUserBLL _userBLL;
        protected ICreditLimitBLL _creditLimitBLL;
        protected Guid _equipmentId;
        protected Equipment _equipment;
        protected Guid _userId;
        protected AppointmentMidiator _appointmentMidiator;
        public AppointmentValidateBase(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator)
            : this(equipmentId, userId)
        {
            this._appointmentMidiator = appointmentMidiator;
        }
        public AppointmentValidateBase(Guid equipmentId, Guid userId)
        {
            _equipmentBLL = BLLFactory.CreateEquipmentBLL();
            _creditLimitBLL = BLLFactory.CreateCreditLimitBLL();
            _appointmentBLL = BLLFactory.CreateAppointmentBLL();
            _viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            this._equipmentId = equipmentId;
            this._userId = userId;
            _equipment = _equipmentBLL.GetEntityById(_equipmentId);
        }
        public abstract bool Validates(out string errorMessage);
    }
}
