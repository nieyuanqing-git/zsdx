using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class DepositAuditPassMessageManager: GlobalMessageManager
    {
        public DepositAuditPassMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new DepositAuditPassMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as DepositRecord, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new DepositAuditPassMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as DepositRecord, _sender);
        }
    }
}
