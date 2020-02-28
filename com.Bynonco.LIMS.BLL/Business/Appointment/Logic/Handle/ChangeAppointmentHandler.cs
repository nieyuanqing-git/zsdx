using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.august.DataLink;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class ChangeAppointmentHandler : AppointmentHandlerBase
    {
        public ChangeAppointmentHandler(AppointmentMidiator appointmentMidiator, Guid operatorId,string operateIP)
            : base(appointmentMidiator, operatorId, operateIP) { }
        public override bool Handle(out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                if (_appointmentMidiator.ChangeOldAppointment.HasClossingAccount) throw new Exception("已结算");
                tran = SessionManager.Instance.BeginTransaction();
                DateTime endTime = _appointmentMidiator.ChangeOldAppointment.EndTime.Value > _appointmentMidiator.ChangeNewAppointment.EndTime.Value ? _appointmentMidiator.ChangeNewAppointment.EndTime.Value : _appointmentMidiator.ChangeOldAppointment.EndTime.Value;
                var payerId = _appointmentMidiator.ChangeNewAppointment.Subject.SubjectDirectorId.Value;
                var viewOpenFundDetailList = _viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(new Guid[] { _appointmentMidiator.ChangeNewAppointment.EquipmentId.Value, _appointmentMidiator.ChangeOldAppointment.EquipmentId.Value }, payerId, endTime);
                bool isDeductSubject = _appointmentMidiator.ChangeNewAppointment.SubjectId.Value != _appointmentMidiator.ChangeOldAppointment.SubjectId.Value;
                double realCurrency = 0d, virtualCurrency = 0d;
                //扣取0元获取账户,注意考虑改约前后不同帐户的情况
                var userAccount = _userAccountBLL.Deduct(false,_appointmentMidiator.ChangeNewAppointment.UserId.Value,
                                                         _appointmentMidiator.ChangeNewAppointment.SubjectId.Value,
                                                         _appointmentMidiator.ChangeNewAppointment.PaymentMethodEnum,
                                                         0, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false);
                UserAccount appointmentUserAccount = _userAccountBLL.Deduct(false, _appointmentMidiator.ChangeNewAppointment.UserId.Value,
                                                                            _appointmentMidiator.ChangeNewAppointment.SubjectId.Value,
                                                                            _appointmentMidiator.ChangeNewAppointment.PaymentMethodEnum,
                                                                            0, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false);
                UserAccount cancelUserAccount = _userAccountBLL.Deduct(false, _appointmentMidiator.ChangeOldAppointment.UserId.Value, _appointmentMidiator.ChangeOldAppointment.SubjectId.Value, _appointmentMidiator.ChangeOldAppointment.PaymentMethodEnum, 0, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false);
                double cancelRealCurrency = _appointmentMidiator.ChangeOldAppointment.RealCurrency.Value;
                double cancelVirtualCurrency = _appointmentMidiator.ChangeOldAppointment.VirtualCurrency.Value;
                userAccount.RealCurrency += cancelRealCurrency;
                userAccount.VirtualCurrency += cancelVirtualCurrency;
                var oldPredictCostDeduct = _costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"", _appointmentMidiator.ChangeOldAppointment.Id.ToString()));
                var oldIsPredictCostDeduct = _appointmentMidiator.ChangeOldAppointment.IsPredictCostDeduct.HasValue && _appointmentMidiator.ChangeOldAppointment.IsPredictCostDeduct.Value;
                if (((_appointmentMidiator.ChangeOldAppointment.IsNeedTutorAudit && _appointmentMidiator.ChangeOldAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed) || (_appointmentMidiator.ChangeOldAppointment.IsNeedAudit.HasValue && _appointmentMidiator.ChangeOldAppointment.IsNeedAudit.Value)) && oldPredictCostDeduct == null)
                    oldIsPredictCostDeduct = false;
                var newIsPredictCostDeduct = _appointmentMidiator.ChangeNewAppointment.IsPredictCostDeduct.HasValue && _appointmentMidiator.ChangeNewAppointment.IsPredictCostDeduct.Value;
                if ((_appointmentMidiator.ChangeNewAppointment.IsNeedTutorAudit && _appointmentMidiator.ChangeNewAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed) || (_appointmentMidiator.ChangeNewAppointment.IsNeedAudit.HasValue && _appointmentMidiator.ChangeNewAppointment.IsNeedAudit.Value))
                    newIsPredictCostDeduct = false;
                var isSupressSaveEquipmentOpenFund = oldIsPredictCostDeduct && newIsPredictCostDeduct && cancelUserAccount.Id == userAccount.Id;
                if (oldIsPredictCostDeduct)
                {
                    cancelUserAccount = _costDeductManager.CancelAppointmentPredictDeduct(_appointmentMidiator.ChangeOldAppointment, ref tran, _operatorId,_operateIP,isDeductSubject, !isSupressSaveEquipmentOpenFund);
                    if (cancelUserAccount != null && cancelUserAccount.Id != appointmentUserAccount.Id)
                        _userAccountBLL.Save(new UserAccount[] { cancelUserAccount }, ref tran, true);
                    _appointmentMidiator.ChangeOldAppointment.VirtualCurrency = 0d;
                    _appointmentMidiator.ChangeOldAppointment.RealCurrency = 0d;
                }
                _appointmentMidiator.ChangeOldAppointment.StatusEnum = AppointmentStatus.Changed;
                _appointmentMidiator.ChangeOldAppointment.ApplyTime = DateTime.Now;
                _appointmentMidiator.ChangeOldAppointment.ChangedTime = DateTime.Now;
                _appointmentMidiator.ChangeOldAppointment.ChangerId = _operatorId;
                _appointmentMidiator.ChangeOldAppointment.SampleApplyId = null;
                _appointmentMidiator.ChangeOldAppointment.ChangedApppointmentId = _appointmentMidiator.ChangeNewAppointment.Id;
                _appointmentBLL.Save(new Appointment[] { _appointmentMidiator.ChangeOldAppointment }, ref tran, true);
                _appointmentBLL.Add(new Appointment[] { _appointmentMidiator.ChangeNewAppointment }, ref tran, true);
                if (newIsPredictCostDeduct)
                {
                   
                    IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
                    if (isSupressSaveEquipmentOpenFund && _appointmentMidiator.ChangeOldAppointment.PredictCostDeduct != null)
                    {
                        costDeductEquipmentOpenFunds = _appointmentMidiator.ChangeOldAppointment.PredictCostDeduct.CostDeductEquipmentOpenFunds;
                        if (viewOpenFundDetailList != null && viewOpenFundDetailList.Count > 0 && costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Count > 0)
                        {
                            foreach (var costDeductEquipmentOpenFund in costDeductEquipmentOpenFunds)
                            {
                                var viewOpenFundDetailListFind = viewOpenFundDetailList.Where(p => p.OpenFundApplyId == costDeductEquipmentOpenFund.OpenFundApplyId);
                                if (viewOpenFundDetailListFind != null && viewOpenFundDetailListFind.Count() > 0)
                                {
                                    foreach (var viewOpenFundDetailFind in viewOpenFundDetailListFind)
                                    {
                                        viewOpenFundDetailFind.BalanceMoney += costDeductEquipmentOpenFund.CalFee;
                                    }
                                }
                            }
                        }
                    }
                    
                    if (appointmentUserAccount.Id == cancelUserAccount.Id)
                    {
                        appointmentUserAccount.RealCurrency += cancelRealCurrency;
                        appointmentUserAccount.VirtualCurrency += cancelVirtualCurrency;
                    }
                    //appointmentUserAccount.Deduct(_appointmentMidiator.ChangeNewAppointment.IsOpenFundCostDeduct, _appointmentMidiator.ChangeNewAppointment.Fee.Value, out realCurrency, out virtualCurrency);
                    var costDeduct = _costDeductManager.AppointmentPredictDeduct(_appointmentMidiator.ChangeNewAppointment, viewOpenFundDetailList, appointmentUserAccount, ref tran,_operatorId,_operateIP, isDeductSubject, costDeductEquipmentOpenFunds);
                    appointmentUserAccount = _userAccountBLL.Deduct(costDeduct.IsOpenFundCostDeduct, _appointmentMidiator.ChangeNewAppointment.UserId.Value,
                                                                    _appointmentMidiator.ChangeNewAppointment.SubjectId.Value,
                                                                    _appointmentMidiator.ChangeNewAppointment.PaymentMethodEnum,
                                                                    costDeduct.RealCurrency.Value + costDeduct.VirtualCurrency.Value,
                                                                    out realCurrency, out virtualCurrency, ref tran, out errorMessage, false, isDeductSubject);
                    _appointmentMidiator.ChangeNewAppointment.RealCurrency = realCurrency;
                    _appointmentMidiator.ChangeNewAppointment.VirtualCurrency = virtualCurrency;
                    if (cancelUserAccount != null && cancelUserAccount.Id != appointmentUserAccount.Id)
                        _userAccountBLL.Save(new UserAccount[] { appointmentUserAccount }, ref tran, true);
                }
                if (appointmentUserAccount.Id == cancelUserAccount.Id && (newIsPredictCostDeduct || oldIsPredictCostDeduct))
                {
                    if (newIsPredictCostDeduct)
                        userAccount = userAccount.Deduct(_appointmentMidiator.ChangeNewAppointment.IsOpenFundCostDeduct, _appointmentMidiator.ChangeNewAppointment.Fee.Value, out realCurrency, out virtualCurrency);
                    _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, true);
                }
                if (!isDeductSubject)
                {
                    var fee = _appointmentMidiator.ChangeNewAppointment.Fee.Value - _appointmentMidiator.ChangeOldAppointment.Fee.Value; 
                    _subjectBLL.Deduct(_appointmentMidiator.ChangeNewAppointment.UserId.Value,
                                        _appointmentMidiator.ChangeNewAppointment.SubjectId.Value,
                                        _appointmentMidiator.ChangeNewAppointment.PaymentMethodEnum,
                                        fee, ref tran);
                }
                tran.CommitTransaction();
                SendAppointmentTutorAuditNotice();
                SendAppointmentSuccessNotice();
                SendDepositNotice(_appointmentMidiator.ChangeNewAppointment, userAccount);
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
