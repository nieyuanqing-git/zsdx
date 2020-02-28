using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class DepositRecordBLL : BLLBase<DepositRecord>, IDepositRecordBLL
    {
        public double GetTotalDepositSumByUserId(Guid userId)
        {
            IUserAccountBLL userAccountBLL = BLLFactory.CreateUserAccountBLL();
            var userAccount = userAccountBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"", userId));
            var depositRecordList = GetEntitiesByExpression(string.Format("AccountId=\"{0}\"*HasChecked=true", userAccount.Id));
            if (depositRecordList == null || depositRecordList.Count == 0) return 0d;
            return (double)depositRecordList.Sum(p => p.DepositSum.Value);
        }
    }
}