
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
    public class NMPUsedConfirmCostDeduct:NMPUsedConfirmRelativeCostDeduct
    {
        private IUserBLL __userBLL;
        private NMPUsedConfirm __nmpUsedConfirm;
        private Guid? __operatorId;
        private User __operator;
        private bool __isAllowAccountMinuse;
        private bool __isSuperAdmin;
        public NMPUsedConfirmCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects, operatorId, operatorIP)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __nmpUsedConfirm = businessObjects[0] as NMPUsedConfirm;
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
                var calFee = __nmpUsedConfirm.RealFee.HasValue ? __nmpUsedConfirm.RealFee.Value : __nmpUsedConfirm.CalcFee;
                double willPayFee = __nmpUsedConfirm.CostDeduct == null ? calFee : calFee - __nmpUsedConfirm.CostDeduct.Fee;
                return _moneyValidateBLL.SimpleMoneyValidate(__nmpUsedConfirm.UserId, __nmpUsedConfirm.SubjectId.Value, __nmpUsedConfirm.PaymentMethodEnum, willPayFee);
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
                var nmpUsedConfirm = _nmpUsedConfirmBLL.GetEntityById(__nmpUsedConfirm.Id); 
                PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
                UserAccount userAccount = null;
                var payer = _userBLL.GetPayer(__nmpUsedConfirm.UserId, __nmpUsedConfirm.SubjectId.Value, out paymentMethod, out userAccount);
                if (nmpUsedConfirm == null)
                    __nmpUsedConfirm = AddNMPUsedConfirm(__operatorId, __nmpUsedConfirm.User, userAccount, __nmpUsedConfirm.BeginAt.Value, __nmpUsedConfirm.EndAt.Value, __nmpUsedConfirm.Remark, __nmpUsedConfirm.RealFee, ref tran, true);
                else
                    __nmpUsedConfirm = UpdateUseTime(__operatorId, __nmpUsedConfirm, __nmpUsedConfirm.User, __nmpUsedConfirm.BeginAt.Value, __nmpUsedConfirm.EndAt.Value, __nmpUsedConfirm.RealFee, ref tran, null, true);
                if (__nmpUsedConfirm.NMPAppointmentId.HasValue)
                {
                    var nmpAppointment = __nmpUsedConfirm.GetNMPAppointment();
                    nmpAppointment.StatusEnum = AppointmentStatus.Success;
                    _nmpAppointmentBLL.Save(new NMPAppointment[] { nmpAppointment }, ref tran, isSupress);
                }
                if (!isSupress) tran.CommitTransaction();
                //消息通知
                SendDeductMessage(userAccount,out errorMessage);
                return new object[] { __nmpUsedConfirm };
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
                if (__nmpUsedConfirm.CostDeduct != null)
                {
                    if (__nmpUsedConfirm.CostDeduct.HasClossingAccount) throw new Exception("已结算");
                    _userAccountBLL.CancelDeduct(__nmpUsedConfirm.UserId,
                                                 __nmpUsedConfirm.SubjectId.Value,
                                                 __nmpUsedConfirm.PaymentMethodEnum,
                                                 __nmpUsedConfirm.CostDeduct.RealCurrency.Value,
                                                 __nmpUsedConfirm.CostDeduct.VirtualCurrency.Value,
                                                 ref tran,
                                                 out errorMessage,
                                                 true,
                                                 true
                                                 );
                    _costDeductBLL.Delete(new Guid[] { __nmpUsedConfirm.CostDeduct.Id }, ref tran, _operatorId, _operateIP, isSupress);
                }
                __nmpUsedConfirm.WhyNotCostDeduct = "撤销扣费";
                __nmpUsedConfirm.Status =(int) UsedConfirmStatus.Complete;
                __nmpUsedConfirm.RealFee = 0d;
                _nmpUsedConfirmBLL.Save(new NMPUsedConfirm[] { __nmpUsedConfirm }, ref tran, isSupress);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.Dispose();
                throw;
            }
            finally { if (!isSupress)tran.Dispose(); }
            return new object[] { __nmpUsedConfirm };
        }

        protected override Model.NMPUsedConfirm CreateNMPUseConfirm(out string errorMessage)
        {
            errorMessage = "";
            GenerateNMPUsedConfirmRelativeFeedBack(__nmpUsedConfirm);
            return __nmpUsedConfirm;
        }

        protected override bool SendDeductMessage(UserAccount userAccount,out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (__nmpUsedConfirm != null && __nmpUsedConfirm.CostDeduct != null)
                {
                    var result = _messageHandler.SendNMPUsedConfirmCostDeductNotice(__nmpUsedConfirm, __operator);
                    __nmpUsedConfirm.CostDeduct.UserAccount = userAccount;
                    result = result && _messageHandler.SendDepositNotice(__nmpUsedConfirm.CostDeduct, __nmpUsedConfirm, __operator);
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
