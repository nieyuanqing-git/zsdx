using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPAppointmentTimesManager : INMPAppointmentTimesManager
    {
        protected INMPBLL _nmpBLL;
        protected INMPEquipmentBLL _nmpEquipmentBLL;
        protected IUserBLL _userBLL;
        protected Guid _nmpId;
        protected Guid _nmpEquipmentId;
        protected Guid? _userId;
        protected NMP _nmp;
        protected NMPEquipment _nmpEquipment;
        protected User _user;
        protected NMPAppointmentRuleBase _rootAppointmentRule;
        protected EquipmentAppointmentTimes _nmpAppointmentTimes;
        protected NMPAppointment __nmpAppointment;
        protected AppointmentTimeBehaviorOfUser _appointmentTimeBehaviorOfUser;
        protected Guid[] _selectNMPPartIds;
        private NMPAppointmentTimesManager(AppointmentTimeBehaviorOfUser appointmentTimeBehaviorOfUser, Guid nmpId,Guid nmpEquipmentId, Guid? userId, NMPAppointment nmpAppointment, Guid[] selectNMPPartIds)
        {
            _nmpBLL = BLLFactory.CreateNMPBLL();
            _nmpEquipmentBLL = BLLFactory.CreateNMPEquipmentBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            _appointmentTimeBehaviorOfUser = appointmentTimeBehaviorOfUser;
            _nmpId = nmpId;
            _nmpEquipmentId = nmpEquipmentId;
            _userId = userId;
            __nmpAppointment = nmpAppointment;
            _selectNMPPartIds = selectNMPPartIds;
            _nmp = _nmpBLL.GetEntityById(nmpId);
            _nmpEquipment = _nmpEquipmentBLL.GetEntityById(nmpEquipmentId); ;
            if (userId.HasValue) _user = _userBLL.GetEntityById(userId.Value);
        }
        public NMPAppointmentTimesManager(AppointmentTimeBehaviorOfUser appointmentTimeBehaviorOfUser, Guid nmpId,Guid nmpEquipmentId, Guid? userId, EquipmentAppointmentTimes equipmentAppointmentTimes, NMPAppointment nmpAppointment, Guid[] selectNMPPartIds)
            : this(appointmentTimeBehaviorOfUser, nmpId,nmpEquipmentId, userId, nmpAppointment, selectNMPPartIds)
        {
            _nmpAppointmentTimes = equipmentAppointmentTimes;
            GenerateRules();
        }
        public NMPAppointmentTimesManager(AppointmentTimeBehaviorOfUser appointmentTimeBehaviorOfUser, Guid nmpId, Guid nmpEquipmentId, Guid? userId, DateTime beginDate, DateTime endDate, NMPAppointment nmpAppointment, Guid[] selectNMPPartIds)
            : this(appointmentTimeBehaviorOfUser, nmpId, nmpEquipmentId, userId, nmpAppointment, selectNMPPartIds)
        {
            int appointmentDays = 0;
            _nmpAppointmentTimes = _nmpBLL.GetNMPAppoitmentTimes(_userId, nmpId, beginDate, endDate, out appointmentDays);
            GenerateRules();
        }
        private void GenerateRules()
        {
            //注意规则的先后顺序
            NMPAppointmentRuleBase speciaAppointmentRule = new NMPSpeciaAppointmentRule(_nmpAppointmentTimes, null, _nmpId, _nmpEquipmentId,_userId,_nmp, _user, __nmpAppointment,_nmpEquipment, _selectNMPPartIds);
            NMPAppointmentRuleBase bookedAppointmentRule = new NMPBookedAppointmentRule(_nmpAppointmentTimes, speciaAppointmentRule, _nmpId, _nmpEquipmentId, _userId, _nmp,_nmpEquipment, _user);
            NMPAppointmentRuleBase delinquencyAppointmentRule = new NMPDelinquencyAppointmentRule(_nmpAppointmentTimes, bookedAppointmentRule, _nmpId, _nmpEquipmentId, _userId, _nmp, _nmpEquipment, _user);
            NMPAppointmentRuleBase unAppointmentPeriodTimeAppointmentRule = new NMPUnAppointmentPeriodTimeAppointmentRule(_nmpAppointmentTimes, delinquencyAppointmentRule, _nmpId, _nmpEquipmentId, _nmp,_nmpEquipment, _user);
            NMPAppointmentRuleBase userEquipmentTimeAppointmentRule = new UserNMPTimeAppointmentRule(_nmpAppointmentTimes, unAppointmentPeriodTimeAppointmentRule, _nmpId, _nmpEquipmentId, _userId, _nmp,_nmpEquipment, _user);
            NMPAppointmentRuleBase tagEquipmentFunAppointmentRule = new NMPTagEquipmentFunAppointmentRule(_nmpAppointmentTimes, userEquipmentTimeAppointmentRule, _nmpId, _nmpEquipmentId, _userId, _nmp, _nmpEquipment, _user);
            NMPAppointmentRuleBase appointmentPriorityAppointmentRule = new NMPAppointmentPriorityAppointmentRule(_nmpAppointmentTimes, tagEquipmentFunAppointmentRule, _nmpId, _nmpEquipmentId, _userId, _nmp, _nmpEquipment, _user);
            NMPAppointmentRuleBase tagEnableBookDaysAppointmentRule = new NMPTagEnableBookDaysAppointmentRule(_appointmentTimeBehaviorOfUser, _nmpAppointmentTimes, appointmentPriorityAppointmentRule, _nmp, _nmpEquipment, _user);
            NMPAppointmentRuleBase outOfTimeAppointmentRule = new NMPOutOfTimeAppointmentRule(_nmpAppointmentTimes, tagEnableBookDaysAppointmentRule, _nmp,_nmpEquipment, _user);
            NMPAppointmentRuleBase workDayAppointmentRule = new NMPWorkDayAppointmentRule(_nmpAppointmentTimes, outOfTimeAppointmentRule, _nmpId, _nmpEquipmentId, _nmp, _nmpEquipment, _user);
            NMPAppointmentRuleBase userStatusAppointmentRule = new NMPUserStatusAppointmentRule(_nmpAppointmentTimes, workDayAppointmentRule, _nmpId, _nmpEquipmentId, _userId, _nmp,_nmpEquipment, _user);

            _rootAppointmentRule = userStatusAppointmentRule;
        }
        public EquipmentAppointmentTimes GetNMPAppointmentTimes()
        {
            if (_nmpAppointmentTimes == null || _nmpAppointmentTimes.AppoitmentTimes == null || _nmpAppointmentTimes.AppoitmentTimes.Count == 0)
                throw new Exception("预约信息为空");
            if (_rootAppointmentRule == null) return _nmpAppointmentTimes;
            foreach (var appointmentTime in _nmpAppointmentTimes.AppoitmentTimes)
            {
                var appointmentRule = _rootAppointmentRule;
                do
                {
                    appointmentRule.DoPrepare();
                    appointmentRule.Superimposing(appointmentTime);
                    appointmentRule = appointmentRule.Next;
                }
                while (appointmentRule != null);
            }
            return _nmpAppointmentTimes;
        }
        public bool Validate(out string errorMessage)
        {
            errorMessage = "";
            if (_nmpAppointmentTimes == null || _nmpAppointmentTimes.AppoitmentTimes == null || _nmpAppointmentTimes.AppoitmentTimes.Count == 0)
            {
                errorMessage = "预约信息为空";
                return false;
            }
            if (_rootAppointmentRule == null)
            {
                errorMessage = "预约规则为空";
                return false;
            }
            foreach (var appointmentTime in _nmpAppointmentTimes.AppoitmentTimes)
            {
                var appointmentRule = _rootAppointmentRule;
                do
                {
                    appointmentRule.DoPrepare();
                    appointmentRule.Superimposing(appointmentTime);
                    if (appointmentTime.Status != EquipmentAppointmentTimeStatus.Valid)
                    {
                        errorMessage = appointmentTime.Remark;
                        return false;
                    }
                    appointmentRule = appointmentRule.Next;
                }
                while (appointmentRule != null);
            }
            return true;
        }
    }
}
