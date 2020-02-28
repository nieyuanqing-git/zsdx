using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Utility;
using System.Data;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class ZGSYDXOrganizationSyncHandler : DefaultOranizationSyncHandler
    {
        public ZGSYDXOrganizationSyncHandler(int syncCountPerTime, string syncQueryExpression)
            : base(syncCountPerTime, syncQueryExpression) { }
        private bool ImportSchoolOrganizationByExcel(out string errorMessage)
        {
            errorMessage="";
            try
            {
                string organizationExcelPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\App_Data\\中国石油大学组织机构.xls";
                DataSet ds = ExcelUtility.ConvertToDataSet(organizationExcelPath);
                if (ds.Tables.Count > 0)
                {
                    var dt = ds.Tables[0];
                    dt.DefaultView.Sort = "LSDWH2 ASC";
                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        XTransaction tran = SessionManager.Instance.BeginTransaction();
                        try
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var orgCode = dt.Rows[i]["DWH"].ToString().Trim();
                                var orgName = dt.Rows[i]["DWMC"].ToString().Trim();
                                var parentOrgCode = dt.Rows[i]["LSDWH2"].ToString();
                                if (string.IsNullOrWhiteSpace(parentOrgCode)) parentOrgCode = null;
                                var schoolOrganization = _schoolOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"", orgCode.Trim()));
                                bool isNew = schoolOrganization == null;
                                if (schoolOrganization == null)
                                {
                                    schoolOrganization = new SchoolOrganization()
                                    {
                                        Id = Guid.NewGuid(),
                                        Code = orgCode,
                                        IsHandled = false
                                    };
                                }
                                schoolOrganization.ParentCode = parentOrgCode;
                                schoolOrganization.CodeLen = orgCode.Length;
                                schoolOrganization.CodeNo = int.Parse(orgCode);
                                schoolOrganization.Name = orgName;
                                if (isNew)
                                    _schoolOrganizationBLL.Add(new SchoolOrganization[] { schoolOrganization }, ref tran, true);
                                else
                                    _schoolOrganizationBLL.Save(new SchoolOrganization[] { schoolOrganization }, ref tran, true);
                                if (SchoolOrganizationList == null) SchoolOrganizationList = new List<SchoolOrganization>();
                                SchoolOrganizationList.Add(schoolOrganization);
                            }
                            if (tran.HasTransaction) tran.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            errorMessage = ex.Message;
                            if (tran.HasTransaction) tran.RollbackTransaction();
                            return false;
                        }
                        finally { tran.Dispose(); }
                    }
                }
                return true;
            }
            catch (Exception ex) 
            {
                errorMessage = ex.Message;
                return false;
            }

        }
        public override void Sync(out int totalCount, out int successCount, out int failCount, out int skipCount, out string errorMessage)
        {
            errorMessage = "";
            totalCount = 0;
            successCount = 0;
            failCount = 0;
            skipCount = 0;
            if (ImportSchoolOrganizationByExcel(out errorMessage))
            {
                base.Sync(out totalCount, out successCount, out failCount, out skipCount, out errorMessage);
            }
        }
    }
}
