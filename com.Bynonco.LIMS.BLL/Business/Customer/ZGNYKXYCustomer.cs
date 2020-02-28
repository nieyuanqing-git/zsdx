using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //中国农业科学院=中国中医科学院
    public class ZGNYKXYCustomer : DefaultCustomer
    {
        private readonly string __CODE = "ZGNYKXY";
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
        public override bool GetIsShowEquipmetnCategoryForHome()
        {
            return true;
        }
        public override bool GetIsShowMenuEquipmetnSearch()
        {
            return true;
        }
        public override string GetHomeMenu()
        {
            return "MenuWithSearch";
        }
        public override string GetEquipmentIconListPage()
        {
            return __CODE + "PaginationItemForIcon";
        }
    }
}
