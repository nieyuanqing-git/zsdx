using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class EquipmentCalcFeeRuleValidate : AppointmentValidateBase
    {
        public EquipmentCalcFeeRuleValidate(Guid equipmentId, Guid userId)
            : base(equipmentId, userId) { }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            if (_equipment.CalcFeeTimeRule == null)
            {
                errorMessage = "未定义设备的计费时间规则";
            }
            return true;
        }
    }
}
