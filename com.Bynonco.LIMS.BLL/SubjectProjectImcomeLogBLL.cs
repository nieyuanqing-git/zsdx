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
    public class SubjectProjectImcomeLogBLL : BLLBase<SubjectProjectImcomeLog>, ISubjectProjectImcomeLogBLL
    {
        private IViewSubjectProjectImcomeBLL __viewSubjectProjectImcomeBLL;
        private ISubjectProjectImcomeErrorLogBLL __subjectProjectImcomeErrorLogBLL;
        private IUserBLL __userBLL;
        private IUserAccountBLL __userAccountBLL;
        private ISubjectBLL __subjectBLL;
        private ISubjectProjectBLL __subjectProjectBLL;
        private IDepositRecordBLL __depositRecordBLL;

        public SubjectProjectImcomeLogBLL()
        {
            __viewSubjectProjectImcomeBLL = BLLFactory.CreateViewSubjectProjectImcomeBLL();
            __subjectProjectImcomeErrorLogBLL = BLLFactory.CreateSubjectProjectImcomeErrorLogBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __userAccountBLL = BLLFactory.CreateUserAccountBLL();
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __subjectProjectBLL = BLLFactory.CreateSubjectProjectBLL();
            __depositRecordBLL = BLLFactory.CreateDepositRecordBLL();

        }
        public void DealSubjectProjectImcomeInfo()
        {
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                bool isDealMoney = false;
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "" };
                var viewSubjectProjectImcomeList = __viewSubjectProjectImcomeBLL.GetEntitiesByExpression(dataGridSettings, null, "DepositAt", true);
                if (viewSubjectProjectImcomeList == null || viewSubjectProjectImcomeList.Count() == 0) return;
                if (viewSubjectProjectImcomeList.Where(p => p.DealType == 2).Count() == viewSubjectProjectImcomeList.Count()) return;
                var subjectProjectImcomeErrorLogList = __subjectProjectImcomeErrorLogBLL.GetEntitiesByExpression(dataGridSettings, null, "", true);
                if (subjectProjectImcomeErrorLogList != null && subjectProjectImcomeErrorLogList.Count() > 0)
                    __subjectProjectImcomeErrorLogBLL.Delete(subjectProjectImcomeErrorLogList.Select(p => p.Id), ref tran, true);
                IList<UserAccount> userAccountList = new List<UserAccount>();
                IList<SubjectProject> subjectProjectList = new List<SubjectProject>();
                foreach (var item in viewSubjectProjectImcomeList)
                {

                    var user = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsOuter=false*IsDel=false", item.ProjectAdminUserNum), null, "", true);
                    if (user == null)
                    {
                        AddSubjectProjectImcomeErrorLog(item, ProjectImcomeErrorType.UserNull, ref tran);
                        continue;
                    }
                    UserAccount userAccount = null;
                    if (isDealMoney)
                    {
                        userAccount = userAccountList.Where(p => p.UserId == user.Id).FirstOrDefault();
                        if (userAccount == null)
                        {
                            userAccount = __userAccountBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*IsDelete=false", user.Id));
                            if (userAccount == null)
                            {
                                AddSubjectProjectImcomeErrorLog(item, ProjectImcomeErrorType.UserAccountNull, ref tran);
                                continue;
                            }
                            userAccountList.Add(userAccount);
                        }
                    }
                    var subject = __subjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", user.Id),null,"",true);
                    if(subject == null)
                    {
                        AddSubjectProjectImcomeErrorLog(item, ProjectImcomeErrorType.NotSubjectDirector, ref tran);
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(item.ProjectName))
                    {
                        AddSubjectProjectImcomeErrorLog(item, ProjectImcomeErrorType.SubjectProjectNull, ref tran);
                        continue;
                    }
                    var subjectProject = subjectProjectList.Where(p => p.SubjectId == subject.Id && (p.ProjectNum == item.ProjectNum || p.Name == item.ProjectName)).FirstOrDefault();
                    if (subjectProject == null)
                    {
                        subjectProject = __subjectProjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectId=\"{0}\"*(ProjectNum=\"{1}\"+Name=\"{2}\")*IsDelete=false", subject.Id, item.ProjectNum, item.ProjectName.Replace("\"", "＂")), null, "", true);
                        if (subjectProject == null)
                        {
                            subjectProject = new SubjectProject();
                            subjectProject.Id = Guid.NewGuid();
                            subjectProject.SubjectId = subject.Id;
                            subjectProject.Name = item.ProjectName.Replace("\"","＂");
                            subjectProject.Description = "系统自动创建(科研系统对接数据)";
                            subjectProject.BeginAt = item.ProjectBeginAt;
                            subjectProject.EndAt = item.ProjectEndAt;
                            subjectProject.IsDelete = false;
                            subjectProject.ProjectType = item.ProjectType;
                            subjectProject.ProjectNum = item.ProjectNum;
                            __subjectProjectBLL.Add(new List<SubjectProject> { subjectProject }, ref tran, true);
                            subjectProjectList.Add(subjectProject);
                        }
                    }
                    if (item.DealType == (int)ProjectImcomeDealType.Insert)
                    {
                        if (isDealMoney && userAccount != null)
                        {
                            AddDepositRecord(item, user.Id, userAccount.Id, subjectProject.Id, ref tran);
                            userAccount.RealCurrency += item.ImcomeSum;
                        }
                        SaveSubjectProjectImcomeLog(item, user.Id, subjectProject.Id, ref tran);
                    }
                    else if (item.DealType == (int)ProjectImcomeDealType.Alter)
                    {
                        if (isDealMoney && userAccount != null)
                        {
                            DeleteSubjectProjectImcomeLog(item, userAccountList, ref tran);
                            AddDepositRecord(item, user.Id, userAccount.Id, subjectProject.Id, ref tran);
                            userAccount.RealCurrency += item.ImcomeSum;
                        }
                        SaveSubjectProjectImcomeLog(item, user.Id, subjectProject.Id, ref tran);
                    }
                    else if (item.DealType == (int)ProjectImcomeDealType.Delete)
                    {
                        if (isDealMoney && userAccount != null)
                        {
                            DeleteSubjectProjectImcomeLog(item, userAccountList, ref tran);
                        }
                        SaveSubjectProjectImcomeLog(item, user.Id, subjectProject.Id, ref tran);
                    }
                }
                if (isDealMoney && userAccountList != null && userAccountList.Count() > 0)
                {
                    __userAccountBLL.Save(userAccountList, ref tran, true);
                }
                tran.CommitTransaction();
                GetMutiResult("UPDATE SubjectProjectImcome SET IsDelete=1");
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                log.ErrorFormat("{0}:{1}", "DealSubjectProjectImcomeInfo", ex.Message);
                return;
            }
            finally { tran.Dispose(); }
            return;
        }

        private void AddSubjectProjectImcomeErrorLog(ViewSubjectProjectImcome item, ProjectImcomeErrorType projectImcomeErrorType, ref XTransaction tran,bool isSupress = true)
        {
            var addErrorLog = new SubjectProjectImcomeErrorLog();
            addErrorLog.Id = Guid.NewGuid();
            addErrorLog.ProjectImcomeId = item.ProjectImcomeId;
            addErrorLog.ImcomeNum = item.ImcomeNum;
            addErrorLog.ImcomeSum = item.ImcomeSum;
            addErrorLog.ImcomeAt = item.ImcomeAt;
            addErrorLog.DepositUserNum = item.DepositUserNum;
            addErrorLog.DepositUserName = item.DepositUserName;
            addErrorLog.DepositAt = item.DepositAt;
            addErrorLog.ProjectId = item.ProjectId;
            addErrorLog.ProjectNum = item.ProjectNum;
            addErrorLog.ProjectName = item.ProjectName;
            addErrorLog.ProjectAdminUserNum = item.ProjectAdminUserNum;
            addErrorLog.ProjectAdminUserName = item.ProjectAdminUserName;
            addErrorLog.ProjectBeginAt = item.ProjectBeginAt;
            addErrorLog.ProjectEndAt = item.ProjectEndAt;
            addErrorLog.ProjectType = item.ProjectType;
            addErrorLog.CreateAt = item.CreateAt;
            addErrorLog.IsDelete = item.IsDelete;
            addErrorLog.DealType = item.DealType;
            addErrorLog.ErrorReason = projectImcomeErrorType.ToCnName();
            __subjectProjectImcomeErrorLogBLL.Add(new List<SubjectProjectImcomeErrorLog>() { addErrorLog }, ref tran, isSupress);
        }

        private void AddDepositRecord(ViewSubjectProjectImcome item, Guid userId, Guid userAccountId, Guid subjectProjectId, ref XTransaction tran, bool isSupress = true)
        {
            AddDepositRecord(item.ProjectImcomeId, item.ImcomeSum,userId, userAccountId, subjectProjectId, ref tran, isSupress);
        }

        private void AddDepositRecord(string projectImcomeId, double imcomeSum, Guid userId, Guid userAccountId, Guid subjectProjectId, ref XTransaction tran, bool isSupress = true)
        {
            var depositRecord = new DepositRecord();
            depositRecord.Id = Guid.NewGuid();
            depositRecord.UserId = userId;
            depositRecord.CurencyType = (int)DepositCurencyType.RealCurency;
            depositRecord.AccountId = userAccountId;
            depositRecord.DepositSum = (decimal)imcomeSum;
            depositRecord.HasChecked = true;
            depositRecord.CheckTime = DateTime.Now;
            depositRecord.CheckRemark = "系统自动审核(科研系统对接数据)";
            depositRecord.ProjectImcomeId = projectImcomeId;
            depositRecord.SubjectProjectId = subjectProjectId;
            __depositRecordBLL.Add(new DepositRecord[] { depositRecord }, ref tran, isSupress);
        }

        private void SaveSubjectProjectImcomeLog(ViewSubjectProjectImcome item, Guid userId, Guid subjectProjectId, ref XTransaction tran, bool isSupress = true)
        {
            var subjectProjectImcomeLog = GetFirstOrDefaultEntityByExpression(string.Format("ProjectImcomeId=\"{0}\"", item.ProjectImcomeId), null, "", true);
            bool isNew = false;
            if (subjectProjectImcomeLog == null)
            {
                subjectProjectImcomeLog =new SubjectProjectImcomeLog();
                subjectProjectImcomeLog.Id = Guid.NewGuid();
                isNew = true;
                subjectProjectImcomeLog.Remark = string.Format("{0}成功对接新建", DateTime.Now.ToString("yyyyMMdd"));
            }
            else if (!item.IsDelete)
            {
                string remark = "";
                if (subjectProjectImcomeLog.ProjectAdminUserNum != item.ProjectAdminUserNum)
                    remark += (remark == "" ? "" : ",") + string.Format("职工号:{0}->{1}", subjectProjectImcomeLog.ProjectAdminUserNum, item.ProjectAdminUserNum);
                if (subjectProjectImcomeLog.ProjectNum != item.ProjectNum)
                    remark += (remark == "" ? "" : ",") + string.Format("项目号:{0}->{1}", subjectProjectImcomeLog.ProjectNum, item.ProjectNum);
                if (subjectProjectImcomeLog.ImcomeSum.HasValue && subjectProjectImcomeLog.ImcomeSum.Value != item.ImcomeSum)
                    remark += (remark == "" ? "" : ",") + string.Format("金额:{0}->{1}", subjectProjectImcomeLog.ImcomeSum.Value, item.ImcomeSum);
                if (remark != "") remark = string.Format("{0}成功对接修改[{1}]", DateTime.Now.ToString("yyyyMMdd"), remark);
                subjectProjectImcomeLog.Remark += (subjectProjectImcomeLog.Remark == "" ? "" : ";") + remark;
            }
            else
            {
                var remark = string.Format("{0}成功对接删除", DateTime.Now.ToString("yyyyMMdd"));
                subjectProjectImcomeLog.Remark += (subjectProjectImcomeLog.Remark == "" ? "" : ";") + remark;
            }
            subjectProjectImcomeLog.ProjectImcomeId = item.ProjectImcomeId;
            subjectProjectImcomeLog.ImcomeSum = item.ImcomeSum;
            subjectProjectImcomeLog.ProjectAdminUserNum = item.ProjectAdminUserNum;
            subjectProjectImcomeLog.UserId = userId;
            subjectProjectImcomeLog.ProjectNum = item.ProjectNum;
            subjectProjectImcomeLog.SubjectProjectId = subjectProjectId;
            subjectProjectImcomeLog.IsDelete = item.IsDelete;
            subjectProjectImcomeLog.UpdateAt = DateTime.Now;
            if(isNew) Add(new List<SubjectProjectImcomeLog>() { subjectProjectImcomeLog }, ref tran, isSupress);
            else Save(new List<SubjectProjectImcomeLog>() { subjectProjectImcomeLog }, ref tran, isSupress);
        }

        private void DeleteSubjectProjectImcomeLog(ViewSubjectProjectImcome item, IList<UserAccount> userAccountList, ref XTransaction tran, bool isSupress = true)
        {
            var oldLog = GetFirstOrDefaultEntityByExpression(string.Format("ProjectImcomeId=\"{0}\"", item.ProjectImcomeId), null, "", isSupress);
            if (oldLog.UserId.HasValue && oldLog.ImcomeSum.HasValue && oldLog.SubjectProjectId.HasValue)
            {
                UserAccount oldAccount = userAccountList.Where(p => p.UserId == oldLog.UserId.Value).FirstOrDefault();
                if (oldAccount == null)
                {
                    oldAccount = __userAccountBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*IsDelete=false", oldLog.UserId.Value));
                    if (oldAccount == null) return;
                    userAccountList.Add(oldAccount);
                }
                AddDepositRecord(oldLog.ProjectImcomeId, 0 - oldLog.ImcomeSum.Value,oldLog.UserId.Value, oldAccount.Id, oldLog.SubjectProjectId.Value, ref tran, isSupress);
                oldAccount.RealCurrency -= oldLog.ImcomeSum.Value;
            }
        }

    }
}