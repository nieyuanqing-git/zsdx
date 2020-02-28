using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.august.DataLink;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Business;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class PhoneMessageSender:MessageSenderBase
    {
         private ISendShortMailBLL __sendShortMailBLL;
         public PhoneMessageSender()
        {
            __sendShortMailBLL = BLLFactory.CreateSendShortMailBLL();
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
                    messageReceiveSettingManager.JudgeIsNeedNotice(messageTemplate.MessageContext.MsgType, MessageSender.ShortMessageSender, out errorMessage, out isReceive);
                    if (!isReceive) continue;
                    messageTemplate.MessageContext.CurReceiver = receiver;
                    SendShortMail sendShortMail = new SendShortMail() { Id = Guid.NewGuid() };
                    sendShortMail.Title = messageTemplate.MessageContext.Title;
                    sendShortMail.Body = messageTemplate.MessageContext.Body;
                    sendShortMail.MobilePhoneNo = string.IsNullOrWhiteSpace(messageTemplate.MessageContext.MobilePhoneNo) ? receiver.PhoneNumber : messageTemplate.MessageContext.MobilePhoneNo;
                    sendShortMail.BusinessId = messageTemplate.MessageContext.BusinessId;
                    sendShortMail.Remark = "";
                    sendShortMail.SendDate = DateTime.Now;
                    sendShortMail.ReceiverId = receiver.Id;
                    sendShortMail.MsgType = (int)messageTemplate.MessageContext.MsgType;
                    sendShortMail.ValidateTime = messageTemplate.MessageContext.ValidateTime;
                    if (messageTemplate.MessageContext.Sender != null)
                        sendShortMail.SendOperaterId = messageTemplate.MessageContext.Sender.Id;
                    __sendShortMailBLL.Add(new SendShortMail[] { sendShortMail }, ref tran, true);
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
