using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class UseEquipmentOutOfTimeMessageTemplate : MessageTemplateBase
    {
        private IEquipmentBLL __equipmentBLL;
        private Appointment __appointment;
        public UseEquipmentOutOfTimeMessageTemplate(MessageTemplateType messageTemplateType,Appointment appointment, User sender)
            : base(messageTemplateType, sender)
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __appointment = appointment;
        }
        protected override MessageContext GenerateMessageContext()
        {
            var equipment = __equipmentBLL.GetEntityById(__appointment.EquipmentId.Value);
            if (equipment == null) return null;
            if (equipment.Directors == null || equipment.Directors.Count == 0) return null;
            string title = string.Format(string.Format("{0}使用{1}超时了", __appointment.User.UserName, __appointment.Equipment.Name));
            string content = string.Format("{0},您好！{1}使用您负责的设备{2}超时了,系统邮件,毋需回复.如有疑问请与设备管理员联系.",
                string.Join(",", equipment.Directors.Select(p => p.User.UserName)),
                __appointment.User.UserName,
                equipment.Name);
            return new MessageContext(Model.Enum.MsgType.UseEquipmentOutOfTime, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), _sender,null, equipment.Directors.Select(p => p.User), null, __appointment.Id, null);
        }
    }
}
