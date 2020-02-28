using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentAuthorizeMessageManager: GlobalMessageManager
    {
        public EquipmentAuthorizeMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new EquipmentAuthorizeMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as UserEquipmentAuthorization, this._businessObjects[1] as UserEquipmentAuthorization, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new EquipmentAuthorizeMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as UserEquipmentAuthorization, this._businessObjects[1] as UserEquipmentAuthorization, _sender);
        }
    }
}
