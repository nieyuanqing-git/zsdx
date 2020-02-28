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
    public abstract class UsedConfirmRelativeCostDeduct : CostDeductBase
    {
        protected IEquipmentBLL _equipmentBLL;
        protected ISubjectBLL _subjectBLL;
        protected IAppointmentBLL _appointmentBLL;
        protected IUsedConfirmBLL _usedConfirmBLL;
        protected IChargeStandardBLL _chargeStandardBLL;
        protected ICalcFeeTimeRuleBLL _calcFeeTimeRuleBLL;
        /// 使用记录反馈
        /// <summary>
        /// 使用记录反馈
        /// </summary>
        protected UsedConfirmFeedBack _usedConfirmFeedBack;
        /// 上一次使用记录反馈
        /// <summary>
        /// 上一次使用记录反馈
        /// </summary>
        protected UsedConfirmFeedBack _preUsedConfirmFeedBack;
        protected IOpenFundApplyBLL _openFundApplyBLL;
        /// 开放基金扣费设备额集合
        /// <summary>
        /// 开放基金扣费设备额集合
        /// </summary>
        protected IList<CostDeductEquipmentOpenFund> _preCostDeductEquipmentOpenFunds;
        public UsedConfirmRelativeCostDeduct(object[] businessObjects, Guid? operatorId, string operateIP)
            : base(businessObjects, operatorId, operateIP)
        {
            _subjectBLL = BLLFactory.CreateSubjectBLL();
            _appointmentBLL = BLLFactory.CreateAppointmentBLL();
            _usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
            _chargeStandardBLL = BLLFactory.CreateChargeStandardBLL();
            _calcFeeTimeRuleBLL = BLLFactory.CreateCalcFeeTimeRuleBLL();
            _equipmentBLL = BLLFactory.CreateEquipmentBLL();
            _openFundApplyBLL = BLLFactory.CreateOpenFundApplyBLL();
        }
        public override abstract object[] Deduct(ref august.DataLink.XTransaction tran);
        public override object[] CancelDeduct(ref august.DataLink.XTransaction tran) { return null; }
        protected abstract UsedConfirm CreateUseConfirm(out string errorMessage);
        protected UsedConfirm AddUsedConfirm(Guid? operatorId, User user, UserAccount userAccount, DateTime beginTime, DateTime endTime, string remark, double? realFee, ref august.DataLink.XTransaction tran, bool isDeduct)
        {
            string errorMessage = "";
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            var usedConfirm = CreateUseConfirm(out errorMessage);
            if (realFee.HasValue) usedConfirm.RealFee = realFee.Value;//保存用户输入的实际费用
            double fee = 0d;
            var count = _usedConfirmBLL.CountModelListByExpression(string.Format("Id=\"{0}\"", usedConfirm.Id));
            if (user.Id != usedConfirm.UserId)
            {
                usedConfirm.SubjectId = _subjectBLL.GetUserSelfJoinSubject(user.Id).Id;
                usedConfirm.UserId = user.Id;
            }
            usedConfirm.Remark = remark;
            try
            {
                bool isAddedOrSavedUsedConfirm = false, isAppointmentPredictCostDeduct = false;
                bool isEnableCostDeduct = JudgeIsEnableCostDeduct(usedConfirm, userAccount, isDeduct, out errorMessage);
                if (!isDeduct || !isEnableCostDeduct) usedConfirm.RealFee = 0d;
                usedConfirm.WhyNotCostDeduct = errorMessage;
                //删除预扣费记录
                if (usedConfirm.AppointmentId.HasValue)
                {
                    var appointment = usedConfirm.GetAppointment();
                    var predictCostDeduct = _costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"*CostDeductType={1}", usedConfirm.AppointmentId.Value, (int)CostDeductType.AppointmentPredictCostDeduct));
                    if (predictCostDeduct != null)
                    {
                        isAppointmentPredictCostDeduct = true;
                        userAccount.RealCurrency += predictCostDeduct.RealCurrency.Value;
                        userAccount.VirtualCurrency += predictCostDeduct.VirtualCurrency.Value;
                        fee = fee - predictCostDeduct.Fee;
                        _costDeductBLL.Delete(new Guid[] { predictCostDeduct.Id }, ref tran, operatorId, "", true, !isEnableCostDeduct || !isDeduct || appointment.SubjectId.Value != usedConfirm.SubjectId.Value);
                    }
                }
                if (isEnableCostDeduct)
                {
                    IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
                    bool isEquipmentOpenFundDiscount = false;
                    double? realEquipmentOpenFundDiscout = null, realEquipmentOpenFundDiscoutTemp = null;
                    ReCalUsedConfirmFee(usedConfirm, realFee, out realEquipmentOpenFundDiscout);
                    var calFee = _openFundApplyBLL.CalEquipmentOpenFundFee(usedConfirm.RealFee.Value, null, usedConfirm.EndAt.Value, usedConfirm.EquipmentId, usedConfirm.Subject.SubjectDirectorId.Value, _preCostDeductEquipmentOpenFunds, out isEquipmentOpenFundDiscount, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscoutTemp);
                    usedConfirm.RealFee = calFee;
                    usedConfirm.IsOpenFundCostDeduct = isEquipmentOpenFundDiscount;
                    double realCurrency = 0d, virtualCurrency = 0d;
                    fee += usedConfirm.RealFee.Value;
                    if (count == 0)
                    {
                        _usedConfirmBLL.Add(new UsedConfirm[] { usedConfirm }, ref tran, isSupress);
                    }
                    else
                    {
                        _usedConfirmBLL.Save(new UsedConfirm[] { usedConfirm }, ref tran, isSupress);
                    }
                    isAddedOrSavedUsedConfirm = true;
                    if (isDeduct)
                    {
                        userAccount.Deduct(isEquipmentOpenFundDiscount, usedConfirm.RealFee.Value, out realCurrency, out virtualCurrency);
                        usedConfirm.CostDeduct = new CostDeduct() { Id = Guid.NewGuid(), UserAccountId = userAccount.Id, UsedConfirmId = usedConfirm.Id, CreaterId = operatorId, DeductAt = DateTime.Now, OpenFundDiscount = null };
                        usedConfirm.CostDeduct.CalcDuration = usedConfirm.CalcDuration;
                        usedConfirm.CostDeduct.Duration = (usedConfirm.EndAt.Value - usedConfirm.BeginAt.Value).TotalHours;
                        usedConfirm.CostDeduct.VirtualCurrency = virtualCurrency;
                        usedConfirm.CostDeduct.RealCurrency = realCurrency;
                        usedConfirm.CostDeduct.CreaterId = operatorId;
                        usedConfirm.CostDeduct.IsOpenFundCostDeduct = isEquipmentOpenFundDiscount;
                        if (usedConfirm.CostDeduct.IsOpenFundCostDeduct) usedConfirm.CostDeduct.OpenFundDiscount = _openFundApplyBLL.GetEquipmentOpenFundDiscount();
                        if (costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Count > 0)
                        {
                            if (usedConfirm.CostDeduct.CostDeductEquipmentOpenFunds == null)
                                usedConfirm.CostDeduct.CostDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
                            foreach (var costDeductEquipmentOpenFund in costDeductEquipmentOpenFunds)
                                usedConfirm.CostDeduct.CostDeductEquipmentOpenFunds.Add(costDeductEquipmentOpenFund);
                        }
                        usedConfirm.CreaterId = operatorId;
                        usedConfirm.WhyNotCostDeduct = "";
                        if (usedConfirm.AppointmentId.HasValue) usedConfirm.CostDeduct.AppointmentId = usedConfirm.AppointmentId.Value;
                        usedConfirm.Status = (int)UsedConfirmStatus.Deduct;
                        usedConfirm.CostDeduct.UsedConfirmForLog = usedConfirm;
                        _costDeductBLL.Add(null, new CostDeduct[] { usedConfirm.CostDeduct }, _preCostDeductEquipmentOpenFunds, ref tran, isSupress);
                    }
                }
                else if (usedConfirm.CostDeduct != null)
                {
                    usedConfirm.RealFee = usedConfirm.CostDeduct.VirtualCurrency.Value + usedConfirm.CostDeduct.RealCurrency.Value;
                }
                if ((isEnableCostDeduct && isDeduct) || isAppointmentPredictCostDeduct)
                {
                    _subjectBLL.Deduct(usedConfirm.UserId, usedConfirm.SubjectId.Value, usedConfirm.PaymentMethodEnum, fee, ref tran);
                    _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, isSupress);

                }
                if (!isAddedOrSavedUsedConfirm)
                {
                    if (usedConfirm.EndAt.HasValue && usedConfirm.EndAt.Value > usedConfirm.BeginAt.Value && usedConfirm.Status == (int)UsedConfirmStatus.UnComplete) usedConfirm.Status = (int)UsedConfirmStatus.Complete;
                    if (!usedConfirm.RealFee.HasValue) usedConfirm.RealFee = 0d;
                    if (count == 0)
                    {
                        _usedConfirmBLL.Add(new UsedConfirm[] { usedConfirm }, ref tran, isSupress);
                    }
                    else
                    {
                        _usedConfirmBLL.Save(new UsedConfirm[] { usedConfirm }, ref tran, isSupress);
                    }
                }
                //保存使用反馈
                SaveUsedConfirmFeedBack(usedConfirm, ref tran, isSupress);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress)tran.Dispose(); }
            return usedConfirm;
        }

        protected bool JudgeIsEnableCostDeduct(UsedConfirm usedConfirm, UserAccount userAccount, bool isDeduct, out string errorMessage)
        {
            return base.JudgeIsEnableCostDeduct(usedConfirm.SubjectId,
                                                  userAccount,
                                                  isDeduct,
                                                  usedConfirm.BeginAt,
                                                  usedConfirm.EndAt,
                                                  usedConfirm.Equipment.ChargeStandard,
                                                  usedConfirm.Equipment.CalcFeeTimeRule,
                                                  (UsedConfirmStatus)usedConfirm.Status,
                                                  usedConfirm.Equipment.MinUnchangeMinutes,
                                                  out errorMessage);
        }
        protected void ReCalUsedConfirmFee(UsedConfirm usedConfirm, double? realFee, out double? realEquipmentOpenFundDiscout)
        {
            realEquipmentOpenFundDiscout = null;
            if (realFee == null)
            {
                bool isEquipmentOpenFundDiscount = false;
                double duration = 0d, openFundFee = 0d, predictFee = 0d, totalFee = 0d;
                _usedConfirmBLL.CalUsedConfirmFee(usedConfirm, out duration, out predictFee, out openFundFee, out totalFee, out isEquipmentOpenFundDiscount);//_chargeStandardBLL.GetUsedConfirmCalFee(usedConfirm, out duration, out unitPrice, out isEquipmentOpenFundDiscount, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscout);
                usedConfirm.CalcDuration = duration;
            }
        }
        protected UsedConfirm UpdateUseTime(Guid? operatorId, UsedConfirm usedConfirm, User user, DateTime startAt, DateTime endAt, double? realFee, ref XTransaction tran, UsedConfirmFeedBack usedConfirmFeedBack, bool isDeduct = true)
        {
            string errorMessage = "";
            var isSupress = tran != null;
            try
            {
                usedConfirm.BeginAt = usedConfirm.BeginAt < startAt ? usedConfirm.BeginAt : startAt;
                usedConfirm.EndAt = usedConfirm.EndAt > endAt ? usedConfirm.EndAt : endAt;
                var remark = string.Format("{0:yyyy-MM-dd HH:mm}~{1:yyyy-MM-dd HH:mm};", startAt, endAt);
                usedConfirm.Remark += (usedConfirm.Remark.IndexOf(remark) == -1 ? remark : "");
                _usedConfirmBLL.GenerateRelativeAppointmentUsedConfirm(usedConfirm.GetAppointment(), usedConfirm);
                GenerateUsedConfirmRelativeFeedBack(usedConfirm);
                if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
                PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
                UserAccount userAccount = null, userAccountForValidate = null;
                var costDeduct = usedConfirm.CostDeduct;
                if (string.IsNullOrEmpty(usedConfirm.Remark))
                {
                    usedConfirm.Remark += string.Format("系统自动合成以下使用时间:{0:yyyy-MM-dd HH:mm}~{1:yyyy-MM-dd HH:mm};", usedConfirm.BeginAt, usedConfirm.EndAt);
                }
                if (usedConfirm.Subject != null && usedConfirm.Subject.Director != null && usedConfirm.Subject.Director.UserAccount != null)
                {
                    userAccountForValidate = usedConfirm.Subject.Director.UserAccount;
                }
                if (_usedConfirmFeedBack != null && _usedConfirmFeedBack.SampleNum.HasValue && _usedConfirmFeedBack.SampleNum.Value > 0)
                {
                    usedConfirm.SampleCount = _usedConfirmFeedBack.SampleNum.Value;
                }
                bool isEnableCostDeduct = JudgeIsEnableCostDeduct(usedConfirm, userAccountForValidate, isDeduct, out errorMessage);
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    if (usedConfirm.WhyNotCostDeduct == null) usedConfirm.WhyNotCostDeduct = "";
                    usedConfirm.WhyNotCostDeduct += usedConfirm.WhyNotCostDeduct.IndexOf(errorMessage) == -1 ? " " + errorMessage : "";
                }
                if (isDeduct && isEnableCostDeduct)
                {

                    if (costDeduct != null)
                    {
                        if (usedConfirm.UserId == user.Id)
                        {
                            bool isEquipmentOpenFundDiscount = false;
                            double? realEquipmentOpenFundDiscout = null, realEquipmentOpenFundDiscoutTemp = null;
                            IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
                            ReCalUsedConfirmFee(usedConfirm, realFee, out realEquipmentOpenFundDiscout);
                            if (realFee.HasValue) usedConfirm.RealFee = realFee.Value;//保存用户输入的实际费用
                            usedConfirm.RealFee = _openFundApplyBLL.CalEquipmentOpenFundFee(usedConfirm.RealFee.Value, null, usedConfirm.EndAt.Value, usedConfirm.EquipmentId, usedConfirm.Subject.SubjectDirectorId.Value, costDeduct.CostDeductEquipmentOpenFunds, out isEquipmentOpenFundDiscount, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscoutTemp);
                            usedConfirm.Status = (int)UsedConfirmStatus.Deduct;
                            usedConfirm.IsOpenFundCostDeduct = isEquipmentOpenFundDiscount;
                            usedConfirm.WhyNotCostDeduct = "";
                            var fee = usedConfirm.RealFee.Value - costDeduct.RealCurrency.Value + costDeduct.VirtualCurrency.Value;
                            userAccount = _subjectBLL.Deduct(usedConfirm.UserId, usedConfirm.SubjectId.Value, usedConfirm.PaymentMethodEnum, fee, ref tran);
                            userAccount.RealCurrency += costDeduct.RealCurrency.Value;
                            userAccount.VirtualCurrency += costDeduct.VirtualCurrency.Value;

                            double virtualCurrency = 0d, realCurrency = 0d;
                            userAccount.Deduct(isEquipmentOpenFundDiscount, usedConfirm.RealFee.Value, out realCurrency, out virtualCurrency);
                            costDeduct.RealCurrency = realCurrency;
                            costDeduct.VirtualCurrency = virtualCurrency;
                            costDeduct.CalcDuration = usedConfirm.CalcDuration;
                            costDeduct.Duration = usedConfirm.Duration;
                            costDeduct.IsOpenFundCostDeduct = isEquipmentOpenFundDiscount;
                            costDeduct.OpenFundDiscount = null;
                            if (costDeduct.IsOpenFundCostDeduct) costDeduct.OpenFundDiscount = _openFundApplyBLL.GetEquipmentOpenFundDiscount();
                            costDeduct.CreaterId = operatorId;

                            if (costDeduct.CostDeductEquipmentOpenFunds == null) costDeduct.CostDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
                            if (costDeduct.CostDeductEquipmentOpenFunds != null && costDeduct.CostDeductEquipmentOpenFunds.Count > 0)
                            {
                                foreach (var costDeductEquipmentOpenFund in costDeduct.CostDeductEquipmentOpenFunds)
                                {
                                    costDeductEquipmentOpenFund.Delete();
                                }
                            }
                            if (costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Count > 0)
                            {
                                foreach (var costDeductEquipmentOpenFund in costDeductEquipmentOpenFunds)
                                {
                                    costDeduct.CostDeductEquipmentOpenFunds.Add(costDeductEquipmentOpenFund);
                                }
                            }
                            if (usedConfirm.AppointmentId.HasValue) costDeduct.AppointmentId = usedConfirm.AppointmentId.Value;
                            costDeduct.UsedConfirmForLog = usedConfirm;
                            _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, true);
                            _costDeductBLL.Save(new CostDeduct[] { costDeduct }, ref tran, true);
                            _usedConfirmBLL.Save(new UsedConfirm[] { usedConfirm }, ref tran, true);
                        }
                        else
                        {
                            //前后用户不一样
                            return ChangeUseTime(null, usedConfirm, user, startAt, endAt, ref tran, isDeduct);
                        }
                    }
                    else
                    {
                        var subjectId = usedConfirm.UserId != user.Id ? _subjectBLL.GetUserSelfJoinSubject(user.Id).Id : usedConfirm.SubjectId.Value;
                        _userBLL.GetPayer(user.Id, subjectId, out paymentMethod, out userAccount);
                        if (_preUsedConfirmFeedBack != null)
                        {
                            _usedconfirmFeedBackBLL.Delete(new Guid[] { _preUsedConfirmFeedBack.Id }, ref tran, true);
                            _preUsedConfirmFeedBack.Delete();
                            if (usedConfirm.FeedBack != null) usedConfirm.FeedBack.Delete();//修改对象状态
                        }
                        var count = _usedConfirmBLL.CountModelListByExpression(string.Format("Id=\"{0}\"", usedConfirm.Id));
                        if (count == 0)
                        {
                            _usedConfirmBLL.Delete(new Guid[] { usedConfirm.Id }, ref tran, true);
                        }
                        return AddUsedConfirm(operatorId, user, userAccount, usedConfirm.BeginAt.Value, usedConfirm.EndAt.Value, usedConfirm.Remark, realFee, ref tran, isDeduct);
                    }
                }
                else
                {
                    //考虑前后用户不一样的情况
                    if (usedConfirm.UserId == user.Id)
                    {
                        usedConfirm.RealFee = usedConfirm.CostDeduct == null ? 0d : usedConfirm.CostDeduct.RealCurrency.Value + usedConfirm.CostDeduct.VirtualCurrency.Value;
                    }
                    else
                    {
                        UserAccount userAccountTemp = null;
                        PaymentMethod paymentMethodTemp = PaymentMethod.SubjectDirectorPay;
                        var subject = _subjectBLL.GetUserSelfJoinSubject(user.Id);
                        if (subject == null) throw new Exception(string.Format("找不到用户【{0}】所属的课题组", user.UserName));
                        _userBLL.GetPayer(user.Id, subject.Id, out paymentMethodTemp, out userAccountTemp);
                        if (usedConfirm.CostDeduct != null)
                        {
                            //errorMessage = "";
                            //userAccount = _userAccountBLL.CancelDeduct(usedConfirm.UserId, usedConfirm.SubjectId.Value, usedConfirm.PaymentMethodEnum, usedConfirm.CostDeduct.RealCurrency.Value, usedConfirm.CostDeduct.VirtualCurrency.Value, ref tran, out errorMessage);
                            if (!string.IsNullOrWhiteSpace(errorMessage)) throw new Exception(errorMessage);
                            _costDeductBLL.Delete(new Guid[] { usedConfirm.CostDeduct.Id }, ref tran, true);
                        }
                        usedConfirm.UserId = user.Id;
                        usedConfirm.SubjectId = subject.Id;
                        usedConfirm.RealFee = 0d;
                    }
                    if (usedConfirm.EndAt.HasValue && usedConfirm.EndAt.Value > usedConfirm.BeginAt.Value && usedConfirm.Status == (int)UsedConfirmStatus.UnComplete) usedConfirm.Status = (int)UsedConfirmStatus.Complete;
                    _usedConfirmBLL.Save(new UsedConfirm[] { usedConfirm }, ref tran, true);
                }
                SaveUsedConfirmFeedBack(usedConfirm, ref tran, true);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress)tran.Dispose(); }

            return usedConfirm;
        }

        protected UsedConfirm ChangeUseTime(Guid? operatorId, UsedConfirm usedConfirm, User user, DateTime startAt, DateTime endAt, ref XTransaction tran, bool isDeduct)
        {
            string errorMessage = "";
            XTransaction tranTemp = SessionManager.Instance.BeginTransaction();
            UserAccount userAccount = null;
            try
            {
                if (usedConfirm.CostDeduct != null)
                {
                    //userAccount = _userAccountBLL.CancelDeduct(usedConfirm.UserId, usedConfirm.SubjectId.Value, usedConfirm.PaymentMethodEnum, usedConfirm.CostDeduct.RealCurrency.Value, usedConfirm.CostDeduct.VirtualCurrency.Value, ref tranTemp, out errorMessage);
                    _costDeductBLL.Delete(new Guid[] { usedConfirm.CostDeduct.Id }, ref tranTemp, null, "", true);
                }
                var willDelusedConfirmFeedBack = _usedconfirmFeedBackBLL.GetFirstOrDefaultEntityByExpression(string.Format("UsedConfirmId=\"{0}\"", usedConfirm.Id));
                if (willDelusedConfirmFeedBack != null)
                {
                    if (_usedConfirmFeedBack != null) _usedConfirmFeedBack.Delete();//修改对象属性
                    _usedconfirmFeedBackBLL.Delete(new Guid[] { willDelusedConfirmFeedBack.Id }, ref tranTemp, true);
                }

                _usedConfirmBLL.Delete(new Guid[] { usedConfirm.Id }, ref tranTemp, true);
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
            return AddUsedConfirm(operatorId, user, userAccount, startAt, endAt, "", null, ref tran, isDeduct);
        }
        protected void GenerateUsedConfirmRelativeFeedBack(UsedConfirm usedConfirm)
        {
            if (usedConfirm != null)
            {
                if (_preUsedConfirmFeedBack == null)
                    _preUsedConfirmFeedBack = _usedconfirmFeedBackBLL.GetFirstOrDefaultEntityByExpression(string.Format("UsedConfirmId=\"{0}\"", usedConfirm.Id));
                if (_usedConfirmFeedBack == null)
                {
                    usedConfirm.FeedBack = _preUsedConfirmFeedBack;
                }
                else
                {
                    usedConfirm.FeedBack = _usedConfirmFeedBack;
                }
                usedConfirm.SetIsFeedBack(usedConfirm.FeedBack);
            }
        }
        protected void SaveUsedConfirmFeedBack(UsedConfirm usedConfirm, ref XTransaction tran, bool isSupress)
        {
            if (usedConfirm == null || usedConfirm.FeedBack == null) return;
            if (usedConfirm.FeedBack.State != ObjectState.Deleted)
            {
                if (_preUsedConfirmFeedBack != null)
                {
                    if ((usedConfirm.FeedBack.StatusEnum == UsedConfirmFeedBackStatus.SystemAuto || usedConfirm.FeedBack.StatusEnum == UsedConfirmFeedBackStatus.UserUnLogOff) &&
                         usedConfirm.FeedBack.StatusEnum == _preUsedConfirmFeedBack.StatusEnum) return;
                    Guid preUsedConfirmFeedBackId = _preUsedConfirmFeedBack.Id;
                    _preUsedConfirmFeedBack.CreateTime = DateTime.Now;
                    _preUsedConfirmFeedBack.CreatorId = usedConfirm.UserId;
                    _preUsedConfirmFeedBack.IsSample = usedConfirm.FeedBack.IsSample;
                    _preUsedConfirmFeedBack.Remark = usedConfirm.FeedBack.Remark;
                    _preUsedConfirmFeedBack.SampleNum = usedConfirm.FeedBack.SampleNum;
                    _preUsedConfirmFeedBack.StatusEnum = usedConfirm.FeedBack.StatusEnum;
                    _preUsedConfirmFeedBack.UsedConfirmId = usedConfirm.Id;
                    _preUsedConfirmFeedBack.BeforeUseStatusEnum = usedConfirm.FeedBack.BeforeUseStatusEnum;
                    _preUsedConfirmFeedBack.BeforeUseRemark = usedConfirm.FeedBack.BeforeUseRemark;
                    _usedconfirmFeedBackBLL.Save(new UsedConfirmFeedBack[] { _preUsedConfirmFeedBack }, ref tran, isSupress);
                    return;
                }
            }
            usedConfirm.FeedBack.Id = Guid.NewGuid();
            usedConfirm.FeedBack.New();//修改回对象属性
            usedConfirm.FeedBack.CreateTime = DateTime.Now;
            usedConfirm.FeedBack.CreatorId = usedConfirm.UserId;
            usedConfirm.FeedBack.UsedConfirmId = usedConfirm.Id;
            _usedconfirmFeedBackBLL.Add(new UsedConfirmFeedBack[] { usedConfirm.FeedBack }, ref tran, isSupress);
        }
        protected void SendErrorChargedTypeNotice(UsedConfirm usedConfirm)//处理扣费方式是按照样品数扣费，直接授权非电脑控制器无法输入样品数，先以使用机时扣费，发消息通知设备管理进行纠正
        {
            _messageHandler.SendErrorChargedTypeNotice(usedConfirm, null);
        }
    }
}
