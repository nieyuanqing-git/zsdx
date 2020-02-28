using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class ZjdxUserSyncManager : IZjdxUserSyncManager
    {
        private static IUserBLL __userBLL;
        private static IZjdxUserSyncManager __zjdxUserSyncManager;
        private static object __lockObjCreate = new object();
        private int __syncCountPerTime = 10;
        static ZjdxUserSyncManager()
        {           
            __userBLL = BLLFactory.CreateUserBLL();
        }
        private ZjdxUserSyncManager(int syncCountPerTime) 
        {
            __syncCountPerTime = syncCountPerTime;
        }
        public static IZjdxUserSyncManager GetInstance(int syncCountPerTime)
        {
            if (__zjdxUserSyncManager == null)
            {
                lock (__lockObjCreate)
                {
                    __zjdxUserSyncManager = new ZjdxUserSyncManager(syncCountPerTime);
                }
            }
            return __zjdxUserSyncManager;
        }
        public void Sync(out int totalCount ,out int successCount, out int failCount, out string errorMessage)
        {
            __userBLL.SyncZjdxUser( __syncCountPerTime, null, out totalCount, out successCount, out failCount, out errorMessage);
        }
    }
}
