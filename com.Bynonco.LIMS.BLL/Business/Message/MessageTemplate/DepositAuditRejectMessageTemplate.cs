using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class DepositAuditRejectMessageTemplate : MessageTemplateBase
    {
        private DepositRecord __depositRecord;
        public DepositAuditRejectMessageTemplate(MessageTemplateType messageTemplateType, DepositRecord depositRecord, User sender)
            : base(messageTemplateType, sender)
        {
            this.__depositRecord = depositRecord;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string content = string.Format("$ReceiverInfo$的大型设备共享平台账户申请存款{1}元不通过,审核人:{2},审核时间:{0}, 审核备注:{3}", __depositRecord.CheckTime.Value.ToString("yyyy-MM-dd"), __depositRecord.DepositSum, __depositRecord.Checker.UserName, __depositRecord.CheckRemark);
            IList<User> recievers = new List<User> { __depositRecord.UserAccount.GetUser() };
            return new MessageContext(Model.Enum.MsgType.DepositAuditReject, null, null, Model.Enum.MsgType.DepositAuditReject.ToCnName(), content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender, __depositRecord.UserAccount.GetUser(), recievers, null, __depositRecord.Id, null);
        }
    }
}
