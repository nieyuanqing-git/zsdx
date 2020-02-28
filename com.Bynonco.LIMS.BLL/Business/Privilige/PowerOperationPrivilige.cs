using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class PowerOperationPrivilige : PriviligeBase
    {
        public PowerOperationPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "PowerOperation");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = viewModuleFunctionList.Any(p => p.ActionName == "List") || IsSuperAmin;
            IsEnableManage = viewModuleFunctionList.Any(p => p.ActionName == "Manage") || IsSuperAmin;
            IsEnableEdit = viewModuleFunctionList.Any(p => p.ActionName == "Edit") || IsSuperAmin;
            IsEnableDelete = viewModuleFunctionList.Any(p => p.ActionName == "Delete") || IsSuperAmin;
            IsEnableSave = viewModuleFunctionList.Any(p => p.ActionName == "Save") || IsSuperAmin;
            IsEnableExportExcel = viewModuleFunctionList.Any(p => p.ActionName == "ExportExcel") || IsSuperAmin;
        }

        public bool IsEnableList{ get; private set; }
        public bool IsEnableManage{ get; private set; }
        public bool IsEnableEdit{ get; private set; }
        public bool IsEnableDelete{ get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableExportExcel { get; private set; }
       
    }
}
