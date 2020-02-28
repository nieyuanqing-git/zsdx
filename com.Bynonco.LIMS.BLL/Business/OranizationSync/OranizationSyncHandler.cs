using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class OranizationSyncHandler
    {
        protected ISchoolOrganizationBLL _schoolOrganizationBLL;
        protected ILabOrganizationBLL _labOrganizationBLL;
        protected IViewSchoolUserBLL _viewSchoolUserBLL; 
        protected static int _syncCountPerTime = 10;
        protected static string _syscQueryExpression;
        public OranizationSyncHandler(int syncCountPerTime, string syncQueryExpression)
        {
            _schoolOrganizationBLL = BLLFactory.CreateSchoolOrganizationBLL();
            _labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            _viewSchoolUserBLL = BLLFactory.CreateViewSchoolUserBLL();
            SchoolOrganizationList = _schoolOrganizationBLL.GetEntitiesByExpression(_syscQueryExpression, null, GenerateWillHandleOranizationOrderExpression());
            _syncCountPerTime = syncCountPerTime;
            _syscQueryExpression = syncQueryExpression;
        }
        public abstract void Sync(out int totalCount, out int successCount, out int failCount, out int skipCount, out string errorMessage);
        protected abstract string GenerateWillHandleOranizationOrderExpression();
        protected IList<SchoolOrganization> SchoolOrganizationList { get; set; }
    }
}
