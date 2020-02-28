using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business.Message.MessageTemplate
{
   public  class UserAuditMessageTemplate : MessageTemplateBase
    {
    

       
       
        private User user;
        public UserAuditMessageTemplate(MessageTemplateType messageTemplateType, User sender, User user) : base(messageTemplateType, sender)
        {

            this.user = user;

        }
        protected override MessageContext GenerateMessageContext()
        {
            return new MessageContext(Model.Enum.MsgType.UserAudit, "", "", "", "", null, _sender, user, new User[] { user }, null, user.id, null);
        }
       
    }
}
