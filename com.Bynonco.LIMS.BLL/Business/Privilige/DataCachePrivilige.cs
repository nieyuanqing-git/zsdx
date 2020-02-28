using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class DataCachePrivilige: PriviligeBase
    {
        public DataCachePrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "DataCache");
        }

        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableRefreshOrganizationDataCache = GetIsEnableOpearteExtend(viewModuleFunctionList, "RefreshOrganizationDataCache");
            IsEnableRefreshEquipmentCategoryDataCache = GetIsEnableOpearteExtend(viewModuleFunctionList, "RefreshEquipmentCategoryDataCache");
            IsEnableRefreshAllDataCache = GetIsEnableOpearteExtend(viewModuleFunctionList, "RefreshAllDataCache");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableRefreshOrganizationDataCache { get; private set; }
        public bool IsEnableRefreshEquipmentCategoryDataCache { get; private set; }
        public bool IsEnableRefreshAllDataCache { get; private set; }
       
    }
}
