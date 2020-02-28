
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //孙逸仙医院
    public class SYXYYCustomer : DefaultCustomer
    {
        private readonly string __CODE = "SYXYY";
        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index";
        }
        public override string GetHomeSkins(string xpath)
        {
            return __CODE + "Skins";
        }
        public override string GetHomeBanner(string xpath)
        {
            return __CODE + "Banner";
        }
       
    }
}
