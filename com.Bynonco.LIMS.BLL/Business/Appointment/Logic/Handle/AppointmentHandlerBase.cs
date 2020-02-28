using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Business.Message.MessageTemplate;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class AppointmentHandlerBase
    {
        protected ISubjectBLL _subjectBLL;
        protected ICostDeductBLL _costDeductBLL;
        protected IUserAccountBLL _userAccountBLL;
        protected IEquipmentBLL _equipmentBLL;
        protected IAppointmentBLL _appointmentBLL;
        protected IAppointmentEquipmentAddtionChargeItemBLL _appointmentEquipmentAddtionChargeItemBLL;
        protected IExperimenterSubjectBLL _experimenterSubjectBLL;
        protected IAppointmentEquipmentPartBLL _appointmentEquipmentPartBLL;
        protected IAppointmentEquipmentUseConditionBLL _appointmentEquipmentUseConditionBLL;
        protected IViewOpenFundDetailBLL _viewOpenFundDetailBLL;
        protected AppointmentMidiator _appointmentMidiator;
        protected IMessageHandler _messageHandler;
        protected ICostDeductManager _costDeductManager;
        protected Guid _operatorId;
        protected string _operateIP;
        private EmailMessageSender _emailMessageSender;
        public AppointmentHandlerBase(AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP)
        {
            _subjectBLL = BLLFactory.CreateSubjectBLL();
            _costDeductBLL = BLLFactory.CreateCostDeductBLL();
            _userAccountBLL = BLLFactory.CreateUserAccountBLL();
            _equipmentBLL = BLLFactory.CreateEquipmentBLL();
            _appointmentBLL = BLLFactory.CreateAppointmentBLL();
            _appointmentEquipmentAddtionChargeItemBLL = BLLFactory.CreateAppointmentEquipmentAddtionChargeItemBLL();
            _appointmentEquipmentPartBLL = BLLFactory.CreateAppointmentEquipmentPartBLL();
            _experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            _appointmentEquipmentUseConditionBLL = BLLFactory.CreateAppointmentEquipmentUseConditionBLL();
            _viewOpenFundDetailBLL = BLLFactory.CreateViewOpenFundDetailBLL();
            _messageHandler = new MessageHandler();
            _costDeductManager = new CostDeductManager();
            _appointmentMidiator = appointmentMidiator;
            _operatorId = operatorId;
            _operateIP = operateIP;
            _emailMessageSender = new EmailMessageSender();
        }
        public abstract bool Handle(out string errorMessage);
        protected void SendAppointmentTutorAuditNotice()
        {
            if (_appointmentMidiator.NewAppointments != null && _appointmentMidiator.NewAppointments.Count() > 0)
            {
                foreach (var appointment in _appointmentMidiator.NewAppointments)
                {
                    if (appointment.StatusEnum == Model.Enum.AppointmentStatus.Waitting &&
                        appointment.IsNeedTutorAudit &&
                        appointment.TutorAuditStatusEnum == Model.Enum.TutorAuditStatus.WaitingAudit &&
                        appointment.User.Tutor != null)
                    {
                        _messageHandler.SendAppointmentTutorAuditNotice(appointment, null);
                    }
                }
            }
            if (_appointmentMidiator.ChangeNewAppointment != null)
            {
                _messageHandler.SendAppointmentSuccessNotice(_appointmentMidiator.ChangeNewAppointment, null);
            }
        }
        protected void SendAppointmentAdminAuditNotice()
        {
            if (_appointmentMidiator.NewAppointments != null && _appointmentMidiator.NewAppointments.Count() > 0)
            {
                foreach (var appointment in _appointmentMidiator.NewAppointments)
                {
                    if (appointment.StatusEnum == Model.Enum.AppointmentStatus.Waitting &&
                        (bool)appointment.IsNeedAudit)
                    {
                        AppointmentAdminAuditMessageTemplate messageTemplate = new AppointmentAdminAuditMessageTemplate(MessageTemplateType.HTML, null, appointment);
                        _emailMessageSender.SendMessage(messageTemplate);
                    }
                }
            }

        }

        protected void SendAppointmentSuccessNotice()
        {
            if (_appointmentMidiator.NewAppointments != null && _appointmentMidiator.NewAppointments.Count() > 0)
            {
                foreach (var appointment in _appointmentMidiator.NewAppointments)
                {
                    if (appointment.StatusEnum == Model.Enum.AppointmentStatus.Waitting)
                    {
                        _messageHandler.SendAppointmentSuccessNotice(appointment, null);
                    }
                }
            }
            if (_appointmentMidiator.ChangeNewAppointment != null)
            {
                _messageHandler.SendAppointmentSuccessNotice(_appointmentMidiator.ChangeNewAppointment, null);
            }
        }
        protected void SendDepositNotice(Appointment appointment, UserAccount userAccount)
        {
            if (userAccount != null) _messageHandler.SendDepositNotice(userAccount, null);
            var experimenterSubject = _experimenterSubjectBLL.GetExperimenterSubject(appointment.SubjectId.Value, appointment.UserId.Value);
            if (experimenterSubject != null) _messageHandler.SendDepositNotice(experimenterSubject, null);
        }
    }
}
