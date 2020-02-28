using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Business;

namespace com.Bynonco.LIMS.BLL
{
    public class MailManager : IMailManager
    {
        private static IMessageNoticeSettingBLL __messageNoticeSettingBLL;
        private static ISendMailBLL __sendMailBLL;
        private static IMailManager __mailManager;
        private static IUserBLL __userBLL;
        private static object __lockObjCreate = new object();
        private static object __lockObjBusiness = new object();
        private int __maxFailTimes = 5;
        private int __sentCountPerTime = 10;
        private static MessageNoticeSetting __messageNoticeSetting;
        static MailManager()
        {
            __sendMailBLL = BLLFactory.CreateSendMailBLL();
            __messageNoticeSettingBLL = new MessageNoticeSettingBLL();
            __userBLL = BLLFactory.CreateUserBLL();
        }
        private MailManager(int maxFailTimes, int sentCountPerTime, MessageNoticeSetting messageNoticeSetting) 
        {
            __maxFailTimes = maxFailTimes;
            __sentCountPerTime = sentCountPerTime;
            if (messageNoticeSetting != null)
                __messageNoticeSetting = messageNoticeSetting;
        }
        public static IMailManager GetInstance(int maxFailTimes, int sentCountPerTime, MessageNoticeSetting messageNoticeSetting = null)
        {
            if (__mailManager == null)
            {
                lock (__lockObjCreate)
                {
                    if (messageNoticeSetting == null)
                        __messageNoticeSetting = __messageNoticeSettingBLL.GenerateMessageNoticeSetting();
                    __mailManager = new MailManager(maxFailTimes, sentCountPerTime, messageNoticeSetting);
                }
            }
            return __mailManager;
        }
        public void SendMail(Guid? operatorId,out int successCount, out int failCount, out string errorMessage)
        {
            lock (__lockObjBusiness)
            {
                successCount = 0;
                failCount = 0;
                errorMessage = "";
                bool isReceive = true;
                try
                {
                    var unSendMailList = __sendMailBLL.GetScopeEntitiesByExpression(string.Format("Status={0}*Address=-null*FailTimes<{1}*(ValidateTime=null+ValidateTime<\"{2}\")", (int)SendMailStatus.UnSend, __maxFailTimes + 1,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 1, __sentCountPerTime);//__maxFailTimes + 1是因为表达式的"<"相当于<=
                    if (unSendMailList == null && unSendMailList.Count == 0) return;
                    StringBuilder sbErrorMsg = new StringBuilder();
                    foreach (var unSendMail in unSendMailList)
                    {
                        XTransaction tran = null;
                        try
                        {
                            if (string.IsNullOrWhiteSpace(unSendMail.Address)) throw new Exception("邮箱地址为空");
                            if (!unSendMail.Address.Trim().IsEmail()) throw new Exception(string.Format("电子邮箱【{0}】非法", unSendMail.Address));
                            if (unSendMail.ReceiverId.HasValue)
                            {
                                var receiver = __userBLL.GetEntityById(unSendMail.ReceiverId.Value);
                                IMessageReceiveSettingManager messageReceiveSettingManager = new MessageReceiveSettingManager(receiver, receiver);
                                if (!messageReceiveSettingManager.JudgeIsNeedNotice((MsgType)unSendMail.MsgType, MessageSender.EmailMessageSender, out errorMessage, out isReceive)) continue;
                            }
                            string errorMessageSend = "";
                            bool result = EmailUtility.SendEmail(__messageNoticeSetting.IsUseSSL,
                                                   __messageNoticeSetting.SMTP,
                                                   __messageNoticeSetting.SMTPPort,
                                                   __messageNoticeSetting.LoginUserName,
                                                   __messageNoticeSetting.LoginPassword,
                                                   __messageNoticeSetting.LoginUserName,
                                                   unSendMail.Address,
                                                   unSendMail.Title,
                                                   unSendMail.Body,
                                                   out errorMessageSend);
                            if (!result) throw new Exception(errorMessageSend);
                            unSendMail.StatusEnum = Model.Enum.SendMailStatus.Finished;
                            unSendMail.SendOperaterId = operatorId;
                            __sendMailBLL.Save(new SendMail[] { unSendMail }, ref tran);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            sbErrorMsg.AppendFormat("编码为:{0},接受邮箱为:{1},主题为:{2}发送失败，原因:{3}",
                                                    unSendMail.Id,
                                                    unSendMail.Address,
                                                    unSendMail.Title,
                                                    ex.Message).AppendLine();
                            unSendMail.FailTimes++;
                            __sendMailBLL.Save(new SendMail[] { unSendMail }, ref tran);
                            failCount++;
                        }
                    }
                    errorMessage = sbErrorMsg.ToString();
                }
                catch (Exception ex) { errorMessage = ex.Message; }
            }
        }
    }
}
