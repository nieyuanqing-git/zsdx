using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleApplyTutorAuditMessageManager : GlobalMessageManager
    {
        public SampleApplyTutorAuditMessageManager(object[] businessObjs, User sender)
            : base(businessObjs, sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new SampleApplyTutorAuditMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as SampleApply, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new SampleApplyTutorAuditMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as SampleApply, _sender);
        }
    }
}
