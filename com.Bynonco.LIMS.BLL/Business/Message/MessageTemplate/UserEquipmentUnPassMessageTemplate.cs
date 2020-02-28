using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL
{
    public class UserEquipmentUnPassMessageTemplate : MessageTemplateBase
    {
        private UserEquipment __userEquipment;
        public UserEquipmentUnPassMessageTemplate(MessageTemplateType messageTemplateType, UserEquipment userEquipment, User sender)
            : base(messageTemplateType, sender)
        {
            this.__userEquipment = userEquipment;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string title = string.Format("-{0}", __userEquipment.Equipment.Name+"审核不通过通知");
            string content = string.Format("$ReceiverInfo$申请的仪器:{0},审核不通过", __userEquipment.Equipment.Name);
            return new MessageContext(Model.Enum.MsgType.UserEquipmentUnPass, "", "", title, content, null, _sender,__userEquipment.User , new User[] { __userEquipment.User }, __userEquipment.User, __userEquipment.Id, null);
        }
    }
}
