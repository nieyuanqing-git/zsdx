using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class EquipmentNoticValidate:AppointmentValidateBase
    {
        private IEquipmentNoticeBLL __equipmentNoticeBLL;
        public EquipmentNoticValidate(Guid equipmentId, Guid userId)
            : base(equipmentId, userId)
        {
            __equipmentNoticeBLL = BLLFactory.CreateEquipmentNoticeBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage=  "";
            return __equipmentNoticeBLL.Validates(_equipmentId, _userId, out errorMessage);
        }
    }
}
