using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPAppointmentAuditMessageTemplate : MessageTemplateBase
    {
        private ViewNMPAppointment __viewNMPAppointment;
        public NMPAppointmentAuditMessageTemplate(MessageTemplateType messageTemplateType, ViewNMPAppointment viewNMPAppointment, User sender)
            : base(messageTemplateType, sender)
        {
            this.__viewNMPAppointment = viewNMPAppointment; 
        }
        protected override MessageContext GenerateMessageContext()
        {
            string content = "";
            string title = "";
            if (__viewNMPAppointment.NMPAppointment.AuditStatusEnum == Model.Enum.AppointmentAuditStatus.Pass)
            {
                content = string.Format("$ReceiverInfo$于{0}预约工位设备【{1}】从【{2}】到【{3}】已经审核通过,请准时赴约！！！系统邮件,毋需回复.如有疑问请与设备管理员联系.",
                    __viewNMPAppointment.ApplyTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    __viewNMPAppointment.NMPEquipmentName,
                    __viewNMPAppointment.BeginTimeStr,
                    __viewNMPAppointment.EndTimeStr
                    );
                title = "工位预约审核通过通知";

            }
            else
            {
                content = string.Format("$ReceiverInfo$于{0}预约设备【{1}】从【{2}】到【{3}】审核不通过,原因:{4},有相关疑问请咨询管理员！！！系统邮件,毋需回复.如有疑问请与设备管理员联系.",
                    __viewNMPAppointment.ApplyTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    __viewNMPAppointment.NMPEquipmentName,
                    __viewNMPAppointment.BeginTimeStr,
                    __viewNMPAppointment.EndTimeStr,
                    string.IsNullOrWhiteSpace(__viewNMPAppointment.AuditingNoPassWhy) ? "" : __viewNMPAppointment.AuditingNoPassWhy
                    );
                title = "警告,工位预约审核不通过通知";
            }

            return new MessageContext(Model.Enum.MsgType.NMPAppointmentAudit, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender, __viewNMPAppointment.NMPAppointment.User, new User[] { __viewNMPAppointment.NMPAppointment.User }, __viewNMPAppointment.NMPAppointment.User, __viewNMPAppointment.Id, __viewNMPAppointment.EndTime);
        }
    }
}
