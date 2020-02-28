using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class MessageReceiveSettingManager : IMessageReceiveSettingManager
    {
        protected User _user;
        protected User _operator;
        protected IUserMessageNoticeSettingBLL _userMessageNoticeSettingBLL;
        protected IUserWorkScopeBLL _userWorkScopeBLL;
        protected readonly string _allDayTimeScopt = "00:00-23:59";
        public MessageReceiveSettingManager(User user,User operateUser)
        {
            _user = user;
            _operator = operateUser;
            _userMessageNoticeSettingBLL = BLLFactory.CreateUserMessageNoticeSettingBLL();
            _userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
        }
        private void GenerateUserMessageNoticeSettings(MsgType msgType, IList<UserMessageNoticeSetting> userMessageNoticeSettingList)
        {
            GenerateUserMessageNoticeSetting(msgType, MessageSender.InnerMessageSender, userMessageNoticeSettingList);
            GenerateUserMessageNoticeSetting(msgType, MessageSender.EmailMessageSender, userMessageNoticeSettingList);
            GenerateUserMessageNoticeSetting(msgType, MessageSender.ShortMessageSender, userMessageNoticeSettingList);
        }


        private void GenerateUserMessageNoticeSettingsByWxManager(MsgType msgType, IList<UserMessageNoticeSetting> userMessageNoticeSettingList)
        {
            GenerateUserMessageNoticeSetting(msgType, MessageSender.weixinManagerMessageSender, userMessageNoticeSettingList);
        }

        private UserMessageNoticeSetting GenerateNewUserMessageNoticeSetting(MsgType? msgType, MessageSender messageSender)
        {
            Guid? userId = null;
            if (_user != null) userId = _user.Id;
            int? msgTypeTemp = null;
            if (msgType.HasValue) msgTypeTemp = (int)msgType.Value;
            var userMessageNoticeSetting = new UserMessageNoticeSetting()
            {
                Id = Guid.NewGuid(),
                IsReceiveMessage = true,
                IsUseDefaultSetting = (_user != null && msgType == null) || msgType != null ? true : false,
                MessageReceiveTimes = _allDayTimeScopt,
                MessageSendType = (int)messageSender,
                MsgType = msgTypeTemp,
                UserId = userId
            };
            return userMessageNoticeSetting;
        }
        private void GenerateDefaultMessageNoticeSettings(List<UserMessageNoticeSetting> userMessageNoticeSettingList)
        {
            Guid? userId = null;
            if (_user != null) userId = _user.Id;
            if (userMessageNoticeSettingList == null || userMessageNoticeSettingList.Count == 0)
            {
                userMessageNoticeSettingList.Add(GenerateNewUserMessageNoticeSetting(null, MessageSender.InnerMessageSender));
                userMessageNoticeSettingList.Add(GenerateNewUserMessageNoticeSetting(null, MessageSender.EmailMessageSender));
                userMessageNoticeSettingList.Add(GenerateNewUserMessageNoticeSetting(null, MessageSender.ShortMessageSender));
                userMessageNoticeSettingList.Add(GenerateNewUserMessageNoticeSetting(null, MessageSender.weixinManagerMessageSender));
            }
            else
            {
                var userDefaultInnerMessageDefaultSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MessageSendType == (int)MessageSender.InnerMessageSender && p.UserId == userId && p.MsgType == null);
                var defaultInnerMessageDefaultSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MessageSendType == (int)MessageSender.InnerMessageSender && p.UserId == null && p.MsgType == null);
                if (_user != null && userDefaultInnerMessageDefaultSetting != null)
                {
                    userMessageNoticeSettingList.Add(userDefaultInnerMessageDefaultSetting);
                    userMessageNoticeSettingList.Remove(defaultInnerMessageDefaultSetting);
                }
                if (defaultInnerMessageDefaultSetting != null)
                {
                    if (userDefaultInnerMessageDefaultSetting == null)
                    {
                        if (_user != null) defaultInnerMessageDefaultSetting.IsUseDefaultSetting = true;
                        userMessageNoticeSettingList.Add(defaultInnerMessageDefaultSetting);
                    }
                }
                else
                {
                    userMessageNoticeSettingList.Add(GenerateNewUserMessageNoticeSetting(null, MessageSender.InnerMessageSender));
                }


                var userDefaultEmailDefaultSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MessageSendType == (int)MessageSender.EmailMessageSender && p.UserId == userId && p.MsgType == null);
                var defaultEmailDefaultSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MessageSendType == (int)MessageSender.EmailMessageSender && p.UserId == null && p.MsgType == null);
                if (_user != null && userDefaultEmailDefaultSetting != null)
                {
                    userMessageNoticeSettingList.Add(userDefaultEmailDefaultSetting);
                    userMessageNoticeSettingList.Remove(defaultEmailDefaultSetting);
                }
                if (defaultEmailDefaultSetting != null)
                {
                    if (userDefaultEmailDefaultSetting == null)
                    {
                        if (_user != null) defaultEmailDefaultSetting.IsUseDefaultSetting = true;
                        userMessageNoticeSettingList.Add(defaultEmailDefaultSetting);
                    }
                }
                else
                {
                    userMessageNoticeSettingList.Add(GenerateNewUserMessageNoticeSetting(null, MessageSender.EmailMessageSender));
                }


                var userDefaultShortMessageDefaultSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MessageSendType == (int)MessageSender.ShortMessageSender && p.UserId == userId && p.MsgType == null);
                var defaultShortMessageDefaultSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MessageSendType == (int)MessageSender.ShortMessageSender && p.UserId == null && p.MsgType == null);
                if (_user != null && userDefaultShortMessageDefaultSetting != null)
                {
                    userMessageNoticeSettingList.Add(userDefaultShortMessageDefaultSetting);
                    userMessageNoticeSettingList.Remove(defaultShortMessageDefaultSetting);
                }
                if (defaultEmailDefaultSetting != null)
                {
                    if (userDefaultShortMessageDefaultSetting == null)
                    {
                        if (_user != null) defaultShortMessageDefaultSetting.IsUseDefaultSetting = true;
                        userMessageNoticeSettingList.Add(defaultShortMessageDefaultSetting);
                    }
                }
                else
                {
                    userMessageNoticeSettingList.Add(GenerateNewUserMessageNoticeSetting(null, MessageSender.ShortMessageSender));
                }
            }
        }
        private void GenerateUserMessageNoticeSetting(MsgType msgType, MessageSender messageSender, IList<UserMessageNoticeSetting> userMessageNoticeSettingList)
        {
            UserMessageNoticeSetting userMessageNoticeSetting = null;
            if (userMessageNoticeSettingList != null && userMessageNoticeSettingList.Count > 0)
            {
                Guid? userId = null;
                if (_user != null) userId = _user.Id;
                userMessageNoticeSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MsgType == (int)msgType && p.UserId == userId && p.MessageSendType == (int)messageSender);
                var defaultUserMessageNoticeSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MsgType == (int)msgType && p.UserId == null && p.MessageSendType == (int)messageSender);
                var defaultMessageNoticeSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MsgType == null && p.UserId == null && p.MessageSendType == (int)messageSender);
                if (userMessageNoticeSetting == null)
                {
                    if (defaultUserMessageNoticeSetting != null)
                    {
                        var defaultUserMessageNoticeSettingClone = (UserMessageNoticeSetting)defaultUserMessageNoticeSetting.Clone();
                        defaultUserMessageNoticeSettingClone.IsUseDefaultSetting = true;
                        userMessageNoticeSettingList.Add(defaultUserMessageNoticeSettingClone);
                        userMessageNoticeSettingList.Remove(defaultUserMessageNoticeSetting);
                    }
                    if (defaultMessageNoticeSetting != null)
                    {
                        if (defaultUserMessageNoticeSetting == null)
                        {
                            var defaultMessageNoticeSettingClone = (UserMessageNoticeSetting)defaultMessageNoticeSetting.Clone();
                            defaultMessageNoticeSettingClone.IsUseDefaultSetting = true;
                            userMessageNoticeSettingList.Add(defaultMessageNoticeSettingClone);
                        }
                        //userMessageNoticeSettingList.Remove(defaultMessageNoticeSetting);
                    }
                }
                else
                {
                    if (defaultUserMessageNoticeSetting != null && defaultUserMessageNoticeSetting.Id != userMessageNoticeSetting.Id && _user != null)
                    {
                        userMessageNoticeSettingList.Remove(defaultUserMessageNoticeSetting);
                    }
                    if (defaultMessageNoticeSetting != null && defaultMessageNoticeSetting.Id != userMessageNoticeSetting.Id && _user != null)
                    {
                        userMessageNoticeSettingList.Remove(defaultMessageNoticeSetting);
                    }
                }
                if (userMessageNoticeSetting == null && defaultUserMessageNoticeSetting == null && defaultUserMessageNoticeSetting == null)
                    userMessageNoticeSettingList.Add(GenerateNewUserMessageNoticeSetting(msgType, messageSender));
            }
           
        }
        public IList<UserMessageNoticeSetting> GetUserMessageNoticeSettingList()
        {
            var userMessageNoticeSettingList = _user != null ?
                _userMessageNoticeSettingBLL.GetEntitiesByExpression(string.Format("UserId=null+UserId=\"{0}\"", _user.Id)) :
                _userMessageNoticeSettingBLL.GetEntitiesByExpression("UserId=null)");
            List<UserMessageNoticeSetting> lstUserMessageNoticeSetting = new List<UserMessageNoticeSetting>();
            if (userMessageNoticeSettingList != null && userMessageNoticeSettingList.Count > 0)
            {
                lstUserMessageNoticeSetting.AddRange(userMessageNoticeSettingList);
            }
            GenerateDefaultMessageNoticeSettings(lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.UserMsg, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.RegisterInform, lstUserMessageNoticeSetting);
            if (_user == null || !_user.TutorId.HasValue)
            {
                GenerateUserMessageNoticeSettings(MsgType.BindTutorMessage, lstUserMessageNoticeSetting);
                GenerateUserMessageNoticeSettings(MsgType.SampleApplyTutorAudit, lstUserMessageNoticeSetting);
            }
            GenerateUserMessageNoticeSettings(MsgType.SampleOutcomeInform, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.AppointmentSuccess, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.AppointmentAudit, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.UseEquipmentRemind, lstUserMessageNoticeSetting);
            if (_user == null || !_user.TutorId.HasValue)
            {

                GenerateUserMessageNoticeSettings(MsgType.DepositAuditPass, lstUserMessageNoticeSetting);
                GenerateUserMessageNoticeSettings(MsgType.DepositAlert, lstUserMessageNoticeSetting);
            }
            GenerateUserMessageNoticeSettings(MsgType.ExperimenterSubjectFundsWarring, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.DeductInform, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.BalanceInform, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.PunishInform, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.RepealBadBehavior, lstUserMessageNoticeSetting);
            
            if (CustomerFactory.GetCustomer().GetIsOpenAnimalModules())
            {
                GenerateUserMessageNoticeSettings(MsgType.AnimalRegisterDeath, lstUserMessageNoticeSetting);
            }
            GenerateUserMessageNoticeSettings(MsgType.UseEquipmentOutOfTime, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.EquipmentBlackListInform, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettings(MsgType.EquipmentAuthorize, lstUserMessageNoticeSetting);
            var count = _user != null ? _userWorkScopeBLL.CountModelListByExpression(string.Format("UserId=\"{0}\"", _user.Id)) : 0;
            if (count > 0 || _user == null)
            {
                GenerateUserMessageNoticeSettings(MsgType.EquipmentManagement, lstUserMessageNoticeSetting);
                GenerateUserMessageNoticeSettings(MsgType.TemperatureHumidityWarring, lstUserMessageNoticeSetting);
                GenerateUserMessageNoticeSettings(MsgType.DoorWarring, lstUserMessageNoticeSetting);
            }
            GenerateUserMessageNoticeSettings(MsgType.DoorAuthorize, lstUserMessageNoticeSetting);

            //微信端消息通知
            GenerateUserMessageNoticeSettingsByWxManager(MsgType.UserEquipmentAudit, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettingsByWxManager(MsgType.UserAudit, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettingsByWxManager(MsgType.AppointmentAdminAudit, lstUserMessageNoticeSetting);
            GenerateUserMessageNoticeSettingsByWxManager(MsgType.UseEquipmentOutOfAppointmentTime, lstUserMessageNoticeSetting);

            return lstUserMessageNoticeSetting.Select(p => new UserMessageNoticeSetting() { IsReceiveMessage = p.IsReceiveMessage, IsUseDefaultSetting=p.IsUseDefaultSetting, MessageReceiveTimes=p.MessageReceiveTimes, MessageSendType = p.MessageSendType, MsgType= p.MsgType, UserId = p.UserId }).Distinct().ToList();
        }
        public UserMessageNoticeSetting GetUserMessageNoticeSetting(MsgType msgType, MessageSender messageSender)
        {
            UserMessageNoticeSetting userMessageNoticeSetting = null;
            var userMessageNoticeSettingList = _userMessageNoticeSettingBLL.GetEntitiesByExpression(string.Format("(UserId=null+UserId=\"{0}\")*MessageSendType={1}*(MsgType=null+MsgType={2})", _user.Id, (int)messageSender, (int)msgType));
            if (userMessageNoticeSettingList != null && userMessageNoticeSettingList.Count > 0)
            {
                userMessageNoticeSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MsgType == (int)msgType && p.MessageSendType == (int)messageSender && p.UserId == _user.Id);
                if (userMessageNoticeSetting == null)
                {
                    userMessageNoticeSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MsgType == null && p.MessageSendType == (int)messageSender && p.UserId == _user.Id);
                }
                //wuzewu  修改消息接收已经设置不接收了，还继续发送的bug
                if (userMessageNoticeSetting == null)
                {
                    userMessageNoticeSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MsgType == (int)msgType && p.MessageSendType == (int)messageSender && p.UserId == null);
                }
                if (userMessageNoticeSetting == null)
                {
                    userMessageNoticeSetting = userMessageNoticeSettingList.FirstOrDefault(p => p.MsgType == null && p.MessageSendType == (int)messageSender && p.UserId == null);
                }
            }
            if (userMessageNoticeSetting == null) userMessageNoticeSetting = GenerateNewUserMessageNoticeSetting(msgType, messageSender);
            return userMessageNoticeSetting;

        }
        public bool JudgeIsNeedNotice(MsgType msgType, MessageSender messageSender, out string errorMessage,out bool isReceive)
        {
            errorMessage = "";
            isReceive = false;
            var now = DateTime.Now;
            var userMessageNoticeSetting = GetUserMessageNoticeSetting(msgType, messageSender);
            if (userMessageNoticeSetting != null)
            {
                isReceive = userMessageNoticeSetting.IsReceiveMessage;
                if (!userMessageNoticeSetting.IsReceiveMessage)
                {
                    errorMessage = "设置了不接受该类型的消息";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(userMessageNoticeSetting.MessageReceiveTimes)) userMessageNoticeSetting.MessageReceiveTimes = _allDayTimeScopt;
                var timeScopes =  TimeUtility.GetTimesByTimeFormatStr(userMessageNoticeSetting.MessageReceiveTimes, now, ',');
                foreach (var timeScope in timeScopes)
                {
                    if (now >= timeScope.BeginTime && now <= timeScope.EndTime)
                    {
                        return true;
                    }
                }
                errorMessage = "未到用户指定的发送时间";
                return false;
            }
            return true;
        }
        public bool SaveUserMessageNoticeSetting(IEnumerable<UserMessageNoticeSetting> userMessageNoticeSettings, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                var privilige = PriviligeFactory.GetMessageReceiveSettingPrivilige(_operator.Id);
                if (privilige == null || !privilige.IsEnableSave)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                tran = SessionManager.Instance.BeginTransaction();
                IList<UserMessageNoticeSetting> userMessageNoticeSettingList = _user != null ?
                    _userMessageNoticeSettingBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", _user.Id)) :
                    _userMessageNoticeSettingBLL.GetEntitiesByExpression("UserId=null");
                if (userMessageNoticeSettingList != null && userMessageNoticeSettingList.Count > 0)
                {
                    _userMessageNoticeSettingBLL.Delete(userMessageNoticeSettingList.Select(p => p.Id), ref tran, true);
                }
                if (userMessageNoticeSettings != null && userMessageNoticeSettings.Count() > 0)
                {
                    _userMessageNoticeSettingBLL.Add(userMessageNoticeSettings, ref tran, true);
                }
                if(tran.HasTransaction)tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (tran != null) tran.RollbackTransaction();
            }
            finally { if (tran != null) tran.Dispose(); }
            return true;
        }
    }
}
