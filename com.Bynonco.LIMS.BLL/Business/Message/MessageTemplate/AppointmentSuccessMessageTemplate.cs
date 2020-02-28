using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentSuccessMessageTemplate : MessageTemplateBase
    {
        private Appointment __appointment;
        public AppointmentSuccessMessageTemplate(MessageTemplateType messageTemplateType, Appointment appointment, User sender)
            : base(messageTemplateType, sender)
        {
            this.__appointment = appointment;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string content = "";
            string title = "";
            content = string.Format("$ReceiverInfo$于{0}预约设备【{1}】从【{2}】到【{3}】已预约成功,{4}系统邮件,毋需回复.如有疑问请与设备管理员联系.",
                    __appointment.ApplyTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    __appointment.Equipment.Name,
                    __appointment.BeginTimeStr,
                    __appointment.EndTimeStr,
                    __appointment.IsNeedAudit.HasValue && __appointment.IsNeedAudit.Value ? "等待管理员审核！" : "请准时赴约！"
                    );
            title = "预约成功通知";

            return new MessageContext(Model.Enum.MsgType.AppointmentSuccess, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,__appointment.User, new User[] { __appointment.User }, __appointment.User, __appointment.Id, __appointment.EndTime);
        }
    }
}
