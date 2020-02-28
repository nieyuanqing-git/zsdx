using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public abstract class GlobalMessageManager : MessageManagerBase
    {
        private IMessageNoticeSettingBLL __messageNoticeSettingBLL;
        protected MessageNoticeSetting _messageNoticeSetting;
        public GlobalMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) 
        {
            __messageNoticeSettingBLL = new MessageNoticeSettingBLL();
            _messageNoticeSetting = __messageNoticeSettingBLL.GenerateMessageNoticeSetting();
        }
        protected override IList<MessageSenderBase> GenerateMessageSenders()
        {
            return base.GenerateMessageSenders(_messageNoticeSetting.SendMode);
        }
        protected override abstract MessageTemplateBase CreateMessageTemplate();

        protected override abstract MessageTemplateBase CreateNoHTMLMessageTemplate();
    }
}
