using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class AnimalCostDeductBLL : BLLBase<AnimalCostDeduct>, IAnimalCostDeductBLL
    {
        public AnimalCostDeduct CreateAnimalCostDeduct(Animal animal,DateTime costDeductDate)
        {
            IUserBLL userBLL  = BLLFactory.CreateUserBLL();
            UserAccount userAccount =null;
            PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
            userBLL.GetPayer(animal.OwnerId.Value, animal.EthicsApply.SubjectId, out paymentMethod, out userAccount);
            AnimalCostDeduct animalCostDeduct = new AnimalCostDeduct();
            animalCostDeduct.AnimalId = animal.Id;
            animalCostDeduct.ChargeExpression = animal.IsUseAnimalCategoryChargeStandard ? animal.AnimalCategory.ChargeExpression : animal.ChargeExpression;
            animalCostDeduct.ChargeType = (int)(animal.IsUseAnimalCategoryChargeStandard ? animal.AnimalCategory.ChargeTypeEnum : animal.ChargeTypeEnum);
            animalCostDeduct.CreateAt = DateTime.Now;
            animalCostDeduct.DeductTime = costDeductDate;
            animalCostDeduct.Id = Guid.NewGuid();
            animalCostDeduct.IsUseAnimalCategoryChargeStandard = animal.IsUseAnimalCategoryChargeStandard;
            animalCostDeduct.Money = animal.CalUnitPrice;
            animalCostDeduct.Name = string.Format("{0}:{1}扣费", animal.Name, costDeductDate.ToString("yyyy-MM-dd"));
            animalCostDeduct.PaymentMethod = (int)paymentMethod;
            animalCostDeduct.Remark = animalCostDeduct.Name;
            animalCostDeduct.SubjectId = animal.EthicsApply.SubjectId;
            animalCostDeduct.SubjectProjectId = null;
            animalCostDeduct.UnitPrice = animal.CalUnitPrice;
            animalCostDeduct.UserId = animal.OwnerId.Value;
            return animalCostDeduct;
        }
    }
}
