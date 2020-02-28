
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //北京工业大学
    public class BJGYDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "BJGYDX";
        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index";
        }
        public override string GetHomeSkins(string xpath)
        {
            return __CODE + "Skins";
        }
 
        public override string GetHomeFooter(string xpath)
        {
            return __CODE + "Footer";
        }
        public override bool GetIsShowMemuOrganizationList()
        {
            return false;
        }
        public override bool GetIsShowExaminationSystemSpecialRelativeInfo()
        {
            return true;
        }
    }
}
