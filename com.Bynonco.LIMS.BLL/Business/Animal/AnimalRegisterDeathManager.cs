using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class AnimalRegisterDeathManager : IAnimalRegisterDeathManager
    {
        private static IAnimalBLL __animalBLL;
        private static IAnimalRegisterDeathManager __animalRegisterDeathManager;
        private static object __lockObjCreate = new object();
        private static object __lockObjBusiness = new object();
        static AnimalRegisterDeathManager()
        {
            __animalBLL = BLLFactory.CreateAnimalBLL();
        }
        private AnimalRegisterDeathManager() { }
        public static IAnimalRegisterDeathManager GetInstance()
        {
            if (__animalRegisterDeathManager == null)
            {
                lock (__lockObjCreate)
                {
                    __animalRegisterDeathManager = new AnimalRegisterDeathManager();
                }
            }
            return __animalRegisterDeathManager;
        }
        public void ConfirmRegisterDeath(Guid? operatorId, out int successCount, out int failCount, out string errorMessage)
        {
            lock (__lockObjBusiness)
            {
                successCount = 0;
                failCount = 0;
                errorMessage = "";
                try
                {
                    var unRegisterDeathAnimalList = __animalBLL.GetEntitiesByExpression(string.Format("Status={0}*StoreStatus={1}*ConfirmDeathOperatorId=null*IsDelete=false", (int)AnimalStatus.Live, (int)AnimalStoreStatus.RegisterDeath));
                    if (unRegisterDeathAnimalList == null && unRegisterDeathAnimalList.Count == 0) return;
                    StringBuilder sbErrorMsg = new StringBuilder();
                    foreach (var unRegisterDeathAnimal in unRegisterDeathAnimalList)
                    {
                        try
                        {
                            if (!unRegisterDeathAnimal.RegisterDeathDate.HasValue || unRegisterDeathAnimal.RegisterDeathDate.Value.AddDays(unRegisterDeathAnimal.ConfirmDeathMaxDays) > DateTime.Now) continue;
                            if (__animalBLL.ConfirmRegisterDeath(unRegisterDeathAnimal, out errorMessage))
                            {
                                throw new Exception(errorMessage);
                            }
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            sbErrorMsg.AppendFormat("编码为:{0},动物名称:{1}，登记死亡失败，原因:{2}",
                                                    unRegisterDeathAnimal.Id,
                                                    unRegisterDeathAnimal.Name,
                                                    ex.Message).AppendLine();
                            failCount++;
                        }
                    }
                    errorMessage = sbErrorMsg.ToString();
                }
                catch (Exception ex) { errorMessage = ex.Message; }
            }
        }
    }
}
