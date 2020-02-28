using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class BJHGDXCostomer : DefaultCustomer
    {
        public override bool GetIsEnableInputAppointmentTimes()
        {
            return false;
        }
    }
}
