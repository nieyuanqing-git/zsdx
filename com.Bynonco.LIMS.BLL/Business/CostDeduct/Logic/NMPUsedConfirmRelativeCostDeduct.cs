using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL
{
    public abstract class NMPUsedConfirmRelativeCostDeduct : CostDeductBase
    {
        protected INMPBLL _nmpBLL;
        protected INMPEquipmentBLL _nmpEquipmentBLL;
        protected ISubjectBLL _subjectBLL;
        protected INMPAppointmentBLL _nmpAppointmentBLL;
        protected INMPUsedConfirmBLL _nmpUsedConfirmBLL;
        protected INMPUsedConfirmFeedBackBLL _nmpUsedconfirmFeedBackBLL;
        protected INMPChargeStandardBLL _nmpChargeStandardBLL;
        protected INMPCalcFeeTimeRuleBLL _nmpCalcFeeTimeRuleBLL;
        protected NMPUsedConfirmFeedBack _nmpUsedConfirmFeedBack;
        protected NMPUsedConfirmFeedBack _preNMPUsedConfirmFeedBack;
        public NMPUsedConfirmRelativeCostDeduct(object[] businessObjects, Guid? operatorId, string operateIP)
            : base(businessObjects, operatorId, operateIP)
        {
            _subjectBLL = BLLFactory.CreateSubjectBLL();
            _nmpAppointmentBLL = BLLFactory.CreateNMPAppointmentBLL();
            _nmpUsedConfirmBLL = BLLFactory.CreateNMPUsedConfirmBLL();
            _nmpChargeStandardBLL = BLLFactory.CreateNMPChargeStandardBLL();
            _nmpCalcFeeTimeRuleBLL = BLLFactory.CreateNMPCalcFeeTimeRuleBLL();
            _nmpBLL = BLLFactory.CreateNMPBLL();
            _nmpEquipmentBLL = BLLFactory.CreateNMPEquipmentBLL();
            _nmpUsedconfirmFeedBackBLL = BLLFactory.CreateNMPUsedConfirmFeedBackBLL();
        }
        public override abstract object[] Deduct(ref august.DataLink.XTransaction tran);
        public override object[] CancelDeduct(ref august.DataLink.XTransaction tran) { return null; }
        protected abstract NMPUsedConfirm CreateNMPUseConfirm(out string errorMessage);
        protected NMPUsedConfirm AddNMPUsedConfirm(Guid? operatorId, User user, UserAccount userAccount, DateTime beginTime, DateTime endTime, string remark, double? realFee, ref august.DataLink.XTransaction tran, bool isDeduct)
        {
            string errorMessage = "";
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            var nmpUsedConfirm = CreateNMPUseConfirm(out errorMessage);
            if (realFee.HasValue) nmpUsedConfirm.RealFee = realFee.Value;//保存用户输入的实际费用
            double fee = 0d;
            var count = _nmpUsedConfirmBLL.CountModelListByExpression(string.Format("Id=\"{0}\"", nmpUsedConfirm.Id));
            if (user.Id != nmpUsedConfirm.UserId)
            {
                nmpUsedConfirm.SubjectId = _subjectBLL.GetUserSelfJoinSubject(user.Id).Id;
                nmpUsedConfirm.UserId = user.Id;
            }
            nmpUsedConfirm.Remark = remark;
            try
            {
                bool isAddedOrSavedNMPUsedConfirm = false;
                bool isEnableCostDeduct = JudgeIsEnableCostDeduct(nmpUsedConfirm, userAccount, isDeduct, out errorMessage);
                if (!isDeduct || !isEnableCostDeduct) nmpUsedConfirm.RealFee = 0d;
                nmpUsedConfirm.WhyNotCostDeduct = errorMessage;
                if (isEnableCostDeduct)
                {
                    ReCalNMPUsedConfirmFee(nmpUsedConfirm, realFee);
                    double realCurrency = 0d, virtualCurrency = 0d;
                    fee += nmpUsedConfirm.RealFee.Value;
                    if (count == 0)
                    {
                        _nmpUsedConfirmBLL.Add(new NMPUsedConfirm[] { nmpUsedConfirm }, ref tran, isSupress);
                    }
                    else
                    {
                        _nmpUsedConfirmBLL.Save(new NMPUsedConfirm[] { nmpUsedConfirm }, ref tran, isSupress);
                    }
                    isAddedOrSavedNMPUsedConfirm = true;
                    if (isDeduct)
                    {
                        userAccount.Deduct(false, nmpUsedConfirm.RealFee.Value, out realCurrency, out virtualCurrency);
                        nmpUsedConfirm.CostDeduct = new CostDeduct() { Id = Guid.NewGuid(), UserAccountId = userAccount.Id, NMPUsedConfirmId = nmpUsedConfirm.Id, CreaterId = operatorId, DeductAt = DateTime.Now, CostDeductTypeEnum= CostDeductType.NMPUsedCostDeduct };
                        nmpUsedConfirm.CostDeduct.CalcDuration = nmpUsedConfirm.CalcDuration;
                        nmpUsedConfirm.CostDeduct.Duration = (nmpUsedConfirm.EndAt.Value - nmpUsedConfirm.BeginAt.Value).TotalHours;
                        nmpUsedConfirm.CostDeduct.VirtualCurrency = virtualCurrency;
                        nmpUsedConfirm.CostDeduct.RealCurrency = realCurrency;
                        nmpUsedConfirm.WhyNotCostDeduct = "";
                        nmpUsedConfirm.Status = (int)UsedConfirmStatus.Deduct;
                        nmpUsedConfirm.CostDeduct.NMPUsedConfirmForLog = nmpUsedConfirm;
                        _costDeductBLL.Add(null, new CostDeduct[] { nmpUsedConfirm.CostDeduct }, null, ref tran, isSupress);
                    }
                }
                else if (nmpUsedConfirm.CostDeduct != null)
                {
                    nmpUsedConfirm.RealFee = nmpUsedConfirm.CostDeduct.VirtualCurrency.Value + nmpUsedConfirm.CostDeduct.RealCurrency.Value;
                }
                if (isEnableCostDeduct && isDeduct)
                {
                    _subjectBLL.Deduct(nmpUsedConfirm.UserId, nmpUsedConfirm.SubjectId.Value, nmpUsedConfirm.PaymentMethodEnum, fee, ref tran);
                    _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, isSupress);
                   
                }
                if (!isAddedOrSavedNMPUsedConfirm)
                {
                    if (nmpUsedConfirm.EndAt.HasValue && nmpUsedConfirm.EndAt.Value > nmpUsedConfirm.BeginAt.Value && nmpUsedConfirm.Status == (int)UsedConfirmStatus.UnComplete) nmpUsedConfirm.Status = (int)UsedConfirmStatus.Complete;
                    if (!nmpUsedConfirm.RealFee.HasValue) nmpUsedConfirm.RealFee = 0d;
                    if (count == 0)
                    {
                        _nmpUsedConfirmBLL.Add(new NMPUsedConfirm[] { nmpUsedConfirm }, ref tran, isSupress);
                    }
                    else
                    {
                        _nmpUsedConfirmBLL.Save(new NMPUsedConfirm[] { nmpUsedConfirm }, ref tran, isSupress);
                    }
                }
                //保存使用反馈
                SaveNMPUsedConfirmFeedBack(nmpUsedConfirm, ref tran, isSupress);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress)tran.Dispose(); }
            return nmpUsedConfirm;
        }
 
        protected bool JudgeIsEnableCostDeduct(NMPUsedConfirm nmpUsedConfirm, UserAccount userAccount, bool isDeduct, out string errorMessage)
        {
            return base.JudgeIsEnableCostDeduct(nmpUsedConfirm.SubjectId,
                                                 userAccount,
                                                 isDeduct,
                                                 nmpUsedConfirm.BeginAt,
                                                 nmpUsedConfirm.EndAt,
                                                 nmpUsedConfirm.NMPEquipment.NMP.ChargeStandard,
                                                 nmpUsedConfirm.NMPEquipment.NMP.CalcFeeTimeRule,
                                                 (UsedConfirmStatus)nmpUsedConfirm.Status,
                                                 nmpUsedConfirm.NMPEquipment.NMP.MinUnchangeMinutes,
                                                 out errorMessage);
        }
        protected void ReCalNMPUsedConfirmFee(NMPUsedConfirm nmpUsedConfirm, double? realFee)
        {
            if (realFee == null)
            {
                double duration = 0d,totalFee = 0d;
                _nmpUsedConfirmBLL.CalNMPUsedConfirmFee(nmpUsedConfirm, out duration,  out totalFee);
                nmpUsedConfirm.CalcDuration = duration;
            }
        }
        protected NMPUsedConfirm UpdateUseTime(Guid? operatorId, NMPUsedConfirm nmpUsedConfirm, User user, DateTime startAt, DateTime endAt, double? realFee, ref XTransaction tran, NMPUsedConfirmFeedBack nmpUsedConfirmFeedBack, bool isDeduct = true)
        {
            string errorMessage = "";
            var isSupress = tran != null;
            try
            {
                nmpUsedConfirm.BeginAt = nmpUsedConfirm.BeginAt < startAt ? nmpUsedConfirm.BeginAt : startAt;
                nmpUsedConfirm.EndAt = nmpUsedConfirm.EndAt > endAt ? nmpUsedConfirm.EndAt : endAt;
                var remark = string.Format("{0:yyyy-MM-dd HH:mm}~{1:yyyy-MM-dd HH:mm};", startAt, endAt);
                nmpUsedConfirm.Remark += (nmpUsedConfirm.Remark.IndexOf(remark) == -1 ? remark : "");
                _nmpUsedConfirmBLL.GenerateRelativeAppointmentNMPUsedConfirm(nmpUsedConfirm.GetNMPAppointment(), nmpUsedConfirm);
                GenerateNMPUsedConfirmRelativeFeedBack(nmpUsedConfirm);
                if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
                PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
                UserAccount userAccount = null, userAccountForValidate = null;
                var costDeduct = nmpUsedConfirm.CostDeduct;
                if (string.IsNullOrEmpty(nmpUsedConfirm.Remark))
                {
                    nmpUsedConfirm.Remark += string.Format("系统自动合成以下使用时间:{0:yyyy-MM-dd HH:mm}~{1:yyyy-MM-dd HH:mm};", nmpUsedConfirm.BeginAt, nmpUsedConfirm.EndAt);
                }
                if (nmpUsedConfirm.Subject != null && nmpUsedConfirm.Subject.Director != null && nmpUsedConfirm.Subject.Director.UserAccount != null)
                {
                    userAccountForValidate = nmpUsedConfirm.Subject.Director.UserAccount;
                }
                bool isEnableCostDeduct = JudgeIsEnableCostDeduct(nmpUsedConfirm, userAccountForValidate, isDeduct, out errorMessage);
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    if (nmpUsedConfirm.WhyNotCostDeduct == null) nmpUsedConfirm.WhyNotCostDeduct = "";
                    nmpUsedConfirm.WhyNotCostDeduct += nmpUsedConfirm.WhyNotCostDeduct.IndexOf(errorMessage) == -1 ? " " + errorMessage : "";
                }
                if (isDeduct && isEnableCostDeduct)
                {
                    
                    if (costDeduct != null)
                    {
                        if (nmpUsedConfirm.UserId == user.Id)
                        {
                            ReCalNMPUsedConfirmFee(nmpUsedConfirm, realFee);
                            if (realFee.HasValue) nmpUsedConfirm.RealFee = realFee.Value;//保存用户输入的实际费用
                            nmpUsedConfirm.Status = (int)UsedConfirmStatus.Deduct;
                            nmpUsedConfirm.WhyNotCostDeduct = "";
                            var fee = nmpUsedConfirm.RealFee.Value - costDeduct.RealCurrency.Value + costDeduct.VirtualCurrency.Value;
                            userAccount = _subjectBLL.Deduct(nmpUsedConfirm.UserId, nmpUsedConfirm.SubjectId.Value, nmpUsedConfirm.PaymentMethodEnum, fee, ref tran);
                            userAccount.RealCurrency += costDeduct.RealCurrency.Value;
                            userAccount.VirtualCurrency += costDeduct.VirtualCurrency.Value;
                            double virtualCurrency = 0d, realCurrency = 0d;
                            userAccount.Deduct(false, nmpUsedConfirm.RealFee.Value, out realCurrency, out virtualCurrency);
                            costDeduct.RealCurrency = realCurrency;
                            costDeduct.VirtualCurrency = virtualCurrency;
                            costDeduct.CalcDuration = nmpUsedConfirm.CalcDuration;
                            costDeduct.Duration = nmpUsedConfirm.Duration;
                            costDeduct.CreaterId = operatorId;
                            costDeduct.NMPUsedConfirmForLog = nmpUsedConfirm;
                            _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, true);
                            _costDeductBLL.Save(new CostDeduct[] { costDeduct }, ref tran, true);
                            _nmpUsedConfirmBLL.Save(new NMPUsedConfirm[] { nmpUsedConfirm }, ref tran, true);
                        }
                        else
                        {
                            //前后用户不一样
                            return ChangeUseTime(null, nmpUsedConfirm, user, startAt, endAt, ref tran, isDeduct);
                        }
                    }
                    else
                    {
                        var subjectId = nmpUsedConfirm.UserId != user.Id ? _subjectBLL.GetUserSelfJoinSubject(user.Id).Id : nmpUsedConfirm.SubjectId.Value;
                        _userBLL.GetPayer(user.Id, subjectId, out paymentMethod, out userAccount);
                        if (_preNMPUsedConfirmFeedBack != null)
                        {
                            _nmpUsedconfirmFeedBackBLL.Delete(new Guid[] { _preNMPUsedConfirmFeedBack.Id }, ref tran, true);
                            _preNMPUsedConfirmFeedBack.Delete();
                            if (nmpUsedConfirm.FeedBack != null) nmpUsedConfirm.FeedBack.Delete();//修改对象状态
                        }
                        var count = _nmpUsedConfirmBLL.CountModelListByExpression(string.Format("Id=\"{0}\"", nmpUsedConfirm.Id));
                        if (count == 0)
                        {
                            _nmpUsedConfirmBLL.Delete(new Guid[] { nmpUsedConfirm.Id }, ref tran, true);
                        }
                        return AddNMPUsedConfirm(operatorId, user, userAccount, nmpUsedConfirm.BeginAt.Value, nmpUsedConfirm.EndAt.Value, nmpUsedConfirm.Remark, realFee, ref tran, isDeduct);
                    }
                }
                else
                {
                    //考虑前后用户不一样的情况
                    if (nmpUsedConfirm.UserId == user.Id)
                    {
                        nmpUsedConfirm.RealFee = nmpUsedConfirm.CostDeduct == null ? 0d : nmpUsedConfirm.CostDeduct.RealCurrency.Value + nmpUsedConfirm.CostDeduct.VirtualCurrency.Value;
                    }
                    else
                    {
                        UserAccount userAccountTemp = null;
                        PaymentMethod paymentMethodTemp = PaymentMethod.SubjectDirectorPay;
                        var subject = _subjectBLL.GetUserSelfJoinSubject(user.Id);
                        if (subject == null) throw new Exception(string.Format("找不到用户【{0}】所属的课题组", user.UserName));
                        _userBLL.GetPayer(user.Id, subject.Id, out paymentMethodTemp, out userAccountTemp);
                        if (nmpUsedConfirm.CostDeduct != null)
                        {
                            errorMessage = "";
                            userAccount = _userAccountBLL.CancelDeduct(nmpUsedConfirm.UserId, nmpUsedConfirm.SubjectId.Value, nmpUsedConfirm.PaymentMethodEnum, nmpUsedConfirm.CostDeduct.RealCurrency.Value, nmpUsedConfirm.CostDeduct.VirtualCurrency.Value, ref tran, out errorMessage);
                            if (!string.IsNullOrWhiteSpace(errorMessage)) throw new Exception(errorMessage);
                            _costDeductBLL.Delete(new Guid[] { nmpUsedConfirm.CostDeduct.Id }, ref tran, true);
                        }
                        nmpUsedConfirm.UserId = user.Id;
                        nmpUsedConfirm.SubjectId = subject.Id;
                        nmpUsedConfirm.RealFee = 0d;
                    }
                    if (nmpUsedConfirm.EndAt.HasValue && nmpUsedConfirm.EndAt.Value > nmpUsedConfirm.BeginAt.Value && nmpUsedConfirm.Status == (int)UsedConfirmStatus.UnComplete) nmpUsedConfirm.Status = (int)UsedConfirmStatus.Complete;
                    _nmpUsedConfirmBLL.Save(new NMPUsedConfirm[] { nmpUsedConfirm }, ref tran, true);
                }
                SaveNMPUsedConfirmFeedBack(nmpUsedConfirm, ref tran, true);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress)tran.Dispose(); }
            return nmpUsedConfirm;
        }
        protected NMPUsedConfirm ChangeUseTime(Guid? operatorId, NMPUsedConfirm nmpUsedConfirm, User user, DateTime startAt, DateTime endAt, ref XTransaction tran, bool isDeduct)
        {
            string errorMessage = "";
            XTransaction tranTemp = SessionManager.Instance.BeginTransaction();
            UserAccount userAccount = null;
            try
            {
                if (nmpUsedConfirm.CostDeduct != null)
                {
                    userAccount = _userAccountBLL.CancelDeduct(nmpUsedConfirm.UserId, nmpUsedConfirm.SubjectId.Value, nmpUsedConfirm.PaymentMethodEnum, nmpUsedConfirm.CostDeduct.RealCurrency.Value, nmpUsedConfirm.CostDeduct.VirtualCurrency.Value, ref tranTemp, out errorMessage);
                    _costDeductBLL.Delete(new Guid[] { nmpUsedConfirm.CostDeduct.Id }, ref tranTemp, null, "", true);
                }
                var willDelNMPUsedConfirmFeedBack = _usedconfirmFeedBackBLL.GetFirstOrDefaultEntityByExpression(string.Format("UsedConfirmId=\"{0}\"", nmpUsedConfirm.Id));
                if (willDelNMPUsedConfirmFeedBack != null)
                {
                    if (_nmpUsedConfirmFeedBack != null) _nmpUsedConfirmFeedBack.Delete();//修改对象属性
                    _usedconfirmFeedBackBLL.Delete(new Guid[] { willDelNMPUsedConfirmFeedBack.Id }, ref tranTemp, true);
                }

                _nmpUsedConfirmBLL.Delete(new Guid[] { nmpUsedConfirm.Id }, ref tranTemp, true);
                if (!string.IsNullOrWhiteSpace(errorMessage)) throw new Exception(errorMessage);
                PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
                var subject = _subjectBLL.GetUserSelfJoinSubject(user.Id);
                if (subject == null) throw new Exception(string.Format("找不到用户【{0}】所属的课题组", user.UserName));
                _userBLL.GetPayer(user.Id, subject.Id, out paymentMethod, out userAccount);
                tranTemp.CommitTransaction();
            }
            catch (Exception ex)
            {
                tranTemp.RollbackTransaction();
                throw;
            }
            finally { tranTemp.Dispose(); }
            return AddNMPUsedConfirm(operatorId, user, userAccount, startAt, endAt, "", null, ref tran, isDeduct);
        }
        protected void GenerateNMPUsedConfirmRelativeFeedBack(NMPUsedConfirm nmpUsedConfirm)
        {
            if (nmpUsedConfirm != null)
            {
                if (_preNMPUsedConfirmFeedBack == null)
                {
                    _preNMPUsedConfirmFeedBack = _nmpUsedconfirmFeedBackBLL.GetFirstOrDefaultEntityByExpression(string.Format("NMPUsedConfirmId=\"{0}\"", nmpUsedConfirm.Id));
                }
                if (_nmpUsedConfirmFeedBack == null) 
                {
                    nmpUsedConfirm.FeedBack = _preNMPUsedConfirmFeedBack;
                }
                else
                {
                    nmpUsedConfirm.FeedBack = _nmpUsedConfirmFeedBack;
                }
                nmpUsedConfirm.SetIsFeedBack(nmpUsedConfirm.FeedBack);
            }
        }
        protected void SaveNMPUsedConfirmFeedBack(NMPUsedConfirm nmpUsedConfirm, ref XTransaction tran, bool isSupress) 
        {
            if (nmpUsedConfirm == null || nmpUsedConfirm.FeedBack == null) return;
            if (nmpUsedConfirm.FeedBack.State != ObjectState.Deleted)
            {
                if (_preNMPUsedConfirmFeedBack != null)
                {
                    if ((nmpUsedConfirm.FeedBack.StatusEnum == UsedConfirmFeedBackStatus.SystemAuto || nmpUsedConfirm.FeedBack.StatusEnum == UsedConfirmFeedBackStatus.UserUnLogOff) &&
                         nmpUsedConfirm.FeedBack.StatusEnum == _preNMPUsedConfirmFeedBack.StatusEnum) return;
                    Guid preUsedConfirmFeedBackId = _preNMPUsedConfirmFeedBack.Id;
                    _preNMPUsedConfirmFeedBack.CreateTime = DateTime.Now;
                    _preNMPUsedConfirmFeedBack.CreatorId = nmpUsedConfirm.UserId;
                    _preNMPUsedConfirmFeedBack.Remark = nmpUsedConfirm.FeedBack.Remark;
                    _preNMPUsedConfirmFeedBack.StatusEnum = nmpUsedConfirm.FeedBack.StatusEnum;
                    _preNMPUsedConfirmFeedBack.NMPUsedConfirmId = nmpUsedConfirm.Id;
                    _preNMPUsedConfirmFeedBack.BeforeUseStatusEnum = nmpUsedConfirm.FeedBack.BeforeUseStatusEnum;
                    _preNMPUsedConfirmFeedBack.BeforeUseRemark = nmpUsedConfirm.FeedBack.BeforeUseRemark;
                    _nmpUsedconfirmFeedBackBLL.Save(new NMPUsedConfirmFeedBack[] { _preNMPUsedConfirmFeedBack }, ref tran, isSupress);
                    return;
                }
            }
            nmpUsedConfirm.FeedBack.Id = Guid.NewGuid();
            nmpUsedConfirm.FeedBack.New();//修改回对象属性
            nmpUsedConfirm.FeedBack.CreateTime = DateTime.Now;
            nmpUsedConfirm.FeedBack.CreatorId = nmpUsedConfirm.UserId;
            nmpUsedConfirm.FeedBack.NMPUsedConfirmId = nmpUsedConfirm.Id;
            _nmpUsedconfirmFeedBackBLL.Add(new NMPUsedConfirmFeedBack[] { nmpUsedConfirm.FeedBack }, ref tran, isSupress);
        }
    }
}
