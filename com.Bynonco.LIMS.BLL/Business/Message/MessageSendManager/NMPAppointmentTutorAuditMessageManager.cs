using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPAppointmentTutorAuditMessageManager : GlobalMessageManager
    {
        public NMPAppointmentTutorAuditMessageManager(object[] businessObjs, User sender)
            : base(businessObjs, sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new NMPAppointmentTutorAuditMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as NMPAppointment, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new NMPAppointmentTutorAuditMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as NMPAppointment, _sender);
        }
    }
}
