
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //北京理工大学
    public class BJLGDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "BJLGDX";
        public override string GetSystemName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SystemName");
        }
        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index" + GetHomeLayoutXPath(xpath);
        }
        public override string GetHomeBanner(string xpath)
        {
            return __CODE + "Banner";
        }
        public override string GetHomeSkins(string xpath)
        {
            return __CODE + "Skins";
        }
        public override string GetHomeCollege()
        {
            return __CODE + "College";
        }
        public override string GetHomeLayoutXPath(string xpath)
        {
            switch(xpath)
            {
                case "000":
                    xpath = "000";
                    break;
                case "000000":
                    xpath = "000000";
                    break;
                default:
                    xpath = "";
                    break;
            }
            return xpath;
        }
        public override bool GetIsBindEquipmentLinkUser()
        {
            return false;
        }
    }
}
