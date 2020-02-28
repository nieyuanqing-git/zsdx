using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class UsedConfirmCostDeduct:UsedConfirmRelativeCostDeduct
    {
        private IUserBLL __userBLL;
        private UsedConfirm __usedConfirm;
        private Guid? __operatorId;
        private User __operator;
        private bool __isAllowAccountMinuse;
        private bool __isSuperAdmin;
        public UsedConfirmCostDeduct(object[] businessObjects,Guid? operatorId,string operatorIP)
            : base(businessObjects, operatorId, operatorIP)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __usedConfirm = businessObjects[0] as UsedConfirm;
            if (_businessObjects.Length >=2 && _businessObjects[1] != null)
                __operatorId = new Guid(_businessObjects[1].ToString());
            if (businessObjects.Length >= 3) __isAllowAccountMinuse = bool.Parse(businessObjects[2].ToString());
            if (businessObjects.Length >= 4) __isSuperAdmin = bool.Parse(businessObjects[3].ToString());
            if (__operatorId.HasValue)
                __operator = __userBLL.GetEntityById(__operatorId.Value);
        }
        protected override bool CheckMoney()
        {
            if (!__isAllowAccountMinuse)
            {
                var calFee = __usedConfirm.RealFee.HasValue ? __usedConfirm.RealFee.Value : __usedConfirm.CalcFee;
                double willPayFee = __usedConfirm.CostDeduct == null ? calFee : calFee - __usedConfirm.CostDeduct.Fee;
                return _moneyValidateBLL.SimpleMoneyValidate(__usedConfirm.UserId, __usedConfirm.SubjectId.Value, __usedConfirm.PaymentMethodEnum, willPayFee);
            }
            return true;
        }
        public override object[] Deduct(ref august.DataLink.XTransaction tran)
        {
            string errorMessage="";
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!CheckMoney()) throw new CheckMoneyFailException(errorMessage);
                var usedConfirm = _usedConfirmBLL.GetEntityById(__usedConfirm.Id);
                PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
                UserAccount userAccount = null;
                var payer = _userBLL.GetPayer(__usedConfirm.UserId, __usedConfirm.SubjectId.Value, out paymentMethod, out userAccount);
                if (usedConfirm == null)
                    __usedConfirm = AddUsedConfirm(__operatorId, __usedConfirm.User, userAccount, __usedConfirm.BeginAt.Value, __usedConfirm.EndAt.Value, __usedConfirm.Remark, __usedConfirm.RealFee, ref tran, true);
                else
                    __usedConfirm = UpdateUseTime(__operatorId, __usedConfirm, __usedConfirm.User, __usedConfirm.BeginAt.Value, __usedConfirm.EndAt.Value, __usedConfirm.RealFee, ref tran, null, true);
                if (__usedConfirm.AppointmentId.HasValue)
                {
                    var appointment = __usedConfirm.GetAppointment();
                    appointment.StatusEnum = AppointmentStatus.Success;
                    _appointmentBLL.Save(new Appointment[] { appointment }, ref tran, isSupress);
                }
                if (!isSupress) tran.CommitTransaction();
                //消息通知
                SendDeductMessage(userAccount,out errorMessage);
                return new object[] { __usedConfirm };
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.Dispose();
                throw;
            }
            finally { if (!isSupress)tran.Dispose(); }
        }

        public override object[] CancelDeduct(ref XTransaction tran)
        {
            string errorMessage = "";
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (__usedConfirm.CostDeduct != null)
                {
                    if (__usedConfirm.CostDeduct.HasClossingAccount) throw new Exception("已结算");
                    _userAccountBLL.CancelDeduct(__usedConfirm.UserId,
                                                 __usedConfirm.SubjectId.Value,
                                                 __usedConfirm.PaymentMethodEnum,
                                                 __usedConfirm.CostDeduct.RealCurrency.Value,
                                                 __usedConfirm.CostDeduct.VirtualCurrency.Value,
                                                 ref tran,
                                                 out errorMessage,
                                                 true,
                                                 true
                                                 );
                    _costDeductBLL.Delete(new Guid[] { __usedConfirm.CostDeduct.Id }, ref tran, _operatorId, _operateIP, isSupress);
                }
                __usedConfirm.WhyNotCostDeduct = "撤销扣费";
                __usedConfirm.Status =(int) UsedConfirmStatus.Complete;
                __usedConfirm.RealFee = 0d;
                _usedConfirmBLL.Save(new UsedConfirm[] { __usedConfirm }, ref tran, isSupress);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.Dispose();
                throw;
            }
            finally { if (!isSupress)tran.Dispose(); }
            return new object[] { __usedConfirm };
        }

        protected override Model.UsedConfirm CreateUseConfirm(out string errorMessage)
        {
            errorMessage = "";
            GenerateUsedConfirmRelativeFeedBack(__usedConfirm);
            return __usedConfirm;
        }

        protected override bool SendDeductMessage(UserAccount userAccount,out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (__usedConfirm != null && __usedConfirm.CostDeduct != null)
                {
                    var result = _messageHandler.SendUsedConfirmCostDeductNotice(__usedConfirm, __operator);
                    __usedConfirm.CostDeduct.UserAccount = userAccount;
                    result = result && _messageHandler.SendDepositNotice(__usedConfirm.CostDeduct, __usedConfirm, __operator);
                    return result;
                }
            }
            catch (Exception ex) 
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
    }
}
