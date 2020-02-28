using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public delegate IChargeStandard FunGetChargeStandardByBusinessId(Guid businessId);
    public delegate ILabel FunGetLabelByBusinessAndUserId(Guid businessId, Guid? userId,Guid? subjectId, bool isNeedExistLaeblChargeStandard = false);
    public delegate IList<ILabelChargeStandard> FunGetLabelChargeStandardListByBusinessIdAndLabelId(Guid businessId, Guid labelId);
    public delegate ILabelChargeStandard FunGetLabelChargeStandardByBusinessIdAndRelationId(Guid businessId, Guid relationId);
    public delegate double FunCalcFeeTime(bool isDurationPrice, Guid equipmentId, Guid? userId, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, string luaExpression, int roundFator = 0, int roundDigits = 2);
    public delegate IList<IAdditionChargeItem> FunGetAddtionChargeItemListByBusinessIdAndUserId(Guid businessId, Guid userId,Guid? subjectId);
    public class ChargeStandardRelativeBLL
    {
        private ITagBLL __tagBLL;
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private IOpenFundApplyBLL __openFundApplyBLL;
        private IViewOpenFundDetailBLL __viewOpenFundDetailBLL;
        private FunGetChargeStandardByBusinessId __funGetChargeStandardByBusinessId;
        private FunGetLabelChargeStandardByBusinessIdAndRelationId __funGetLabelChargeStandardByBusinessIdAndRelationId;
        private FunGetLabelByBusinessAndUserId __funGetLabelByBusinessAndUserId;
        private FunGetLabelChargeStandardListByBusinessIdAndLabelId __funGetLabelChargeStandardListByBusinessIdAndLabelId;
        private FunGetAddtionChargeItemListByBusinessIdAndUserId __funGetAddtionChargeItemListByBusinessIdAndUserId;
        private FunCalcFeeTime __funCalcFeeTime;
        public ChargeStandardRelativeBLL(FunGetChargeStandardByBusinessId funGetChargeStandardByBusinessId,
            FunGetLabelByBusinessAndUserId funGetLabelByBusinessAndUserId,
            FunGetLabelChargeStandardListByBusinessIdAndLabelId funGetLabelChargeStandardListByBusinessIdAndLabelId,
            FunGetLabelChargeStandardByBusinessIdAndRelationId funGetLabelChargeStandardByBusinessIdAndRelationId,
            FunCalcFeeTime funCalcFeeTime,
            FunGetAddtionChargeItemListByBusinessIdAndUserId funGetAddtionChargeItemListByBusinessIdAndUserId)
        {
            __tagBLL = BLLFactory.CreateTagBLL();
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __openFundApplyBLL = BLLFactory.CreateOpenFundApplyBLL();
            __viewOpenFundDetailBLL = BLLFactory.CreateViewOpenFundDetailBLL();
            __funGetChargeStandardByBusinessId = funGetChargeStandardByBusinessId;
            __funGetLabelByBusinessAndUserId = funGetLabelByBusinessAndUserId;
            __funGetLabelChargeStandardListByBusinessIdAndLabelId = funGetLabelChargeStandardListByBusinessIdAndLabelId;
            __funGetLabelChargeStandardByBusinessIdAndRelationId = funGetLabelChargeStandardByBusinessIdAndRelationId;
            __funGetAddtionChargeItemListByBusinessIdAndUserId = funGetAddtionChargeItemListByBusinessIdAndUserId;
            __funCalcFeeTime = funCalcFeeTime;
        }
        public IChargeStandard GetChargeStandardByUserId(Guid businessId, Guid? userId,Guid? subjectId, out double discount)
        {
            discount = 1;
            var chargeStandard = __funGetChargeStandardByBusinessId(businessId);
            if (chargeStandard == null) return null;
            if (!userId.HasValue || chargeStandard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.None) return chargeStandard;
            var lable = __funGetLabelByBusinessAndUserId(businessId, userId.Value, subjectId, true);
            if (lable == null) return chargeStandard;
            var labelChargeStandardList = __funGetLabelChargeStandardListByBusinessIdAndLabelId(businessId, lable.Id); 
            if (labelChargeStandardList != null && labelChargeStandardList.Count > 0)
            {
                var labelChargeStandard = labelChargeStandardList.First();
                if (chargeStandard.StandardPrice != 0d)
                {
                    discount = labelChargeStandard.StandardPrice / chargeStandard.StandardPrice;
                }
                chargeStandard.StandardPrice = labelChargeStandard.StandardPrice;
                chargeStandard.ChargeTypeEnum = labelChargeStandard.ChargeTypeEnum;
            }
            return chargeStandard;
        }
        public IChargeStandard GetChargeStandard(string functionName,Guid businessId, Guid? userId, Guid? subjectId, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, IDictionary<string, object> ddParameters, out double discount)
        {
            discount = 1;
            var chargeStandard = __funGetChargeStandardByBusinessId(businessId);
            if (chargeStandard == null) return null;
            if (chargeStandard.ChargeTypeEnum != com.Bynonco.LIMS.Model.Enum.ChargeType.SelfDefined)
            {
                return GetChargeStandardByUserId(businessId, userId,subjectId, out discount);
            }
            if (string.IsNullOrWhiteSpace(chargeStandard.Expression)) throw new Exception("出错,自定义表达式为空");
            if (usedBeginTime != null && usedEndTime != null && appointmentBeginTime == null && appointmentEndTime == null)
            {
                appointmentBeginTime = usedBeginTime;
                appointmentEndTime = usedEndTime;
            }
            if (appointmentBeginTime != null && appointmentEndTime != null && usedBeginTime == null && usedEndTime == null)
            {
                usedBeginTime = appointmentBeginTime;
                usedEndTime = appointmentEndTime;
            }
            IList<object> lstParameterValues = new List<object>();
            lstParameterValues.Add(usedBeginTime);
            lstParameterValues.Add(usedEndTime);
            lstParameterValues.Add(appointmentBeginTime);
            lstParameterValues.Add(appointmentEndTime);
            lstParameterValues.Add(userId.ToString());
            lstParameterValues.Add(chargeStandard.StandardPrice);
            var ddParameterTemp =  ddParameters.Where(p=>p.Key!="usedBeginTime" && p.Key!="usedEndTime" && p.Key!="appointmentBeginTime" && p.Key!="appointmentEndTime" && p.Key!="userId" && p.Key!="price");
            var keys = ddParameterTemp != null ? ddParameterTemp.Select(p => p.Key) : null;
            var values = ddParameterTemp != null ? ddParameterTemp.Select(p => p.Value) : null;
            foreach (var value in values) lstParameterValues.Add(value);
            StringBuilder sbLuaCommand = new StringBuilder();
            sbLuaCommand.AppendFormat("function {0}(usedBeginTime,usedEndTime,appointmentBeginTime,appointmentEndTime,userId,price{1}) ",functionName, keys != null && keys.Count() > 0 ? ("," + string.Join(",", keys)) : "").AppendLine();
            sbLuaCommand.AppendLine(chargeStandard.Expression);
            sbLuaCommand.AppendLine(" end");
            LuaManager.Instance.ExecuteString(sbLuaCommand.ToString());
            var results = LuaManager.Instance.CallFunction(functionName, lstParameterValues.ToArray<object>());
            chargeStandard.StandardPrice = double.Parse(results[0].ToString());
            return chargeStandard;
        }
        public double GetCalFee(string functionName, IChargeStandard chargeStandard, ICalcFeeTimeRule calcFeeTimeRule,Guid businessId, Guid userId,Guid? subjectId, int? sampleCount, DateTime beginTime, DateTime endTime)
        {
            double duration = 0d;
            double? unitPrice = null;
            int roundDigit = 2;
            double discount = 1d;
            var fee = CalFee(functionName, chargeStandard, calcFeeTimeRule, businessId, userId,  sampleCount, beginTime, endTime, null, null, null, out duration, out unitPrice, out roundDigit, out discount, subjectId);
            var additionChargeItemList = __funGetAddtionChargeItemListByBusinessIdAndUserId(businessId, userId,subjectId);
            if (additionChargeItemList != null && additionChargeItemList.Count > 0)
                fee += additionChargeItemList.Sum(p => p.StandardPrice);
            return Math.Round(fee, roundDigit);
        }
        /// <summary>
        /// 计算费用
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="chargeStandard"></param>
        /// <param name="calcFeeTimeRule"></param>
        /// <param name="business"></param>
        /// <param name="userId"></param>
        /// <param name="sampleCount"></param>
        /// <param name="usedBeginTime"></param>
        /// <param name="usedEndTime"></param>
        /// <param name="appointmentBeginTime"></param>
        /// <param name="appointmentEndTime"></param>
        /// <param name="ddParameters"></param>
        /// <param name="duration"></param>
        /// <param name="unitPrice"></param>
        /// <param name="roundDigit"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        public double CalFee(string functionName, IChargeStandard chargeStandard, ICalcFeeTimeRule calcFeeTimeRule, Guid business, Guid userId, int? sampleCount, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, IDictionary<string, object> ddParameters, out double duration, out double? unitPrice, out int roundDigit, out double discount, Guid? subjectId)
        {
            discount = 1;
            roundDigit = 2;
            unitPrice = null;
            double fee = 0d;
            if (chargeStandard.IsDurationPrice && chargeStandard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.ByHour)
            {
                fee = CalcDurationPrice(chargeStandard, calcFeeTimeRule, userId, usedBeginTime.Value, usedEndTime.Value, out duration, out unitPrice);
            }
            else
            {
                fee = CalcPrice(functionName,
                                 calcFeeTimeRule,
                                 chargeStandard,
                                 chargeStandard.BusinessId,
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
                                 out roundDigit,
                                 out discount);
            }
            return fee;
        }
        public double CalcDurationPrice(IChargeStandard chargeStandard, ICalcFeeTimeRule calcFeeTimeRule, Guid userId, DateTime startAt, DateTime endAt, out double duration, out double? unitPrice)
        {
            unitPrice = null;
            duration = 0d;
            if (chargeStandard == null) throw new Exception("出错,未定义计费标准");
            if (chargeStandard.DurationChargeStandards == null || chargeStandard.DurationChargeStandards.Count == 0)throw new Exception("出错,分段计费定义为空");
            double fee = 0d;
            unitPrice = chargeStandard.StandardPrice;
            DateTime start = startAt;
            while (start < endAt)
            {
                var durationPrice = chargeStandard.DurationChargeStandards
                    .FirstOrDefault(x => start.Date.AddHours(x.StartTime) <= start && start.Date.AddHours(x.EndTime) > start);
                if (durationPrice == null) break;
                var end = start.Date.AddHours(durationPrice.EndTime);
                end = end > endAt ? endAt : end;
                if (calcFeeTimeRule != null)
                {
                    var calHour = __funCalcFeeTime(chargeStandard.IsDurationPrice, chargeStandard.BusinessId, userId, start, end, start, end, calcFeeTimeRule.Expression, calcFeeTimeRule.RoundFator, calcFeeTimeRule.RoundDigits);
                    fee += durationPrice.Price * calHour;
                    duration += calHour;
                }
                else fee += durationPrice.Price * (end - start).TotalHours;
                start = end;
            }
            return fee;
        }
        public double CalcPrice(string functionName, ICalcFeeTimeRule calFeeTimeRule, IChargeStandard chargeStandard, Guid businessId, Guid userId, Guid? subjectId, int? sampleCount, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, IDictionary<string, object> ddParameters, out double duration, out double? unitPrice, out int roundDigits, out double discount)
        {
            unitPrice = null;
            duration = -1d;
            roundDigits = 2;
            discount = 1d;
            if (calFeeTimeRule == null) throw new Exception("出错,未定义计算时间规则");
            var chargeStard = GetChargeStandard(functionName, businessId, userId,subjectId, usedBeginTime, usedEndTime, appointmentBeginTime, appointmentEndTime, ddParameters, out discount);
            if (chargeStard == null) throw new Exception("出错,未定义计费标准");
            roundDigits = chargeStard.RoundDigits;
            unitPrice = chargeStard.StandardPrice;
            var calHour = __funCalcFeeTime(chargeStard.IsDurationPrice, businessId, userId, usedBeginTime, usedEndTime, appointmentBeginTime, appointmentEndTime, calFeeTimeRule.Expression, calFeeTimeRule.RoundFator, calFeeTimeRule.RoundDigits);
            duration = calHour;
            if (chargeStard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.ByTimes)
            {
                return chargeStard.StandardPrice;
            }
            if (chargeStard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.BySampleCount && sampleCount.HasValue && sampleCount.Value > 0)//如果样品数为空，按照机时进行扣费
            {
                return sampleCount.Value * chargeStard.StandardPrice;
            }
            var price = calHour * chargeStard.StandardPrice;
            return price;
        }
        public IChargeStandard GetChargeStandardByTagName(Guid businessId, string tagName)
        {
            var chargeStandard = __funGetChargeStandardByBusinessId(businessId);
            if (string.IsNullOrWhiteSpace(tagName) || chargeStandard.ChargeTypeEnum == com.Bynonco.LIMS.Model.Enum.ChargeType.None) return chargeStandard;
            var tag = __tagBLL.GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*IsDelete=false", tagName));
            if (tag == null) return chargeStandard;
            var labelChargeStandard = __funGetLabelChargeStandardByBusinessIdAndRelationId(businessId, tag.Id);
            if (labelChargeStandard != null)
            {
                chargeStandard.StandardPrice = labelChargeStandard.StandardPrice;
                chargeStandard.ChargeTypeEnum = labelChargeStandard.ChargeTypeEnum;
            }
            return chargeStandard;
        }
        public string GetUnitPriceStr(bool? isDurationCharge, com.Bynonco.LIMS.Model.Enum.ChargeType? chargeType, double? unitPrice)
        {
            if (isDurationCharge.HasValue && isDurationCharge.Value) return "分段计费";
            if (chargeType == null || unitPrice == null) return "未定义计费标准";
            var str = unitPrice.Value.ToString();
            switch (chargeType)
            {
                case com.Bynonco.LIMS.Model.Enum.ChargeType.None:
                    str = "免费";
                    break;
                case com.Bynonco.LIMS.Model.Enum.ChargeType.ByHour:
                    str = str == "" ? "" : str + "/小时";
                    break;
                case com.Bynonco.LIMS.Model.Enum.ChargeType.ByTimes:
                    str = str == "" ? "" : str + "/次";
                    break;
                case com.Bynonco.LIMS.Model.Enum.ChargeType.BySampleCount:
                    str = str == "" ? "" : str + "/样品";
                    break;
                case com.Bynonco.LIMS.Model.Enum.ChargeType.SelfDefined:
                    str = "自定义";
                    break;
            }
            return str;
        }
    }
}
