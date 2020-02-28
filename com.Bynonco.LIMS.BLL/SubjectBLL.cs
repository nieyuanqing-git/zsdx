using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class SubjectBLL : BLLBase<Subject>, ISubjectBLL
    {
        private IUserBLL __userBLL;
        public SubjectBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
        }
        public Subject GetSubjectByTurtorId(Guid? turtorId)
        {
            if (turtorId == null || turtorId == Guid.Empty) return null;
            var subjectList = GetEntitiesByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", turtorId.Value.ToString()), null, "Status");
            return subjectList != null && subjectList.Count > 0 ? subjectList.First() : null;
        }
        /// <summary>
        /// 获取用户有关课题组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Subject> GetUserRelevantSubjects(Guid userId)
        {
            var experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            List<Subject> lstSubjects = new List<Subject>();
            var selfSubjectList = GetEntitiesByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", userId.ToString()), null, "Status");
            if (selfSubjectList != null && selfSubjectList.Count > 0)
                lstSubjects.AddRange(selfSubjectList);
            var subjectExperimenterList = experimenterSubjectBLL.GetEntitiesByExpression(string.Format("ExperimenterId=\"{0}\"*IsDelete=false", userId.ToString()), null, "Status D");
            if (subjectExperimenterList != null && subjectExperimenterList.Count > 0)
            {
                foreach (var item in subjectExperimenterList)
                {
                    var subject = item.GetSubject();
                    if (subject != null) lstSubjects.Add(subject);
                }
            }
            return lstSubjects;
        }
        /// <summary>
        /// 获取用户加入课题组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Subject GetUserSelfJoinSubject(Guid userId)
        {
            // 课题组成员
            var experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            var user = __userBLL.GetEntityById(userId);
            Subject subject = null;
            if (user.IsUndergraduate())
            {
                // 本科生特殊处理，优先返回其负责课题组
                var subjectList = GetEntitiesByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", userId.ToString()), null, "Status");
                return subjectList == null || subjectList.Count == 0 ? null : subjectList.First();
            }
            if (user.TutorId.HasValue)
            {
                // 优先返回用户分配导师课题组
                var experimenterSubjectList = experimenterSubjectBLL.GetEntitiesByExpression(string.Format("ExperimenterId=\"{0}\"*OwnerId=\"{1}\"*IsDelete=false", userId.ToString(), user.TutorId.Value.ToString()), null, "Status D");
                if (experimenterSubjectList != null && experimenterSubjectList.Count > 0)
                    return experimenterSubjectList.First().GetSubject();
            }
            else
            {
                var subjectList = GetEntitiesByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", userId.ToString()), null, "Status");
                return subjectList == null || subjectList.Count == 0 ? null : subjectList.First();
            }
            return subject;
        }
        public IList<Subject> GetUserCooperativeSubjects(Guid userId)
        {
            var experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            IList<Subject> subjectList = null;
            IList<ExperimenterSubject> experimenterSubjectList = null;
            var user = __userBLL.GetEntityById(userId);
            if (!user.TutorId.HasValue)
                experimenterSubjectList = experimenterSubjectBLL.GetEntitiesByExpression(string.Format("ExperimenterId=\"{0}\"*IsDelete=false", userId.ToString()));
            else
                experimenterSubjectList = experimenterSubjectBLL.GetEntitiesByExpression(string.Format("ExperimenterId=\"{0}\"*OwnerId=-\"{1}\"*IsDelete=false", userId.ToString(), user.TutorId.Value.ToString()));
            if (experimenterSubjectList != null && experimenterSubjectList.Count > 0)
                return experimenterSubjectList.Select(p => p.GetSubject()).ToList();
            return subjectList;
        }
        public IList<Subject> GetSubjectListByDirectorId(Guid directorId)
        {
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            var subjectList = GetEntitiesByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", directorId), null, "Status");
            return subjectList;
        }
        /// <summary> 扣费 </summary>
        /// <param name="userId"></param>
        /// <param name="subjectId"></param>
        /// <param name="paymentMethod"></param>
        /// <param name="fee"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public UserAccount Deduct(Guid userId, Guid subjectId, PaymentMethod paymentMethod, double fee, ref XTransaction tran)
        {
            // mark: 课题组扣费
            var experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            var user = __userBLL.GetEntityById(userId);
            if (user == null) throw new ArgumentException("出错,找不到用户信息");
            if (paymentMethod != PaymentMethod.SubjectDirectorPay && paymentMethod != PaymentMethod.TutorSubjectFunds)
                throw new Exception("出错,支付方式错误");
            var subject = GetEntityById(subjectId);
            if (subject == null) throw new ArgumentException("出错,找不到课题信息");
            var subjectDirector = subject.Director;
            if (subjectDirector.UserAccount == null) throw new Exception("出错,找不到用户账户信息");
            UserAccount userAccount = subjectDirector.UserAccount;
            var experimenterSubject = experimenterSubjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectId=\"{0}\"*ExperimenterId=\"{1}\"*IsDelete=false", subjectId.ToString(), user.Id.ToString()));
            if (experimenterSubject != null && fee != 0d)
            {
                bool isNeedDeductSubject = false;
                if (experimenterSubject.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
                {
                    experimenterSubject.OddSum -= fee;
                    experimenterSubjectBLL.Save(new ExperimenterSubject[] { experimenterSubject }, ref tran, true);
                    isNeedDeductSubject = true;
                }
                var subjectDirectorExperimenter = experimenterSubjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectId=\"{0}\"*ExperimenterId=\"{1}\"*IsDelete=false", subjectId.ToString(), experimenterSubject.OwnerId.Value));
                if (subjectDirectorExperimenter != null && subjectDirectorExperimenter.UseMoneyTypeEnum == ExperimenterSubjectUseMoneyType.Assign)
                {
                    subjectDirectorExperimenter.OddSum -= fee;
                    experimenterSubjectBLL.Save(new ExperimenterSubject[] { subjectDirectorExperimenter }, ref tran, true);
                    isNeedDeductSubject = true;
                }
                if (isNeedDeductSubject)
                {
                    subject.OddSum -= fee;
                    Save(new Subject[] { subject }, ref tran, true);
                }
            }
            return userAccount;
        }
        public void GenerateSubjectAndProjectIdByPayerId(Guid payerId, Guid oldSubjectId, out Guid subjectId, out  Guid? projectId)
        {
            ISubjectProjectBLL subjectProjectBLL = BLLFactory.CreateSubjectProjectBLL();
            projectId = null;
            var payer = __userBLL.GetEntityById(payerId);
            if (payer == null) throw new Exception(string.Format("找不到编码为【{0}】的付费人信息", payerId));
            var subject = GetFirstOrDefaultEntityByExpression(string.Format("SubjectDirectorId=\"{0}\"", payerId), null, "IsDelete");
            if (subject == null) throw new Exception(string.Format("找不到负责人编码为【{0}】的课题组信息", payerId));
            subjectId = subject.Id;
            var subjectProject = subjectProjectBLL.GetEntityById(oldSubjectId);
            if (subjectProject != null) projectId = subjectProject.Id;
        }
    }
}