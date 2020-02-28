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
   public  class UserEquipmentAuditMessageTemplate : MessageTemplateBase
    {
    

       
       
        private UserEquipment userEquipment;
        public UserEquipmentAuditMessageTemplate(MessageTemplateType messageTemplateType, User sender, UserEquipment userEquipment) : base(messageTemplateType, sender)
        {

            this.userEquipment = userEquipment;

        }
        protected override MessageContext GenerateMessageContext()
        {
            return new MessageContext(Model.Enum.MsgType.UserEquipmentAudit, "", "", "", "", null, _sender, userEquipment.User, new User[] { userEquipment.User }, null, userEquipment.id, null);
        }
       
    }
}
