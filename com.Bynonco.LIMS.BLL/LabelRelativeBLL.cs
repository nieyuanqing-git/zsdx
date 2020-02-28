using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public delegate IList<ILabel> FunGetLabelListByBusinessId(Guid businessId);
    public delegate IList<ILabel> FunGetLabelListByBusissIdAndLabelName(Guid businessId, string name);
    public delegate int FunGetLabelChargeStandardCount(Guid businessId, Guid labelId);
    public delegate ILabel FunGetLabel(IList<ILabel> lstLabel, Guid businessId);
    public class LabelRelativeBLL
    {
        private IUserBLL __userBLL;
        private ISubjectBLL __subjectBLL;
        private IExperimenterSubjectBLL __experimenterSubjectBLL;
        private FunGetLabel __funGetLabel;
        private FunGetLabelChargeStandardCount __funGetLabelChargeStandardCount;
        private FunGetLabelListByBusinessId __funGetLabelListByBusinessId;
        private FunGetLabelListByBusissIdAndLabelName __funGetLabelListByBusissIdAndLabelName;//Dictionary
       
        public static Dictionary<string,IList<User>> _userListMap = new Dictionary<string, IList<User>>();
        public LabelRelativeBLL(FunGetLabelListByBusinessId funGetLabelListByBusinessId,
            FunGetLabelListByBusissIdAndLabelName funGetLabelListByBusissIdAndLabelName,
            FunGetLabel funGetLabel,
            FunGetLabelChargeStandardCount funGetLabelChargeStandardCount)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            __funGetLabel = funGetLabel;
            __funGetLabelChargeStandardCount = funGetLabelChargeStandardCount;
            __funGetLabelListByBusinessId = funGetLabelListByBusinessId;
            __funGetLabelListByBusissIdAndLabelName = funGetLabelListByBusissIdAndLabelName;
        }
        public bool JudgeIsExistLabelName(Guid businessId, Guid? userId, string labelName)
        {
            string key = businessId.ToString() + labelName;
            if (!userId.HasValue) return false;
            IList<User> userList = null;
            if (!_userListMap.ContainsKey(key))
            {
                var labelList = __funGetLabelListByBusissIdAndLabelName(businessId, labelName);
                userList = GetLabelUserList(labelList);

                _userListMap.Add(key, userList);
            }
            else
            {
                _userListMap.TryGetValue(key, out userList);
            }
            if (userList == null || userList.Count == 0) return false;
            return userList.Any(p => p.Id == userId.Value);
        }
        public IList<User> GetLabelUserList(Guid businessId)
        {
            var labelList = __funGetLabelListByBusinessId(businessId);
            return GetLabelUserList(labelList);
        }
        public ILabel GetLabelByUserId(Guid businessId, Guid? userId, Guid? subjectId, bool isNeedExistLaeblChargeStandard = false)
        {
            if (!userId.HasValue) return null;
            var user = __userBLL.GetEntityById(userId.Value);
            var labelList = __funGetLabelListByBusinessId(businessId);
            if (labelList == null || labelList.Count == 0) return null;
            ILabel fistFindLabel = null;
            List<ILabel> lstLabel = new List<ILabel>();
            List<ILabelChargeStandard> lstLabelChargeStandard = new List<ILabelChargeStandard>();
            PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
            UserAccount userAccount = null;
            User payer = null;
            foreach (var label in labelList)
            {
                var count = __funGetLabelChargeStandardCount(businessId, label.Id);
                if (isNeedExistLaeblChargeStandard && count == 0) continue;
                if (label.LabelItemList == null || label.LabelItemList.Count() == 0) continue;
                switch (label.LabelTypeEnum)
                {
                    case Model.Enum.LabelType.User:
                        if (label.LabelItemList.Any(p => p.LabelItemId == userId))
                        {
                            fistFindLabel = label;
                            lstLabel.Add(label);
                        }
                        break;
                    case Model.Enum.LabelType.Suject:
                        var subjects = __subjectBLL.GetUserRelevantSubjects(userId.Value);
                        if (subjects != null && subjects.Count > 0)
                        {
                            if (label.LabelItemList.Any(p => subjects.Select(s => s.Id).Contains(p.LabelItemId)))
                            {
                                fistFindLabel = label;
                                lstLabel.Add(label);
                            }
                        }
                        break;
                    case Model.Enum.LabelType.Organization:

                        if (subjectId.HasValue && payer == null)
                        {
                            payer = BLLFactory.CreateUserBLL().GetPayer(userId.Value, subjectId.Value, out paymentMethod, out userAccount);
                        }
                        //组织机构按付费人的机构来
                        Guid? orgID = payer == null ? user.OrganizationId : payer.OrganizationId;
                        if (orgID.HasValue && label.LabelItemList.Any(p => p.LabelItemId == orgID.Value))
                        {
                            fistFindLabel = label;
                            lstLabel.Add(label);
                        }
                        break;
                    case Model.Enum.LabelType.Tag:
                        if (subjectId.HasValue && payer == null)
                        {
                            payer = BLLFactory.CreateUserBLL().GetPayer(userId.Value, subjectId.Value, out paymentMethod, out userAccount);
                        }
                        //用户标签按付费人的标签来
                        Guid? tagId = payer == null ? user.TagId : payer.TagId;
                        if (tagId.HasValue && label.LabelItemList.Any(p => p.LabelItemId == tagId.Value))
                        {
                            fistFindLabel = label;
                            lstLabel.Add(label);
                        }
                        break;
                }
            }
            if (lstLabel.Count > 0)
            {
                return __funGetLabel(lstLabel, businessId);
                ;
            }
            return fistFindLabel;
        }
        public IList<User> GetLabelUserList(IEnumerable<ILabel> labelList)
        {
            if (labelList == null || labelList.Count() == 0) return null;
            List<User> lstUsers = new List<User>();
            foreach (var label in labelList)
            {
                if (label.LabelItemList == null || label.LabelItemList.Count() == 0) continue;
                IList<User> userList = null;
                switch (label.LabelTypeEnum)
                {
                    case Model.Enum.LabelType.Organization:
                        userList = __userBLL.GetEntitiesByExpression(label.LabelItemList.Select(p => p.LabelItemId).ToFormatStr("OrganizationId") + "*IsDel=false");
                        break;
                    case Model.Enum.LabelType.Suject:
                        List<Guid> ids = new List<Guid>();
                        var subjectList = __subjectBLL.GetEntitiesByExpression(label.LabelItemList.Select(p => p.LabelItemId).ToFormatStr());
                        ids.AddRange(subjectList.Select(p => p.SubjectDirectorId.Value));
                        var experimenterSubjectList = __experimenterSubjectBLL.GetEntitiesByExpression(label.LabelItemList.Select(p => p.LabelItemId).ToFormatStr("SubjectId") + "*IsDelete=false");
                        if (experimenterSubjectList != null && experimenterSubjectList.Count > 0)
                            ids.AddRange(experimenterSubjectList.Select(p => p.ExperimenterId.Value));
                        userList = __userBLL.GetEntitiesByExpression(ids.ToFormatStr() + "*IsDel=false");
                        break;
                    case Model.Enum.LabelType.Tag:
                        userList = __userBLL.GetEntitiesByExpression(label.LabelItemList.Select(p => p.LabelItemId).ToFormatStr("TagId") + "*IsDel=false");
                        break;
                    case Model.Enum.LabelType.User:
                        userList = __userBLL.GetEntitiesByExpression(label.LabelItemList.Select(p => p.LabelItemId).ToFormatStr() + "*IsDel=false");
                        break;
                }
                if (userList != null && userList.Count > 0) lstUsers.AddRange(userList);
            }
            return lstUsers;
        }
    }
}
