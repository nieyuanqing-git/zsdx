using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class ExperimenterSubjectBLL : BLLBase<ExperimenterSubject>, IExperimenterSubjectBLL
    {
        private ISubjectBLL __subjectBLL;
        private IUserBLL __userBLL;
        private IDictCodeBLL __dictCodeBLL;
        public ExperimenterSubjectBLL()
        {
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, Guid? subjectDirectorId, IList<ExperimenterSubject> experimenterSubjectList,bool isCooperative, bool isSupressLazyLoad)
        {
            if (experimenterSubjectList == null || experimenterSubjectList.Count == 0) return;
            foreach (var experimenterSubject in experimenterSubjectList)
            {
                experimenterSubject.IsSupressLazyLoad = false;
                experimenterSubject.IsCooperative = isCooperative;
                experimenterSubject.InitOperate();
                OperateDecorate(userId,subjectDirectorId,isCooperative, experimenterSubject);
                experimenterSubject.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, Guid? subjectDirectorId,bool isCooperative, ExperimenterSubject experimenterSubject)
        {
            if (experimenterSubject == null) throw new ArgumentException("用户信息为空");
            var userPrivilige = userId.HasValue ? PriviligeFactory.GetSubjectPrivilige(userId.Value, experimenterSubject.SubjectId.Value) : null;
            if (userPrivilige == null)
            {
                experimenterSubject.ModifyOperate = "";
                experimenterSubject.DeleteOperate = "";
                return;
            }
            if (!userPrivilige.IsEnableEditExperimenterSubject)
                experimenterSubject.ModifyOperate = "";
            if (!userPrivilige.IsEnableDeleteExperimenterSubject)
                experimenterSubject.DeleteOperate = "";
            if (isCooperative)
            {
                if (userId.HasValue && subjectDirectorId.HasValue &&
                    userId.Value != subjectDirectorId.Value &&
                   (userPrivilige.IsEnableEditExperimenterSubject || userPrivilige.IsEnableDeleteExperimenterSubject))
                {
                    if (!experimenterSubject.IsAdmin.HasValue || !experimenterSubject.IsAdmin.Value)
                    {
                        if (experimenterSubject.OwnerId.HasValue && experimenterSubject.OwnerId.Value == userId.Value)
                            return;
                        experimenterSubject.ModifyOperate = "";
                        experimenterSubject.DeleteOperate = "";
                    }
                } 
            }
            if (!userPrivilige.IsEnableEditExperimenterSubject && userId != experimenterSubject.ExperimenterId.Value)
            {
                experimenterSubject.OddSumStr = "";
                experimenterSubject.PreAlert = null;
                experimenterSubject.RealCurency = null;
                experimenterSubject.TotalSumStr = "";
                experimenterSubject.Unappointment = null;
                experimenterSubject.UnUseable = null;
            }
        }
        public IList<ExperimenterSubject> GetExperimenterSubjectListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            var experimenterSubjectList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, null, experimenterSubjectList,false, isSupressLazyLoad);
            return experimenterSubjectList;
        }
        public IList<ExperimenterSubject> GetExperimenterSubjectListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewSubjectList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId,null, viewSubjectList,false, isSupressLazyLoad);
            return viewSubjectList;
        }
        public IList<ExperimenterSubject> GetExperimenterSubjectListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var experimenterSubjectList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId,null, experimenterSubjectList,false,isSupressLazyLoad);
            return experimenterSubjectList;
        }
        public GridData<ExperimenterSubject> GetGridExperimenterSubjectListByExpression(Guid? userId,Guid subjectId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewSubjectList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId,subjectId, viewSubjectList == null ? null : viewSubjectList.Data,false, isSupressLazyLoad);
            return viewSubjectList;
        }
        public GridData<ExperimenterSubject> GetGridExperimenterSubjectListByExpression(Guid? userId,Guid subjectId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var experimenterSubjectList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId,subjectId, experimenterSubjectList == null ? null : experimenterSubjectList.Data,false ,isSupressLazyLoad);
            return experimenterSubjectList;
        }
        public GridData<ExperimenterSubject> GetGridCooperativeExperimenterSubjectListByExpression(Guid? userId, Guid subjectId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var isShowOtherCooperativeTutor = __dictCodeBLL.GetDictCodeBoolValueByCode("ExperimenterSubject", "IsShowOtherCooperativeTutor");
            GridData<ExperimenterSubject> experimenterSubjectList = null;
            if (isShowOtherCooperativeTutor.HasValue && !isShowOtherCooperativeTutor.Value)
            {
                if (!userId.HasValue) return null;
                var itemList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
                if (itemList != null && itemList.Count() > 0)
                {
                    itemList = itemList.Where(p => p.TypeEnum != ExperimenterSubjectType.CooperatedTutor || p.ExperimenterId == userId.Value).ToList();
                    experimenterSubjectList = new GridData<ExperimenterSubject>()
                    {
                        Data = itemList.Skip((dataGridSettings.PageIndex - 1) * dataGridSettings.PageSize).Take(dataGridSettings.PageSize).ToList(),
                        Count = itemList.Count
                    };
                }
            }
            else experimenterSubjectList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, subjectId, experimenterSubjectList == null ? null : experimenterSubjectList.Data, true, isSupressLazyLoad);
            return experimenterSubjectList;
        }
        public ExperimenterSubject GetExperimenterSubject(Guid subjectId, Guid experimenterId)
        {
            if (subjectId == null || subjectId == Guid.Empty || experimenterId == null || experimenterId == Guid.Empty) return null;
            var experimenterSubjectList = GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"*ExperimenterId=\"{1}\"*IsDelete=false", subjectId.ToString(), experimenterId.ToString()));
            return experimenterSubjectList != null && experimenterSubjectList.Count > 0 ? experimenterSubjectList.First() : null;
        }
        private void Validates(Guid userId, Subject subject, double totalSum, double newTotalSum)
        {
            var subjectList = __subjectBLL.GetUserRelevantSubjects(userId);
            if (subjectList == null || subjectList.Count == 0) throw new Exception("找不到课题信息");
            var user = __userBLL.GetEntityById(userId);
            if (user.UserAccount == null) throw new Exception("找不到该用户帐户");
            double oddSumTemp = 0d;
            var subjectDirectorId = subjectList.FirstOrDefault(p => p.Id == subject.Id).SubjectDirectorId;
            if (subjectList.FirstOrDefault(p => p.Id == subject.Id).SubjectDirectorId == userId)
            {
                var subjectTemps = subjectList.Where(p => p.Id != subject.Id && p.SubjectDirectorId.Value == userId);
                foreach (var subjectTemp in subjectTemps)
                    oddSumTemp += subjectTemp.Experiments.Where(p => p.OwnerId == subjectTemp.SubjectDirectorId).Sum(p => p.OddSum);
                if (totalSum > user.UserAccount.TotalCurrency - oddSumTemp) throw new Exception("分配给成员经费总额超过了导师帐户余额");
            }
            else
            {
                var subjectTemp = subjectList.FirstOrDefault(p => p.Id == subject.Id);
                var experimenter = subjectTemp.Experiments.FirstOrDefault(p => p.ExperimenterId == userId);
                if (experimenter.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
                {
                    oddSumTemp = experimenter.OddSum;
                    var totalSumTemp = subjectTemp.Experiments.Where(p => p.OwnerId == userId).Sum(p => p.OddSum) + newTotalSum;
                    if (totalSumTemp > oddSumTemp) throw new Exception("分配给成员经费总额超过了导师帐户余额");
                }
            }

        }
        public bool Add(ExperimenterSubject[] entities, Guid ownerId)
        {
            var subjectList = __subjectBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", entities[0].SubjectId.ToString()));
            if (subjectList == null || subjectList.Count == 0) throw new Exception("找不到课题信息");
            var subject = subjectList.FirstOrDefault();
            List<ExperimenterSubject> lstExperimenterSubjects = new List<ExperimenterSubject>();
            lstExperimenterSubjects.AddRange(entities);
            foreach (var experimenterSubject in subject.Experiments)
            {
                if (!lstExperimenterSubjects.Any(p => p.Id == experimenterSubject.Id))
                    lstExperimenterSubjects.Add(experimenterSubject);
            }
            var totalSum = lstExperimenterSubjects.Where(p => p.OwnerId == ownerId).Sum(p => p.OddSum);
            var newTotalSum = 0d;
            foreach (var entity in entities)
            {
                var experimentTemp = subject.Experiments.FirstOrDefault(p => p.Id == entity.Id);
                if (experimentTemp == null)
                    newTotalSum += entity.OddSum;
                else
                    newTotalSum += entity.OddSum - experimentTemp.OddSum;
            }
            Validates(ownerId, subject, totalSum, newTotalSum);
            var tranTemp = SessionManager.Instance.BeginTransaction();
            try
            {

                foreach (var entity in entities)
                {
                    if (entity.UseMoneyType == 1) entity.OddSum = 0;
                    entity.Id = Guid.NewGuid();
                    entity.ApplyAt = DateTime.Now;
                    entity.JoinAt = DateTime.Now;
                    entity.TotalSum = entity.OddSum;
                    if (entity.Status == 2)
                    {
                        entity.StopAt = DateTime.Now;
                        entity.OddSum = 0;
                        entity.TotalSum -= entity.OddSum;
                    }
                }
                subject.OddSum += entities.Sum(p => p.OddSum);
                subject.TotalSum += entities.Sum(p => p.TotalSum);
                __subjectBLL.Save(new Subject[] { subject }, ref tranTemp, true);
                base.Add(entities, ref tranTemp, true);
                tranTemp.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "Add", ex.Message));
                tranTemp.RollbackTransaction();
                return false;
            }
            finally { tranTemp.Dispose(); }
        }
        private void AdjustOddSum(Subject entity, ExperimenterSubject oldExperimenter, double currentOddSum)
        {
            if (oldExperimenter.Experimenter.UserType.UserIdentityEnum == UserIdentity.Tutor ||oldExperimenter.Experimenter.UserType.UserIdentityEnum == UserIdentity.OutTutor)
            {
                var adjusted = currentOddSum - oldExperimenter.OddSum;
                if (adjusted < 0)
                {
                    var itExpers = entity.Experiments.Where(x => x.OwnerId == oldExperimenter.Experimenter.Id && x.OddSum > 0);
                    if (itExpers.Count() == 0) return;
                    var experSum = itExpers.Sum(x => x.OddSum);

                    var idle = oldExperimenter.OddSum - experSum;
                    var needAdjust = idle + adjusted;
                    if (needAdjust < 0)
                    {
                        var rest = needAdjust;
                        foreach (var exper in itExpers)
                        {
                            var tmp = (int)(needAdjust * (exper.OddSum / experSum));
                            rest -= tmp;
                            AdjustOddSum(entity, exper, exper.OddSum + tmp);
                            exper.TotalSum = exper.TotalSum + tmp;
                            exper.OddSum = exper.OddSum + tmp;
                        }

                        if (rest != 0)
                        {
                            var maxOddExper = itExpers.OrderByDescending(x => x.OddSum).FirstOrDefault();
                            if (maxOddExper != null)
                            {
                                AdjustOddSum(entity, maxOddExper, maxOddExper.OddSum + rest);
                                maxOddExper.OddSum = maxOddExper.OddSum + rest;
                            }
                        }
                    }
                }

            }
        }
        public bool Save(ExperimenterSubject[] entities, Guid ownerId, ExperimenterSubject editExperimenterSubject, ref XTransaction tran)
        {
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            var subject = __subjectBLL.GetEntityById(entities[0].SubjectId.Value);
            if (subject==null) throw new Exception("找不到课题信息");
            List<ExperimenterSubject> lstExperimenterSubjects = new List<ExperimenterSubject>();
            lstExperimenterSubjects.AddRange(entities);
            if (subject.Experiments != null)
            {
                foreach (var schoolExperimenterSubject in subject.Experiments)
                {
                    if (!lstExperimenterSubjects.Any(p => p.Id == schoolExperimenterSubject.Id))
                        lstExperimenterSubjects.Add(schoolExperimenterSubject);
                }
            }
            var totalSum = lstExperimenterSubjects.Where(p => p.OwnerId == ownerId).Sum(p => p.OddSum);
            var newTotalSum = 0d;
            foreach (var entity in entities)
            {
                var experimentTemp = subject.Experiments == null ? null : subject.Experiments.FirstOrDefault(p => p.Id == entity.Id);
                if (experimentTemp == null)
                    newTotalSum += entity.OddSum;
                else
                    newTotalSum += entity.OddSum - experimentTemp.OddSum;
            }
            if (editExperimenterSubject.UseMoneyTypeEnum== ExperimenterSubjectUseMoneyType.Assign)
                Validates(ownerId, subject, totalSum, newTotalSum);
            try
            {
                var oldlist = subject.Experiments == null ? null : subject.Experiments.Where(x => x.OwnerId == ownerId);
                var list = oldlist == null ? null : oldlist.ToList();
                double odd = 0d, total = 0d, difference = 0d;
                foreach (var entity in entities)
                {
                    if (oldlist != null && oldlist.Any(x => x.ExperimenterId == entity.ExperimenterId))
                    {
                        var oldExperimenter = oldlist.FirstOrDefault(p => p.ExperimenterId == entity.ExperimenterId);
                        var currentOddSum = entity.OddSum;
                        difference += oldExperimenter.OddSum;
                        AdjustOddSum(subject, oldExperimenter, currentOddSum);

                        if (oldExperimenter.StatusEnum == ExperimenterSubjectStatus.UnAuthorized && entity.StatusEnum == ExperimenterSubjectStatus.Authorized)
                        {
                            oldExperimenter.JoinAt = DateTime.Now;
                        }
                        oldExperimenter.StopAt = null;
                        if (entity.StatusEnum == ExperimenterSubjectStatus.Stoped)
                        {
                            oldExperimenter.StopAt = DateTime.Now;
                            entity.OddSum = 0;
                        }
                        oldExperimenter.Status = entity.Status;
                        switch (entity.UseMoneyTypeEnum)
                        {
                            case ExperimenterSubjectUseMoneyType.Assign:
                                break;
                            case ExperimenterSubjectUseMoneyType.TutorAccount:
                                entity.OddSum = 0;
                                break;
                        }
                        oldExperimenter.IsAdmin = entity.IsAdmin;
                        oldExperimenter.UseMoneyType = entity.UseMoneyType;
                        oldExperimenter.TotalSum += (entity.OddSum - oldExperimenter.OddSum);
                        oldExperimenter.OddSum = entity.OddSum;
                        oldExperimenter.Unappointment = entity.Unappointment;
                        oldExperimenter.UnUseable = entity.UnUseable;
                        oldExperimenter.PreAlert = entity.PreAlert;
                        oldExperimenter.IsDelete = false;
                        odd += oldExperimenter.OddSum;
                        total += oldExperimenter.TotalSum;
                    }
                    else
                    {
                        entity.TotalSum = entity.OddSum;
                        base.Add(new ExperimenterSubject[] { entity }, ref tran, true);
                        odd += entity.OddSum;
                        total += entity.TotalSum;
                    }
                }
                if (subject.Director.Id == ownerId)
                {
                    subject.OddSum += odd - difference;
                    subject.TotalSum += total - difference;
                }
                __subjectBLL.Save(new Subject[] { subject }, ref tran, true);
                if (subject.Experiments != null && subject.Experiments.Count > 0)
                    base.Save(subject.Experiments, ref tran, true);
                if (!isSupress) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "Save", ex.Message));
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress)tran.Dispose(); }
        }
    }
}