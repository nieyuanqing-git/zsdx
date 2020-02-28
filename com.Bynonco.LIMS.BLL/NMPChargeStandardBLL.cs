using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPChargeStandardBLL : BLLBase<NMPChargeStandard>, INMPChargeStandardBLL
    {
        private ITagBLL __tagBLL;
        private IUserBLL __userBLL;
        private ILabOrganizationBLL __labOrganizationBLL;
        private INMPBLL __nmpBLL;
        private INMPLabelBLL __nmpLabelBLL;
        private INMPLabelChargeStandardBLL __nmpLabelChargeStandardBLL;
        private INMPAdditionChargeItemBLL __nmpAdditionChargeItemBLL;
        private INMPCalcFeeTimeRuleBLL __nmpCalcFeeTimeRuleBL;
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private ChargeStandardRelativeBLL __chargeStandardRelativeBLL;
        private const string __FUNCTIONNAME = "getNMPSelfDefinedChargeStatandardPrice";
        public NMPChargeStandardBLL()
        {
            __tagBLL = BLLFactory.CreateTagBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            __nmpBLL = BLLFactory.CreateNMPBLL();
            __nmpCalcFeeTimeRuleBL = BLLFactory.CreateNMPCalcFeeTimeRuleBLL();
            __nmpLabelBLL = BLLFactory.CreateNMPLabelBLL();
            __nmpLabelChargeStandardBLL = BLLFactory.CreateNMPLabelChargeStandardBLL();
            __nmpAdditionChargeItemBLL = BLLFactory.CreateNMPAdditionChargeItemBLL();
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __chargeStandardRelativeBLL = new ChargeStandardRelativeBLL(GetNMPChargeStandardByNMPId,
            __nmpLabelBLL.GetNMPLabelByUserId,
            __nmpLabelChargeStandardBLL.GetNMPLabelChargeStandardListByNMPIdAndLableId,
            __nmpLabelChargeStandardBLL.GetNMPLabelChargeStandard,
            __nmpCalcFeeTimeRuleBL.CalcFeeTime,
            __nmpAdditionChargeItemBLL.GetNMPAdditionChargeItemListByNMPIdAndUserId);

        }
        private IDictionary<string, object> GenerateParamerters(Guid nmpId, Guid? userId, Guid[] nmpPartIds)
        {
            User user = null;
            string userOrgXpath = "", nmpOrgXpath = "";
            IDictionary<string, object> ddParameters = new Dictionary<string, object>();
            if (userId.HasValue) user = __userBLL.GetEntityById(userId.Value);
            if (user != null && user.Organization != null)
            {
                userOrgXpath = user.Organization.XPath;
            }
            var nmp = __nmpBLL.GetEntityById(nmpId);
            if (nmp.OrganizationId.HasValue)
            {
                nmpOrgXpath = __labOrganizationBLL.GetEntityById(nmp.OrganizationId.Value).XPath;
            }
            ddParameters["nmpId"] = nmpId.ToString();
            ddParameters["userOrgXpath"] = userOrgXpath;
            ddParameters["nmpOrgXpath"] = nmpOrgXpath;
            ddParameters["nmpPartIds"] = nmpPartIds;
            return ddParameters;
        }
        private NMPChargeStandard GetNMPChargeStandardByNMPId(Guid nmpId)
        {
            var chargeStandard = GetFirstOrDefaultEntityByExpression(string.Format("NMPId=\"{0}\"", nmpId));
            return chargeStandard;
        }
        public NMPChargeStandard GetNMPChargeStandardByUserId(Guid nmpId, Guid? userId, Guid? subjectId, out double discount)
        {
            return (NMPChargeStandard)__chargeStandardRelativeBLL.GetChargeStandardByUserId(nmpId, userId, subjectId, out discount);
        }
        public NMPChargeStandard GetNMPChargeStandard(Guid nmpId, Guid? userId, Guid? subjectId, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, Guid[] nmpPartIds, out double discount)
        {
            var ddParameters = GenerateParamerters(nmpId, userId, nmpPartIds);
            return (NMPChargeStandard)__chargeStandardRelativeBLL.GetChargeStandard(__FUNCTIONNAME, nmpId, userId, subjectId, usedBeginTime, usedEndTime, appointmentBeginTime, appointmentEndTime, ddParameters, out discount);
        }
        public double GetCalFee(Guid nmpId, Guid userId,Guid? subjectId ,int? sampleCount, DateTime beginTime, DateTime endTime)
        {
            var nmp = __nmpBLL.GetEntityById(nmpId);
            return __chargeStandardRelativeBLL.GetCalFee(__FUNCTIONNAME, nmp.ChargeStandard, nmp.CalcFeeTimeRule, nmpId, userId, subjectId, sampleCount, beginTime, endTime);
        }
        private double CalFee(Guid nmpId, Guid userId, Guid? subjectId, int? sampleCount, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, Guid[] nmpPartIds, out double duration, out double? unitPrice, out int roundDigit, out double discount)
        {
            var ddParameters = GenerateParamerters(nmpId, userId, nmpPartIds);
            var nmp = __nmpBLL.GetEntityById(nmpId);
            return __chargeStandardRelativeBLL.CalFee(__FUNCTIONNAME,
                                                        nmp.ChargeStandard,
                                                        nmp.CalcFeeTimeRule,
                                                        nmpId,
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
        private double CalcPrice(NMP nmp, Guid userId, Guid? subjectId, int? sampleCount, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, Guid[] nmpPartIds, out double duration, out double? unitPrice, out int roundDigits, out double discount)
        {
            var ddParameters = GenerateParamerters(nmp.Id, userId, nmpPartIds);
            return __chargeStandardRelativeBLL.CalcPrice(__FUNCTIONNAME,
                nmp.CalcFeeTimeRule,
                nmp.ChargeStandard,
                nmp.Id,
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
        public NMPChargeStandard GetNMPChargeStandardByTagName(Guid nmpId, string tagName)
        {
            return (NMPChargeStandard)__chargeStandardRelativeBLL.GetChargeStandardByTagName(nmpId, tagName);
        }
        public string GetUnitPriceStr(bool? isDurationCharge, com.Bynonco.LIMS.Model.Enum.ChargeType? chargeType, double? unitPrice)
        {
            return __chargeStandardRelativeBLL.GetUnitPriceStr(isDurationCharge, chargeType, unitPrice);
        }
        public double GetNMPAppointmentCalcFee(NMPAppointment nmpAppointment, out double duration)
        {
            int roundDigit = 2;
            double discount = 1d;
            var isSupressLazyLoad = nmpAppointment.IsSupressLazyLoad;
            nmpAppointment.IsSupressLazyLoad = false;
            double? unitPrice = null;
            double fee = CalFee(nmpAppointment.NMPEquipment.NMPId.Value,
                nmpAppointment.UserId.Value,
                nmpAppointment.SubjectId,
                null,
                nmpAppointment.BeginTime,
                nmpAppointment.EndTime,
                nmpAppointment.BeginTime,
                nmpAppointment.EndTime,
                null,
                out duration,
                out unitPrice,
                out roundDigit,
                out discount);
            if (nmpAppointment.AppointmentAddtionChargeItems != null && nmpAppointment.AppointmentAddtionChargeItems.Count > 0)
            {
                fee += nmpAppointment.AppointmentAddtionChargeItems.Sum(p => p.Fee);
            }
            nmpAppointment.IsSupressLazyLoad = isSupressLazyLoad;
            return Math.Round(fee, roundDigit);
        }
        public double GetNMPUsedConfirmCalFee(NMPUsedConfirm nmpUsedConfirm, out double duration, out double? unitPricet)
        {
            INMPAdditionChargeItemBLL nmpAdditionChargeItemBLL = BLLFactory.CreateNMPAdditionChargeItemBLL();
            var isSupressLazyLoad = nmpUsedConfirm.IsSupressLazyLoad;
            nmpUsedConfirm.IsSupressLazyLoad = false;
            var nmp = nmpUsedConfirm.NMPEquipment.NMP;
            var nmpAppointment = nmpUsedConfirm.GetNMPAppointment();
            DateTime? appointmentBeginTime = null;
            int roundDigit = 2;
            double discount = 1d;
            if (nmpAppointment != null && nmpAppointment.BeginTime.HasValue) appointmentBeginTime = nmpAppointment.BeginTime.Value;
            DateTime? appointmentEndTime = null;
            if (nmpAppointment != null && nmpAppointment.EndTime.HasValue) appointmentEndTime = nmpAppointment.EndTime.Value;
            double fee = CalFee(nmp.Id,
                nmpUsedConfirm.UserId,
                nmpUsedConfirm.SubjectId,
                null,
                nmpUsedConfirm.BeginAt,
                nmpUsedConfirm.EndAt,
                appointmentBeginTime,
                appointmentEndTime,
                null, out duration, out unitPricet, out roundDigit, out discount);
            if (nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems != null && nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems.Count > 0)
            {
                fee += nmpUsedConfirm.NMPUsedConfirmAddtionChargeItems.Sum(p => p.Fee);
            }
            nmpUsedConfirm.IsSupressLazyLoad = isSupressLazyLoad;
            return Math.Round(fee, roundDigit);
        }
    }
}
