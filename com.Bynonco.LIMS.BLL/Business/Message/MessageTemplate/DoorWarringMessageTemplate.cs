using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class DoorWarringMessageTemplate : MessageTemplateBase
    {
        private IUserBLL __userBLL;
        private Guid? __doorId;
        private DateTime __optTime;
        private string __doorName;
        private string __labName;
        private string __labAddress;
        private string __receiveUserLabels;
        private string __openUserLabel;
        private User __openDoorUser;
        private IList<User> __receiverUsers;
        public DoorWarringMessageTemplate(MessageTemplateType messageTemplateType, string doorId, DateTime optTime, string doorName, string labName, string labAddress, string receiveUserLabels, string openDoorUserLabel, User sender)
            : base(messageTemplateType, sender)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            Guid doorIdTemp = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(doorId) && Guid.TryParse(doorId, out doorIdTemp))
                __doorId = doorIdTemp;
            this.__optTime = optTime;
            this.__doorName = doorName ?? "";
            this.__labName = labName ?? "";
            this.__labAddress = labAddress ?? "";
            this.__receiveUserLabels = receiveUserLabels ?? "";
            this.__openUserLabel = openDoorUserLabel ?? "";
            if (this.__openUserLabel != "") __openDoorUser = __userBLL.GetUserByLabel(__openUserLabel.Trim());
            if (this.__receiveUserLabels != "")
            {
                var userLabels = string.IsNullOrWhiteSpace(__receiveUserLabels) ? null : __receiveUserLabels.Split(',');
                __receiverUsers = __userBLL.GetUserListByLabels(userLabels);
            }
        }
        protected override MessageContext GenerateMessageContext()
        {
            var title = __labName + " 房间门锁未关好";
            var content = "房间地址: " + __labAddress + "   " + "<br />";
            content += " 最后一次开门人: " + __openDoorUser == null ? "" : __openDoorUser.UserName + "   " + "<br />";
            content += " 开门时间: " + __optTime.ToString() + "   " + "<br />";
            content += " 开门人联系电话: " + __openDoorUser == null ? "" : __openDoorUser.PhoneNumber ?? "" + "   " + "<br />";
            if (_messageTemplateType == MessageTemplateType.NoHTML)
            {
                content.Replace("<br />","\r\n");
            }
            var messageContext = new MessageContext(Model.Enum.MsgType.DoorWarring, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), _sender, null,__receiverUsers, null, __doorId,null);
            return messageContext;
        }
    }
}
