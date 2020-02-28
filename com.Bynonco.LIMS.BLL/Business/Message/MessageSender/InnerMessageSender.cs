using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Business;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;

namespace com.Bynonco.LIMS.BLL
{
    public class InnerMessageSender : MessageSenderBase
    {
        private IMessageBLL __messageBLL;
        private IReceiverBLL __receiverBLL;
        public InnerMessageSender()
        {
            __messageBLL = BLLFactory.CreateMessageBLL();
            __receiverBLL = BLLFactory.CreateReceiverBLL();
        }
        protected override void Send(MessageTemplateBase messageTemplate)
        {
            if (CustomerFactory.GetCustomer().GetIsSendMessageToTutorSameTime())
            {
                SendSeparately(messageTemplate);
                return;
            }
            string errorMessage = "";
            if (messageTemplate.MessageContext == null || messageTemplate.MessageContext.Receivers == null || messageTemplate.MessageContext.Receivers.Count() == 0) return;
            var tran = SessionManager.Instance.BeginTransaction();
            try
            {
                Message message = new Message() { Id = Guid.NewGuid(), IsDeleted = false };
                message.Subject = messageTemplate.MessageContext.Title;
                message.ContentHTML = messageTemplate.MessageContext.Body;
                message.ContentNoHTML = messageTemplate.MessageContext.Body;
                message.MsgType = (int)messageTemplate.MessageContext.MsgType;
                message.SendAt = DateTime.Now;
                if (messageTemplate.MessageContext.Sender != null) 
                    message.SenderId = messageTemplate.MessageContext.Sender.Id;
                message.BusinessId = messageTemplate.MessageContext.BusinessId;
                __messageBLL.Add(new Message[] { message }, ref tran, true);
                foreach (var receiver in messageTemplate.MessageContext.Receivers)
                {
                    bool isReceive = true;
                    IMessageReceiveSettingManager messageReceiveSettingManager = new MessageReceiveSettingManager(receiver, receiver);
                    if (!messageReceiveSettingManager.JudgeIsNeedNotice(messageTemplate.MessageContext.MsgType, MessageSender.InnerMessageSender, out errorMessage, out isReceive)) continue;
                    messageTemplate.MessageContext.CurReceiver = receiver;
                    Receiver msgReceiver = new Receiver()
                    {
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        MessageId = message.Id,
                        UserId = receiver.Id,
                        HasReaded = false
                    };
                    __receiverBLL.Add(new Receiver[] { msgReceiver }, ref tran, true);
                }
                tran.CommitTransaction();
            }
            catch (Exception ex) { tran.RollbackTransaction(); }
            finally { tran.Dispose(); }
        }
        protected override void SendSeparately(MessageTemplateBase messageTemplate)
        {
            string errorMessage = "";
            if (messageTemplate.MessageContext == null || messageTemplate.MessageContext.Receivers == null || messageTemplate.MessageContext.Receivers.Count() == 0) return;
            var tran = SessionManager.Instance.BeginTransaction();
            try
            {
                foreach (var receiver in messageTemplate.MessageContext.Receivers)
                {
                    bool isReceive = true;
                    Message message = new Message() { Id = Guid.NewGuid(), IsDeleted = false };
                    if (messageTemplate.MessageContext.Sender != null) message.SenderId = messageTemplate.MessageContext.Sender.Id;
                    message.Subject = messageTemplate.MessageContext.Title;
                    messageTemplate.MessageContext.CurReceiver = receiver;
                    message.ContentHTML = messageTemplate.MessageContext.Body;
                    message.ContentNoHTML = messageTemplate.MessageContext.Body;
                    message.MsgType = (int)messageTemplate.MessageContext.MsgType;
                    message.SendAt = DateTime.Now;
                    message.BusinessId = messageTemplate.MessageContext.BusinessId;
                    __messageBLL.Add(new Message[] { message }, ref tran, true);
                    IMessageReceiveSettingManager messageReceiveSettingManager = new MessageReceiveSettingManager(receiver, receiver);
                    if (!messageReceiveSettingManager.JudgeIsNeedNotice(messageTemplate.MessageContext.MsgType, MessageSender.InnerMessageSender, out errorMessage, out isReceive)) continue;
                    Receiver msgReceiver = new Receiver()
                    {
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        MessageId = message.Id,
                        UserId = receiver.Id,
                        HasReaded = false
                    };
                    __receiverBLL.Add(new Receiver[] { msgReceiver }, ref tran, true);
                }
                tran.CommitTransaction();
            }
            catch (Exception ex) { tran.RollbackTransaction(); }
            finally { tran.Dispose(); }
        }
    }
}
