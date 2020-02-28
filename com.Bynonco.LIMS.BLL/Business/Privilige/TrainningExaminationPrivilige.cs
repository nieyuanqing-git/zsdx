using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class TrainningExaminationPrivilige : PriviligeBase
    {
        public TrainningExaminationPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "TrainningExamination");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableList = GetIsEnableOpearteExtend(viewModuleFunctionList, "List");
            IsEnableManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "Manage");
            IsEnableViewQuestion = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewExaminationQuestion");
            IsEnableViewMaterial = GetIsEnableOpearteExtend(viewModuleFunctionList, "ViewExaminationMaterial");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnableSetQuestion = GetIsEnableOpearteExtend(viewModuleFunctionList, "Question");
            IsEnableSetMaterial = GetIsEnableOpearteExtend(viewModuleFunctionList, "Material");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableList { get; private set; }
        public bool IsEnableManage { get; private set; }
        public bool IsEnableViewQuestion { get; private set; }
        public bool IsEnableViewMaterial { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnableSetQuestion { get; private set; }
        public bool IsEnableSetMaterial { get; private set; }
    }
}
