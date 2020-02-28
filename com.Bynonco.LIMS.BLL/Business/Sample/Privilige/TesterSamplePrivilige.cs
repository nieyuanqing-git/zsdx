using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class TesterSamplePrivilige:SamplePrivilige
    {
        public TesterSamplePrivilige()
        {
            _isEnableApply = false;
            _isEnableCopy = false;
            _isEnableEdit = false;
            _isEnableCancel = false;
            _isEnableAudit = false;
            _isEnableAccept = false;
            _isEnableRefuse = false;
            _isEnableCharge = true;
            _isEnableTest = true;
            _isEnableAnalysis = true;
            _isEnableReleaseResult = true;
            _isEnableAuditResult = false;
            _isEnableDownLoad = false;
            _isEnableDoubts = false;
            _isEnableAuditDoubts = false;
            _isEnableRegister = false;
            _isEnableRegisterStatus = false;
            _isEnbaleInputFinishedQuatity = true;
            _isEnableView = true;
            _isEnableReportConfirm = false;
        }
    }
}
