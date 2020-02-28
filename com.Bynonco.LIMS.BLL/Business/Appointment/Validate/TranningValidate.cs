using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class TranningValidate : AppointmentValidateBase
    {
        private IEquipmentTranningBLL __equipmentTranningBLL;
        public TranningValidate(Guid equipmentId, Guid userId) : base(equipmentId, userId)
        {
            __equipmentTranningBLL = BLLFactory.CreateEquipmentTranningBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage=  "";
            return __equipmentTranningBLL.Validates(_equipmentId, _userId, out errorMessage);
        }
    }
}
