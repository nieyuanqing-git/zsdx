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
    public class UsedConfirmCostDeductMessageTemplate : MessageTemplateBase
    {
        private UsedConfirm __usedConfirm;
        public UsedConfirmCostDeductMessageTemplate(MessageTemplateType messageTemplateType, UsedConfirm usedConfirm, User sender)
            : base(messageTemplateType, sender)
        {
            this.__usedConfirm = usedConfirm;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string title = string.Format("$ReceiverInfo$发生{0}使用扣费", __usedConfirm.Equipment.Name);
            string content = string.Format("{0}使用扣费,使用时间:{1}~{2},扣费:{3}元", __usedConfirm.Equipment.Name, __usedConfirm.BeginAt.Value.ToString("yyyy-MM-dd HH:mm:ss"), __usedConfirm.EndAt.Value.ToString("yyyy-MM-dd HH:mm:ss"), __usedConfirm.CostDeduct.RealCurrency + __usedConfirm.CostDeduct.VirtualCurrency);
            return new MessageContext(Model.Enum.MsgType.DeductInform, "", "", title, content, null, _sender, __usedConfirm.User, new User[] { __usedConfirm.User }, __usedConfirm.User, __usedConfirm.Id, null);
        }
    }
}
