
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //北京科技大学
    public class BJKJDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "BJKJDX";
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
        public override string GetHomeFooter(string xpath)
        {
            return __CODE + "Footer";
        }
        public override string GetHomeCollege()
        {
            return __CODE + "College";
        }
        public override bool GetIsShowMemuOrganizationList()
        {
            return false;
        }
        public override bool GetIsMainPageChangeToMore()
        {
            return true;
        }
        public override bool GetIsOriginalCheckLogin()
        {
            return true;
        }
        
    }
}
