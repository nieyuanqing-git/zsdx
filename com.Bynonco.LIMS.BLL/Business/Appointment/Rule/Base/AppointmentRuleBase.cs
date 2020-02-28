using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public abstract class AppointmentRuleBase : IAppointmentRule
    {
        protected IEquipmentBLL _equipmentBLL;
        protected IUserBLL _userBLL;
        protected EquipmentAppointmentTimes _equipmentAppoitmentTimes;
        protected AppointmentRuleBase _next;
        protected bool __hasPreparedHook = false;
        protected Equipment _equipment;
        protected User _user;
        public virtual void DoPrepare()
        {
            if (!__hasPreparedHook) Prepare();
            __hasPreparedHook = true;
        }
        protected abstract void Prepare();
        public AppointmentRuleBase(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next,Equipment equipment,User user)
        {
            _equipmentBLL = BLLFactory.CreateEquipmentBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            this._equipmentAppoitmentTimes = equipmentAppoitmentTimes;
            this._next = next;
            this._equipment = equipment;
            this._user = user;
        }
        public abstract void Superimposing(EquipmentAppointmentTime appointmentTime);
        public AppointmentRuleBase Next { get { return _next; } }

    }
}
