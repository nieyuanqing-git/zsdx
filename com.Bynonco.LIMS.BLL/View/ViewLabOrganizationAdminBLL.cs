using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
namespace com.Bynonco.LIMS.BLL
{
    public class ViewLabOrganizationAdminBLL : BLLBase<ViewLabOrganizationAdmin>, IViewLabOrganizationAdminBLL
    {
        private ILabOrganizationBLL __labOrganizationBLL;
        public ViewLabOrganizationAdminBLL()
        {
           __labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
        }
        public IList<LabOrganization> GetLabOrganizationAdminList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!userId.HasValue) return null;
            var viewLabOrganizationAdminList = GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.Value));
            if (viewLabOrganizationAdminList != null && viewLabOrganizationAdminList.Count() > 0)
            {
                string querySql = "";
                foreach (var item in viewLabOrganizationAdminList)
                    querySql += (querySql == "" ? "" : "+") + string.Format("XPath→\"{0}\"", item.OrgXPath);
                dataGridSettings.AppendAndQueryExpression("IsDelete=false*IsStop=false");
                if(querySql != "") dataGridSettings.AppendAndQueryExpression(querySql);
                var labOrganizationList = __labOrganizationBLL.GetEntitiesByExpression(dataGridSettings, null, "XPath", isSupressLazyLoad);
                if (labOrganizationList != null && labOrganizationList.Count() > 0)
                    return labOrganizationList;
            }
            return null;
        }
    }
}
