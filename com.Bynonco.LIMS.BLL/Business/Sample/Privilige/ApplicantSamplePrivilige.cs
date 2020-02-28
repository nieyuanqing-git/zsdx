using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class ApplicantSamplePrivilige:SamplePrivilige
    {
        public ApplicantSamplePrivilige()
        {
              _isEnableApply = true;
              _isEnableCopy = true;
              _isEnableEdit = true;
              _isEnableCancel = true;
              _isEnableAudit = false;
              _isEnableAccept = false;
              _isEnableRefuse = false;
              _isEnableCharge = false;
              _isEnableTest = false;
              _isEnableAnalysis = false;
              _isEnableReleaseResult = false;
              _isEnableAuditResult = false;
              _isEnableDownLoad = true;
              _isEnableDoubts = true;
              _isEnableAuditDoubts = false;
              _isEnableRegister = false;
              _isEnableRegisterStatus = false;
              _isEnableView = true;
              _isEnableReportConfirm = false;
        }
    }
}
