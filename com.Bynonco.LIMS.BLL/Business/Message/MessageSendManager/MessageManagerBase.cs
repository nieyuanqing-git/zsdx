using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using System.Collections;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public abstract class MessageManagerBase
    {
        protected IViewMessageReceiveBLL _viewMessageReceiveBLL;
        protected ISendMailBLL _sendMailBLL;
        protected ISendShortMailBLL _sendShortMailBLL;
        protected object[] _businessObjects;
        protected User _sender;
        protected IList<MessageSenderBase> _messageSenders ;

        public MessageManagerBase(object[] businessObjs,User sender)
        {
            _viewMessageReceiveBLL = BLLFactory.CreateViewMessageReceiveBLL();
            _sendMailBLL = BLLFactory.CreateSendMailBLL();
            _sendShortMailBLL = BLLFactory.CreateSendShortMailBLL();
            this._businessObjects = businessObjs;
            this._sender = sender;
        }
        protected bool JudgeHasNoticeRecently(string title, User receiver)
        {
            var date = DateTime.Now.Date.AddSeconds(-1).AddDays(-6);
            int count = 0;
            if (!string.IsNullOrWhiteSpace(receiver.PhoneNumber))
            {
                count = _sendShortMailBLL.CountModelListByExpression(string.Format("Title=\"{0}\"*MobilePhoneNo=\"{1}\"*SendDate>\"{2}\"", title, receiver.PhoneNumber.Trim(), date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (count > 0) return true;
            }
            if (!string.IsNullOrWhiteSpace(receiver.Email))
            {
                //count = _sendShortMailBLL.CountModelListByExpression(string.Format("Title=\"{0}\"*Address=\"{1}\"*SendDate>\"{2}\"", title, receiver.Email.Trim(), date.ToString("yyyy-MM-dd HH:mm:ss")));
                count = _sendMailBLL.CountModelListByExpression(string.Format("Title=\"{0}\"*Address=\"{1}\"*SendDate>\"{2}\"", title, receiver.Email.Trim(), date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (count > 0) return true;
            }
            count = _viewMessageReceiveBLL.CountModelListByExpression(string.Format("Subject=\"{0}\"*ReceiveUserId=\"{1}\"*SendAt>\"{2}\"", title, receiver.Id, date.ToString("yyyy-MM-dd HH:mm:ss")));
            if (count > 0) return true;

            return false;
        }
        protected virtual IList<MessageSenderBase> GenerateMessageSenders(SendMode sendMode)
        {
            IList<MessageSenderBase> messageSenders = new List<MessageSenderBase>();
            if (BitUtility.IsContain((int)SendMode.Email, (int)sendMode)) messageSenders.Add(new EmailMessageSender());
            if (BitUtility.IsContain((int)SendMode.Message, (int)sendMode)) messageSenders.Add(new InnerMessageSender());
            if (BitUtility.IsContain((int)SendMode.PhoneMessage, (int)sendMode)) messageSenders.Add(new PhoneMessageSender());
            return messageSenders;
        }
        protected abstract IList<MessageSenderBase> GenerateMessageSenders();
        protected abstract MessageTemplateBase  CreateMessageTemplate();
        protected abstract MessageTemplateBase CreateNoHTMLMessageTemplate();
        private MessageTemplateBase GenerateMessageTemplate(MessageSenderBase messageSender)
        {
            var messageTemplate = messageSender is PhoneMessageSender ? CreateNoHTMLMessageTemplate() : CreateMessageTemplate();
            if (messageTemplate == null || messageTemplate.MessageContext == null) return null;
            messageTemplate.RenderTemplate();
            return messageTemplate;
        }
        protected virtual bool SendMessage()
        {
            if (_messageSenders == null || _messageSenders.Count == 0) return false;
            foreach (var messageSender in _messageSenders)
            {
                MessageTemplateBase messageTempldate = GenerateMessageTemplate(messageSender);
                if (messageTempldate == null) continue;
                messageSender.SendMessage(messageTempldate);
            }
            return true;
        }
        protected virtual bool SendMessageSeparately()
        {
            if (_messageSenders == null || _messageSenders.Count == 0) return false;
           
            foreach (var messageSender in _messageSenders)
            {
                MessageTemplateBase messageTempldate = GenerateMessageTemplate(messageSender);
                if (messageTempldate == null) continue;
                messageSender.SendMessageSeparately(messageTempldate);
            }
            return true;
        }
        public virtual bool Send()
        {
            try
            {
                _messageSenders = GenerateMessageSenders();
                return SendMessage();
            }
            catch(ExecutionEngineException xc) { return false; }
        }
        public virtual bool Send(SendMode sendMode)
        {
            try
            {
                _messageSenders = GenerateMessageSenders(sendMode);
                return SendMessage();
            }
            catch { return false; }
        }
        public virtual bool SendSeparately()
        {
            try
            {
                _messageSenders = GenerateMessageSenders();
                return SendMessageSeparately();
            }
            catch { return false; }
        }
        public virtual bool SendSeparately(SendMode sendMode)
        {
            try
            {
                _messageSenders = GenerateMessageSenders(sendMode);
                return SendMessageSeparately();
            }
            catch { return false; }
        }
    }
}
