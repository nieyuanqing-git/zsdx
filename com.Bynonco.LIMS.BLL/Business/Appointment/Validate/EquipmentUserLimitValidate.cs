using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class EquipmentUserLimitValidate: AppointmentValidateBase
    {
        private IAppointmentTimeLimitBLL __appointmentTimeLimitBLL;
        public EquipmentUserLimitValidate(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator)
            : base(equipmentId, userId, appointmentMidiator) 
        {
            __appointmentTimeLimitBLL = BLLFactory.CreateAppointmentTimeLimitBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            if (_appointmentMidiator.NewAppointments != null && _appointmentMidiator.NewAppointments.Count() > 0)
                return Validates(_appointmentMidiator.NewAppointments.First(), _appointmentMidiator.NewAppointments, out errorMessage);
            if (_appointmentMidiator.ChangeOldAppointment != null && _appointmentMidiator.ChangeNewAppointment != null)
                return Validates(_appointmentMidiator.ChangeOldAppointment, new Appointment[] { _appointmentMidiator.ChangeNewAppointment }, out errorMessage);
            return true;
        }
        private bool  Validates(Appointment appointment,IEnumerable<Appointment> newAppointments,out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var lstAppointments = new List<Appointment>();
                if (newAppointments != null && newAppointments.Count() > 0)
                {
                    foreach (var appointmentTemp in newAppointments)
                    {
                        //取消预约的记录,这里应用了一个小技巧,如果是新增预约的情况,第一个参数为新预约记录的第一条记录,第二个参数对应应该有两条与第一个参数一样的预约记录,因为第一个参数在算未计费的预约总金额时候会减去该记录的金额，所以第二参数需要两条与第一个参数一样的预约记录这样才能保证余额的准确
                        int count = appointmentTemp.Id == appointment.Id && newAppointments.Count(p => p.Id == appointmentTemp.Id) == 1 ? 2 : 1;
                        for (int i = 0; i < count; i++)
                        {
                            lstAppointments.Add(appointmentTemp);
                        }
                    }
                }
                var payer = appointment.PaymentMethodEnum == PaymentMethod.SubjectDirectorPay ? appointment.Subject.Director : appointment.User;
                var appointmentTimeLimitList = __appointmentTimeLimitBLL.GetUserAppointmentTimeLimitList(appointment.EquipmentId.Value,payer.Id);
                if (appointmentTimeLimitList == null || appointmentTimeLimitList.Count == 0)
                {
                    appointmentTimeLimitList = __appointmentTimeLimitBLL.GetUserAppointmentTimeLimitList(appointment.EquipmentId.Value, appointment.UserId.Value);
                    if (appointmentTimeLimitList == null || appointmentTimeLimitList.Count == 0) return true;
                    var appointmentTimeLimit = appointmentTimeLimitList.First();
                    var minDate = appointment.BeginTime.Value.Date.AddDays(-appointmentTimeLimit.Period + 1);
                    var endDate = appointment.BeginTime.Value.Date;
                    for (DateTime dateTime = minDate; dateTime <= endDate; dateTime = dateTime.AddDays(1))
                    {
                        var appointmentList = _appointmentBLL.GetUserPeriodAppointmentList(appointment.EquipmentId.Value, appointment.UserId.Value, dateTime, dateTime.AddDays(appointmentTimeLimit.Period));
                        if (appointmentList != null && appointmentList.Count > 0) lstAppointments.AddRange(appointmentList);
                        var currentHours = lstAppointments.Sum(p => (p.EndTime.Value - p.BeginTime.Value).TotalHours) - (appointment.EndTime.Value - appointment.BeginTime.Value).TotalHours;
                        if (appointmentTimeLimit.Duration >= currentHours) continue;
                        errorMessage = "超出设备用户预约限制【" + appointmentTimeLimit.Desription + "】";
                        return false;
                    }
                }
                else
                {
                    var appointmentTimeLimit = appointmentTimeLimitList.First();
                    var minDate = appointment.BeginTime.Value.Date.AddDays(-appointmentTimeLimit.Period + 1);
                    var endDate = appointment.BeginTime.Value.Date;
                    for (DateTime dateTime = minDate; dateTime <= endDate; dateTime = dateTime.AddDays(1))
                    {
                        var appointmentList = _appointmentBLL.GetUserPeriodAppointmentList(appointment.EquipmentId.Value, payer.Id, dateTime, dateTime.AddDays(appointmentTimeLimit.Period));
                        if (appointmentList != null && appointmentList.Count > 0) lstAppointments.AddRange(appointmentList);
                        var currentHours = lstAppointments.Sum(p => (p.EndTime.Value - p.BeginTime.Value).TotalHours) - (appointment.EndTime.Value - appointment.BeginTime.Value).TotalHours;
                        if (appointmentTimeLimit.Duration >= currentHours) continue;
                        errorMessage = "超出设备用户预约限制【" + appointmentTimeLimit.Desription + "】";
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
