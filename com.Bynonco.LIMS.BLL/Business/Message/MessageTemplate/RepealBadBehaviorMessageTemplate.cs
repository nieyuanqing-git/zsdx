using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class RepealBadBehaviorMessageTemplate:MessageTemplateBase
    {
        private RepealDelinquency __repealDelinquency;
        public RepealBadBehaviorMessageTemplate(MessageTemplateType messageTemplateType, RepealDelinquency repealDelinquency, User sender)
            : base(messageTemplateType, sender)
        {
            this.__repealDelinquency = repealDelinquency;
        }
        protected override MessageContext GenerateMessageContext()
        {
            IList<User> recievers = new List<User>{__repealDelinquency.DelinquencyConfirm.PunishRecord.Punisher};
            return new MessageContext(Model.Enum.MsgType.RepealBadBehavior, null, null, "撤消不良行为处罚通知", __repealDelinquency.Content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,__repealDelinquency.DelinquencyConfirm.PunishRecord.Punisher, recievers, null, __repealDelinquency.Id, null);
        }
    }
}
