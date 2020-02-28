using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class DoorContinueAuthorizeMessageManager: GlobalMessageManager
    {
        public DoorContinueAuthorizeMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new DoorContinueAuthorizeMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as  UserDoorContinuedAuthorization,this._businessObjects[1] as UserDoorContinuedAuthorization, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new DoorContinueAuthorizeMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as UserDoorContinuedAuthorization,this._businessObjects[1] as UserDoorContinuedAuthorization, _sender);
        }
    }
}
