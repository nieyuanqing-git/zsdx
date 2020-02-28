using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL
{
    public class DelinquencyConfirmMessageTemplate : MessageTemplateBase
    {
        private ViewDelinquencyConfirm __viewDelinquencyConfirm;
        private string __address;
        private string __title;
        private string __content;
        public DelinquencyConfirmMessageTemplate(MessageTemplateType messageTemplateType, ViewDelinquencyConfirm viewDelinquencyConfirm, User sender, string address, string title, string content)
            : base(messageTemplateType, sender)
        {
            this.__viewDelinquencyConfirm = viewDelinquencyConfirm;
            this.__address = address;
            this.__title = title;
            this.__content = content;
        }
        public override void RenderSender()
        {
            string sender = "不良行为通知";
            __defaultMessageTemplate.Replace("{sender}", sender);
        }
        protected override MessageContext GenerateMessageContext()
        {
            var title = __title;
            string content = !string.IsNullOrWhiteSpace(__content) ? __content : __viewDelinquencyConfirm.Cause;
            return new MessageContext(Model.Enum.MsgType.DelinquencyConfirm, __address, "", title, content, null, _sender, __viewDelinquencyConfirm.User, new User[] { __viewDelinquencyConfirm.User }, __viewDelinquencyConfirm.User, __viewDelinquencyConfirm.id, null);
        }
    }
}
