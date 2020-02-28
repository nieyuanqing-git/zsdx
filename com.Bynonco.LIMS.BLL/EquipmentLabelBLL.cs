using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentLabelBLL : BLLBase<EquipmentLabel>, IEquipmentLabelBLL
    {
        private IEquipmentLabelChargeStandardBLL __equipmentLabelChargeStandardBLL;
        private LabelRelativeBLL __labelRelativeBLL;
        public EquipmentLabelBLL()
        {
            __equipmentLabelChargeStandardBLL = BLLFactory.CreateEquipmentLabelChargeStandardBLL();
            __labelRelativeBLL = new LabelRelativeBLL(GetEquipmentLabelList, GetEquipmentLabelListByEquipmentIdAndLabelName, GetEquipmentLabel, __equipmentLabelChargeStandardBLL.GetEquipmentLabelChargeStandardCount);
        }
        private IList<ILabel> GetEquipmentLabelList(Guid equipmentId)
        {
            IList<ILabel> lstLabel = new List<ILabel>();
            var equipmentLabelList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId), null, "LabelType");
            if (equipmentLabelList == null || equipmentLabelList.Count == 0) return null;
            foreach (var equipmentLabel in equipmentLabelList) lstLabel.Add(equipmentLabel);
            return lstLabel;
        }
        private IList<ILabel> GetEquipmentLabelListByEquipmentIdAndLabelName(Guid equipmentId, string labelName)
        {
            IList<ILabel> lstLabel = new List<ILabel>();
            var equipmentLabelList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*Name=\"{1}\"", equipmentId.ToString(), labelName));
            if (equipmentLabelList == null || equipmentLabelList.Count == 0) return null;
            foreach (var equipmentLabel in equipmentLabelList) lstLabel.Add(equipmentLabel);
            return lstLabel;
        }
        private ILabel GetEquipmentLabel(IList<ILabel> lstEquipmentLabel, Guid equipmentId)
        {
            if (lstEquipmentLabel.Count > 0)
            {
                var queryExpression = string.Format("EquipmentId=\"{0}\"", equipmentId);
                queryExpression += "*" + lstEquipmentLabel.Select(p => p.Id.ToString()).ToFormatStr("EquipmentLabelId");
                var equipmentLabelChargeStandards = __equipmentLabelChargeStandardBLL.GetEntitiesByExpression(queryExpression);
                if (equipmentLabelChargeStandards != null && equipmentLabelChargeStandards.Count > 0)
                {
                    return equipmentLabelChargeStandards.OrderBy(p => p.OrderNo).ThenBy(p => p.EquipmentLabel.LabelType).First().EquipmentLabel;
                }
            }
            return null;
        }

        public bool JudgeIsExistEquipmentLabelName(Guid equipmentId, Guid? userId, string labelName)
        {
            return __labelRelativeBLL.JudgeIsExistLabelName(equipmentId, userId, labelName);

            #region original
            //if (!userId.HasValue) return false;
            //var equipmentLabelList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*Name=\"{1}\"", equipmentId.ToString(), labelName));
            //var userList = GetEquipmentLabelUserList(equipmentLabelList);
            //if (userList == null || userList.Count == 0) return false;
            //return userList.Any(p => p.Id == userId.Value);
            #endregion
        }
        public IList<User> GetEquipmentLabelUserList(Guid equipmentId)
        {
            return __labelRelativeBLL.GetLabelUserList(equipmentId);

            #region original
            //var equipmentLabelList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"",equipmentId.ToString()));
            //return GetEquipmentLabelUserList(equipmentLabelList);
            #endregion
        }
        public EquipmentLabel GetEquipmentLabelByUserId(Guid equipmentId, Guid? userId, Guid? subjectId, bool isNeedExistEquipmentLaeblChargeStandard=false)
        {
            return (EquipmentLabel)__labelRelativeBLL.GetLabelByUserId(equipmentId, userId, subjectId, isNeedExistEquipmentLaeblChargeStandard);

            #region original
            //IEquipmentLabelChargeStandardBLL equipmentLabelChargeStandardBLL = BLLFactory.CreateEquipmentLabelChargeStandardBLL();   
            //if (!userId.HasValue) return null;
            //var user = __userBLL.GetEntityById(userId.Value);
            //var equipmentLabelList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId.ToString()),null,"LabelType");
            //if (equipmentLabelList == null || equipmentLabelList.Count == 0) return null;
            //EquipmentLabel fistFindEquipmentLabel = null;
            //List<EquipmentLabel> lstEquipmentLabel = new List<EquipmentLabel>();
            //List<EquipmentLabelChargeStandard> lstEquipmentLabelChargeStandard = new List<EquipmentLabelChargeStandard>();
            //foreach (var equipmentLabel in equipmentLabelList)
            //{
            //    var count = equipmentLabelChargeStandardBLL.CountModelListByExpression(string.Format("EquipmentId=\"{0}\"*EquipmentLabelId=\"{1}\"", equipmentId.ToString(), equipmentLabel.Id));
            //    if (isNeedExistEquipmentLaeblChargeStandard && count == 0) continue;
            //    if (equipmentLabel.LabelItems == null || equipmentLabel.LabelItems.Count == 0) continue;
            //    switch (equipmentLabel.LabelTypeEnum)
            //    {
            //        case Model.Enum.LabelType.User:
            //            if (equipmentLabel.LabelItems.Any(p => p.LabelItemId == userId))
            //            {
            //                fistFindEquipmentLabel = equipmentLabel;
            //                lstEquipmentLabel.Add(equipmentLabel);
            //            }
            //            break;
            //        case Model.Enum.LabelType.Suject:
            //            var subjects = __subjectBLL.GetUserRelevantSubjects(userId.Value);
            //            if (subjects != null && subjects.Count > 0)
            //            {
            //                if (equipmentLabel.LabelItems.Any(p => subjects.Select(s => s.Id).Contains(p.LabelItemId)))
            //                {
            //                    fistFindEquipmentLabel = equipmentLabel;
            //                    lstEquipmentLabel.Add(equipmentLabel);
            //                }
            //            }
            //            break;
            //        case Model.Enum.LabelType.Organization:
            //            if (user.OrganizationId.HasValue && equipmentLabel.LabelItems.Any(p => p.LabelItemId == user.OrganizationId.Value))
            //            {
            //                fistFindEquipmentLabel = equipmentLabel;
            //                lstEquipmentLabel.Add(equipmentLabel);
            //            }
            //            break;
            //        case Model.Enum.LabelType.Tag:
            //            if (user.TagId.HasValue && equipmentLabel.LabelItems.Any(p => p.LabelItemId == user.TagId.Value))
            //            {
            //                fistFindEquipmentLabel = equipmentLabel;
            //                lstEquipmentLabel.Add(equipmentLabel);
            //            }
            //            break;
            //    }
            //}
            //if (lstEquipmentLabel.Count > 0)
            //{
            //    var queryExpression = string.Format("EquipmentId=\"{0}\"", equipmentId);
            //    queryExpression += "*" + lstEquipmentLabel.Select(p => p.Id.ToString()).ToFormatStr("EquipmentLabelId");
            //    var equipmentLabelChargeStandards = equipmentLabelChargeStandardBLL.GetEntitiesByExpression(queryExpression);
            //    if (equipmentLabelChargeStandards != null && equipmentLabelChargeStandards.Count > 0)
            //        return equipmentLabelChargeStandards.OrderBy(p => p.OrderNo).ThenBy(p => p.EquipmentLabel.LabelType).First().EquipmentLabel;
            //}
            //return fistFindEquipmentLabel;
            #endregion
        }
        private IList<User> GetEquipmentLabelUserList(IEnumerable<EquipmentLabel> equipmentLabelList)
        {
            return __labelRelativeBLL.GetLabelUserList(equipmentLabelList);

            #region original
            //if (equipmentLabelList == null || equipmentLabelList.Count() == 0) return null;
            //List<User> lstUsers = new List<User>();
            //foreach (var equipmentLabel in equipmentLabelList)
            //{
            //    if (equipmentLabel.LabelItems == null || equipmentLabel.LabelItems.Count == 0) continue;
            //    IList<User> userList = null;
            //    switch (equipmentLabel.LabelTypeEnum)
            //    {
            //        case Model.Enum.LabelType.Organization:
            //            userList = __userBLL.GetEntitiesByExpression(equipmentLabel.LabelItems.Select(p => p.LabelItemId).ToFormatStr("OrganizationId") + "*IsDel=false");
            //            break;
            //        case Model.Enum.LabelType.Suject:
            //            List<Guid> ids = new List<Guid>();
            //            var subjectList = __subjectBLL.GetEntitiesByExpression(equipmentLabel.LabelItems.Select(p => p.LabelItemId).ToFormatStr());
            //            ids.AddRange(subjectList.Select(p => p.SubjectDirectorId.Value));
            //            var experimenterSubjectList = __experimenterSubjectBLL.GetEntitiesByExpression(equipmentLabel.LabelItems.Select(p => p.LabelItemId).ToFormatStr("SubjectId") + "*IsDelete=false");
            //            if (experimenterSubjectList != null && experimenterSubjectList.Count > 0)
            //                ids.AddRange(experimenterSubjectList.Select(p => p.ExperimenterId.Value));
            //            userList = __userBLL.GetEntitiesByExpression(ids.ToFormatStr() + "*IsDel=false");
            //            break;
            //        case Model.Enum.LabelType.Tag:
            //            userList = __userBLL.GetEntitiesByExpression(equipmentLabel.LabelItems.Select(p => p.LabelItemId).ToFormatStr("TagId") + "*IsDel=false");
            //            break;
            //        case Model.Enum.LabelType.User:
            //            userList = __userBLL.GetEntitiesByExpression(equipmentLabel.LabelItems.Select(p => p.LabelItemId).ToFormatStr() + "*IsDel=false");
            //            break;
            //    }
            //    if (userList != null && userList.Count > 0) lstUsers.AddRange(userList);
            //}
            //return lstUsers;
            #endregion
        }
    }
}