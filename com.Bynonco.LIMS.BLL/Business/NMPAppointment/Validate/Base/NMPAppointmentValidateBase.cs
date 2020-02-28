using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class NMPAppointmentValidateBase
    {
        protected INMPAppointmentBLL _nmpAppointmentBLL;
        protected INMPBLL _nmpBLL;
        protected IUserBLL _userBLL;
        protected ICreditLimitBLL _creditLimitBLL;
        protected IEnumerable<NMPAppointment> _nmpAppointments;
        protected Guid _nmpEquipmentId;
        protected Guid _nmpId;
        protected NMP _nmp;
        protected Guid _userId;
        public NMPAppointmentValidateBase(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments)
            : this(nmpEquipmentId,nmpId, userId)
        {
            this._nmpAppointments = nmpAppointments;
        }
        public NMPAppointmentValidateBase(Guid nmpEquipmentId, Guid nmpId, Guid userId)
        {
            _nmpBLL = BLLFactory.CreateNMPBLL();
            _creditLimitBLL = BLLFactory.CreateCreditLimitBLL();
            _nmpAppointmentBLL = BLLFactory.CreateNMPAppointmentBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            this._nmpId = nmpId;
            this._nmpEquipmentId = nmpEquipmentId;
            this._userId = userId;
            this._nmp = _nmpBLL.GetEntityById(_nmpId);
        }
        public abstract bool Validates(out string errorMessage);
    }
}
