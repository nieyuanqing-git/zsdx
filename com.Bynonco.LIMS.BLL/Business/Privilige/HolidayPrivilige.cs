using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class HolidayPrivilige : PriviligeBase
    {
        private UserWorkScope __userWorkScope = null;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        public HolidayPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Holiday");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableGetGridHolidayList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridHolidayList");
            IsEnableGetHolidayIncludeList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetHolidayIncludeList");
            IsEnableGetHolidayExcludeList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetHolidayExcludeList");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableGetGridHolidayList { get; private set; }
        public bool IsEnableGetHolidayIncludeList { get; private set; }
        public bool IsEnableGetHolidayExcludeList { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableDelete { get; private set; }
    }
}
