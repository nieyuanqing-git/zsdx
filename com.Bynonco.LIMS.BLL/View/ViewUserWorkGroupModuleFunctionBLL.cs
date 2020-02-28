using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Utility;


namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewUserWorkGroupModuleFunctionBLL : BLLBase<ViewUserWorkGroupModuleFunction>, IViewUserWorkGroupModuleFunctionBLL
    {
        public IList<ViewUserWorkGroupModuleFunction> GetModuleFunctionListByWorkGroupIds(IEnumerable<Guid> workGroupIds)
        {
            if (workGroupIds == null || workGroupIds.Count() == 0) return null;
            return GetEntitiesByExpression(workGroupIds.ToFormatStr("WorkGroupId"));
        }
        public IList<ViewUserWorkGroupModuleFunction> GetModuleFunctionListByWorkGroupIdsControllerName(IEnumerable<Guid> workGroupIds, string controllerName,string addtionQueryExpression="")
        {
            if (workGroupIds == null || workGroupIds.Count() == 0 || string.IsNullOrWhiteSpace(controllerName)) return null;
            var queryExpression =  workGroupIds.ToFormatStr("WorkGroupId");
            queryExpression += !string.IsNullOrWhiteSpace(addtionQueryExpression) ?
                "*(ControllerName=\"" + controllerName + "\"" + "+" + addtionQueryExpression + ")" 
                : "*ControllerName=\"" + controllerName + "\"";
            return GetEntitiesByExpression(queryExpression);
        }
    }
}
