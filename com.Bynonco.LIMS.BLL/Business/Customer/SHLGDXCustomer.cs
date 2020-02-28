
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //上海理工大学
    public class SHLGDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "SHLGDX";
        public override bool GetIsRegistryCard()
        {
            return true;
        }
        public override bool GetIsUserWorkTeam()
        {
            return true;
        }
    }
}
