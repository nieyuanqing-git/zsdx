using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class ZGSYDXCustomer : DefaultCustomer
    {
        public override bool GetIsShowCostDeductCalcDurationStatistics()
        {
            return true;
        }

        public override bool GetIsShowImportAuditUser()
        {
            return true;
        }
    }
}
