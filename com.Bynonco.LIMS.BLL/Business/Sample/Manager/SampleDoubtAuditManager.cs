﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleDoubtAuditManager: SampleManager
    {
        public SampleDoubtAuditManager(Guid userId)
            : base(userId) { }
        public override Logic.Model.GridData<Model.View.ViewSampleApply> GetGridSampleApplyList(DataGridSettings dataGridSettings)
        {
            var queryExpression = "(Status=15+Status=16)";
            var dictcodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=UnAuditDoubtSampleQueryExpression");
            if (dictcodeTypes != null && dictcodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(dictcodeTypes.First().Value))
                queryExpression = dictcodeTypes.First().Value.Trim();
            return _viewSampleApplyBLL.GetGridEntitiesByExpression(dataGridSettings.AppendAndQueryExpression(queryExpression + string.Format("*EnableOperatorId=\"{0}\"", _user.Id.ToString())),_MAPPING, "", true, false, true, _OUTMAPPINGS);
        }
    }
}
