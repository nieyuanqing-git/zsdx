using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using log4net;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary>
    /// 账户余额校验类型
    /// </summary>
    public enum MoneyValidateType
    {
        /// <summary>
        /// 预约余额校验
        /// </summary>
        AppointmentMoneyValidate,

        /// <summary>
        /// 送样余额校验
        /// </summary>
        SampleApplyMoneyValidate,

        /// <summary>
        /// 使用余额校验，一般用在刷卡上机
        /// </summary>
        UseMoneyValidate,

        /// <summary>
        /// 工位预约余额校验
        /// </summary>
        NMPAppointmentMoneyValidate
    }

    public abstract class MoneyValidateBLL : IMoneyValidateBLL
    {
        private readonly IAppointmentBLL __appointmentBLL;
        private readonly IChargeStandardBLL __chargeStandardBLL;
        private readonly IEquipmentBLL __equipmentBLL;
        protected readonly ISubjectBLL __subjectBLL;
        private readonly IUsedConfirmBLL __usedConfirmBLL;
        private readonly IUserBLL __userBLL;
        private ISampleApplyBLL __sampleApplyBLL;
        protected IViewAppointmentBLL __viewAppointmentBLL;
        protected IViewSampleApplyBLL __viewSampleApplyBLL;
        protected readonly ILog log = LogManager.GetLogger(typeof(MoneyValidateBLL));

        public MoneyValidateBLL()
        {
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __appointmentBLL = BLLFactory.CreateAppointmentBLL();
            __sampleApplyBLL = BLLFactory.CreateSampleApplyBLL();
            __chargeStandardBLL = BLLFactory.CreateChargeStandardBLL();
            __viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
            __viewSampleApplyBLL = BLLFactory.CreateViewSampleApplyBLL();
            __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
        }

        #region IMoneyValidateBLL Members

        public bool TutorSimpleMoneyValidate(Guid userId, double money, out string errorMessage)
        {
            errorMessage = "";
            User user = __userBLL.GetEntityById(userId);
            if (user == null)
            {
                errorMessage = string.Format("找不到编码为【{0}】的用户信息", userId.ToString());
                return false;
            }
            User tutor = user.UserType.UserIdentityEnum == UserIdentity.Tutor ||
                         user.UserType.UserIdentityEnum == UserIdentity.OutTutor
                             ? user
                             : user.Tutor;
            if (tutor == null)
            {
                errorMessage = "导师信息为空";
                return false;
            }
            if (tutor.UserAccount == null)
            {
                errorMessage = "导师账户为空";
                return false;
            }
            Subject subject = __subjectBLL.GetUserSelfJoinSubject(tutor.Id);
            if (subject == null)
            {
                errorMessage = "找不到课题组信息";
                return false;
            }
            ExperimenterSubject experimentSubject = subject.Experiments == null
                                                        ? null
                                                        : subject.Experiments.FirstOrDefault(
                                                            p => p.ExperimenterId == userId && !p.IsDelete);
            if (experimentSubject != null &&
                experimentSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
            {
                if (experimentSubject.UnUseable > experimentSubject.OddSum - money)
                {
                    errorMessage =
                        string.Format(@"余额提醒:\r\n您的课题经费余额不足!您的课题经费余额:{0}元,\r\n可能产生的费用:{1}元,\r\n经费余额不可使用值:{2}元.",
                                      experimentSubject.OddSum,
                                      money,
                                      experimentSubject.UnUseable);
                    return false;
                }
                if (experimentSubject.Unappointment > experimentSubject.OddSum - money)
                {
                    errorMessage =
                        string.Format(@"余额提醒:\r\n您的课题经费余额不足!您的课题经费余额:{0}元,\r\n可能产生的费用:{1}元,\r\n经费余额不可预约值:{2}元.",
                                      experimentSubject.OddSum,
                                      money,
                                      experimentSubject.Unappointment);
                    return false;
                }
            }
            else
            {
                var usnUseable = tutor.UserAccount.UnUseable ?? tutor.UserAccount.CreditLimit.UnUseable;
                var unAppointment = tutor.UserAccount.UnAppointment ?? tutor.UserAccount.CreditLimit.UnAppointment;

                ZsdxAccountBLL __accountBLL = new ZsdxAccountBLL();
                var xjAccount = __accountBLL.GetAccount(tutor.Label) as XjDsAccountResult;
                var xjBalance = xjAccount == null ? 0 : xjAccount.balance;

                if (tutor.UserAccount.TotalCurrency - money < usnUseable)
                {
                    if (!ValidateOtherAccount(tutor, (usnUseable ?? 0) - (tutor.UserAccount.TotalCurrency - money)))
                    {
                        errorMessage =
                            string.Format(@"余额提醒:\r\n您的课题经费余额不足!您的课题经费余额:{0}元,\r\n可能产生的费用:{1}元,\r\n付费人帐户余额不可使用值:{2}元.",
                                          tutor.UserAccount.TotalCurrency,
                                          money,
                                          tutor.UserAccount.CreditLimit.UnUseable);
                        return false;
                    }
                }
                if (tutor.UserAccount.TotalCurrency - money < unAppointment)
                {
                    if (!ValidateOtherAccount(tutor, (unAppointment ?? 0) - (tutor.UserAccount.TotalCurrency - money)))
                    {
                        errorMessage =
                            string.Format(@"余额提醒:\r\n您的课题经费余额不足!您的课题经费余额:{0}元,\r\n可能产生的费用:{1}元,\r\n付费人帐户余额不可预约值:{2}元.",
                                          tutor.UserAccount.TotalCurrency,
                                          money,
                                          tutor.UserAccount.CreditLimit.UnAppointment);
                        return false;
                    }
                }
            }
            return true;
        }

        public virtual bool SimpleMoneyValidate(Guid userId, Guid subjectId, PaymentMethod paymentMethod, double money)
        {
            Subject subject = __subjectBLL.GetEntityById(subjectId);
            if (subject == null) throw new Exception(string.Format("找不到编码为【{0}】的课题信息", subjectId));
            ExperimenterSubject experimenterSubject = subject.Experiments == null
                                                          ? null
                                                          : subject.Experiments.FirstOrDefault(
                                                              p => p.ExperimenterId.Value == userId);
            if (subject.SubjectDirectorId != userId && experimenterSubject == null)
                throw new Exception("非课题组成员");
            User payer = subject.Director;

            var unAppointment = payer.UserAccount.UnAppointment ?? payer.UserAccount.CreditLimit.UnAppointment;

            if (unAppointment > (payer.UserAccount.TotalCurrency - money))
            {
                if (!ValidateOtherAccount(payer, (unAppointment ?? 0) - (payer.UserAccount.TotalCurrency - money)))
                {
                    throw new CheckMoneyFailException("付费人产生费用的总额超出其帐户不可预约值");
                }
            }
            if (experimenterSubject != null)
            {
                switch (paymentMethod)
                {
                    case PaymentMethod.SubjectDirectorPay:
                        if (experimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
                        {
                            if (experimenterSubject.OddSum < experimenterSubject.Unappointment ||
                                experimenterSubject.OddSum < experimenterSubject.UnUseable)
                                throw new CheckMoneyFailException("课题组分配经费余额不足");

                            if (experimenterSubject.Unappointment > (experimenterSubject.OddSum - money))
                                throw new CheckMoneyFailException("产生费用的总额超出课题组分配经费的不可预约值");
                        }
                        break;
                    case PaymentMethod.TutorSubjectFunds:
                        if (experimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign &&
                            experimenterSubject.Unappointment > (experimenterSubject.OddSum - money))
                            throw new CheckMoneyFailException("产生费用的总额超出课题组分配经费的不可预约值");
                        ExperimenterSubject tutorExperimenterSubject =
                            subject.Experiments.FirstOrDefault(
                                p => p.ExperimenterId == experimenterSubject.OwnerId.Value && !p.IsDelete);
                        if (tutorExperimenterSubject != null &&
                            tutorExperimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign &&
                            tutorExperimenterSubject.Unappointment > (tutorExperimenterSubject.OddSum - money))
                            throw new CheckMoneyFailException("产生费用的总额超出课题组分配给导师经费的不可预约值");
                        break;
                }
            }
            return true;
        }

        public bool SampleApplyMoneyValidate(Guid userId, Guid subjectId, PaymentMethod paymentMethod,
                                             IEnumerable<SampleApply> lstSampleApplies, double curSampleWillPayFee,
                                             out string errorMessage)
        {
            errorMessage = "";
            double experimenterWillPayFee = 0d;
            User user = __userBLL.GetEntityById(userId);
            Subject subject = __subjectBLL.GetEntityById(subjectId);
            ExperimenterSubject experimenterSubject = subject.Experiments == null
                                                          ? null
                                                          : subject.Experiments.FirstOrDefault(
                                                              p => p.ExperimenterId.Value == user.Id);
            if (experimenterSubject != null)
            {
                experimenterWillPayFee = CalCurSampleApplySubjectExperimenterWillPayFee(curSampleWillPayFee);
            }
            double willTotalPayFee = CalSampleApplyWillPayFee(subject, lstSampleApplies, curSampleWillPayFee);
            return MoneyValidate(MoneyValidateType.SampleApplyMoneyValidate, willTotalPayFee, user, subject,
                                 paymentMethod, experimenterSubject, experimenterWillPayFee, null, null,
                                 lstSampleApplies, curSampleWillPayFee, null, out errorMessage);
        }

        public bool AppointmentMoneyValidate(Guid userId, Guid subjectId, PaymentMethod paymentMethod,
                                             Appointment appointment, IEnumerable<Appointment> lstAppointments,
                                             double? minAccountBalance, out string errorMessage)
        {
            errorMessage = "";
            double experimenterWillPayFee = 0d;
            User user = __userBLL.GetEntityById(userId);
            Subject subject = __subjectBLL.GetEntityById(subjectId);
            ExperimenterSubject experimenterSubject = subject.Experiments == null
                                                          ? null
                                                          : subject.Experiments.FirstOrDefault(
                                                              p => p.ExperimenterId.Value == user.Id);
            if (experimenterSubject != null)
            {
                experimenterWillPayFee = CalCurAppointmentSubjectExperimenterWillPayFee(appointment, lstAppointments);
            }
            double willTotalPayFee = CalAppointmentWillPayFee(subject, appointment, lstAppointments);
            return MoneyValidate(MoneyValidateType.AppointmentMoneyValidate, willTotalPayFee, user, subject,
                                 paymentMethod, experimenterSubject, experimenterWillPayFee, appointment,
                                 lstAppointments, null, 0, minAccountBalance, out errorMessage);
        }

        public bool UseAppointmentMoneyValidate(string appointmentIdStr, string userLabel, string equipmentLabel,
                                                string tutorName, DateTime optTime, out string errorMessage)
        {
            errorMessage = "";
            double experimenterWillPayFee = 0d;
            if (string.IsNullOrWhiteSpace(userLabel))
            {
                errorMessage = "用户标识为空";
                return false;
            }
            User user =
                __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsDel=false",
                                                                            userLabel.Trim()));
            if (user == null)
            {
                errorMessage = string.Format("找不到标识为【{0}】的用户信息", userLabel.Trim());
                return false;
            }
            if (string.IsNullOrWhiteSpace(equipmentLabel))
            {
                errorMessage = "设备标识为空";
                return false;
            }
            Equipment equipment =
                __equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsDelete=false",
                                                                                 equipmentLabel.Trim()));
            if (equipment == null)
            {
                errorMessage = string.Format("找不到标识为【{0}】的设备信息", equipmentLabel.Trim());
                return false;
            }
            Appointment appointment = null;
            Guid? appointmentId = null;
            DateTime? beginTime = null, endTime = null;
            DateTime now = DateTime.Now;
            Subject subject = null;
            var paymentMethod = PaymentMethod.SubjectDirectorPay;
            if (!string.IsNullOrWhiteSpace(appointmentIdStr))
            {
                appointmentId = new Guid(appointmentIdStr.Trim());
                appointment = __appointmentBLL.GetEntityById(appointmentId.Value);
                if (appointment == null)
                {
                    errorMessage = string.Format("找不到标识为【{0}】的机时预约信息", equipmentLabel.Trim());
                    return false;
                }
                subject = appointment.Subject;
                paymentMethod = appointment.PaymentMethodEnum;
                if (optTime > appointment.EndTime.Value)
                {
                    beginTime = optTime > now ? now : optTime;
                    endTime = optTime > now ? optTime : now;
                }
                else
                {
                    beginTime = optTime;
                    endTime = appointment.EndTime.Value;
                }
            }
            else
            {
                if (user.TutorId.HasValue)
                {
                    subject = __subjectBLL.GetSubjectByTurtorId(user.TutorId.Value);
                    ExperimenterSubject experimenterSubjectFind = subject.Experiments == null
                                                                      ? null
                                                                      : subject.Experiments.FirstOrDefault(
                                                                          p => p.ExperimenterId.Value == user.Id);
                    if (experimenterSubjectFind != null &&
                        experimenterSubjectFind.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
                    {
                        paymentMethod = PaymentMethod.TutorSubjectFunds;
                    }
                }
                else
                {
                    subject = __subjectBLL.GetSubjectByTurtorId(user.Id);
                }
                beginTime = optTime > now ? now : optTime;
                endTime = optTime > now ? optTime : now;
            }
            double duration = 0d;
            double? unitPrice = null;
            bool isOpenFundCostDeduct = false;
            double? realEquipmentOpenFundDiscout = null;
            IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
            // 计算使用产生费用
            double predictWillPayFee = __chargeStandardBLL.GetUsedConfirmCalFee(new UsedConfirm
                                                                                    {
                                                                                        UserId = user.Id,
                                                                                        BeginAt = beginTime,
                                                                                        EndAt = endTime,
                                                                                        AppointmentId = appointmentId,
                                                                                        EquipmentId = equipment.Id,
                                                                                        SubjectId = subject.Id,
                                                                                        PaymentMethod =
                                                                                            (int)paymentMethod
                                                                                    }, out duration, out unitPrice,
                                                                                out isOpenFundCostDeduct,
                                                                                out costDeductEquipmentOpenFunds,
                                                                                out realEquipmentOpenFundDiscout);
            UsedConfirm usedConfirm = null;
            // 有预约
            if (appointment != null)
            {
                // 预约预扣费，需要在使用费用中移除当前预约预扣费部分
                if (appointment.IsPredictCostDeduct.HasValue && appointment.IsPredictCostDeduct.Value)
                {
                    predictWillPayFee = predictWillPayFee - appointment.VirtualCurrency.Value -
                                        appointment.RealCurrency.Value;
                }
                usedConfirm =
                    __usedConfirmBLL.GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"",
                                                                                       appointment.Id));
            }
            else
            {
                usedConfirm =
                    __usedConfirmBLL.GetFirstOrDefaultEntityByExpression(
                        string.Format("UserId=\"{0}\"*EquipmentId=\"{1}\"*BeginAt=\"{2}\"", user.Id.ToString(),
                                      equipment.Id.ToString(), optTime.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            // 预约预扣费，需要在使用费用中移除当前预约使用费用
            if (usedConfirm != null && usedConfirm.CostDeduct != null)
            {
                predictWillPayFee = predictWillPayFee - usedConfirm.CostDeduct.VirtualCurrency.Value -
                                    usedConfirm.CostDeduct.RealCurrency.Value;
            }
            ExperimenterSubject experimenterSubject = subject.Experiments == null
                                                          ? null
                                                          : subject.Experiments.FirstOrDefault(
                                                              p => p.ExperimenterId.Value == user.Id);
            if (experimenterSubject != null)
            {
                experimenterWillPayFee = predictWillPayFee;
            }
            double willPayTotalFee = CalUseWillPayFee(subject, null, predictWillPayFee);
            return MoneyValidate(MoneyValidateType.UseMoneyValidate, willPayTotalFee, user, subject, paymentMethod,
                                 experimenterSubject, experimenterWillPayFee, null, null, null, 0d,
                                 equipment.AppointmentMinAccountBalance, out errorMessage);
        }

        #endregion

        /// <summary> 验证其他账号，创建验证结果并存入结果管理器 </summary>
        /// <param name="payer">付款人</param>
        /// <param name="needMoney">仍需要多少资金</param>
        /// <returns>返回true表示验证通过，否则验证不通过</returns>
        protected virtual bool ValidateOtherAccount(User payer, double needMoney)
        {
            //MoneyValidateResultManager.SetResult(payer.Label, MoneyValidateResult.CreateNotNeedXjAccount(payer));
            return false;
        }

        protected virtual double CalUseWillPayFee(Subject subject, IEnumerable<SampleApply> lstSampleApplies,
                                                  double predictWillPayFee = 0d)
        {
            User payer = subject.Director;
            IList<ViewSampleApply> viewSampleApplyList = __viewSampleApplyBLL.GetUserWillPaySampleApplyList(payer.Id);
            //未扣费送样可能要产生的费用
            double willSampleApplyTotalPayFee = 0d;

            IList<SampleApply> sampleApplies = lstSampleApplies == null
                                                   ? new List<SampleApply>()
                                                   : (lstSampleApplies as IList<SampleApply> ??
                                                      lstSampleApplies.ToList());
            if (sampleApplies.Any())
            {
                willSampleApplyTotalPayFee += sampleApplies.Sum(p => p.Price.Value - p.VirtualCurrency - p.RealCurrency);
            }
            if (viewSampleApplyList != null && viewSampleApplyList.Count > 0)
            {
                willSampleApplyTotalPayFee += viewSampleApplyList.Sum(p => (p.Price.HasValue ? p.Price.Value : 0d));
                if (sampleApplies.Any())
                {
                    if (sampleApplies.Any(p => p.EditId.HasValue))
                    {
                        List<ViewSampleApply> eidtViewSampleApplies =
                            viewSampleApplyList.Where(
                                q =>
                                sampleApplies.Where(p => p.EditId.HasValue).Select(p => p.EditId.Value).Contains(q.Id)).
                                ToList();
                        if (eidtViewSampleApplies.Any())
                            willSampleApplyTotalPayFee -=
                                eidtViewSampleApplies.Sum(p => p.Price.HasValue ? p.Price.Value : 0d);
                    }
                }
            }
            /* 2015.11.4
             * chao: 未扣费机时预约可能要产生的费用（重复扣费）
             * 获取用户待支付预约费用合计
             */
            double willAppointmentTotalPayFee = 0d;
            IList<ViewAppointment> viewAppointmentList = __viewAppointmentBLL.GetUserWillPayAppointmentList(payer.Id);
            if (viewAppointmentList != null && viewAppointmentList.Count > 0)
            {
                willAppointmentTotalPayFee = viewAppointmentList.Sum(p => p.Fee.HasValue ? p.Fee.Value : 0d);
            }
            //未扣费耗材预约可能要产生的费用
            return willSampleApplyTotalPayFee + willAppointmentTotalPayFee + predictWillPayFee;
        }

        private double CalAppointmentWillPayFee(Subject subject, Appointment appointment,
                                                IEnumerable<Appointment> lstAppointments)
        {
            var lstViewAppointments = new List<ViewAppointment>();
            if (lstAppointments != null && lstAppointments.Count() > 0 && appointment != null)
            {
                foreach (Appointment appointmentTemp in lstAppointments)
                {
                    //取消预约的记录,这里应用了一个小技巧,如果是新增预约的情况,第一个参数为新预约记录的第一条记录,第二个参数对应应该有两条与第一个参数一样的预约记录,因为第一个参数在算未计费的预约总金额时候会减去该记录的金额，所以第二参数需要两条与第一个参数一样的预约记录这样才能保证余额的准确
                    int count = appointmentTemp.Id == appointment.Id &&
                                lstAppointments.Count(p => p.Id == appointmentTemp.Id) == 1
                                    ? 2
                                    : 1;
                    for (int i = 0; i < count; i++)
                    {
                        lstViewAppointments.Add(new ViewAppointment
                                                    {
                                                        Id = appointmentTemp.Id,
                                                        Fee = appointment.Fee
                                                    });
                    }
                }
            }
            double willPayTotalFee = CalUseWillPayFee(subject, null);
            willPayTotalFee += lstViewAppointments.Where(p => p.Fee.HasValue).Sum(p => p.Fee.Value) -
                               (appointment == null ? 0 : appointment.Fee.Value);
            return willPayTotalFee;
        }

        private double CalCurAppointmentSubjectExperimenterWillPayFee(Appointment appointment,
                                                                      IEnumerable<Appointment> lstAppointments)
        {
            var lstViewAppointments = new List<ViewAppointment>();
            if (lstAppointments != null && lstAppointments.Count() > 0 && appointment != null)
            {
                foreach (Appointment appointmentTemp in lstAppointments)
                {
                    //取消预约的记录,这里应用了一个小技巧,如果是新增预约的情况,第一个参数为新预约记录的第一条记录,第二个参数对应应该有两条与第一个参数一样的预约记录,因为第一个参数在算未计费的预约总金额时候会减去该记录的金额，所以第二参数需要两条与第一个参数一样的预约记录这样才能保证余额的准确
                    int count = appointmentTemp.Id == appointment.Id &&
                                lstAppointments.Count(p => p.Id == appointmentTemp.Id) == 1
                                    ? 2
                                    : 1;
                    for (int i = 0; i < count; i++)
                    {
                        lstViewAppointments.Add(new ViewAppointment
                                                    {
                                                        Id = appointmentTemp.Id,
                                                        Fee = appointment.Fee
                                                    });
                    }
                }
            }
            double curAppointmentTotalWillPay = lstViewAppointments.Where(p => p.Fee.HasValue).Sum(p => p.Fee.Value) -
                                                (appointment == null ? 0 : appointment.Fee.Value);
            return curAppointmentTotalWillPay;
        }

        private double CalSampleApplyWillPayFee(Subject subject, IEnumerable<SampleApply> lstSampleApplies,
                                                double curSampleTotalFee)
        {
            double willPayTotalFee = CalUseWillPayFee(subject, lstSampleApplies);
            return willPayTotalFee;
        }

        private double CalCurSampleApplySubjectExperimenterWillPayFee(double sampleApplyTotalFee)
        {
            return sampleApplyTotalFee;
        }

        private bool MoneyValidate(MoneyValidateType moneyValidateType,
                                   double willPayTotalFee,
                                   User user,
                                   Subject subject,
                                   PaymentMethod paymentMethod,
                                   ExperimenterSubject experimenterSubject,
                                   double experimenterWillPayFee,
                                   Appointment appointment,
                                   IEnumerable<Appointment> lstAppointments,
                                   IEnumerable<SampleApply> lstSampleApplies,
                                   double curSampleWillPayFee,
                                   double? minAccountBalance,
                                   out string errorMessage)
        {
            errorMessage = "";
            if (subject.SubjectDirectorId != user.Id && experimenterSubject == null) throw new Exception("非课题组成员");
            User payer = subject.Director;

            var unAppointment = payer.UserAccount.UnAppointment ?? payer.UserAccount.CreditLimit.UnAppointment;

            if (unAppointment > (payer.UserAccount.TotalCurrency - willPayTotalFee))
            {
                if (!ValidateOtherAccount(payer, (unAppointment ?? 0) - (payer.UserAccount.TotalCurrency - willPayTotalFee)))
                {
                    errorMessage = "付费人的所有未开始预约可能产生费用的总额超出其帐户不可预约值，账户余额为【" +
                                   payer.UserAccount.TotalCurrency.ToString("0.00") + "】,可能产生的费用为【" +
                                   willPayTotalFee.ToString("0.00") + "】";
                    return false;
                }
            }
            if (minAccountBalance.HasValue &&
                (payer.UserAccount.TotalCurrency - willPayTotalFee) < minAccountBalance.Value)
            {
                if (!ValidateOtherAccount(payer, minAccountBalance.Value - (payer.UserAccount.TotalCurrency - willPayTotalFee)))
                {
                    errorMessage = "付费人的所有未开始预约可能产生费用的总额超出预约的最小额度，账户余额为【" +
                                   payer.UserAccount.TotalCurrency.ToString("0.00") +
                                   "】,可能产生的费用为【" + willPayTotalFee.ToString("0.00") + "】,所需最小额度为【" +
                                   minAccountBalance.Value.ToString("0.00") + "】";
                    return false;
                }
            }
            var lstExperimenterViewAppointments = new List<ViewAppointment>();
            IList<ViewAppointment> totalWillPayViewAppointmentList =
                __viewAppointmentBLL.GetUserWillPayAppointmentList(user.Id, subject.Id);
            if (totalWillPayViewAppointmentList != null && totalWillPayViewAppointmentList.Count > 0)
                lstExperimenterViewAppointments.AddRange(totalWillPayViewAppointmentList);
            if (experimenterSubject != null)
            {
                switch (paymentMethod)
                {
                    case PaymentMethod.SubjectDirectorPay:
                        if (experimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
                        {
                            if (experimenterSubject.OddSum < experimenterSubject.Unappointment ||
                                experimenterSubject.OddSum < experimenterSubject.UnUseable)
                            {
                                errorMessage = "课题组分配经费余额不足";
                                return false;
                            }
                            double totalWillPayExperimenterSubjectFunds =
                                lstExperimenterViewAppointments.Where(p => p.Fee.HasValue).Sum(p => p.Fee.Value) +
                                experimenterWillPayFee;
                            if (experimenterSubject.Unappointment >
                                (experimenterSubject.OddSum - totalWillPayExperimenterSubjectFunds))
                            {
                                errorMessage = "您的所有未开始预约可能产生费用的总额超出课题组分配经费的不可预约值，可用经费为【" +
                                               experimenterSubject.OddSum.ToString("0.00") + "】,可能产生的总费用为【" +
                                               experimenterWillPayFee.ToString("0.00") + "】";
                                return false;
                            }
                            if (minAccountBalance.HasValue &&
                                (experimenterSubject.OddSum - totalWillPayExperimenterSubjectFunds) < minAccountBalance)
                            {
                                errorMessage = "您的所有未开始预约可能产生费用的总额超出预约的最小额度，可用经费为【" +
                                               experimenterSubject.OddSum.ToString("0.00") + "】,可能产生的总费用为【" +
                                               experimenterWillPayFee.ToString("0.00") + "】，预约所需最小额度为【" +
                                               minAccountBalance.Value.ToString("0.00") + "】";
                                return false;
                            }
                        }
                        break;
                    case PaymentMethod.TutorSubjectFunds:
                        double totalWillPayExperimenter =
                            lstExperimenterViewAppointments.Where(p => p.Fee.HasValue).Sum(p => p.Fee.Value) +
                            experimenterWillPayFee;
                        if (appointment != null && appointment.GetPayer().UserType != null &&
                            appointment.GetPayer().UserType.UserIdentityEnum != UserIdentity.Tutor &&
                            appointment.GetPayer().UserType.UserIdentityEnum != UserIdentity.OutTutor &&
                            appointment.GetPayer().UserType.UserIdentityEnum != UserIdentity.OutTutor)
                        {
                            errorMessage = "付费人不是导师";
                            return false;
                        }
                        if (experimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign &&
                            experimenterSubject.Unappointment > (experimenterSubject.OddSum - totalWillPayExperimenter))
                        {
                            errorMessage = "您的所有未开始预约可能产生费用的总额超出课题组分配经费的不可预约值，可用经费为【" +
                                           experimenterSubject.OddSum.ToString("0.00") + "】,可能产生的总费用为【" +
                                           totalWillPayExperimenter.ToString("0.00") + "】";
                            return false;
                        }
                        if (appointment != null && appointment.UserId.Value != appointment.GetPayer().Id)
                        {
                            ExperimenterSubject tutorExperimenterSubject =
                                subject.Experiments.FirstOrDefault(
                                    p => p.ExperimenterId == experimenterSubject.OwnerId.Value && !p.IsDelete);
                            if (tutorExperimenterSubject == null)
                            {
                                errorMessage = "非课题组成员";
                                return false;
                            }
                            if (tutorExperimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign &&
                                tutorExperimenterSubject.Unappointment >
                                (tutorExperimenterSubject.OddSum - totalWillPayExperimenter))
                            {
                                errorMessage = "您的所有未开始预约可能产生费用的总额超出课题组分配经费的不可预约值，可用经费为【" +
                                               experimenterSubject.OddSum.ToString("0.00") + "】,可能产生的总费用为【" +
                                               totalWillPayExperimenter.ToString("0.00") + "】";
                                return false;
                            }
                        }
                        break;
                }
            }
            return true;
        }
    }

    public class ZsdxMoneyValidateBLL : MoneyValidateBLL
    {
        private static readonly ZsdxAccountBLL __accountBLL = new ZsdxAccountBLL();

        private DataSyncAndCommunicationSettingBLL __dataSyncBLL = new DataSyncAndCommunicationSettingBLL();
        private DataSyncAndCommunicationSetting __dataSync;
        public ZsdxMoneyValidateBLL()
        {
            __dataSync = __dataSyncBLL.GetDataSyncSetting();
        }
        #region Overrides of MoneyValidateBLL

        /// <summary> 验证其他账户，创建验证结果并存入结果管理器 </summary>
        /// <param name="payer"></param>
        /// <param name="needMoney"></param>
        /// <returns>返回true表示验证通过，否则验证不通过</returns>
        protected override bool ValidateOtherAccount(User payer, double needMoney)
        {
            if(!__dataSync.IsAutoCostDeductSync) return false;
            /* 费用验证
            * 优先检查本地账户剩余资金是否充足，不充足则获取校级账户剩余资金
            */
            XjDsAccountResult xjAccount = null;
            try
            {

                 xjAccount = __accountBLL.GetAccount(payer.Label) as XjDsAccountResult;
            }catch (Exception ex)
            {
                log.Info(string.Format("本地账户余额不足,并且连接校级平台失败,用户id:{0},用户姓名:{1},用户账号:{2},", payer.Id.ToString(), payer.UserName,payer.Label));
                return false;
            }
            if (xjAccount == null)
            {
                return false;
            }
            if (xjAccount.balance < needMoney)
            {
                return false;
            }
            //var result = MoneyValidateResult.CreateNeedXjAccount(payer, needMoney, xjAccount);
            //MoneyValidateResultManager.SetResult(payer.Id.ToString(), result);
            return true;
        }

        #endregion
    }
    ///// <summary> 金额验证结果 </summary>
    //public class MoneyValidateResult
    //{
    //    private MoneyValidateResult() { }
    //    /// <summary> 是否需要校级账户 </summary>
    //    public bool IsNeedXjAccount { get; private set; }
    //    ///// <summary> 总共计费金额 </summary>
    //    //public double Money { get; private set; }
    //    /// <summary> 需要校级的金额 </summary>
    //    public double XjMoney { get; private set; }
    //    /// <summary> 校级账户信息 </summary>
    //    public XjDsAccountResult XjAccount { get; private set; }
    //    /// <summary> 支付账户 </summary>
    //    public User Payer { get; private set; }
    //    /// <summary> 创建不需要校级账户验证结果 </summary>
    //    /// <returns></returns>
    //    public static MoneyValidateResult CreateNotNeedXjAccount(User payer)
    //    {
    //        return new MoneyValidateResult { Payer = payer };
    //    }
    //    /// <summary> 创建需要校级账户验证结果 </summary>
    //    /// <param name="payer"></param>
    //    /// <param name="needMoney"></param>
    //    /// <param name="accountResult"></param>
    //    /// <returns></returns>
    //    public static MoneyValidateResult CreateNeedXjAccount(User payer, double needMoney, XjDsAccountResult accountResult)
    //    {
    //        var result = new MoneyValidateResult { IsNeedXjAccount = true, XjAccount = accountResult };
    //        //var unAppointment = 0d;
    //        //if (payer.UserAccount.CreditLimit.UnAppointment.HasValue)
    //        //{
    //        //    unAppointment = payer.UserAccount.CreditLimit.UnAppointment.Value;
    //        //}
    //        result.XjMoney = needMoney;
    //        result.Payer = payer;
    //        return result;
    //    }
    //}
    ///// <summary> 金额验证结果管理器 </summary>
    //public static class MoneyValidateResultManager
    //{
    //    private static readonly string typeName = typeof(MoneyValidateResultManager).Name;
    //    public static void SetResult(string identify, MoneyValidateResult result)
    //    {
    //        if (string.IsNullOrEmpty(identify)) return;
    //        var key = ToKey(identify);
    //        HttpContext.Current.Session[key] = result;
    //    }

    //    public static MoneyValidateResult GetResult(string identify)
    //    {
    //        if (string.IsNullOrEmpty(identify)) return null;
    //        var key = ToKey(identify);
    //        return HttpContext.Current.Session[key] as MoneyValidateResult;
    //    }

    //    public static void Remove(string identify)
    //    {
    //        if (string.IsNullOrEmpty(identify)) return;
    //        var key = ToKey(identify);
    //        HttpContext.Current.Session.Remove(key);
    //    }

    //    private static string ToKey(string identify)
    //    {
    //        return string.Format("{0}_{1}", typeName, identify);
    //    }
    //}

    //public class XjMoneyValidate : IMoneyValidateBLL
    //{
    //    private readonly IAppointmentBLL __appointmentBLL;
    //    private readonly IChargeStandardBLL __chargeStandardBLL;
    //    private readonly IEquipmentBLL __equipmentBLL;
    //    private readonly ISubjectBLL __subjectBLL;
    //    private readonly IUsedConfirmBLL __usedConfirmBLL;
    //    private readonly IUserBLL __userBLL;
    //    private ISampleApplyBLL __sampleApplyBLL;
    //    protected IViewAppointmentBLL __viewAppointmentBLL;
    //    protected IViewSampleApplyBLL __viewSampleApplyBLL;
    //    protected ZsdxAccountBLL __zsdxAccountBLL;
    //    public XjMoneyValidate()
    //    {
    //        __subjectBLL = BLLFactory.CreateSubjectBLL();
    //        __userBLL = BLLFactory.CreateUserBLL();
    //        __equipmentBLL = BLLFactory.CreateEquipmentBLL();
    //        __appointmentBLL = BLLFactory.CreateAppointmentBLL();
    //        __sampleApplyBLL = BLLFactory.CreateSampleApplyBLL();
    //        __chargeStandardBLL = BLLFactory.CreateChargeStandardBLL();
    //        __viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
    //        __viewSampleApplyBLL = BLLFactory.CreateViewSampleApplyBLL();
    //        __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
    //        __zsdxAccountBLL = new ZsdxAccountBLL();
    //    }

    //    public bool TutorSimpleMoneyValidate(Guid userId, double money, out string errorMessage)
    //    {
    //        errorMessage = "";
    //        User user = __userBLL.GetEntityById(userId);
    //        if (user == null)
    //        {
    //            errorMessage = string.Format("找不到编码为【{0}】的用户信息", userId.ToString());
    //            return false;
    //        }
    //        User tutor = user.UserType.UserIdentityEnum == UserIdentity.Tutor ||
    //                     user.UserType.UserIdentityEnum == UserIdentity.OutTutor
    //                         ? user
    //                         : user.Tutor;
    //        if (tutor == null)
    //        {
    //            errorMessage = "导师信息为空";
    //            return false;
    //        }
    //        var xjAccountInfo = __zsdxAccountBLL.GetAccount(tutor.Label);
    //        if (xjAccountInfo.ContainsKey("errcode") && Equals(xjAccountInfo["errcode"], "0"))
    //        {
    //            if (xjAccountInfo.ContainsKey("balance"))
    //            {
    //                if (!Equals(xjAccountInfo["status"], "0"))
    //                {
    //                    errorMessage = "付款人账户已经停用！";
    //                    return false;
    //                }
    //                if (money > double.Parse(xjAccountInfo["balance"].ToString()))
    //                {
    //                    errorMessage = "付款人余额不足！";
    //                    return false; ;
    //                }
    //            }
    //            else
    //            {
    //                errorMessage = "付款人必须为导师！";
    //                return false;
    //            }
    //        }
    //        else
    //        {
    //            errorMessage = "导师账户为空";
    //            return false;

    //        }

    //        Subject subject = __subjectBLL.GetUserSelfJoinSubject(tutor.Id);
    //        if (subject == null)
    //        {
    //            errorMessage = "找不到课题组信息";
    //            return false;
    //        }
    //        ExperimenterSubject experimentSubject = subject.Experiments == null
    //                                                    ? null
    //                                                    : subject.Experiments.FirstOrDefault(
    //                                                        p => p.ExperimenterId == userId && !p.IsDelete);
    //        if (experimentSubject != null &&
    //            experimentSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
    //        {
    //            if (experimentSubject.UnUseable > experimentSubject.OddSum - money)
    //            {
    //                errorMessage =
    //                    string.Format(@"余额提醒:\r\n您的课题经费余额不足!您的课题经费余额:{0}元,\r\n可能产生的费用:{1}元,\r\n经费余额不可使用值:{2}元.",
    //                                  experimentSubject.OddSum,
    //                                  money,
    //                                  experimentSubject.UnUseable);
    //                return false;
    //            }
    //            if (experimentSubject.Unappointment > experimentSubject.OddSum - money)
    //            {
    //                errorMessage =
    //                    string.Format(@"余额提醒:\r\n您的课题经费余额不足!您的课题经费余额:{0}元,\r\n可能产生的费用:{1}元,\r\n经费余额不可预约值:{2}元.",
    //                                  experimentSubject.OddSum,
    //                                  money,
    //                                  experimentSubject.Unappointment);
    //                return false;
    //            }
    //        }
    //        else
    //        {
    //            if (tutor.UserAccount.TotalCurrency - money < tutor.UserAccount.CreditLimit.UnUseable)
    //            {
    //                errorMessage =
    //                    string.Format(@"余额提醒:\r\n您的课题经费余额不足!您的课题经费余额:{0}元,\r\n可能产生的费用:{1}元,\r\n付费人帐户余额不可使用值:{2}元.",
    //                                  tutor.UserAccount.TotalCurrency,
    //                                  money,
    //                                  tutor.UserAccount.CreditLimit.UnUseable);
    //                return false;
    //            }
    //            if (tutor.UserAccount.TotalCurrency - money < tutor.UserAccount.CreditLimit.UnAppointment)
    //            {
    //                errorMessage =
    //                    string.Format(@"余额提醒:\r\n您的课题经费余额不足!您的课题经费余额:{0}元,\r\n可能产生的费用:{1}元,\r\n付费人帐户余额不可预约值:{2}元.",
    //                                  tutor.UserAccount.TotalCurrency,
    //                                  money,
    //                                  tutor.UserAccount.CreditLimit.UnAppointment);
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //    public bool SimpleMoneyValidate(Guid userId, Guid subjectId, PaymentMethod paymentMethod, double money)
    //    {
    //        Subject subject = __subjectBLL.GetEntityById(subjectId);
    //        if (subject == null) throw new Exception(string.Format("找不到编码为【{0}】的课题信息", subjectId));
    //        ExperimenterSubject experimenterSubject = subject.Experiments == null
    //                                                      ? null
    //                                                      : subject.Experiments.FirstOrDefault(
    //                                                          p => p.ExperimenterId.Value == userId);
    //        if (subject.SubjectDirectorId != userId && experimenterSubject == null)
    //            throw new Exception("非课题组成员");
    //        User payer = subject.Director;
    //        var xjAccountInfo = __zsdxAccountBLL.GetAccount(payer.Label);
    //        if (xjAccountInfo.ContainsKey("errcode") && Equals(xjAccountInfo["errcode"], "0"))
    //        {
    //            if (xjAccountInfo.ContainsKey("balance"))
    //            {
    //                if (!Equals(xjAccountInfo["status"], "0"))
    //                {
    //                    throw new Exception("付款人账户已经停用！");
    //                }
    //                if (money > double.Parse(xjAccountInfo["balance"].ToString()))
    //                {
    //                    throw new Exception("付款人余额不足！");
    //                }
    //            }
    //            else
    //            {
    //                throw new Exception("付款人必须为导师！");
    //            }
    //        }
    //        //if (payer.UserAccount.CreditLimit.UnAppointment > (payer.UserAccount.TotalCurrency - money))
    //        //    throw new CheckMoneyFailException("付费人产生费用的总额超出其帐户不可预约值");
    //        if (experimenterSubject != null)
    //        {
    //            switch (paymentMethod)
    //            {
    //                case PaymentMethod.SubjectDirectorPay:
    //                    if (experimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
    //                    {
    //                        if (experimenterSubject.OddSum < experimenterSubject.Unappointment ||
    //                            experimenterSubject.OddSum < experimenterSubject.UnUseable)
    //                            throw new CheckMoneyFailException("课题组分配经费余额不足");

    //                        if (experimenterSubject.Unappointment > (experimenterSubject.OddSum - money))
    //                            throw new CheckMoneyFailException("产生费用的总额超出课题组分配经费的不可预约值");
    //                    }
    //                    break;
    //                case PaymentMethod.TutorSubjectFunds:
    //                    if (experimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign &&
    //                        experimenterSubject.Unappointment > (experimenterSubject.OddSum - money))
    //                        throw new CheckMoneyFailException("产生费用的总额超出课题组分配经费的不可预约值");
    //                    ExperimenterSubject tutorExperimenterSubject =
    //                        subject.Experiments.FirstOrDefault(
    //                            p => p.ExperimenterId == experimenterSubject.OwnerId.Value && !p.IsDelete);
    //                    if (tutorExperimenterSubject != null &&
    //                        tutorExperimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign &&
    //                        tutorExperimenterSubject.Unappointment > (tutorExperimenterSubject.OddSum - money))
    //                        throw new CheckMoneyFailException("产生费用的总额超出课题组分配给导师经费的不可预约值");
    //                    break;
    //            }
    //        }
    //        return true;
    //    }


    //    public bool SampleApplyMoneyValidate(Guid userId, Guid subjectId, PaymentMethod paymentMethod,
    //                                         IEnumerable<SampleApply> lstSampleApplies, double curSampleWillPayFee,
    //                                         out string errorMessage)
    //    {
    //        errorMessage = "";
    //        double experimenterWillPayFee = 0d;
    //        User user = __userBLL.GetEntityById(userId);
    //        Subject subject = __subjectBLL.GetEntityById(subjectId);
    //        ExperimenterSubject experimenterSubject = subject.Experiments == null
    //                                                      ? null
    //                                                      : subject.Experiments.FirstOrDefault(
    //                                                          p => p.ExperimenterId.Value == user.Id);
    //        if (experimenterSubject != null)
    //        {
    //            experimenterWillPayFee = CalCurSampleApplySubjectExperimenterWillPayFee(curSampleWillPayFee);
    //        }
    //        double willTotalPayFee = CalSampleApplyWillPayFee(subject, lstSampleApplies, curSampleWillPayFee);
    //        return MoneyValidate(MoneyValidateType.SampleApplyMoneyValidate, willTotalPayFee, user, subject,
    //                             paymentMethod, experimenterSubject, experimenterWillPayFee, null, null,
    //                             lstSampleApplies, curSampleWillPayFee, null, out errorMessage);
    //    }

    //    public bool AppointmentMoneyValidate(Guid userId, Guid subjectId, PaymentMethod paymentMethod,
    //                                         Appointment appointment, IEnumerable<Appointment> lstAppointments,
    //                                         double? minAccountBalance, out string errorMessage)
    //    {
    //        errorMessage = "";
    //        double experimenterWillPayFee = 0d;
    //        User user = __userBLL.GetEntityById(userId);
    //        Subject subject = __subjectBLL.GetEntityById(subjectId);
    //        ExperimenterSubject experimenterSubject = subject.Experiments == null
    //                                                      ? null
    //                                                      : subject.Experiments.FirstOrDefault(
    //                                                          p => p.ExperimenterId.Value == user.Id);
    //        if (experimenterSubject != null)
    //        {
    //            experimenterWillPayFee = CalCurAppointmentSubjectExperimenterWillPayFee(appointment, lstAppointments);
    //        }
    //        double willTotalPayFee = CalAppointmentWillPayFee(subject, appointment, lstAppointments);
    //        return MoneyValidate(MoneyValidateType.AppointmentMoneyValidate, willTotalPayFee, user, subject,
    //                             paymentMethod, experimenterSubject, experimenterWillPayFee, appointment,
    //                             lstAppointments, null, 0, minAccountBalance, out errorMessage);
    //    }

    //    public bool UseAppointmentMoneyValidate(string appointmentIdStr, string userLabel, string equipmentLabel,
    //                                            string tutorName, DateTime optTime, out string errorMessage)
    //    {
    //        errorMessage = "";
    //        double experimenterWillPayFee = 0d;
    //        if (string.IsNullOrWhiteSpace(userLabel))
    //        {
    //            errorMessage = "用户标识为空";
    //            return false;
    //        }
    //        User user =
    //            __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsDel=false",
    //                                                                        userLabel.Trim()));
    //        if (user == null)
    //        {
    //            errorMessage = string.Format("找不到标识为【{0}】的用户信息", userLabel.Trim());
    //            return false;
    //        }
    //        if (string.IsNullOrWhiteSpace(equipmentLabel))
    //        {
    //            errorMessage = "设备标识为空";
    //            return false;
    //        }
    //        Equipment equipment =
    //            __equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsDelete=false",
    //                                                                             equipmentLabel.Trim()));
    //        if (equipment == null)
    //        {
    //            errorMessage = string.Format("找不到标识为【{0}】的设备信息", equipmentLabel.Trim());
    //            return false;
    //        }
    //        Appointment appointment = null;
    //        Guid? appointmentId = null;
    //        DateTime? beginTime = null, endTime = null;
    //        DateTime now = DateTime.Now;
    //        Subject subject = null;
    //        var paymentMethod = PaymentMethod.SubjectDirectorPay;
    //        if (!string.IsNullOrWhiteSpace(appointmentIdStr))
    //        {
    //            appointmentId = new Guid(appointmentIdStr.Trim());
    //            appointment = __appointmentBLL.GetEntityById(appointmentId.Value);
    //            if (appointment == null)
    //            {
    //                errorMessage = string.Format("找不到标识为【{0}】的机时预约信息", equipmentLabel.Trim());
    //                return false;
    //            }
    //            subject = appointment.Subject;
    //            paymentMethod = appointment.PaymentMethodEnum;
    //            if (optTime > appointment.EndTime.Value)
    //            {
    //                beginTime = optTime > now ? now : optTime;
    //                endTime = optTime > now ? optTime : now;
    //            }
    //            else
    //            {
    //                beginTime = optTime;
    //                endTime = appointment.EndTime.Value;
    //            }
    //        }
    //        else
    //        {
    //            if (user.TutorId.HasValue)
    //            {
    //                subject = __subjectBLL.GetSubjectByTurtorId(user.TutorId.Value);
    //                ExperimenterSubject experimenterSubjectFind = subject.Experiments == null
    //                                                                  ? null
    //                                                                  : subject.Experiments.FirstOrDefault(
    //                                                                      p => p.ExperimenterId.Value == user.Id);
    //                if (experimenterSubjectFind != null &&
    //                    experimenterSubjectFind.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
    //                {
    //                    paymentMethod = PaymentMethod.TutorSubjectFunds;
    //                }
    //            }
    //            else
    //            {
    //                subject = __subjectBLL.GetSubjectByTurtorId(user.Id);
    //            }
    //            beginTime = optTime > now ? now : optTime;
    //            endTime = optTime > now ? optTime : now;
    //        }
    //        double duration = 0d;
    //        double? unitPrice = null;
    //        bool isOpenFundCostDeduct = false;
    //        double? realEquipmentOpenFundDiscout = null;
    //        IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
    //        // 计算使用产生费用
    //        double predictWillPayFee = __chargeStandardBLL.GetUsedConfirmCalFee(new UsedConfirm
    //        {
    //            UserId = user.Id,
    //            BeginAt = beginTime,
    //            EndAt = endTime,
    //            AppointmentId = appointmentId,
    //            EquipmentId = equipment.Id,
    //            SubjectId = subject.Id,
    //            PaymentMethod =
    //                (int)paymentMethod
    //        }, out duration, out unitPrice,
    //                                                                            out isOpenFundCostDeduct,
    //                                                                            out costDeductEquipmentOpenFunds,
    //                                                                            out realEquipmentOpenFundDiscout);
    //        UsedConfirm usedConfirm = null;
    //        // 有预约
    //        if (appointment != null)
    //        {
    //            // 预约预扣费，需要在使用费用中移除当前预约预扣费部分
    //            if (appointment.IsPredictCostDeduct.HasValue && appointment.IsPredictCostDeduct.Value)
    //            {
    //                predictWillPayFee = predictWillPayFee - appointment.VirtualCurrency.Value -
    //                                    appointment.RealCurrency.Value;
    //            }
    //            usedConfirm =
    //                __usedConfirmBLL.GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"",
    //                                                                                   appointment.Id));
    //        }
    //        else
    //        {
    //            usedConfirm =
    //                __usedConfirmBLL.GetFirstOrDefaultEntityByExpression(
    //                    string.Format("UserId=\"{0}\"*EquipmentId=\"{1}\"*BeginAt=\"{2}\"", user.Id.ToString(),
    //                                  equipment.Id.ToString(), optTime.ToString("yyyy-MM-dd HH:mm:ss")));
    //        }
    //        // 预约预扣费，需要在使用费用中移除当前预约使用费用
    //        if (usedConfirm != null && usedConfirm.CostDeduct != null)
    //        {
    //            predictWillPayFee = predictWillPayFee - usedConfirm.CostDeduct.VirtualCurrency.Value -
    //                                usedConfirm.CostDeduct.RealCurrency.Value;
    //        }
    //        ExperimenterSubject experimenterSubject = subject.Experiments == null
    //                                                      ? null
    //                                                      : subject.Experiments.FirstOrDefault(
    //                                                          p => p.ExperimenterId.Value == user.Id);
    //        if (experimenterSubject != null)
    //        {
    //            experimenterWillPayFee = predictWillPayFee;
    //        }
    //        double willPayTotalFee = CalUseWillPayFee(subject, null, predictWillPayFee);
    //        return MoneyValidate(MoneyValidateType.UseMoneyValidate, willPayTotalFee, user, subject, paymentMethod,
    //                             experimenterSubject, experimenterWillPayFee, null, null, null, 0d,
    //                             equipment.AppointmentMinAccountBalance, out errorMessage);
    //    }

    //}
}