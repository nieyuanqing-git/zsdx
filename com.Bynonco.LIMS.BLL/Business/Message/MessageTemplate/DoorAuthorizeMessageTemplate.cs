using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL
{
    public class DoorAuthorizeMessageTemplate : MessageTemplateBase
    {
        private IUserBLL __userBLL;
        private IUserDoorAuthorizationBLL __userDoorAuthorizationBLL;
        private User __user;
        private UserDoorAuthorization __userDoorAuthorization;
        private UserDoorAuthorization __originalUserDoorAuthorization;
        public DoorAuthorizeMessageTemplate(MessageTemplateType messageTemplateType,UserDoorAuthorization originalUserDoorAuthorization, UserDoorAuthorization userDoorAuthorization, User sender)
            : base(messageTemplateType, sender)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userDoorAuthorizationBLL = BLLFactory.CreateUserDoorAuthorizationBLL();
            this.__userDoorAuthorization = userDoorAuthorization;
            __user = __userBLL.GetEntityById(__userDoorAuthorization.UserID.Value);
            __originalUserDoorAuthorization = originalUserDoorAuthorization;
        }
        protected override MessageContext GenerateMessageContext()
        {
            OperateType operateType = __userDoorAuthorization.State != august.DataLink.ObjectState.Deleted ? (__originalUserDoorAuthorization==null ? OperateType.New:OperateType.Modified) : OperateType.Deleted;
            string title = "", content = "", contentHTML = "", entityName = "", entityCnName = "";
            __userDoorAuthorizationBLL.GenerateEntityLogData(operateType, __originalUserDoorAuthorization, __userDoorAuthorization, out title, out content, out contentHTML, out entityName, out entityCnName);
            return new MessageContext(Model.Enum.MsgType.DoorAuthorize, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,__user, new User[] { __user }, __user, __userDoorAuthorization.Id, null);
        }
    }
}
