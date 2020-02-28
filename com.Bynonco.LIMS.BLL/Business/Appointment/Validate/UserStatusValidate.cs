using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class UserStatusValidate: AppointmentValidateBase
    {
        public UserStatusValidate(Guid equipmentId, Guid userId)
            : base(equipmentId, userId) { }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            return _userBLL.CheckUserStatus(_userId, out errorMessage);
        }
    }
}
