using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class MaterialCostDeduct : CommondCostDeduct
    {
        private IUserBLL __userBLL;
        private MaterialOutput __materialOutput;
        private Guid? __operatorId;
        private User __operator;
        private bool __isAllowAccountMinuse;
        private bool __isSuperAdmin;
        public MaterialCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects,operatorId,operatorIP)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __materialOutput = _businessObjects[0] as MaterialOutput;
            if (_businessObjects.Length > 1 && _businessObjects[1] != null)
                __operatorId = new Guid(_businessObjects[1].ToString());
            if (businessObjects.Length > 2) __isAllowAccountMinuse = bool.Parse(businessObjects[2].ToString());
            if (businessObjects.Length > 3) __isSuperAdmin = bool.Parse(businessObjects[3].ToString());
            if (__operatorId.HasValue)
                __operator = __userBLL.GetEntityById(__operatorId.Value);
        }
        private CostDeduct GetPreviousCostDeduct()
        {
            var previousCostDeduct = _costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("MaterialOutputId=\"{0}\"", __materialOutput.Id));
            return previousCostDeduct;
        }
        protected override bool CheckMoney()
        {
            if (!__isAllowAccountMinuse && !__isSuperAdmin)
            {
                var previousCostDeduct = GetPreviousCostDeduct();
                double willPayFee = previousCostDeduct == null ? __materialOutput.Money : __materialOutput.Money - previousCostDeduct.Fee;
                return _moneyValidateBLL.SimpleMoneyValidate(__materialOutput.OutputUserId, __materialOutput.SubjectId, __materialOutput.PaymentMethodEnum, willPayFee);
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
                CostDeductTypeEnum = CostDeductType.MaterialCostDeduct,
                Duration = 0d,
                DeductAt = DateTime.Now,
                HasClossingAccount = false
            };
            var paymentMethod = PaymentMethod.SubjectDirectorPay;
            UserAccount userAccount = null;
            _userBLL.GetPayer(__materialOutput.OutputUserId, __materialOutput.SubjectId, out paymentMethod, out userAccount);
            if (isNew)
            {
                userAccount.Deduct(false,__materialOutput.Money, out realCurrency, out virtualCurrency);
                costDeduct.RealCurrency = realCurrency;
                costDeduct.VirtualCurrency = virtualCurrency;
            }
            else
            {
                userAccount.RealCurrency += costDeduct.RealCurrency.Value;
                userAccount.VirtualCurrency += costDeduct.VirtualCurrency.Value;
                userAccount.Deduct(false,__materialOutput.Money, out realCurrency, out virtualCurrency);
                costDeduct.RealCurrency = realCurrency;
                costDeduct.VirtualCurrency = virtualCurrency;
            }
            costDeduct.UserAccountId = userAccount.Id;
            costDeduct.CreaterId = __operatorId;
            costDeduct.MaterialOutputId = __materialOutput.Id;
            costDeduct.MaterialOutput = __materialOutput;
            return costDeduct;
        }

        protected override CostDeduct BusinessHandle(out Guid subjectId, out Guid userId, out PaymentMethod paymentMethod)
        {
            var previousCostDeduct = GetPreviousCostDeduct();
            subjectId = __materialOutput.SubjectId;
            userId = __materialOutput.OutputUserId;
            paymentMethod = __materialOutput.PaymentMethodEnum;
            return previousCostDeduct;
        }

        protected override bool SendDeductMessage(UserAccount userAccount,out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var result = _messageHandler.SendMaterialCostDeductNotice(__materialOutput, __operator);
                var previousCostDeduct = GetPreviousCostDeduct();
                if (previousCostDeduct != null && previousCostDeduct.MaterialOutput != null)
                    result = result && _messageHandler.SendDepositNotice(previousCostDeduct, previousCostDeduct.MaterialOutput, null);
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
