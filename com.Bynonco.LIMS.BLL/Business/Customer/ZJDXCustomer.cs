using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //浙江大学
    public class ZJDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "ZJDX";
        public override string GetSystemName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SystemName");
        }
        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index";
        }
        public override string GetHomeBanner(string xpath)
        {
            return __CODE + "Banner";
        }
        public override string GetHomeSkins(string xpath)
        {
            return __CODE + "Skins";
        }
        public override string GetEquipmentShow()
        {
            return __CODE + "Show";
        }

        public override bool GetIsShowNewEquipmetn()
        {
            return true;
        }

        public override bool GetIsShowEquipmentMenuRedirectToHome()
        {
            return true;
        }
        public override string GetHomeTopLogin()
        {
            return __CODE + "TopLogin";
        }
        public override string GetHomeFooter(string xpath)
        {
            return __CODE + "Footer";
        }
        public override string GetHomeMenu()
        {
            return "";
        }
    }
}
