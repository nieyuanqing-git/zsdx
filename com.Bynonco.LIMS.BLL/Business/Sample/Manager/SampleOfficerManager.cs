using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleOfficerManager: SampleManager
    {
        protected static DictCodeType _isNeedConfirmAcceptSampleDictCodeType;
        protected bool _isNeedConfirmAcceptSample = false;
        public SampleOfficerManager(Guid userId)
            : base(userId) 
        {
            if (_isNeedConfirmAcceptSampleDictCodeType == null)
                _isNeedConfirmAcceptSampleDictCodeType = _dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=IsNeedConfirmAcceptSample");
            if (_isNeedConfirmAcceptSampleDictCodeType != null)
                _isNeedConfirmAcceptSample = !string.IsNullOrWhiteSpace(_isNeedConfirmAcceptSampleDictCodeType.Value) && _isNeedConfirmAcceptSampleDictCodeType.Value.Trim() == "1";
        }
        public override Logic.Model.GridData<Model.View.ViewSampleApply> GetGridSampleApplyList(DataGridSettings dataGridSettings)
        {
            var queryExpression = "(Status=1+Status=2+Status=3+Status=4+Status=5+Status=6+Status=7+Status=8+Status=9+Status=10+Status=11+Status=12+Status=13+Status=14+Status=15+Status=16)";
            var dictcodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=SampleOfficeSampleListQueryExpression");
            if (dictcodeTypes != null && dictcodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(dictcodeTypes.First().Value))
                queryExpression = dictcodeTypes.First().Value.Trim();
            if (!_isNeedConfirmAcceptSample)
            {
                return _viewSampleApplyBLL.GetGridEntitiesByExpression(dataGridSettings.AppendAndQueryExpression(queryExpression + string.Format("*CreatorId=\"{0}\"*IsOuter=true", _user.Id.ToString())), _MAPPING, "", true, false, true, _OUTMAPPINGS);
            }
            return _viewSampleApplyBLL.GetGridEntitiesByExpression(dataGridSettings.AppendAndQueryExpression(queryExpression), _MAPPING, "", true, false, true, _OUTMAPPINGS);
        }
    }
}
