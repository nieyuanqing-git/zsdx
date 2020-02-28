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
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class SubjectProjectSyncBLL : BLLBase<SubjectProjectSync>, ISubjectProjectSyncBLL
    {
        private IUserBLL __userBLL;
        private IUserAccountBLL __userAccountBLL;
        private ISubjectBLL __subjectBLL;
        private ISubjectProjectBLL __subjectProjectBLL;
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private IDictCodeBLL __dictCodeBLL;
        private ISubjectAchievementBLL __subjectAchievementBLL;

        public SubjectProjectSyncBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userAccountBLL = BLLFactory.CreateUserAccountBLL();
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __subjectProjectBLL = BLLFactory.CreateSubjectProjectBLL();
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __subjectAchievementBLL = BLLFactory.CreateSubjectAchievementBLL();
        }
        public void DealSubjectProjectSyncInfo(bool dealSubjectAchievement = true)
        {
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "IsHandled=\"0\"*ProjectBeginAt=-null" };
                var subjectProjectSyncList = GetEntitiesByExpression(dataGridSettings, null, "CreateAt", true);
                if (subjectProjectSyncList == null || subjectProjectSyncList.Count() == 0) return;
                IList<SubjectProject> subjectProjectList = new List<SubjectProject>();
                var dictCodeType_ComeFrom = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"*IsDelete=false", "SubjectProjectComeFrom"));
                var dictCodeType_Domain = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"*IsDelete=false", "SubjectProjectDomain"));
                if (dictCodeType_ComeFrom == null) throw new Exception("找不到辅助字典->课题组管理->课题项目->项目来源");
                if (dictCodeType_Domain == null) throw new Exception("找不到辅助字典->课题组管理->课题项目->项目所属领域");
                var dictCodeList_ComeFrom = __dictCodeBLL.GetEntitiesByExpression(string.Format("DictCodeTypeId=\"{0}\"*IsDelete=false", dictCodeType_ComeFrom.Id), null, "Ext1");
                var dictCodeList_Domain = __dictCodeBLL.GetEntitiesByExpression(string.Format("DictCodeTypeId=\"{0}\"*IsDelete=false", dictCodeType_Domain.Id), null, "Ext1");
                if (dictCodeList_ComeFrom == null) dictCodeList_ComeFrom = new List<DictCode>();
                if (dictCodeList_Domain == null) dictCodeList_ComeFrom = new List<DictCode>();
                foreach (var item in subjectProjectSyncList)
                {
                    item.IsHandled = 1;
                    var user = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsOuter=false*IsDel=false", item.ProjectAdminUserNum), null, "", true);
                    if (user == null)
                    {
                        item.IsOK = false;
                        item.Remark = string.Format("找不到职工号为【{0}】的用户信息", item.ProjectAdminUserNum);
                        continue;
                    }

                    var subject = __subjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", user.Id), null, "", true);
                    if (subject == null)
                    {
                        item.IsOK = false;
                        item.Remark = string.Format("找不到职工号为【{0}】的课题信息", item.ProjectAdminUserNum);
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(item.ProjectName))
                    {
                        item.IsOK = false;
                        item.Remark = string.Format("找不到职工号为【{0}】的科研项目名称", item.ProjectAdminUserNum);
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(item.ProjectNum))
                    {
                        item.IsOK = false;
                        item.Remark = string.Format("找不到职工号为【{0}】的科研项目编号", item.ProjectAdminUserNum);
                        continue;
                    }
                    DictCode itemComFrom = null;
                    DictCode itemDomain = null;
                    if (!string.IsNullOrWhiteSpace(item.ProjectComeFrom))
                    {
                        itemComFrom = dictCodeList_ComeFrom.Where(p => p.Name == item.ProjectComeFrom.Trim()).FirstOrDefault();
                        if (itemComFrom == null)
                        {
                            itemComFrom = new DictCode()
                            {
                                Id = Guid.NewGuid(),
                                Code = item.ProjectComeFrom.Trim(),
                                Name = item.ProjectComeFrom.Trim(),
                                Value = item.ProjectComeFrom.Trim(),
                                DictCodeTypeId = dictCodeType_ComeFrom.Id,
                                Remark = "系统自动创建(科研系统对接数据)"
                            };
                            __dictCodeBLL.Add(new List<DictCode> { itemComFrom }, ref tran, true);
                            dictCodeList_ComeFrom.Add(itemComFrom);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.ProjectDomain))
                    {
                        itemDomain = dictCodeList_Domain.Where(p => p.Name == item.ProjectDomain.Trim()).FirstOrDefault();
                        if (itemDomain == null)
                        {
                            itemDomain = new DictCode()
                            {
                                Id = Guid.NewGuid(),
                                Code = item.ProjectDomain.Trim(),
                                Name = item.ProjectDomain.Trim(),
                                Value = item.ProjectDomain.Trim(),
                                DictCodeTypeId = dictCodeType_Domain.Id,
                                Remark = "系统自动创建(科研系统对接数据)"
                            };
                            __dictCodeBLL.Add(new List<DictCode> { itemDomain }, ref tran, true);
                            dictCodeList_Domain.Add(itemDomain);
                        }
                    }

                    var subjectProject = subjectProjectList.Where(p => p.SubjectId == subject.Id && (p.ProjectNum == item.ProjectNum && p.Name == item.ProjectName)).FirstOrDefault();
                    if (subjectProject == null)
                    {
                        subjectProject = __subjectProjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectId=\"{0}\"*(ProjectNum=\"{1}\"*Name=\"{2}\")*IsDelete=false", subject.Id, item.ProjectNum, item.ProjectName.Replace("\"", "＂")), null, "", true);
                        bool isNewSubjectProject = false;
                        if (subjectProject == null)
                        {
                            subjectProject = new SubjectProject();
                            subjectProject.Id = Guid.NewGuid();
                            isNewSubjectProject = true;
                        }
                        subjectProject.SubjectId = subject.Id;
                        subjectProject.Name = item.ProjectName.Replace("\"", "＂");
                        subjectProject.Description = "系统自动同步(科研系统对接数据)";
                        subjectProject.BeginAt = item.ProjectBeginAt;
                        subjectProject.IsDelete = false;
                        subjectProject.ProjectType = item.ProjectType;
                        subjectProject.ProjectNum = item.ProjectNum;
                        if (itemComFrom != null) subjectProject.ProjectComeFrom = itemComFrom.Id;
                        if (itemDomain != null) subjectProject.ProjectDomain = itemDomain.Id;
                        if (isNewSubjectProject)
                            __subjectProjectBLL.Add(new List<SubjectProject> { subjectProject }, ref tran, true);
                        else
                            __subjectProjectBLL.Save(new List<SubjectProject> { subjectProject }, ref tran, true);
                        item.IsOK = true;
                        item.Remark = "同步科研项目成功";
                        subjectProjectList.Add(subjectProject);
                        if (dealSubjectAchievement)
                        {
                            var subjectAchievement =
                                __subjectAchievementBLL.GetFirstOrDefaultEntityByExpression(
                                    string.Format("SubjectNum=\"{0}\"*SubjectName=\"{1}\"*IsDelete=false",
                                                  item.ProjectNum, item.ProjectName.Replace("\"", "＂")), null, "", true);
                            bool isNewSubjectAchievement = false;
                            if (subjectAchievement == null)
                            {
                                subjectAchievement = new SubjectAchievement
                                    {
                                        Id = Guid.NewGuid()
                                    };
                                isNewSubjectAchievement = true;
                            }

                            subjectAchievement.EndTime = subjectProject.EndAt;
                            subjectAchievement.SubjectName = item.ProjectName.Replace("\"", "＂");
                            subjectAchievement.SubjectChief = item.ProjectAdminUserName;
                            subjectAchievement.SubjectNum = subjectProject.ProjectNum;
                            subjectAchievement.StartTime = item.ProjectBeginAt; if (itemComFrom != null) subjectAchievement.SubjectCome = itemComFrom.Id;
                            subjectAchievement.SubjectChiefId = null;
                            if (!string.IsNullOrWhiteSpace(subjectAchievement.SubjectChief))
                            {
                                var userItems = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"", subjectAchievement.SubjectChief));
                                if (userItems != null) subjectAchievement.SubjectChiefId = userItems.Id;
                            }
                            if(isNewSubjectAchievement)
                            {
                                __subjectAchievementBLL.Add(new List<SubjectAchievement> { subjectAchievement }, ref tran, true);
                            }
                            else
                            {
                                __subjectAchievementBLL.Save(new List<SubjectAchievement> { subjectAchievement }, ref tran, true);
                            }
                        }
                    }
                }
                Save(subjectProjectSyncList, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                log.ErrorFormat("{0}:{1}", "DealSubjectProjectSyncInfo", ex.Message);
                return;
            }
            finally { tran.Dispose(); }
            return;
        }
    }
}