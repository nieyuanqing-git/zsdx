using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public static class SmaplePriviligeExtend
    {
        public static UserEquipmentPrivilige ToUserEquipmentPrivilige(this SamplePrivilige samplePrivilige)
        {
            var userEquipmentPrivilige = new UserEquipmentPrivilige();
            userEquipmentPrivilige.IsEnableAccept = samplePrivilige.IsEnableAccept;
            userEquipmentPrivilige.IsEnableAnalysis = samplePrivilige.IsEnableAnalysis;
            userEquipmentPrivilige.IsEnableApply = samplePrivilige.IsEnableApply;
            userEquipmentPrivilige.IsEnableAuditDoubts = samplePrivilige.IsEnableAuditDoubts;
            userEquipmentPrivilige.IsEnableAuditResult = samplePrivilige.IsEnableAuditResult;
            userEquipmentPrivilige.IsEnableCancel = samplePrivilige.IsEnableCancel;
            userEquipmentPrivilige.IsEnableCharge = samplePrivilige.IsEnableCharge;
            userEquipmentPrivilige.IsEnableCopy = samplePrivilige.IsEnableCopy;
            userEquipmentPrivilige.IsEnableDoubts = samplePrivilige.IsEnableDoubts;
            userEquipmentPrivilige.IsEnableDownLoad = samplePrivilige.IsEnableDownLoad;
            userEquipmentPrivilige.IsEnableEdit = samplePrivilige.IsEnableEdit;
            userEquipmentPrivilige.IsEnableRefuse = samplePrivilige.IsEnableRefuse;
            userEquipmentPrivilige.IsEnableRegister = samplePrivilige.IsEnableRegister;
            userEquipmentPrivilige.IsEnableRegistStatus = samplePrivilige.IsEnableRegisterStatus;
            userEquipmentPrivilige.IsEnableRelease = samplePrivilige.IsEnableReleaseResult;
            userEquipmentPrivilige.IsEnableTest = samplePrivilige.IsEnableTest;
            userEquipmentPrivilige.IsEnableAudit = samplePrivilige.IsEnableAudit;
            userEquipmentPrivilige.IsEnableHandleInner = samplePrivilige.IsEnableHandleInner;
            userEquipmentPrivilige.IsEnableHandleOuter = samplePrivilige.IsEnableHandleOuter;
            userEquipmentPrivilige.IsEnableView = samplePrivilige.IsEnableView;
            userEquipmentPrivilige.IsEnableReportConfirm = samplePrivilige.IsEnableReportConfirm;
            return userEquipmentPrivilige;
        }
        public static SamplePrivilige ToSamplePrivilige(this UserEquipmentPrivilige userEquipmentPrivilige)
        {
            var samplePrivilige = new SamplePrivilige();
            samplePrivilige.IsEnableAccept = userEquipmentPrivilige.IsEnableAccept;
            samplePrivilige.IsEnableAnalysis = userEquipmentPrivilige.IsEnableAnalysis;
            samplePrivilige.IsEnableApply = userEquipmentPrivilige.IsEnableApply;
            samplePrivilige.IsEnableAuditDoubts = userEquipmentPrivilige.IsEnableAuditDoubts;
            samplePrivilige.IsEnableAuditResult = userEquipmentPrivilige.IsEnableAuditResult;
            samplePrivilige.IsEnableCancel = userEquipmentPrivilige.IsEnableCancel;
            samplePrivilige.IsEnableCharge = userEquipmentPrivilige.IsEnableCharge;
            samplePrivilige.IsEnableCopy = userEquipmentPrivilige.IsEnableCopy;
            samplePrivilige.IsEnableDoubts = userEquipmentPrivilige.IsEnableDoubts;
            samplePrivilige.IsEnableDownLoad = userEquipmentPrivilige.IsEnableDownLoad;
            samplePrivilige.IsEnableEdit = userEquipmentPrivilige.IsEnableEdit;
            samplePrivilige.IsEnableRefuse = userEquipmentPrivilige.IsEnableRefuse;
            samplePrivilige.IsEnableRegister = userEquipmentPrivilige.IsEnableRegister;
            samplePrivilige.IsEnableRegisterStatus = userEquipmentPrivilige.IsEnableRegistStatus;
            samplePrivilige.IsEnableReleaseResult = userEquipmentPrivilige.IsEnableRelease;
            samplePrivilige.IsEnableTest = userEquipmentPrivilige.IsEnableTest;
            samplePrivilige.IsEnableAudit = userEquipmentPrivilige.IsEnableAudit;
            samplePrivilige.IsEnableHandleInner = userEquipmentPrivilige.IsEnableHandleInner;
            samplePrivilige.IsEnableHandleOuter = userEquipmentPrivilige.IsEnableHandleOuter;
            samplePrivilige.IsEnableView = userEquipmentPrivilige.IsEnableView;
            samplePrivilige.IsEnableReportConfirm = userEquipmentPrivilige.IsEnableReportConfirm;
            return samplePrivilige;
        }
    }
}
