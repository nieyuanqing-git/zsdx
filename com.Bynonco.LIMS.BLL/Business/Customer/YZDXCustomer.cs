using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //扬州大学
    public class YZDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "YZDX";
        public override bool GetIsDepositPhoto()
        {
            return true;
        }
        public override bool GetIsDepositPhotoRequired()
        {
            return false;
        }
    }
}
