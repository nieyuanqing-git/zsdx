using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class AppointmentTimesManager : IAppointmentTimesManager
    {
        protected IEquipmentBLL _equipmentBLL;
        protected IUserBLL _userBLL;
        protected Guid _equipmentId;
        protected Guid? _userId;
        protected Equipment _equipment;
        protected User _user;
        protected AppointmentRuleBase _rootAppointmentRule;
        protected EquipmentAppointmentTimes _equipmentAppointmentTimes;
        protected Appointment __appointment;
        protected AppointmentTimeBehaviorOfUser _appointmentTimeBehaviorOfUser;
        protected Guid[] _selectEquipmentPartIds;
        protected EquipmentTimeAppointmemtMode _equipmentTimeAppointmemtMode;
        private AppointmentTimesManager(AppointmentTimeBehaviorOfUser appointmentTimeBehaviorOfUser, Guid equipmentId, Guid? userId, Appointment appointment, Guid[] selectEquipmentPartIds, EquipmentTimeAppointmemtMode equipmentTimeAppointmemtMode)
        {
            _equipmentBLL = BLLFactory.CreateEquipmentBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            _appointmentTimeBehaviorOfUser = appointmentTimeBehaviorOfUser;
            _equipmentId = equipmentId;
            _userId = userId;
            __appointment = appointment;
            _selectEquipmentPartIds = selectEquipmentPartIds;
            _equipment = _equipmentBLL.GetEntityById(equipmentId);
            if (userId.HasValue) _user = _userBLL.GetEntityById(userId.Value);
            _equipmentTimeAppointmemtMode = equipmentTimeAppointmemtMode;
        }
        public AppointmentTimesManager(AppointmentTimeBehaviorOfUser appointmentTimeBehaviorOfUser, Guid equipmentId, Guid? userId, EquipmentAppointmentTimes equipmentAppointmentTimes, Appointment appointment, Guid[] selectEquipmentPartIds, EquipmentTimeAppointmemtMode equipmentTimeAppointmemtMode)
            : this(appointmentTimeBehaviorOfUser, equipmentId, userId, appointment, selectEquipmentPartIds, equipmentTimeAppointmemtMode)
        {
            _equipmentAppointmentTimes = equipmentAppointmentTimes;
            GenerateRules();
        }
        public AppointmentTimesManager(AppointmentTimeBehaviorOfUser appointmentTimeBehaviorOfUser, Guid equipmentId, Guid? userId, DateTime beginDate, DateTime endDate, Appointment appointment, Guid[] selectEquipmentPartIds, EquipmentTimeAppointmemtMode equipmentTimeAppointmemtMode)
            : this(appointmentTimeBehaviorOfUser, equipmentId, userId, appointment, selectEquipmentPartIds, equipmentTimeAppointmemtMode)
        {
            int appointmentDays = 0;
            _equipmentAppointmentTimes = _equipmentBLL.GetEquipmentAppoitmentTimes(equipmentTimeAppointmemtMode,_userId, equipmentId, beginDate, endDate, out appointmentDays);
            GenerateRules();
        }
        private void GenerateRules()
        {
            //注意规则的先后顺序
            AppointmentRuleBase speciaAppointmentRule = new SpeciaAppointmentRule(_equipmentAppointmentTimes, null, _equipmentId, _userId, _equipment, _user, __appointment,_selectEquipmentPartIds);
            AppointmentRuleBase bookedAppointmentRule = new BookedAppointmentRule(_equipmentAppointmentTimes, speciaAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase equipmentStatusAppointmentRule = new EquipmentStatusAppointmentRule(_equipmentAppointmentTimes, bookedAppointmentRule, _equipmentId, _equipment, _user);
            AppointmentRuleBase delinquencyAppointmentRule = new DelinquencyAppointmentRule(_equipmentAppointmentTimes, equipmentStatusAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase equipmentBlackListAppointmentRule = new EquipmentBlackListAppointmentRule(_equipmentAppointmentTimes, delinquencyAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase usedConfirmFeedBackAppointmentRule = new UsedConfirmFeedBackAppointmentRule(_equipmentAppointmentTimes, equipmentBlackListAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase trainningAppointmentRule = new TrainningAppointmentRule(_equipmentAppointmentTimes, usedConfirmFeedBackAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase equipmentNoticeAppointmentRule = new EquipmentNoticeAppointmentRule(_equipmentAppointmentTimes, trainningAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase unAppointmentPeriodTimeAppointmentRule = new UnAppointmentPeriodTimeAppointmentRule(_equipmentAppointmentTimes, equipmentNoticeAppointmentRule, _equipmentId, _equipment, _user);
            AppointmentRuleBase userEquipmentTimeAppointmentRule = new UserEquipmentTimeAppointmentRule(_equipmentAppointmentTimes, unAppointmentPeriodTimeAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase tagEquipmentFunAppointmentRule = new TagEquipmentFunAppointmentRule(_equipmentAppointmentTimes, userEquipmentTimeAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase appointmentPriorityAppointmentRule = new AppointmentPriorityAppointmentRule(_equipmentAppointmentTimes, tagEquipmentFunAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase organizationAppointmentRule = new OrganizationAppointmentRule(_equipmentAppointmentTimes, appointmentPriorityAppointmentRule, _userId, _equipment, _user);
            AppointmentRuleBase tagEnableBookDaysAppointmentRule = new TagEnableBookDaysAppointmentRule(_appointmentTimeBehaviorOfUser,_equipmentAppointmentTimes, organizationAppointmentRule, _equipment, _user);
            AppointmentRuleBase outOfTimeAppointmentRule = new OutOfTimeAppointmentRule(_equipmentAppointmentTimes, tagEnableBookDaysAppointmentRule, _equipment, _user);
            AppointmentRuleBase whichWeekDayBeginOpenForBookAppointmentRule = new WhichWeekDayStopBookOfNextWeekAppointmentRule(_equipmentAppointmentTimes, outOfTimeAppointmentRule, _equipment, _user);
            AppointmentRuleBase holidayAppointmentRule = new HolidayAppointmentRule(_equipmentAppointmentTimes, whichWeekDayBeginOpenForBookAppointmentRule, _equipmentId, _equipment, _user);
            AppointmentRuleBase userEquipmentAppointmentRule = new UserEquipmentAppointmentRule(_equipmentAppointmentTimes, holidayAppointmentRule, _equipmentId, _userId, _equipment, _user);
            AppointmentRuleBase workDayAppointmentRule = new WorkDayAppointmentRule(_equipmentAppointmentTimes, userEquipmentAppointmentRule, _equipmentId, _equipment, _user);
            AppointmentRuleBase userStatusAppointmentRule = new UserStatusAppointmentRule(_equipmentAppointmentTimes, workDayAppointmentRule, _equipmentId, _userId, _equipment, _user);

            _rootAppointmentRule = userStatusAppointmentRule;
        }
        public EquipmentAppointmentTimes GetEquipmentAppointmentTimes()
        {
            if (_equipmentAppointmentTimes == null || _equipmentAppointmentTimes.AppoitmentTimes == null || _equipmentAppointmentTimes.AppoitmentTimes.Count == 0)
                throw new Exception("预约信息为空");
            if (_rootAppointmentRule == null) return _equipmentAppointmentTimes;
            foreach (var appointmentTime in _equipmentAppointmentTimes.AppoitmentTimes)
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
            LabelRelativeBLL._userListMap.Clear();
            return _equipmentAppointmentTimes;
        }
        public bool Validate(out string errorMessage)
        {
            errorMessage = "";
            if (_equipmentAppointmentTimes == null || _equipmentAppointmentTimes.AppoitmentTimes == null || _equipmentAppointmentTimes.AppoitmentTimes.Count == 0)
            {
                errorMessage = "预约信息为空";
                return false;
            }
            if (_rootAppointmentRule == null)
            {
                errorMessage = "预约规则为空";
                return false;
            }
            foreach (var appointmentTime in _equipmentAppointmentTimes.AppoitmentTimes)
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
