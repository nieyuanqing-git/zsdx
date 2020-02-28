using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public abstract class MessageSenderBase
    {
        public virtual void SendMessage(MessageTemplateBase messageTemplate)
        {
            messageTemplate.RenderTemplate();
            Send(messageTemplate);
        }
        public virtual void SendMessageSeparately(MessageTemplateBase messageTemplate)
        {
            messageTemplate.RenderTemplate();
            SendSeparately(messageTemplate);
        }
        protected abstract void Send(MessageTemplateBase messageTemplate);
        protected abstract void SendSeparately(MessageTemplateBase messageTemplate);
    }
}
