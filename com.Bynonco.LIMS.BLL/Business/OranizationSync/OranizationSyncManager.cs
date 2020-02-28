using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class OranizationSyncManager : IOranizationSyncManager
    {
        private static IOranizationSyncManager __oranizationSyncManager;
        private static ICutomer __costomer = CustomerFactory.GetCustomer();
        private static object __lockObj = new object();
        private static int __syncCountPerTime = 10;
        private static string __syscQueryExpression;
        private OranizationSyncManager() { }
        public static IOranizationSyncManager GetInstance(int syncCountPerTime, string syscQueryExpression)
        {
            __syncCountPerTime = syncCountPerTime;
            __syscQueryExpression = syscQueryExpression;
            if (__oranizationSyncManager == null)
            {
                lock (__lockObj)
                {
                    __oranizationSyncManager = new OranizationSyncManager();
                }
            }
            return __oranizationSyncManager;
        }
        public void Sync(out int totalCount, out int successCount, out int failCount,out int skipCount, out string errorMessage)
        {
            var organiztionSyncHandler = OranizationSyncHandlerFactory.GetOranizationSyncHandler(__costomer, __syncCountPerTime, __syscQueryExpression);
            organiztionSyncHandler.Sync(out totalCount, out successCount, out failCount,out skipCount,out errorMessage);
        }
    }
}
