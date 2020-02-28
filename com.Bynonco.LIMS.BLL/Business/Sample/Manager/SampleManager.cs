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
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;

namespace com.Bynonco.LIMS.BLL
{
    public abstract partial class SampleManager
    {
        protected IUserBLL _userBLL;
        protected ISampleApplyBLL _sampleApplyBLL;
        protected IViewSampleApplyBLL _viewSampleApplyBLL;
        protected ISampleApplyChargeItemBLL _sampleApplyChargeItemBLL;
        protected IDictCodeTypeBLL _dictCodeTypeBLL;
        protected ISubjectBLL _subjectBLL;
        protected IUserAccountBLL _userAccountBLL;
        protected ISampleTesterBLL _sampleTesterBLL;
        protected IReceiverBLL _receiverBLL;
        protected IMessageBLL _messageBLL;
        protected ISendMailBLL _sendMailBLL;
        protected ISampleTestRecordBLL _sampleTestRecordBLL;
        protected ISampleItemResultBLL _sampleItemResultBLL;
        protected ISampleAnalysisAttachmentBLL _sampleAnalysisAttachmentBLL;
        protected ICostDeductBLL _costDeductBLL;
        protected IUserEquipmentPriviligeBLL _userEquipmentSamplePriviligeBLL;
        protected ISampleApplyAttachmentBLL _sampleApplyAttachmentBLL;
        protected ISampleApplyNumberBLL _sampleApplyNumberBLL;
        protected IMessageHandler _messageHandler;
        protected ISampleChargeItemBLL _sampleChargeItemBLL;
        private ISampleApplyParameterBLL _sampleApplyParameterBLL;
        protected IMoneyValidateBLL _appointmentMoneyValidateBLL;
        protected IDepositRecordBLL _depositRecordBLL;
        protected ISampleItemBLL _sampleItemBLL;
        protected ISampleFeedBackAttachmentBLL _sampleFeedBackAttachmentBLL;
        protected IViewOpenFundDetailBLL _viewOpenFundDetailBLL;
        protected IOpenFundApplyBLL _openFundApplyBLL;
        protected User _user;
        protected Dictionary<string, string> _MAPPING = new Dictionary<string, string>();
        protected string[] _OUTMAPPINGS = null;
        public SampleManager(Guid userId)
        {
            _dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            _sampleApplyBLL = BLLFactory.CreateSampleApplyBLL();
            _viewSampleApplyBLL = BLLFactory.CreateViewSampleApplyBLL();
            _sampleApplyChargeItemBLL = BLLFactory.CreateSampleApplyChargeItemBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            _subjectBLL = BLLFactory.CreateSubjectBLL();
            _userAccountBLL = BLLFactory.CreateUserAccountBLL();
            _sampleTesterBLL = BLLFactory.CreateSampleTesterBLL();
            _messageBLL = BLLFactory.CreateMessageBLL();
            _receiverBLL = BLLFactory.CreateReceiverBLL();
            _sendMailBLL = BLLFactory.CreateSendMailBLL();
            _sampleTestRecordBLL = BLLFactory.CreateSampleTestRecordBLL();
            _sampleItemResultBLL = BLLFactory.CreateSampleItemResultBLL();
            _sampleAnalysisAttachmentBLL = BLLFactory.CreateSampleAnalysisAttachmentBLL();
            _costDeductBLL = BLLFactory.CreateCostDeductBLL();
            _userEquipmentSamplePriviligeBLL = BLLFactory.CreateUserEquipmentPriviligeBLL();
            _sampleApplyAttachmentBLL = BLLFactory.CreateSampleApplyAttachmentBLL();
            _sampleApplyNumberBLL = BLLFactory.CreateSampleApplyNumberBLL();
            _sampleChargeItemBLL = BLLFactory.CreateSampleChargeItemBLL();
            _messageHandler = new MessageHandler();
            _appointmentMoneyValidateBLL = BLLFactory.CreateMoneyValidateBLL();
            _sampleApplyParameterBLL = BLLFactory.CreateSampleApplyParameterBLL();
            _depositRecordBLL = BLLFactory.CreateDepositRecordBLL();
            _sampleItemBLL = BLLFactory.CreateSampleItemBLL();
            _sampleFeedBackAttachmentBLL = BLLFactory.CreateSampleFeedBackAttachmentBLL();
            _viewOpenFundDetailBLL = BLLFactory.CreateViewOpenFundDetailBLL();
            _openFundApplyBLL = BLLFactory.CreateOpenFundApplyBLL();
            _user = _userBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", userId.ToString())).First();
            _OUTMAPPINGS = _viewSampleApplyBLL.GetViewSampleApplyOutMappings();
            _MAPPING["Id"] = "SampleApplyId";
        }

        public abstract Logic.Model.GridData<Model.View.ViewSampleApply> GetGridSampleApplyList(DataGridSettings dataGridSettings);

        private void GenerateSampleApplyOpenFundCostDeduct(SampleApply sampleApply, IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, IList<CostDeductEquipmentOpenFund> newCostDeductEquipmentOpenFunds)
        {
            if (sampleApply.CostDeduct != null)
            {
                if (costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Count > 0)
                {
                    if (sampleApply.CostDeduct.CostDeductEquipmentOpenFunds == null)
                    {
                        sampleApply.CostDeduct.CostDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
                    }
                    foreach (var costDeductEquipmentOpenFund in costDeductEquipmentOpenFunds)
                    {
                        sampleApply.CostDeduct.CostDeductEquipmentOpenFunds.Add(costDeductEquipmentOpenFund);
                    }
                }
                if (sampleApply.CostDeduct.CostDeductEquipmentOpenFunds != null && sampleApply.CostDeduct.CostDeductEquipmentOpenFunds.Count > 0)
                {
                    if (newCostDeductEquipmentOpenFunds != null)
                    {
                        foreach (var costDeductEquipmentOpenFund in sampleApply.CostDeduct.CostDeductEquipmentOpenFunds)
                        {
                            newCostDeductEquipmentOpenFunds.Add(costDeductEquipmentOpenFund);
                        }
                    }
                }
            }
        }

        public bool TutorAudit(IEnumerable<SampleApply> sampleApplies, Guid auditTutorId, AuditStatus auditStatus, string auditRemark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                foreach (var sampleApply in sampleApplies)
                {
                    sampleApply.AuditTutorId = auditTutorId;
                    sampleApply.TutorAuditTime = DateTime.Now;
                    sampleApply.TutorAuditRemark = auditRemark;
                    sampleApply.TutorAuditStatus = (int)auditStatus;
                }
                _sampleApplyBLL.Save(sampleApplies, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = "出错" + ex.Message;
                return false;
            }
            return true;
        }

