
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //天然药物
    public class SKLNBDCustomer : DefaultCustomer
    {
        private readonly string __CODE = "SKLNBD";
        public override string GetHomeIndex(string xpath)
        {
            if (xpath == "000000") return __CODE + "Index";
            return "Index";
        }
        public override string GetHomeSkins(string xpath)
        {
            if (xpath == "000000") return __CODE + "Skins";
            return "DefaultSkins";
        }
        public override string GetHomeBanner(string xpath)
        {
            if (xpath == "000000") return __CODE + "Banner";
            return "DefaultBanner";
        }
        public override string GetEquipmentShowList()
        {
            return __CODE + "ShowList";
        }
        public override string GetEquipmentShowTopInfo()
        {
            return __CODE + "ShowTopInfo";
        }
        public override bool GetIsAutoCalUsedConfirmFee()
        {
            return false;
        }
        public override bool GetIsOnlySuperAdminEnableHandleMinuseCostDeduct()
        {
            return true;
        }
        public override bool GetIsEnableInputAppointmentTimes()
        {
            return false;
        }
    }
}
