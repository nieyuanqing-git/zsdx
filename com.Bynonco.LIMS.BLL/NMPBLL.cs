using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPBLL : BLLBase<NMP>, INMPBLL
    {
        private IUserBLL __userBLL;
        private AppointmentRelativeBLL __appointmentTimeRelativeBLL;
        private INMPPublicHolidaysBLL __nmpPublicHolidaysBLL;
        public NMPBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __nmpPublicHolidaysBLL = BLLFactory.CreateNMPPublicHolidaysBLL();
            __appointmentTimeRelativeBLL = new AppointmentRelativeBLL(__nmpPublicHolidaysBLL.GetValidatePublicHolidaysListForAppointment, __nmpPublicHolidaysBLL.ValidateForAppointment);
        }
        public int GetUserNMPAppoitmentDaysBeforeBeginDate(User user, int appointmentTimeDays, int aHaedOfTime, IEnumerable<DayOfWeek> equipmentWorkDays, DateTime beginDate)
        {
            return __appointmentTimeRelativeBLL.GetUserAppoitmentDaysBeforeBeginDate(user, appointmentTimeDays, aHaedOfTime, equipmentWorkDays, beginDate);
        }
        public EquipmentAppointmentTimes GetNMPAppoitmentTimes(Guid nmpId, Guid? userId, int aHaedOfTime, int maxAdvanceTime, int aHeadOfCancelTime, ref int appointmentTimeDays, int appointmentTimeStep, string workDays, string workHours, DateTime beginDate, DateTime endDate)
        {
            return __appointmentTimeRelativeBLL.GetAppoitmentTimes(nmpId, EquipmentTimeAppointmemtMode.CommondCalendar, userId, aHaedOfTime, maxAdvanceTime, aHeadOfCancelTime, ref appointmentTimeDays, appointmentTimeStep, workDays, workHours, beginDate, endDate);
        }
        public EquipmentAppointmentTimes GetNMPAppoitmentTimes(Guid? userId, Guid nmpId, DateTime beginDate, DateTime endDate, out int appointmentTimeDays)
        {
            var nmp = GetEntityById(nmpId);
            if (nmp == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的工位信息", nmpId.ToString()));
            var aHeadOfTime = nmp.OpenAdvanceTime;
            var maxAdvanceTime = nmp.MaxAppointmentAdvanceTime;
            var aHeadOfCancelTime = nmp.MinAppointmentCancelTime;
            appointmentTimeDays = nmp.AppointmentDays;
            var appointmentTimeStep = nmp.AppointmentTimeStep;
            var workDays = nmp.WorkDays;
            var workHours = nmp.WorkHours;
            return GetNMPAppoitmentTimes(nmpId,userId, aHeadOfTime, maxAdvanceTime, aHeadOfCancelTime, ref appointmentTimeDays, appointmentTimeStep, workDays, workHours, beginDate, endDate);
        }
        public string GetTutorUnAuditMessage()
        {
            return __appointmentTimeRelativeBLL.GetTutorUnAuditMessage();
        }
        public string GetUnTrainningMessage()
        {
            return __appointmentTimeRelativeBLL.GetUnTrainningMessage();
        }
        public string GetUnAuditMessage()
        {
            return __appointmentTimeRelativeBLL.GetUnAuditMessage();
        }
    }
}
