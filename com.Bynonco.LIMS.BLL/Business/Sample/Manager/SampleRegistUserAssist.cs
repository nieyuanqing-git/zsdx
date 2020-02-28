using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public partial class SampleManager
    {
        private Guid RegisterNewUser(User user, ref XTransaction tran, out UserAccount userAccountTmep)
        {
            user.Id = Guid.NewGuid();
            user.RegisterTime = DateTime.Now;
            _userBLL.Add(new User[] { user }, ref tran, true);
            var creditLimitId = _dictCodeTypeBLL.GetEntitiesByExpression("Code=DefaultCreditLimit").First().DictCodes.First().Value;
            UserAccount userAccount = new UserAccount();
            userAccount.Id = Guid.NewGuid();
            userAccount.UserId = user.Id;
            userAccount.CreditLimitId = new Guid(creditLimitId);
            userAccount.RealCurrency = 0d;
            userAccount.VirtualCurrency = 0d;
            _userAccountBLL.Add(new UserAccount[] { userAccount }, ref tran, true);
            var subject = new Subject();
            subject.Id = Guid.NewGuid();
            subject.Name = user.UserName + "课题组";
            subject.SubjectDirectorId = user.Id;
            subject.CreateAt = DateTime.Now;
            subject.Status = 0;
            subject.TotalSum = 0d;
            subject.OddSum = 0d;
            _subjectBLL.Add(new Subject[] { subject }, ref tran, true);
            userAccountTmep = userAccount;
            return subject.Id;
        }
    }
}
