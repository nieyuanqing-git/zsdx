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
    public class ManualCostDeductMessageTemplate : MessageTemplateBase
    {
        private ManualCostDeduct __manualCostDeduct;
        public ManualCostDeductMessageTemplate(MessageTemplateType messageTemplateType, ManualCostDeduct manualCostDeduct, User sender)
            : base(messageTemplateType, sender)
        {
            this.__manualCostDeduct = manualCostDeduct;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string title = string.Format("$ReceiverInfo$发生手工扣费:{0}", __manualCostDeduct.Name);
            string content = string.Format("扣费名目:{0},扣费:{1}元,扣费时间:{2},扣费备注:{3}", __manualCostDeduct.Name, __manualCostDeduct.Money, __manualCostDeduct.DeductTime.Value.ToString("yyyy-MM-dd HH:mm"), __manualCostDeduct.Remark);
            return new MessageContext(Model.Enum.MsgType.DeductInform,"", "", title, content, null, _sender,__manualCostDeduct.User, new User[] { __manualCostDeduct.User }, __manualCostDeduct.User, __manualCostDeduct.Id, null);
        }
    }
}
