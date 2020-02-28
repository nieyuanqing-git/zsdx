using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleSearchManager : SampleManager
    {
        public SampleSearchManager(Guid userId)
            : base(userId) { }
        public override Logic.Model.GridData<Model.View.ViewSampleApply> GetGridSampleApplyList(DataGridSettings dataGridSettings)
        {
            var queryExpression = "(Status=0+Status=1+Status=2+Status=3+Status=4+Status=5+Status=6+Status=7+Status=8+Status=9+Status=10+Status=11+Status=12+Status=13+Status=14+Status=15+Status=16)";
            var dictcodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=SampleListSampleQueryExpression");
            if (dictcodeTypes != null && dictcodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(dictcodeTypes.First().Value))
                queryExpression = dictcodeTypes.First().Value.Trim();
            return _viewSampleApplyBLL.GetGridEntitiesByExpression(dataGridSettings.AppendAndQueryExpression(queryExpression + string.Format("*EnableOperatorId=\"{0}\"", _user.Id.ToString())), _MAPPING, "", true, false, true, _OUTMAPPINGS);
        }
    }
}
