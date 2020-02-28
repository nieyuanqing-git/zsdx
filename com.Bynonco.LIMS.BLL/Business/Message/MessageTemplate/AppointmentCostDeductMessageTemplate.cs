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
    public class AppointmentCostDeductMessageTemplate : MessageTemplateBase
    {
        private Appointment __appointment;
        public AppointmentCostDeductMessageTemplate(MessageTemplateType messageTemplateType, Appointment appointment, User sender)
            : base(messageTemplateType, sender)
        {
            this.__appointment = appointment;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string title = string.Format("$ReceiverInfo$发生{0}预约预扣费", __appointment.Equipment.Name);
            string content = string.Format("{0}预约预扣费,预约时间:{1}~{2},扣费:{3}元", __appointment.Equipment.Name, __appointment.BeginTimeStr, __appointment.EndTimeStr, __appointment.RealCurrency + __appointment.VirtualCurrency);
            return new MessageContext(Model.Enum.MsgType.DeductInform, "", "", title, content, null, _sender,__appointment.User, new User[] { __appointment.User }, __appointment.User, __appointment.Id, null);
        }
    }
}
