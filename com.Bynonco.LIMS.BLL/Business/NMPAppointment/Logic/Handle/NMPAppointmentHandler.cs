using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business.AppointmentRule;
using com.Bynonco.LIMS.Model.Business;
using com.august.DataLink;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPAppointmentHandler :NMPAppointmentHandlerBase
    {
        public NMPAppointmentHandler(Guid nmpId, IEnumerable<NMPAppointment> nmpAppointments, Guid operatorId, string operateIP)
            : base(nmpId,nmpAppointments, operatorId, operateIP) { }

        public override bool Handle(out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                tran = SessionManager.Instance.BeginTransaction();
                double realCurrency = 0d, virtualCurrency = 0d;
                var firstAppointment = _nmpAppointments.First();
                UserAccount userAccount = _userAccountBLL.Deduct(false, firstAppointment.UserId.Value, firstAppointment.SubjectId.Value, firstAppointment.PaymentMethodEnum, 0, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false);
                foreach (var nmpAppointment in _nmpAppointments)
                    _nmpAppointmentBLL.Add(new NMPAppointment[] { nmpAppointment }, ref tran, true);
                tran.CommitTransaction();
                SendNMPAppointmentTutorAuditNotice();
                SendNMPAppointmentSuccessNotice();
                SendDepositNotice(firstAppointment, userAccount);
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
