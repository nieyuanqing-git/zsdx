using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleSearcherPrivilige:SamplePrivilige
    {
        public SampleSearcherPrivilige()
        {
            _isEnableApply = false;
            _isEnableCopy = false;
            _isEnableEdit = false;
            _isEnableCancel = false;
            _isEnableAccept = false;
            _isEnableRefuse = false;
            _isEnableCharge = false;
            _isEnableAudit = false; 
            _isEnableTest = false;
            _isEnableAnalysis = false;
            _isEnableReleaseResult = false;
            _isEnableAuditResult = false;
            _isEnableDownLoad = false;
            _isEnableDoubts = false;
            _isEnableAuditDoubts = false;
            _isEnableRegister = false;
            _isEnableRegisterStatus = false;
            _isEnableView = true;
            _isEnableReportConfirm = false;
        }
    }
}
