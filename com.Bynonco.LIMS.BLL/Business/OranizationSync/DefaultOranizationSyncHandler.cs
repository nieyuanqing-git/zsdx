using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.Core.XQuery;
using com.Bynonco.LIMS.Utility;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class DefaultOranizationSyncHandler : OranizationSyncHandler
    {
        public DefaultOranizationSyncHandler(int syncCountPerTime, string syncQueryExpression) : base(syncCountPerTime, syncQueryExpression) { }
        public override void Sync(out int totalCount, out int successCount, out int failCount, out int skipCount, out string errorMessage)
        {
            errorMessage = "";
            totalCount = 0;
            successCount = 0;
            failCount = 0;
            skipCount = 0;
            if (SchoolOrganizationList != null && SchoolOrganizationList.Count > 0)
            {
                totalCount = SchoolOrganizationList.Count;
                foreach (var schoolOrganization in SchoolOrganizationList)
                {
                    XTransaction tran = null;
                    bool isDelete = false;
                    try
                    {
                        if (!JudegeIsNeedSync(schoolOrganization))
                        {
                            skipCount++;
                            continue;
                        }
                        tran = SessionManager.Instance.BeginTransaction();
                        Guid? parentId = null;
                        if (!string.IsNullOrWhiteSpace(schoolOrganization.ParentCode))
                        {

                            var parentSchoolOrg = SchoolOrganizationList.FirstOrDefault(p => p.Code == schoolOrganization.ParentCode);
                            if (parentSchoolOrg == null) continue;
                            var parentOrg = _labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"", schoolOrganization.ParentCode.Trim()));
                            if (parentOrg == null)
                                parentOrg = _labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"", parentSchoolOrg.Name.Trim()));
                            if (parentOrg == null) continue;
                            parentId = parentOrg.Id;
                            isDelete = parentOrg.IsDelete.HasValue && parentOrg.IsDelete.Value;
                        }
                        var labOrganization = new LabOrganization() { Id = Guid.NewGuid(), LabOrganizationType = Model.Enum.LabOrganizationType.Organziton, IsDelete = isDelete, IsStop = false };
                        labOrganization.Name = schoolOrganization.Name;
                        labOrganization.Code = schoolOrganization.Code;
                        labOrganization.ParentId = parentId;
                        labOrganization.Phone = schoolOrganization.Phone;
                        labOrganization.Fax = schoolOrganization.Fax;
                        labOrganization.InputStr = ShortcutStringUtility.GetFirstPYLetter(labOrganization.Name);
                        labOrganization.LatitudeAndlongitude = schoolOrganization.LatitudeAndlongitude;
                        if (!isDelete)
                        {
                            labOrganization.XPath = XPathUtility.GenerateXPath(null, parentId,
                                (entityId) => { return _labOrganizationBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", entityId.ToString())).First(); },
                                (parentEntityId) => { return _labOrganizationBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", parentEntityId.ToString())); },
                                () => { return _labOrganizationBLL.GetEntitiesByExpression("ParentId=null"); });
                        }
                        _labOrganizationBLL.Add(new LabOrganization[] { labOrganization }, ref tran, true);
                        schoolOrganization.IsHandled = true;
                        _schoolOrganizationBLL.Save(new SchoolOrganization[] { schoolOrganization }, ref tran, true);
                        tran.CommitTransaction();
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                        failCount++;
                        if (tran != null) tran.RollbackTransaction();
                        break;
                    }
                    finally { if (tran != null) tran.Dispose(); }
                }
                       
            }
        }
        protected bool JudegeIsNeedSync(SchoolOrganization schoolOrganization)
        {
            if (SchoolOrganizationList != null && SchoolOrganizationList.Count > 0)
            {
                var orgQueryExpression = string.Format("Name∽\"{0}\"+Code=\"{1}\"", schoolOrganization.Name.Trim(), schoolOrganization.Code.Trim());
                var orgCount = _labOrganizationBLL.CountModelListByExpression(orgQueryExpression);
                if (orgCount > 0) return false;
            }
            var userQueryExpression = string.Format("OrganizationNo=\"{0}\"", schoolOrganization.Code.Trim());
            var userCount = _viewSchoolUserBLL.CountModelListByExpression(userQueryExpression);
            if (userCount == 0)
            {
                var children = SchoolOrganizationList.Where(p => p.ParentCode == schoolOrganization.Code.Trim());
                if (children != null && children.Count() > 0)
                {
                    userQueryExpression = children.Select(p => p.Code).ToFormatStr("OrganizationNo");
                    userCount = _viewSchoolUserBLL.CountModelListByExpression(userQueryExpression);
                    if (userCount == 0) return false;
                }
            }
            return true;
        }
        protected override string GenerateWillHandleOranizationOrderExpression()
        {
            return "ParentCode A,CodeNo A";
        }
    }
}
