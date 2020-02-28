using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class FindPasswordBLL : BLLBase<FindPassword>, IFindPasswordBLL
    {
        private IUserBLL __userBLL;
        private IDictCodeBLL __dictCodeBLL;
        private IMessageNoticeSettingBLL __messageNoticeSettingBLL;
        
        public FindPasswordBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __messageNoticeSettingBLL = new MessageNoticeSettingBLL();
        }

        public bool SendFindPasswordEmail(string label, string email, string ip,string hostUrl, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var requestCount = CountModelListByExpression(string.Format("CreateIP=\"{0}\"*CreateTime>\"{1}\"", ip, DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss")));
                if (requestCount > 10) throw new Exception("禁止频繁找回密码");
                var user = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*Email=\"{1}\"*IsDel=false", label.Trim(), email.Trim()));
                if (user == null) throw new Exception("用户不存在");
                var item = new FindPassword();
                item.Id = Guid.NewGuid();
                item.UserId = user.Id;
                item.CreateIP = ip;
                item.CreateTime = DateTime.Now;
                item.ValidityTime = DateTime.Now.AddDays(1);
                var title = __dictCodeBLL.GetDictCodeValueByCode("FindPassword", "EmailTitile");
                var bodyDictCode = __dictCodeBLL.GetDictCodeByCode("FindPassword", "EmailBody");
                var body = bodyDictCode == null ? "" : bodyDictCode.Remark;
                if(string.IsNullOrWhiteSpace(title)) title = "找回密码";
                if (string.IsNullOrWhiteSpace(body)) body = "找回密码: [Url]";
                if(body.IndexOf("[Url]") == -1) body += "[Url]";
                hostUrl += "/Account/FindPassword?a=" + SymmetryEncryptUtility.Encrypto(item.Id.ToString());
                body = body.Replace("\r\n", "<br />");
                body = body.Replace("[UserName]", user.UserName);
                body = body.Replace("[Label]", user.Label);
                body = body.Replace("点击这里", "<a href='[Url]'>点击这里</a>");
                body = body.Replace("[Url]", hostUrl);
                var messageNoticeSetting = __messageNoticeSettingBLL.GenerateMessageNoticeSetting();
                bool result = EmailUtility.SendEmail(messageNoticeSetting.IsUseSSL,
                                                   messageNoticeSetting.SMTP,
                                                   messageNoticeSetting.SMTPPort,
                                                   messageNoticeSetting.LoginUserName,
                                                   messageNoticeSetting.LoginPassword,
                                                   messageNoticeSetting.LoginUserName,
                                                   user.Email,
                                                   title,
                                                   body,
                                                   out errorMessage);
                if (!result) throw new Exception(errorMessage);
                Add(new FindPassword[] { item }, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.StartsWith("出错,") ? "" : "出错," + ex.Message;
                if (tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
        }

        public bool CheckAndResetPassword(Guid id, string ip, out string label, out string errorMessage)
        {
            errorMessage = "";
            label = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var item = GetEntityById(id);
                if (item == null) throw new Exception("无效ID");
                if (item.ValidityTime < DateTime.Now) throw new Exception("找回密码已过期");
                label = __userBLL.GetEntityById(item.UserId).Label;
                if(!__userBLL.LoginAndResetUserPassword(item.UserId, "111111", false, false, ip, true, ref tran, out errorMessage)) throw new Exception(errorMessage);
                Delete(new Guid[]{item.Id}, ref tran ,true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.StartsWith("出错,") ? "" : "出错," + ex.Message;
                if (tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
        }

    }
}