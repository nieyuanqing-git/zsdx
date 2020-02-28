using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewHomeMenuBLL : BLLBase<ViewHomeMenu>, IViewHomeMenuBLL
    {
        public IList<ViewHomeMenu> GetViewHomeMenuByOrgXPath(string orgXPath, SystemType systemType)
        {
            if (string.IsNullOrEmpty(orgXPath)) orgXPath = "000";
            string queryExpression = string.Format("OrganizationXPath=\"{0}\"", orgXPath);
            queryExpression = queryExpression + "*IsForExamination=" + (systemType == SystemType.ShareE ? "false" : "true");
            var viewHomeMenuList = GetEntitiesByExpression(queryExpression, null, "XPath");
            return viewHomeMenuList;
        }
        public ViewHomeMenu GetFirstHomeMenuForExamination(string orgXPath)
        {
            if (string.IsNullOrEmpty(orgXPath)) orgXPath = "000";
            var viewHomeMenu = GetFirstOrDefaultEntityByExpression(string.Format("OrganizationXPath=\"{0}\"*IsForExamination=true*Url=-null*Url=-\"\"", orgXPath), null, "XPath");
            return viewHomeMenu;
        }
    }
}
