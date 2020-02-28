using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class RepaelPunishMessageManager: GlobalMessageManager
    {
        public RepaelPunishMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new RepealBadBehaviorMessageTemplate( Model.Enum.MessageTemplateType.HTML ,this._businessObjects[0] as RepealDelinquency,_sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new RepealBadBehaviorMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as RepealDelinquency, _sender);
        }
    }
}
