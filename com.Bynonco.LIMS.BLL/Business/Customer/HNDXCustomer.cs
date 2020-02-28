using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    /// <summary>
    /// 河南大学
    /// </summary>
    public class HNDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "HNDX";
        public override string GetEquipmentDirectorName()
        {
            return "管理员";
        }
        public override string GetEquipmentLinkMenName()
        {
            return "操作员";
        }
        public override bool GetIsSuperAdminOnlyShowAdminEqList()
        {
            return true;
        }
        public override bool GetIsSampleItemUnitUsedHourRequired()
        {
            return true;
        }
    }
}
