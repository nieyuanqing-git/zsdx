using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class WaterControlCostDeductMessageManager: GlobalMessageManager
    {
        public WaterControlCostDeductMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) { }
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            return new WaterControlCostDeductMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as WaterControlCostDeduct, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new WaterControlCostDeductMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as WaterControlCostDeduct, _sender);
        }
    }
}
