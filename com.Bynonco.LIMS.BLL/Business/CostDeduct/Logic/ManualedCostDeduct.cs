using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class ManualedCostDeduct :CommondCostDeduct
    {
        private IUserBLL __userBLL;
        private ManualCostDeduct __manualCostDeduct;
        private Guid? __operatorId;
        private User __operator;
        private bool __isAllowAccountMinuse;
        private bool __isSuperAdmin;
        public ManualedCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects, operatorId, operatorIP)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __manualCostDeduct = _businessObjects[0] as ManualCostDeduct;
            if (_businessObjects.Length > 1 && _businessObjects[1] != null)
                __operatorId = new Guid(_businessObjects[1].ToString());
            if (businessObjects.Length > 2) __isAllowAccountMinuse = bool.Parse(businessObjects[2].ToString());
            if (businessObjects.Length > 3) __isSuperAdmin = bool.Parse(businessObjects[3].ToString());
            if (__operatorId.HasValue) __operator = __userBLL.GetEntityById(__operatorId.Value);
        }
        private CostDeduct GetPreviousCostDeduct()
        {
            var previousCostDeduct = _costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("ManualCostDeductId=\"{0}\"", __manualCostDeduct.Id));
            return previousCostDeduct;
        }
        protected override bool CheckMoney()
        {
            IDictCodeBLL dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            var isCheckMoneyManualedCostDeduct = dictCodeBLL.GetDictCodeBoolValueByCode("CostDeduct", "IsCheckMoneyManualedCostDeduct"); //是否检测管理员余额
            bool isCheckMoney = isCheckMoneyManualedCostDeduct.HasValue && isCheckMoneyManualedCostDeduct.Value;
            if ((!__isAllowAccountMinuse && !__isSuperAdmin) || isCheckMoney)
            {
                var previousCostDeduct = GetPreviousCostDeduct();
                double willPayFee = previousCostDeduct == null ? __manualCostDeduct.Money : __manualCostDeduct.Money - previousCostDeduct.Fee;
                return _moneyValidateBLL.SimpleMoneyValidate(__manualCostDeduct.UserId, __manualCostDeduct.SubjectId, __manualCostDeduct.PaymentMethodEnum, willPayFee);
            }
            return true;
        }
        protected override Model.CostDeduct CreateCostDeduct()
        {
            double realCurrency = 0d, virtualCurrency = 0d;
            var costDeduct = GetPreviousCostDeduct();
            bool isNew = costDeduct == null;
            if (costDeduct == null) costDeduct = new CostDeduct() 
            { 
                Id = Guid.NewGuid(), 
                CalcDuration = 0d,
                CostDeductTypeEnum =  CostDeductType.ManualCostDeduct,
                Duration = 0d,
                DeductAt = DateTime.Now,
                HasClossingAccount = false
            };
            var paymentMethod = PaymentMethod.SubjectDirectorPay;
            UserAccount userAccount = null;
            _userBLL.GetPayer(__manualCostDeduct.UserId, __manualCostDeduct.SubjectId, out paymentMethod, out userAccount);
            if (isNew)
            {
                userAccount.Deduct(false,__manualCostDeduct.Money, out realCurrency, out virtualCurrency);
                costDeduct.RealCurrency = realCurrency;
                costDeduct.VirtualCurrency = virtualCurrency;
            }
            else
            {
                userAccount.RealCurrency += costDeduct.RealCurrency.Value;
                userAccount.VirtualCurrency += costDeduct.VirtualCurrency.Value;
                userAccount.Deduct(false,__manualCostDeduct.Money, out realCurrency, out virtualCurrency);
                costDeduct.RealCurrency = realCurrency;
                costDeduct.VirtualCurrency = virtualCurrency;
            }
            costDeduct.UserAccountId = userAccount.Id;
            costDeduct.CreaterId = __operatorId;
            costDeduct.ManualCostDeductId = __manualCostDeduct.Id;
            costDeduct.ManualCostDeduct = __manualCostDeduct;
            costDeduct.ManualCostDeductForLog = __manualCostDeduct;
            return costDeduct;
        }

        protected override CostDeduct BusinessHandle(out Guid subjectId, out Guid userId, out PaymentMethod paymentMethod)
        {
            var previousCostDeduct = GetPreviousCostDeduct();
            subjectId = __manualCostDeduct.SubjectId;
            userId = __manualCostDeduct.UserId;
            paymentMethod = __manualCostDeduct.PaymentMethodEnum;
            return previousCostDeduct;
        }

        protected override bool SendDeductMessage(UserAccount userAccount,out string errorMessage)
        {
            errorMessage = "";
            try
            {
                bool result = _messageHandler.SendManualCostDeductNotice(__manualCostDeduct,__operator);
                var previousCostDeduct = GetPreviousCostDeduct();
                if (previousCostDeduct != null && previousCostDeduct.ManualCostDeduct != null)
                    result = result && _messageHandler.SendDepositNotice(previousCostDeduct, previousCostDeduct.ManualCostDeduct, __operator);
                return result;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
