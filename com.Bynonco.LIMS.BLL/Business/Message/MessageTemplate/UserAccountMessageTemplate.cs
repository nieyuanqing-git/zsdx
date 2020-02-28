using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class UserAccountMessageTemplate : MessageTemplateBase
    {
        private UserAccount __userAccount;
        public UserAccountMessageTemplate(MessageTemplateType messageTemplateType, UserAccount userAccount, User sender)
            : base(messageTemplateType, sender)
        {
            this.__userAccount = userAccount;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string content = "";
            if (__userAccount.IsUnUseable)
            {
                content = string.Format("您账户余额 {0:##}元 低于不可使用额 {1},现在已不可以使用设备，请及时充值！系统邮件,毋需回复.如有疑问请与设备管理员联系.", __userAccount.TotalCurrency, __userAccount.CreditLimit.UnUseable);
            }
            else if (__userAccount.IsUnAppointment)
            {
                content = string.Format("您账户余额 {0:##}元 低于不可预约额 {1},现在已不可以预约设备，请及时充值！系统邮件,毋需回复.如有疑问请与设备管理员联系.", __userAccount.TotalCurrency, __userAccount.CreditLimit.UnAppointment);
            }
            else if (__userAccount.IsPreAlert)
            {
                content = "您账户余额已不多，请及时充值！系统邮件,毋需回复.如有疑问请与设备管理员联系.";
            }
            else
            {
                return null;
            }
            IList<User> recievers = new List<User> { __userAccount.GetUser() };
            return new MessageContext(Model.Enum.MsgType.DepositAlert, null, null, "存款预警", content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,__userAccount.GetUser(), recievers, null, __userAccount.Id, null);
        }
    }
}
