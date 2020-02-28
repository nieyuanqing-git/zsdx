using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class DelinquencyValidate : AppointmentValidateBase
    {
        private IPunishActionBLL __punishActionBLL;
        public DelinquencyValidate(Guid equipmentId, Guid userId)
            : base(equipmentId, userId)
        {
            __punishActionBLL = BLLFactory.CreatePunishActionBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage=  "";
            return __punishActionBLL.Validates(_userId, out errorMessage);
        }
    }
}
