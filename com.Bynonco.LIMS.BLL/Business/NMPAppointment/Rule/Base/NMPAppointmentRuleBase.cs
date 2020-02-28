using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public abstract class NMPAppointmentRuleBase : INMPAppointmentRule
    {
        protected INMPBLL _nmpBLL;
        protected IUserBLL _userBLL;
        protected EquipmentAppointmentTimes _equipmentAppoitmentTimes;
        protected NMPAppointmentRuleBase _next;
        protected bool __hasPreparedHook = false;
        protected NMP _nmp;
        protected NMPEquipment _nmpEquipment;
        protected User _user;
        public virtual void DoPrepare()
        {
            if (!__hasPreparedHook) Prepare();
            __hasPreparedHook = true;
        }
        protected abstract void Prepare();
        public NMPAppointmentRuleBase(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, NMP nmp,NMPEquipment nmpEquipment, User user)
        {
            _nmpBLL = BLLFactory.CreateNMPBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            this._equipmentAppoitmentTimes = equipmentAppoitmentTimes;
            this._next = next;
            this._nmp = nmp;
            this._nmpEquipment = nmpEquipment;
            this._user = user;
        }
        public abstract void Superimposing(EquipmentAppointmentTime appointmentTime);
        public NMPAppointmentRuleBase Next { get { return _next; } }

    }
}
