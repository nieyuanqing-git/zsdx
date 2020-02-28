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
    public class NMPUsedConfirmCostDeductMessageTemplate : MessageTemplateBase
    {
        private NMPUsedConfirm __nmpUsedConfirm;
        public NMPUsedConfirmCostDeductMessageTemplate(MessageTemplateType messageTemplateType, NMPUsedConfirm nmpUsedConfirm, User sender)
            : base(messageTemplateType, sender)
        {
            this.__nmpUsedConfirm = nmpUsedConfirm;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string title = string.Format("$ReceiverInfo$发生{0}工位使用扣费", __nmpUsedConfirm.NMPEquipment.Name);
            string content = string.Format("{0}使用扣费,使用时间:{1}~{2},扣费:{3}元", __nmpUsedConfirm.NMPEquipment.Name, __nmpUsedConfirm.BeginAt.Value.ToString("yyyy-MM-dd HH:mm:ss"), __nmpUsedConfirm.EndAt.Value.ToString("yyyy-MM-dd HH:mm:ss"), __nmpUsedConfirm.CostDeduct.RealCurrency + __nmpUsedConfirm.CostDeduct.VirtualCurrency);
            return new MessageContext(Model.Enum.MsgType.DeductInform, "", "", title, content, null, _sender, __nmpUsedConfirm.User, new User[] { __nmpUsedConfirm.User }, __nmpUsedConfirm.User, __nmpUsedConfirm.Id, null);
        }
    }
}
