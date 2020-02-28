using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class AppointmentMidiator
    {
        private IAppointmentBLL __appointmentBLL;
        private IEnumerable<Appointment> __newAppointments;
        private Appointment __canceledAppointment;
        private Appointment __changeOldAppointment;
        private Appointment __changeNewAppointment;
        private bool __isAutoCancel = false;
        private AppointmentMidiator()
        {
            __appointmentBLL = BLLFactory.CreateAppointmentBLL();
        }
        public AppointmentMidiator(IEnumerable<Appointment> newAppointments)
            :this()
        {
            __newAppointments = newAppointments;
        }
        public AppointmentMidiator(Appointment canceledAppointment, bool isAutoCancel)
            : this()
        {
            this.__canceledAppointment = canceledAppointment;
            __isAutoCancel = isAutoCancel;
        }
        public AppointmentMidiator(Guid canceledAppointmentId,bool isAutoCancel)
            : this()
        {
            this.__canceledAppointment = __appointmentBLL.GetEntityById(canceledAppointmentId);
            __isAutoCancel = isAutoCancel;
        }
        public AppointmentMidiator(Appointment changeOldAppointment, Appointment changeNewAppointment)
            : this()
        {
            this.__changeOldAppointment = changeOldAppointment;
            this.__changeNewAppointment = changeNewAppointment;
        }
        public AppointmentMidiator(Guid changeOldAppointmentId, Appointment changeNewAppointment)
            : this()
        {
            this.__changeOldAppointment = __appointmentBLL.GetEntityById(changeOldAppointmentId);
            this.__changeNewAppointment = changeNewAppointment;
        }
        public IEnumerable<Appointment> NewAppointments
        {
            get{return __newAppointments;}
        }
        public Appointment CanceledAppointment
        {
            get{return __canceledAppointment;}
        }
        public Appointment ChangeOldAppointment
        {
            get { return __changeOldAppointment; }
        }
        public Appointment ChangeNewAppointment
        {
            get { return __changeNewAppointment; }
        }
        public bool IsAutoCancel 
        {
            get { return __isAutoCancel; }
        }
    }
}