        public bool Apply(IList<SampleApply> models, User user, out string errorMessage)
        {

            models = models.OrderByDescending(p => p.Price).ToList();
            UserAccount userAccount = null;
            errorMessage = "";
            IList<SampleApply> lstSampleApplyForValidate = new List<SampleApply>();
            IList<CostDeductEquipmentOpenFund> newCostDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (user.Id == default(Guid))
                {
                    var subjectId = RegisterNewUser(user, ref tran, out userAccount);
                    foreach (var model in models)
                    {
                        model.ApplicantId = user.Id;
                        model.SubjectId = subjectId;
                        model.PayerId = user.Id;
                    }
                }
                else
                {
                    _userBLL.Save(new User[] { user }, ref tran, true);
                }

                bool isCostDeduct = JudgeIsCostDeduct(models[0]);
                var viewOpenFundDetailList = models.First().Subject != null ? _viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(models.Select(p => p.SampleItem.EquipmentId), models.First().Subject.SubjectDirectorId.Value, models.Max(p => p.ExpectSendDate.Value)) : null;
                foreach (var model in models)
                {
                    bool isOpenFund = false;
                    double? openFundDiscount = null;
                    double realCurrency = 0d, virtualCurrency = 0d, openfundFee = 0d;
                    bool isSaveOpenFund = model.Id == models.Last().Id;
                    var calPrice = model.Price;
                    if (isCostDeduct)
                    {
                        IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
                        model.CostDeduct = new Model.CostDeduct() { CalcDuration = 0, CostDeductType = (int)CostDeductType.SampleCostDeduct, CreaterId = _user.Id, DeductAt = DateTime.Now, Id = Guid.NewGuid(), Duration = 0, SampleApplyId = model.Id };
                        _sampleApplyBLL.GetSampleApplyPrice(viewOpenFundDetailList, model, user.TagId, null, false, isCostDeduct, CustomerFactory.GetCustomer().GetIsOpenFundCostDeduct(), out isOpenFund, out costDeductEquipmentOpenFunds);
                        GenerateSampleApplyOpenFundCostDeduct(model, costDeductEquipmentOpenFunds, newCostDeductEquipmentOpenFunds);
                        calPrice = model.Price;
                        if (userAccount == null)
                        {
                            userAccount = _userAccountBLL.Deduct(isOpenFund, model.ApplicantId, model.SubjectId, model.PaymentMethodEnum, model.Price.Value, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false);
                        }
                        else
                        {
                            userAccount = userAccount.Deduct(isOpenFund, model.Price.Value, out realCurrency, out virtualCurrency);
                        }
                        model.RealCurrency = realCurrency;
                        model.VirtualCurrency = virtualCurrency;
                        model.ChargeStatusEnum = SampleChargeStatus.Charged;
                        model.CostDeduct.VirtualCurrency = virtualCurrency;
                        model.CostDeduct.RealCurrency = realCurrency;
                        model.CostDeduct.UserAccountId = userAccount.Id;

                    }
                    else
                    {
                        if (model.Applicant != null)
                        {
                            calPrice = _sampleApplyBLL.GetSampleApplyPrice(viewOpenFundDetailList, CustomerFactory.GetCustomer().GetIsOpenFundCostDeduct(), model.SubjectId, model.ApplicantId, model.Applicant.TagId, model.SampleItemId, model.Quatity, null, out isOpenFund, out openfundFee, out openFundDiscount);
                        }
                    }
                    lstSampleApplyForValidate.Add(new SampleApply()
                    {
                        Id = model.Id,
                        EditId = model.EditId,
                        SampleItemId = model.SampleItemId,
                        ApplicantId = model.ApplicantId,
                        SubjectId = model.SubjectId,
                        PaymentMethod = model.PaymentMethod,
                        ChargeStatus = model.ChargeStatus,
                        Status = model.Status,
                        Price = calPrice
                    });
                    model.IsCalPriceUsingOpenFund = isOpenFund;
                    model.OpenFundDiscount = openFundDiscount;
                    model.OpenFundCurrency = openfundFee;
                    _sampleApplyBLL.Add(new SampleApply[] { model }, ref tran, true);
                    if (model.CostDeduct != null)
                    {
                        model.CostDeduct.SampleApplyForLog = model;
                        _costDeductBLL.Add(new CostDeduct[] { model.CostDeduct }, ref tran, true);
                    }
                    if (model.SampleApplyChargeItems != null && model.SampleApplyChargeItems.Count > 0)
                    {
                        _sampleApplyChargeItemBLL.Add(model.SampleApplyChargeItems.ToArray(), ref tran, true);
                    }

                    if (model.SampleApplyAttachments != null && model.SampleApplyAttachments.Count > 0)
                    {
                        foreach (var sampleApplyAttachment in model.SampleApplyAttachments)
                        {
                            sampleApplyAttachment.CreatorId = model.ApplicantId;
                        }
                        _sampleApplyAttachmentBLL.Add(model.SampleApplyAttachments.ToArray(), ref tran, true);
                    }
                    if (model.SampleApplyNumbers != null && model.SampleApplyNumbers.Count > 0)
                    {
                        _sampleApplyNumberBLL.Add(model.SampleApplyNumbers.ToArray(), ref tran, true);
                    }
                    if (model.SampleTesters != null && model.SampleTesters.Count > 0)
                    {
                        foreach (var sampleTester in model.SampleTesters) sampleTester.SampleApplyId = model.Id;
                        _sampleTesterBLL.Add(model.SampleTesters.ToArray(), ref tran, true);
                    }
                    if (model.SampleApplyParameters != null && model.SampleApplyParameters.Count > 0)
                    {
                        _sampleApplyParameterBLL.Add(model.SampleApplyParameters, ref tran, true);
                    }
                }
                if (isCostDeduct)
                {
                    _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, true);
                    _openFundApplyBLL.SaveSampleAppyOpenFund(viewOpenFundDetailList, null, newCostDeductEquipmentOpenFunds, ref tran);
                }
                if (!ValidateUserCreditLimit(lstSampleApplyForValidate, false, out errorMessage, user)) throw new Exception(errorMessage);
                DoCreateOuterDeposit(models, userAccount, ref tran);
                tran.CommitTransaction();
                foreach (var model in models)
                {
                    if (model.IsNeedTutorAudit) _messageHandler.SendSampleApplyTutorAuditNotice(model, null);
                    if (model.CostDeduct != null) _messageHandler.SendSampleApplyCostDeductNotice(model, null);
                }
                SendMessage(models, null, null, null, out errorMessage);
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错" + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }

        public bool Copy(IList<SampleApply> models, User user, out string errorMessage)
        {
            if (!ValidateUserCreditLimit(models, false, out errorMessage)) throw new Exception(errorMessage);
            return Apply(models, user, out errorMessage);
        }

        private void HandleCostDeductOpenFundForEdit(CostDeduct costDeduct, UserAccount userAccount, UserAccount cancelUserAccount, IList<Guid> deletedIds, IList<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFunds, ref XTransaction tran)
        {
            if (costDeduct != null && !deletedIds.Contains(costDeduct.Id))
            {
                bool isDeleteCostDeduct = false;
                if (userAccount != null && cancelUserAccount != null && cancelUserAccount.Id != userAccount.Id)
                {
                    _costDeductBLL.Delete(new Guid[] { costDeduct.Id }, ref tran, _user.Id, "", true, true);
                    isDeleteCostDeduct = true;
                }
                if (!isDeleteCostDeduct)
                {
                    _costDeductBLL.Delete(new Guid[] { costDeduct.Id }, ref tran, _user.Id, "", true, false);
                    if (costDeduct.CostDeductEquipmentOpenFunds != null && costDeduct.CostDeductEquipmentOpenFunds.Count > 0)
                    {
                        foreach (var costDeductEquipmentOpenFund in costDeduct.CostDeductEquipmentOpenFunds)
                        {
                            if (!preCostDeductEquipmentOpenFunds.Any(p => p.Id == costDeductEquipmentOpenFund.Id))
                            {
                                preCostDeductEquipmentOpenFunds.Add(costDeductEquipmentOpenFund);
                            }
                        }
                    }
                }
                deletedIds.Add(costDeduct.Id);
            }
        }

