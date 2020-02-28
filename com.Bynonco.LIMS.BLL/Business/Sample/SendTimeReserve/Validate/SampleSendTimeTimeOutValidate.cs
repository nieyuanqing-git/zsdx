using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Sample
{
    public class SampleSendTimeTimeOutValidate : SampleSendTimeValidateBase
    {
        public SampleSendTimeTimeOutValidate(Equipment equipment)
            : base(equipment) { }

        protected override bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime)
        {
            errorMessage = "";
            if (!equipmentAppointmentTime.IsUnValidate && equipmentAppointmentTime.EndTime <= DateTime.Now)
            {
                equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                equipmentAppointmentTime.Remark = "该时间段已经失效";
                return false;
            }
            return true;
        }
    }
}
