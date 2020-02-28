using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentAuditMessageManager : GlobalMessageManager
    {
        public AppointmentAuditMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new AppointmentAuditMessageTemplate(Model.Enum.MessageTemplateType.HTML,this._businessObjects[0] as ViewAppointment,_sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new AppointmentAuditMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as ViewAppointment, _sender);
        }
    }
}
