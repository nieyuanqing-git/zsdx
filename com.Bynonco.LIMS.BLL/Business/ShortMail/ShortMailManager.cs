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
using com.Bynonco.Leams.GsmComm.GsmCommunication;
using com.Bynonco.Leams.GsmComm.PDUConverter;
using com.Bynonco.Leams.GsmComm.PDUConverter.SmartMessaging;
using System.Threading;

namespace com.Bynonco.LIMS.BLL
{
    public class ShortMailManager : IShortMailManager
    {
        private static ISendShortMailBLL __sendShortMailBLL;
        private static IShortMailManager __shortMailManager;
        private static IUserBLL __userBLL;
        private static object __lockObjCreate = new object();
        private static object __lockObjBusiness = new object();
        private int __maxFailTimes = 5;
        private int __comPort = 1;
        private string __shortMessageCenter = "+8613800200500";
        private int __waitInterval = 1500;
        static ShortMailManager()
        {
            __sendShortMailBLL = BLLFactory.CreateSendShortMailBLL();
            __userBLL = BLLFactory.CreateUserBLL();
        }
        private ShortMailManager(int maxFailTimes, int comPort, string shortMessageCenter, int waitInterval)
        {
            __maxFailTimes = maxFailTimes;
            __comPort = comPort;
            __shortMessageCenter = shortMessageCenter;
            __waitInterval = waitInterval;
        }
        public static IShortMailManager GetInstance(int maxFailTimes, int comPort, string shortMessageCenter, int waitInterval)
        {
            if (__shortMailManager == null)
            {
                lock (__lockObjCreate)
                {
                    __shortMailManager = new ShortMailManager(maxFailTimes, comPort, shortMessageCenter, waitInterval);
                }
            }
            return __shortMailManager;
        }

