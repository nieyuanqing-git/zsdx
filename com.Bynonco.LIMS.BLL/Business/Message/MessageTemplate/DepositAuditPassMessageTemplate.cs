using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class DepositAuditPassMessageTemplate : MessageTemplateBase
    {
        private DepositRecord __depositRecord;
        public DepositAuditPassMessageTemplate(MessageTemplateType messageTemplateType, DepositRecord depositRecord, User sender)
            : base(messageTemplateType, sender)
        {
            this.__depositRecord = depositRecord;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string content = string.Format("$ReceiverInfo$的大型设备共享平台账户于{0}存款{1}元,审核人:{2},审核备注:{3},当前账户总额:{4},其中真实币:{5},虚拟币:{6}", __depositRecord.CheckTime.Value.ToString("yyyy-MM-dd"), __depositRecord.DepositSum, __depositRecord.Checker.UserName, __depositRecord.CheckRemark, __depositRecord.UserAccount.TotalCurrency, __depositRecord.UserAccount.RealCurrency, __depositRecord.UserAccount.VirtualCurrency);
            IList<User> recievers = new List<User> { __depositRecord.UserAccount.GetUser() };
            return new MessageContext(Model.Enum.MsgType.DepositAuditPass, null, null, Model.Enum.MsgType.DepositAuditPass.ToCnName(), content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,__depositRecord.UserAccount.GetUser(), recievers, null, __depositRecord.Id, null);
        }
    }
}
