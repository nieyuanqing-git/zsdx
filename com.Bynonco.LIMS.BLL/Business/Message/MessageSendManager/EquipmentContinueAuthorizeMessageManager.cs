using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentContinueAuthorizeMessageManager: GlobalMessageManager
    {
        public EquipmentContinueAuthorizeMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new EquipmentContinueAuthorizeMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as UserEquipmentContinuedAuthorization, this._businessObjects[1] as UserEquipmentContinuedAuthorization, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new EquipmentContinueAuthorizeMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as UserEquipmentContinuedAuthorization, this._businessObjects[1] as UserEquipmentContinuedAuthorization, _sender);
        }
    }
}
