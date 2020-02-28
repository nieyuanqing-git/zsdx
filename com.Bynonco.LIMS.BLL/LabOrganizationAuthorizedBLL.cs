using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class LabOrganizationAuthorizedBLL : BLLBase<LabOrganizationAuthorized>, ILabOrganizationAuthorizedBLL
    {
        private IUserBLL __userBLL;
        private IUserSystemSettingBLL __userSystemSettingBLL;
        public LabOrganizationAuthorizedBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userSystemSettingBLL = BLLFactory.CreateUserSystemSettingBLL();
        }

        public bool CheckOrganizationControlPower(Guid userId, Guid labOrganizationId, LabOrganizationAuthorizedType labOrganizationAuthorizedType)
        {
            var user = __userBLL.GetEntityById(userId); ;
            if (user == null) return false;
            var labOrganizationIds = GetAuthorizedLabOrganizationIds(userId, labOrganizationAuthorizedType);
            if (labOrganizationIds == null || labOrganizationIds.Count() == 0) return false;
            if (!labOrganizationIds.Contains(labOrganizationId)) return false;
            return true;
        }

        public IEnumerable<Guid> GetAuthorizedLabOrganizationIds(Guid userId, LabOrganizationAuthorizedType labOrganizationAuthorizedType)
        {
            var user = __userBLL.GetEntityById(userId);
            if (user == null) return null;
            IEnumerable<Guid> labOrganizationIds = null;
            IList<LabOrganizationAuthorized> userAuthorizedList = null;
            string queryExpression = string.Format("(BusinessId=\"{0}\"*BusinessType=\"{1}\"*AuthorizedType=\"{2}\")", userId, (int)LabOrganizationAuthorizedBusinessType.User, (int)labOrganizationAuthorizedType);
            var userSystemSettingList = __userSystemSettingBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.ToString()));
            IEnumerable<Guid> workGroupIds = null;
            if (userSystemSettingList != null && userSystemSettingList.Any() && userSystemSettingList.First().WorkGroupId.HasValue)
                workGroupIds = new Guid[] { userSystemSettingList.First().WorkGroupId.Value };
            else if (user.WorkGroups != null && user.WorkGroups.Any())
                workGroupIds = user.WorkGroups.Select(p => p.Id);
            if (workGroupIds != null && workGroupIds.Any())
                queryExpression += string.Format("+({0}*BusinessType=\"{1}\"*AuthorizedType=\"{2}\")", workGroupIds.ToFormatStr("BusinessId"), (int)LabOrganizationAuthorizedBusinessType.WorkGroup, (int)labOrganizationAuthorizedType);
            if (user.TagId.HasValue)
                queryExpression += string.Format("+(BusinessId=\"{0}\"*BusinessType=\"{1}\"*AuthorizedType=\"{2}\")", user.TagId.Value, (int)LabOrganizationAuthorizedBusinessType.Tag, (int)labOrganizationAuthorizedType);
            if (user.OrganizationId.HasValue)
                queryExpression += string.Format("+(BusinessId=\"{0}\"*BusinessType=\"{1}\"*AuthorizedType=\"{2}\")", user.OrganizationId.Value, (int)LabOrganizationAuthorizedBusinessType.Organization, (int)labOrganizationAuthorizedType);
            userAuthorizedList = GetEntitiesByExpression(queryExpression);
            if (userAuthorizedList != null && userAuthorizedList.Any())
                labOrganizationIds = userAuthorizedList.Select(p => p.LabOrganizationId).Distinct();
            return labOrganizationIds;
        }
    }
}