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
    public class EquipmentAuthorizeMessageTemplate : MessageTemplateBase
    {
        private IUserBLL __userBLL;
        private IUserEquipmentAuthorizationBLL __userEquipmentAuthorizationBLL;
        private User __user;
        private UserEquipmentAuthorization __userEquipmentAuthorization;
        private UserEquipmentAuthorization __originalUserEquipmentAuthorization;
        public EquipmentAuthorizeMessageTemplate(MessageTemplateType messageTemplateType, UserEquipmentAuthorization originalUserEquipmentAuthorization, UserEquipmentAuthorization userEquipmentAuthorization, User sender)
            : base(messageTemplateType, sender)
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userEquipmentAuthorizationBLL = BLLFactory.CreateUserEquipmentAuthorizationBLL();
            this.__userEquipmentAuthorization = userEquipmentAuthorization;
            this.__originalUserEquipmentAuthorization = originalUserEquipmentAuthorization;
            __user = __userBLL.GetEntityById(__userEquipmentAuthorization.UserID.Value);
        }
        protected override MessageContext GenerateMessageContext()
        {
            OperateType operateType = __userEquipmentAuthorization.State != august.DataLink.ObjectState.Deleted ? (__originalUserEquipmentAuthorization == null ? OperateType.New : OperateType.Modified) : OperateType.Deleted;
            string title = "", content = "", contentHTML = "", entityName = "", entityCnName = "";
            __userEquipmentAuthorizationBLL.GenerateEntityLogData(operateType, __originalUserEquipmentAuthorization, __userEquipmentAuthorization, out title, out content, out contentHTML, out entityName, out entityCnName);
            return new MessageContext(Model.Enum.MsgType.EquipmentAuthorize, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,__user, new User[] { __user }, __user, __userEquipmentAuthorization.Id, null);
        }
    }
}
