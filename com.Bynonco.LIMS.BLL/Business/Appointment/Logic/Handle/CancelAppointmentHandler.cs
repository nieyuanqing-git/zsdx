using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class CancelAppointmentHandler:AppointmentHandlerBase
    {
        public CancelAppointmentHandler(AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP)
            : base(appointmentMidiator, operatorId, operateIP) { }
        public override bool Handle(out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                tran = SessionManager.Instance.BeginTransaction();
                var predictCostDeduct = _costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"", _appointmentMidiator.CanceledAppointment.Id.ToString()));
                if (_appointmentMidiator.CanceledAppointment.IsPredictCostDeduct.HasValue &&
                    _appointmentMidiator.CanceledAppointment.IsPredictCostDeduct.Value &&
                    predictCostDeduct != null)
                {
                    if (_appointmentMidiator.CanceledAppointment.HasClossingAccount) throw new Exception("已结算");
                    var userAccount = _costDeductManager.CancelAppointmentPredictDeduct(_appointmentMidiator.CanceledAppointment, ref tran,_operatorId,_operateIP);
                    if (userAccount != null) _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, true);
                    _appointmentMidiator.CanceledAppointment.RealCurrency = 0d;
                    _appointmentMidiator.CanceledAppointment.VirtualCurrency = 0d;
                }
                // chao: 取消预约
                _appointmentMidiator.CanceledAppointment.StatusEnum = AppointmentStatus.Cancel;
                _appointmentMidiator.CanceledAppointment.ApplyTime = DateTime.Now;
                _appointmentMidiator.CanceledAppointment.ChangedTime = DateTime.Now;
                _appointmentMidiator.CanceledAppointment.ChangerId = _operatorId;
                _appointmentMidiator.CanceledAppointment.SampleApplyId = null;
                _appointmentBLL.Save(new Appointment[] { _appointmentMidiator.CanceledAppointment }, ref tran, true);
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
