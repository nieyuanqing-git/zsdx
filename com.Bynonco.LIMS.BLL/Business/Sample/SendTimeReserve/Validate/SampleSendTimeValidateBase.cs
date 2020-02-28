using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL.Sample
{
    public abstract class SampleSendTimeValidateBase
    {
        protected Equipment _equipment;
        public SampleSendTimeValidateBase(Equipment equipment)
        {
            this._equipment = equipment;
        }
        public bool DoValidates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime)
        {
            errorMessage = "";
            if (!_equipment.IsEnableSelectSampleSendTime)
            {
                equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                equipmentAppointmentTime.Remark = "该设备不开放送样时间预约";
                return false;
            }
            if (equipmentAppointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid)
            {
                errorMessage = equipmentAppointmentTime.Remark;
                return false;
            }
            return Validates(out errorMessage, equipmentAppointmentTime);
        }
        protected abstract bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime);
    }
}
