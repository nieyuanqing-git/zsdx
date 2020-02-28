using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPCancelAppointmentHandler:NMPAppointmentHandlerBase
    {
        public NMPCancelAppointmentHandler(Guid nmpId, NMPAppointment nmpAppointment, Guid operatorId, string operateIP)
            : base(nmpId, new List<NMPAppointment>() { nmpAppointment }, operatorId, operateIP) { }
        public override bool Handle(out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                tran = SessionManager.Instance.BeginTransaction();
                var nmpAppointment = _nmpAppointments.First();
                nmpAppointment.StatusEnum = AppointmentStatus.Cancel;
                nmpAppointment.ApplyTime = DateTime.Now;
                _nmpAppointmentBLL.Save(new NMPAppointment[] { nmpAppointment }, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null) tran.RollbackTransaction();
                errorMessage = ex.Message;
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
        }
    }
}
