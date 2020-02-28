using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentBlackListBLL : BLLBase<EquipmentBlackList>, IEquipmentBlackListBLL
    {
        private ISubjectBLL __subjectBLL;
        private IUserBLL __userBLL;
        public EquipmentBlackListBLL()
        {
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __userBLL = BLLFactory.CreateUserBLL();
        }

        public bool Validates(Guid equipmentId, Guid? userId,out string errorMessage)
        {
            errorMessage = "";
            if (!userId.HasValue) return true;
           
            var user = __userBLL.GetEntityById(userId.Value);
            var equipmentBlackLists = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*EndTime>\"{1}\"", equipmentId.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), null, "LabelType");
            if (equipmentBlackLists == null || equipmentBlackLists.Count == 0) return true;
            foreach (var equipmentBlackList in equipmentBlackLists)
            {
                switch (equipmentBlackList.LabelTypeEnum)
                {
                    case Model.Enum.LabelType.User:
                        if (equipmentBlackList.LabelId != user.Id) continue;
                        break;
                    case Model.Enum.LabelType.Suject:
                        var subjectList = __subjectBLL.GetUserRelevantSubjects(userId.Value);
                        if (subjectList == null || subjectList.Count == 0) return true;
                        if (!subjectList.Any(p => p.Id == equipmentBlackList.LabelId)) continue;
                        break;
                    case Model.Enum.LabelType.Organization:
                        if (!user.OrganizationId.HasValue || user.OrganizationId.Value != equipmentBlackList.LabelId) continue;
                        break;
                    case Model.Enum.LabelType.Tag:
                        if (!user.TagId.HasValue || user.TagId.Value != equipmentBlackList.LabelId) continue;
                        break;
                }
                errorMessage = string.Format("您已经被列入该设备的黑名单,原因:{0}", equipmentBlackList.Reason);
                return false;
            }
            return true;
        }
    }
}
