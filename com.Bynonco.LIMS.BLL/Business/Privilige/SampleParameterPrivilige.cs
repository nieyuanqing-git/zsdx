using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class SampleParameterPrivilige : PriviligeBase
    {
        public SampleParameterPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "SampleParameter");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableGetGridSampleParameterList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridSampleParameterList");
            IsEnableDeleteSampleParameter = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteSampleParameter");
            IsEnableSaveSampleParameter = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveSampleParameter");
            IsEnableAddSampleParameter = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddSampleParameter");
            IsEnableEditSampleParameter = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditSampleParameter");
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        public bool IsEnableGetGridSampleParameterList { get; private set; }
        public bool IsEnableDeleteSampleParameter { get; private set; }
        public bool IsEnableSaveSampleParameter { get; private set; }
        public bool IsEnableAddSampleParameter { get; private set; }
        public bool IsEnableEditSampleParameter { get; private set; }
    }
}

