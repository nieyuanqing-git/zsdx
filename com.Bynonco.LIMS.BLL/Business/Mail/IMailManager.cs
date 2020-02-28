using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public interface IMailManager
    {
        void SendMail(Guid? operatorId, out int successCount, out int failCount, out string errorMessage);
    }
}
