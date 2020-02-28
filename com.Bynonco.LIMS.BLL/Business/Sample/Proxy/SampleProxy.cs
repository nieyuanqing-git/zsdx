using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL;
using com.Bynonco.LIMS.BLL.View;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public abstract partial class SampleProxy
    {
        protected IDictCodeTypeBLL _dictCodeTypeBLL;
        protected IUserBLL _userBLL;
        protected IUserEquipmentPriviligeBLL _userEquipmentPriviligeBLL;
        protected SampleFactory _sampleFactory;
        protected User _user;
        protected SamplePrivilige _samplePrivilige;
        protected SampleManager _sampleManager;
        protected IViewSampleTesterBLL _viewSampleTesterBLL;
        protected ISampleTesterBLL _sampleTesterBLL;
        protected ISampleApplyBLL _sampleApplyBLL;
        protected ISampleTestRecordBLL _sampleTestRecordBLL;
        protected IViewSampleApplyBLL _viewSampleApplyBLL;
        public SampleProxy(Guid userId, SampleFactory sampleFactory)
        {
            this._sampleFactory = sampleFactory;
            _userBLL = BLLFactory.CreateUserBLL();
            _dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            _userEquipmentPriviligeBLL = BLLFactory.CreateUserEquipmentPriviligeBLL();
            _viewSampleTesterBLL = BLLFactory.CreateViewSampleTesterBLL();
            _sampleTesterBLL = BLLFactory.CreateSampleTesterBLL();
            _samplePrivilige = _sampleFactory.CreateSampePrivlige();
            _sampleManager = _sampleFactory.CreateSampleManager();
            _sampleTesterBLL = BLLFactory.CreateSampleTesterBLL();
            _sampleApplyBLL = BLLFactory.CreateSampleApplyBLL();
            _sampleTestRecordBLL = BLLFactory.CreateSampleTestRecordBLL();
            _viewSampleApplyBLL = BLLFactory.CreateViewSampleApplyBLL();
            _user = _userBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", userId.ToString())).First();
            
        }
        protected virtual bool ApplyHook { get { return false; } }
        protected virtual bool CopyHook { get { return false; } }
        protected virtual bool EditHook { get { return false; } }
        protected virtual bool CancelHook { get { return false; } }
        protected virtual bool DownLoadHook { get { return false; } }
        protected virtual bool DoubtsHook { get { return false; } }
        protected virtual bool ViewHook { get { return false; } }
        public abstract void DoPriviligeFacade(ViewSampleApply viewSampleApply);
        public abstract string BuildOperate(ViewSampleApply viewSampleApply);
        public virtual UserEquipmentPrivilige GetUserEquipmentPrivilige(ViewSampleApply viewSampleApply)
        {
            var userSamplePrivilige = viewSampleApply == null ?
               _userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserId(_user.Id) :
               _userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserItemId(_user.Id, viewSampleApply.SampleItemId);
            return userSamplePrivilige == null ? _samplePrivilige.ToUserEquipmentPrivilige() : userSamplePrivilige;
        }
        private bool JudgeIsEnableOperate(Guid operatorId,SampleApply sampleApply, SampleOperate sampleOperate,object[] parameters,out string errorMessage)
        {
            errorMessage = "";
            if (sampleApply == null)
            {
                errorMessage = "申请单为空";
                return false;
            }
            switch (sampleOperate)
            {
                case SampleOperate.TutorAudit:
                    if (parameters == null || parameters.Length == 0)
                    {
                        errorMessage = "审核导师信息为空";
                        return false;
                    }
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.TutorAudit, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableTutorAudit(operatorId, new Guid(parameters[0].ToString()), sampleApply.AuditTutorId, sampleApply.Applicant.TutorId.Value, sampleApply.SampleNo, sampleApply.RowNo, sampleApply.IsNeedTutorAudit, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Apply:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, ApplyHook, SampleOperate.Apply, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableApply(operatorId, ApplyHook, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Edit:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, EditHook, SampleOperate.Edit, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableEdit(operatorId, sampleApply.ApplicantId, sampleApply.CreatorId.Value, sampleApply.StatusEnum, EditHook, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Cancel:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, CancelHook, SampleOperate.Cancel, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableCancel(operatorId, sampleApply.ApplicantId, sampleApply.CreatorId.Value, sampleApply.StatusEnum, CancelHook, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Pass:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.Pass, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnablePass(operatorId, sampleApply.IsNeedConfirmAcceptSample, sampleApply.IsNeedTutorAudit, (AuditStatus)sampleApply.TutorAuditStatus, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Refuse:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.Refuse, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableRefuse(operatorId, sampleApply.IsNeedConfirmAcceptSample, sampleApply.IsNeedTutorAudit, (AuditStatus)sampleApply.TutorAuditStatus, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Accept:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.Accept, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableAccept(operatorId, sampleApply.IsNeedConfirmAcceptSample, sampleApply.IsNeedTutorAudit, (AuditStatus)sampleApply.TutorAuditStatus, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.RefuseAccept:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.RefuseAccept, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableRefuseAccept(operatorId, sampleApply.IsNeedConfirmAcceptSample, sampleApply.IsNeedTutorAudit, (AuditStatus)sampleApply.TutorAuditStatus, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Charge:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.Charge, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableCharge(operatorId, sampleApply.ChargeStatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Test:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.Test, out errorMessage)) return false;
                    break;
                case SampleOperate.InputFinishedQuatity:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.InputFinishedQuatity, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableInputFinishedQuatity(operatorId, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Register:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.Register, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableRegister(operatorId, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Release:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.Release, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableReleaseResult(operatorId, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.DownLoad:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, DownLoadHook, SampleOperate.DownLoad, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableDownLoad(operatorId, sampleApply.ReportObtainerId, sampleApply.ApplicantId, sampleApply.StatusEnum, DownLoadHook, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.Doubts:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, DoubtsHook, SampleOperate.Doubts, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableDoubts(operatorId, sampleApply.ApplicantId, sampleApply.StatusEnum, DoubtsHook, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.AuditDoublt:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.AuditDoublt, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableAuditDoubts(operatorId, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
                case SampleOperate.FeedBack:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.FeedBack, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableFeedBack(operatorId, sampleApply.IsNeedFeedBack, sampleApply.ApplicantId, sampleApply.StatusEnum, out errorMessage)) return false;
                    break;
                case SampleOperate.ReportConfirm:
                    if (!_viewSampleApplyBLL.JudgeIsEnableOperate(operatorId, new SampleApply[] { sampleApply }, false, SampleOperate.ReportConfirm, out errorMessage)) return false;
                    if (!_viewSampleApplyBLL.JudgeIsEnableReportConfirm(operatorId, sampleApply.StatusEnum, _samplePrivilige, out errorMessage)) return false;
                    break;
            }
            return true;
        }
        private bool JudgeIsEnableOperate(Guid operatorId, IEnumerable<Guid> ids, SampleOperate sampleOperate, object[] parameters, out string errorMessage)
        {
            errorMessage= "";
            if (ids == null || ids.Count() == 0)
            {
                errorMessage = "申请单信息为空";
                return false;
            }
            var sampleApplyList = _sampleApplyBLL.GetEntitiesByExpression(ids.ToFormatStr());
            return JudgeIsEnableOperate(operatorId,sampleApplyList, sampleOperate, parameters, out errorMessage);
        }
        private bool JudgeIsEnableOperate(Guid operatorId, IEnumerable<SampleApply> sampleApplyList, SampleOperate sampleOperate, object[] parameters, out string errorMessage)
        {
            errorMessage = "";
            if (sampleApplyList == null || sampleApplyList.Count() == 0)
            {
                errorMessage = "申请单信息为空";
                return false;
            }
            foreach (var sampleApply in sampleApplyList)
            {
                if (!JudgeIsEnableOperate(operatorId,sampleApply, sampleOperate, parameters, out errorMessage))
                {
                    return false;
                }
            }
            return true;
        }

        public Logic.Model.GridData<Model.View.ViewSampleApply> GetGridSampleApplyList(DataGridSettings dataGridSettings)
        {
            string errorMessage = "";
            var viewSampleApplies = _sampleManager.GetGridSampleApplyList(dataGridSettings);
            if (viewSampleApplies == null || viewSampleApplies.Count == 0 || viewSampleApplies.Data == null || viewSampleApplies.Data.Count==0) return null;
            IList<UserEquipmentPrivilige> lstSamplePriviliges = new List<UserEquipmentPrivilige>();
            foreach (var viewSampleApply in viewSampleApplies.Data)
            {
                viewSampleApply.InitOperation();
                SamplePrivilige samplePrivilige = new NoSamplePrivilige();
                var userEquipmentPrivilige = lstSamplePriviliges.Count > 0 ? lstSamplePriviliges.FirstOrDefault(p => p != null && p.SampleItemId != null && p.SampleItemId == viewSampleApply.SampleItemId) : null;
                if (userEquipmentPrivilige == null)
                {
                    userEquipmentPrivilige = _userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserItemId(_user.Id, viewSampleApply.SampleItemId);
                    lstSamplePriviliges.Add(userEquipmentPrivilige);
                }
                if (userEquipmentPrivilige != null)
                {
                    samplePrivilige = userEquipmentPrivilige.ToSamplePrivilige();
                }
                if (!_viewSampleApplyBLL.JudgeIsEnableEdit(_user.Id, viewSampleApply.ApplicantId, viewSampleApply.CreatorId.Value, viewSampleApply.SampleStatus, EditHook, samplePrivilige, out errorMessage))
                {
                    viewSampleApply.EditOperate = "";
                }
                if (!_viewSampleApplyBLL.JudgeIsEnableView(_user.Id,ViewHook, samplePrivilige, out errorMessage))
                {
                    viewSampleApply.ViewSampleApplyInfoOperate = "";
                }
                if (!_viewSampleApplyBLL.JudgeIsEnableApply(_user.Id, ApplyHook, samplePrivilige, out errorMessage))
                {
                    viewSampleApply.ApplyOperate = "";
                }
                
                if (!_viewSampleApplyBLL.JudgeIsEnableCharge(_user.Id, viewSampleApply.ChargeStatusEnum, samplePrivilige, out errorMessage))
                {
                    viewSampleApply.ChargeOperate = "";
                }
                if (!_viewSampleApplyBLL.JudgeIsEnableCancel(_user.Id, viewSampleApply.ApplicantId, viewSampleApply.CreatorId.Value, viewSampleApply.SampleStatus, CancelHook, samplePrivilige, out errorMessage))
                {
                    viewSampleApply.CancelOperate = "";
                }
                if (!_viewSampleApplyBLL.JudgeIsEnableCopy(_user.Id, viewSampleApply.ApplicantId,CopyHook, samplePrivilige, out errorMessage))
                {
                    viewSampleApply.CopyOperate = "";
                }

                if (!_viewSampleApplyBLL.JudgeIsEnableAudit(_user.Id, viewSampleApply.IsNeedConfirmAcceptSample, viewSampleApply.IsNeedTutorAudit, (AuditStatus)viewSampleApply.TutorAuditStatus, viewSampleApply.SampleStatus, samplePrivilige, out errorMessage))
                {
                    viewSampleApply.AuditOperate = "";
                }
                if (!_viewSampleApplyBLL.JudgeIsEnableRefuse(_user.Id, viewSampleApply.IsNeedConfirmAcceptSample, viewSampleApply.IsNeedTutorAudit, (AuditStatus)viewSampleApply.TutorAuditStatus, viewSampleApply.SampleStatus, samplePrivilige, out errorMessage))
                {
                    viewSampleApply.RefuseOperate = "";
                }
               
                
                if (viewSampleApply.IsSecret && _user.Id != viewSampleApply.ApplicantId)
                {
                    if (userEquipmentPrivilige == null || (userEquipmentPrivilige != null && !userEquipmentPrivilige.IsEnableLookSecretInfo))
                    {
                        viewSampleApply.OrganizationName = "***";
                        viewSampleApply.TelephoneNo = "***";
                        viewSampleApply.Email = "***";
                        viewSampleApply.SubjectName = "***";
                        viewSampleApply.ApplicantName = "***";
                    }
                }
                DoPriviligeFacade(viewSampleApply);
                if (viewSampleApply.IsNeedFeedBack)
                {
                    viewSampleApply.SampleNo += viewSampleApply.FeedbackTime.HasValue ? "(已反馈)" : viewSampleApply.SampleStatus == Model.Enum.SampleApplyStatus.Obtained ? "(未反馈)" : "";
                }
                if (viewSampleApply.SampleStatus == Model.Enum.SampleApplyStatus.Canceled)
                {
                    viewSampleApply.PringQrCodeOperate = "";
                }
                viewSampleApply.SampleNoStr = string.Format("<span class=\"{0}\">{1}<span>", viewSampleApply.IsUrgent ? "color-span red-span" : "", viewSampleApply.SampleNo);
                string operateStr = BuildOperate(viewSampleApply);
                if (operateStr != "") viewSampleApply.SampleNoStr = viewSampleApply.SampleNoStr + "<div class='button-box hide'>" + operateStr + "</div>"; ;
            }
            return viewSampleApplies;
        }

        public bool TutorAudit(IEnumerable<Guid> sampleApplyIds, Guid auditTutorId, AuditStatus auditStatus, string auditRemark, out string errorMessage)
        {
            var sampleApplies = _sampleApplyBLL.GetEntitiesByExpression(sampleApplyIds.ToFormatStr());
            if (!JudgeIsEnableOperate(_user.Id, sampleApplies, SampleOperate.TutorAudit, new object[] { auditTutorId }, out errorMessage)) return false;
            return _sampleManager.TutorAudit(sampleApplies, auditTutorId, auditStatus, auditRemark, out errorMessage);
        }

        public bool Apply(IList<SampleApply> models, User user, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, models, SampleOperate.Apply, null, out errorMessage)) return false;
            return _sampleManager.Apply(models, user, out errorMessage);
        }

        public bool Edit(ref List<SampleApply> models, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, models, SampleOperate.Edit, null, out errorMessage)) return false;
            return _sampleManager.Edit(ref models, out errorMessage);
        }

        public bool Cancel(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, ids, SampleOperate.Cancel, null, out errorMessage)) return false;
            return _sampleManager.Cancel(ids, operatorId, remark, out errorMessage); 
        }

        public bool Pass(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            var orginalSampleApply = _sampleApplyBLL.GetEntityById(sampleApply.Id);
            if (!JudgeIsEnableOperate(_user.Id, orginalSampleApply, SampleOperate.Pass, null, out errorMessage)) return false;
            return _sampleManager.Pass(sampleApply, operatorId, remark, out errorMessage);
        }

        public bool Pass(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, ids, SampleOperate.Pass, null, out errorMessage)) return false;
            return _sampleManager.Pass(ids, operatorId, remark, out errorMessage);
        }

        public bool Refuse(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, ids, SampleOperate.Refuse, null, out errorMessage)) return false;
            return _sampleManager.Refuse(ids, operatorId, remark, out errorMessage);
        }

        public bool Refuse(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            var orginalSampleApply = _sampleApplyBLL.GetEntityById(sampleApply.Id);
            if (!JudgeIsEnableOperate(_user.Id, orginalSampleApply, SampleOperate.Refuse, null, out errorMessage)) return false;
            return _sampleManager.Refuse(sampleApply, operatorId, remark, out errorMessage);
        }

        public bool Accept(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, ids, SampleOperate.Accept, null, out errorMessage)) return false;
            return _sampleManager.Accept(ids, operatorId, remark, out errorMessage);
        }

        public bool Accept(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            var orginalSampleApply = _sampleApplyBLL.GetEntityById(sampleApply.Id);
            if (!JudgeIsEnableOperate(_user.Id, orginalSampleApply, SampleOperate.Accept, null, out errorMessage)) return false;
            return _sampleManager.Accept(sampleApply, operatorId, remark, out errorMessage); 
        }

        public bool RefuseAccept(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, ids, SampleOperate.RefuseAccept, null, out errorMessage)) return false;
            return _sampleManager.RefuseAccept(ids, operatorId, remark, out errorMessage);
        }

        public bool RefuseAccept(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            var orginalSampleApply = _sampleApplyBLL.GetEntityById(sampleApply.Id);
            if (!JudgeIsEnableOperate(_user.Id, orginalSampleApply, SampleOperate.RefuseAccept, null, out errorMessage)) return false;
            return _sampleManager.RefuseAccept(sampleApply, operatorId, remark, out errorMessage);
        }

        public bool Charge(SampleApply sampleApply,bool isEidtSampleApplyChargeItem, Guid operatorId, out string errorMessage, ref XTransaction tran)
        {
            if (!JudgeIsEnableOperate(_user.Id, sampleApply, SampleOperate.Charge, null, out errorMessage)) return false;
            return _sampleManager.Charge(sampleApply, operatorId, isEidtSampleApplyChargeItem, false, out errorMessage, ref tran);
        }

        public bool Test(Guid sampleApplyId, Guid operaterId, string remark, out string errorMessage, SampleTestOption testOption)
        {
            if (!JudgeIsEnableOperate(_user.Id, new Guid[] { sampleApplyId }, SampleOperate.Test, null, out errorMessage)) return false;
            return _sampleManager.Test(sampleApplyId, operaterId, remark, out errorMessage, testOption);
        }

        public bool InputFinishedQuatity(SampleApply sampleApply, int realQuatity, double? duration, Guid operatorId, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, sampleApply, SampleOperate.InputFinishedQuatity, null, out errorMessage)) return false;
            return _sampleManager.InputFinishedQuatity(sampleApply, realQuatity, duration, operatorId, out errorMessage);
        }

        public bool Register(SampleApply sampleApply, Guid operatorId, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, sampleApply, SampleOperate.Register, null, out errorMessage)) return false;
            return _sampleManager.Register(sampleApply, operatorId, out errorMessage);
        }

        public bool Release(Guid sampleApplyId, Guid reportObtainerId, Guid operateId, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, new Guid[] { sampleApplyId }, SampleOperate.Release, null, out errorMessage)) return false;
            return _sampleManager.Release(sampleApplyId, reportObtainerId, operateId, remark, out errorMessage);
        }

        public bool DownLoad(Guid sampleApplyId, Guid operateId, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, new Guid[] { sampleApplyId }, SampleOperate.DownLoad, null, out errorMessage)) return false;
            return _sampleManager.DownLoad(sampleApplyId, operateId, out errorMessage);
        }

        public bool Doubts(Guid sampleApplyId, Guid operateId, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, new Guid[] { sampleApplyId }, SampleOperate.Doubts, null, out errorMessage)) return false;
            return _sampleManager.Doubts(sampleApplyId, operateId, remark, out errorMessage);
        }

        public bool AuditDoublt(Guid sampleApplyId, Guid operateId, SampleApplyStatus sampleStatus, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, new Guid[] { sampleApplyId }, SampleOperate.AuditDoublt, null, out errorMessage)) return false;
            return _sampleManager.AuditDoublt(sampleApplyId, operateId, sampleStatus, remark, out errorMessage);
        }
        
        public bool FeedBack(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, sampleApply, SampleOperate.FeedBack, null, out errorMessage)) return false;
            if (sampleApply.SampleFeedBackAttachments == null || sampleApply.SampleFeedBackAttachments.Count == 0)
            {
                errorMessage = "附件为空";
                return false;
            }
            return _sampleManager.FeedBack(sampleApply, operatorId, remark, out errorMessage);
           
        }


        public bool ReportConfirm(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            var orginalSampleApply = _sampleApplyBLL.GetEntityById(sampleApply.Id);
            if (!JudgeIsEnableOperate(_user.Id, orginalSampleApply, SampleOperate.ReportConfirm, null, out errorMessage)) return false;
            return _sampleManager.ReportConfirm(sampleApply, operatorId, remark, out errorMessage);
        }

        public bool ReportConfirm(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableOperate(_user.Id, ids, SampleOperate.ReportConfirm, null, out errorMessage)) return false;
            return _sampleManager.ReportConfirm(ids, operatorId, remark, out errorMessage);
        }
    
    }
}
