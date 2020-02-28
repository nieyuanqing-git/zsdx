using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL
{
    public class WaterControlCostDeductMessageTemplate : MessageTemplateBase
    {
        private WaterControlCostDeduct __WaterControlCostDeduct;
        public WaterControlCostDeductMessageTemplate(MessageTemplateType messageTemplateType, WaterControlCostDeduct WaterControlCostDeduct, User sender)
            : base(messageTemplateType, sender)
        {
            this.__WaterControlCostDeduct = WaterControlCostDeduct;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string title = string.Format("发生纯水扣费:{0}", __WaterControlCostDeduct.Name);
            string content = string.Format("扣费名目:{0},扣费:{1}元,扣费时间:{2},扣费备注:{3}", __WaterControlCostDeduct.Name, __WaterControlCostDeduct.Money, __WaterControlCostDeduct.DeductTime.Value.ToString("yyyy-MM-dd HH:mm"), __WaterControlCostDeduct.Remark);
            return new MessageContext(Model.Enum.MsgType.DeductInform,"", "", title, content, null, _sender,__WaterControlCostDeduct.User, new User[] { __WaterControlCostDeduct.User }, __WaterControlCostDeduct.User, __WaterControlCostDeduct.Id, null);
        }
    }
}
