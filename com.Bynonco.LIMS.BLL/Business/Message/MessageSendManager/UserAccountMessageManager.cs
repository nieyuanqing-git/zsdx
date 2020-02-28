using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class UserAccountMessageManager:GlobalMessageManager
    {
        public UserAccountMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }

        private bool __isHTMLTemplate = false;
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            __isHTMLTemplate = true;
            return new UserAccountMessageTemplate(MessageTemplateType.HTML,this._businessObjects[0] as UserAccount,_sender);
        }


        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new UserAccountMessageTemplate(MessageTemplateType.NoHTML, this._businessObjects[0] as UserAccount, _sender);
        }
        protected override bool SendMessage()
        {
            var userAccount = this._businessObjects[0] as UserAccount;
            if (userAccount != null)
            {
                UserAccountMessageTemplate userAccountMessageTemplate = new UserAccountMessageTemplate(__isHTMLTemplate ? MessageTemplateType.HTML : MessageTemplateType.NoHTML, userAccount, _sender);
                if (userAccountMessageTemplate == null) return false;
                if (userAccountMessageTemplate.MessageContext.Receivers != null && userAccountMessageTemplate.MessageContext.Receivers.Count() > 0)
                {
                    if (JudgeHasNoticeRecently(userAccountMessageTemplate.MessageContext.Title, userAccountMessageTemplate.MessageContext.Receivers.First()))
                    {
                        return true;
                    }
                }
                foreach (var messageSender in _messageSenders)
                    messageSender.SendMessage(userAccountMessageTemplate);
            }
            return true;
        }
       
    }
}
