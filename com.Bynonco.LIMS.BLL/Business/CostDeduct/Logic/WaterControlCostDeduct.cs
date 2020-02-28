using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class WaterControledCostDeduct :CommondCostDeduct
    {
        private IUserBLL __userBLL;
        private WaterControlCostDeduct __WaterControlCostDeduct;
        private Guid? __operatorId;
        private User __operator;
        private bool __isAllowAccountMinuse;
        private bool __isSuperAdmin;
        public WaterControledCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects, operatorId, operatorIP)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __WaterControlCostDeduct = _businessObjects[0] as WaterControlCostDeduct;
            if (_businessObjects.Length > 1 && _businessObjects[1] != null)
                __operatorId = new Guid(_businessObjects[1].ToString());
            if (businessObjects.Length > 2) __isAllowAccountMinuse = bool.Parse(businessObjects[2].ToString());
            if (businessObjects.Length > 3) __isSuperAdmin = bool.Parse(businessObjects[3].ToString());
            if (__operatorId.HasValue) __operator = __userBLL.GetEntityById(__operatorId.Value);
        }
        private CostDeduct GetPreviousCostDeduct()
        {
            var previousCostDeduct = _costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("WaterControlCostDeductId=\"{0}\"", __WaterControlCostDeduct.Id));
            return previousCostDeduct;
        }
        protected override bool CheckMoney()
        {
            IDictCodeBLL dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            var isChenckMoneyWaterControlCostDeduct = dictCodeBLL.GetDictCodeBoolValueByCode("CostDeduct", "IsChenckMoneyWaterControlCostDeduct"); //是否检测管理员余额
            bool isCheckMoney = isChenckMoneyWaterControlCostDeduct.HasValue && isChenckMoneyWaterControlCostDeduct.Value;
            if ((!__isAllowAccountMinuse && !__isSuperAdmin) || isCheckMoney)
            {
                var previousCostDeduct = GetPreviousCostDeduct();
                double willPayFee = previousCostDeduct == null ? __WaterControlCostDeduct.Money : __WaterControlCostDeduct.Money - previousCostDeduct.Fee;
                return _moneyValidateBLL.SimpleMoneyValidate(__WaterControlCostDeduct.UserId, __WaterControlCostDeduct.SubjectId, __WaterControlCostDeduct.PaymentMethodEnum, willPayFee);
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
                CostDeductTypeEnum =  CostDeductType.WaterControlCostDeduct,
                Duration = 0d,
                DeductAt = DateTime.Now,
                HasClossingAccount = false
            };
            var paymentMethod = PaymentMethod.SubjectDirectorPay;
            UserAccount userAccount = null;
            _userBLL.GetPayer(__WaterControlCostDeduct.UserId, __WaterControlCostDeduct.SubjectId, out paymentMethod, out userAccount);
            if (isNew)
            {
                userAccount.Deduct(false,__WaterControlCostDeduct.Money, out realCurrency, out virtualCurrency);
                costDeduct.RealCurrency = realCurrency;
                costDeduct.VirtualCurrency = virtualCurrency;
            }
            else
            {
                userAccount.RealCurrency += costDeduct.RealCurrency.Value;
                userAccount.VirtualCurrency += costDeduct.VirtualCurrency.Value;
                userAccount.Deduct(false,__WaterControlCostDeduct.Money, out realCurrency, out virtualCurrency);
                costDeduct.RealCurrency = realCurrency;
                costDeduct.VirtualCurrency = virtualCurrency;
            }
            costDeduct.UserAccountId = userAccount.Id;
            costDeduct.CreaterId = __operatorId;
            costDeduct.WaterControlCostDeductId = __WaterControlCostDeduct.Id;
            costDeduct.WaterControlCostDeduct = __WaterControlCostDeduct;
            costDeduct.WaterControlCostDeductForLog = __WaterControlCostDeduct;
            return costDeduct;
        }

        protected override CostDeduct BusinessHandle(out Guid subjectId, out Guid userId, out PaymentMethod paymentMethod)
        {
            var previousCostDeduct = GetPreviousCostDeduct();
            subjectId = __WaterControlCostDeduct.SubjectId;
            userId = __WaterControlCostDeduct.UserId;
            paymentMethod = __WaterControlCostDeduct.PaymentMethodEnum;
            return previousCostDeduct;
        }

        protected override bool SendDeductMessage(UserAccount userAccount,out string errorMessage)
        {
            errorMessage = "";
            try
            {
                bool result = _messageHandler.SendWaterControlCostDeductNotice(__WaterControlCostDeduct,__operator);
                var previousCostDeduct = GetPreviousCostDeduct();
                if (previousCostDeduct != null && previousCostDeduct.WaterControlCostDeduct != null)
                    result = result && _messageHandler.SendDepositNotice(previousCostDeduct, previousCostDeduct.WaterControlCostDeduct, __operator);
                return result;
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
