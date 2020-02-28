using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class EquipmentBlackListValidate : AppointmentValidateBase
    {
        private IEquipmentBlackListBLL __equipmentBlackListBLL;
        public EquipmentBlackListValidate(Guid equipmentId, Guid userId)
            : base(equipmentId, userId)
        {
            __equipmentBlackListBLL = BLLFactory.CreateEquipmentBlackListBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage=  "";
            return __equipmentBlackListBLL.Validates(_equipmentId, _userId, out errorMessage);
        }
    }
}
