using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class BJLYDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "BJLYDX";

        public override bool GetIsAudiExportSlwUser()
        {
            return true;
        }
    }
}
