using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleMTesteranager : SampleManager
    {
        public SampleMTesteranager(Guid userId)
            : base(userId) { }
        public override Logic.Model.GridData<Model.View.ViewSampleApply> GetGridSampleApplyList(JqueryEasyUI.Core.DataGridSettings dataGridSettings)
        {
            var queryExpression = "(Status=1+Status=5+Status=7+Status=8+Status=9+Status=10)";
            var dictcodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=UnTestSampleQueryExpression");
            if (dictcodeTypes != null && dictcodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(dictcodeTypes.First().Value))
                queryExpression = dictcodeTypes.First().Value.Trim();
            return _viewSampleApplyBLL.GetGridEntitiesByExpression(dataGridSettings.AppendAndQueryExpression(queryExpression + string.Format("*EnableOperatorId=\"{0}\"", _user.Id.ToString())), _MAPPING, "", true, false, true, _OUTMAPPINGS);
        }
    }
}
