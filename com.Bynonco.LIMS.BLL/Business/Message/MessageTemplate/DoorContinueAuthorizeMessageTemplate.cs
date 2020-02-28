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
    public class DoorContinueAuthorizeMessageTemplate : MessageTemplateBase
    {
        private IUserBLL __userBLL;
        private IUserDoorContinuedAuthorizationBLL __userDoorContinuedAuthorizationBLL;
        private User __user;
        private UserDoorContinuedAuthorization __userDoorContinuedAuthorization;
        private UserDoorContinuedAuthorization __originalUserDoorContinuedAuthorization;
        public DoorContinueAuthorizeMessageTemplate(MessageTemplateType messageTemplateType, UserDoorContinuedAuthorization originalUserDoorContinuedAuthorization, UserDoorContinuedAuthorization userDoorContinuedAuthorization, User sender)
            : base(messageTemplateType, sender)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userDoorContinuedAuthorizationBLL = BLLFactory.CreateUserDoorContinuedAuthorizationBLL();
            this.__userDoorContinuedAuthorization = userDoorContinuedAuthorization;
            this.__originalUserDoorContinuedAuthorization = originalUserDoorContinuedAuthorization;
            __user = __userBLL.GetEntityById(__userDoorContinuedAuthorization.UserID.Value);
        }
        protected override MessageContext GenerateMessageContext()
        {
            OperateType operateType = __userDoorContinuedAuthorization.State != august.DataLink.ObjectState.Deleted ? (__originalUserDoorContinuedAuthorization == null ? OperateType.New : OperateType.Modified) : OperateType.Deleted;
            string title = "", content = "", contentHTML = "", entityName = "", entityCnName = "";
            __userDoorContinuedAuthorizationBLL.GenerateEntityLogData(operateType, __originalUserDoorContinuedAuthorization, __userDoorContinuedAuthorization, out title, out content, out contentHTML, out entityName, out entityCnName);
            return new MessageContext(Model.Enum.MsgType.DoorAuthorize, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,__user, new User[] { __user }, __user, __userDoorContinuedAuthorization.Id, null);
        }
    }
}
