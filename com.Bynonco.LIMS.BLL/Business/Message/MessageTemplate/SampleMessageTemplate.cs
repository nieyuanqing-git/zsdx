using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleMessageTemplate : MessageTemplateBase
    {
        private SampleApply __sampleApply;
        private string __address;
        private string __title;
        private string __content;
        public SampleMessageTemplate(MessageTemplateType messageTemplateType, SampleApply sampleApply, User sender, string address, string title, string content)
            : base(messageTemplateType, sender)
        {
            this.__sampleApply = sampleApply;
            this.__address = address;
            this.__title = title;
            this.__content = content;
        }
        public override void RenderSender()
        {
            string sender = "";
            var dictCodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=SampleApplyReportTitle");
            if (dictCodeTypes != null && dictCodeTypes.Count > 0 && !string.IsNullOrEmpty(dictCodeTypes.First().Value))
                sender = dictCodeTypes.First().Value;
            __defaultMessageTemplate.Replace("{sender}", sender);
        }
        protected override MessageContext GenerateMessageContext()
        {
            var title = !string.IsNullOrWhiteSpace(__title) ? __title :
                string.Format("$ReceiverInfo$的送样委托申请单【{0}】截止到【{1}】的处理情况", __sampleApply.SampleNo, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            string content = !string.IsNullOrWhiteSpace(__content) ? __content : __sampleApply.Notice;
            return new MessageContext(Model.Enum.MsgType.SampleOutcomeInform, __address, __sampleApply.Applicant.PhoneNumber, title, content, null, _sender,__sampleApply.Applicant, new User[] { __sampleApply.Applicant }, __sampleApply.Applicant, __sampleApply.Id, null);
        }
    }
}
