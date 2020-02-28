using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    
    public class UsedConfirmBLL : BLLBase<UsedConfirm>, IUsedConfirmBLL
    {
        private IChargeStandardBLL __chargeStandardBLL;
        private ICostDeductBLL __costDeductBLL;
        private IEquipmentBLL __equipmentBLL;
        private IUsedConfirmEquipmentAddtionChargeItemBLL __usedConfirmEquipmentAddtionChargeItemBLL;
        private IUsedConfirmEquipmentPartBLL __usedConfirmEquipmentPartBLL;
        private IUsedConfirmEquipmentUseConditionBLL __usedConfirmEquipmentUseConditionBLL;
        private IEquipmentAdditionChargeItemBLL __equipmentAdditionChargeItemBLL;
        private ICalcFeeTimeRuleBLL __calFeeTimeRuleBLL;
        private IUsedConfirmFeedBackBLL __usedConfirmFeedBackBLL;
        private ISubjectProjectBLL __subjectProjectBLL;
        public UsedConfirmBLL()
        { 
            __chargeStandardBLL = BLLFactory.CreateChargeStandardBLL();
            __costDeductBLL = BLLFactory.CreateCostDeductBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __usedConfirmEquipmentAddtionChargeItemBLL = BLLFactory.CreateUsedConfirmEquipmentAddtionChargeItemBLL();
            __usedConfirmEquipmentPartBLL = BLLFactory.CreateUsedConfirmEquipmentPartBLL();
            __usedConfirmEquipmentUseConditionBLL = BLLFactory.CreateUsedConfirmEquipmentUseConditionBLL();
            __equipmentAdditionChargeItemBLL = BLLFactory.CreateEquipmentAdditionChargeItemBLL();
            __calFeeTimeRuleBLL = BLLFactory.CreateCalcFeeTimeRuleBLL();
            __usedConfirmFeedBackBLL = BLLFactory.CreateUsedConfirmFeedBackBLL();
            __subjectProjectBLL = BLLFactory.CreateSubjectProjectBLL();
        }
        public bool Add(IEnumerable<UsedConfirm> entities, ref august.DataLink.XTransaction tran, bool isWithFeedBack,out string errorMessage, bool isSupress = false)
        {
            errorMessage = "";
            try
            {
                if (!isSupress && tran != null) tran = SessionManager.Instance.BeginTransaction();
                base.Add(entities, ref tran, isSupress);
                foreach (var entity in entities)
                {
                    if (entity.UsedConfirmEquipmentAddtionChargeItems != null && entity.UsedConfirmEquipmentAddtionChargeItems.Count > 0)
                    {
                        foreach (var usedConfirmEquipmentAddtionChargeItem in entity.UsedConfirmEquipmentAddtionChargeItems)
                            usedConfirmEquipmentAddtionChargeItem.UsedConfirmId = entity.Id;
                        __usedConfirmEquipmentAddtionChargeItemBLL.Add(entity.UsedConfirmEquipmentAddtionChargeItems, ref tran, isSupress);
                    }
                    if (entity.UsedConfirmEquipmentParts != null && entity.UsedConfirmEquipmentParts.Count > 0)
                    {
                        foreach (var usedConfirmEquipmentPart in entity.UsedConfirmEquipmentParts)
                            usedConfirmEquipmentPart.UsedConfirmId = entity.Id;
                        __usedConfirmEquipmentPartBLL.Add(entity.UsedConfirmEquipmentParts, ref tran, isSupress);
                    }
                    if (entity.UsedConfirmEquipmentUseConditions != null && entity.UsedConfirmEquipmentUseConditions.Count > 0)
                    {
                        foreach (var usedConfirmEquipmentUseCondition in entity.UsedConfirmEquipmentUseConditions)
                            usedConfirmEquipmentUseCondition.UsedConfirmId = entity.Id;
                        __usedConfirmEquipmentUseConditionBLL.Add(entity.UsedConfirmEquipmentUseConditions, ref tran, isSupress);
                    }
                    if (isWithFeedBack && entity.FeedBack != null)
                    {
                        __usedConfirmFeedBackBLL.Add(new UsedConfirmFeedBack[] { entity.FeedBack }, ref tran, isSupress);
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
        public override bool Add(IEnumerable<UsedConfirm> entities, ref august.DataLink.XTransaction tran, bool isSupress = false)
        {
            string errorMessage = "";
            return Add(entities, ref tran, false, out errorMessage, isSupress);
        }
        public bool Save(IEnumerable<UsedConfirm> entities, ref XTransaction tran, bool isWithFeedBack, out string errorMessage, bool isSupress = false)
        {
            errorMessage = "";
            try
            {
                if (!isSupress && tran != null) tran = SessionManager.Instance.BeginTransaction();
                base.Save(entities, ref tran, isSupress);
                foreach (var entity in entities)
                {
                    var usedConfirmEquipmentAddtionChargeItemList = __usedConfirmEquipmentAddtionChargeItemBLL.GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", entity.Id));
                    if (usedConfirmEquipmentAddtionChargeItemList != null && usedConfirmEquipmentAddtionChargeItemList.Count > 0)
                        __usedConfirmEquipmentAddtionChargeItemBLL.Delete(usedConfirmEquipmentAddtionChargeItemList.Select(p => p.Id), ref tran, isSupress);
                    var usedConfirmEquipmentPartList = __usedConfirmEquipmentPartBLL.GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", entity.Id));
                    if (usedConfirmEquipmentPartList != null && usedConfirmEquipmentPartList.Count > 0)
                        __usedConfirmEquipmentPartBLL.Delete(usedConfirmEquipmentPartList.Select(p => p.Id), ref tran, isSupress);
                    var usedConfirmEquipmentUseConditionList = __usedConfirmEquipmentUseConditionBLL.GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", entity.Id));
                    if (usedConfirmEquipmentUseConditionList != null && usedConfirmEquipmentUseConditionList.Count > 0)
                        __usedConfirmEquipmentUseConditionBLL.Delete(usedConfirmEquipmentUseConditionList.Select(p => p.Id), ref tran, isSupress);

                    if (isWithFeedBack)
                    {
                        var usedConfirmFeedBackList = __usedConfirmFeedBackBLL.GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", entity.Id));
                        if (usedConfirmFeedBackList != null && usedConfirmFeedBackList.Count > 0)
                        {
                            __usedConfirmFeedBackBLL.Delete(usedConfirmFeedBackList.Select(p => p.Id), ref tran, isSupress);
                        }
                        entity.FeedBack.UsedConfirmId = entity.Id;
                        entity.FeedBack.Id = Guid.NewGuid();
                        __usedConfirmFeedBackBLL.Add(new UsedConfirmFeedBack[] { entity.FeedBack }, ref tran, isSupress);
                    }
                    
                    if (entity.UsedConfirmEquipmentAddtionChargeItems != null && entity.UsedConfirmEquipmentAddtionChargeItems.Count > 0)
                    {
                        foreach (var usedConfirmEquipmentAddtionChargeItem in entity.UsedConfirmEquipmentAddtionChargeItems)
                            usedConfirmEquipmentAddtionChargeItem.UsedConfirmId = entity.Id;
                        __usedConfirmEquipmentAddtionChargeItemBLL.Add(entity.UsedConfirmEquipmentAddtionChargeItems, ref tran, isSupress);
                    }
                    if (entity.UsedConfirmEquipmentParts != null && entity.UsedConfirmEquipmentParts.Count > 0)
                    {
                        foreach (var usedConfirmEquipmentPart in entity.UsedConfirmEquipmentParts)
                            usedConfirmEquipmentPart.UsedConfirmId = entity.Id;
                        __usedConfirmEquipmentPartBLL.Add(entity.UsedConfirmEquipmentParts, ref tran, isSupress);
                    }
                    if (entity.UsedConfirmEquipmentUseConditions != null && entity.UsedConfirmEquipmentUseConditions.Count > 0)
                    {
                        foreach (var usedConfirmEquipmentUseCondition in entity.UsedConfirmEquipmentUseConditions)
                            usedConfirmEquipmentUseCondition.UsedConfirmId = entity.Id;
                        __usedConfirmEquipmentUseConditionBLL.Add(entity.UsedConfirmEquipmentUseConditions, ref tran, isSupress);
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
        public override bool Save(IEnumerable<UsedConfirm> entities, ref XTransaction tran, bool isSupress = false)
        {
            string errorMessage = "";
            return Save(entities, ref tran, false, out errorMessage, isSupress);
        }
        public override bool Delete(IEnumerable<Guid> ids, ref XTransaction tran, bool isSupress = false)
        {
            try
            {
                if (!isSupress && tran != null) tran = SessionManager.Instance.BeginTransaction();
                var usedConfirmEquipmentAddtionChargeItemList = __usedConfirmEquipmentAddtionChargeItemBLL.GetEntitiesByExpression(ids.ToFormatStr("UsedConfirmId"));
                if (usedConfirmEquipmentAddtionChargeItemList != null && usedConfirmEquipmentAddtionChargeItemList.Count > 0)
                    __usedConfirmEquipmentAddtionChargeItemBLL.Delete(usedConfirmEquipmentAddtionChargeItemList.Select(p => p.Id), ref tran, true);
                var usedConfirmEquipmentPartList = __usedConfirmEquipmentPartBLL.GetEntitiesByExpression(ids.ToFormatStr("UsedConfirmId"));
                if (usedConfirmEquipmentPartList != null && usedConfirmEquipmentPartList.Count > 0)
                    __usedConfirmEquipmentPartBLL.Delete(usedConfirmEquipmentPartList.Select(p => p.Id), ref tran, true);
                var usedConfirmEquipmentUseConditionList = __usedConfirmEquipmentUseConditionBLL.GetEntitiesByExpression(ids.ToFormatStr("UsedConfirmId"));
                if (usedConfirmEquipmentUseConditionList != null && usedConfirmEquipmentUseConditionList.Count > 0)
                    __usedConfirmEquipmentUseConditionBLL.Delete(usedConfirmEquipmentUseConditionList.Select(p => p.Id), ref tran, true);
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
        public object GetUsedConfirmPriviliges(Guid? userId, IList<ViewUsedConfirm> viewUsedConfirmList)
        {
            IList<UsedConfirmPrivilige> lstUsedConfirmPriviliges = new List<UsedConfirmPrivilige>();
            if (userId.HasValue && viewUsedConfirmList != null && viewUsedConfirmList.Count > 0)
            {
                foreach (ViewUsedConfirm viewUsedConfirm in viewUsedConfirmList)
                {
                    UsedConfirmPrivilige usedConfirmPrivilige = lstUsedConfirmPriviliges.FirstOrDefault(p => p.UsedConfirmId.HasValue && p.UsedConfirmId.Value == viewUsedConfirm.Id);
                    if (usedConfirmPrivilige == null)
                    {
                        usedConfirmPrivilige = PriviligeFactory.GetUsedConfirmPrivilige(userId.Value, viewUsedConfirm.Id);
                        lstUsedConfirmPriviliges.Add(usedConfirmPrivilige);
                    }
                }
            }
            return lstUsedConfirmPriviliges;
        }
        public bool JudgeIsEnableEditUsedConfirm(Guid? userId, IList<ViewUsedConfirm> viewUsedConfirms, object usedConfirmPriviliges, out string errorMessage)
        {
            return JudgeIsEnableSaveConfirm(userId, viewUsedConfirms, usedConfirmPriviliges, out errorMessage) || JudgeIsEnableCostDeductConfirm(userId, viewUsedConfirms, usedConfirmPriviliges, out errorMessage);
        }
        public bool JudgeIsEnableDeleteUsedConfirm(Guid? userId, IList<ViewUsedConfirm> viewUsedConfirms, object usedConfirmPriviliges, out string errorMessage)
        {
            errorMessage = "";
            var lstUsedConfirmPrivilige = (IList<UsedConfirmPrivilige>)usedConfirmPriviliges;
            foreach (var viewUsedConfirm in viewUsedConfirms)
            {
                var usedConfirmPrivilige = lstUsedConfirmPrivilige.FirstOrDefault(p => p.UsedConfirmId.HasValue && p.UsedConfirmId == viewUsedConfirm.Id);
                if (usedConfirmPrivilige == null || !usedConfirmPrivilige.IsEnableDeleteUsedConfirm)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewUsedConfirm.Status == (int)UsedConfirmStatus.Deduct)
                {
                    errorMessage = "已扣费,不能删除";
                    return false;
                }
                if (viewUsedConfirm.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    errorMessage = "已结算,不能删除";
                    return false;
                }

            }
            return true;
        }
        public bool JudgeIsEnableSelfSaveConfirm(Guid? userId, IList<ViewUsedConfirm> viewUsedConfirms, object usedConfirmPriviliges, out string errorMessage)
        {
            errorMessage = "";
            var lstUsedConfirmPrivilige = (IList<UsedConfirmPrivilige>)usedConfirmPriviliges;
            foreach (var viewUsedConfirm in viewUsedConfirms)
            {
                var usedConfirmPrivilige = lstUsedConfirmPrivilige.FirstOrDefault(p => p.UsedConfirmId.HasValue && p.UsedConfirmId == viewUsedConfirm.Id);
                if (usedConfirmPrivilige == null || !usedConfirmPrivilige.IsEnableSelfSaveAppointmentUsedConfirm)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewUsedConfirm.Status == (int)UsedConfirmStatus.Deduct)
                {
                    errorMessage = "已扣费,不能保存";
                    return false;
                }
                if (viewUsedConfirm.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    errorMessage = "已结算,不能保存";
                    return false;
                }

            }
            return true;

        }
        public bool JudgeIsEnableSaveConfirm(Guid? userId, IList<ViewUsedConfirm> viewUsedConfirms, object usedConfirmPriviliges, out string errorMessage)
        {
            errorMessage = "";
            var lstUsedConfirmPrivilige = (IList<UsedConfirmPrivilige>)usedConfirmPriviliges;
            foreach (var viewUsedConfirm in viewUsedConfirms)
            {
                var usedConfirmPrivilige = lstUsedConfirmPrivilige.FirstOrDefault(p => p.UsedConfirmId.HasValue && p.UsedConfirmId == viewUsedConfirm.Id);
                if (usedConfirmPrivilige == null || !usedConfirmPrivilige.IsEnableSaveUsedConfirm)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewUsedConfirm.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    errorMessage = "已结算,不能保存";
                    return false;
                }

            }
            return true;

        }
        public bool JudgeIsEnableCostDeductConfirm(Guid? userId, IList<ViewUsedConfirm> viewUsedConfirms, object usedConfirmPriviliges, out string errorMessage)
        {
            errorMessage = "";
            var lstUsedConfirmPrivilige = (IList<UsedConfirmPrivilige>)usedConfirmPriviliges;
            foreach (var viewUsedConfirm in viewUsedConfirms)
            {
                var usedConfirmPrivilige = lstUsedConfirmPrivilige.FirstOrDefault(p => p.UsedConfirmId.HasValue && p.UsedConfirmId == viewUsedConfirm.Id);
                if (usedConfirmPrivilige == null || !usedConfirmPrivilige.IsEnableDirectCostDeduct)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewUsedConfirm.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    errorMessage = "已结算,不能扣费";
                    return false;
                }

            }
            return true;
        }
        /// <summary>
        /// 新建使用记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subjectId"></param>
        /// <param name="equipmentId"></param>
        /// <param name="paymentMethod"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="sampleCount"></param>
        /// <param name="funcPreInitUsedConfirm">预初始使用记录函数</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public UsedConfirm CreatedUsedConfirm(Guid userId, Guid? subjectId, Guid equipmentId, PaymentMethod paymentMethod, DateTime beginTime, DateTime endTime, int? sampleCount,FuncPreInitUsedConfirm funcPreInitUsedConfirm,out string errorMessage)
        {
            errorMessage = "";
            double discount = 1;
            var usedConfirm = new UsedConfirm() { Id = Guid.NewGuid(), CalcDuration = 0d, CalcFee = 0d, CreateAt = DateTime.Now, Status = (int)UsedConfirmStatus.UnComplete, RealFee = 0d, SampleCount = sampleCount};
            if (funcPreInitUsedConfirm != null) funcPreInitUsedConfirm(usedConfirm);
            usedConfirm.BeginAt = beginTime;
            usedConfirm.SubjectId = subjectId;
            usedConfirm.EquipmentId = equipmentId;
            usedConfirm.UserId = userId;
            usedConfirm.PaymentMethodEnum = paymentMethod;
            usedConfirm.Status = (int)UsedConfirmStatus.UnComplete;
            usedConfirm.Remark = "";
            // 设备允许反馈且可反馈
            usedConfirm.IsEnableAppoitmentWithFeedBack = usedConfirm.Equipment.IsEnableAppoitmentWithFeedBack.HasValue && usedConfirm.Equipment.IsEnableAppoitmentWithFeedBack.Value ? true : false;
            GenerateRelativeAppointmentUsedConfirm(usedConfirm.GetAppointment(), usedConfirm);
            // 更新使用记录设备附加收费项目
            if (usedConfirm.UsedConfirmEquipmentAddtionChargeItems == null || usedConfirm.UsedConfirmEquipmentAddtionChargeItems.Count == 0)
            {
                var equipmentAdditionChargeItems = __equipmentAdditionChargeItemBLL.GetUserEquipmentAdditionChargeItemList(usedConfirm.EquipmentId, usedConfirm.UserId, usedConfirm.SubjectId);
                if (equipmentAdditionChargeItems != null && equipmentAdditionChargeItems.Count > 0)
                {
                    usedConfirm.UsedConfirmEquipmentAddtionChargeItems = new List<UsedConfirmEquipmentAddtionChargeItem>();
                    foreach (var equipmentAdditionChargeItem in equipmentAdditionChargeItems)
                    {
                        var usedConfirmEquipmentAddtionChargeItem = new UsedConfirmEquipmentAddtionChargeItem() { Id = Guid.NewGuid() };
                        usedConfirmEquipmentAddtionChargeItem.UsedConfirmId = usedConfirm.Id;
                        usedConfirmEquipmentAddtionChargeItem.EquipmentAddtionChargeItemId = equipmentAdditionChargeItem.Id;
                        usedConfirmEquipmentAddtionChargeItem.Amount = 1;
                        usedConfirmEquipmentAddtionChargeItem.Price = equipmentAdditionChargeItem.StandardPrice;
                        usedConfirm.UsedConfirmEquipmentAddtionChargeItems.Add(usedConfirmEquipmentAddtionChargeItem);
                    }
                }
            }
            try
            {
                // 更新计费过程
                var chargeStandard = __chargeStandardBLL.GetEquipmentChargeStandardByUserId(usedConfirm.EquipmentId, usedConfirm.UserId, usedConfirm.SubjectId, out discount);
                if (chargeStandard == null) throw new Exception("计费标准为空");
                if (usedConfirm.Equipment.CalcFeeTimeRule == null) throw new Exception("计费时间规则为空");
                usedConfirm.UnitPrice = chargeStandard.StandardPrice;
                usedConfirm.ChargeTypeEnum = chargeStandard.ChargeTypeEnum;
                usedConfirm.Expression = usedConfirm.Equipment.CalcFeeTimeRule.Expression;
                usedConfirm.RoundDigits = usedConfirm.Equipment.CalcFeeTimeRule.RoundDigits;
                usedConfirm.RoundFator = usedConfirm.Equipment.CalcFeeTimeRule.RoundFator;
                if ((endTime - beginTime).TotalMilliseconds >= 0)
                {
                    usedConfirm.Status = (int)UsedConfirmStatus.Complete;
                    usedConfirm.EndAt = endTime;
                    double calDuration = -1;
                    double? unitPrice = null;
                    bool isOpenFundCostDeduct = false;
                    double? realEquipmentOpenFundDiscout = null;
                    IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
                    usedConfirm.CalcFee = __chargeStandardBLL.GetUsedConfirmCalFee(usedConfirm, out calDuration, out unitPrice, out isOpenFundCostDeduct, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscout);
                    usedConfirm.CalcDuration = calDuration == -1d ? (usedConfirm.EndAt.Value - usedConfirm.BeginAt.Value).TotalHours : calDuration;
                    usedConfirm.RealFee = usedConfirm.CalcFee;
                }
                else
                {
                    usedConfirm.Status = (int)UsedConfirmStatus.UnComplete;
                }
            }
            catch (Exception ex) { errorMessage = ex.Message; }
            return usedConfirm;
        }
        /// <summary> 计算使用费用 </summary>
        /// <param name="usedConfirm"></param>
        /// <param name="duration"></param>
        /// <param name="predictFee"></param>
        /// <param name="openFundFee"></param>
        /// <param name="totalFee"></param>
        /// <param name="isEquipmentOpenFundDiscount"></param>
        public void CalUsedConfirmFee(UsedConfirm usedConfirm, out double duration, out double predictFee, out double openFundFee, out double totalFee, out bool isEquipmentOpenFundDiscount)
        {
            IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
            double? unitPrice = null, realEquipmentOpenFundDiscout = null;
            predictFee = __chargeStandardBLL.GetUsedConfirmCalFee(usedConfirm, out duration, out unitPrice, out isEquipmentOpenFundDiscount, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscout);
            openFundFee = costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Count > 0 ? costDeductEquipmentOpenFunds.Sum(p => p.CalFee) : 0d;
            totalFee = predictFee + openFundFee;
            usedConfirm.RealFee = totalFee;
            usedConfirm.CalcFee = totalFee;
        }
        /// <summary> 创建预约扣费 </summary>
        /// <param name="appointment"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isDeduct"></param>
        /// <param name="fee"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public UsedConfirm CreateAppointmentUsedConfirm(Appointment appointment, DateTime beginTime, DateTime endTime, bool isDeduct, double? fee, out string errorMessage)
        {
            errorMessage = "";
            var usedConfirm = CreatedUsedConfirm(appointment.UserId.Value, 
                                                 appointment.SubjectId.Value, 
                                                 appointment.EquipmentId.Value, 
                                                 appointment.PaymentMethodEnum, 
                                                 beginTime, 
                                                 endTime, 
                                                 appointment.SampleCount,
                                                 (usedConfirmTemp) => 
                                                 { 
                                                     usedConfirmTemp.SetAppointment(appointment); 
                                                 }, 
                                                 out errorMessage);
            usedConfirm.AppointmentId = appointment.Id;
            if (fee.HasValue) usedConfirm.RealFee = fee.Value;//用户手工扣费，保存用户输入的实收费用
            GenerateRelativeAppointmentUsedConfirm(appointment, usedConfirm);
            if (!isDeduct)
            {
                usedConfirm.RealFee = null;
                var preCostDeduct = __costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"",appointment.Id.ToString()));
                if (preCostDeduct != null)
                {
                    usedConfirm.RealFee = preCostDeduct.CalFee;
                    usedConfirm.CalcFee = preCostDeduct.CalFee;
                }
                else
                {
                    bool isEquipmentOpenFundDiscount = false;
                    double duration = 0d,predictFee = 0d, openFundFee = 0d, totalFee = 0d;
                    CalUsedConfirmFee(usedConfirm, out duration, out predictFee, out openFundFee, out totalFee, out isEquipmentOpenFundDiscount);
                }
            }
            return usedConfirm;
        }
        /// <summary> 构建有关预约使用记录 </summary>
        /// <param name="appointment"></param>
        /// <param name="usedConfirm"></param>
        public void GenerateRelativeAppointmentUsedConfirm(Appointment appointment, UsedConfirm usedConfirm)
        {
            // 构建使用记录设备附加收费项目
            if (usedConfirm.UsedConfirmEquipmentAddtionChargeItems == null || usedConfirm.UsedConfirmEquipmentAddtionChargeItems.Count == 0)
                usedConfirm.UsedConfirmEquipmentAddtionChargeItems = __usedConfirmEquipmentAddtionChargeItemBLL.GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", usedConfirm.Id));
            if ((usedConfirm.UsedConfirmEquipmentAddtionChargeItems == null || usedConfirm.UsedConfirmEquipmentAddtionChargeItems.Count == 0)
                 && (appointment != null && appointment.AppointmentEquipmentAddtionChargeItems != null && appointment.AppointmentEquipmentAddtionChargeItems.Count > 0))
            {
                usedConfirm.UsedConfirmEquipmentAddtionChargeItems = new List<UsedConfirmEquipmentAddtionChargeItem>();
                foreach (var appointmentEquipmentAddtionChargeItem in appointment.AppointmentEquipmentAddtionChargeItems)
                {
                    usedConfirm.UsedConfirmEquipmentAddtionChargeItems.Add(new UsedConfirmEquipmentAddtionChargeItem()
                    {
                        Id = Guid.NewGuid(),
                        EquipmentAddtionChargeItemId = appointmentEquipmentAddtionChargeItem.EquipmentAddtionChargeItemId,
                        Price = appointmentEquipmentAddtionChargeItem.Price,
                        Amount = appointmentEquipmentAddtionChargeItem.Amount,
                        UsedConfirmId = usedConfirm.Id
                    });
                }
            }
            // 构建使用记录设备部件
            if (usedConfirm.UsedConfirmEquipmentParts == null || usedConfirm.UsedConfirmEquipmentParts.Count == 0)
                usedConfirm.UsedConfirmEquipmentParts = __usedConfirmEquipmentPartBLL.GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", usedConfirm.Id));
            if ((usedConfirm.UsedConfirmEquipmentParts == null || usedConfirm.UsedConfirmEquipmentParts.Count == 0)
                && (appointment != null && appointment.AppointmentEquipmentParts != null && appointment.AppointmentEquipmentParts.Count > 0))
            {
                usedConfirm.UsedConfirmEquipmentParts = new List<UsedConfirmEquipmentPart>();
                foreach (var appointmentEquipmentPart in appointment.AppointmentEquipmentParts)
                {
                    usedConfirm.UsedConfirmEquipmentParts.Add(new UsedConfirmEquipmentPart()
                    {
                        Id = Guid.NewGuid(),
                        EquipmentPartId = appointmentEquipmentPart.EquipmentPartId,
                        Price = appointmentEquipmentPart.Price,
                        Amount = appointmentEquipmentPart.Amount,
                        UsedConfirmId = usedConfirm.Id,
                        Duration = usedConfirm.CalcDuration,
                        Discount = appointmentEquipmentPart.Discount,
                        IsMutiplyByDuration = appointmentEquipmentPart.IsMutiplyByDuration
                    });
                }
            }
            // 构建使用记录设备运行参数
            if (usedConfirm.UsedConfirmEquipmentUseConditions == null || usedConfirm.UsedConfirmEquipmentUseConditions.Count == 0)
                usedConfirm.UsedConfirmEquipmentUseConditions = __usedConfirmEquipmentUseConditionBLL.GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", usedConfirm.Id));
            if ((usedConfirm.UsedConfirmEquipmentUseConditions == null || usedConfirm.UsedConfirmEquipmentUseConditions.Count == 0)
                && (appointment != null && appointment.AppointmentEquipmentUseConditions != null && appointment.AppointmentEquipmentUseConditions.Count > 0))
            {
                usedConfirm.UsedConfirmEquipmentUseConditions = new List<UsedConfirmEquipmentUseCondition>();
                foreach (var appointmentEquipmentUseCondition in appointment.AppointmentEquipmentUseConditions)
                {
                    usedConfirm.UsedConfirmEquipmentUseConditions.Add(new UsedConfirmEquipmentUseCondition()
                    {
                        Id = Guid.NewGuid(),
                        EquipmentUseConditionId = appointmentEquipmentUseCondition.EquipmentUseConditionId,
                        Price = appointmentEquipmentUseCondition.Price,
                        Amount = appointmentEquipmentUseCondition.Amount,
                        UsedConfirmId = usedConfirm.Id,
                        Val = appointmentEquipmentUseCondition.Val,
                        Duration = usedConfirm.CalcDuration,
                        Discount = appointmentEquipmentUseCondition.Discount,
                        IsMutiplyByDuration = appointmentEquipmentUseCondition.IsMutiplyByDuration
                    });
                }
            }
            DateTime? beginAppointmentTime = null, endAppointmentTime = null;
            // 将预约信息完善使用记录
            if (appointment != null)
            {
                beginAppointmentTime = appointment.BeginTime;
                endAppointmentTime = appointment.EndTime;
                if (string.IsNullOrWhiteSpace(usedConfirm.SampleStuff))
                {
                    usedConfirm.SampleStuff = appointment.SampleStuff;
                }
                if (string.IsNullOrWhiteSpace(usedConfirm.SampleSize))
                {
                    usedConfirm.SampleSize = appointment.SampleSize;
                }
                if (string.IsNullOrWhiteSpace(usedConfirm.Target))
                {
                    usedConfirm.Target = appointment.Target;
                }
                if (string.IsNullOrWhiteSpace(usedConfirm.SampleNo))
                {
                    usedConfirm.SampleNo = appointment.SampleNo;
                }
                if (usedConfirm.SampleCount == 0 || usedConfirm.SampleCount == null)
                {
                    usedConfirm.SampleCount = appointment.SampleCount;
                }
                if (usedConfirm.SubjectProject == null && appointment.SubjectProjectId.HasValue)
                {
                    SubjectProject subjectPoject = __subjectProjectBLL.GetEntityById(appointment.SubjectProjectId.Value);
                    usedConfirm.subjectPorjectName = subjectPoject.Name;
                }
            }
            // 记录使用时长
            var duration = usedConfirm.EndAt.HasValue ? __calFeeTimeRuleBLL.CalcFeeTime(false, usedConfirm.EquipmentId, usedConfirm.UserId, usedConfirm.BeginAt, usedConfirm.EndAt, beginAppointmentTime, endAppointmentTime, usedConfirm.Equipment.CalcFeeTimeRule.Expression) : 0d;
            if (usedConfirm.UsedConfirmEquipmentParts != null && usedConfirm.UsedConfirmEquipmentParts.Count > 0)
            {
                foreach (var usedConfirmEquipmentPart in usedConfirm.UsedConfirmEquipmentParts)
                {
                    usedConfirmEquipmentPart.Duration = duration;
                }
            }
            if (usedConfirm.UsedConfirmEquipmentUseConditions != null && usedConfirm.UsedConfirmEquipmentUseConditions.Count > 0)
            {
                foreach (var usedConfirmEquipmentUseCondition in usedConfirm.UsedConfirmEquipmentUseConditions)
                {
                    usedConfirmEquipmentUseCondition.Duration = duration;
                }
            }
        }
        public bool Validates(Guid equipmentId, Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            var equipment = __equipmentBLL.GetEntityById(equipmentId);
            if (!userId.HasValue || !equipment.IsEnableAppoitmentWithFeedBack.HasValue || !equipment.IsEnableAppoitmentWithFeedBack.Value) return true;
            var count = CountModelListByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"*IsEnableAppoitmentWithFeedBack=true*IsFeedBack=false*Status=-\"{2}\"", equipmentId.ToString(), userId.Value.ToString(),(int)UsedConfirmStatus.UnComplete));
            if (count > 0)
            {
                errorMessage = string.Format("未填写使用反馈的使用记录{0}个", count);
                return false;
            }
            return true;
        }
    }
}