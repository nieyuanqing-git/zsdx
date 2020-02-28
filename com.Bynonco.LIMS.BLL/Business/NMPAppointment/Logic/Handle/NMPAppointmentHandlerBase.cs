using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL.Business
{
    public abstract class NMPAppointmentHandlerBase
    {
        protected ISubjectBLL _subjectBLL;
        protected IUserAccountBLL _userAccountBLL;
        protected INMPBLL _nmpBLL;
        protected INMPAppointmentBLL _nmpAppointmentBLL;
        protected INMPAppointmentAddtionChargeItemBLL _nmpAddtionChargeItemBLL;
        protected IExperimenterSubjectBLL _experimenterSubjectBLL;
        protected IEnumerable<NMPAppointment> _nmpAppointments;
        protected IMessageHandler _messageHandler;
        protected Guid _nmpId;
        protected Guid _operatorId;
        protected string _operateIP;
        public NMPAppointmentHandlerBase(Guid nmpId,IEnumerable<NMPAppointment> nmpAppointments, Guid operatorId, string operateIP)
        {
            _subjectBLL = BLLFactory.CreateSubjectBLL();
            _userAccountBLL = BLLFactory.CreateUserAccountBLL();
            _nmpBLL = BLLFactory.CreateNMPBLL();
            _nmpAppointmentBLL = BLLFactory.CreateNMPAppointmentBLL();
            _nmpAddtionChargeItemBLL = BLLFactory.CreateNMPAppointmentAddtionChargeItemBLL();
            _experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            _messageHandler = new MessageHandler();
            this._nmpId = nmpId;
            _nmpAppointments = nmpAppointments;
            _operatorId = operatorId;
            _operateIP = operateIP;
        }
        public abstract bool Handle(out string errorMessage);
        protected void SendNMPAppointmentTutorAuditNotice()
        {
            if (_nmpAppointments != null && _nmpAppointments.Count() > 0)
            {
                foreach (var appointment in _nmpAppointments)
                {
                    if (appointment.StatusEnum == Model.Enum.AppointmentStatus.Waitting &&
                        appointment.IsNeedTutorAudit &&
                        appointment.TutorAuditStatusEnum == Model.Enum.TutorAuditStatus.WaitingAudit &&
                        appointment.User.Tutor != null)
                    {
                        _messageHandler.SendNMPAppointmentTutorAuditNotice(appointment, null);
                    }
                }
            }
        }
        protected void SendNMPAppointmentSuccessNotice()
        {
            if (_nmpAppointments != null && _nmpAppointments.Count() > 0)
            {
                foreach (var appointment in _nmpAppointments)
                {
                    if (appointment.StatusEnum == Model.Enum.AppointmentStatus.Waitting)
                    {
                        _messageHandler.SendNMPAppointmentSuccessNotice(appointment, null);
                    }
                }
            }
        }
        protected void SendDepositNotice(NMPAppointment appointment, UserAccount userAccount)
        {
            if (userAccount != null) _messageHandler.SendDepositNotice(userAccount, null);
            var experimenterSubject = _experimenterSubjectBLL.GetExperimenterSubject(appointment.SubjectId.Value, appointment.UserId.Value);
            if (experimenterSubject != null) _messageHandler.SendDepositNotice(experimenterSubject, null);
        }
    }
}
