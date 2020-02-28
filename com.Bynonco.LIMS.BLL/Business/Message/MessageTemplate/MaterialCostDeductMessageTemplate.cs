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
    public class MaterialCostDeductMessageTemplate : MessageTemplateBase
    {
        private MaterialOutput __materialOutput;
        public MaterialCostDeductMessageTemplate(MessageTemplateType messageTemplateType, MaterialOutput materialOutput, User sender)
            : base(messageTemplateType, sender)
        {
            this.__materialOutput = materialOutput;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string title = string.Format("$ReceiverInfo$发生耗材扣费:{0}", __materialOutput.MaterialOutputItemList != null && __materialOutput.MaterialOutputItemList.Count > 0 ? string.Join(",", __materialOutput.MaterialOutputItemList.Select(p => p.MaterialInfo.Name)) : "");
            string content = string.Format("耗材项目:{0},扣费:{1}元,扣费时间:{2},扣费备注:{3}", __materialOutput.MaterialOutputItemList != null && __materialOutput.MaterialOutputItemList.Count > 0 ? string.Join(",", __materialOutput.MaterialOutputItemList.Select(p => p.MaterialInfo.Name)) : "", __materialOutput.Money, __materialOutput.DeductTime.Value.ToString("yyyy-MM-dd HH:mm"), __materialOutput.Remark);
            return new MessageContext(Model.Enum.MsgType.DeductInform, "", "", title, content, null, _sender, __materialOutput.OutputUser, new User[] { __materialOutput.OutputUser }, __materialOutput.OutputUser, __materialOutput.Id, null);
        }
    }
}
