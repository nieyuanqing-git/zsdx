using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class EqBlackListMessageTemplate : MessageTemplateBase
    {
        private IUserBLL __userBLL;
        private EquipmentBlackList __equipmentBlackList;
        public EqBlackListMessageTemplate(MessageTemplateType messageTemplateType, EquipmentBlackList equipmentBlackList, User sender)
            : base(messageTemplateType, sender)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            this.__equipmentBlackList = equipmentBlackList;
        }
        protected override MessageContext GenerateMessageContext()
        {
            var title = __equipmentBlackList.NoticeTitle.Replace("$equipment$", __equipmentBlackList.Equipment.Name);
            var content = __equipmentBlackList.NoticeContent.Replace("$equipment$", __equipmentBlackList.Equipment.Name)
                .Replace("$reason$", __equipmentBlackList.Reason ?? "");
            var receivers = __userBLL.GetUserList(__equipmentBlackList.LabelTypeEnum, __equipmentBlackList.LabelIds);
            var messageContext = new MessageContext(Model.Enum.MsgType.EquipmentBlackListInform, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), _sender, null,receivers, null, __equipmentBlackList.Id, null);
            if (!messageContext.IsInitCurReceiverHandlerHasValue)
            {
                messageContext.InitCurReceiverHandler += () =>
                {
                    var index = receivers.IndexOf(messageContext.CurReceiver);
                    var userName = messageContext.CurReceiver.UserName;
                    messageContext.Body = messageContext.Body.Replace(index > 0 ? receivers.ElementAt(index - 1).UserName : "$user$", userName);
                };
            }
            return messageContext;
        }
    }
}
