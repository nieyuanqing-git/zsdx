using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class BindTutorMessageManager : GlobalMessageManager
    {
        public BindTutorMessageManager(object[] businessObjs, User sender)
            : base(businessObjs, sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new BindTutorMessageTemplate( Model.Enum.MessageTemplateType.HTML,this._businessObjects[0] as User, this._businessObjects[1] as User, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new BindTutorMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as User, this._businessObjects[1] as User, _sender);
        }
    }
}
