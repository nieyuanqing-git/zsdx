using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public interface IOranizationSyncManager
    {
        void Sync(out int totalCount, out int successCount, out int failCount, out int skipCount, out string errorMessage);
    }
}
