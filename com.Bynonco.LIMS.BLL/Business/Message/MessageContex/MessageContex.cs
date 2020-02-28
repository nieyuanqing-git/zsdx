using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;

namespace com.Bynonco.LIMS.BLL
{
    public delegate void InitCurReceiverHandler();
    public class MessageContext
    {
        private string __title = "";
        private string __body = "";
        private MessageContext(MsgType msgType, string title, string body, string sendDate, User sender, User targetUser,IEnumerable<User> receivers, User curReceiver, Guid? businessId,DateTime? validateTime)
        {
            this.__title = title;
            this.__body = body;
            this.SendDate = sendDate;
            this.Sender = sender;
            this.MsgType = msgType;
            this.CurReceiver = curReceiver;
            this.BusinessId = businessId;
            this.ValidateTime = validateTime;
            if (receivers != null && receivers.Count() > 0)
            {
                var isSendMessageToTutorSameTime = CustomerFactory.GetCustomer().GetIsSendMessageToTutorSameTime();
                IList<User> lstReceiver =  new List<User>();
                foreach (var receiver in receivers)
                {
                    if (!lstReceiver.Any(p => p.Id == receiver.Id))
                    {
                        lstReceiver.Add(receiver);
                    }
                    if (isSendMessageToTutorSameTime && receiver.Tutor != null && !lstReceiver.Any(p => p.Id == receiver.Tutor.Id))
                    {
                        lstReceiver.Add(receiver.Tutor);
                    }
                }
                Receivers = lstReceiver;
            }
            this.TargetUser = targetUser;
        }
        public MessageContext(MsgType msgType, string emailAddress, string mobilPhoneNo, string title, string body, string sendDate, User sender, User targetUser,IEnumerable<User> receivers, User curReceiver, Guid? businessId,DateTime? validateTime)
            : this(msgType, title, body, sendDate, sender, targetUser, receivers, curReceiver, businessId, validateTime)
        {
            this.EmailAddress = emailAddress;
            this.MobilePhoneNo = mobilPhoneNo;
        }
        public string MobilePhoneNo { get; private set; }
        public string EmailAddress { get; private set; }
        public string Title 
        {
            get 
            {
                return __title.Replace("$ReceiverInfo$", GenerateReceiverInfo());
            }
        }
        public string Body 
        {
            get { return __body.Replace("$ReceiverInfo$", GenerateReceiverInfo()); }
            set { __body = value; }
        }
        public string SendDate { get; private set; }
        public DateTime? ValidateTime { get; private set; }
        public User Sender { get; private set; }
        public MsgType MsgType { get; private set; }
        private User __curReveiver;
        public Guid? BusinessId { get; private set; }
        public IEnumerable<User> Receivers { get; private set; }
        public event InitCurReceiverHandler InitCurReceiverHandler;
        public bool IsInitCurReceiverHandlerHasValue
        {
            get { return InitCurReceiverHandler != null; }
        }
        public User CurReceiver
        {
            get { return __curReveiver; }
            set
            {
                __curReveiver = value;
                if (InitCurReceiverHandler != null) InitCurReceiverHandler();
            }
        }
        public User TargetUser { get; private set; }
        private string GenerateReceiverInfo()
        {
            string str = "您";
            if (TargetUser != null && CurReceiver != null &&
                TargetUser.Id != CurReceiver.Id && TargetUser.Tutor != null &&
                TargetUser.Tutor.Id == CurReceiver.Id)
            {
                str = "您的学生" + TargetUser.UserName;
            }
            return str;
        }
    }
}
