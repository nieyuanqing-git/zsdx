using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleTransactorSamplePrivilige:SamplePrivilige
    {
        public SampleTransactorSamplePrivilige()
        {
            _isEnableApply = true;
            _isEnableCopy = true;
            _isEnableEdit = true;
            _isEnableCancel = true;
            _isEnableAccept = true;
            _isEnableRefuse = false;
            _isEnableCharge = false;
            _isEnableAudit = true;
            _isEnableTest = false;
            _isEnableAnalysis = false;
            _isEnableReleaseResult = false;
            _isEnableAuditResult = false;
            _isEnableDownLoad = false;
            _isEnableDoubts = false;
            _isEnableAuditDoubts = false;
            _isEnableRegister = true;
            _isEnableRegisterStatus = false;
            _isEnableHandleInner = true;
            _isEnableHandleOuter = true;
            _isEnableView = true;
            _isEnableReportConfirm = false;
            _isEnbaleInputFinishedQuatity = true;
        }
    }
}
