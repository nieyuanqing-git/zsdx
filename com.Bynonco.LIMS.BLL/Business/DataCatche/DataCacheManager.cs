using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Enum;
using com.august.Core.XQuery;
using System.Threading;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class DataCacheManager : IDataCacheManager
    {
        private static IDataCacheManager __dataCacheManager;
        private static object __lockObj = new object();
        private IEquipmentCategoryBLL __equipmentCategoryBLL;
        private ILabOrganizationBLL __labOrganizationBLL;
        private DataCacheManager()
        {
            __equipmentCategoryBLL = BLLFactory.CreateEquipmentCategoryBLL();
            __labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
        }
        public static IDataCacheManager GetInstance()
        {
            if (__dataCacheManager == null)
            {
                lock (__lockObj)
                {
                    __dataCacheManager = new DataCacheManager();
                }
            }
            return __dataCacheManager;
        }

        public void LoadAllCache()
        {
            new Thread(new ParameterizedThreadStart((obj) => { GetEquipmentCategoryJson(); })).Start();
            var threadOrganizationAndLabRoom = new Thread(new ParameterizedThreadStart((obj) => { GetOrganizationAndLabRoomJson(); }));
            threadOrganizationAndLabRoom.Start();

            var threadOrganization = new Thread(new ParameterizedThreadStart((obj) =>
            {
                threadOrganizationAndLabRoom.Join();
                GetOrganizationJson();
            }));
            threadOrganization.Start();

            var threadLabRoom = new Thread(new ParameterizedThreadStart((obj) =>
            {
                threadOrganization.Join();
                GetLabRoomJson();
            }));
            threadLabRoom.Start();
        }
        public void RefreshAllCache()
        {
            RefreshEquipmentCategoryCache();
            RefreshOrganizationCache();
            LoadAllCache();
        }

        public void RefreshEquipmentCategoryCache()
        {
            __equipmentCategoryList = null;
            __equipmentCategoryJson = null;
        }
        private object __lockEquipmentCategoryObj = new object();
        private IList<EquipmentCategory> __equipmentCategoryList;
        public IList<EquipmentCategory> EquipmentCategoryList
        {
            get
            {
                if (__equipmentCategoryList == null)
                {
                    lock (__lockEquipmentCategoryObj)
                    {
                        var queryExpression = string.Format("IsDelete=false*IsStop=false");
                        __equipmentCategoryList = __equipmentCategoryBLL.GetEntitiesByExpression(queryExpression, null, "XPath");

                    }
                }
                return __equipmentCategoryList;
            }
        }
        private string __equipmentCategoryJson;
        public string GetEquipmentCategoryJson(bool isShowAllNode = true)
        {
            if (string.IsNullOrWhiteSpace(__equipmentCategoryJson))
                __equipmentCategoryJson = EquipmentCategoryList.ToJsionTreeDataGridData(null, null, isShowAllNode);
            return __equipmentCategoryJson;
        }
        public string GetEquipmentCategoryJson(IEnumerable<Guid> equipmentCategoryIds, bool isShowAllNode = true)
        {
            IEnumerable<EquipmentCategory> selectedEquipmentCategories = null;
            if (equipmentCategoryIds != null && equipmentCategoryIds.Count() > 0)
            {
                selectedEquipmentCategories = __equipmentCategoryBLL.GetEntitiesByExpression(equipmentCategoryIds.ToFormatStr());
                return EquipmentCategoryList.ToJsionTreeDataGridData(selectedEquipmentCategories, null, isShowAllNode);
            }
            return GetEquipmentCategoryJson(isShowAllNode);
        }


        private object __lockLabOrganizationObj = new object();
        public void RefreshOrganizationCache()
        {
            __organizationList = null;
            __organizationJson = null;
            __labRoomList = null;
            __labRoomJson = null;
            __organizationAndLabRoomList = null;
            __organizationAndLabRoomJson = null;
        }
        private IList<LabOrganization> __organizationAndLabRoomList;
        public IList<LabOrganization> OrganizationAndLabRoomList
        {
            get
            {
                if (__organizationAndLabRoomList == null)
                {
                    lock (__lockLabOrganizationObj)
                    {
                        var queryExpression = "IsDelete=false";
                        __organizationAndLabRoomList = __labOrganizationBLL.GetEntitiesByExpression(queryExpression, null, "XPath");
                    }
                }
                return __organizationAndLabRoomList;
            }

        }
        private string __organizationAndLabRoomJson;
        public string GetOrganizationAndLabRoomJson(bool isShowAllNode = true)
        {
            if (string.IsNullOrWhiteSpace(__organizationAndLabRoomJson))
                __organizationAndLabRoomJson = OrganizationAndLabRoomList.ToJsionTreeDataGridData(null, null, isShowAllNode);
            return __organizationAndLabRoomJson;
        }

        public string GetOrganizationAndLabRoomJson(IEnumerable<Guid> ids, bool isShowAllNode = true)
        {
            IList<LabOrganization> selectedOrganizationAndLabRooms = null;
            if (ids != null && ids.Count() > 0)
            {
                selectedOrganizationAndLabRooms = __labOrganizationBLL.GetEntitiesByExpression(ids.ToFormatStr());
                return __organizationAndLabRoomJson = OrganizationAndLabRoomList.ToJsionTreeDataGridData(selectedOrganizationAndLabRooms, null, isShowAllNode);
            }
            return GetOrganizationAndLabRoomJson(isShowAllNode);
        }

        private IList<LabOrganization> __organizationList;
        public IList<LabOrganization> OrganizationList
        {
            get
            {
                if (__organizationList == null)
                {

                    lock (__lockLabOrganizationObj)
                    {
                        var queryExpression = string.Format("IsDelete=false*IsStop=false*Type={0}", (int)LabOrganizationType.Organziton);
                        if (OrganizationAndLabRoomList != null && OrganizationAndLabRoomList.Count > 0)
                            __organizationList = OrganizationAndLabRoomList.Where(queryExpression, null).OrderBy(p => p.XPath).ToList();
                    }
                }
                return __organizationList;
            }
        }
        private string __organizationJson;
        public string GetOrganizationJson(bool isShowAllNode = true)
        {
            if (string.IsNullOrWhiteSpace(__organizationJson))
                __organizationJson = OrganizationList.ToJsionTreeDataGridData(null, null, isShowAllNode);
            return __organizationJson;
        }
        public string GetOrganizationJson(IEnumerable<Guid> organizationIds, bool isShowAllNode = true)
        {
            IEnumerable<LabOrganization> selectedOrganizations = null;
            if (organizationIds != null && organizationIds.Count() > 0)
            {
                selectedOrganizations = OrganizationAndLabRoomList.Where(organizationIds.ToFormatStr(), null);
                return OrganizationList.ToJsionTreeDataGridData(selectedOrganizations, null, isShowAllNode);
            }
            return GetOrganizationJson(isShowAllNode);
        }
        public string GetOrganizationJson(IEnumerable<LabOrganization> selectedOrganizations, bool isShowAllNode = true)
        {
            if(OrganizationList!=null) 
                foreach (var item in OrganizationList) item.@checked = false;
            if (selectedOrganizations != null && selectedOrganizations.Count() > 0)
            {
                return OrganizationList.ToJsionTreeDataGridData(selectedOrganizations, null, isShowAllNode);
            }
            return GetOrganizationJson(isShowAllNode);
        }

        private IList<LabOrganization> __labRoomList;
        public IList<LabOrganization> LabRoomList
        {
            get
            {
                if (__labRoomList == null)
                {
                    lock (__lockLabOrganizationObj)
                    {
                        var queryExpression = string.Format("IsDelete=false*IsStop=false*Type={0}", (int)LabOrganizationType.LabRoom);
                        if (OrganizationAndLabRoomList != null && OrganizationAndLabRoomList.Count > 0)
                            __labRoomList = OrganizationAndLabRoomList.Where(queryExpression, null).OrderBy(p => p.XPath).ToList();
                    }
                }
                return __labRoomList;
            }
        }
        private string __labRoomJson;
        public string GetLabRoomJson(bool isShowAllNode = true)
        {
            if (string.IsNullOrWhiteSpace(__labRoomJson))
                __labRoomJson = LabRoomList.ToJsionTreeDataGridData(null, null, isShowAllNode);
            return __labRoomJson;
        }
        public string GetLabRoomJson(IEnumerable<Guid> labRoomIds, bool isShowAllNode = true)
        {
            IEnumerable<LabOrganization> selectedLabRooms = null;
            if (labRoomIds != null && labRoomIds.Count() > 0)
            {
                selectedLabRooms = OrganizationAndLabRoomList.Where(labRoomIds.ToFormatStr(), null);
                return LabRoomList.ToJsionTreeDataGridData(selectedLabRooms, null, isShowAllNode);
            }
            return GetLabRoomJson(isShowAllNode);
        }
    }
}
