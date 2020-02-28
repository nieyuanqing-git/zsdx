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
    public class AppointmentHandler : AppointmentHandlerBase
    {
        public AppointmentHandler(AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP)
            : base(appointmentMidiator, operatorId, operateIP) { }

        public override bool Handle(out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                tran = SessionManager.Instance.BeginTransaction();
                double realCurrency = 0d, virtualCurrency = 0d;
                var firstAppointment = _appointmentMidiator.NewAppointments.First();
                var isNeedPredictCostDeduct = firstAppointment.IsPredictCostDeduct.HasValue && firstAppointment.IsPredictCostDeduct.Value;
                if ((firstAppointment.IsNeedTutorAudit && firstAppointment.TutorAuditStatusEnum!= TutorAuditStatus.Passed) || (firstAppointment.IsNeedAudit.HasValue && firstAppointment.IsNeedAudit.Value)) isNeedPredictCostDeduct = false;
                //扣取0元获取账户
                UserAccount userAccount = _userAccountBLL.Deduct(firstAppointment.IsOpenFundCostDeduct, firstAppointment.UserId.Value, firstAppointment.SubjectId.Value, firstAppointment.PaymentMethodEnum, 0, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false);
                IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
                IList<ViewOpenFundDetail> viewOpenFundDetailList = null;
                if (isNeedPredictCostDeduct)
                    viewOpenFundDetailList = _viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(firstAppointment.EquipmentId.Value, firstAppointment.Subject.SubjectDirectorId.Value, _appointmentMidiator.NewAppointments.Min(p => p.EndTime.Value));
                foreach (var appointment in _appointmentMidiator.NewAppointments)
                {
                    if (isNeedPredictCostDeduct)
                    {
                        bool isSaveEquipmentOpenFun = appointment.Id==_appointmentMidiator.NewAppointments.Last().Id;
                        var costDeduct = _costDeductManager.AppointmentPredictDeduct(appointment, viewOpenFundDetailList, userAccount, ref tran, _operatorId,_operateIP,true, costDeductEquipmentOpenFunds, isSaveEquipmentOpenFun);
                        if (costDeduct.CostDeductEquipmentOpenFunds != null && costDeduct.CostDeductEquipmentOpenFunds.Count > 0)
                        {
                            foreach (var costDeductEquipmentOpenFund in costDeduct.CostDeductEquipmentOpenFunds)
                            {
                                costDeductEquipmentOpenFunds.Add(new CostDeductEquipmentOpenFund()
                                {
                                    Id = costDeductEquipmentOpenFund.Id,
                                    CalFee = costDeductEquipmentOpenFund.CalFee * -1,
                                    OpenFundApplyId = costDeductEquipmentOpenFund.OpenFundApplyId,
                                    CostDeductId = costDeductEquipmentOpenFund.CostDeductId
                                });
                            }
                        }
                        //userAccount.Deduct(costDeduct.IsOpenFundCostDeduct, appointment.Fee.Value, out realCurrency, out virtualCurrency);
                        //appointment.RealCurrency = costDeduct.VirtualCurrency.Value;
                        //appointment.VirtualCurrency = costDeduct.RealCurrency.Value;
                        //costDeducts.Add(costDeduct);
                    }
                     _appointmentBLL.Add(new Appointment[] { appointment }, ref tran, true);
                    
                }
                if (isNeedPredictCostDeduct)
                {
                    _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, true);
                }
                tran.CommitTransaction();
                SendAppointmentTutorAuditNotice();
                SendAppointmentSuccessNotice();
                SendAppointmentAdminAuditNotice();
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
