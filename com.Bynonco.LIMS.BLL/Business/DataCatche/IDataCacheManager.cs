using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business
{
    public interface IDataCacheManager
    {
        void LoadAllCache();
        void RefreshAllCache();

        void RefreshEquipmentCategoryCache();
        IList<EquipmentCategory> EquipmentCategoryList { get; }
        string GetEquipmentCategoryJson(bool isShowAllNode = true);
        string GetEquipmentCategoryJson(IEnumerable<Guid> equipmentCategoryIds, bool isShowAllNode = true);

        void RefreshOrganizationCache();
        IList<LabOrganization> OrganizationList { get; }
        string GetOrganizationJson(bool isShowAllNode = true);
        string GetOrganizationJson(IEnumerable<Guid> organizationIds, bool isShowAllNode = true);
        string GetOrganizationJson(IEnumerable<LabOrganization> selectedOrganizations, bool isShowAllNode = true);

        IList<LabOrganization> LabRoomList { get; }
        string GetLabRoomJson(bool isShowAllNode = true);
        string GetLabRoomJson(IEnumerable<Guid> labRoomIds, bool isShowAllNode = true);

        IList<LabOrganization> OrganizationAndLabRoomList { get; }
        string GetOrganizationAndLabRoomJson(bool isShowAllNode = true);
        string GetOrganizationAndLabRoomJson(IEnumerable<Guid> ids, bool isShowAllNode = true);
        
    }
}
