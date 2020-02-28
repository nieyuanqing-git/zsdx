using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentSyncManager : IEquipmentSyncManager
    {
        private static IEquipmentImportBLL __equipmentImportBLL;
        private static IUserBLL __userBLL;
        private static IEquipmentSyncManager __equipmentSyncManager;
        private static object __lockObjCreate = new object();
        private int __syncCountPerTime = 10;
        private string __syscQueryExpression;
        static EquipmentSyncManager()
        {
            __equipmentImportBLL = BLLFactory.CreateEquipmentImportBLL();
        }
        private EquipmentSyncManager(int syncCountPerTime, string syscQueryExpression) 
        {
            __syncCountPerTime = syncCountPerTime;
            __syscQueryExpression = syscQueryExpression;
        }
        public static IEquipmentSyncManager GetInstance(int syncCountPerTime, string syscQueryExpression)
        {
            if (__equipmentSyncManager == null)
            {
                lock (__lockObjCreate)
                {
                    __equipmentSyncManager = new EquipmentSyncManager(syncCountPerTime, syscQueryExpression);
                }
            }
            return __equipmentSyncManager;
        }
        public void Sync(out int totalCount ,out int successCount, out int failCount, out string errorMessage)
        {
            __equipmentImportBLL.SyncEquipmentImport(__syscQueryExpression, __syncCountPerTime, null, out totalCount, out successCount, out failCount, out errorMessage);
        }
    }
}
