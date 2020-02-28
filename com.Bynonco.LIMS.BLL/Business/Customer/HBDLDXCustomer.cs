using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    /// <summary>
    /// 华北电力大学
    /// </summary>
    public class HBDLDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "HBDLDX";              
        public override bool GetIsBindEquipmentLinkUser()
        {
            return false;
        }
    }
}
