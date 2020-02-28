using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class MessagePrivilige : PriviligeBase
    {
        public MessagePrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Message");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableView = GetIsEnableOpearteExtend(viewModuleFunctionList, "MessageView");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDeleteSend = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteMessageSend");
            IsEnableDeleteReceive = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteReceive");
            IsEnableDeleteDraft = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteMessageDraft");
            IsEnableSetReadedStatus = GetIsEnableOpearteExtend(viewModuleFunctionList, "SetReaded") && GetIsEnableOpearteExtend(viewModuleFunctionList, "SetNotReaded");
            IsEnableSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "Save");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableView { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableDeleteSend { get; private set; }
        public bool IsEnableDeleteReceive { get; private set; }
        public bool IsEnableDeleteDraft { get; private set; }
        public bool IsEnableSetReadedStatus { get; private set; }
        public bool IsEnableSave { get; private set; }
    }
}
