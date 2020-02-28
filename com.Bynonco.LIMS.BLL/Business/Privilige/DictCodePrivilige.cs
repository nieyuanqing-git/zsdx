using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class DictCodePrivilige : PriviligeBase
    {
        public DictCodePrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "DictCodeType");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridDictCodeList");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditDictCode");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveDictCodes");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteDictCodes");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddDictCode");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableDelete { get; private set; }
    }
}
