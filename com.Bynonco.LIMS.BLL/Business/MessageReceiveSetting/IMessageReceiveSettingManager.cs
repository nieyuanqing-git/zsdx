using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business
{
    public interface IMessageReceiveSettingManager
    {
        IList<UserMessageNoticeSetting> GetUserMessageNoticeSettingList();
        UserMessageNoticeSetting GetUserMessageNoticeSetting(MsgType msgType, MessageSender messageSender);
        bool JudgeIsNeedNotice(MsgType msgType, MessageSender messageSender, out string errorMessage, out bool isReceive);
        bool SaveUserMessageNoticeSetting(IEnumerable<UserMessageNoticeSetting> userMessageNoticeSettings, out string errorMessage);
    }
}
