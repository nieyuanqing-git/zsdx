using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;
namespace com.Bynonco.LIMS.BLL
{
    public class UserExaminationBLL : BLLBase<UserExamination>, IUserExaminationBLL
    {
        private IEquipmentBLL __equipmentBLL;
        private ITrainningTypeBLL __trainningTypeBLL;
        private ITrainningExaminationBLL __trainningExaminationBLL;
        private ITrainningExaminationMaterialBLL __trainningExaminationMaterialBLL;
        private ITrainningMaterialBLL __trainningMaterialBLL;
        private IViewTrainningExaminationBLL __viewTrainningExaminationBLL;
        private IViewUserMaterialReadingTimeBLL __viewUserMaterialReadingTimeBLL;
        private IUserExaminationQuestionBLL __userExaminationQuestionBLL;
        private ILabOrganizationBLL __labOrganizationBLL;
        private ITrainningQuestionBLL __trainningQuestionBLL;
        public UserExaminationBLL()
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __trainningTypeBLL = BLLFactory.CreateTrainningTypeBLL();
            __trainningExaminationBLL = BLLFactory.CreateTrainningExaminationBLL();
            __trainningExaminationMaterialBLL = BLLFactory.CreateTrainningExaminationMaterialBLL();
            __trainningMaterialBLL = BLLFactory.CreateTrainningMaterialBLL();
            __viewTrainningExaminationBLL = BLLFactory.CreateViewTrainningExaminationBLL();
            __viewUserMaterialReadingTimeBLL = BLLFactory.CreateViewUserMaterialReadingTimeBLL();
            __userExaminationQuestionBLL = BLLFactory.CreateUserExaminationQuestionBLL();
            __labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            __trainningQuestionBLL = BLLFactory.CreateTrainningQuestionBLL();
        }
        public object GetUserEquipmentExaminationPriviliges(Guid? userId, IList<ViewUserEquipmentExamination> viewUserEquipmentExaminationList)
        {
            IList<UserEquipmentExaminationPrivilige> lstUserEquipmentExaminationPriviliges = new List<UserEquipmentExaminationPrivilige>();
            if (userId.HasValue && viewUserEquipmentExaminationList != null && viewUserEquipmentExaminationList.Count > 0)
            {
                foreach (ViewUserEquipmentExamination viewUserEquipmentExamination in viewUserEquipmentExaminationList)
                {
                    UserEquipmentExaminationPrivilige userEquipmentExaminationPrivilige = lstUserEquipmentExaminationPriviliges.FirstOrDefault(p => p.UserExaminationId.HasValue && p.UserExaminationId.Value == viewUserEquipmentExamination.Id);
                    if (userEquipmentExaminationPrivilige == null)
                    {
                        userEquipmentExaminationPrivilige = PriviligeFactory.GetUserEquipmentExaminationPrivilige(userId.Value, viewUserEquipmentExamination.Id);
                        lstUserEquipmentExaminationPriviliges.Add(userEquipmentExaminationPrivilige);
                    }
                }
            }
            return lstUserEquipmentExaminationPriviliges;
        }
        public object GetUserLabOrganizationExaminationPriviliges(Guid? userId, IList<ViewUserLabOrganizationExamination> viewUserLabOrganizationExaminationList)
        {
            IList<UserLabOrganizationExaminationPrivilige> lstUserLabOrganizationExaminationPriviliges = new List<UserLabOrganizationExaminationPrivilige>();
            if (userId.HasValue && viewUserLabOrganizationExaminationList != null && viewUserLabOrganizationExaminationList.Count > 0)
            {
                foreach (ViewUserLabOrganizationExamination viewUserLabOrganizationExamination in viewUserLabOrganizationExaminationList)
                {
                    UserLabOrganizationExaminationPrivilige userLabOrganizationExaminationPrivilige = lstUserLabOrganizationExaminationPriviliges.FirstOrDefault(p => p.UserExaminationId.HasValue && p.UserExaminationId.Value == viewUserLabOrganizationExamination.Id);
                    if (userLabOrganizationExaminationPrivilige == null)
                    {
                        userLabOrganizationExaminationPrivilige = PriviligeFactory.GetUserLabOrganizationExaminationPrivilige(userId.Value, viewUserLabOrganizationExamination.Id);
                        lstUserLabOrganizationExaminationPriviliges.Add(userLabOrganizationExaminationPrivilige);
                    }
                }
            }
            return lstUserLabOrganizationExaminationPriviliges;
        }
        public bool Validates(Guid equipmentId, Guid? userId, TrainningExaminationBusinessType trainningExaminationBusinessType, out string errorMessage)
        {
            errorMessage = "";
            if (!userId.HasValue) return true;
            var equipment = __equipmentBLL.GetEntityById(equipmentId);
            if (equipment == null) return false;
            Guid businessId = Guid.Empty;
            if (trainningExaminationBusinessType == TrainningExaminationBusinessType.Equipment)
            {
                businessId = equipment.Id;
            }
            else
            {
                if (!equipment.OrgId.HasValue) { errorMessage = "设备机构编码为空"; return false; }
                var labOrganization = __labOrganizationBLL.GetEntityById(equipment.OrgId.Value);
                if (labOrganization == null) { errorMessage = "找不到设备所属机构"; return false; };
                businessId = labOrganization.Id;
            }
            var originalItem = GetExaminationBusiness(userId, (int)trainningExaminationBusinessType, businessId, out errorMessage);
            if (originalItem == null) { errorMessage = "考试设置为空"; return false; }
            if(!originalItem.IsNeedExaminationBeforeAppointment.HasValue || !originalItem.IsNeedExaminationBeforeAppointment.Value) return true;
            var paperList = GetExaminationBusinessPaper(userId.Value, originalItem, out errorMessage);
            if (paperList != null && paperList.Count() > 0)
            {
                var str = "";
                int i = 0;
                foreach (var item in paperList)
                {
                    if (item.IsPass.HasValue && item.IsPass.Value) continue;
                    i++;
                    str += (string.IsNullOrWhiteSpace(str) ? "" : "<br />") + string.Format("{0}. [<span style='font-weight:bold;margin:0 5px;'>{1}</span>]  {2}", i, item.TrainningTypeName, string.IsNullOrWhiteSpace(item.TrainningExaminationName) ? "(自动生成试卷)" : item.TrainningExaminationName);
                }
                if (!string.IsNullOrWhiteSpace(str))
                {
                    errorMessage = str;
                    return false;
                }
            }
            return true;
        }
        public bool CheckCreateUserExamination(Guid userId, int businessType, Guid businessId, Guid? trainningExaminationId, out string errorMessage)
        {
            errorMessage = "";
            var userExaminationList = GetEntitiesByExpression(string.Format("BusinessId=\"{0}\"*UserId=\"{1}\"*IsStop=false*IsDelete=false", businessId, userId));
            if (userExaminationList != null && userExaminationList.Count > 0)
            {
                var userExaminationStartedList = userExaminationList.Where(p => p.Status == (int)UserExaminationStatus.Stadted && p.EndTime.HasValue && p.EndTime.Value >= DateTime.Now);
                if (userExaminationStartedList != null && userExaminationStartedList.Count() > 0)
                {
                    errorMessage = string.Format("出错,用户有进行中的在线考试,无法再次进行在线考试");
                    return false;
                }
            }
            var examinationBusiness = GetExaminationBusiness(userId, businessType, businessId, trainningExaminationId, out errorMessage);
            if (examinationBusiness == null)
            {
                errorMessage = "出错,找不到考试信息";
                return false;
            }
            if (examinationBusiness.MaxExaminationTime.HasValue && examinationBusiness.TestedCount.HasValue && examinationBusiness.TestedCount.Value >= examinationBusiness.MaxExaminationTime.Value)
            {
                errorMessage = string.Format("出错, 您的考试次数已经超出最大考试次数【{0}】", examinationBusiness.TestedCount.Value);
                return false;
            }
            if (examinationBusiness.IsNeedReadMaterialBeforeExamination.HasValue && examinationBusiness.IsNeedReadMaterialBeforeExamination.Value)
            {
                if (!examinationBusiness.MinReadMaterialDateTime.HasValue)
                {
                    errorMessage = string.Format("出错, 设置出错,最少阅读时间为空");
                    return false;
                }
                else if (!examinationBusiness.ReadedHours.HasValue || (examinationBusiness.ReadedHours.HasValue && examinationBusiness.ReadedHours.Value < examinationBusiness.MinReadMaterialDateTime.Value))
                {
                    errorMessage = string.Format("出错,在线学时不足,您当前的学时为[{0}], 该考试要求学时[{1}]", examinationBusiness.ReadedHours.HasValue ? Math.Round(examinationBusiness.ReadedHours.Value,2).ToString() : "0", Math.Round(examinationBusiness.MinReadMaterialDateTime.Value,2).ToString());
                    return false;
                }
            }
            if (!examinationBusiness.IsNeedExaminationBeforeAppointment.HasValue || !examinationBusiness.IsNeedExaminationBeforeAppointment.Value)
            {
                errorMessage = string.Format("出错,【{0】不需要进行在线考试", examinationBusiness.BusinessName);
                return false;
            }
            if(examinationBusiness.IsPass.HasValue && examinationBusiness.IsPass.Value)
            {
                errorMessage = string.Format("出错,用户已经通过考试,不需要再次进行在线考试");
                return false;
            }
            return true;
        }
        public bool CreateUserExamination(Guid userId, int businessType, Guid businessId, Guid? trainningExaminationId, out string errorMessage)
        {
            errorMessage = "";
            if(!CheckCreateUserExamination(userId,businessType, businessId,trainningExaminationId, out errorMessage)) return false;
            var userExamination = new UserExamination();
            userExamination.Id = Guid.NewGuid();
            userExamination.UserId = userId;
            userExamination.StartTime = DateTime.Now;
            userExamination.IsStop = false;
            userExamination.IsDelete = false;
            userExamination.BusinessType = businessType;
            userExamination.BusinessId = businessId;
            userExamination.Status = (int)UserExaminationStatus.Stadted;
            Guid? trainningTypeId = null;
            int? passType = null;
            double? passStandard = null;
            int? randomCount = null;
            IList<TrainningQuestion> trainningQuestionList = null;
            if (businessType == (int)TrainningExaminationBusinessType.LabOrganization)
            {
                var labOrganization = __labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*IsDelete=false", businessId), null, "", true);
                passType = labOrganization.ExaminationPassType;
                passStandard = labOrganization.PassStandard;
                randomCount = labOrganization.TrainningQuestionCount;
                trainningTypeId = labOrganization.TrainningTypeId;
            }
            else
            {
                var equipment = __equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*IsDelete=false", businessId), null, "", true);
                passType = equipment.ExaminationPassType;
                passStandard = equipment.PassStandard;
                randomCount = equipment.TrainningQuestionCount;
                trainningTypeId = equipment.TrainningTypeId;
            }
            if (!trainningExaminationId.HasValue)
            {
                userExamination.Name = "随机生成试卷";
                userExamination.PassType = passType;
                userExamination.PassStandard = passStandard;
            }
            else
            {
                var trainningExamination = __trainningExaminationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*IsStop=false*IsDelete=false", trainningExaminationId.Value));
                var item = trainningExamination;
                userExamination.TrainningExaminationId = item.Id;
                userExamination.Name = item.Name;
                userExamination.PassType = item.PassType;
                userExamination.PassStandard = item.PassStandard;
                trainningTypeId = item.TrainningTypeId;

                if (item.TrainningQuestionList != null && item.TrainningQuestionList.Count() > 0)
                {
                    trainningQuestionList = item.TrainningQuestionList;
                    userExamination.QuestionCount = item.QuestionCount;
                    userExamination.TotalTime = item.TotalTimeSpend;
                    userExamination.TotalScore = item.TotalScore;
                    userExamination.EndTime = DateTime.Now.AddMinutes(item.TotalTimeSpend);
                }
            }
            userExamination.TrainningTypeId = trainningTypeId;
            userExamination.RandomQuestionCount = randomCount;
            if (trainningQuestionList == null || trainningQuestionList.Count() == 0)
            {
                if (randomCount.HasValue && randomCount.Value > 0 && trainningTypeId.HasValue)
                {
                    var viewTrainningQuestionRandomList = BLLFactory.CreateViewTrainningQuestionBLL().GetRandomViewTrainningQuestionList(trainningTypeId.Value, randomCount.Value);
                    if (viewTrainningQuestionRandomList != null && viewTrainningQuestionRandomList.Count() > 0)
                        trainningQuestionList = __trainningQuestionBLL.GetEntitiesByExpression(viewTrainningQuestionRandomList.Select(p => p.Id).ToFormatStr());
                }
                userExamination.IsRandom = true;
                userExamination.QuestionCount = trainningQuestionList == null ? 0 : trainningQuestionList.Count();
                userExamination.TotalTime = trainningQuestionList == null ? 0 : trainningQuestionList.Sum(p=>p.TimeSpend);
                userExamination.TotalScore = trainningQuestionList == null ? 0 : trainningQuestionList.Sum(p => p.Score);
                userExamination.EndTime = DateTime.Now.AddMinutes(userExamination.TotalTime.Value);
            }
            if (trainningQuestionList == null || trainningQuestionList.Count() == 0)
            {
                errorMessage = "出错,无法从题库找到相关试题生成试卷,请确保题库中有题目数据.";
                return false;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                Add(new UserExamination[] { userExamination }, ref tran,true);
                if(trainningQuestionList != null && trainningQuestionList.Count() > 0)
                {
                    IList<UserExaminationQuestion> userExaminationQuestionList = new List<UserExaminationQuestion>();
                    for(int i= 0 ; i< trainningQuestionList.Count(); i++ )
                    {
                        var item = trainningQuestionList[i];
                        UserExaminationQuestion addItem = new UserExaminationQuestion();
                        addItem.Id = Guid.NewGuid();
                        addItem.UserExaminationId = userExamination.Id;
                        addItem.TrainningQuestionId = item.Id;
                        addItem.QuestionName = item.Name;
                        addItem.QuestionContent = GetQuestionContent(i+1,item);
                        addItem.QuestionXPath = i.ToString().PadLeft(9,'0');
                        addItem.Answer = item.Answer;
                        addItem.Score = item.Score;
                        addItem.TimeSpend = item.TimeSpend;
                        addItem.UserAnswer = "";
                        addItem.IsCorrect = false;
                        userExaminationQuestionList.Add(addItem);
                    }
                    __userExaminationQuestionBLL.Add(userExaminationQuestionList, ref tran,true);
                }
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (tran != null) tran.RollbackTransaction();
                errorMessage = "出错," + ex.Message;
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
            return true;
        }
        private string GetQuestionContent(int? questionNo, TrainningQuestion trainningQuestion)
        {
            string str = "";
            if (trainningQuestion != null)
            {
                var name = (trainningQuestion.Name == "" ? ((QuestionType)trainningQuestion.QuestionType).ToCnName() : trainningQuestion.Name) + "   [" + trainningQuestion.Score + "分, 答题时间:" + trainningQuestion.TimeSpend + "分钟, 答案:" + trainningQuestion.Answer + "]";
                str += "<p>" + (questionNo == null ? "" : questionNo.Value.ToString() + ".") + name + "</p>";
                str += trainningQuestion.ContentHTML;
                if (trainningQuestion.QuestionType != (int)QuestionType.YesNo)
                {
                    var trainningQuestionItem = trainningQuestion.TrainningQuestionItemList;
                    if (trainningQuestionItem != null && trainningQuestionItem.Count() > 0)
                    {
                        str += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"  style=\"margin-left:20px;\">";
                        foreach (var item in trainningQuestionItem)
                        {
                            str += "<tr>";
                            str += "<td>" + item.ItemValue + "</td>";
                            str += "<td>" + item.ItemTextHTML + "</td>";
                            str += "</tr>";
                        }
                        str += "</table>";
                    }
                }
            }
            return str;
        }
        public int GetExaminationBusinessType(Guid businessId, out string errorMessage)
        {
            errorMessage = "";
            var labOrganization = __labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*IsDelete=false", businessId), null, "", true);
            if (labOrganization != null) return (int)TrainningExaminationBusinessType.LabOrganization;
            var equipment = __equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*IsDelete=false", businessId), null, "", true);
            if (equipment != null) return (int)TrainningExaminationBusinessType.Equipment;
            errorMessage = string.Format("出错,找不到编码【\"{0}\"】的业务类型", businessId);
            return -1;
        }
        public ExaminationBusiness GetExaminationBusiness(Guid? userId, int businessType, Guid businessId, out string errorMessage)
        {
            errorMessage = "";
            ExaminationBusiness examinationBusiness = new ExaminationBusiness();
            examinationBusiness.Id = businessId;
            examinationBusiness.BusinessId = businessId;
            examinationBusiness.BusinessType = businessType;
            if (businessType == (int)TrainningExaminationBusinessType.LabOrganization)
            {
                var labOrganization = __labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*IsDelete=false", businessId), null, "", true);
                if (labOrganization == null)
                {
                    errorMessage = string.Format("出错,找不到编码【{0}】的机构信息", businessId);
                    return null;
                }
                examinationBusiness.BusinessName = labOrganization.Name;
                examinationBusiness.IsNeedReadMaterialBeforeExamination = labOrganization.IsNeedReadMaterialBeforeExamination;
                examinationBusiness.MinReadMaterialDateTime = labOrganization.MinReadMaterialDateTime;
                examinationBusiness.IsNeedExaminationBeforeAppointment = labOrganization.IsNeedExaminationBeforeAppointment;
                examinationBusiness.MaxExaminationTime = labOrganization.MaxExaminationTime;
                examinationBusiness.TrainningTypeId = labOrganization.TrainningTypeId;
                examinationBusiness.TrainningTypeName = labOrganization.TrainningType != null ? labOrganization.TrainningType.Name : "";
                examinationBusiness.PassType = labOrganization.ExaminationPassType;
                examinationBusiness.PassStandard = labOrganization.PassStandard;
                examinationBusiness.TrainningQuestionCount = labOrganization.TrainningQuestionCount;
            }
            else
            {
                var equipment = __equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*IsDelete=false", businessId), null, "", true);
                if (equipment == null)
                {
                    errorMessage = string.Format("出错,找不到编码【{0}】的设备信息", businessId);
                    return null;
                }
                examinationBusiness.BusinessName = equipment.Name;
                examinationBusiness.IsNeedReadMaterialBeforeExamination = equipment.IsNeedReadMaterialBeforeExamination;
                examinationBusiness.MinReadMaterialDateTime = equipment.MinReadMaterialDateTime;
                examinationBusiness.IsNeedExaminationBeforeAppointment = equipment.IsNeedExaminationBeforeAppointment;
                examinationBusiness.MaxExaminationTime = equipment.MaxExaminationTime;
                examinationBusiness.TrainningTypeId = equipment.TrainningTypeId;
                examinationBusiness.TrainningTypeName = equipment.TrainningType != null ? equipment.TrainningType.Name : "";
                examinationBusiness.PassType = equipment.ExaminationPassType;
                examinationBusiness.PassStandard = equipment.PassStandard;
                examinationBusiness.TrainningQuestionCount = equipment.TrainningQuestionCount;
            }
            if(examinationBusiness.TrainningTypeId.HasValue)
            {
                var trainningType = __trainningTypeBLL.GetEntityById(examinationBusiness.TrainningTypeId.Value);
                if (trainningType != null)
                {
                    IViewTrainningMaterialBLL viewTrainningMaterialBLL = BLLFactory.CreateViewTrainningMaterialBLL();
                    examinationBusiness.TrainningMaterialCount = trainningType.IsGeneral ?
                        viewTrainningMaterialBLL.CountModelListByExpression("IsDelete=false*IsStop=false")
                        : viewTrainningMaterialBLL.CountModelListByExpression(string.Format("IsDelete=false*IsStop=false*(TrainningTypeIsGeneral=true+TrainningTypeXPath→\"{0}\"", trainningType.XPath));
                }
            }
            if (userId.HasValue)
            {
                var userExaminationList = GetEntitiesByExpression(string.Format("UserId=\"{0}\"*IsStop=false*IsDelete=false", userId.Value));
                var totalReadedHours = examinationBusiness.TrainningTypeId.HasValue ? __viewUserMaterialReadingTimeBLL.GetUserTotalReadingHours(userId.Value, examinationBusiness.TrainningTypeId.Value) : 0;
                examinationBusiness.ReadedHours = totalReadedHours;
                if (userExaminationList != null && userExaminationList.Count() > 0)
                {
                    var fitUserExaminationList = userExaminationList.Where(
                        p => p.IsRandom
                        && p.PassType == examinationBusiness.PassType && p.PassStandard == examinationBusiness.PassStandard
                        && (p.TrainningTypeId.HasValue && p.TrainningTypeId == examinationBusiness.TrainningTypeId)
                        && (p.RandomQuestionCount.HasValue && p.RandomQuestionCount.Value == examinationBusiness.TrainningQuestionCount));
                    examinationBusiness.TestedCount = fitUserExaminationList == null ? 0 : fitUserExaminationList.Count();
                    examinationBusiness.IsPass = fitUserExaminationList == null ? false : fitUserExaminationList.Where(p => p.IsPass.HasValue && p.IsPass.Value).Count() > 0;
                }
            }
            return examinationBusiness;
        }     
        public ExaminationBusiness GetExaminationBusiness(Guid userId, int businessType, Guid businessId, Guid? trainningExaminationId, out string errorMessage)
        {
            errorMessage = "";
            ExaminationBusiness originalItem = GetExaminationBusiness(userId, businessType, businessId, out errorMessage);
            if (originalItem == null) { return null; }
            if (trainningExaminationId.HasValue)
            {
                var viewTrainningExamination = __viewTrainningExaminationBLL.GetFirstOrDefaultEntityByExpression(string.Format("TrainningExaminationId=\"{0}\"*IsDelete=false*IsStop=false", trainningExaminationId.Value));
                if (viewTrainningExamination == null)
                {
                    errorMessage = string.Format("出错,找不到编码【\"{0}\"】的试卷信息", trainningExaminationId.Value);
                    return null;
                }
                return GetExaminationBusiness(userId, originalItem, viewTrainningExamination, out errorMessage);
            }
            return originalItem;
        }
        public ExaminationBusiness GetExaminationBusiness(Guid userId, ExaminationBusiness originalItem, ViewTrainningExamination item, out string errorMessage)
        {
            errorMessage = "";
            var totalReadedHours = __viewUserMaterialReadingTimeBLL.GetUserTotalReadingHours(userId, item.TrainningTypeId);
            var addItem = new com.Bynonco.LIMS.Model.Business.ExaminationBusiness();
            addItem.BusinessId = originalItem.BusinessId;
            addItem.BusinessType = originalItem.BusinessType;
            addItem.BusinessName = originalItem.BusinessName;
            addItem.TrainningExaminationId = item.Id;
            addItem.TrainningExaminationName = item.Name;
            addItem.TrainningTypeId = item.TrainningTypeId;
            addItem.IsNeedReadMaterialBeforeExamination = originalItem.IsNeedReadMaterialBeforeExamination;
            addItem.MinReadMaterialDateTime = item.MinReadMaterialDateTime.HasValue ? item.MinReadMaterialDateTime.Value : originalItem.MinReadMaterialDateTime;
            addItem.IsNeedExaminationBeforeAppointment = originalItem.IsNeedExaminationBeforeAppointment;
            addItem.MaxExaminationTime = item.MaxExaminationTime.HasValue ? item.MaxExaminationTime.Value : originalItem.MaxExaminationTime;
            addItem.TrainningTypeId = item.TrainningTypeId;
            addItem.TrainningTypeName = item.TrainningTypeName;
            addItem.PassType = item.PassType;
            addItem.PassStandard = item.PassStandard;
            addItem.ReadedHours = totalReadedHours;
            addItem.TrainningQuestionCount = item.QuestionCount != 0 ? item.QuestionCount : item.RandomQuestionCount.HasValue ? item.RandomQuestionCount.Value : originalItem.TrainningQuestionCount.HasValue ? originalItem.TrainningQuestionCount.Value : 0;
            if (item.QuestionCount > 0)
            {
                addItem.TrainningQuestionTotalScore = item.TotalScore;
                addItem.TrainningQuestionTotalTimeSpend = item.TotalTimeSpend;
            }
            var userExaminationList = GetEntitiesByExpression(string.Format("UserId=\"{0}\"*IsStop=false*IsDelete=false", userId));
            if (userExaminationList != null && userExaminationList.Count() > 0)
            {
                if (item.QuestionCount == 0
                    && (!item.RandomQuestionCount.HasValue || (item.RandomQuestionCount.HasValue && item.RandomQuestionCount == originalItem.TrainningQuestionCount))
                    && item.PassType == originalItem.PassType && item.PassStandard == originalItem.PassStandard
                    && (originalItem.TrainningTypeId.HasValue && item.TrainningTypeId == originalItem.TrainningTypeId.Value)
                    )
                {
                    var fitUserExaminationList = userExaminationList.Where(
                        p => p.IsRandom
                        && p.PassType == item.PassType && p.PassStandard == item.PassStandard
                        && (p.TrainningTypeId.HasValue && p.TrainningTypeId.Value == item.TrainningTypeId)
                        && (p.RandomQuestionCount.HasValue && p.RandomQuestionCount.Value == (item.RandomQuestionCount.HasValue ? item.RandomQuestionCount : originalItem.TrainningQuestionCount)));
                    addItem.TestedCount = fitUserExaminationList == null ? 0 : fitUserExaminationList.Count();
                    addItem.IsPass = fitUserExaminationList == null ? false : fitUserExaminationList.Where(p => p.IsPass.HasValue && p.IsPass.Value).Count() > 0;
                }
                else
                {
                    var fitUserExaminationList = userExaminationList.Where(p => p.TrainningExaminationId.HasValue && p.TrainningExaminationId.Value == item.Id);
                    addItem.TestedCount = fitUserExaminationList == null ? 0 : fitUserExaminationList.Count();
                    addItem.IsPass = fitUserExaminationList == null ? false : fitUserExaminationList.Where(p => p.IsPass.HasValue && p.IsPass.Value).Count() > 0;
                }
            }
            var trainningExaminationMaterialCount = __trainningExaminationMaterialBLL.CountModelListByExpression(string.Format("TrainningExaminationId=\"{0}\"", item.Id));
            addItem.TrainningMaterialCount = trainningExaminationMaterialCount == 0 ? originalItem.TrainningMaterialCount : trainningExaminationMaterialCount;
            return addItem;
        }
        public IList<ExaminationBusiness> GetExaminationBusinessPaper(Guid userId, ExaminationBusiness originalItem, out string errorMessage)
        {
            errorMessage = "";
            if (originalItem == null) return null;
            IList<ExaminationBusiness> examinationBusinessList = new List<ExaminationBusiness>();
            if (originalItem.IsNeedExaminationBeforeAppointment.HasValue && originalItem.IsNeedExaminationBeforeAppointment.Value)
            {
                var viewTrainningExaminationList = __viewTrainningExaminationBLL.GetEntitiesByExpression(string.Format("BusinessId=\"{0}\"*BusinessType=\"{1}\"*IsDelete=false*IsStop=false", originalItem.BusinessId, originalItem.BusinessType), null, "TrainningTypeXPath A");
                if (viewTrainningExaminationList != null && viewTrainningExaminationList.Count() > 0)
                {
                    foreach (var item in viewTrainningExaminationList)
                    {
                        var addItem = GetExaminationBusiness(userId, originalItem, item, out errorMessage);
                        examinationBusinessList.Add(addItem);
                    }
                }
                else
                {
                    examinationBusinessList.Add(originalItem);
                }
                return examinationBusinessList;
            }
            return null;
        }
    }
}
