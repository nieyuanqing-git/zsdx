using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public interface IUserSyncManager
    {
        void Sync(out int totalCount, out int successCount, out int failCount, out string errorMessage);
    }
}
