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
    public class EquipmentContinueAuthorizeMessageTemplate : MessageTemplateBase
    {
        private IEquipmentBLL __equipmentBLL;
        private IUserBLL __userBLL;
        private IUserEquipmentContinuedAuthorizationBLL __userEquipmentContinuedAuthorizationBLL;
        private Equipment __equipment;
        private Equipment __originalEquipment;
        private User __user;
        private UserEquipmentContinuedAuthorization __userEquipmentContinuedAuthorization;
        private UserEquipmentContinuedAuthorization __originalUserEquipmentContinuedAuthorization;
        public EquipmentContinueAuthorizeMessageTemplate(MessageTemplateType messageTemplateType, UserEquipmentContinuedAuthorization originalUserEquipmentContinuedAuthorization, UserEquipmentContinuedAuthorization userEquipmentContinuedAuthorization, User sender)
            : base(messageTemplateType, sender)
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __userEquipmentContinuedAuthorizationBLL = BLLFactory.CreateUserEquipmentContinuedAuthorizationBLL();
            this.__userEquipmentContinuedAuthorization = userEquipmentContinuedAuthorization;
            __equipment = __equipmentBLL.GetEntityById(__userEquipmentContinuedAuthorization.EquipmentID.Value);
            __user = __userBLL.GetEntityById(__userEquipmentContinuedAuthorization.UserID.Value);
            __originalUserEquipmentContinuedAuthorization = originalUserEquipmentContinuedAuthorization;
            if (__originalUserEquipmentContinuedAuthorization != null && __originalUserEquipmentContinuedAuthorization.UserID != __userEquipmentContinuedAuthorization.UserID)
                __originalUserEquipmentContinuedAuthorization = null;
            if (__originalUserEquipmentContinuedAuthorization != null)
            {
                __originalEquipment = __equipmentBLL.GetEntityById(__originalUserEquipmentContinuedAuthorization.EquipmentID.Value);
            }
        }
        protected override MessageContext GenerateMessageContext()
        {
            OperateType operateType = __userEquipmentContinuedAuthorization.State != august.DataLink.ObjectState.Deleted ? (__originalUserEquipmentContinuedAuthorization == null ? OperateType.New : OperateType.Modified) : OperateType.Deleted;
            string title = "", content = "", contentHTML = "", entityName = "", entityCnName = "";
            __userEquipmentContinuedAuthorizationBLL.GenerateEntityLogData(operateType, __originalUserEquipmentContinuedAuthorization, __userEquipmentContinuedAuthorization, out title, out content, out contentHTML, out entityName, out entityCnName);
            return new MessageContext(Model.Enum.MsgType.EquipmentAuthorize, null, null, title, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _sender,null, new User[] { __user }, __user, __userEquipmentContinuedAuthorization.Id, null);
        }
    }
}
