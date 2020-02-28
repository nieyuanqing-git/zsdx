using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class UsedConfirmFeedBackValidate : AppointmentValidateBase
    {
        private IUsedConfirmBLL __usedConfirmBLL;
        public UsedConfirmFeedBackValidate(Guid equipmentId, Guid userId)
            : base(equipmentId, userId)
        {
            __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            return __usedConfirmBLL.Validates(_equipmentId, _userId, out errorMessage);
        }
    }
}
