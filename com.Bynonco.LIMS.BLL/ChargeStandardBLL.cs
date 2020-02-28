using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary> 分段计费业务逻辑 </summary>
    public class ChargeStandardBLL : BLLBase<ChargeStandard>, IChargeStandardBLL
    {
        private ITagBLL __tagBLL;
        private IUserBLL __userBLL;
        private ILabOrganizationBLL __labOrganizationBLL;
        private IEquipmentBLL __equipmentBLL;
        private IEquipmentLabelBLL __equipmentLabelBLL;
        private IEquipmentLabelChargeStandardBLL __equipmentLabelChargeStandardBLL;
        private IEquipmentAdditionChargeItemBLL __equipmentAdditionChargeItemBLL;
        private ICalcFeeTimeRuleBLL __calcFeeTimeRuleBL;
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private IOpenFundApplyBLL __openFundApplyBLL;
        private IViewOpenFundDetailBLL __viewOpenFundDetailBLL;
        private ChargeStandardRelativeBLL __chargeStandardRelativeBLL;
        private const string __FUNCTIONNAME = "getSelfDefinedChargeStatandardPrice";
        public ChargeStandardBLL()
        {
            __tagBLL = BLLFactory.CreateTagBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __calcFeeTimeRuleBL = BLLFactory.CreateCalcFeeTimeRuleBLL();
            __equipmentLabelBLL = BLLFactory.CreateEquipmentLabelBLL();
            __equipmentLabelChargeStandardBLL = BLLFactory.CreateEquipmentLabelChargeStandardBLL();
            __equipmentAdditionChargeItemBLL = BLLFactory.CreateEquipmentAdditionChargeItemBLL();
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __openFundApplyBLL = BLLFactory.CreateOpenFundApplyBLL();
            __viewOpenFundDetailBLL = BLLFactory.CreateViewOpenFundDetailBLL();
            __chargeStandardRelativeBLL = new ChargeStandardRelativeBLL(GetEquipmentChargeStandardByEquipmentId,
            __equipmentLabelBLL.GetEquipmentLabelByUserId,
            __equipmentLabelChargeStandardBLL.GetEquipmentLabelChargeStandardListByEquipmentIdAndLableId,
            __equipmentLabelChargeStandardBLL.GetEquipmentLabelChargeStandard,
            __calcFeeTimeRuleBL.CalcFeeTime,
            __equipmentAdditionChargeItemBLL.GetEquipmentAdditionChargeItemListByEquipmentIdAndUserId);

        }
        private IDictionary<string, object> GenerateParamerters(Guid equipmentId, Guid? userId, Guid[] equipmentPartIds)
        {
            User user = null;
            string userOrgXpath = "", equipmentOrgXpath = "";
            IDictionary<string, object> ddParameters = new Dictionary<string, object>();
            if (userId.HasValue) user = __userBLL.GetEntityById(userId.Value);
            if (user != null && user.Organization != null)
            {
                userOrgXpath = user.Organization.XPath;
            }
            var equipment = __equipmentBLL.GetEntityById(equipmentId);
            if (equipment.OrgId.HasValue)
            {
                equipmentOrgXpath = __labOrganizationBLL.GetEntityById(equipment.OrgId.Value).XPath;
            }
            ddParameters["equipmentId"] = equipmentId.ToString();
            ddParameters["userOrgXpath"] = userOrgXpath;
            ddParameters["equipmentOrgXpath"] = equipmentOrgXpath;
            ddParameters["equipmentPartIds"] = equipmentPartIds;
            return ddParameters;
        }
        private ChargeStandard GetEquipmentChargeStandardByEquipmentId(Guid equipmentId)
        {
            var chargeStandard = GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId));
            return chargeStandard;
        }
        public ChargeStandard GetEquipmentChargeStandardByUserId(Guid equipmentId, Guid? userId, Guid? subjectId, out double discount)
        {
            #region OriginalCode
            //discount = 1;
            //var chargeStandard = GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId.ToString()));
            //if (chargeStandard == null) return null;
            //if (!userId.HasValue || chargeStandard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.None) return chargeStandard;
            //var equipmentLable = __equipmentLabelBLL.GetEquipmentLabelByUserId(equipmentId, userId.Value, true);
            //if (equipmentLable == null) return chargeStandard;
            //var equipmentLabelChargeStandardList = __equipmentLabelChargeStandardBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*EquipmentLabelId=\"{1}\"", equipmentId.ToString(), equipmentLable.Id.ToString()));
            //if (equipmentLabelChargeStandardList != null && equipmentLabelChargeStandardList.Count > 0)
            //{
            //    var equipmentLabelChargeStandard = equipmentLabelChargeStandardList.First();
            //    if (chargeStandard.StandardPrice != 0d)
            //    {
            //        discount = equipmentLabelChargeStandard.StandardPrice / chargeStandard.StandardPrice;
            //    }
            //    chargeStandard.StandardPrice = equipmentLabelChargeStandard.StandardPrice;
            //    chargeStandard.ChargeTypeEnum = equipmentLabelChargeStandard.ChargeTypeEnum;
            //}
            //return chargeStandard;
            #endregion

            return (ChargeStandard)__chargeStandardRelativeBLL.GetChargeStandardByUserId(equipmentId, userId, subjectId, out discount);
        }
        public ChargeStandard GetEquipmentChargeStandard(Guid equipmentId, Guid? userId, Guid? subjectId, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, Guid[] equipmentPartIds, out double discount)
        {
            #region OriginalCode
            //IUserBLL userBLL = BLLFactory.CreateUserBLL();
            //IEquipmentBLL equipmentBLL = BLLFactory.CreateEquipmentBLL();
            //ILabOrganizationBLL labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            //discount = 1;
            //var chargeStandard = GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId.ToString()));
            //if (chargeStandard == null) return null;
            //if (chargeStandard.ChargeTypeEnum != com.Bynonco.LIMS.Model.Enum.ChargeType.SelfDefined)
            //    return GetEquipmentChargeStandardByUserId(equipmentId, userId, out discount);
            //if (string.IsNullOrWhiteSpace(chargeStandard.Expression)) throw new Exception("出错,自定义表达式为空");
            //var userOrgXpath = "";
            //User user = null;
            //if (userId.HasValue) user = userBLL.GetEntityById(userId.Value);
            //if (user != null && user.Organization != null)
            //{
            //    userOrgXpath = user.Organization.XPath;
            //}
            //var equipmentOrgXpath = "";
            //var equipment = equipmentBLL.GetEntityById(equipmentId);
            //if (equipment.OrgId.HasValue)
            //{
            //    equipmentOrgXpath = labOrganizationBLL.GetEntityById(equipment.OrgId.Value).XPath;
            //}
            //StringBuilder sbLuaCommand = new StringBuilder();
            //sbLuaCommand.AppendLine("function getSelfDefinedChargeStatandardPrice(usedBeginTime,usedEndTime,appointmentBeginTime,appointmentEndTime,equipmentId,userId,userOrgXpath,equipmentOrgXpath,price,equipmentPartIds) ");
            //sbLuaCommand.AppendLine(chargeStandard.Expression);
            //sbLuaCommand.AppendLine(" end");
            //LuaManager.Instance.ExecuteString(sbLuaCommand.ToString());
            //var results = LuaManager.Instance.CallFunction("getSelfDefinedChargeStatandardPrice", usedBeginTime, usedEndTime, appointmentBeginTime, appointmentEndTime, equipmentId.ToString(), userId.HasValue ? userId.Value.ToString() : "", userOrgXpath, equipmentOrgXpath, chargeStandard.StandardPrice, equipmentPartIds);
            //chargeStandard.StandardPrice = double.Parse(results[0].ToString());
            //return chargeStandard;
            #endregion

            var ddParameters = GenerateParamerters(equipmentId, userId, equipmentPartIds);
            return (ChargeStandard)__chargeStandardRelativeBLL.GetChargeStandard(__FUNCTIONNAME, equipmentId, userId, subjectId, usedBeginTime, usedEndTime, appointmentBeginTime, appointmentEndTime, ddParameters, out discount);
        }
        /// <summary> 计算机时预约费用 </summary>
        /// <param name="viewOpenFundDetailList">开放基金明细视图</param>
        /// <param name="appointment">机时预约</param>
        /// <param name="duration">持续时间</param>
        /// <param name="isEquipmentOpenFundDiscount">是否开放基金设备</param>
        /// <param name="costDeductEquipmentOpenFunds">开发基金设备费用</param>
        /// <param name="realEquipmentOpenFundDiscout">开放基金设备真实费用</param>
        /// <returns>费用</returns>
        public double GetAppointmentCalcFee(IList<ViewOpenFundDetail> viewOpenFundDetailList, Appointment appointment, out double duration, out bool isEquipmentOpenFundDiscount, out double openFundApplyFee, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, out double? realEquipmentOpenFundDiscout)
        {
            costDeductEquipmentOpenFunds = null;
            realEquipmentOpenFundDiscout = null;
            int roundDigit = 2;
            double discount = 1d;
            isEquipmentOpenFundDiscount = false;
            var isSupressLazyLoad = appointment.IsSupressLazyLoad;
            appointment.IsSupressLazyLoad = false;
            double? unitPrice = null;
            double fee = CalFee(appointment.EquipmentId.Value,
                appointment.UserId.Value,
                appointment.SampleCount,
                appointment.BeginTime,
                appointment.EndTime,
                appointment.BeginTime,
                appointment.EndTime,
                appointment.AppointmentEquipmentParts != null && appointment.AppointmentEquipmentParts.Count > 0 ?
                appointment.AppointmentEquipmentParts.Select(p => p.EquipmentPartId).ToArray() :
                null,
                out duration,
                out unitPrice,
                out roundDigit,
                out discount,
                appointment.SubjectId);
            // 计算设备附件费用
            if (appointment.AppointmentEquipmentAddtionChargeItems != null && appointment.AppointmentEquipmentAddtionChargeItems.Count > 0)
            {
                fee += appointment.AppointmentEquipmentAddtionChargeItems.Sum(p => p.Fee);
            }
            // 计算设备部件费用
            if (appointment.AppointmentEquipmentParts != null && appointment.AppointmentEquipmentParts.Count > 0)
            {
                foreach (var appointmentEquipmentPart in appointment.AppointmentEquipmentParts)
                {
                    fee += appointmentEquipmentPart.EquipmentPart.IsUseDiscount.HasValue && appointmentEquipmentPart.EquipmentPart.IsUseDiscount.Value ? appointmentEquipmentPart.Fee * discount : appointmentEquipmentPart.Fee;
                    appointmentEquipmentPart.Discount = appointmentEquipmentPart.EquipmentPart.IsUseDiscount.HasValue && appointmentEquipmentPart.EquipmentPart.IsUseDiscount.Value ? discount : 1d;
                }
            }
            // 计算机时运行参数费用
            if (appointment.AppointmentEquipmentUseConditions != null && appointment.AppointmentEquipmentUseConditions.Count > 0)
            {
                foreach (var appointmentEquipmentUseCondition in appointment.AppointmentEquipmentUseConditions)
                {
                    fee += appointmentEquipmentUseCondition.EquipmentUseCondition.IsUseDiscount.HasValue && appointmentEquipmentUseCondition.EquipmentUseCondition.IsUseDiscount.Value ? appointmentEquipmentUseCondition.Fee * discount : appointmentEquipmentUseCondition.Fee;
                    appointmentEquipmentUseCondition.Discount = appointmentEquipmentUseCondition.EquipmentUseCondition.IsUseDiscount.HasValue && appointmentEquipmentUseCondition.EquipmentUseCondition.IsUseDiscount.Value ? discount : 1d;
                }
            }
            // 开放基金费用计算
            var realFee = __openFundApplyBLL.CalEquipmentOpenFundFee(viewOpenFundDetailList, fee, null, appointment.EndTime.Value, appointment.EquipmentId.Value, appointment.Subject.SubjectDirectorId.Value, appointment.PredictCostDeduct != null ? appointment.PredictCostDeduct.CostDeductEquipmentOpenFunds : null, out isEquipmentOpenFundDiscount, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscout);
            openFundApplyFee = fee - realFee;
            fee = realFee;
            appointment.IsSupressLazyLoad = isSupressLazyLoad;
            return Math.Round(fee, roundDigit);
        }
        public double GetAppointmentCalcFee(Appointment appointment, out double duration, out bool isEquipmentOpenFundDiscount, out double openFundApplyFee, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, out double? realEquipmentOpenFundDiscout)
        {
            return GetAppointmentCalcFee(null, appointment, out duration, out isEquipmentOpenFundDiscount, out openFundApplyFee, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscout);
        }
        /// <summary>
        /// 计算费用
        /// </summary>
        /// <param name="usedConfirm">使用记录</param>
        /// <param name="duration">时长</param>
        /// <param name="unitPrice">单价</param>
        /// <param name="isEquipmentOpenFundDiscount">是否设备开放基金折扣</param>
        /// <param name="costDeductEquipmentOpenFunds">费用扣除设备开放基金</param>
        /// <param name="realEquipmentOpenFundDiscout">真实设备开放基金折扣</param>
        /// <returns>扣费结果</returns>
        public double GetUsedConfirmCalFee(UsedConfirm usedConfirm, out double duration, out double? unitPrice, out bool isEquipmentOpenFundDiscount, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, out double? realEquipmentOpenFundDiscout)
        {
            //IEquipmentAdditionChargeItemBLL equipmentAdditionChargeItemBLL = BLLFactory.CreateEquipmentAdditionChargeItemBLL();
            //costDeductEquipmentOpenFunds = null;
            //realEquipmentOpenFundDiscout = null;
            //isEquipmentOpenFundDiscount = false;
            var isSupressLazyLoad = usedConfirm.IsSupressLazyLoad;
            usedConfirm.IsSupressLazyLoad = false;
            //var equipment = usedConfirm.Equipment;
            var appointment = usedConfirm.GetAppointment();
            DateTime? appointmentBeginTime = null;
            int roundDigit = 2;
            double discount = 1d;
            if (appointment != null && appointment.BeginTime.HasValue) appointmentBeginTime = appointment.BeginTime.Value;
            DateTime? appointmentEndTime = null;
            if (appointment != null && appointment.EndTime.HasValue) appointmentEndTime = appointment.EndTime.Value;
            double fee = CalFee(usedConfirm.EquipmentId,
                usedConfirm.UserId,
                usedConfirm.SampleCount,
                usedConfirm.BeginAt,
                usedConfirm.EndAt,
                appointmentBeginTime,
                appointmentEndTime,
                usedConfirm.UsedConfirmEquipmentParts != null &&
                usedConfirm.UsedConfirmEquipmentParts.Count > 0 ?
                usedConfirm.UsedConfirmEquipmentParts.Select(p => p.EquipmentPartId).ToArray()
                : null, out duration, out unitPrice, out roundDigit, out discount,
                usedConfirm.SubjectId);
            // 计算附加费用合计
            if (usedConfirm.UsedConfirmEquipmentAddtionChargeItems != null && usedConfirm.UsedConfirmEquipmentAddtionChargeItems.Count > 0)
            {
                fee += usedConfirm.UsedConfirmEquipmentAddtionChargeItems.Sum(p => p.Fee);
            }
            // 设备部件费用合计及折扣
            if (usedConfirm.UsedConfirmEquipmentParts != null && usedConfirm.UsedConfirmEquipmentParts.Count > 0)
            {
                foreach (var usedConfirmEquipmentPart in usedConfirm.UsedConfirmEquipmentParts)
                {
                    fee += usedConfirmEquipmentPart.GetEquipmentPart().IsUseDiscount.HasValue && usedConfirmEquipmentPart.GetEquipmentPart().IsUseDiscount.Value ? usedConfirmEquipmentPart.Fee * discount : usedConfirmEquipmentPart.Fee;
                    usedConfirmEquipmentPart.Discount = usedConfirmEquipmentPart.GetEquipmentPart().IsUseDiscount.HasValue && usedConfirmEquipmentPart.GetEquipmentPart().IsUseDiscount.Value ? discount : 1d;
                }
            }
            // 使用记录设备运行参数费用合计及折扣
            if (usedConfirm.UsedConfirmEquipmentUseConditions != null && usedConfirm.UsedConfirmEquipmentUseConditions.Count > 0)
            {
                foreach (var usedConfirmEquipmentUseCondition in usedConfirm.UsedConfirmEquipmentUseConditions)
                {
                    fee += usedConfirmEquipmentUseCondition.GetEquipmentUseCondition().IsUseDiscount.HasValue && usedConfirmEquipmentUseCondition.GetEquipmentUseCondition().IsUseDiscount.Value ? usedConfirmEquipmentUseCondition.Fee * discount : usedConfirmEquipmentUseCondition.Fee;
                    usedConfirmEquipmentUseCondition.Discount = usedConfirmEquipmentUseCondition.GetEquipmentUseCondition().IsUseDiscount.HasValue && usedConfirmEquipmentUseCondition.GetEquipmentUseCondition().IsUseDiscount.Value ? discount : 1d;
                }
            }
            // 开放基金设备明细
            if (usedConfirm.Subject != null)
            {
                var viewOpenFundDetailList =
                    __viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(usedConfirm.EquipmentId,
                        usedConfirm.Subject.SubjectDirectorId.Value,
                        usedConfirm.EndAt.HasValue ? usedConfirm.EndAt.Value : DateTime.Now);
                fee = __openFundApplyBLL.CalEquipmentOpenFundFee(viewOpenFundDetailList, fee, null,
                    usedConfirm.EndAt.Value, usedConfirm.EquipmentId, usedConfirm.Subject.SubjectDirectorId.Value,
                    usedConfirm.CostDeduct != null ? usedConfirm.CostDeduct.CostDeductEquipmentOpenFunds : null,
                    out isEquipmentOpenFundDiscount, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscout);
            }
            else
            {
                isEquipmentOpenFundDiscount = false;
                costDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
                realEquipmentOpenFundDiscout = 0;
            }
            usedConfirm.IsSupressLazyLoad = isSupressLazyLoad;
            return Math.Round(fee, roundDigit);
        }
        public double GetCalFee(Guid equipmentId, Guid userId, Guid? subjectId, int? sampleCount, DateTime beginTime, DateTime endTime)
        {
            #region OriginalCode
            //IEquipmentAdditionChargeItemBLL equipmentAdditionChargeItemBLL = BLLFactory.CreateEquipmentAdditionChargeItemBLL();
            //double duration = 0d;
            //double? unitPrice = null;
            //int roundDigit = 2;
            //double discount = 1d;
            //var fee = CalFee(equipmentId, userId, sampleCount, beginTime, endTime, null, null, null, out duration, out unitPrice, out roundDigit, out discount);
            //var equipmentAdditionChargeItemList = equipmentAdditionChargeItemBLL.GetUserEquipmentAdditionChargeItemList(equipmentId, userId);
            //if (equipmentAdditionChargeItemList != null && equipmentAdditionChargeItemList.Count > 0)
            //    fee += equipmentAdditionChargeItemList.Sum(p => p.StandardPrice);
            //return Math.Round(fee, roundDigit);
            #endregion

            var equipment = __equipmentBLL.GetEntityById(equipmentId);
            return __chargeStandardRelativeBLL.GetCalFee(__FUNCTIONNAME, equipment.ChargeStandard, equipment.CalcFeeTimeRule, equipmentId, userId, subjectId, sampleCount, beginTime, endTime);
        }

        /// <summary> 计算费用 </summary>
        /// <param name="equipmentId"></param>
        /// <param name="userId"></param>
        /// <param name="sampleCount"></param>
        /// <param name="usedBeginTime"></param>
        /// <param name="usedEndTime"></param>
        /// <param name="appointmentBeginTime"></param>
        /// <param name="appointmentEndTime"></param>
        /// <param name="equipmentPartIds"></param>
        /// <param name="duration"></param>
        /// <param name="unitPrice"></param>
        /// <param name="roundDigit"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        private double CalFee(Guid equipmentId, Guid userId, int? sampleCount, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, Guid[] equipmentPartIds, out double duration, out double? unitPrice, out int roundDigit, out double discount, Guid? subjectId)
        {
            #region OriginalCode
            //IEquipmentBLL equipmentBLL = BLLFactory.CreateEquipmentBLL();
            //discount = 1;
            //roundDigit = 2;
            //unitPrice = null;
            //double fee = 0d;

            //var equipment = equipmentBLL.GetEntityById(equipmentId);
            //if (equipment.ChargeStandard.IsDurationPrice && equipment.ChargeStandard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.ByHour)
            //{
            //    fee = CalcDurationPrice(equipment.ChargeStandard, equipment.CalcFeeTimeRule, userId, usedBeginTime.Value, usedEndTime.Value, out duration, out unitPrice);
            //}
            //else
            //{
            //    fee = CalcPrice(equipment,
            //                     userId,
            //                     sampleCount,
            //                     usedBeginTime,
            //                     usedEndTime,
            //                     appointmentBeginTime,
            //                     appointmentEndTime,
            //                     equipmentPartIds,
            //                     out duration,
            //                     out unitPrice,
            //                     out roundDigit,
            //                     out discount);
            //}
            //return fee;
            #endregion

            var ddParameters = GenerateParamerters(equipmentId, userId, equipmentPartIds);
            var equipment = __equipmentBLL.GetEntityById(equipmentId);
            return __chargeStandardRelativeBLL.CalFee(__FUNCTIONNAME,
                                                        equipment.ChargeStandard,
                                                        equipment.CalcFeeTimeRule,
                                                        equipmentId,
                                                        userId,
                                                        sampleCount,
                                                        usedBeginTime,
                                                        usedEndTime,
                                                        appointmentBeginTime,
                                                        appointmentEndTime,
                                                        ddParameters,
                                                        out duration,
                                                        out unitPrice,
                                                        out roundDigit,
                                                        out discount,
                                                         subjectId);
        }
        private double CalcPrice(Equipment equipment, Guid userId, Guid? subjectId, int? sampleCount, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, Guid[] equipmentPartIds, out double duration, out double? unitPrice, out int roundDigits, out double discount)
        {
            #region OriginalCode
            //ICalcFeeTimeRuleBLL calcFeeTimeRuleBLL = BLLFactory.CreateCalcFeeTimeRuleBLL();
            //unitPrice = null;
            //duration = -1d;
            //roundDigits = 2;
            //discount = 1d;
            //if (equipment.CalcFeeTimeRule == null) throw new Exception("出错,未定义计算时间规则");
            //var chargeStard = GetEquipmentChargeStandard(equipment.Id, userId, usedBeginTime, usedEndTime, appointmentBeginTime, appointmentEndTime, equipmentPartIds, out discount);
            //if (chargeStard == null) throw new Exception("出错,未定义计费标准");
            //roundDigits = chargeStard.RoundDigits;
            //unitPrice = chargeStard.StandardPrice;
            //var calHour = calcFeeTimeRuleBLL.CalcFeeTime(chargeStard.IsDurationPrice, equipment.Id, userId, usedBeginTime, usedEndTime, appointmentBeginTime, appointmentEndTime, equipment.CalcFeeTimeRule.Expression, equipment.CalcFeeTimeRule.RoundFator, equipment.CalcFeeTimeRule.RoundDigits);
            //duration = calHour;
            //if (chargeStard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.ByTimes)
            //    return chargeStard.StandardPrice;
            //if (chargeStard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.BySampleCount && sampleCount.HasValue && sampleCount.Value > 0)//如果样品数为空，按照机时进行扣费
            //    return sampleCount.Value * chargeStard.StandardPrice;
            //var price = calHour * chargeStard.StandardPrice;
            //return price;
            #endregion

            var ddParameters = GenerateParamerters(equipment.Id, userId, equipmentPartIds);
            return __chargeStandardRelativeBLL.CalcPrice(__FUNCTIONNAME,
                equipment.CalcFeeTimeRule,
                equipment.ChargeStandard,
                equipment.Id,
                userId,
                subjectId,
                sampleCount,
                usedBeginTime,
                usedEndTime,
                appointmentBeginTime,
                appointmentEndTime,
                ddParameters,
                out duration,
                out unitPrice,
                out roundDigits,
                out discount);
        }


        public ChargeStandard GetEquipmentChargeStandardByTagName(Guid equipmentId, string tagName)
        {
            #region OriginalCode
            //var chargeStandard = GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId.ToString()));
            //if (string.IsNullOrWhiteSpace(tagName) || chargeStandard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.None) return chargeStandard;
            //var tag = __tagBLL.GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*IsDelete=false", tagName));
            //if (tag == null) return chargeStandard;
            //var equipmentLabelChargeStandard = __equipmentLabelChargeStandardBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*RelationId=\"{1}\"", equipmentId, tag.Id));
            //if (equipmentLabelChargeStandard != null)
            //{
            //    chargeStandard.StandardPrice = equipmentLabelChargeStandard.StandardPrice;
            //    chargeStandard.ChargeTypeEnum = equipmentLabelChargeStandard.ChargeTypeEnum;
            //}
            //return chargeStandard;
            #endregion

            return (ChargeStandard)__chargeStandardRelativeBLL.GetChargeStandardByTagName(equipmentId, tagName);
        }
        public string GetUnitPriceStr(bool? isDurationCharge, com.Bynonco.LIMS.Model.Enum.ChargeType? chargeType, double? unitPrice)
        {
            return __chargeStandardRelativeBLL.GetUnitPriceStr(isDurationCharge, chargeType, unitPrice);
        }
    }
}