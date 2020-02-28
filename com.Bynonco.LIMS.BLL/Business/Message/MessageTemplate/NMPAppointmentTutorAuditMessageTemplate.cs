using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPAppointmentTutorAuditMessageTemplate: MessageTemplateBase
    {
        private NMPAppointment __appointment; 
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        public NMPAppointmentTutorAuditMessageTemplate(MessageTemplateType messageTemplateType, NMPAppointment appointment, User sender)
            : base(messageTemplateType, sender)
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __appointment = appointment;
        }
        protected override MessageContext GenerateMessageContext()
        {
            var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("IsDelete=false*IsStop=false*Code=CustomerName");
            var customerName = dictCodeType != null && !string.IsNullOrWhiteSpace(dictCodeType.Value) ? dictCodeType.Value : "";
            customerName = string.Format("{0}《大型仪器管理系统》", customerName);
            var title = string.Format("您的学生的【{0}】预约工位设备【{1}】【{2}】申请单需要您进行审核", __appointment.User != null ? __appointment.User.UserName : "", __appointment.NMPEquipment.Name, __appointment.BeginTimeStr + "～" + __appointment.EndTimeStr);
            string content = string.Format("申请单详细信息：工位设备:{0}<br />预约时间:{1}<br />预估收费价格:{2}<br />", __appointment.NMPEquipment.Name, __appointment.BeginTimeStr + "～" + __appointment.EndTimeStr, __appointment.Fee.ToString("0.00"));
            if (_messageTemplateType == MessageTemplateType.HTML)
            {
                content = content + "请您审核确认?请点击链接1同意。<br />";
                string tutorAuditPassURL = (string.Format("<a href=\"{0}\" style=\"text-decoration:underline;color:green;\" onclick=\"return confirm('确定同意吗?');\">点击这里同意</a>", URLConverterUtility.ToMVCUrl("NMPAppointment", "TutorAuditPass") + "?tutorId=" + __appointment.User.Tutor.Id.ToString() + "&appointmentId=" + __appointment.Id));
                string tutorAuditNoPassURL = (string.Format("<a href=\"{0}\" style=\"text-decoration:underline;color:red;padding-bottom:5px;\" onclick=\"return confirm('确定否决吗?');\">点击这里否决</a>", URLConverterUtility.ToMVCUrl("NMPAppointment", "TutorAuditNotPass") + "?tutorId=" + __appointment.User.Tutor.Id.ToString() + "&appointmentId=" + __appointment.Id));
                content = content + "链接1：" + tutorAuditPassURL + "<br />";
                content = content + "如果不是，请点击链接2否决。" + "<br />";//\r\n
                content = content + "链接2：" + tutorAuditNoPassURL + "<br />";
            }
            else
            {
                content = content + "请登录系统或者邮箱进行审核。<br />";
                content.Replace("<br />", "\r\n");
            }
            content = content + "该信息无需回复，感谢您的支持，谢谢！\r\n " + customerName + ".";
            return new MessageContext(Model.Enum.MsgType.NMPAppointmentTutorAudit, __appointment.User.Tutor.Email, __appointment.User.Tutor.PhoneNumber, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender, __appointment.User.Tutor, new User[] { __appointment.User.Tutor }, null, __appointment.Id, null);
        }
    }
}
