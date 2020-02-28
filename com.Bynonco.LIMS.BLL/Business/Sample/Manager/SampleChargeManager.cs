using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleChargeManager: SampleManager
    {
        public SampleChargeManager(Guid userId)
            : base(userId) { }
        public override Logic.Model.GridData<Model.View.ViewSampleApply> GetGridSampleApplyList(DataGridSettings dataGridSettings)
        {
            return _viewSampleApplyBLL.GetGridEntitiesByExpression(dataGridSettings, _MAPPING, "", true, false, true, _OUTMAPPINGS);
        }
    }
}
