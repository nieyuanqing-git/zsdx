using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.JqueryEasyUI.Core;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.BLL.Business;
using System.Collections;

namespace com.Bynonco.LIMS.BLL
{
    public class LabOrganizationBLL : BLLBase<LabOrganization>, ILabOrganizationBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private static object __lockObj = new object();
        public LabOrganizationBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
        }
        public IList<LabOrganization> GetUserAutherOrganizationList(Guid? userId)
        {
            if (!userId.HasValue) return null;
            var orgIds = __labOrganizationAuthorizedBLL.GetAuthorizedLabOrganizationIds(userId.Value, LabOrganizationAuthorizedType.User);
            if (orgIds == null || orgIds.Count() == 0) return null;
            var orgList = GetEntitiesByExpression("IsDelete=false*" + orgIds.ToFormatStr(), null, "XPath");
            return orgList;
        }
        public LabOrganization GetLabOrganizationByXPath(string xPath)
        {
            if (string.IsNullOrEmpty(xPath)) xPath = "000";
            var labOrganizationList = GetEntitiesByExpression(string.Format("IsDelete=false*XPath=\"{0}\"", xPath),null,"",true);
            return labOrganizationList == null || labOrganizationList.Count() == 0 ? null : labOrganizationList.First();
        }
        public IList<LabOrganization> GetUserManageLabOrganizationList(Guid userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false)
        {
            var user = __userBLL.GetEntityById(userId);
            if (!user.IsSuperAdmin)
                dataGridSettings.AppendAndQueryExpression(__workGroupModuleBLL.GetQueryManagerUserOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.LabOrganization, "", "", "Id")));
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct);
        }
        public bool SaveImportLabOrganization(IList<ImportLabOrganizationLog> importLabOrganizationLogList, out string errorMessage)
        {
            errorMessage = "";
            if (importLabOrganizationLogList != null && importLabOrganizationLogList.Count() == 0)
            {
                errorMessage = "导入数据为空";
                return false;
            }
            importLabOrganizationLogList = importLabOrganizationLogList.Where(p => !p.IsSuccessed).ToList();
            importLabOrganizationLogList = importLabOrganizationLogList.OrderBy(p => p.XPath).ToList();
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                IList<LabOrganization> labOrganizationList = new List<LabOrganization>();
                foreach (var item in importLabOrganizationLogList)
                {
                    if (labOrganizationList.Where(p => p.Id == item.LabOrganizationId).FirstOrDefault() == null)
                    {
                        var labOrganization = new LabOrganization();
                        labOrganization.Id = item.LabOrganizationId;
                        labOrganization.Name = item.Name;
                        labOrganization.Code = item.Code;
                        labOrganization.InputStr = ShortcutStringUtility.Chinese2Pinyin(labOrganization.Name);
                        labOrganization.XPath = item.XPath;
                        labOrganization.Type = item.Type;
                        labOrganization.ParentId = item.ParentId;
                        labOrganization.Phone = item.Phone;
                        labOrganization.Fax = item.Fax;
                        labOrganizationList.Add(labOrganization);
                    }
                }
                Add(labOrganizationList, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
        public IList<SiteMapTreeNode> GetSiteMapTreeList(string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath)) xpath = "000";
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("XPath", xpath));
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetSiteMapTree", dataParams);

            var results = (IList<object[]>)execResult.Result;
            IList<SiteMapTreeNode> siteMapTreeNodeList = new List<SiteMapTreeNode>();
            for (int i = 0; i < results.Count; i++)
            {
                var item = new SiteMapTreeNode();
                item.id = new Guid(results[i][0].ToString()); ;
                item.text = ShortcutStringUtility.GetSubString(results[i][1].ToString(), 26, "..");
                if (results[i][2].ToString() != "")
                    item.ParentId = new Guid(results[i][2].ToString());
                else
                    item.ParentId = null;
                item.url = results[i][3].ToString() == "0" ? "" : "/SiteMap/Index?Id=" + item.id;
                siteMapTreeNodeList.Add(item);
            }
            return siteMapTreeNodeList;
        }
        public IList<LabOrganizationTreeNode> GetLabOrganizationTreeList(string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath)) xpath = "000";
            //List<IDataParameter> dataParams = new List<IDataParameter>();
            //dataParams.Add(DataParameterFactory.CreateDataParameter("XPath", xpath));
            //var execResult = ProcedureAdapter.ExecuteProcedure("ProGetLabOrganizationTree", dataParams);


           // var results = (IList<object[]>)execResult.Result;
            var results = DataCacheManager.GetInstance().OrganizationList;
            IList<LabOrganizationTreeNode> labOrganizationTreeNodeList = new List<LabOrganizationTreeNode>();
            foreach (var labOrganization in results)
            {
                if (labOrganization.OrgEquipmentCount == 0) continue;
                var item = new LabOrganizationTreeNode();
                item.id = new Guid(labOrganization.Id.ToString()); ;
                item.text = ShortcutStringUtility.GetSubString(labOrganization.Name.ToString(), 26, "..")+"("+labOrganization.OrgEquipmentCount+"台)";
                if (labOrganization.ParentId.ToString() != "")
                    item.ParentId = new Guid(labOrganization.ParentId.ToString());
                else
                    item.ParentId = null;
                //item.url = labOrganization.Type.ToString() == "1" ? "" : "/SiteMap/Index?Id=" + item.id;
                item.xpath = labOrganization.XPath;
                labOrganizationTreeNodeList.Add(item);
            }
            //for (int i = 0; i < results.Count; i++)
            //{
            //    var item = new SiteMapTreeNode();
            //    item.id = new Guid(results[i][0].ToString()); ;
            //    item.text = ShortcutStringUtility.GetSubString(results[i][1].ToString(), 26, "..");
            //    if (results[i][2].ToString() != "")
            //        item.ParentId = new Guid(results[i][2].ToString());
            //    else
            //        item.ParentId = null;
            //    item.url = results[i][3].ToString() == "1" ? "" : "/SiteMap/Index?Id=" + item.id;
            //    siteMapTreeNodeList.Add(item);
            //}
            return labOrganizationTreeNodeList;
        }
        public IList<string> GetAuthorizationFlagList()
        {
            IList<string> authorizationFlagList = new List<string>();
             var obj = GetMutiResult("SELECT DISTINCT AuthorizationFlag FROM LabOrganization WHERE AuthorizationFlag IS NOT NULL AND AuthorizationFlag != '' AND AuthorizationFlag != ' '");
             if (obj != null)
             {
                 foreach (var authorizationFlag in (System.Collections.ArrayList)obj)
                 {
                     authorizationFlagList.Add(authorizationFlag.ToString());
                 }
             }
            return authorizationFlagList;
        }
        public IDictionary<Guid, Guid?> GenerateOrgAndRoomByName(string orgName, string roomName, ref XTransaction tran)
        {
            return GenerateOrgAndRoomByName(null, orgName, roomName, ref tran);
        }
        public IDictionary<Guid, Guid?> GenerateOrgAndRoomByName(Guid? parentId,string orgName, string roomName, ref XTransaction tran)
        {
            lock (__lockObj)
            {
                IDictionary<Guid, Guid?> ddResult = null;
                Guid? tempOrgId = null, tempOrgParentId = parentId, tempRoomId = null, tempRoomParentId = null, roomId = null;
                bool isNewOrg = false;
                if (string.IsNullOrWhiteSpace(orgName)) throw new ArgumentException("单位名称为空");
                var isSupress = true;
                if (tran == null)
                {
                    isSupress = false;
                    tran = SessionManager.Instance.BeginTransaction();
                }
                try
                {
                    var rootOrg = parentId == null ? GetFirstOrDefaultEntityByExpression(string.Format("ParentId=null*IsDelete=false")) : GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*IsDelete=false",parentId.Value));
                    if (null == rootOrg) throw new Exception("一级单位为空");
                    var org = GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*IsDelete=false*Type={1}", orgName.Trim(), (int)LabOrganizationType.Organziton));
                    if (org == null)
                    {
                        isNewOrg = true;
                        org = new LabOrganization() { Id = Guid.NewGuid(), Name = orgName.Trim(), ParentId = rootOrg.Id, Type = (int)LabOrganizationType.Organziton };
                        org.XPath = XPathUtility.GenerateXPath(tempOrgId, tempOrgParentId,
                             (entityId) => { return GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false", entityId.ToString())).First(); },
                             (parentEntityId) => { return GetEntitiesByExpression(string.Format("ParentId=\"{0}\"*IsDelete=false", parentEntityId.ToString())); },
                             () => { return GetEntitiesByExpression("ParentId=null*IsDelete=false"); });
                        Add(new LabOrganization[] { org }, ref tran, true);
                    }
                    if (!string.IsNullOrWhiteSpace(roomName))
                    {
                        var room = GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*IsDelete=false*Type={1}", roomName.Trim(), (int)LabOrganizationType.LabRoom));
                        if (room == null)
                        {
                            room = new LabOrganization() { Id = Guid.NewGuid(), Name = roomName.Trim(), ParentId = org.Id, Type = (int)LabOrganizationType.LabRoom };
                            if (!isNewOrg)
                            {
                                tempRoomParentId = org.Id;
                                room.XPath = XPathUtility.GenerateXPath(tempRoomId, tempRoomParentId,
                                     (entityId) => { return GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false", entityId.ToString())).First(); },
                                     (parentEntityId) => { return GetEntitiesByExpression(string.Format("ParentId=\"{0}\"*IsDelete=false", parentEntityId.ToString())); },
                                     () => { return GetEntitiesByExpression("ParentId=null*IsDelete=false"); });
                            }
                            else
                            {
                                room.XPath = org.XPath + "000";
                            }
                            Add(new LabOrganization[] { room }, ref tran, true);
                            roomId = room.Id;
                        }
                    }
                    ddResult = new Dictionary<Guid, Guid?>();
                    ddResult[org.Id] = roomId;
                    if (tran.HasTransaction && !isSupress) tran.CommitTransaction();
                }
                catch { if (tran.HasTransaction && !isSupress) tran.RollbackTransaction(); }
                finally { if (!isSupress) tran.Dispose(); }
                return ddResult;
            }
        }
        
    }
}