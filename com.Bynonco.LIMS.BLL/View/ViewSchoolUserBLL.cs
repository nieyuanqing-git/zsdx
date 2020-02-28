using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewSchoolUserBLL : BLLBase<ViewSchoolUser>, IViewSchoolUserBLL
    {
        public IList<ViewSchoolUser> GetUnsyncViewSchoolUserList(string syscQueryExpression, int syscCountPerTime)
        {
            //先导入导师后导入学生
            var queryExpression= string.Format("(UserType={0}+UserType={1})", (int)SchoolUserType.Tutor, (int)SchoolUserType.Student);
            if (!string.IsNullOrWhiteSpace(syscQueryExpression))
                queryExpression += "*" + syscQueryExpression;
            return GetScopeEntitiesByExpression(queryExpression, 1, syscCountPerTime, null, "UserType D");
        }
    }
}
