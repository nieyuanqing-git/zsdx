using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class EthicsApplyBLL : BLLBase<EthicsApply>, IEthicsApplyBLL
    {
        public IList<EthicsApply> GetEthicsApplyListByUserId(Guid userId)
        {
            var ethicsApplyList =  GetEntitiesByExpression(string.Format("ApplicantId=\"{0}\"*Status={1}", userId, (int)EthicsApplyStatus.AuditedPass));
            return ethicsApplyList;
        }
    }
}
