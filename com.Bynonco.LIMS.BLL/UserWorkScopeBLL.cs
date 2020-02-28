using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL
{
    public class UserWorkScopeBLL : BLLBase<UserWorkScope>, IUserWorkScopeBLL
    {
        public IList<UserWorkScope> GetUserWorkScopeListByUserId(Guid userId)
        {
            return GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.ToString()));
        }
        public bool JudgeIsEquipmentAdmin(Guid equipmentId, Guid userId)
        {
            var userWorkScopeCount = CountModelListByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", equipmentId, userId));
            return userWorkScopeCount > 0;
        }
    }
}