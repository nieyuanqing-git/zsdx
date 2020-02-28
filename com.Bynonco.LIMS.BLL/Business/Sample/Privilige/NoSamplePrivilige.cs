using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NoSamplePrivilige:SamplePrivilige
    {
        public NoSamplePrivilige()
        {
              _isEnableApply = false;
              _isEnableCopy = false;
              _isEnableEdit = false;
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
              _isEnableAuditDoubts = false;
              _isEnableRegister = false;
              _isEnableRegisterStatus = false;
              _isEnableView = false;
              _isEnableReportConfirm = false;
        }
    }
}
