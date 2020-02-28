using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class EqBlackListMessageManager : MessageManagerBase
    {
        public EqBlackListMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override IList<MessageSenderBase> GenerateMessageSenders()
        {
            var equipmentBlackList = this._businessObjects[0] as EquipmentBlackList;
            IList<MessageSenderBase> lstMessageSenders = new List<MessageSenderBase>();
            if (equipmentBlackList.IsEmailNotice) lstMessageSenders.Add(new EmailMessageSender());
            if (equipmentBlackList.IsInnerMessageNotice) lstMessageSenders.Add(new InnerMessageSender());
            if (equipmentBlackList.IsMobilePhoneNotice) lstMessageSenders.Add(new PhoneMessageSender());
            return lstMessageSenders;
        }

        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new EqBlackListMessageTemplate( Model.Enum.MessageTemplateType.HTML,this._businessObjects[0] as EquipmentBlackList,_sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new EqBlackListMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as EquipmentBlackList, _sender);
        }
    }
}
