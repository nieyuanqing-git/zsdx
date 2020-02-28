using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPAdminBLL : BLLBase<NMPAdmin>, INMPAdminBLL
    {
        public IList<NMPAdmin> GetNMPAdminListByAdminId(Guid adminId)
        {
            return GetEntitiesByExpression(string.Format("AdminId=\"{0}\"", adminId));
        }
    }
}
