using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class BadBehaviorMessageTemplate : MessageTemplateBase
    {
        private PunishAction __punishAction;
        public BadBehaviorMessageTemplate(MessageTemplateType messageTemplateType, PunishAction punishAction, User sender)
            : base(messageTemplateType, sender)
        {
            this.__punishAction = punishAction;
        }
        protected override MessageContext GenerateMessageContext()
        {
            IList<User> recievers = new List<User>{__punishAction.PunishRecord.Punisher};
            if (__punishAction.PunishTypeEnum == Model.Enum.PunishType.TutorWarning && __punishAction.PunishRecord.Punisher.Tutor!=null)
            {
                recievers.Add(__punishAction.PunishRecord.Punisher.Tutor);
            }

            return new MessageContext(Model.Enum.MsgType.PunishInform, null, null, "处罚通知", __punishAction.ContentStr, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender, __punishAction.PunishRecord.Punisher, recievers, null, __punishAction.Id, null);
        }
    }
}
