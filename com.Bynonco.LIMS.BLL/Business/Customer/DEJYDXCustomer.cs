using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //第二军医大学
    public class DEJYDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "DEJYDX";
        public override string GetSystemName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SystemName");
        }
        public override string GetHomeIndex(string xpath)
        {
            if (xpath == "000") return __CODE + "Index";

            return "Index";
        }
        public override bool GetIsRegistTutorMoneyCard()
        {
            return true;
        }
        public override bool GetIsEnableInputAppointmentTimes()
        {
            return true;
        }
    }
}
