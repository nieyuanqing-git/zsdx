using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //珠江医院
    public class ZJYYCustomer : DefaultCustomer
    {
        private readonly string __CODE = "ZJYY";
        public override string GetSystemName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SystemName");
        }

        public override bool GetIsCheckAppointmentCancelTime()
        {
            return false;
        }
        
    }
}
