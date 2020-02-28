using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class UploadSamplePrivilige : SamplePrivilige
    {
        public UploadSamplePrivilige()
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
            _isEnableRegister = true;
            _isEnableRegisterStatus = false;
        }
    }
}
