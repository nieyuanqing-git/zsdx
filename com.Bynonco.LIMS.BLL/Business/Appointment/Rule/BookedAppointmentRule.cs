using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class BookedAppointmentRule:AppointmentRuleBase
    {
        private IAppointmentBLL __appointmentBLL;
        private IList<Appointment> __appointmentList;
        private Guid __equipmentId;
        private Guid? __userId;
        public BookedAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Guid? userId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            this.__userId = userId;
            __appointmentBLL = BLLFactory.CreateAppointmentBLL();
            __equipmentId = equipmentId;
        }
        protected override void Prepare()
        {
            __appointmentList = __appointmentBLL.GetPeriodValidateAppointmentList( __equipmentId, _equipmentAppoitmentTimes.BeginDate.Value, _equipmentAppoitmentTimes.EndDate.Value);
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (__appointmentList == null || __appointmentList.Count == 0) return;
            foreach (var appointment in __appointmentList)
            {
                if (DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, appointment.BeginTime.Value, appointment.EndTime.Value))
                {
                    appointmentTime.Status = !__userId.HasValue || __userId.Value != appointment.UserId.Value ? EquipmentAppointmentTimeStatus.OtherPersonAppointmented : EquipmentAppointmentTimeStatus.SelfAppointmented;
                    StringBuilder sbRemark = new StringBuilder();
                    if (_equipment.IsShowAppointmentUserContactInfo && __userId.HasValue)
                    {
                        sbRemark.AppendFormat("该时间段被【{0}】预约了", appointment.User.UserName).AppendLine();
                        sbRemark.AppendFormat("联系电话:{0}", string.IsNullOrWhiteSpace(appointment.User.PhoneNumber) ? appointment.User.FixedPhone : appointment.User.PhoneNumber).AppendLine();
                        sbRemark.AppendFormat("电子邮箱:{0}", appointment.User.Email).AppendLine();
                        if (appointment.User.Tutor != null)
                        {
                            sbRemark.AppendFormat("导师:{0}", appointment.User.Tutor.UserName).AppendLine();
                        }
                    }
                    else
                    {
                        sbRemark.Append("该时间段被预约了").AppendLine();
                    }
                    appointmentTime.CurAppointment = appointment;
                    appointmentTime.Remark = sbRemark.ToString();
                }
            }
        }
    }
}
