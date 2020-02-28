using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //中山大学生命科学学院
    public class ZSDXSMKXXYCustomer : DefaultCustomer
    {
        private readonly string __CODE = "ZSDXSMKXXY";
        public override string GetSystemName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SystemName");
        }
        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index";
        }
 
        public override bool GetIsRegistTutorMoneyCard()
        {
            return true;
        }
        public override string GetEquipmentSearchAdvanced()
        {
            return __CODE+"SearchAdvanced";
        }
    }
}
