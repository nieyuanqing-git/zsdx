using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class OranizationSyncHandlerFactory
    {
        public static OranizationSyncHandler GetOranizationSyncHandler(ICutomer customer, int syncCountPerTime, string syscQueryExpression)
        {
            if (customer is ZGSYDXCustomer) return new ZGSYDXOrganizationSyncHandler(syncCountPerTime, syscQueryExpression);
            return new DefaultOranizationSyncHandler(syncCountPerTime, syscQueryExpression);
        }
    }
}
