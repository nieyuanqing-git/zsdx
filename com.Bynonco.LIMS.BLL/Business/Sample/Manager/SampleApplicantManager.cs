using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleApplicantManager : SampleManager
    {
        public SampleApplicantManager(Guid userId)
            : base(userId) { }
        public override Logic.Model.GridData<Model.View.ViewSampleApply> GetGridSampleApplyList(DataGridSettings dataGridSettings)
        {
            //var subjectList = _subjectBLL.GetSubjectListByDirectorId(_user.Id);
            if(!string.IsNullOrEmpty(dataGridSettings.QueryExpression))
            {
                dataGridSettings.QueryExpression = "(" + dataGridSettings.QueryExpression + ")";
                dataGridSettings.AppendAndQueryExpression(string.Format("(ApplicantId=\"{0}\"+ReportObtainerId=\"{0}\"+CreatorId=\"{0}\"+SubjectDirectorId=\"{0}\")", _user.Id.ToString()));
                return _viewSampleApplyBLL.GetGridEntitiesByExpression(dataGridSettings, _MAPPING, "", true, false, true, _OUTMAPPINGS);
            }
            return _viewSampleApplyBLL.GetGridEntitiesByExpression(dataGridSettings.AppendAndQueryExpression(string.Format("(ApplicantId=\"{0}\"+ReportObtainerId=\"{0}\"+CreatorId=\"{0}\"+SubjectDirectorId=\"{0}\")", _user.Id.ToString())), _MAPPING, "", true, false, true, _OUTMAPPINGS);
        }
    }
}
