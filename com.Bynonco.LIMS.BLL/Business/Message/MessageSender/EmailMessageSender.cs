using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Business;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class EmailMessageSender:MessageSenderBase
    {
        private ISendMailBLL __sendMailBLL;
        public EmailMessageSender()
        {
            __sendMailBLL = BLLFactory.CreateSendMailBLL();
        }
        protected override void Send(MessageTemplateBase messageTemplate)
        {
            string errorMessage = "";
            if (messageTemplate.MessageContext == null || messageTemplate.MessageContext.Receivers == null || messageTemplate.MessageContext.Receivers.Count() == 0) return;
            var tran = SessionManager.Instance.BeginTransaction();
            try
            {
                foreach (var receiver in messageTemplate.MessageContext.Receivers)
                {
                    bool isReceive = true;
                    IMessageReceiveSettingManager messageReceiveSettingManager = new MessageReceiveSettingManager(receiver, receiver);
                    messageReceiveSettingManager.JudgeIsNeedNotice(messageTemplate.MessageContext.MsgType, MessageSender.EmailMessageSender, out errorMessage,out isReceive);
                    if (!isReceive) continue;
                    messageTemplate.MessageContext.CurReceiver = receiver;
                    SendMail sendMail = new SendMail() { Id = Guid.NewGuid() };
                    sendMail.Title = messageTemplate.MessageContext.Title;
                    sendMail.Body = messageTemplate.MessageContext.Body;
                    sendMail.Address = string.IsNullOrWhiteSpace(messageTemplate.MessageContext.EmailAddress) ? receiver.Email : messageTemplate.MessageContext.EmailAddress;
                    sendMail.BusinessId = messageTemplate.MessageContext.BusinessId;
                    sendMail.Remark = "";
                    sendMail.SendDate = DateTime.Now;
                    sendMail.ReceiverId = receiver.Id;
                    sendMail.MsgType = (int)messageTemplate.MessageContext.MsgType;
                    sendMail.ValidateTime = messageTemplate.MessageContext.ValidateTime;
                    if (messageTemplate.MessageContext.Sender != null)
                        sendMail.SendOperaterId = messageTemplate.MessageContext.Sender.Id;
                    __sendMailBLL.Add(new SendMail[] { sendMail }, ref tran, true);
                }
                tran.CommitTransaction();
            }
            catch (Exception ex) { tran.RollbackTransaction(); }
            finally { tran.Dispose(); }
        }
        protected override void SendSeparately(MessageTemplateBase messageTemplate)
        {
            Send(messageTemplate);
        }
    }
}
