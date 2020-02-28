
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //兰州大学
    public class LZDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "LZDX";
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
        public override string GetHomeMenu()
        {
            return __CODE + "Menu";
        }
        public override string GetHomeSkins(string xpath)
        {
            return __CODE + "Skins";
        }
        public override string GetHomeLayoutXPath(string xpath)
        {
            return xpath;
            //switch(xpath)
            //{
            //    case "000":
            //        xpath = "000";
            //        break;
            //    case "000000":
            //    case "000001":
            //    case "000002":
            //        xpath = "000000";
            //        break;
            //    case "000003":
            //    case "000004":
            //        xpath = "000003";
            //        break;
            //    case "000006":
            //    case "000007":
            //        xpath = "000006";
            //        break;
            //    case "000008":
            //    case "000009":
            //        xpath = "000008";
            //        break;
            //    case "000010":
            //    case "000011":
            //        xpath = "000010";
            //        break;
            //    case "000005":
            //    case "000012":
            //        xpath = "000012";
            //        break;
            //    case "000013":
            //    case "000014":
            //        xpath = "000013";
            //        break;
            //    case "000015":
            //        xpath = "000015";
            //        break;
            //}
            //return xpath;
        }
        public override bool GetIsRegistUserUploadCertificatePhoto()
        {
            return true;
        }
        public override bool GetIsDepositPhoto()
        {
            return true;
        }
    }
}
