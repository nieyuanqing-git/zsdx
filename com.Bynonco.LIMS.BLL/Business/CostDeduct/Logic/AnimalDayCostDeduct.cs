using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
namespace com.Bynonco.LIMS.BLL
{
    public class AnimalDayCostDeduct : CommondCostDeduct
    {

        private Animal __animal;
        private Guid? __operatorId;
        private DateTime __endCostDeductDate;
        private IAnimalBLL __animalBLL;
        private IAnimalCostDeductBLL __animalCostDeductBLL;
        private AnimalCostDeduct __animalCostDeduct;
        private IViewNeedCostDeductAnimalBLL __viewNeedCostDeductAnimalBLL;
        public AnimalDayCostDeduct(object[] businessObjects, DateTime endCostDeductDate, Guid? operatorId, string operatorIP)
            : base(businessObjects,operatorId,operatorIP)
        {
            __animalBLL = BLLFactory.CreateAnimalBLL();
            __animalCostDeductBLL = BLLFactory.CreateAnimalCostDeductBLL();
            __viewNeedCostDeductAnimalBLL = BLLFactory.CreateViewNeedCostDeductAnimalBLL();
            this.__endCostDeductDate = endCostDeductDate.AddDays(1).AddSeconds(-1);
            __animal = _businessObjects[0] as Animal;
            if (_businessObjects.Length > 1 && _businessObjects[1] != null)
                __operatorId = new Guid(_businessObjects[1].ToString());
            if (_businessObjects.Length > 2 && _businessObjects[2] != null)
                __animalCostDeduct = _businessObjects[1] as AnimalCostDeduct;
        }
        private void GetBeginAndEndDate(out double calFee,out DateTime beginDate,out DateTime endDate)
        {
            int days = 0;
            calFee = 0d;
            beginDate = __animal.InDate.Value;
            endDate = DateTime.Now.Date.AddSeconds(-1);
            switch ((AnimalStoreStatus)__animal.StoreStatus)
            {
                case AnimalStoreStatus.In:
                    if (__animal.InDate.Value.Date <= __endCostDeductDate)
                    {
                        if (__animal.LatestCostDeductTime.HasValue) beginDate = __animal.LatestCostDeductTime.Value.Date.AddDays(1);
                        days = (int)(__endCostDeductDate - __animal.InDate.Value.Date).TotalDays;
                        endDate = __endCostDeductDate;
                    }
                    break;
                case AnimalStoreStatus.GetOut:
                    if (__animal.InDate.Value.Date <= __animal.OutDate.Value.Date)
                    {
                        days = (int)(__animal.OutDate.Value.Date - __animal.InDate.Value.Date).TotalDays;
                        endDate = __animal.OutDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                    }
                    break;
                case AnimalStoreStatus.RegisterDeath:
                case AnimalStoreStatus.Die:
                    if (__animal.InDate.Value.Date <= __animal.DieTime.Value.Date)
                    {
                        days = (int)(__animal.DieTime.Value.Date - __animal.InDate.Value.Date).TotalDays;
                        endDate = __animal.DieTime.Value.Date.AddDays(1).AddMilliseconds(-1);
                    }
                    break;
            }
            calFee = days == 0 ? 1 * __animal.CalUnitPrice : days * __animal.CalUnitPrice;
        }
        protected override Model.CostDeduct CreateCostDeduct()
        {
            var calFee = 0d;
            UserAccount userAccount = null;
            var date = DateTime.Now.Date.AddDays(-1);
            double realCurrency = 0d, virtualCurrency = 0d;
            var paymentMethod = PaymentMethod.SubjectDirectorPay;
            DateTime beginDate = __animal.InDate.Value, endDate = DateTime.MaxValue;
            var costDeduct = new CostDeduct()
            {
                Id = Guid.NewGuid(),
                CalcDuration = 0d,
                CostDeductTypeEnum = CostDeductType.Animal,
                Duration = 0d,
                DeductAt = DateTime.Now,
                HasClossingAccount = false
            };
            var animalCostDeduct = new AnimalCostDeduct() 
            {
                Id = Guid.NewGuid(), 
                UserId = __animal.OwnerId.Value, 
                CreateAt = DateTime.Now, 
                DeductTime = DateTime.Now, 
                SubjectId = __animal.EthicsApply.SubjectId,
                AnimalId = __animal.Id, 
                UnitPrice = __animal.CalUnitPrice,
                ChargeType=__animal.ChargeType, 
                ChargeExpression = __animal.ChargeExpression,
                IsUseAnimalCategoryChargeStandard = __animal.IsUseAnimalCategoryChargeStandard
            };
            GetBeginAndEndDate(out calFee, out beginDate, out endDate);
            animalCostDeduct.BeginDate = beginDate;
            animalCostDeduct.EndDate = endDate;
            animalCostDeduct.Money = calFee;
            animalCostDeduct.Name = string.Format("{0}:{1}~{2}扣费", __animal.Name, animalCostDeduct.BeginDate.ToString("yyyy-MM-dd"), animalCostDeduct.EndDate.ToString("yyyy-MM-dd"));
            animalCostDeduct.Remark = animalCostDeduct.Name;
            _userBLL.GetPayer(__animal.OwnerId.Value, __animal.EthicsApply.SubjectId, out paymentMethod, out userAccount);
            userAccount.Deduct(false, __animal.CalUnitPrice, out realCurrency, out virtualCurrency);
            costDeduct.RealCurrency = realCurrency;
            costDeduct.VirtualCurrency = virtualCurrency;
            costDeduct.UserAccountId = userAccount.Id;
            costDeduct.CreaterId = __operatorId;
            costDeduct.AnimalCostDeductId = animalCostDeduct.Id;
            costDeduct.AnimalCostDeduct = animalCostDeduct;
            return costDeduct;
        }

