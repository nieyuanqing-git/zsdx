using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Sample
{
    public class SampleSendTimeExistValidate : SampleSendTimeValidateBase
    {
        public SampleSendTimeExistValidate(Equipment equipment)
            : base(equipment) { }

        protected override bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime)
        {
            errorMessage = "";
            var invalidateMsg = "该时间段不是定义的预约时间段";
            if (string.IsNullOrWhiteSpace(_equipment.SampleSendTimes))
            {
                equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                equipmentAppointmentTime.Remark = invalidateMsg;
            }
            var timeScopes = TimeUtility.GetTimeScopersByTimeFormatStr(_equipment.SampleSendTimes, _equipment.SampleSendTimeInerval, equipmentAppointmentTime.BeginTime.Date);
            if (!equipmentAppointmentTime.IsUnValidate && (timeScopes == null || !timeScopes.Any(p => p.BeginTime == equipmentAppointmentTime.BeginTime)))
            {
                equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                equipmentAppointmentTime.Remark = invalidateMsg;
                return false;
            }
            return true;
        }
    }
}
