using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class SamplePrivilige
    {
        protected bool _isEnableApply;
        protected bool _isEnableCopy;
        protected bool _isEnableEdit;
        protected bool _isEnableCancel;
        protected bool _isEnableAudit;
        protected bool _isEnableAccept;
        protected bool _isEnableRefuse;
        protected bool _isEnableCharge;
        protected bool _isEnableTest;
        protected bool _isEnableAnalysis;
        protected bool _isEnableReleaseResult;
        protected bool _isEnableAuditResult;
        protected bool _isEnableDownLoad;
        protected bool _isEnableDoubts;
        protected bool _isEnableAuditDoubts;
        protected bool _isEnableRegister;
        protected bool _isEnableRegisterStatus;
        protected bool _isEnableHandleInner;
        protected bool _isEnableHandleOuter;
        protected bool _isEnbaleInputFinishedQuatity;
        protected bool _isEnableView;
        protected bool _isEnableReportConfirm;
        public virtual bool IsEnableHandleOuter
        {
            get { return _isEnableHandleOuter; }
            set { _isEnableHandleOuter = value; }
        }
        public virtual bool IsEnableHandleInner
        {
            get { return _isEnableHandleInner; }
            set { _isEnableHandleInner = value; }
        }
        public virtual bool IsEnableApply
        {
            get { return _isEnableApply; }
            set { _isEnableApply = value; }
        }
        public virtual bool IsEnableCopy
        {
            get { return _isEnableCopy; }
            set { _isEnableCopy = value; }
        }

        public virtual bool IsEnableEdit
        {
            get { return _isEnableEdit; }
            set { _isEnableEdit = value; }
        }

        public virtual bool IsEnableAudit
        {
            get { return _isEnableAudit; }
            set { _isEnableAudit = value; }
        }

        public virtual bool IsEnableCancel
        {
            get { return _isEnableCancel; }
            set { _isEnableCancel = value; }
        }

        public virtual bool IsEnableAccept
        {
            get { return _isEnableAccept; }
            set { _isEnableAccept = value; }
        }

        public virtual bool IsEnableRefuse
        {
            get { return _isEnableRefuse; }
            set { _isEnableRefuse = value; }
        }

        public virtual bool IsEnableCharge
        {
            get { return _isEnableCharge; }
            set { _isEnableCharge = value; }
        }

        public virtual bool IsEnableTest
        {
            get { return _isEnableTest; }
            set { _isEnableTest = value; }
        }

        public virtual bool IsEnableAnalysis
        {
            get { return _isEnableAnalysis; }
            set { _isEnableAnalysis = value; }
        }

        public virtual bool IsEnableReleaseResult
        {
            get { return _isEnableReleaseResult; }
            set { _isEnableReleaseResult = value; }
        }

        public virtual bool IsEnableAuditResult
        {
            get { return _isEnableAuditResult; }
            set { _isEnableAuditResult = value; }
        }

        public virtual bool IsEnableDownLoad
        {
            get { return _isEnableDownLoad; }
            set { _isEnableDownLoad = value; }
        }

        public virtual bool IsEnableDoubts
        {
            get { return _isEnableDoubts; }
            set { _isEnableDoubts = value; }
        }

        public virtual bool IsEnableAuditDoubts
        {
            get { return _isEnableAuditDoubts; }
            set { _isEnableAuditDoubts = value; }
        }

        public virtual bool IsEnableRegister
        {
            get { return _isEnableRegister; }
            set { _isEnableRegister = value; }
        }


        public virtual bool IsEnableRegisterStatus
        {
            get { return _isEnableRegisterStatus; }
            set { _isEnableRegisterStatus = value; }
        }

        public virtual bool IsEnableInputFinishedQuatity
        {
            get { return _isEnbaleInputFinishedQuatity; }
            set { _isEnbaleInputFinishedQuatity = value; }
        }

        public virtual bool IsEnableView
        {
            get { return _isEnableView; }
            set { _isEnableView = value; }
        }


        public virtual bool IsEnableReportConfirm
        {
            get { return _isEnableReportConfirm; }
            set { _isEnableReportConfirm = value; }
        }

        
    }
}
