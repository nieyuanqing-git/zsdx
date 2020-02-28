using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL.Sample
{
    public class SampleSendTimeOutOfEnableReserveDaysValidate:SampleSendTimeValidateBase
    {
        public SampleSendTimeOutOfEnableReserveDaysValidate(Equipment equipment)
            : base(equipment) { }

        protected override bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime)
        {
            errorMessage = "";
            if (DateTime.Now.AddDays(_equipment.SampleEnableSendDays) < equipmentAppointmentTime.BeginTime)
            {
                equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                equipmentAppointmentTime.Remark = "该时间段还未到提前开放预约时间,暂时不可预约";
                return false;
            }
            return true;
        }
    }
}
