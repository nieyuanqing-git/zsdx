using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleMessageManager : MessageManagerBase
    {
        private IDictCodeTypeBLL _dictCodeTypeBLL;
        private SampleApply __sampleApply;
        private string __applicantName;
        private string __address;
        private string __title;
        private string __content;
        public SampleMessageManager(object[] businessObjs, User sender)
            : base(businessObjs, sender)
        {
            _dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __sampleApply = businessObjs[0] as SampleApply;
            __applicantName = businessObjs[1].ToString();
            __address = businessObjs[2] == null ? null : businessObjs[2].ToString();
            __title = businessObjs[3] == null ? null : businessObjs[3].ToString();
            __content = businessObjs[4] == null ? null : businessObjs[4].ToString();
        }
        private SendMode SendMode
        {
            get
            {
                var dictCodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=SampleMessageNoticeSendMode");
                if (dictCodeTypes != null && dictCodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(dictCodeTypes.First().Value))
                {
                    return (SendMode)System.Enum.ToObject(typeof(SendMode), int.Parse(dictCodeTypes.First().Value.Trim()));
                }
                return Model.Enum.SendMode.None;
            }
        }
        private bool IsNeedSendMessage
        {
            get
            {
                var dictCodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=WhichSampleStatusSendMessage");
                if (dictCodeTypes != null && dictCodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(dictCodeTypes.First().Value))
                {
                    var statuses = dictCodeTypes.First().Value.Trim().Replace("，", ",").Split(',');
                    return statuses.Contains(__sampleApply.Status.ToString());
                }
                return true;
            }
        }

        protected override IList<MessageSenderBase> GenerateMessageSenders()
        {
            if (!IsNeedSendMessage) return null;
            var sampleApply = this._businessObjects[0] as SampleApply;
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
            return new SampleMessageTemplate(MessageTemplateType.HTML, this._businessObjects[0] as SampleApply, _sender, __address, __title, __content);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new SampleMessageTemplate(MessageTemplateType.NoHTML, this._businessObjects[0] as SampleApply, _sender, __address, __title, __content);
        }
    }
}