        public void SendShortMail(Guid? operatorId, GsmCommMain comm, out int successCount, out int failCount, out string errorMessage)
        {
            lock (__lockObjBusiness)
            {
                successCount = 0;
                failCount = 0;
                errorMessage = "";
                try
                {
                    if (comm.IsConnected())
                    {
                        var unSendShortMailList = __sendShortMailBLL.GetScopeEntitiesByExpression(string.Format("Status={0}*MobilePhoneNo=-null*FailTimes<{1}*(ValidateTime=null+ValidateTime<\"{2}\")", (int)SendShortMailStatus.UnSend, __maxFailTimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 1, 9999999);
                        if (unSendShortMailList == null) return;
                        if (unSendShortMailList == null && unSendShortMailList.Count == 0) return;
                        StringBuilder sbErrorMsg = new StringBuilder();
                        foreach (var unSendShortMail in unSendShortMailList)
                        {
                            XTransaction tran = null;
                            try
                            {
                                if (string.IsNullOrWhiteSpace(unSendShortMail.MobilePhoneNo)) throw new Exception("手机号码为空");
                                if (!unSendShortMail.MobilePhoneNo.Trim().IsMobilePhoneNumber()) throw new Exception(string.Format("手机号码【{0}】非法", unSendShortMail.MobilePhoneNo));
                                if (unSendShortMail.ReceiverId.HasValue)
                                {
                                    bool isReceive = true;
                                    var receiver = __userBLL.GetEntityById(unSendShortMail.ReceiverId.Value);
                                    IMessageReceiveSettingManager messageReceiveSettingManager = new MessageReceiveSettingManager(receiver, receiver);
                                    var r = messageReceiveSettingManager.JudgeIsNeedNotice((MsgType)unSendShortMail.MsgType, MessageSender.ShortMessageSender, out errorMessage, out isReceive);
                                    if (!r)
                                    {
                                        if (!isReceive) throw new Exception(errorMessage);
                                        continue;
                                    }
                                }
                                if (string.IsNullOrWhiteSpace(unSendShortMail.MobilePhoneNo.Trim())) throw new ArgumentException("手机号码为空");
                                if (!ValidateUtility.IsMobilePhoneNumber(unSendShortMail.MobilePhoneNo.Trim())) throw new ArgumentException("手机号码非法");
                                if (string.IsNullOrWhiteSpace(unSendShortMail.Body)) throw new ArgumentException("发送内容为空");
                                if (!comm.IsConnected()) throw new Exception("短信设备未就绪");
                                SmsSubmitPdu[] pdus;
                                pdus = SmartMessageFactory.CreateConcatUnicodeTextMessage(unSendShortMail.Body, unSendShortMail.MobilePhoneNo.Trim(), __shortMessageCenter);
                                comm.SendMessages(pdus);
                                unSendShortMail.StatusEnum = Model.Enum.SendShortMailStatus.Finished;
                                __sendShortMailBLL.Save(new SendShortMail[] { unSendShortMail }, ref tran);
                                successCount++;
                            }
                            catch (Exception ex)
                            {
                                sbErrorMsg.AppendFormat("编码为:{0},接受手机号码为:{1},主题为:{2}发送失败，原因:{3}",
                                                        unSendShortMail.Id,
                                                        unSendShortMail.MobilePhoneNo,
                                                        unSendShortMail.Title,
                                                        ex.Message).AppendLine();
                                unSendShortMail.FailTimes++;
                                __sendShortMailBLL.Save(new SendShortMail[] { unSendShortMail }, ref tran);
                                failCount++;
                            }
                            finally
                            {
                                Thread.Sleep(__waitInterval);
                            }
                        }
                        errorMessage = sbErrorMsg.ToString();
                    }
                }
                catch (Exception ex) { errorMessage = ex.Message; }
            }
        }
        //public void SendShortMail(Guid? operatorId, out int successCount, out int failCount, out string errorMessage)
        //{
        //    lock (__lockObjBusiness)
        //    {
        //        successCount = 0;
        //        failCount = 0;
        //        errorMessage = "";
        //        try
        //        {
        //           var isConnected = ShortMessageUtility.Connection(__comPort, out errorMessage);
        //           if (isConnected)
        //           {
        //               var unSendShortMailList = __sendShortMailBLL.GetScopeEntitiesByExpression(string.Format("Status={0}*MobilePhoneNo=-null*FailTimes<{1}*(ValidateTime=null+ValidateTime<\"{2}\")", (int)SendShortMailStatus.UnSend, __maxFailTimes  , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 1, 9999999);
        //               if (unSendShortMailList == null ) return;
        //               if (unSendShortMailList == null && unSendShortMailList.Count == 0) return;
        //               StringBuilder sbErrorMsg = new StringBuilder();
        //               foreach (var unSendShortMail in unSendShortMailList)
        //               {
        //                   XTransaction tran = null;
        //                   try
        //                   {
        //                       if (string.IsNullOrWhiteSpace(unSendShortMail.MobilePhoneNo)) throw new Exception("手机号码为空");
        //                       if (!unSendShortMail.MobilePhoneNo.Trim().IsMobilePhoneNumber()) throw new Exception(string.Format("手机号码【{0}】非法", unSendShortMail.MobilePhoneNo));
        //                       string errorMessageSend = "";
        //                       if (unSendShortMail.ReceiverId.HasValue)
        //                       {
        //                           bool isReceive = true;
        //                           var receiver = __userBLL.GetEntityById(unSendShortMail.ReceiverId.Value);
        //                           IMessageReceiveSettingManager messageReceiveSettingManager = new MessageReceiveSettingManager(receiver, receiver);
        //                           var r = messageReceiveSettingManager.JudgeIsNeedNotice((MsgType)unSendShortMail.MsgType, MessageSender.ShortMessageSender, out errorMessage, out isReceive);
        //                           if (!r)
        //                           {
        //                               if (!isReceive) throw new Exception(errorMessage);
        //                               continue;
        //                           }
        //                       }
        //                       bool result = ShortMessageUtility.GetInstace(__comPort, __shortMessageCenter).SendShortMessage(unSendShortMail.MobilePhoneNo.Trim(),
        //                                                unSendShortMail.Body,
        //                                                out errorMessageSend);
        //                       if (!result) throw new Exception(errorMessageSend);
        //                       unSendShortMail.StatusEnum = Model.Enum.SendShortMailStatus.Finished;
        //                       //unSendShortMail.SendOperaterId = operatorId;
        //                       __sendShortMailBLL.Save(new SendShortMail[] { unSendShortMail }, ref tran);
        //                       successCount++;
        //                   }
        //                   catch (Exception ex)
        //                   {
        //                       sbErrorMsg.AppendFormat("编码为:{0},接受手机号码为:{1},主题为:{2}发送失败，原因:{3}",
        //                                               unSendShortMail.Id,
        //                                               unSendShortMail.MobilePhoneNo,
        //                                               unSendShortMail.Title,
        //                                               ex.Message).AppendLine();
        //                       unSendShortMail.FailTimes++;
        //                       __sendShortMailBLL.Save(new SendShortMail[] { unSendShortMail }, ref tran);
        //                       failCount++;
        //                   }
        //               }
        //               errorMessage = sbErrorMsg.ToString();
        //           }
        //           ShortMessageUtility.Disconnection( out errorMessage);
        //        }
        //        catch (Exception ex) { errorMessage = ex.Message; }
        //    }
        //}

        public void SendShortMailWebRequest(Guid? operatorId, string userName, string passWord, string url, out int successCount, out int failCount, out string errorMessage)
        {
            lock (__lockObjBusiness)
            {
                successCount = 0;
                failCount = 0;
                errorMessage = "";
                try
                {
                    var unSendShortMailList = __sendShortMailBLL.GetScopeEntitiesByExpression(string.Format("Status={0}*MobilePhoneNo=-null*FailTimes<{1}*(ValidateTime=null+ValidateTime<\"{2}\")", (int)SendShortMailStatus.UnSend, __maxFailTimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 1, 9999999);
                    if (unSendShortMailList == null) return;
                    if (unSendShortMailList == null && unSendShortMailList.Count == 0) return;
                    StringBuilder sbErrorMsg = new StringBuilder();
                    foreach (var unSendShortMail in unSendShortMailList)
                    {
                        XTransaction tran = null;
                        try
                        {
                            if (string.IsNullOrWhiteSpace(unSendShortMail.MobilePhoneNo)) throw new Exception("手机号码为空");
                            if (!unSendShortMail.MobilePhoneNo.Trim().IsMobilePhoneNumber()) throw new Exception(string.Format("手机号码【{0}】非法", unSendShortMail.MobilePhoneNo));
                            string errorMessageSend = "";
                            if (unSendShortMail.ReceiverId.HasValue)
                            {
                                bool isReceive = true;
                                var receiver = __userBLL.GetEntityById(unSendShortMail.ReceiverId.Value);
                                IMessageReceiveSettingManager messageReceiveSettingManager = new MessageReceiveSettingManager(receiver, receiver);
                                var r = messageReceiveSettingManager.JudgeIsNeedNotice((MsgType)unSendShortMail.MsgType, MessageSender.ShortMessageSender, out errorMessage, out isReceive);
                                if (!r)
                                {
                                    if (!isReceive) throw new Exception(errorMessage);
                                    continue;
                                }
                            }
                            bool result = ShortMessageUtility.GetInstace(__comPort, __shortMessageCenter).SendShortMessageWebRequest(userName, passWord, url, unSendShortMail.MobilePhoneNo.Trim(),
                                                        unSendShortMail.Body,
                                                        out errorMessageSend);
                            if (!result) throw new Exception(errorMessageSend);
                            unSendShortMail.StatusEnum = Model.Enum.SendShortMailStatus.Finished;
                            __sendShortMailBLL.Save(new SendShortMail[] { unSendShortMail }, ref tran);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            sbErrorMsg.AppendFormat("编码为:{0},接受手机号码为:{1},主题为:{2}发送失败，原因:{3}",
                                                    unSendShortMail.Id,
                                                    unSendShortMail.MobilePhoneNo,
                                                    unSendShortMail.Title,
                                                    ex.Message).AppendLine();
                            unSendShortMail.FailTimes++;
                            __sendShortMailBLL.Save(new SendShortMail[] { unSendShortMail }, ref tran);
                            failCount++;
                        }
                    }
                    errorMessage = sbErrorMsg.ToString();
                    
                }
                catch (Exception ex) { errorMessage = ex.Message; }
            }
        }

        public void SendShortMailWebRequest(int bm, Guid? operatorId, string userName, string passWord, string url, out int successCount, out int failCount, out string errorMessage)
        {
            lock (__lockObjBusiness)
            {
                successCount = 0;
                failCount = 0;
                errorMessage = "";
                try
                {
                    var unSendShortMailList = __sendShortMailBLL.GetScopeEntitiesByExpression(string.Format("Status={0}*MobilePhoneNo=-null*FailTimes<{1}*(ValidateTime=null+ValidateTime<\"{2}\")", (int)SendShortMailStatus.UnSend, __maxFailTimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 1, 9999999);
                    if (unSendShortMailList == null) return;
                    if (unSendShortMailList == null && unSendShortMailList.Count == 0) return;
                    StringBuilder sbErrorMsg = new StringBuilder();
                    foreach (var unSendShortMail in unSendShortMailList)
                    {
                        XTransaction tran = null;
                        try
                        {
                            if (string.IsNullOrWhiteSpace(unSendShortMail.MobilePhoneNo)) throw new Exception("手机号码为空");
                            if (!unSendShortMail.MobilePhoneNo.Trim().IsMobilePhoneNumber()) throw new Exception(string.Format("手机号码【{0}】非法", unSendShortMail.MobilePhoneNo));
                            string errorMessageSend = "";
                            if (unSendShortMail.ReceiverId.HasValue)
                            {
                                bool isReceive = true;
                                var receiver = __userBLL.GetEntityById(unSendShortMail.ReceiverId.Value);
                                IMessageReceiveSettingManager messageReceiveSettingManager = new MessageReceiveSettingManager(receiver, receiver);
                                var r = messageReceiveSettingManager.JudgeIsNeedNotice((MsgType)unSendShortMail.MsgType, MessageSender.ShortMessageSender, out errorMessage, out isReceive);
                                if (!r)
                                {
                                    if (!isReceive) throw new Exception(errorMessage);
                                    continue;
                                }
                            }
                            bool result = ShortMessageUtility.GetInstace(__comPort, __shortMessageCenter).SendShortMessageWebRequest(bm, userName, passWord, url, unSendShortMail.MobilePhoneNo.Trim(),
                                                        unSendShortMail.Body,
                                                        out errorMessageSend);
                            if (!result) throw new Exception(errorMessageSend);
                            unSendShortMail.StatusEnum = Model.Enum.SendShortMailStatus.Finished;
                            //__sendShortMailBLL.Save(new SendShortMail[] { unSendShortMail }, ref tran);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            sbErrorMsg.AppendFormat("编码为:{0},接受手机号码为:{1},主题为:{2}发送失败，原因:{3}",
                                                    unSendShortMail.Id,
                                                    unSendShortMail.MobilePhoneNo,
                                                    unSendShortMail.Title,
                                                    ex.Message).AppendLine();
                            unSendShortMail.FailTimes++;
                            __sendShortMailBLL.Save(new SendShortMail[] { unSendShortMail }, ref tran);
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
