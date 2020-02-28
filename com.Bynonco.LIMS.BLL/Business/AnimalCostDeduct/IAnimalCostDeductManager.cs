using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public interface IAnimalCostDeductManager
    {
        void Deduct(IEnumerable<Animal> animalList, DateTime endCostDeductDate, Guid? operatorId, string ip, out int successCount, out int failCount, out string errorMessage);
        void Deduct(DateTime endCostDeductDate, Guid? operatorId, string ip, out int successCount, out int failCount, out string errorMessage);
        void Deduct(Guid? operatorId, string ip, out int successCount, out int failCount, out string errorMessage);
    }
}
