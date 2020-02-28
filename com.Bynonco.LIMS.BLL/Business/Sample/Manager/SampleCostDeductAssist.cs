using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    public partial class SampleManager
    {
        private static IList<DictCodeType> __whenSampleCharge = null;
        private static IList<DictCodeType> __whenSampleCancelCharge = null;
        /// <summary>
        /// 根据状态判断是否进行扣费
        /// </summary>
        private bool JudgeIsCostDeduct(SampleApply sampleApply)
        {
            string[] statuses = null;
            if (string.IsNullOrWhiteSpace(sampleApply.WhenChargeStatus))
            {
                if (__whenSampleCharge == null)
                    __whenSampleCharge = _dictCodeTypeBLL.GetEntitiesByExpression("Code=WhenSampleCharge");
                if (__whenSampleCharge == null || __whenSampleCharge.Count == 0) throw new Exception("出错,找不到IsChargeWhenSampleAppoinment配置");
                var dictCodeType = __whenSampleCharge.First();
                if (!string.IsNullOrWhiteSpace(dictCodeType.Value))
                {
                    statuses = dictCodeType.Value.Trim().Replace("，", ",").Split(',');

                }
            }
            else
            {
                statuses = sampleApply.WhenChargeStatus.Trim().Replace("，", ",").Split(',');
            }
            return statuses.Contains(sampleApply.Status.ToString());
        }
        /// <summary>
        /// 根据状态判断是否进行退费
        /// </summary>
        private bool JudgeIsCancelCostDeduct(SampleApply sampleApply)
        {
            string[] statuses = null;
            if (string.IsNullOrWhiteSpace(sampleApply.WhenCancelChargeStatus))
            {
                if (__whenSampleCancelCharge == null) __whenSampleCancelCharge = _dictCodeTypeBLL.GetEntitiesByExpression("Code=WhenSampleCancelCharge*IsDelete=false*IsStop=false");
                if (__whenSampleCancelCharge == null || __whenSampleCancelCharge.Count == 0) throw new Exception("出错,找不到WhenSampleCancelCharge配置");
                var dictCodeType = __whenSampleCancelCharge.First();
                if (!string.IsNullOrWhiteSpace(dictCodeType.Value))
                {
                    statuses = dictCodeType.Value.Trim().Replace("，", ",").Split(',');
                }
            }
            else
            {
                statuses = sampleApply.WhenCancelChargeStatus.Trim().Replace("，", ",").Split(',');
            }
            return statuses.Contains(sampleApply.Status.ToString());
        }
        /// <summary>
        /// 是否非预存方式的用户类型
        /// </summary>
        private bool JudgeNotPredictDeposit(SampleApply sampleApply)
        {
            var result = sampleApply.Applicant == null || sampleApply.Applicant.UserType.UserIdentityEnum == UserIdentity.Outcommer;
            return result;
        }
        private bool JudgeIsCreateDepositWhenSampleOuterApply()
        {
            bool isCreateDepositWhenSampleOuterApply = false;
            var isCreateDepositWhenSampleOuterApplyDictCodeType = _dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=IsCreateDepositWhenSampleOuterApply*IsDelete=false*IsStop=false");
            if (isCreateDepositWhenSampleOuterApplyDictCodeType != null && !string.IsNullOrWhiteSpace(isCreateDepositWhenSampleOuterApplyDictCodeType.Value))
                isCreateDepositWhenSampleOuterApply = isCreateDepositWhenSampleOuterApplyDictCodeType.Value.Trim() == "1";
            return isCreateDepositWhenSampleOuterApply;
        }
        private IList<UserAccount> CostDeduct(IList<SampleApply> sampleApplies, bool isEditSampleApplyChargeItem, bool isSysAutoCostDeduct, bool isSkipCreditLimitValidate, ref XTransaction tran, out string errorMessage)
        {
            errorMessage = "";
            List<UserAccount> userAccounts = new List<UserAccount>();
            IList<Guid> deletedIds = new List<Guid>();
            IList<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
            IList<CostDeductEquipmentOpenFund> newCostDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
            IList<ViewOpenFundDetail> viewOpenFundDetails = new List<ViewOpenFundDetail>();
            sampleApplies = sampleApplies.OrderByDescending(p => p.Price).ToList();
            foreach (var sampleApply in sampleApplies)
            {
                var viewOpenFundDetailList = _viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(sampleApply.SampleItem.EquipmentId, sampleApply.Subject.SubjectDirectorId.Value, sampleApply.ExpectSendDate.Value);
                if (viewOpenFundDetailList != null && viewOpenFundDetailList.Count > 0)
                {
                    foreach (var viewOpenFundDetail in viewOpenFundDetailList)
                    {
                        if (!viewOpenFundDetails.Any(p => p.OpenFundApplyId == viewOpenFundDetail.OpenFundApplyId && viewOpenFundDetail.EquipmentId == p.EquipmentId))
                        {
                            viewOpenFundDetails.Add(viewOpenFundDetail);
                        }
                    }
                }
            }
            foreach (var sampleApply in sampleApplies)
            {
                if (sampleApply.CostDeduct != null && sampleApply.CostDeduct.CostDeductEquipmentOpenFunds != null && sampleApply.CostDeduct.CostDeductEquipmentOpenFunds.Count > 0)
                {
                    foreach (var costDeductEquipmentOpenFund in sampleApply.CostDeduct.CostDeductEquipmentOpenFunds)
                    {
                        var viewOpenFundDetailListFind = viewOpenFundDetails.Where(p => p.OpenFundApplyId == costDeductEquipmentOpenFund.OpenFundApplyId);
                        if (viewOpenFundDetailListFind != null && viewOpenFundDetailListFind.Count() > 0)
                        {
                            foreach (var viewOpenFundDetailFind in viewOpenFundDetailListFind)
                            {
                                viewOpenFundDetailFind.BalanceMoney += costDeductEquipmentOpenFund.CalFee;
                            }
                        }
                        preCostDeductEquipmentOpenFunds.Add(costDeductEquipmentOpenFund);
                    }
                }
            }
            foreach (var sampleApply in sampleApplies)
            {
                if (!ValidateUserCreditLimit(new List<SampleApply> { sampleApply }, isSkipCreditLimitValidate, out errorMessage)) throw new Exception(errorMessage);
                double cancelDeductRealCurrency = 0d, cancelVirtualCurrency = 0d, realCurrency = 0d, virtualCurrency = 0d;
                bool isCostDeduct = JudgeIsCostDeduct(sampleApply);
                bool isCancelCostDeduct = JudgeIsCancelCostDeduct(sampleApply);
                if (isCostDeduct) sampleApply.ChargeStatus = (int)SampleChargeStatus.Charged;
                if (isCancelCostDeduct) sampleApply.ChargeStatus = (int)SampleChargeStatus.CancelCharge;
                if (isCostDeduct || isCancelCostDeduct || !isSysAutoCostDeduct)
                {
                    var userAccount = userAccounts.FirstOrDefault(p => p.UserId == sampleApply.Subject.SubjectDirectorId.Value);
                    if (userAccount == null)
                    {
                        //扣费0元获取到取消扣费账户，因为当前是编辑状态，扣费需要做两个步骤，第一先把原来扣掉的费用加回用户账户，然后再把当前实际费用从账户扣费，考虑到申请单的课题信息和支持方式信息可能发生修改，所以以下处理取消扣费和扣费可能是不同账户的情况
                        userAccount = _userAccountBLL.Deduct(false, sampleApply.ApplicantId, sampleApply.SubjectId, sampleApply.PaymentMethodEnum, 0, out cancelDeductRealCurrency, out cancelVirtualCurrency, ref tran, out errorMessage, true);
                        userAccounts.Add(userAccount);
                    }
                    userAccount.RealCurrency += sampleApply.RealCurrency;
                    userAccount.VirtualCurrency += sampleApply.VirtualCurrency;
                    if (sampleApply.CostDeduct != null && !deletedIds.Contains(sampleApply.CostDeduct.Id))
                    {
                        _costDeductBLL.Delete(new Guid[] { sampleApply.CostDeduct.Id }, ref tran, _user.Id, "", true, false);
                        deletedIds.Add(sampleApply.CostDeduct.Id);
                    }
                    sampleApply.CostDeduct = new Model.CostDeduct() { CalcDuration = 0, CostDeductType = (int)CostDeductType.SampleCostDeduct, CreaterId = _user.Id, DeductAt = DateTime.Now, Id = Guid.NewGuid(), Duration = 0, SampleApplyId = sampleApply.Id, UserAccountId = userAccount.Id };
                    if (isSysAutoCostDeduct && isCancelCostDeduct)
                    {
                        sampleApply.CostDeduct.RealCurrency = 0d;
                        sampleApply.CostDeduct.VirtualCurrency = 0d;
                        sampleApply.RealCurrency = 0d;
                        sampleApply.VirtualCurrency = 0d;
                        sampleApply.IsCalPriceUsingOpenFund = false;
                        sampleApply.OpenFundDiscount = null;
                        sampleApply.OpenFundCurrency = 0d;
                        if (sampleApply.SampleApplyChargeItems != null && sampleApply.SampleApplyChargeItems.Count > 0)
                        {
                            foreach (var sampleApplyChargeItem in sampleApply.SampleApplyChargeItems)
                            {
                                sampleApplyChargeItem.Price = 0d;
                                sampleApplyChargeItem.OpenFundPrice = 0d;
                            }
                            _sampleApplyChargeItemBLL.Save(sampleApply.SampleApplyChargeItems, ref tran, true);
                        }
                    }
                    if (!isSysAutoCostDeduct || (isSysAutoCostDeduct && isCostDeduct && !isCancelCostDeduct))
                    {
                        IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
                        bool isOpenFund = false;
                        _sampleApplyBLL.GetSampleApplyPrice(viewOpenFundDetails, sampleApply, null,isEditSampleApplyChargeItem, isCostDeduct, CustomerFactory.GetCustomer().GetIsOpenFundCostDeduct(), out isOpenFund, out costDeductEquipmentOpenFunds);
                        GenerateSampleApplyOpenFundCostDeduct(sampleApply, costDeductEquipmentOpenFunds, newCostDeductEquipmentOpenFunds);
                        if (userAccount != null)
                        {
                            userAccount = userAccount.Deduct(isOpenFund, sampleApply.Price.Value, out realCurrency, out virtualCurrency);
                        }
                        sampleApply.RealCurrency = realCurrency;
                        sampleApply.VirtualCurrency = virtualCurrency;
                        sampleApply.CostDeduct.VirtualCurrency = virtualCurrency;
                        sampleApply.CostDeduct.RealCurrency = realCurrency;
                    }
                    sampleApply.CostDeduct.SampleApplyForLog = sampleApply;
                    _costDeductBLL.Add(new CostDeduct[] { sampleApply.CostDeduct }, ref tran, true);
                }
            }
            if (userAccounts != null && userAccounts.Count > 0)
            {
                _openFundApplyBLL.SaveSampleAppyOpenFund(viewOpenFundDetails, preCostDeductEquipmentOpenFunds, newCostDeductEquipmentOpenFunds, ref tran);
            }
            return userAccounts;
        }
        private bool DoCostDeduct(IList<SampleApply> sampleApplies, bool isEditSampleApplyChargeItem, bool isSysAutoCostDeduct, bool isSkipCreditLimitValidate,ref XTransaction tran, out string errorMessage)
        {
            var userAccounts = CostDeduct(sampleApplies, isEditSampleApplyChargeItem, isSysAutoCostDeduct, isSkipCreditLimitValidate, ref tran, out errorMessage);
            if (!string.IsNullOrWhiteSpace(errorMessage)) throw new Exception(errorMessage);
            if (userAccounts != null && userAccounts.Count > 0)
            {
                _userAccountBLL.Save(userAccounts.ToArray(), ref tran, true);
            }
            return true;
        }
        private bool DoCreateOuterDeposit(IList<SampleApply> sampleApplies, UserAccount userAccount, ref XTransaction tran)
        {
            var sampleApply = sampleApplies.First();
            if (sampleApply.IsOuter && JudgeIsCreateDepositWhenSampleOuterApply())
            {
                if (userAccount == null)
                    userAccount = _subjectBLL.GetEntityById(sampleApply.SubjectId).Director.UserAccount;
                var depositRecord = _depositRecordBLL.GetFirstOrDefaultEntityByExpression(string.Format("SampleNo=\"{0}\"", sampleApply.SampleNo), null, "DepositDate D");
                bool isNew = depositRecord == null;
                if (JudgeIsCancelCostDeduct(sampleApply) && depositRecord != null && !depositRecord.HasChecked)
                {
                    depositRecord.DepositSum -= (decimal)sampleApply.Price.Value;
                    if (depositRecord.DepositSum == (decimal)0d)
                    {
                        _depositRecordBLL.Delete(new Guid[] { depositRecord.Id }, ref tran, true);
                    }
                    else
                    {
                        _depositRecordBLL.Save(new DepositRecord[] { depositRecord }, ref tran, true);
                    }
                    return true;
                }
                double depositSum = 0d;
                depositSum += sampleApplies.Sum(p => p.Price.Value);
                if(sampleApply.EditId.HasValue)
                {
                    var sampleApplyFindList = _sampleApplyBLL.GetEntitiesByExpression(string.Format("SampleNo=\"{0}\"", sampleApply.SampleNo));
                    if (sampleApplyFindList != null && sampleApplyFindList.Count > 0)
                    {
                        depositSum += sampleApplyFindList.Where(p => !JudgeIsCancelCostDeduct(p)).Sum(p => p.Price.Value);
                    }
                    var editSampleApply = sampleApplyFindList.First(p => p.Id == sampleApply.EditId.Value);
                    depositSum -= editSampleApply.Price.Value;
                }
               
                if (depositRecord == null)
                {
                    depositRecord = new DepositRecord() { Id = Guid.NewGuid(), HasChecked = false };
                }
                if (!depositRecord.HasChecked)
                {
                    depositRecord.AccountId = userAccount.Id;
                    depositRecord.ApplyDate = DateTime.Now;
                    depositRecord.CheckerId = null;
                    depositRecord.CheckTime = null;
                    depositRecord.DepositDate = DateTime.Now;
                    depositRecord.DepositSum = (decimal)depositSum;
                    if (string.IsNullOrWhiteSpace(depositRecord.Remark)) depositRecord.Remark = "校外送样预约存款";
                    depositRecord.SampleNo = sampleApply.SampleNo;
                    depositRecord.UserId = sampleApply.ApplicantId;
                }
                if (isNew)
                {
                    _depositRecordBLL.Add(new DepositRecord[] { depositRecord }, ref tran, true);
                }
                else
                {
                    _depositRecordBLL.Save(new DepositRecord[] { depositRecord }, ref tran, true);
                }
            }
            return true;
        }
        private bool JudegIsValidateUserCreditLimit(SampleApply sampleApply, bool isSkipValidate)
        {
            var result = sampleApply.Applicant == null || sampleApply.Applicant.UserType.UserIdentityEnum == UserIdentity.Outcommer;
            return !(result);
        }
        private bool SimpleValidateUserCreditLimit(User user, Guid subjectId, PaymentMethod paymentMethod, double fee, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (fee <= 0d) return true;
                var result = _appointmentMoneyValidateBLL.SimpleMoneyValidate(user.Id, subjectId, paymentMethod, fee);
                if (!result) throw new Exception(errorMessage);
            }
            catch (Exception ex) { errorMessage = "出错," + ex.Message; return false; }
            return true;
        }
        /// <summary>
        /// 校验用户信用额度
        /// </summary>
        private bool ValidateUserCreditLimit(User user, Guid subjectId, PaymentMethod paymentMethod,IEnumerable<SampleApply> sampleApplies, double fee, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var result = _appointmentMoneyValidateBLL.SampleApplyMoneyValidate(user.Id, subjectId, paymentMethod, sampleApplies, fee, out errorMessage);
                if (!result) throw new Exception(errorMessage);
            }
            catch (Exception ex) { errorMessage = "出错," + ex.Message; return false; }
            return true;
        }
        private bool ValidateUserCreditLimit(IEnumerable<SampleApply> sampleApplies, bool isSkipValidate, out string errorMessage, User user = null)
        {
            errorMessage = "";
            if (!isSkipValidate)
            {
                if (sampleApplies.GroupBy(p => p.ApplicantId).Count() > 1) throw new Exception("出错,存在多个用户");
                double totalFee = sampleApplies.Sum(p => p.Price.HasValue ? p.Price.Value : 0d);
                var sampleApply = sampleApplies.First();
                bool isNotPredictDeposit = JudgeNotPredictDeposit(sampleApply);
                bool isCostDeduct = JudgeIsCostDeduct(sampleApply);
                bool isValidateUserCreditLimit = JudegIsValidateUserCreditLimit(sampleApply, isSkipValidate);
                if (!isNotPredictDeposit && isValidateUserCreditLimit)
                {
                    foreach (var samplaApply in sampleApplies)
                    {
                        if (JudgeIsCancelCostDeduct(samplaApply))
                            totalFee -= samplaApply.RealCurrency + samplaApply.VirtualCurrency;
                        if (JudgeIsCostDeduct(samplaApply) || !samplaApply.HasChanged)
                        {
                            if (samplaApply.ChargeStatusEnum == Model.Enum.SampleChargeStatus.Charged)
                                totalFee -= samplaApply.RealCurrency + samplaApply.VirtualCurrency;
                        }
                    }
                    if (isCostDeduct && sampleApply.StatusEnum != SampleApplyStatus.UnAudit)
                    {
                        return SimpleValidateUserCreditLimit(sampleApplies.First().Applicant == null ? user : sampleApplies.First().Applicant,
                                                   sampleApplies.First().SubjectId,
                                                   sampleApplies.First().PaymentMethodEnum,
                                                   totalFee,
                                                   out errorMessage);
                    }
                    return ValidateUserCreditLimit(sampleApplies.First().Applicant == null ? user : sampleApplies.First().Applicant,
                                                   sampleApplies.First().SubjectId,
                                                   sampleApplies.First().PaymentMethodEnum,
                                                   sampleApplies,
                                                   totalFee,
                                                   out errorMessage);
                }
            }
            return true;
        }
    }    
}
