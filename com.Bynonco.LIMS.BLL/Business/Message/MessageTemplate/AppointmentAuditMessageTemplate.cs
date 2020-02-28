using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentAuditMessageTemplate : MessageTemplateBase
    {
        private ViewAppointment __viewAppointment;
        public AppointmentAuditMessageTemplate(MessageTemplateType messageTemplateType, ViewAppointment viewAppointment, User sender)
            : base(messageTemplateType, sender)
        {
            this.__viewAppointment = viewAppointment;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string content = "";
            string title = "";
            if (__viewAppointment.Appointment.AuditStatusEnum == Model.Enum.AppointmentAuditStatus.Pass)
            {
                content = string.Format("$ReceiverInfo$于{0}预约设备【{1}】从【{2}】到【{3}】已经审核通过,请准时赴约！！！系统邮件,毋需回复.如有疑问请与设备管理员联系.",
                    __viewAppointment.ApplyTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    __viewAppointment.EquipmentName,
                    __viewAppointment.BeginTimeStr,
                    __viewAppointment.EndTimeStr
                    );
                title = "预约审核通过通知";

            }
            else
            {
                content = string.Format("$ReceiverInfo$于{0}预约设备【{1}】从【{2}】到【{3}】审核不通过,原因:{4},有相关疑问请咨询管理员！！！系统邮件,毋需回复.如有疑问请与设备管理员联系.",
                    __viewAppointment.ApplyTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    __viewAppointment.EquipmentName,
                    __viewAppointment.BeginTimeStr,
                    __viewAppointment.EndTimeStr,
                    string.IsNullOrWhiteSpace(__viewAppointment.AuditingNoPassWhy) ? "" : __viewAppointment.AuditingNoPassWhy
                    );
                title = "警告,预约审核不通过通知";
            }

            return new MessageContext(Model.Enum.MsgType.AppointmentAudit, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender, __viewAppointment.Appointment.User,new User[] { __viewAppointment.Appointment.User }, __viewAppointment.Appointment.User, __viewAppointment.Id, __viewAppointment.EndTime);
        }
    }
}
