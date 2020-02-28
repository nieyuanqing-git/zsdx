using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class UserSyncManager : IUserSyncManager
    {
        private static ISchoolUserBLL __schoolUserBLL;
        private static IUserBLL __userBLL;
        private static IUserSyncManager __userSyncManager;
        private static object __lockObjCreate = new object();
        private int __syncCountPerTime = 10;
        private string __syscQueryExpression;
        static UserSyncManager()
        {
            __schoolUserBLL = BLLFactory.CreateSchoolUserBLL();
            __userBLL = BLLFactory.CreateUserBLL();
        }
        private UserSyncManager(int syncCountPerTime, string syscQueryExpression) 
        {
            __syncCountPerTime = syncCountPerTime;
            __syscQueryExpression = syscQueryExpression;
        }
        public static IUserSyncManager GetInstance(int syncCountPerTime, string syscQueryExpression)
        {
            if (__userSyncManager == null)
            {
                lock (__lockObjCreate)
                {
                    __userSyncManager = new UserSyncManager(syncCountPerTime, syscQueryExpression);
                }
            }
            return __userSyncManager;
        }
        public void Sync(out int totalCount ,out int successCount, out int failCount, out string errorMessage)
        {
            __schoolUserBLL.SyncSchoolUser(__syscQueryExpression,__syncCountPerTime, null,out totalCount,out successCount,out failCount, out errorMessage);
        }
    }
}
