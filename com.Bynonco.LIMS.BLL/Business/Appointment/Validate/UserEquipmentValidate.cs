using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class UserEquipmentValidate : AppointmentValidateBase
    {
        private IUserEquipmentBLL __userEquipmentBLL;
        public UserEquipmentValidate(Guid equipmentId, Guid userId)
            : base(equipmentId, userId)
        {
            __userEquipmentBLL = BLLFactory.CreateUserEquipmentBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage=  "";
            return __userEquipmentBLL.Validates(_equipmentId, _userId, out errorMessage);
        }
    }
}