        private bool Edit(ref List<SampleApply> models, bool isEditSampleApplyChargeItem, out string errorMessage)
        {

            models = models.OrderByDescending(p => p.Price).ToList();
            errorMessage = "";
            IList<SampleApply> lstSampleApplyForValidate = new List<SampleApply>();
            List<SampleApplyChargeItem> lstSampleApplyChargeItems = new List<SampleApplyChargeItem>();
            IList<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
            IList<CostDeductEquipmentOpenFund> newCostDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
            IList<Guid> deletedIds = new List<Guid>();
            CostDeduct costDeduct = null;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var editId = models[0].EditId.Value;
                var editSampleApply = _sampleApplyBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"", editId.ToString()));
                List<Guid> equipmentIds = new List<Guid>() { editSampleApply.SampleItem.EquipmentId };
                equipmentIds.AddRange(models.Select(p => p.SampleItem.EquipmentId));
                //处理编辑撤销的情况
                if (!JudgeIsEnableOperate(editSampleApply.Status))
                {
                    var sampleApplyTemp = RejectChange(editSampleApply.Id, models[0].Status, out errorMessage);
                    if (sampleApplyTemp == null) throw new Exception(errorMessage);
                    models = new List<SampleApply> { sampleApplyTemp };
                }
                var defaultSampleChargeItem = _sampleChargeItemBLL.GetDefaultSampleChargeItem();
                if (defaultSampleChargeItem == null) throw new Exception("默认收费项目为空");
                double cancelDeductRealCurrency = 0d, cancelVirtualCurrency = 0d, realCurrency = 0d, virtualCurrency = 0d;
                UserAccount cancelUserAccount = null, userAccount = null;
                var isHasNewPayerId = models.Any(p => p.Subject.SubjectDirectorId.Value != editSampleApply.Subject.SubjectDirectorId.Value);
                bool isCostDeduct = JudgeIsCostDeduct(models[0]);
                bool isCancelCostDeduct = JudgeIsCancelCostDeduct(models[0]);
                if ((isCostDeduct || isCancelCostDeduct))
                {
                    //扣费0元获取到取消扣费账户，因为当前是编辑状态，扣费需要做两个步骤，第一先把原来扣掉的费用加回用户账户，然后再把当前实际费用从账户扣费，考虑到申请单的课题信息和支持方式信息可能发生修改，所以以下处理取消扣费和扣费可能是不同账户的情况
                    cancelUserAccount = _userAccountBLL.Deduct(false, editSampleApply.ApplicantId, editSampleApply.SubjectId, editSampleApply.PaymentMethodEnum, 0, out cancelDeductRealCurrency, out cancelVirtualCurrency, ref tran, out errorMessage, false);
                    cancelUserAccount.RealCurrency += editSampleApply.RealCurrency;
                    cancelUserAccount.VirtualCurrency += editSampleApply.VirtualCurrency;
                    //扣费0元获取到扣费账户
                    userAccount = isHasNewPayerId ? _userAccountBLL.Deduct(false, models[0].ApplicantId, models[0].SubjectId, models[0].PaymentMethodEnum, 0, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false) : cancelUserAccount;
                    if (cancelUserAccount.Id != userAccount.Id)
                    {
                        equipmentIds.Remove(editSampleApply.SampleItem.EquipmentId);
                        _userAccountBLL.Save(new UserAccount[] { cancelUserAccount }, ref tran, true);
                    }
                }
                var viewOpenFundDetailList = _viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(equipmentIds, models.First().Subject.SubjectDirectorId.Value, models.Max(p => p.ExpectSendDate.Value));
                var sampleApplies = _sampleApplyBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", editId.ToString()));
                if (sampleApplies != null && sampleApplies.Count > 0)
                {
                    var defaultSampleApplyChargeTemp = _sampleApplyChargeItemBLL.GetFirstOrDefaultEntityByExpression(string.Format("SampleApplyId=\"{0}\"*SampleChargeItemId=\"{1}\"", editId.ToString(), defaultSampleChargeItem.Id));
                    if (defaultSampleApplyChargeTemp != null)
                    {
                        _sampleApplyChargeItemBLL.Delete(new Guid[] { defaultSampleApplyChargeTemp.Id }, ref tran, true);
                    }
                    var sampleTestersTemp = _sampleTesterBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", editId.ToString()));
                    if (sampleTestersTemp != null && sampleTestersTemp.Count > 0)
                    {
                        _sampleTesterBLL.Delete(sampleTestersTemp.Select(p => p.Id).ToArray(), ref tran, true);
                    }
                    costDeduct = _costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("SampleApplyId=\"{0}\"", editId.ToString()));
                    HandleCostDeductOpenFundForEdit(costDeduct, userAccount, cancelUserAccount, deletedIds, preCostDeductEquipmentOpenFunds, ref tran);
                    var sampleApplyNumbers = _sampleApplyNumberBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", editId.ToString()));
                    if (sampleApplyNumbers != null && sampleApplyNumbers.Count > 0)
                    {
                        _sampleApplyNumberBLL.Delete(sampleApplyNumbers.Select(p => p.Id).ToArray(), ref tran, true);
                    }
                    var sampleApplyAttachments = _sampleApplyAttachmentBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", editId.ToString()));
                    if (sampleApplyAttachments != null && sampleApplyAttachments.Count > 0)
                    {
                        _sampleApplyAttachmentBLL.Delete(sampleApplyAttachments.Select(p => p.Id).ToArray(), ref tran, true);
                    }
                    var sampleApplyParameters = _sampleApplyParameterBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", editId.ToString()));
                    if (sampleApplyParameters != null && sampleApplyParameters.Count > 0)
                    {
                        _sampleApplyParameterBLL.Delete(sampleApplyParameters.Select(p => p.Id).ToArray(), ref tran, true);
                    }
                    _sampleApplyBLL.Delete(new Guid[] { editId }, ref tran, true);
                }
                var index = 0;
                foreach (var model in models)
                {
                    bool isSaveOpenFund = model.Id == models.Last().Id, isOpenFund = false;
                    var sampleApplyForValidate = new SampleApply()
                    {
                        Id = model.Id,
                        EditId = model.EditId,
                        SampleItemId = model.SampleItemId,
                        ApplicantId = model.ApplicantId,
                        SubjectId = model.SubjectId,
                        PaymentMethod = model.PaymentMethod,
                        ChargeStatus = model.ChargeStatus,
                        Status = model.Status,
                        Price = model.Price,
                        RealCurrency = model.RealCurrency,
                        VirtualCurrency = model.VirtualCurrency
                    };
                    costDeduct = _costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("SampleApplyId=\"{0}\"", model.Id.ToString()));
                    HandleCostDeductOpenFundForEdit(costDeduct, userAccount, cancelUserAccount, deletedIds, preCostDeductEquipmentOpenFunds, ref tran);
                    model.CostDeduct = new Model.CostDeduct() { CalcDuration = 0, CostDeductType = (int)CostDeductType.SampleCostDeduct, CreaterId = _user.Id, DeductAt = DateTime.Now, Id = Guid.NewGuid(), Duration = 0 };
                    sampleApplies = _sampleApplyBLL.GetEntitiesByExpression(string.Format("Id=-\"{0}\"*SampleItemId=\"{1}\"*RelateId=\"{2}\"", model.Id.ToString(), model.SampleItemId.ToString(), model.RelateId.ToString()));
                    if (sampleApplies != null && sampleApplies.Count > 0) throw new Exception(string.Format("出错,委托单【{0}】已经包含项目【{1}】,不能重复添加", model.SampleNo, model.SampleItem.Name));
                    if (isCancelCostDeduct && model.Id == editSampleApply.Id)
                    {
                        model.VirtualCurrency = 0d;
                        model.RealCurrency = 0d;
                        model.ChargeStatusEnum = SampleChargeStatus.CancelCharge;
                        model.CostDeduct.VirtualCurrency = 0d;
                        model.CostDeduct.RealCurrency = 0d;
                    }
                    var otherSampleApplyChargeItems = _sampleApplyChargeItemBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"*SampleChargeItemId=-\"{1}\"", model.Id, defaultSampleChargeItem.Id));
                    model.Id = Guid.NewGuid();
                    //_sampleApplyBLL.Add(new SampleApply[] { model }, ref tran, true);
                    if (isCostDeduct)
                    {
                        if (!isCancelCostDeduct)
                        {
                            index++;
                            IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
                            _sampleApplyBLL.GetSampleApplyPrice(viewOpenFundDetailList, model, index == 1 ? preCostDeductEquipmentOpenFunds : null, isEditSampleApplyChargeItem, isCostDeduct, CustomerFactory.GetCustomer().GetIsOpenFundCostDeduct(), out isOpenFund, out costDeductEquipmentOpenFunds);
                            GenerateSampleApplyOpenFundCostDeduct(model, costDeductEquipmentOpenFunds, newCostDeductEquipmentOpenFunds);
                            if (userAccount != null)
                            {
                                userAccount = userAccount.Deduct(isOpenFund, model.Price.Value, out realCurrency, out virtualCurrency);
                            }
                            model.RealCurrency = realCurrency;
                            model.VirtualCurrency = virtualCurrency;
                            model.ChargeStatusEnum = SampleChargeStatus.Charged;
                            model.CostDeduct.VirtualCurrency = virtualCurrency;
                            model.CostDeduct.RealCurrency = realCurrency;
                        }
                        sampleApplyForValidate.Price = model.Price;
                    }
                    else
                    {
                        double? openFundDiscount = null;
                        double openfundFee = 0d;
                        var calPrice = _sampleApplyBLL.GetSampleApplyPrice(viewOpenFundDetailList, CustomerFactory.GetCustomer().GetIsOpenFundCostDeduct(), model.SubjectId, model.ApplicantId, model.Applicant.TagId, model.SampleItemId, model.Quatity, null, out isOpenFund, out openfundFee, out openFundDiscount);
                        model.IsCalPriceUsingOpenFund = isOpenFund;
                        model.OpenFundDiscount = openFundDiscount;
                        model.OpenFundCurrency = openfundFee;
                        sampleApplyForValidate.Price = calPrice;
                    }
                    _sampleApplyBLL.Add(new SampleApply[] { model }, ref tran, true);
                    lstSampleApplyForValidate.Add(sampleApplyForValidate);
                    var otherSampleApplyChargeItemTotalFee = 0d;
                    if (otherSampleApplyChargeItems != null && otherSampleApplyChargeItems.Count > 0)
                    {
                        IList<SampleApplyChargeItem> otherSampleApplyChargeItemsTemp = new List<SampleApplyChargeItem>();
                        foreach (var otherSampleApplyChargeItem in otherSampleApplyChargeItems)
                        {
                            if (lstSampleApplyChargeItems.Any(p => p.Id == otherSampleApplyChargeItem.Id)) continue;
                            if (isCancelCostDeduct)
                            {
                                otherSampleApplyChargeItem.Price = 0d;
                                otherSampleApplyChargeItem.OpenFundPrice = 0d;
                            }
                            otherSampleApplyChargeItem.SampleApplyId = model.Id;
                            var otherSampleApplyChargeItemFind = model.SampleApplyChargeItems != null && model.SampleApplyChargeItems.Count > 0 ? model.SampleApplyChargeItems.FirstOrDefault(p => p.SampleChargeItemId == otherSampleApplyChargeItem.SampleChargeItemId) : null;
                            if (otherSampleApplyChargeItemFind != null)
                            {
                                otherSampleApplyChargeItem.OpenFundPrice = otherSampleApplyChargeItemFind.OpenFundPrice;
                                otherSampleApplyChargeItem.OpenFundDiscount = otherSampleApplyChargeItemFind.OpenFundDiscount;
                                otherSampleApplyChargeItem.Price = otherSampleApplyChargeItemFind.Price;
                                otherSampleApplyChargeItem.Quantity = otherSampleApplyChargeItemFind.Quantity;
                            }
                            otherSampleApplyChargeItemsTemp.Add(otherSampleApplyChargeItem);
                        }
                        if (otherSampleApplyChargeItemsTemp.Count > 0)
                        {
                            foreach (var otherSampleApplyChargeItem in otherSampleApplyChargeItemsTemp)
                            {
                                _sampleApplyChargeItemBLL.Delete(new Guid[] { otherSampleApplyChargeItem.Id }, ref tran, true);
                                _sampleApplyChargeItemBLL.Add(new SampleApplyChargeItem[] { new SampleApplyChargeItem() { Id = Guid.NewGuid(), ChargeOperatorId = otherSampleApplyChargeItem.ChargeOperatorId, Price = otherSampleApplyChargeItem.Price, ChargeTime = otherSampleApplyChargeItem.ChargeTime, Other = otherSampleApplyChargeItem.Other, Quantity = otherSampleApplyChargeItem.Quantity, Remark = otherSampleApplyChargeItem.Remark, SampleApplyId = model.Id, SampleChargeItemId = otherSampleApplyChargeItem.SampleChargeItemId, IsOpenFundCostDeduct = otherSampleApplyChargeItem.IsOpenFundCostDeduct, OpenFundDiscount = otherSampleApplyChargeItem.OpenFundDiscount, OpenFundPrice = otherSampleApplyChargeItem.OpenFundPrice } }, ref tran, true);
                                otherSampleApplyChargeItemTotalFee += otherSampleApplyChargeItem.Price;
                            }
                        }
                        lstSampleApplyChargeItems.AddRange(otherSampleApplyChargeItems);
                    }
                    if (defaultSampleChargeItem != null)
                    {
                        SampleApplyChargeItem defaultSampleApplyChargeItem = model.SampleApplyChargeItems != null ? model.SampleApplyChargeItems.FirstOrDefault(p => p.SampleChargeItemId == defaultSampleChargeItem.Id) : null;
                        if (defaultSampleApplyChargeItem == null)
                        {
                            defaultSampleApplyChargeItem = new SampleApplyChargeItem() { Id = Guid.NewGuid(), Price = model.Price.Value - otherSampleApplyChargeItemTotalFee, Quantity = model.Quatity, SampleApplyId = model.Id, SampleChargeItemId = defaultSampleChargeItem.Id, IsOpenFundCostDeduct = model.CostDeduct != null ? model.CostDeduct.IsOpenFundCostDeduct : false, OpenFundDiscount = model.CostDeduct != null && model.CostDeduct.OpenFundDiscount.HasValue ? model.CostDeduct.OpenFundDiscount.Value : 1d };
                        }
                        defaultSampleApplyChargeItem.SampleApplyId = model.Id;
                        defaultSampleApplyChargeItem.Id = Guid.NewGuid();
                        if (isCancelCostDeduct) defaultSampleApplyChargeItem.Price = 0d;
                        _sampleApplyChargeItemBLL.Add(new SampleApplyChargeItem[] { defaultSampleApplyChargeItem }, ref tran, true);
                    }
                    if (model.SampleTesters != null && model.SampleTesters.Count > 0)
                    {
                        foreach (var sampleTester in model.SampleTesters) sampleTester.SampleApplyId = model.Id;
                        _sampleTesterBLL.Add(model.SampleTesters.ToArray(), ref tran, true);
                    }
                    if (isCostDeduct || isCancelCostDeduct)
                    {
                        model.CostDeduct.UserAccountId = userAccount.Id;
                        model.CostDeduct.SampleApplyId = model.Id;
                        model.CostDeduct.SampleApplyForLog = model;
                        _costDeductBLL.Add(new CostDeduct[] { model.CostDeduct }, ref tran, true);
                    }

                    if (model.SampleApplyAttachments != null && model.SampleApplyAttachments.Count > 0)
                    {
                        var sampleApplyAttachments = _sampleApplyAttachmentBLL.GetEntitiesByExpression(model.SampleApplyAttachments.Select(p => p.Id).ToFormatStr());
                        foreach (var sampleApplyAttachment in model.SampleApplyAttachments)
                        {
                            sampleApplyAttachment.SampleApplyId = model.Id;
                            if (sampleApplyAttachments != null && sampleApplyAttachments.Any(p => p.Id == sampleApplyAttachment.Id))
                            {
                                _sampleApplyAttachmentBLL.Save(new SampleApplyAttachment[] { sampleApplyAttachment }, ref tran, true);
                            }
                            else
                            {
                                _sampleApplyAttachmentBLL.Add(new SampleApplyAttachment[] { sampleApplyAttachment }, ref tran, true);
                            }
                        }
                    }
                    if (model.SampleApplyNumbers != null && model.SampleApplyNumbers.Count > 0)
                    {
                        foreach (var sampleApplyNumber in model.SampleApplyNumbers)
                        {
                            sampleApplyNumber.Id = Guid.NewGuid();
                            sampleApplyNumber.SampleApplyId = model.Id;
                        }
                        _sampleApplyNumberBLL.Add(model.SampleApplyNumbers, ref tran, true);
                    }
                    if (model.SampleApplyParameters != null && model.SampleApplyParameters.Count > 0)
                    {
                        foreach (var sampleApplyParameter in model.SampleApplyParameters)
                        {
                            sampleApplyParameter.Id = Guid.NewGuid();
                            sampleApplyParameter.SampleApplyId = model.Id;
                        }
                        _sampleApplyParameterBLL.Add(model.SampleApplyParameters, ref tran, true);
                    }
                }
                if ((isCostDeduct || isCancelCostDeduct))
                {
                    _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, true);
                    _openFundApplyBLL.SaveSampleAppyOpenFund(viewOpenFundDetailList, preCostDeductEquipmentOpenFunds, newCostDeductEquipmentOpenFunds, ref tran);
                }
                if (!ValidateUserCreditLimit(lstSampleApplyForValidate, false, out errorMessage)) throw new Exception(errorMessage);
                DoCreateOuterDeposit(models, userAccount, ref tran);
                tran.CommitTransaction();
                SendMessage(models, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错" + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool Edit(ref List<SampleApply> models, out string errorMessage)
        {
            return Edit(ref models, false, out errorMessage);
        }

        public bool Cancel(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            errorMessage = "";
            bool isEnaleAutoCancel = false;
            bool isEnableAutoCancelWhenHasCharged = false;
            try
            {
                var dictCodeType = _dictCodeTypeBLL.GetEntitiesByExpression(string.Format("Code=SampleApplyCancelSetting")).First();
                isEnaleAutoCancel = dictCodeType.DictCodes.First(p => p.Code == "IsEnaleAutoCancel").Value.Trim() == "1";
                isEnableAutoCancelWhenHasCharged = dictCodeType.DictCodes.First(p => p.Code == "IsEnableAutoCancelWhenHasCharged").Value.Trim() == "1";
            }
            catch { throw new Exception("出错,辅助字典dictCodeTypes节点配置错误"); }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                IList<SampleApply> lstSampleApplies = new List<SampleApply>();
                foreach (var id in ids)
                {
                    var sampleApply = _sampleApplyBLL.GetEntityById(id);
                    sampleApply.StatusEnum = SampleApplyStatus.ApplyCancel;
                    sampleApply.CancelOperaterId = operatorId;
                    sampleApply.CancelTime = DateTime.Now;
                    sampleApply.CancelRemark = remark;
                    sampleApply.OperateDate = DateTime.Now;
                    if (sampleApply.ChargeStatusEnum == SampleChargeStatus.UnCharge && isEnaleAutoCancel) sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.Canceled;
                    if (sampleApply.ChargeStatusEnum == SampleChargeStatus.Charged && isEnaleAutoCancel && isEnableAutoCancelWhenHasCharged) sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.Canceled;
                    _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                    lstSampleApplies.Add(sampleApply);
                }
                DoCostDeduct(lstSampleApplies, true, true, true, ref tran, out errorMessage);
                DoCreateOuterDeposit(lstSampleApplies, null, ref tran);
                tran.CommitTransaction();
                SendMessage(lstSampleApplies, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错" + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool Pass(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            var orginalSampleApply = _sampleApplyBLL.GetEntityById(sampleApply.Id);
            switch (orginalSampleApply.StatusEnum)
            {
                case SampleApplyStatus.UnAudit:
                case SampleApplyStatus.Accepted:
                    if (sampleApply.IsNeedTutorAudit && sampleApply.TutorAuditStatus != (int)AuditStatus.Passed) throw new Exception("出错, 该申请单需要导师审核通过才能进行通过操作");
                    sampleApply.StatusEnum = SampleApplyStatus.Audited;//已审核
                    break;
                case SampleApplyStatus.RefuseCancel:
                case SampleApplyStatus.ApplyCancel:
                    sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.Canceled;//已撤销
                    break;
                default:
                    throw new Exception("出错, 不能对该状态下的申请单进行通过操作");
            }
            sampleApply.PassDate = DateTime.Now;
            sampleApply.PasserId = operatorId;
            sampleApply.PassRemark = remark;
            sampleApply.Remark = remark;
            sampleApply.OperateDate = DateTime.Now;
            var models = new List<SampleApply>() { sampleApply };
            bool result = Edit(ref models, true, out errorMessage);
            sampleApply.Id = models.First().Id;
            return result;
        }

        public bool Pass(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                IList<SampleApply> lstSampleApplies = new List<SampleApply>();
                foreach (var id in ids)
                {
                    var sampleApply = _sampleApplyBLL.GetEntityById(id);
                    switch (sampleApply.StatusEnum)
                    {
                        case SampleApplyStatus.Accepted:
                        case SampleApplyStatus.UnAudit:
                            sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.Audited;//已审核
                            break;
                        case SampleApplyStatus.RefuseCancel:
                        case SampleApplyStatus.ApplyCancel:
                            sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.Canceled;
                            break;
                        default:
                            throw new Exception("出错, 不能对该状态下的申请单进行通过操作");
                    }
                    sampleApply.PassDate = DateTime.Now;
                    sampleApply.PasserId = operatorId;
                    sampleApply.PassRemark = remark;
                    sampleApply.Remark = remark;
                    sampleApply.OperateDate = DateTime.Now;
                    sampleApply.LoadReference();
                    _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                    if (sampleApply.SampleApplyChargeItems != null && sampleApply.SampleApplyChargeItems.Count > 0)
                    {
                        _sampleApplyChargeItemBLL.Save(sampleApply.SampleApplyChargeItems, ref tran, true);
                    }
                    lstSampleApplies.Add(sampleApply);
                }
                DoCostDeduct(lstSampleApplies, true, true, false, ref tran, out errorMessage);
                tran.CommitTransaction();
                SendMessage(lstSampleApplies, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错" + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool ReportConfirm(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.ReportConfirmed;//报告已发
            sampleApply.ReportConfirmTime = DateTime.Now;
            sampleApply.ReportConfirmOperatorId = operatorId;
            sampleApply.ReportConfirmRemark = remark;
            sampleApply.Remark = remark;
            sampleApply.OperateDate = DateTime.Now;
            var models = new List<SampleApply>() { sampleApply };
            bool result = Edit(ref models, true, out errorMessage);
            sampleApply.Id = models.First().Id;
            return result;
        }

        public bool ReportConfirm(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                IList<SampleApply> lstSampleApplies = new List<SampleApply>();
                foreach (var id in ids)
                {
                    var sampleApply = _sampleApplyBLL.GetEntityById(id);
                    sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.ReportConfirmed;//报告已发
                    sampleApply.ReportConfirmTime = DateTime.Now;
                    sampleApply.ReportConfirmOperatorId = operatorId;
                    sampleApply.ReportConfirmRemark = remark;
                    sampleApply.Remark = remark;
                    sampleApply.OperateDate = DateTime.Now;
                    sampleApply.LoadReference();
                    _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                    if (sampleApply.SampleApplyChargeItems != null && sampleApply.SampleApplyChargeItems.Count > 0)
                    {
                        _sampleApplyChargeItemBLL.Save(sampleApply.SampleApplyChargeItems, ref tran, true);
                    }
                    lstSampleApplies.Add(sampleApply);
                }
                DoCostDeduct(lstSampleApplies, true, true, false, ref tran, out errorMessage);
                tran.CommitTransaction();
                SendMessage(lstSampleApplies, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错" + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool Refuse(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            errorMessage = "";
            if (ids == null || ids.Length == 0)
            {
                errorMessage = "出错,编码为空";
                return false;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                IList<SampleApply> lstSampleApplies = new List<SampleApply>();
                foreach (var id in ids)
                {
                    var sampleApply = _sampleApplyBLL.GetEntityById(id);
                    switch (sampleApply.StatusEnum)
                    {
                        case SampleApplyStatus.UnAudit:
                        case SampleApplyStatus.Audited:
                        case SampleApplyStatus.Accepted:
                            sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.Canceled;//已撤销
                            break;
                        case SampleApplyStatus.ApplyCancel:
                        case SampleApplyStatus.Canceled:
                            sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.RefuseCancel;//拒绝撤销
                            break;
                        default:
                            throw new Exception("出错, 不能对该状态下的申请单进行否决操作");
                    }
                    sampleApply.RefuseRemark = remark;
                    sampleApply.RefuserId = operatorId;
                    sampleApply.RefuseDate = DateTime.Now;
                    sampleApply.Remark = remark;
                    sampleApply.OperateDate = DateTime.Now;
                    sampleApply.LoadReference();
                    _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                    lstSampleApplies.Add(sampleApply);
                }
                DoCostDeduct(lstSampleApplies, true, true, true, ref tran, out errorMessage);
                tran.CommitTransaction();
                SendMessage(lstSampleApplies, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错" + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool Refuse(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            var orginalSampleApply = _sampleApplyBLL.GetEntityById(sampleApply.Id);
            switch (orginalSampleApply.StatusEnum)
            {
                case SampleApplyStatus.UnAudit:
                case SampleApplyStatus.Audited:
                case SampleApplyStatus.Accepted:
                    sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.Canceled;//已撤销
                    break;
                case SampleApplyStatus.ApplyCancel:
                case SampleApplyStatus.Canceled:
                    sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.RefuseCancel;//拒绝撤销
                    break;
                default:
                    throw new Exception("出错, 不能对该状态下的申请单进行否决操作");
            }
            sampleApply.RefuseRemark = remark;
            sampleApply.RefuserId = operatorId;
            sampleApply.RefuseDate = DateTime.Now;
            sampleApply.Remark = remark;
            sampleApply.OperateDate = DateTime.Now;
            var models = new List<SampleApply> { sampleApply };
            bool result = Edit(ref models, true, out errorMessage);
            sampleApply.Id = models.First().Id;
            return result;
        }

        public bool Accept(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                IList<SampleApply> lstSampleApplies = new List<SampleApply>();
                foreach (var id in ids)
                {
                    var sampleApply = _sampleApplyBLL.GetEntityById(id);
                    sampleApply.AcceptRemark = remark;
                    sampleApply.AccepterId = operatorId;
                    sampleApply.AcceptDate = DateTime.Now;
                    sampleApply.StatusEnum = SampleApplyStatus.Accepted;
                    sampleApply.Remark = remark;
                    sampleApply.OperateDate = DateTime.Now;
                    sampleApply.LoadReference();
                    _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                    lstSampleApplies.Add(sampleApply);
                }
                DoCostDeduct(lstSampleApplies, true, true, true, ref tran, out errorMessage);
                tran.CommitTransaction();
                SendMessage(lstSampleApplies, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错" + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool Accept(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.Accepted;
            sampleApply.AcceptRemark = remark;
            sampleApply.AccepterId = operatorId;
            sampleApply.AcceptDate = DateTime.Now;
            sampleApply.Remark = remark;
            sampleApply.OperateDate = DateTime.Now;
            var models = new List<SampleApply> { sampleApply };
            return Edit(ref models, true, out errorMessage);
        }

        public bool RefuseAccept(Guid[] ids, Guid operatorId, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                IList<SampleApply> lstSampleApplies = new List<SampleApply>();
                foreach (var id in ids)
                {
                    var sampleApply = _sampleApplyBLL.GetEntityById(id);
                    sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.RefuseAccept;
                    sampleApply.RefuseAcceptRemark = remark;
                    sampleApply.RefuseAcceptOperaterId = operatorId;
                    sampleApply.RefuseAcceptDate = DateTime.Now;
                    sampleApply.Remark = remark;
                    sampleApply.OperateDate = DateTime.Now;
                    sampleApply.LoadReference();
                    _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                    lstSampleApplies.Add(sampleApply);
                }
                DoCostDeduct(lstSampleApplies, true, true, true, ref tran, out errorMessage);
                tran.CommitTransaction();
                SendMessage(lstSampleApplies, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错" + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool RefuseAccept(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            sampleApply.StatusEnum = Model.Enum.SampleApplyStatus.RefuseAccept;
            sampleApply.RefuseAcceptRemark = remark;
            sampleApply.RefuseAcceptOperaterId = operatorId;
            sampleApply.RefuseAcceptDate = DateTime.Now;
            sampleApply.OperateDate = DateTime.Now;
            sampleApply.Remark = remark;
            var models = new List<SampleApply> { sampleApply };
            return Edit(ref models, true, out errorMessage);
        }

        private bool Charge(SampleApply sampleApply, Guid operatorId, bool isSaveSampleApplySameTime, bool isEditSampleApplyChargeItem, bool isSysAutoCostDeduct, out string errorMessage, ref XTransaction tran)
        {
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var viewOpenFundDetailList = _viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(sampleApply.SampleItem.EquipmentId, sampleApply.PayerId, sampleApply.ExpectSendDate.Value);
                var userAccounts = CostDeduct(new List<SampleApply>() { sampleApply }, isEditSampleApplyChargeItem, isSysAutoCostDeduct, false, ref tran, out errorMessage);
                if (userAccounts != null && userAccounts.Count > 0) _userAccountBLL.Save(userAccounts.ToArray(), ref tran, true);
                if (isSaveSampleApplySameTime) _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                if (!string.IsNullOrWhiteSpace(errorMessage)) throw new Exception(errorMessage);
                if (!isSupress) tran.CommitTransaction();
                if (sampleApply.CostDeduct != null) _messageHandler.SendSampleApplyCostDeductNotice(sampleApply, _userBLL.GetEntityById(operatorId));
                return true;
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                errorMessage = "出错," + ex.Message;
                return false;
            }
            finally { if (!isSupress)tran.Dispose(); }
        }

        public bool Charge(SampleApply sampleApply, Guid operatorId, bool isEditSampleApplyChargeItem, bool isSysAutoCostDeduct, out string errorMessage, ref XTransaction tran)
        {
            return Charge(sampleApply, operatorId, true, isEditSampleApplyChargeItem, isSysAutoCostDeduct, out errorMessage, ref tran);
        }

        public bool Test(Guid sampleApplyId, Guid operaterId, string remark, out string errorMessage, SampleTestOption testOption)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                var sampleApply = _sampleApplyBLL.GetEntityById(sampleApplyId);
                var viewSampleApply = _viewSampleApplyBLL.GetEntityById(sampleApply.Id);
                var sampleTesters = _sampleTesterBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", viewSampleApply.Id.ToString()));
                var sampleTestRecords = _sampleTestRecordBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", viewSampleApply.Id.ToString()));
                SampleTestRecord sammpleTestRecord = new SampleTestRecord() { Id = Guid.NewGuid(), TesterId = operaterId, SampleApplyId = sampleApplyId };
                SampleTestRecord sammpleTestRecordTemp = null;
                var userSamplePrivilige = _userEquipmentSamplePriviligeBLL.GetUserEquipmentPriviligeByUserItemId(_user.Id, sampleApply.SampleItemId);
                bool isNew = true;
                if (!_viewSampleApplyBLL.JudgeIsEnableRelativeTestOperate(operaterId, viewSampleApply, sampleTesters, sampleTestRecords, testOption, userSamplePrivilige != null ? userSamplePrivilige.ToSamplePrivilige() : new NoSamplePrivilige(), out errorMessage)) throw new Exception(errorMessage);
                switch (testOption)
                {
                    case SampleTestOption.BeginHandle:
                        if (sampleTestRecords != null && sampleTestRecords.Count > 0)
                        {
                            sammpleTestRecordTemp = sampleTestRecords.FirstOrDefault(p => p.TestCategoryEnum == Model.Enum.SampleTestCategory.Test);
                        }
                        sammpleTestRecord.BeginTime = DateTime.Now;
                        sammpleTestRecord.EndTime = null;
                        sammpleTestRecord.TestCategoryEnum = Model.Enum.SampleTestCategory.Handle;
                        sampleApply.StatusEnum = SampleApplyStatus.BeginHandle;
                        break;
                    case SampleTestOption.BeginTest:
                        if (sampleTestRecords != null && sampleTestRecords.Count > 0)
                        {
                            sammpleTestRecordTemp = sampleTestRecords.FirstOrDefault(p => p.TestCategoryEnum == Model.Enum.SampleTestCategory.Handle && !p.EndTime.HasValue);
                        }
                        sammpleTestRecord.BeginTime = DateTime.Now;
                        sammpleTestRecord.EndTime = null;
                        sammpleTestRecord.TestCategoryEnum = Model.Enum.SampleTestCategory.Test;
                        sampleApply.StatusEnum = SampleApplyStatus.BeginTest;
                        break;
                    case SampleTestOption.EndHandle:
                        if (sampleTestRecords != null && sampleTestRecords.Count > 0)
                        {
                            sammpleTestRecordTemp = sampleTestRecords.FirstOrDefault(p => p.TestCategoryEnum == Model.Enum.SampleTestCategory.Test);
                        }
                        if (sampleTestRecords != null && sampleTestRecords.Count > 0)
                        {
                            sammpleTestRecordTemp = sampleTestRecords.FirstOrDefault(p => p.TestCategoryEnum == Model.Enum.SampleTestCategory.Handle && p.TesterId == operaterId);
                        }
                        sammpleTestRecord.EndTime = DateTime.Now;
                        sammpleTestRecord.TestCategoryEnum = Model.Enum.SampleTestCategory.Handle;
                        sampleApply.StatusEnum = SampleApplyStatus.FinishHandle;
                        break;
                    case SampleTestOption.EndTest:

                        if (sampleTestRecords != null && sampleTestRecords.Count > 0)
                        {
                            sammpleTestRecordTemp = sampleTestRecords.FirstOrDefault(p => p.TestCategoryEnum == Model.Enum.SampleTestCategory.Handle && !p.EndTime.HasValue);
                        }
                        if (sampleTestRecords != null && sampleTestRecords.Count > 0)
                        {
                            sammpleTestRecordTemp = sampleTestRecords.FirstOrDefault(p => p.TestCategoryEnum == Model.Enum.SampleTestCategory.Test && p.TesterId == operaterId);
                        }
                        sammpleTestRecord.EndTime = DateTime.Now;
                        sammpleTestRecord.TestCategoryEnum = SampleTestCategory.Test;
                        sampleApply.StatusEnum = SampleApplyStatus.BeginTest;
                        sammpleTestRecord.Remark = sammpleTestRecordTemp.Remark;
                        sammpleTestRecord.TestContent = sammpleTestRecordTemp.TestContent;
                        break;
                }
                SampleTestRecord sammpleTestRecordFind = null;
                if (sampleTestRecords != null && sampleTestRecords.Count > 0)
                {
                    if (sampleTestRecords.Any(p => p.TesterId == operaterId && p.TestCategoryEnum == sammpleTestRecord.TestCategoryEnum))
                    {
                        sammpleTestRecordFind = sampleTestRecords.FirstOrDefault(p => p.TesterId == operaterId && p.TestCategoryEnum == sammpleTestRecord.TestCategoryEnum);
                        isNew = false;
                        sammpleTestRecordFind.Category = sammpleTestRecord.Category;
                        sammpleTestRecordFind.EndTime = sammpleTestRecord.EndTime;
                        sammpleTestRecordFind.Remark = sammpleTestRecord.Remark;
                        sammpleTestRecordFind.TestCategoryEnum = sammpleTestRecord.TestCategoryEnum;
                        sammpleTestRecordFind.TesterId = sammpleTestRecord.TesterId;
                    }
                }
                tran = SessionManager.Instance.BeginTransaction();
                if (isNew) _sampleTestRecordBLL.Add(new SampleTestRecord[] { isNew ? sammpleTestRecord : sammpleTestRecordFind }, ref tran, true);
                else _sampleTestRecordBLL.Save(new SampleTestRecord[] { isNew ? sammpleTestRecord : sammpleTestRecordFind }, ref tran, true);
                if (testOption == SampleTestOption.EndTest)
                {
                    var testers = _sampleTesterBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", sampleApplyId.ToString()));
                    if (testers != null && testers.Count > 0)
                    {
                        if (testers.Count == 1) sampleApply.StatusEnum = SampleApplyStatus.FinishTest;
                        else
                        {
                            var queryExpression = new StringBuilder(string.Format("SampleApplyId=\"{0}\"*(", sampleApply.Id.ToString()));
                            int index = -1;
                            foreach (var tester in testers)
                            {
                                index++;
                                queryExpression.Append(index == 0 ? string.Format("TesterId=\"{0}\"", tester.TesterId.ToString()) : string.Format("+TesterId=\"{0}\"", tester.TesterId.ToString()));
                            }
                            queryExpression.Append(string.Format(")*Id=-\"{0}\"*Category=1*EndTime=-null", sammpleTestRecord.Id.ToString()));
                            var sampleTestRecordsTemp = _sampleTestRecordBLL.GetEntitiesByExpression(queryExpression.ToString());
                            if (sampleTestRecordsTemp != null && sampleTestRecordsTemp.Count == testers.Count - 1) sampleApply.StatusEnum = SampleApplyStatus.FinishTest;
                        }
                    }
                    else sampleApply.StatusEnum = SampleApplyStatus.FinishTest;
                }
                if (sampleApply.StatusEnum == SampleApplyStatus.FinishTest)
                {
                    sampleApply.FinishDate = DateTime.Now;
                }
                sampleApply.OperateDate = DateTime.Now;
                _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                DoCostDeduct(new List<SampleApply>() { sampleApply }, true, true, true, ref tran, out errorMessage);
                tran.CommitTransaction();
                SendMessage(new SampleApply[] { sampleApply }, null, null, null, out errorMessage);
            }
            catch (Exception ex)
            {
                if (tran != null) tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
            return true;
        }

        public bool InputFinishedQuatity(SampleApply sampleApply, int realQuatity, double? duration, Guid operatorId, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                sampleApply.RealQuatity = realQuatity;
                sampleApply.Duration = duration;
                if (sampleApply.ChargeCategoryEnum == Model.Enum.SampleItemChargeCategory.SampleQuatity && sampleApply.ChargeStatusEnum == Model.Enum.SampleChargeStatus.Charged)
                {
                    var result = Charge(sampleApply, operatorId, false, false, false, out errorMessage, ref tran);
                    if (sampleApply.SampleApplyChargeItems != null && sampleApply.SampleApplyChargeItems.Count > 0)
                    {
                        _sampleApplyChargeItemBLL.Save(sampleApply.SampleApplyChargeItems, ref tran, true);
                    }
                    if (!result) throw new Exception(errorMessage);
                }
                _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错," + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }

        public bool Register(SampleApply sampleApply, Guid operatorId, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var lstSampleItemResults = _sampleItemResultBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", sampleApply.Id.ToString()));
                if (lstSampleItemResults == null || lstSampleItemResults.Count == 0)
                    _sampleItemResultBLL.Add(sampleApply.SampleItemResults.ToArray(), ref tran, true);
                else
                    _sampleItemResultBLL.Save(sampleApply.SampleItemResults.ToArray(), ref tran, true);

                var sampleAnalysisAttachmentsTemp = _sampleAnalysisAttachmentBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", sampleApply.Id.ToString()));
                if (sampleAnalysisAttachmentsTemp != null && sampleAnalysisAttachmentsTemp.Count > 0)
                {
                    var willdelSampleAnalysisAttachments = sampleAnalysisAttachmentsTemp;
                    if (sampleApply.SampleAnalysisAttachments != null && sampleApply.SampleAnalysisAttachments.Count > 0)
                        willdelSampleAnalysisAttachments = sampleAnalysisAttachmentsTemp.Where(p => !sampleApply.SampleAnalysisAttachments.Select(x => x.Id).Contains(p.Id)).ToList();
                    if (willdelSampleAnalysisAttachments != null && willdelSampleAnalysisAttachments.Count > 0)
                        _sampleAnalysisAttachmentBLL.Delete(willdelSampleAnalysisAttachments.Select(p => p.Id).ToArray(), ref tran, true);
                }
                if (sampleApply.SampleAnalysisAttachments != null && sampleApply.SampleAnalysisAttachments.Count > 0)
                {

                    foreach (var sampleAnalysisAttachment in sampleApply.SampleAnalysisAttachments)
                    {
                        var sampleAnalysisAttachments = _sampleAnalysisAttachmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", sampleAnalysisAttachment.Id.ToString()));
                        if (sampleAnalysisAttachments == null || sampleAnalysisAttachments.Count == 0)
                        {
                            _sampleAnalysisAttachmentBLL.Add(new SampleAnalysisAttachment[] { sampleAnalysisAttachment }, ref tran, true);
                        }
                        else
                        {
                            _sampleAnalysisAttachmentBLL.Save(new SampleAnalysisAttachment[] { sampleAnalysisAttachment }, ref tran, true);
                        }
                    }
                }
                sampleApply.StatusEnum = SampleApplyStatus.Registed;
                sampleApply.RegisterId = operatorId;
                sampleApply.RegisterTime = DateTime.Now;
                sampleApply.OperateDate = DateTime.Now;
                _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                DoCostDeduct(new List<SampleApply> { sampleApply }, true, true, true, ref tran, out errorMessage);
                if (sampleApply.SampleApplyChargeItems != null && sampleApply.SampleApplyChargeItems.Count > 0)
                {
                    _sampleApplyChargeItemBLL.Save(sampleApply.SampleApplyChargeItems, ref tran, true);
                }
                tran.CommitTransaction();
                SendMessage(new SampleApply[] { sampleApply }, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错," + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool Release(Guid sampleApplyId, Guid reportObtainerId, Guid operateId, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction(); ;
            try
            {
                var sampleApply = _sampleApplyBLL.GetEntityById(sampleApplyId);
                sampleApply.ReportObtainerId = reportObtainerId;
                sampleApply.ReportObtainTime = DateTime.Now;
                sampleApply.ReleaseRemark = remark;
                sampleApply.ReleaserId = operateId;
                sampleApply.ReleaseTime = DateTime.Now;
                sampleApply.OperateDate = DateTime.Now;
                sampleApply.StatusEnum = SampleApplyStatus.Released;
                sampleApply.Remark = remark;
                _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                sampleApply.IsSupressLazyLoad = false;//下面的扣费需要对申请人进行延迟加载
                DoCostDeduct(new List<SampleApply>() { sampleApply }, true, true, true, ref tran, out errorMessage);
                if (sampleApply.SampleApplyChargeItems != null && sampleApply.SampleApplyChargeItems.Count > 0)
                {
                    _sampleApplyChargeItemBLL.Save(sampleApply.SampleApplyChargeItems, ref tran, true);
                }
                tran.CommitTransaction();
                SendMessage(new SampleApply[] { sampleApply }, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错," + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool DownLoad(Guid sampleApplyId, Guid operateId, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var sampleApply = _sampleApplyBLL.GetEntityById(sampleApplyId);
                sampleApply.ReportObtainerId = operateId;
                sampleApply.StatusEnum = SampleApplyStatus.Obtained;
                sampleApply.OperateDate = DateTime.Now;
                if (!sampleApply.ReportObtainTime.HasValue) sampleApply.ReportObtainTime = DateTime.Now;
                _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                DoCostDeduct(new List<SampleApply>() { sampleApply }, true, true, true, ref tran, out errorMessage);
                if (sampleApply.SampleApplyChargeItems != null && sampleApply.SampleApplyChargeItems.Count > 0)
                {
                    _sampleApplyChargeItemBLL.Save(sampleApply.SampleApplyChargeItems, ref tran, true);
                }
                tran.CommitTransaction();
                SendMessage(new SampleApply[] { sampleApply }, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错," + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool Doubts(Guid sampleApplyId, Guid operateId, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var sampleApply = _sampleApplyBLL.GetEntityById(sampleApplyId);
                sampleApply.DoubterId = operateId;
                sampleApply.StatusEnum = SampleApplyStatus.Doubt;
                if (!sampleApply.DoubtTime.HasValue) sampleApply.DoubtTime = DateTime.Now;
                sampleApply.DoubtRemark = remark;
                sampleApply.OperateDate = DateTime.Now;
                sampleApply.Remark = remark;
                sampleApply.IsDoubt = true;
                _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                DoCostDeduct(new List<SampleApply>() { sampleApply }, true, true, true, ref tran, out errorMessage);
                tran.CommitTransaction();
                SendMessage(new SampleApply[] { sampleApply }, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错," + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool AuditDoublt(Guid sampleApplyId, Guid operateId, SampleApplyStatus sampleStatus, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var sampleApply = _sampleApplyBLL.GetEntityById(sampleApplyId);
                sampleApply.DoubtAuditorId = operateId;
                sampleApply.DoubtAuditTime = DateTime.Now;
                sampleApply.DoubtAuditRemark = remark;
                sampleApply.StatusEnum = sampleStatus;
                sampleApply.OperateDate = DateTime.Now;
                sampleApply.Remark = remark;
                _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                DoCostDeduct(new List<SampleApply>() { sampleApply }, true, true, true, ref tran, out errorMessage);
                tran.CommitTransaction();
                SendMessage(new SampleApply[] { sampleApply }, null, null, null, out errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错," + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

        public bool FeedBack(SampleApply sampleApply, Guid operatorId, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {

                var sampleFeedBackAttachmentsTemp = _sampleFeedBackAttachmentBLL.GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", sampleApply.Id.ToString()));
                if (sampleFeedBackAttachmentsTemp != null && sampleFeedBackAttachmentsTemp.Count > 0)
                {
                    var willdelSampleFeedBackAttachments = sampleFeedBackAttachmentsTemp;
                    if (sampleApply.SampleFeedBackAttachments != null && sampleApply.SampleFeedBackAttachments.Count > 0)
                        willdelSampleFeedBackAttachments = sampleFeedBackAttachmentsTemp.Where(p => !sampleApply.SampleFeedBackAttachments.Select(x => x.Id).Contains(p.Id)).ToList();
                    if (willdelSampleFeedBackAttachments != null && willdelSampleFeedBackAttachments.Count > 0)
                        _sampleFeedBackAttachmentBLL.Delete(willdelSampleFeedBackAttachments.Select(p => p.Id).ToArray(), ref tran, true);
                }
                if (sampleApply.SampleFeedBackAttachments != null && sampleApply.SampleFeedBackAttachments.Count > 0)
                {

                    foreach (var sampleFeedBackAttachment in sampleApply.SampleFeedBackAttachments)
                    {
                        var sampleFeedBackAttachments = _sampleFeedBackAttachmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", sampleFeedBackAttachment.Id.ToString()));
                        if (sampleFeedBackAttachments == null || sampleFeedBackAttachments.Count == 0)
                            _sampleFeedBackAttachmentBLL.Add(new SampleFeedBackAttachment[] { sampleFeedBackAttachment }, ref tran, true);
                        else
                            _sampleFeedBackAttachmentBLL.Save(new SampleFeedBackAttachment[] { sampleFeedBackAttachment }, ref tran, true);
                    }
                }
                sampleApply.FeedbackTime = DateTime.Now;
                sampleApply.FeedbackRemark = remark;
                sampleApply.Remark = remark;
                _sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = "出错," + ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }
    }
}
