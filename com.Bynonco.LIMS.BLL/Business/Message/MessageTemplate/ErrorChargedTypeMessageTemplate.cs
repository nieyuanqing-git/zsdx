using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
namespace com.Bynonco.LIMS.BLL
{
    public class ErrorChargedTypeMessageTemplate : MessageTemplateBase
    {
        private IEquipmentBLL __equipmentBLL;
        private UsedConfirm __usedConfirm;
        private IMessageBLL __messageBLL;
        public ErrorChargedTypeMessageTemplate(MessageTemplateType messageTemplateType, UsedConfirm usedConfirm, User sender)
            : base(messageTemplateType, sender)
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __messageBLL = BLLFactory.CreateMessageBLL();
            __usedConfirm = usedConfirm;
        }
        protected override MessageContext GenerateMessageContext()
        {
            if (__usedConfirm == null
                || __usedConfirm.ChargeTypeEnum != Model.Enum.ChargeType.BySampleCount
                || !__usedConfirm.EndAt.HasValue
                || __usedConfirm.CostDeduct == null
                || (__usedConfirm.SampleCount.HasValue && __usedConfirm.SampleCount.Value > 0)
                || __usedConfirm.Equipment.Directors == null
                || __usedConfirm.Equipment.Directors.Count == 0) return null;
            var count = __messageBLL.CountModelListByExpression(string.Format("BusinessId=\"{0}\"*ContentHTML∽\"扣费记录需纠正\"", __usedConfirm.Id));
            if (count > 0) return null;
            string title = string.Format(string.Format("{0}使用{1}从{2}~{3}的扣费记录需纠正", __usedConfirm.User.UserName, __usedConfirm.Equipment.Name, __usedConfirm.BeginAt.Value.ToString("yyyy-MM-dd HH:mm"), __usedConfirm.EndAt.Value.ToString("yyyy-MM-dd HH:mm")));
            string content = string.Format("{0}使用{1}从{2}~{3}的扣费记录需纠正,原因:{1}的扣费方式为{4},目前暂时是以{5}进行扣费,请进行纠正！",
                 __usedConfirm.User.UserName,
                 __usedConfirm.Equipment.Name,
                 __usedConfirm.BeginAt.Value.ToString("yyyy-MM-dd HH:mm"),
                 __usedConfirm.EndAt.Value.ToString("yyyy-MM-dd HH:mm"),
                 Model.Enum.ChargeType.BySampleCount.ToCnName(),
                 Model.Enum.ChargeType.ByHour.ToCnName());
            return new MessageContext(Model.Enum.MsgType.EquipmentManagement, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), _sender, null ,__usedConfirm.Equipment.Directors.Select(p => p.User), null, __usedConfirm.Id, null);
        }
    }
}
