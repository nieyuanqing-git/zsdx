using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class DefaultTagEquipmentFunPrivilige: PriviligeBase
    {
        public DefaultTagEquipmentFunPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "DefaultTagEquipmentFun");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableSaveDefaultTagEquipmentFun = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveDefaultTagEquipmentFun");
            IsEnableGetGridDefaultTagEquipmentFunList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridDefaultTagEquipmentFunList");
            IsEnableAddDefaultTagEquipmentFun = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddDefaultTagEquipmentFun");
            IsEnableEditDefaultTagEquipmentFun = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditDefaultTagEquipmentFun");
            IsEnableDeleteDefaultTagEquipmentFun = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteDefaultTagEquipmentFun");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableGetGridDefaultTagEquipmentFunList { get; private set; }
        public bool IsEnableAddDefaultTagEquipmentFun { get; private set; }
        public bool IsEnableEditDefaultTagEquipmentFun { get; private set; }
        public bool IsEnableSaveDefaultTagEquipmentFun { get; private set; }
        public bool IsEnableDeleteDefaultTagEquipmentFun { get; private set; }
    }
}
