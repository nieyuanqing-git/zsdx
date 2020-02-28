using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPTagEnableBookDaysAppointmentRule : NMPAppointmentRuleBase
    {
        private AppointmentTimeBehaviorOfUser __appointmentTimeBehaviorOfUser;
        private int __appointmentDays = 0, enableAppointmentDays = 0;
        private int __aHaedOfTime = 0;
        private IEnumerable<DayOfWeek> __equipmentWorkDays;
        public NMPTagEnableBookDaysAppointmentRule(AppointmentTimeBehaviorOfUser appointmentTimeBehaviorOfUser, EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, NMP nmp, NMPEquipment nmpEquipment, User user)
            : base(equipmentAppoitmentTimes, next, nmp,nmpEquipment, user)
        {
            __appointmentTimeBehaviorOfUser = appointmentTimeBehaviorOfUser;
        }
        protected override void Prepare()
        {
            __appointmentDays = _nmp.AppointmentDays;
            __aHaedOfTime = _nmp.MaxAppointmentAdvanceTime;
            __equipmentWorkDays = WeekDayUtility.ConvertToWordDays(_nmp.WorkDays);

        }

        public override void Superimposing(EquipmentAppointmentTime appointmentTime)
        {
            if (__appointmentTimeBehaviorOfUser == AppointmentTimeBehaviorOfUser.Book)
            {
                var now = DateTime.Now.Date;
                var enableAppointmentDays = _nmpBLL.GetUserNMPAppoitmentDaysBeforeBeginDate(_user, __appointmentDays, __aHaedOfTime, __equipmentWorkDays, appointmentTime.BeginTime.Date);
                var dayGaps = (appointmentTime.EndTime.Date.Date - now.Date).TotalDays;
                if (dayGaps > enableAppointmentDays)
                {
                    appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                    appointmentTime.Remark = string.Format("{0}该时间段还未到提前开放预约时间,暂时不可预约", _user != null && _user.Tag != null ? "您的身份类型【" + _user.Tag.Name + "】" : "");
                }
            }
        }
    }
}
