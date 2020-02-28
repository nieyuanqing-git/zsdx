using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public interface IAutoCancelAppointmentManager
    {
        void Cancel(out int totalCount, out int successCount, out int failCount, out string errorMessage);
    }
}
