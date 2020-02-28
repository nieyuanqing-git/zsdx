
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //国防科技大学
    public class GFKJDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "GFKJDX";
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
        public override string GetHomeTopLogin()
        {
            return __CODE + "TopLogin";
        }
        public override bool GetIsHomeHideTopLogin()
        {
            return true;
        }
        public override bool GetIsShowFriendList()
        {
            return false;
        }
    }
}
