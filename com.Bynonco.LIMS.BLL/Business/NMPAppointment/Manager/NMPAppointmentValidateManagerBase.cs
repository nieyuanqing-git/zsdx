using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class NMPAppointmentValidateManagerBase
    {
        protected INMPBLL _nmpBLL;
        protected Guid _nmpEquipmentId;
        protected Guid _nmpId;
        protected Guid _userId;
        protected IEnumerable<NMPAppointment> _nmpAppointments;
        public NMPAppointmentValidateManagerBase(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments)
        {
            _nmpBLL = BLLFactory.CreateNMPBLL();
            this._nmpId = nmpId;
            this._nmpEquipmentId = nmpEquipmentId;
            this._userId = userId;
            this._nmpAppointments = nmpAppointments;
        }
        public virtual bool Validate(out string errorMessage)
        {
            errorMessage = "";
            var nmpAppointmentValidates = CreateNMPAppointmentValidates();
            if (nmpAppointmentValidates == null) return true;
            foreach (var nmpAppointmentValidate in nmpAppointmentValidates)
            {
                if (!nmpAppointmentValidate.Validates(out errorMessage)) return false;
            }
            return true;
        }
        protected abstract IList<NMPAppointmentValidateBase> CreateNMPAppointmentValidates();
    }
}
