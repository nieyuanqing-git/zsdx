using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class AnimalOutAppointmentPrivilige: PriviligeBase
    {
        public AnimalOutAppointmentPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "AnimalOutAppointment");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableGetGridViewAnimalOutAppointmentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewAnimalOutAppointmentList");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableCancel = GetIsEnableOpearteExtend(viewModuleFunctionList, "Cancel");
            IsEnablePass = GetIsEnableOpearteExtend(viewModuleFunctionList, "Pass");
            IsEnableRefuse = GetIsEnableOpearteExtend(viewModuleFunctionList, "Refuse");
            IsEnableFeedBack = GetIsEnableOpearteExtend(viewModuleFunctionList, "FeedBack");
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "View");
            IsEnablePrint = GetIsEnableOpearteExtend(viewModuleFunctionList, "Print");
            
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableGetGridViewAnimalOutAppointmentList { get; private set; }
        public bool IsEnableSave { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableCancel { get; private set; }
        public bool IsEnablePass { get; private set; }
        public bool IsEnableRefuse { get; private set; }
        public bool IsEnableFeedBack { get; private set; }
        public bool IsEnableView { get; private set; }
        public bool IsEnablePrint { get; private set; }
    }
}
