using com.august.DataLink;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPUsedConfirmBLL : BLLBase<NMPUsedConfirm>, INMPUsedConfirmBLL
    {
        private INMPChargeStandardBLL __nmpChargeStandardBLL;
        private ICostDeductBLL __costDeductBLL;
        private INMPEquipmentBLL __nmpEquipmentBLL;
        private INMPUsedConfirmAddtionChargeItemBLL __nmpUsedConfirmAddtionChargeItemBLL;
        private INMPAdditionChargeItemBLL __nmpAdditionChargeItemBLL;
        private INMPCalcFeeTimeRuleBLL __nmpCalFeeTimeRuleBLL;
        private INMPUsedConfirmFeedBackBLL __nmpUsedConfirmFeedBackBLL;
        public NMPUsedConfirmBLL()
        {
            __nmpChargeStandardBLL = BLLFactory.CreateNMPChargeStandardBLL();
            __costDeductBLL = BLLFactory.CreateCostDeductBLL();
            __nmpEquipmentBLL = BLLFactory.CreateNMPEquipmentBLL();
            __nmpUsedConfirmAddtionChargeItemBLL = BLLFactory.CreateNMPUsedConfirmAddtionChargeItemBLL();
            __nmpAdditionChargeItemBLL = BLLFactory.CreateNMPAdditionChargeItemBLL();
            __nmpCalFeeTimeRuleBLL = BLLFactory.CreateNMPCalcFeeTimeRuleBLL();
            __nmpUsedConfirmFeedBackBLL = BLLFactory.CreateNMPUsedConfirmFeedBackBLL();
        }
        public bool JudgeIsEnableEditNMPUsedConfirm(Guid? userId, IList<ViewNMPUsedConfirm> viewNMPUsedConfirms, object nmpUsedConfirmPriviliges, out string errorMessage)
        {
            return JudgeIsEnableSaveNMPUsedConfirm(userId, viewNMPUsedConfirms, nmpUsedConfirmPriviliges, out errorMessage) || JudgeIsEnableCostDeductNMPUsedConfirm(userId, viewNMPUsedConfirms, nmpUsedConfirmPriviliges, out errorMessage);
        }
        public bool JudgeIsEnableDeleteNMPUsedConfirm(Guid? userId, IList<ViewNMPUsedConfirm> viewNMPUsedConfirms, object nmpUsedConfirmPriviliges, out string errorMessage)
        {
            errorMessage = "";
            var lstNMPUsedConfirmPrivilige = (IList<NMPUsedConfirmPrivilige>)nmpUsedConfirmPriviliges;
            foreach (var viewNMPUsedConfirm in viewNMPUsedConfirms)
            {
                var nmpUsedConfirmPrivilige = lstNMPUsedConfirmPrivilige.FirstOrDefault(p => p.NMPUsedConfirmId.HasValue && p.NMPUsedConfirmId == viewNMPUsedConfirm.Id);
                if (nmpUsedConfirmPrivilige == null || !nmpUsedConfirmPrivilige.IsEnableDeleteNMPUsedConfirm)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewNMPUsedConfirm.Status == (int)UsedConfirmStatus.Deduct)
                {
                    errorMessage = "已扣费,不能删除";
                    return false;
                }
                if (viewNMPUsedConfirm.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    errorMessage = "已结算,不能删除";
                    return false;
                }

            }
            return true;
        }
        public bool JudgeIsEnableSelfSaveNMPUsedConfirm(Guid? userId, IList<ViewNMPUsedConfirm> viewNMPUsedConfirms, object nmpUsedConfirmPriviliges, out string errorMessage)
        {
            errorMessage = "";
            var lstNMPUsedConfirmPrivilige = (IList<NMPUsedConfirmPrivilige>)nmpUsedConfirmPriviliges;
            foreach (var viewNMPUsedConfirm in viewNMPUsedConfirms)
            {
                var nmpUsedConfirmPrivilige = lstNMPUsedConfirmPrivilige.FirstOrDefault(p => p.NMPUsedConfirmId.HasValue && p.NMPUsedConfirmId == viewNMPUsedConfirm.Id);
                if (nmpUsedConfirmPrivilige == null || !nmpUsedConfirmPrivilige.IsEnableSelfSaveAppointmentNMPUsedConfirm)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewNMPUsedConfirm.Status == (int)UsedConfirmStatus.Deduct)
                {
                    errorMessage = "已扣费,不能保存";
                    return false;
                }
                if (viewNMPUsedConfirm.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    errorMessage = "已结算,不能保存";
                    return false;
                }

            }
            return true;

        }
        public bool JudgeIsEnableSaveNMPUsedConfirm(Guid? userId, IList<ViewNMPUsedConfirm> viewNMPUsedConfirms, object nmpUsedConfirmPriviliges, out string errorMessage)
        {
            errorMessage = "";
            var lstNMPUsedConfirmPrivilige = (IList<NMPUsedConfirmPrivilige>)nmpUsedConfirmPriviliges;
            foreach (var viewNMPUsedConfirm in viewNMPUsedConfirms)
            {
                var nmpUsedConfirmPrivilige = lstNMPUsedConfirmPrivilige.FirstOrDefault(p => p.NMPUsedConfirmId.HasValue && p.NMPUsedConfirmId == viewNMPUsedConfirm.Id);
                if (nmpUsedConfirmPrivilige == null || !nmpUsedConfirmPrivilige.IsEnableSaveNMPUsedConfirm)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewNMPUsedConfirm.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    errorMessage = "已结算,不能保存";
                    return false;
                }

            }
            return true;

        }
        public bool JudgeIsEnableCostDeductNMPUsedConfirm(Guid? userId, IList<ViewNMPUsedConfirm> viewNMPUsedConfirms, object nmpUsedConfirmPriviliges, out string errorMessage)
        {
            errorMessage = "";
            var lstNMPUsedConfirmPrivilige = (IList<NMPUsedConfirmPrivilige>)nmpUsedConfirmPriviliges;
            foreach (var viewNMPUsedConfirm in viewNMPUsedConfirms)
            {
                var nmpUsedConfirmPrivilige = lstNMPUsedConfirmPrivilige.FirstOrDefault(p => p.NMPUsedConfirmId.HasValue && p.NMPUsedConfirmId == viewNMPUsedConfirm.Id);
                if (nmpUsedConfirmPrivilige == null || !nmpUsedConfirmPrivilige.IsEnableDirectNMPCostDeduct)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewNMPUsedConfirm.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    errorMessage = "已结算,不能扣费";
                    return false;
                }

            }
            return true;
        }
        public object GetNMPUsedConfirmPriviliges(Guid? userId, IList<ViewNMPUsedConfirm> viewNMPUsedConfirmList)
        {
            IList<NMPUsedConfirmPrivilige> lstNMPUsedConfirmPriviliges = new List<NMPUsedConfirmPrivilige>();
            if (userId.HasValue && viewNMPUsedConfirmList != null && viewNMPUsedConfirmList.Count > 0)
            {
                foreach (ViewNMPUsedConfirm viewNMPUsedConfirm in viewNMPUsedConfirmList)
                {
                    NMPUsedConfirmPrivilige nmpUsedConfirmPrivilige = lstNMPUsedConfirmPriviliges.FirstOrDefault(p => p.NMPUsedConfirmId.HasValue && p.NMPUsedConfirmId.Value == viewNMPUsedConfirm.Id);
                    if (nmpUsedConfirmPrivilige == null)
                    {
                        nmpUsedConfirmPrivilige = PriviligeFactory.GetNMPUsedConfirmPrivilige(userId.Value, viewNMPUsedConfirm.Id);
                        lstNMPUsedConfirmPriviliges.Add(nmpUsedConfirmPrivilige);
                    }
                }
            }
            return lstNMPUsedConfirmPriviliges;
        }
        public NMPUsedConfirm CreateAppointmentNMPUsedConfirm(NMPAppointment nmpAppointment, DateTime beginTime, DateTime endTime, bool isDeduct, double? fee, out string errorMessage)
        {
            errorMessage = "";
            var nmpUsedConfirm = CreatedNMPUsedConfirm(nmpAppointment.UserId.Value,
                                                 nmpAppointment.SubjectId.Value,
                                                 nmpAppointment.NMPEquipmentId.Value,
                                                 nmpAppointment.PaymentMethodEnum,
                                                 beginTime,
                                                 endTime,
                                                 (usedConfirmTemp) =>
                                                 {
                                                     usedConfirmTemp.SetNMPAppointment(nmpAppointment);
                                                 },
                                                 out errorMessage);
            nmpUsedConfirm.NMPAppointmentId = nmpAppointment.Id;
            if (fee.HasValue) nmpUsedConfirm.RealFee = fee.Value;//用户手工扣费，保存用户输入的实收费用
            GenerateRelativeAppointmentNMPUsedConfirm(nmpAppointment, nmpUsedConfirm);
            if (!isDeduct)
            {
                nmpUsedConfirm.RealFee = null;
                double duration = 0d,totalFee = 0d;
                CalNMPUsedConfirmFee(nmpUsedConfirm, out duration, out totalFee);
            }
            return nmpUsedConfirm;
        }
        public NMPUsedConfirm CreatedNMPUsedConfirm(Guid userId, Guid? subjectId, Guid nmpEquipmentId, PaymentMethod paymentMethod, DateTime beginTime, DateTime endTime, FuncPreInitNMPUsedConfirm funcPreInitNMPUsedConfirm, out string errorMessage)
        {
            errorMessage = "";
            double discount = 1;
            var nmpUsedConfirm = new NMPUsedConfirm() { Id = Guid.NewGuid(), CalcDuration = 0d, CalcFee = 0d, CreateAt = DateTime.Now, Status = (int)UsedConfirmStatus.UnComplete, RealFee = 0d };
            if (funcPreInitNMPUsedConfirm != null) funcPreInitNMPUsedConfirm(nmpUsedConfirm);
            nmpUsedConfirm.BeginAt = beginTime;
            nmpUsedConfirm.SubjectId = subjectId;
            nmpUsedConfirm.NMPEquipmentId = nmpEquipmentId;
            nmpUsedConfirm.UserId = userId;
            nmpUsedConfirm.PaymentMethodEnum = paymentMethod;
            nmpUsedConfirm.Status = (int)UsedConfirmStatus.UnComplete;
            nmpUsedConfirm.Remark = "";
            nmpUsedConfirm.IsEnableAppoitmentWithFeedBack = false;
            GenerateRelativeAppointmentNMPUsedConfirm(nmpUsedConfirm.GetNMPAppointment(), nmpUsedConfirm);
            if (nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems == null || nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems.Count == 0)
            {
                var nmpAdditionChargeItems = __nmpAdditionChargeItemBLL.GetNMPAdditionChargeItemListByNMPIdAndUserId(nmpUsedConfirm.NMPEquipment.NMPId.Value, nmpUsedConfirm.UserId, nmpUsedConfirm.SubjectId);
                if (nmpAdditionChargeItems != null && nmpAdditionChargeItems.Count > 0)
                {
                    nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems = new List<NMPUsedConfirmAddtionChargeItem>();
                    foreach (var nmpAdditionChargeItem in nmpAdditionChargeItems)
                    {
                        var usedConfirmEquipmentAddtionChargeItem = new NMPUsedConfirmAddtionChargeItem() { Id = Guid.NewGuid() };
                        usedConfirmEquipmentAddtionChargeItem.NMPUsedConfirmId = nmpUsedConfirm.Id;
                        usedConfirmEquipmentAddtionChargeItem.NMPAddtionChargeItemId = nmpAdditionChargeItem.Id;
                        usedConfirmEquipmentAddtionChargeItem.Amount = 1;
                        usedConfirmEquipmentAddtionChargeItem.Price = nmpAdditionChargeItem.StandardPrice;
                        nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems.Add(usedConfirmEquipmentAddtionChargeItem);
                    }
                }
            }
            try
            {
                var chargeStandard = __nmpChargeStandardBLL.GetNMPChargeStandardByUserId(nmpUsedConfirm.NMPEquipment.NMPId.Value, nmpUsedConfirm.UserId, nmpUsedConfirm.SubjectId, out discount);
                if (chargeStandard == null) throw new Exception("计费标准为空");
                if (nmpUsedConfirm.NMPEquipment.NMP.CalcFeeTimeRule == null) throw new Exception("计费时间规则为空");
                nmpUsedConfirm.UnitPrice = chargeStandard.StandardPrice;
                nmpUsedConfirm.ChargeTypeEnum = chargeStandard.ChargeTypeEnum;
                nmpUsedConfirm.Expression = nmpUsedConfirm.NMPEquipment.NMP.CalcFeeTimeRule.Expression;
                nmpUsedConfirm.RoundDigits = nmpUsedConfirm.NMPEquipment.NMP.CalcFeeTimeRule.RoundDigits;
                nmpUsedConfirm.RoundFator = nmpUsedConfirm.NMPEquipment.NMP.CalcFeeTimeRule.RoundFator;
                if ((endTime - beginTime).TotalMilliseconds >= 0)
                {
                    nmpUsedConfirm.Status = (int)UsedConfirmStatus.Complete;
                    nmpUsedConfirm.EndAt = endTime;
                    double calDuration = -1;
                    double? unitPrice = null;
                    nmpUsedConfirm.CalcFee = __nmpChargeStandardBLL.GetNMPUsedConfirmCalFee(nmpUsedConfirm, out calDuration, out unitPrice);
                    nmpUsedConfirm.CalcDuration = calDuration == -1d ? (nmpUsedConfirm.EndAt.Value - nmpUsedConfirm.BeginAt.Value).TotalHours : calDuration;
                    nmpUsedConfirm.RealFee = nmpUsedConfirm.CalcFee;
                }
                else
                {
                    nmpUsedConfirm.Status = (int)UsedConfirmStatus.UnComplete;
                }
            }
            catch (Exception ex) { errorMessage = ex.Message; }
            return nmpUsedConfirm;
        }
        public void CalNMPUsedConfirmFee(NMPUsedConfirm nmpUsedConfirm, out double duration, out double totalFee)
        {
            double? unitPrice = null;
            totalFee = __nmpChargeStandardBLL.GetNMPUsedConfirmCalFee(nmpUsedConfirm, out duration, out unitPrice);
            nmpUsedConfirm.RealFee = totalFee;
            nmpUsedConfirm.CalcFee = totalFee;
        }
        public void GenerateRelativeAppointmentNMPUsedConfirm(NMPAppointment nmpAppointment, NMPUsedConfirm nmpUsedConfirm)
        {
            if (nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems == null || nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems.Count == 0)
                nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems = __nmpUsedConfirmAddtionChargeItemBLL.GetEntitiesByExpression(string.Format("NMPUsedConfirmId=\"{0}\"", nmpUsedConfirm.Id));
            if ((nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems == null || nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems.Count == 0)
                 && (nmpAppointment != null && nmpAppointment.AppointmentAddtionChargeItems != null && nmpAppointment.AppointmentAddtionChargeItems.Count > 0))
            {
                nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems = new List<NMPUsedConfirmAddtionChargeItem>();
                foreach (var appointmentAddtionChargeItem in nmpAppointment.AppointmentAddtionChargeItems)
                {
                    nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems.Add(new NMPUsedConfirmAddtionChargeItem()
                    {
                        Id = Guid.NewGuid(),
                        NMPAddtionChargeItemId = appointmentAddtionChargeItem.NMPAddtionChargeItemId,
                        Price = appointmentAddtionChargeItem.Price,
                        Amount = appointmentAddtionChargeItem.Amount,
                        NMPUsedConfirmId = nmpUsedConfirm.Id
                    });
                }
            }
            
            DateTime? beginAppointmentTime = null, endAppointmentTime = null;
            if (nmpAppointment != null)
            {
                beginAppointmentTime = nmpAppointment.BeginTime;
                endAppointmentTime = nmpAppointment.EndTime;
               
            }
            var duration = nmpUsedConfirm.EndAt.HasValue ? __nmpCalFeeTimeRuleBLL.CalcFeeTime(false, nmpUsedConfirm.NMPEquipment.NMPId.Value, nmpUsedConfirm.UserId, nmpUsedConfirm.BeginAt, nmpUsedConfirm.EndAt, beginAppointmentTime, endAppointmentTime, nmpUsedConfirm.NMPEquipment.NMP.CalcFeeTimeRule.Expression) : 0d;
           
        }
        public bool Add(IEnumerable<NMPUsedConfirm> entities, ref august.DataLink.XTransaction tran, bool isWithFeedBack, out string errorMessage, bool isSupress = false)
        {
            errorMessage = "";
            try
            {
                if (!isSupress && tran != null) tran = SessionManager.Instance.BeginTransaction();
                base.Add(entities, ref tran, isSupress);
                foreach (var entity in entities)
                {
                    if (entity.NMPUsedConfirmAddtionChargeItems != null && entity.NMPUsedConfirmAddtionChargeItems.Count > 0)
                    {
                        foreach (var nmpUsedConfirmAddtionChargeItem in entity.NMPUsedConfirmAddtionChargeItems)
                            nmpUsedConfirmAddtionChargeItem.NMPUsedConfirmId = entity.Id;
                        __nmpUsedConfirmAddtionChargeItemBLL.Add(entity.NMPUsedConfirmAddtionChargeItems, ref tran, isSupress);
                    }
                  
                    if (isWithFeedBack && entity.FeedBack != null)
                    {
                        __nmpUsedConfirmFeedBackBLL.Add(new NMPUsedConfirmFeedBack[] { entity.FeedBack }, ref tran, isSupress);
                    }
                }
                if (!isSupress) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress) tran.Dispose(); }
        }
        public override bool Add(IEnumerable<NMPUsedConfirm> entities, ref august.DataLink.XTransaction tran, bool isSupress = false)
        {
            string errorMessage = "";
            return Add(entities, ref tran, false, out errorMessage, isSupress);
        }
        public bool Save(IEnumerable<NMPUsedConfirm> entities, ref XTransaction tran, bool isWithFeedBack, out string errorMessage, bool isSupress = false)
        {
            errorMessage = "";
            try
            {
                if (!isSupress && tran != null) tran = SessionManager.Instance.BeginTransaction();
                base.Save(entities, ref tran, isSupress);
                foreach (var entity in entities)
                {
                    var nmpUsedConfirmAddtionChargeItemList = __nmpUsedConfirmAddtionChargeItemBLL.GetEntitiesByExpression(string.Format("NMPUsedConfirmId=\"{0}\"", entity.Id));
                    if (nmpUsedConfirmAddtionChargeItemList != null && nmpUsedConfirmAddtionChargeItemList.Count > 0)
                        __nmpUsedConfirmAddtionChargeItemBLL.Delete(nmpUsedConfirmAddtionChargeItemList.Select(p => p.Id), ref tran, isSupress);
                    if (isWithFeedBack)
                    {
                        var usedConfirmFeedBackList = __nmpUsedConfirmFeedBackBLL.GetEntitiesByExpression(string.Format("NMPUsedConfirmId=\"{0}\"", entity.Id));
                        if (usedConfirmFeedBackList != null && usedConfirmFeedBackList.Count > 0)
                        {
                            __nmpUsedConfirmFeedBackBLL.Delete(usedConfirmFeedBackList.Select(p => p.Id), ref tran, isSupress);
                        }
                        entity.FeedBack.NMPUsedConfirmId = entity.Id;
                        entity.FeedBack.Id = Guid.NewGuid();
                        __nmpUsedConfirmFeedBackBLL.Add(new NMPUsedConfirmFeedBack[] { entity.FeedBack }, ref tran, isSupress);
                    }
                    if (entity.NMPUsedConfirmAddtionChargeItems != null && entity.NMPUsedConfirmAddtionChargeItems.Count > 0)
                    {
                        foreach (var nmpUsedConfirmAddtionChargeItem in entity.NMPUsedConfirmAddtionChargeItems)
                            nmpUsedConfirmAddtionChargeItem.NMPUsedConfirmId = entity.Id;
                        __nmpUsedConfirmAddtionChargeItemBLL.Add(entity.NMPUsedConfirmAddtionChargeItems, ref tran, isSupress);
                    }
                }
                if (!isSupress) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress) tran.Dispose(); }
        }
        public override bool Save(IEnumerable<NMPUsedConfirm> entities, ref XTransaction tran, bool isSupress = false)
        {
            string errorMessage = "";
            return Save(entities, ref tran, false, out errorMessage, isSupress);
        }
        public override bool Delete(IEnumerable<Guid> ids, ref XTransaction tran, bool isSupress = false)
        {
            try
            {
                if (!isSupress && tran != null) tran = SessionManager.Instance.BeginTransaction();
                var nmpUsedConfirmAddtionChargeItemList = __nmpUsedConfirmAddtionChargeItemBLL.GetEntitiesByExpression(ids.ToFormatStr("NMPUsedConfirmId"));
                if (nmpUsedConfirmAddtionChargeItemList != null && nmpUsedConfirmAddtionChargeItemList.Count > 0)
                    __nmpUsedConfirmAddtionChargeItemBLL.Delete(nmpUsedConfirmAddtionChargeItemList.Select(p => p.Id), ref tran, true);
                base.Delete(ids, ref tran, isSupress);
                if (!isSupress) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress) tran.Dispose(); }
        }
    }
}
