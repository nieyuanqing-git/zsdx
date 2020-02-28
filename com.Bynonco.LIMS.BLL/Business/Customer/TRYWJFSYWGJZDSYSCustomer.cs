using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    /// <summary>
    /// 北大医学部天然药物及仿真药物国家重点实验室
    /// </summary>
    public class TRYWJFSYWGJZDSYSCustomer:DefaultCustomer
    {
        public override bool GetIsAutoCalUsedConfirmFee()
        {
            return false;
        }
        public override bool GetIsOnlySuperAdminEnableHandleMinuseCostDeduct()
        {
            return true;
        }
        public override bool GetIsEnableInputAppointmentTimes()
        {
            return false;
        }
    }
}
