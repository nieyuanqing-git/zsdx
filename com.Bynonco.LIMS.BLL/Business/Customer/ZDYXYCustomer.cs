
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //中大药学院
    public class ZDYXYCustomer : DefaultCustomer
    {
        private readonly string __CODE = "ZDYXY";
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
        public override bool GetIsEnableInputAppointmentTimes()
        {
            return false;
        }
        public override string GetEquipmentShowBasic()
        {
            return __CODE + "ShowBasic";
        }
        public override string GetCode()
        {
            return __CODE;
        }

    }
}
