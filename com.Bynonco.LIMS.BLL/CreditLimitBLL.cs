using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class CreditLimitBLL : BLLBase<CreditLimit>, ICreditLimitBLL
    {
        private IUserBLL __userBLL;
        private ISubjectBLL __subjectBLL;
        private IExperimenterSubjectBLL __experimenterSubjectBLL;
        public CreditLimitBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
        }
        public CreditLimit GetUserCreditLimit(Guid userId, Guid? subjectId)
        {

            
            CreditLimit creditLimit = new CreditLimit();
            var user = __userBLL.GetEntityById(userId);
            if (!subjectId.HasValue)
            {
                if (user.UserAccount == null || user.UserAccount.CreditLimit == null) return creditLimit;
                return user.UserAccount.CreditLimit;
            }
            var subject = __subjectBLL.GetEntityById(subjectId.Value);
            if (subject == null) return creditLimit;
            if (subject.SubjectDirectorId.Value == userId)
            {
                if (user.UserAccount == null || user.UserAccount.CreditLimit == null) return creditLimit;
                return user.UserAccount.CreditLimit;
            }
            var experimenterSubjectList = __experimenterSubjectBLL.GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"*ExperimenterId=\"{1}\"", subjectId.Value.ToString(), userId.ToString()));
            if (experimenterSubjectList == null || experimenterSubjectList.Count == 0) return creditLimit;
            var experimenterSubject = experimenterSubjectList.First();
            creditLimit.PreAlert = experimenterSubject.PreAlert ?? 0;
            creditLimit.UnAppointment = experimenterSubject.Unappointment ?? 0;
            creditLimit.UnUseable = experimenterSubject.UnUseable ?? 0;
            return creditLimit;
        }
        public CreditLimit GenerateDefaultCreditLimit(ref XTransaction tran)
        {
            var creditLimit = GetFirstOrDefaultEntityByExpression("IsDefault=true*IsStop=false*IsDelete=false");
            if (creditLimit == null)
            {
                creditLimit = new CreditLimit()
                {
                    Id = Guid.NewGuid(),
                    IsDefault = true,
                    IsDelete = false,
                    IsStop = false,
                    Name = "普通",
                    PreAlert = 0d,
                    UnAppointment = 0d,
                    UnUseable = 0d
                };
                Add(new CreditLimit[] { creditLimit }, ref tran, true);
            }
            return creditLimit;
        }
    }
}