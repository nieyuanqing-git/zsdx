using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Utility;
namespace com.Bynonco.LIMS.BLL
{
    public class TrainningTypeBLL : BLLBase<TrainningType>, ITrainningTypeBLL
    {
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private ILabOrganizationBLL __labOrganizationBLL;
        public TrainningTypeBLL()
        {
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
        }
        public IList<TrainningType> GetUserAutherizeTrainningTypeList(Guid? userId, string orgId)
        {
            if (userId == null) return null;
            List<Guid> orgIds = new List<Guid>();
            var hasPowerOrgIds = __labOrganizationAuthorizedBLL.GetAuthorizedLabOrganizationIds(userId.Value, Model.Enum.LabOrganizationAuthorizedType.User);
            if (hasPowerOrgIds == null || !hasPowerOrgIds.Any()) return null;
            if (!string.IsNullOrWhiteSpace(orgId) && !hasPowerOrgIds.Any(p => p.ToString().ToLower() == orgId.Trim().ToLower())) return null;
            var trainningTypes = GetEntitiesByExpression(string.Format("IsDelete=false*" + hasPowerOrgIds.ToFormatStr("OrgId")), null, "XPath", true);
            if (trainningTypes == null || trainningTypes.Count == 0) return null;
            if (!string.IsNullOrWhiteSpace(orgId))
            {
                if (trainningTypes.All(p => p.OrgId != new Guid(orgId))) return null;
                return trainningTypes.Where(p => p.OrgId == new Guid(orgId)).ToList();
            }
            return trainningTypes;
        }
        public IList<TrainningType> GetUserAutherizeOrgTrainningTypeList(Guid? userId)
        {
           if (userId == null) return null;
            var hasPowerOrgIds = __labOrganizationAuthorizedBLL.GetAuthorizedLabOrganizationIds(userId.Value, Model.Enum.LabOrganizationAuthorizedType.User);
            if (hasPowerOrgIds == null || hasPowerOrgIds.Count() == 0) return null;
            var orgList = __labOrganizationBLL.GetEntitiesByExpression("IsDelete=false*" + hasPowerOrgIds.ToFormatStr(), null, "XPath");
            if (orgList == null || orgList.Count == 0) return null;
            var trainningTypeList = GetEntitiesByExpression( "IsDelete=false*" + hasPowerOrgIds.ToFormatStr("OrgId"));
            IList<Guid> lstHasHandleTrainningTypeIds = new List<Guid>();
            IList<TrainningType> lstTrainningType =  new List<TrainningType>();
            foreach (var org in orgList) GenerateOrgTrainningType(lstTrainningType, trainningTypeList, org, orgList, lstHasHandleTrainningTypeIds);
            return lstTrainningType;
        }
        private void GenerateOrgTrainningType(IList<TrainningType> lstTrainningType, IList<TrainningType> allTrainningTypeList, LabOrganization org, IList<LabOrganization> orgList, IList<Guid> lstHasHandleTrainningTypeIds)
        {
            if (org == null) return ;
            TrainningType parentTrainningType = new TrainningType()
            {
                Id = org.Id,
                Name = org.Name,
                ParentId = org.ParentId.HasValue && orgList.Any(p => p.Id == org.ParentId.Value) ? org.ParentId : null,
                IsEnableSelect = false,
                XPath = "NULL"
            };
            var trainningTypeListFind = allTrainningTypeList != null ? allTrainningTypeList.Where(p => p.OrgId == org.Id) : null;
            if (trainningTypeListFind == null || trainningTypeListFind.Count()==0) return;
            lstTrainningType.Add(parentTrainningType);
            foreach (var trainningTypeFind in trainningTypeListFind)
            {
                GenerateTrainningType(lstTrainningType, trainningTypeFind, lstHasHandleTrainningTypeIds);
                if (!trainningTypeFind.ParentId.HasValue) trainningTypeFind.ParentId = org.Id;
            }
            if (org.children != null && org.children.Count>0)
            {
                foreach (var child in org.children) GenerateOrgTrainningType(lstTrainningType, allTrainningTypeList, child, orgList, lstHasHandleTrainningTypeIds);
            }
        }
        private void GenerateTrainningType(IList<TrainningType> lstTrainningType, TrainningType trainningType,IList<Guid> lstHasHandleTrainningTypeIds)
        {
            if (lstHasHandleTrainningTypeIds.Contains(trainningType.Id)) return;
            lstHasHandleTrainningTypeIds.Add(trainningType.Id);
            lstTrainningType.Add(trainningType);
            if (trainningType.children != null && trainningType.children.Count > 0)
            {
                foreach (var child in trainningType.children)
                {
                    if (child.IsDelete) continue;
                    GenerateTrainningType(lstTrainningType, child, lstHasHandleTrainningTypeIds);
                }
            }
            
        }
    }
}