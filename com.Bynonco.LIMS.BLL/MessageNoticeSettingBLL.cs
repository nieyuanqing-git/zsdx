using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class MessageNoticeSettingBLL : IMessageNoticeSettingBLL
    {
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private IDictCodeBLL __dictCodeBLL;
        private readonly string __CODE = "MessageNoticeSetting";
        private readonly string __NAME = "消息通知";
        public MessageNoticeSettingBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        public bool Save(MessageNoticeSetting messageNoticeSetting, out string errorMessage)
        {
            XTransaction tran = null;
            try
            {
                errorMessage = "";
                if (messageNoticeSetting == null) throw new Exception(errorMessage);
                tran = SessionManager.Instance.BeginTransaction();
                var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=" + __CODE);
                if (dictCodeType != null)
                {
                    if (dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
                    {
                        __dictCodeBLL.Delete(dictCodeType.DictCodes.Select(p => p.Id), ref tran, true);
                    }
                    __dictCodeTypeBLL.Delete(new Guid[] { dictCodeType.Id }, ref tran, true);
                }
                DictCodeType dictCodeTypeNew = new DictCodeType() { Id = Guid.NewGuid(), Code = __CODE, Name = __NAME, Value = "" };
                IList<DictCode> lstNewDictCodes = new List<DictCode>()
                {
                    new DictCode() { Id = Guid.NewGuid(), Code = "IsInnerMessageNotice",Name="是否发送站内消息", Value = messageNoticeSetting.IsInnerMessageNotice ? "1" : "0", DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "IsEmailNotice", Name="是否发送邮件",Value = messageNoticeSetting.IsEmailNotice ? "1" : "0", DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "IsMobilePhoneNotice ",Name="是否发送手机短信", Value = messageNoticeSetting.IsMobilePhoneNotice  ? "1" : "0", DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "IsUseSSL",Name="此服务是否要求安全连接", Value = messageNoticeSetting.IsUseSSL ? "1" : "0", DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "Autograph",Name="签名", Value = messageNoticeSetting.Autograph, DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "LoginUserName",Name="账号", Value = messageNoticeSetting.LoginUserName, DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "LoginPassword",Name="密码", Value = messageNoticeSetting.LoginPassword, DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "SMTP",Name="发送邮件服务器(SMTP)", Value = messageNoticeSetting.SMTP, DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "SMTPPort",Name="SMTP服务器端口", Value = messageNoticeSetting.SMTPPort.ToString(), DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "IsStartAutoSendingEmail",Name="开启邮件发送服务", Value = messageNoticeSetting.IsStartAutoSendingEmail?"1":"0", DictCodeTypeId = dictCodeTypeNew.Id },
                    new DictCode() { Id = Guid.NewGuid(), Code = "IsStartAutoSendingShortMessage",Name="开启手机短信发送服务", Value = messageNoticeSetting.IsStartAutoSendingShortMessage?"1":"0", DictCodeTypeId = dictCodeTypeNew.Id }
                };
                __dictCodeTypeBLL.Add(new DictCodeType[] { dictCodeTypeNew }, ref tran, true);
                __dictCodeBLL.Add(lstNewDictCodes, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
        }
        public MessageNoticeSetting GenerateMessageNoticeSetting()
        {
            MessageNoticeSetting messageNoticeSetting = new MessageNoticeSetting();
            try
            {
                var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=MessageNoticeSetting");
                if (dictCodeType != null && dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
                {
                    messageNoticeSetting.IsInnerMessageNotice = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsInnerMessageNotice").Value.Trim() == "1";
                    messageNoticeSetting.IsEmailNotice = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsEmailNotice").Value.Trim() == "1";
                    messageNoticeSetting.IsMobilePhoneNotice = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsMobilePhoneNotice").Value.Trim() == "1";
                    messageNoticeSetting.IsUseSSL = dictCodeType.DictCodes.First(p => p.Code.Trim() == "IsUseSSL").Value.Trim() == "1";
                    messageNoticeSetting.Autograph = dictCodeType.DictCodes.First(p => p.Code.Trim() == "Autograph").Value.Trim();
                    messageNoticeSetting.LoginPassword = SymmetryEncryptUtility.Decrypto(dictCodeType.DictCodes.First(p => p.Code.Trim() == "LoginPassword").Value);
                    messageNoticeSetting.LoginUserName = dictCodeType.DictCodes.First(p => p.Code.Trim() == "LoginUserName").Value.Trim();
                    messageNoticeSetting.SMTP = dictCodeType.DictCodes.First(p => p.Code.Trim() == "SMTP").Value.Trim();
                    messageNoticeSetting.SMTPPort = int.Parse(dictCodeType.DictCodes.First(p => p.Code.Trim() == "SMTPPort").Value.Trim());
                    //后面新加的，所以要做NULL判断
                    messageNoticeSetting.IsStartAutoSendingEmail = false;
                    var dictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code.Trim() == "IsStartAutoSendingEmail");
                    if (dictCode != null) messageNoticeSetting.IsStartAutoSendingEmail = dictCode.Value.Trim() == "1";
                    messageNoticeSetting.IsStartAutoSendingShortMessage = false;
                    dictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code.Trim() == "IsStartAutoSendingShortMessage");
                    if (dictCode != null) messageNoticeSetting.IsStartAutoSendingShortMessage = dictCode.Value.Trim() == "1";
                }
            }
            catch (Exception ex) {}
            return messageNoticeSetting;
        }
    }
}
