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
    public class BindTutorMessageTemplate: MessageTemplateBase
    {
        private User __tutor;
        private User __student;
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        public BindTutorMessageTemplate(MessageTemplateType messageTemplateType, User tutor, User student, User sender)
            : base(messageTemplateType, sender)
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            this.__tutor = tutor;
            this.__student = student;
        }
        protected override MessageContext GenerateMessageContext()
        {
            var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("IsDelete=false*IsStop=false*Code=CustomerName");
            var customerName = dictCodeType != null && !string.IsNullOrWhiteSpace(dictCodeType.Value) ? dictCodeType.Value : "";
            var title = string.Format("{0}《大型仪器管理系统》用户注册审核申请", customerName);
            string content = __tutor == null ? "导师" : __tutor.UserName + "导师您好！<br />";
            content = content + "现有" + __student.UserName + ",性别：" + (__student.Sex.Value ? "男" : "女") + ",学生证号/工作证号:【" + __student.CertificateType.Name + ":" + __student.IdentityCardNo + "】,";
           
            if (_messageTemplateType == MessageTemplateType.HTML)
            {
                content = content + "申请注册成为《大型仪器管理系统》用户，请您审核确认此人是否是您的组员? 如果是，请点击链接1同意。<br />";
                string tutorAuditPassURL = (string.Format("<a href=\"{0}\" style=\"text-decoration:underline;color:green;\" onclick=\"return confirm('确定同意吗?');\">点击这里同意</a>", URLConverterUtility.ToMVCUrl("User", "TutorAuditPass") + "?studentId=" + __student.Id.ToString() + "&tutorId=" + __tutor.Id.ToString()));
                string tutorAuditNoPassURL = (string.Format("<a href=\"{0}\" style=\"text-decoration:underline;color:red;padding-bottom:5px;\" onclick=\"return confirm('确定拒绝吗?');\">点击这里拒绝</a>", URLConverterUtility.ToMVCUrl("User", "TutorAuditNotPass") + "?studentId=" + __student.Id.ToString() + "&tutorId=" + __tutor.Id.ToString()));
                content = content + "链接1：" + tutorAuditPassURL + "<br />";
                content = content + "如果不是，请点击链接2否决。" + "<br />";//\r\n
                content = content + "链接2：" + tutorAuditNoPassURL + "<br />";
            }
            else
            {
                content = content + "申请注册成为《大型仪器管理系统》用户，请您审核确认此人是否是您的组员? 如果是请登录系统或者邮箱进行审核。<br />";
                content.Replace("<br />", "\r\n");
            }
            content = content + "该邮件无需回复，感谢您的支持，谢谢！\r\n " + customerName + ".";
            return new MessageContext(Model.Enum.MsgType.BindTutorMessage, __tutor.Email, __tutor.PhoneNumber, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,__tutor, new User[] { __tutor }, null, __student.Id, null);
        }
    }
}
