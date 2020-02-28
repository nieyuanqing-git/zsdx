using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class GDGYDXCustomer : DefaultCustomer
    {
        public override bool GetIsBindEquipmentLinkUser()
        {
            return false;
        }

        public override bool GetIsBindEquipmentAdminUser()
        {
            return false;
        }
        public override bool GetIsShowEquipmentPriceUnit()
        {
            return true;
        }
    }
}
