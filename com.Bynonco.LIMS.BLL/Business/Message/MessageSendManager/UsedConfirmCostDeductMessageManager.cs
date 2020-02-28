using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class UsedConfirmCostDeductMessageManager: GlobalMessageManager
    {
        public UsedConfirmCostDeductMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new UsedConfirmCostDeductMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as UsedConfirm, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new UsedConfirmCostDeductMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as UsedConfirm, _sender);
        }
    }
}
