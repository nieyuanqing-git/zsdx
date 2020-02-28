
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //江西中医药大学
    public class JXZYYDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "JXZYYDX";
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
            return base.GetHomeBanner(xpath);
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
        public override bool GetIsShowFriendList()
        {
            return false;
        }
    }
}
