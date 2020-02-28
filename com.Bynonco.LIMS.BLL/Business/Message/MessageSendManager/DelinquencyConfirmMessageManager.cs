using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class DelinquencyConfirmMessageManager : MessageManagerBase
    {
        private IDictCodeBLL _dictCodeBLL;
        private ViewDelinquencyConfirm __viewDelinquencyConfirm;
        private string __punisherName;
        private string __address;
        private string __title;
        private string __content;
        public DelinquencyConfirmMessageManager(object[] businessObjs, User sender)
            : base(businessObjs, sender)
        {
            _dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __viewDelinquencyConfirm = businessObjs[0] as ViewDelinquencyConfirm;
            __punisherName = businessObjs[1].ToString();
            __address = businessObjs[2] == null ? null : businessObjs[2].ToString();
            __title = businessObjs[3] == null ? null : businessObjs[3].ToString();
            __content = businessObjs[4] == null ? null : businessObjs[4].ToString();
        }
        private SendMode SendMode
        {
            get
            {
                string sendModeStr = _dictCodeBLL.GetDictCodeValueByCode("DelinquencyManage", "DelinquencyMessageNoticeSendMode").Trim().ToUpper();
                var sendMode = (SendMode)System.Enum.ToObject(typeof(SendMode), Convert.ToInt32(sendModeStr));
                if (sendMode!=null)
                {
                    return (SendMode)System.Enum.ToObject(typeof(SendMode), int.Parse(sendMode.ToString()));
                }
                return Model.Enum.SendMode.None;
            }
        }       

        protected override IList<MessageSenderBase> GenerateMessageSenders()
        {
            var viewDelinquencyConfirm = this._businessObjects[0] as ViewDelinquencyConfirm;
            IList<MessageSenderBase> lstMessageSenders = new List<MessageSenderBase>();
            switch (SendMode)
            {
                case Model.Enum.SendMode.All:
                    lstMessageSenders.Add(new InnerMessageSender());
                    lstMessageSenders.Add(new EmailMessageSender());
                    lstMessageSenders.Add(new PhoneMessageSender());
                    break;
                case Model.Enum.SendMode.Email:
                    lstMessageSenders.Add(new EmailMessageSender());
                    break;
                case Model.Enum.SendMode.EmailAndPhoneMessage:
                    lstMessageSenders.Add(new EmailMessageSender());
                    lstMessageSenders.Add(new PhoneMessageSender());
                    break;
                case Model.Enum.SendMode.Message:
                    lstMessageSenders.Add(new InnerMessageSender());
                    break;
                case Model.Enum.SendMode.MessageAndEmail:
                    lstMessageSenders.Add(new InnerMessageSender());
                    lstMessageSenders.Add(new EmailMessageSender());
                    break;
                case Model.Enum.SendMode.MessageAndPhoneMessage:
                    lstMessageSenders.Add(new InnerMessageSender());
                    lstMessageSenders.Add(new PhoneMessageSender());
                    break;
                case Model.Enum.SendMode.None:
                    break;
                case Model.Enum.SendMode.PhoneMessage:
                    lstMessageSenders.Add(new PhoneMessageSender());
                    break;
            }
            return lstMessageSenders;
        }

        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new DelinquencyConfirmMessageTemplate(MessageTemplateType.HTML, this._businessObjects[0] as ViewDelinquencyConfirm, _sender, __address, __title, __content);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new DelinquencyConfirmMessageTemplate(MessageTemplateType.NoHTML, this._businessObjects[0] as ViewDelinquencyConfirm, _sender, __address, __title, __content);
        }
    }
}