        protected override void BusinessHandleAfterDeduct(ref august.DataLink.XTransaction tran)
        {
            var calFee = 0d;
            DateTime beginDate = __animal.InDate.Value, endDate = DateTime.MaxValue;
            GetBeginAndEndDate(out calFee, out beginDate, out endDate);
            if (endDate > DateTime.Now.Date.AddSeconds(-1)) throw new Exception("最大截止扣费时间是昨天,截止扣费时间大于昨天");
            if (__animal.LatestCostDeductTime.HasValue && endDate <= __animal.LatestCostDeductTime.Value) throw new Exception(string.Format("已扣费,最近一次扣费时间为:{0}", __animal.LatestCostDeductTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            var viewNeedCostDeductAnimal = __viewNeedCostDeductAnimalBLL.GetEntityById(__animal.Id);
            if (viewNeedCostDeductAnimal == null) throw new Exception("当前状态不满足扣费条件");
            __animal.LatestCostDeductTime = endDate;
            __animalBLL.Save(new Animal[] { __animal }, ref tran, true);
           
        }
        protected override bool SendDeductMessage(UserAccount userAccount, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                _messageHandler.SendAnimalCostDeductNotice(__animalCostDeduct, null);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }

        protected override CostDeduct BusinessHandle(out Guid subjectId, out Guid userId, out PaymentMethod paymentMethod)
        {
            UserAccount userAccount = null;
            if (__animalCostDeduct != null)
            {
                subjectId = __animalCostDeduct.SubjectId;
                userId = __animalCostDeduct.UserId;
                paymentMethod = (PaymentMethod)__animalCostDeduct.PaymentMethod;
                return _costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("AnimalCostDeductId=\"{0}\"", __animalCostDeduct.Id));
            }
            else
            {
                subjectId = __animal.EthicsApply.SubjectId;
                userId = __animal.OwnerId.Value;
                _userBLL.GetPayer(userId, subjectId, out paymentMethod, out userAccount);
                return null;
            }
           
        }
    }
}
