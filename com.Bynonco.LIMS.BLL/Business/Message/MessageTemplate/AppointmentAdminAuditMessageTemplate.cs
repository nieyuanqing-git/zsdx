using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business.Message.MessageTemplate
{
    class AppointmentAdminAuditMessageTemplate : MessageTemplateBase
    {
      private IViewAppointmentBLL __viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();

       
        private ISendMailBLL __sendMailBLL = BLLFactory.CreateSendMailBLL();
        private IUserBLL _userbll = BLLFactory.CreateUserBLL();
        IChangePassUserMsgBLL __changePassUserMsgBLL = new ChangePassUserMsgBLL();
        List<Dictionary<string, string>> sampleDeductList = new List<Dictionary<string, string>>();//存放过滤掉的送样扣费记录
        private IUserSystemSettingBLL __userSystemSettingBLL = BLLFactory.CreateUserSystemSettingBLL();
        IViewUserWorkGroupModuleFunctionBLL viewUserWorkGroupModuleFunctionBLL = BLLFactory.CreateViewUserWorkGroupModuleFunctionBLL();
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
        private IEquipmentBLL __equipmentBLL = BLLFactory.CreateEquipmentBLL();
        private Appointment appointment;
        public AppointmentAdminAuditMessageTemplate(MessageTemplateType messageTemplateType, User sender, Appointment appointment) : base(messageTemplateType, sender)
        {

            this.appointment = appointment;


        }
        protected override MessageContext GenerateMessageContext()
        {

         


            return new MessageContext(Model.Enum.MsgType.AppointmentAdminAudit, "", "", "", "", null, _sender, appointment.User, new User[] { appointment.User }, null, appointment.id, null);
        }
       
    }
}
