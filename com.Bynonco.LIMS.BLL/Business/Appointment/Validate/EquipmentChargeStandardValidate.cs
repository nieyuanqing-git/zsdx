using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class EquipmentChargeStandardValidate: AppointmentValidateBase
    {
        public EquipmentChargeStandardValidate(Guid equipmentId, Guid userId)
            : base(equipmentId, userId) { }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            if (_equipment.ChargeStandard == null)
            {
                errorMessage = "未定义设备的计费标准";
            }
            return true;
        }
    }
}
