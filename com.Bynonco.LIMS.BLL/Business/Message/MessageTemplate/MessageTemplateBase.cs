using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public abstract class MessageTemplateBase
    {
        protected IDictCodeTypeBLL _dictCodeTypeBLL;
        protected IUserBLL _userBLL;
        private object __lockObj = new object();
        protected User _sender;
        protected MessageTemplateType _messageTemplateType;
        protected MessageContext __messageContext;
        protected bool IsSupressRenderTitleHook { get; set; }
        protected bool IsSupressRenderBodyHook { get; set; }
        protected bool IsSupressRenderSendDateHook { get; set; }
        protected bool IsSupressRenderSenderHook { get; set; }
        protected bool IsSupressRenderReceiverHook { get; set; }
        protected string __defaultMessageTemplate = @"
                <html>
                <head>
                <title>{subject}</title>
                </head>
                <body>
                <table width='100%'>
                <tr><td>
                {username}您好：
                </td></tr>
                <tr><td>
                {content}
                </td></tr>
                <tr><td align='right'>
                {sender}
                </td></tr>
                <tr><td align='right'>
                {date}
                </td></tr>
                </table>
                </body>
                </html>
                ";
        public MessageTemplateBase(MessageTemplateType messageTemplateType, User sender)
        {
            _dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            this._sender = sender;
            this._messageTemplateType = messageTemplateType;
        }
        public MessageTemplateBase(MessageTemplateType messageTemplateType, string filePath, User sender)
            : this(messageTemplateType, sender)
        {
            if (File.Exists(filePath))
            {
                var streamReader = File.OpenText(filePath);
                __defaultMessageTemplate = streamReader.ReadToEnd();
            }
        }
        public virtual MessageContext MessageContext
        {
            get
            {
                lock (__lockObj)
                {
                    if (__messageContext == null)
                        __messageContext = GenerateMessageContext();
                    return __messageContext;
                }
            }
        }
        protected abstract MessageContext GenerateMessageContext();

        public virtual void RenderTitle()
        {
            __defaultMessageTemplate = __defaultMessageTemplate.Replace("{subject}", __messageContext.Title ?? "");
        }

        public virtual void RenderBody()
        {
            __defaultMessageTemplate = __defaultMessageTemplate.Replace("{content}", __messageContext.Body ?? "");
        }

        public virtual void RenderSendDate()
        {
            __defaultMessageTemplate = __defaultMessageTemplate.Replace("{date}", __messageContext.SendDate);
        }

        public virtual void RenderSender()
        {
            __defaultMessageTemplate = __defaultMessageTemplate.Replace("{sender}", __messageContext.Sender == null ? "系统管理员" : __messageContext.Sender.UserName);
        }

        public virtual void RenderReceiver()
        {
            var receiverName = "";
            if (__messageContext.CurReceiver == null && __messageContext.Receivers.Count() == 1)
            {
                receiverName = __messageContext.Receivers.First().UserName;
            }
            else if (__messageContext.CurReceiver != null)
            {
                receiverName = __messageContext.CurReceiver.UserName;
            }

            __defaultMessageTemplate = __defaultMessageTemplate.Replace("{username}", receiverName);

        }

        public virtual string RenderTemplate()
        {
            __messageContext = GenerateMessageContext();
            if (__messageContext != null)
            {
                if (!IsSupressRenderTitleHook) RenderTitle();
                if (!IsSupressRenderBodyHook) RenderBody();
                if (!IsSupressRenderSendDateHook) RenderSendDate();
                if (!IsSupressRenderSenderHook) RenderSender();
                if (!IsSupressRenderReceiverHook) RenderReceiver();
            }
            return __defaultMessageTemplate;
        }
    }
}
