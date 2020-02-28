using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class ExperimenterSubjectMessageTemplate : MessageTemplateBase
    {
        private ExperimenterSubject __experimenterSubject;
        public ExperimenterSubjectMessageTemplate(MessageTemplateType messageTemplateType, ExperimenterSubject experimenterSubject, User sender)
            : base(messageTemplateType, sender)
        {
            this.__experimenterSubject = experimenterSubject;
        }
        protected override MessageContext GenerateMessageContext()
        {
            IList<User> recievers = new List<User> { __experimenterSubject.Experimenter };
            string content;
            if (__experimenterSubject.IsUnUseable)
            {
                content = string.Format("$ReceiverInfo$参与的课题 {2} 经费余额 {0:##}元 低于不可使用额 {1},现在已不能通过 {2} 课题使用设备，请及时通知课题导师为您分配课题续费！系统邮件,毋需回复.如有疑问请与管理员联系.",
                                        __experimenterSubject.OddSum,
                                        __experimenterSubject.UnUseable,
                                        __experimenterSubject.GetSubject().Name);
            }
            else if (__experimenterSubject.IsUnAppointment)
            {
                content = string.Format("$ReceiverInfo$参与的课题 {2} 经费余额 {0:##}元 低于不可预约额 {1},现在已不能通过 {2} 课题预约设备，请及时通知课题导师为您分配课题续费！系统邮件,毋需回复.如有疑问请与管理员联系.",
                                        __experimenterSubject.OddSum,
                                        __experimenterSubject.Unappointment,
                                        __experimenterSubject.GetSubject().Name);
            }
            else if (__experimenterSubject.IsPreAlert)
            {
                content = string.Format("$ReceiverInfo$参与的课题 {0} 经费余额已不多，请及时通知课题导师为您分配课题续费！系统邮件,毋需回复.如有疑问请与设备处联系.", __experimenterSubject.GetSubject().Name); ;
            }
            else
            {
                return null;
            }
            return new MessageContext(Model.Enum.MsgType.ExperimenterSubjectFundsWarring, null, null, "存款预警", content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,__experimenterSubject.Experimenter , recievers, null, __experimenterSubject.Id, null);
        }
    }
}
