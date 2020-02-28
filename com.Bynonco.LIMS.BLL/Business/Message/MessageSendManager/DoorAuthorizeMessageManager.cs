using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class DoorAuthorizeMessageManager: GlobalMessageManager
    {
        public DoorAuthorizeMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new DoorAuthorizeMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as UserDoorAuthorization, this._businessObjects[1] as UserDoorAuthorization, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new DoorAuthorizeMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as UserDoorAuthorization, this._businessObjects[1] as UserDoorAuthorization, _sender);
        }
    }
}
