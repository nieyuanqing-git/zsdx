using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    class SampleOfficerPrivilige:SamplePrivilige
    {
        public SampleOfficerPrivilige()
        {
              _isEnableApply = true;
              _isEnableCopy = true;
              _isEnableEdit = true;
              _isEnableCancel = false;
              _isEnableAudit = false;
              _isEnableAccept = true;
              _isEnableRefuse = true;
              _isEnableCharge = false;
              _isEnableTest = false;
              _isEnableAnalysis = false;
              _isEnableReleaseResult = false;
              _isEnableAuditResult = false;
              _isEnableDownLoad = false;
              _isEnableDoubts = false;
              _isEnableAuditDoubts = false;
              _isEnableRegister = false;
              _isEnableRegisterStatus = false;
              _isEnableView = false;
              _isEnableReportConfirm = false;
        }
    }
}
