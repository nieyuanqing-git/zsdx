using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public  class DoubtAuditorSamplePrivilige:SamplePrivilige
    {
        public DoubtAuditorSamplePrivilige()
        {
              _isEnableApply = true;
              _isEnableCopy = true;
              _isEnableEdit = true;
              _isEnableCancel = false;
              _isEnableAudit = false;
              _isEnableAccept = false;
              _isEnableRefuse = false;
              _isEnableCharge = false;
              _isEnableTest = false;
              _isEnableAnalysis = false;
              _isEnableReleaseResult = false;
              _isEnableAuditResult = false;
              _isEnableDownLoad = false;
              _isEnableDoubts = false;
              _isEnableAuditDoubts = true;
              _isEnableRegister = false;
              _isEnableRegisterStatus = false;
              _isEnableView = true;
              _isEnableReportConfirm = false;
        }
    }
}
