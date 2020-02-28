using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class UseEquipmentOutOfTimeMessageManager:GlobalMessageManager
    {
        public UseEquipmentOutOfTimeMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new UseEquipmentOutOfTimeMessageTemplate(Model.Enum.MessageTemplateType.HTML,this._businessObjects[0] as Appointment, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new UseEquipmentOutOfTimeMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as Appointment, _sender);
        }
    }
}
