
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL;

namespace com.Bynonco.LIMS.BLL.Business
{
    // 华创 广东华南新药创制中心
    public class HCCustomer : DefaultCustomer
    {
        private readonly string __CODE = "HC";
        public override string GetSystemName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SystemName");
        }
        public override string GetEquipmentShowTopInfo()
        {
            return __CODE + "ShowTopInfo";
        }
        public override string GetEquipmentShowBasic()
        {
            return __CODE + "ShowBasic";
        }
        public override bool GetIsArticleShowOneColumn()
        {
            return true;
        }
        public override bool GetIsArticleShowListOneColumn()
        {
            return true;
        }
        public override string GetEquipmentLinkMenName()
        {
            return "联系方式";
        }
    }
}
