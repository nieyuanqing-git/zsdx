using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary> 成本扣费业务逻辑 </summary>
    public class CostDeductBLL : BLLBase<CostDeduct>, ICostDeductBLL
    {
        private IUserAccountBLL __userAccountBLL;
        private ISystemLogBLL __systemLogBLL;
        private ICostDeductLogBLL __costDeductLogBLL;
        private ICostDeductEquipmentOpenFundBLL __costDeductEquipmentOpenFundBLL;
        private IManualCostDeductBLL __manualCostDeductBLL;
        private IOpenFundApplyBLL __openFundApplyBLL;
        /// <summary> 分段计费逻辑接口 </summary>
        private IChargeStandardBLL __chargeStandardBLL;
        private IViewOpenFundDetailBLL __viewOpenFundDetailBLL;
        private ICostDeductForStatisticsBLL __costDeductForStatisticsBLL;
        private ICostDeductSynBLL __costDeductSynBLL;
        private DataSyncAndCommunicationSettingBLL __dataSyncBLL = new DataSyncAndCommunicationSettingBLL();

        public CostDeductBLL()
        {
            __userAccountBLL = BLLFactory.CreateUserAccountBLL();
            __costDeductLogBLL = BLLFactory.CreateCostDeductLogBLL();
            __systemLogBLL = BLLFactory.CreateSystemLogBLL();
            __costDeductEquipmentOpenFundBLL = BLLFactory.CreateCostDeductEquipmentOpenFundBLL();
            __manualCostDeductBLL = BLLFactory.CreateManualCostDeductBLL();
            __openFundApplyBLL = BLLFactory.CreateOpenFundApplyBLL();
            __chargeStandardBLL = BLLFactory.CreateChargeStandardBLL();
            __viewOpenFundDetailBLL = BLLFactory.CreateViewOpenFundDetailBLL();
            __costDeductForStatisticsBLL = BLLFactory.CreateCostDeductForStatisticsBLL();
            __costDeductSynBLL = BLLFactory.CreateCostDeductSynBLL();
        }
        /// <summary> 创建机时预约扣费记录 </summary>
        /// <param name="appointment">机时预约</param>
        /// <param name="userAccount">用户账号</param>
        /// <param name="viewOpenFundDetailList">开放基金明细视图</param>
        /// <returns>成本扣费实例</returns>
        public CostDeduct CreateAppointmentCostDeduct(Appointment appointment, UserAccount userAccount, IList<ViewOpenFundDetail> viewOpenFundDetailList)
        {
            double duration = 0d;
            bool isEquipmentOpenFundDiscount = false;
            double? realEquipmentOpenFundDiscout = null;
            double openFundApplyFee;
            IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
            appointment.Fee = __chargeStandardBLL.GetAppointmentCalcFee(viewOpenFundDetailList, appointment, out duration, out isEquipmentOpenFundDiscount, out openFundApplyFee, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscout);
            appointment.IsOpenFundCostDeduct = isEquipmentOpenFundDiscount;
            double realCurrency = 0d, virtualCurrency = 0d;
            userAccount.Deduct(appointment.IsOpenFundCostDeduct, appointment.Fee ?? 0, out realCurrency, out virtualCurrency);
            appointment.RealCurrency = realCurrency;
            appointment.VirtualCurrency = virtualCurrency;
            CostDeduct costDeduct = new CostDeduct() { OpenFundDiscount = null };
            costDeduct.CostDeductEquipmentOpenFunds = costDeductEquipmentOpenFunds;
            costDeduct.CalcDuration = appointment.CalDuration;
            costDeduct.CostDeductTypeEnum = Model.Enum.CostDeductType.AppointmentPredictCostDeduct;
            costDeduct.CreaterId = appointment.CreaterId;
            costDeduct.DeductAt = DateTime.Now;
            costDeduct.Duration = appointment.Duration;
            costDeduct.HasClossingAccount = false;
            costDeduct.Id = Guid.NewGuid();
            costDeduct.RealCurrency = realCurrency;
            costDeduct.VirtualCurrency = virtualCurrency;
            costDeduct.UserAccountId = userAccount.Id;
            costDeduct.AppointmentId = appointment.Id;
            costDeduct.IsOpenFundCostDeduct = isEquipmentOpenFundDiscount;

            if (costDeduct.IsOpenFundCostDeduct) costDeduct.OpenFundDiscount = __openFundApplyBLL.GetEquipmentOpenFundDiscount();
            return costDeduct;
        }
        /// <summary> 处理并通过预约获取预约扣费 </summary>
        /// <param name="appointment"></param>
        /// <param name="subjectId"></param>
        /// <param name="userId"></param>
        /// <param name="paymentMethod"></param>
        /// <returns>预约扣费</returns>
        public CostDeduct HandleAppointmentCostDeduct(Appointment appointment, out Guid subjectId, out Guid userId, out PaymentMethod paymentMethod)
        {
            userId = appointment.UserId.Value;
            subjectId = appointment.SubjectId.Value;
            paymentMethod = appointment.PaymentMethodEnum;
            var costDeduct = GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"", appointment.Id.ToString()));
            return costDeduct;
        }
        public object GetCostDeductPriviliges(Guid? userId, IList<ViewCostDeduct> viewCostDeductList)
        {
            IList<CostDeductPrivilige> lstCostDeductPriviliges = new List<CostDeductPrivilige>();
            if (userId.HasValue && viewCostDeductList != null && viewCostDeductList.Count > 0)
            {
                foreach (ViewCostDeduct viewCostDeduct in viewCostDeductList)
                {
                    CostDeductPrivilige costDeductPrivilige = lstCostDeductPriviliges.FirstOrDefault(p => p.CostDeductId.HasValue && p.CostDeductId.Value == viewCostDeduct.Id);
                    if (costDeductPrivilige == null)
                    {
                        costDeductPrivilige = PriviligeFactory.GetCostDeductPrivilige(userId.Value, viewCostDeduct.Id);
                        lstCostDeductPriviliges.Add(costDeductPrivilige);
                    }
                }
            }
            return lstCostDeductPriviliges;
        }
        private void GenerateCostDeductLogData(OperateType operateType, CostDeduct originalCostDeduct, CostDeduct curCostDeduct, out string content, out string contentHTML, out string entityName, out string entityCnName)
        {
            entityName = "CostDeduct";
            entityCnName = "扣费记录";
            StringBuilder sbContnet = new StringBuilder();
            try
            {
                sbContnet.AppendFormat("扣费编码:【{0}】→【{1}】", originalCostDeduct.Id, curCostDeduct.Id).Append("\r\n");
                sbContnet.AppendFormat("创建人:【{0}】→【{1}】", originalCostDeduct.CreaterId.HasValue ? originalCostDeduct.Creator.UserName : "", curCostDeduct.CreaterId.HasValue ? curCostDeduct.Creator.UserName : "").Append("\r\n");
                sbContnet.AppendFormat("创建时间:【{0}】→【{1}】", originalCostDeduct.DeductAt.HasValue ? originalCostDeduct.DeductAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curCostDeduct.DeductAt.HasValue ? curCostDeduct.DeductAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                sbContnet.AppendFormat("支付人:【{0}】→【{1}】", originalCostDeduct.UserAccount.GetUser().UserName, curCostDeduct.UserAccount.GetUser().UserName).Append("\r\n");
                sbContnet.AppendFormat("状态:【{0}】→【{1}】", originalCostDeduct.HasClossingAccountStr, curCostDeduct.HasClossingAccountStr).Append("\r\n");
                sbContnet.AppendFormat("虚拟币:【{0}】→【{1}】", originalCostDeduct.RealCurrency.Value, curCostDeduct.RealCurrency.Value).Append("\r\n");
                sbContnet.AppendFormat("真实币:【{0}】→【{1}】", originalCostDeduct.VirtualCurrency.Value, curCostDeduct.VirtualCurrency.Value).Append("\r\n");
                sbContnet.AppendFormat("是否开放基金:【{0}】→【{1}】", originalCostDeduct.IsOpenFundCostDeductStr, curCostDeduct.IsOpenFundCostDeductStr).Append("\r\n");
                sbContnet.AppendFormat("开放基金折扣比:【{0}】→【{1}】", originalCostDeduct.OpenFundDiscount, curCostDeduct.OpenFundDiscount).Append("\r\n");
                sbContnet.AppendFormat("扣费类型:【{0}】→【{1}】", originalCostDeduct.CostDeductTypeEnum.ToCnName(), curCostDeduct.CostDeductTypeEnum.ToCnName()).Append("\r\n");

                if (curCostDeduct.ManualCostDeductId != null)
                {
                    entityName = "ManualCostDeduct";
                    entityCnName = "手工扣费记录";
                    var originalManualCostDeduct = operateType == OperateType.New ? originalCostDeduct.ManualCostDeductForLog : originalCostDeduct.ManualCostDeduct;
                    var curManualCostDeduct = operateType != OperateType.Deleted ? curCostDeduct.ManualCostDeductForLog : curCostDeduct.ManualCostDeduct;
                    sbContnet.AppendFormat("手工扣费编码:【{0}】→【{1}】", originalManualCostDeduct.Id, curManualCostDeduct.Id).Append("\r\n");
                    sbContnet.AppendFormat("名目:【{0}】→【{1}】", originalManualCostDeduct.Name, curManualCostDeduct.Name).Append("\r\n");
                    sbContnet.AppendFormat("使用者:【{0}】→【{1}】", originalManualCostDeduct.User.UserName, curManualCostDeduct.User.UserName).Append("\r\n");
                    sbContnet.AppendFormat("课题组:【{0}】→【{1}】", originalManualCostDeduct.Subject.Name, curManualCostDeduct.Subject.Name).Append("\r\n");
                    sbContnet.AppendFormat("项目名称:【{0}】→【{1}】", originalManualCostDeduct.SubjectProjectId.HasValue ? originalManualCostDeduct.SubjectProject.Name : "", curManualCostDeduct.SubjectProjectId.HasValue ? curManualCostDeduct.SubjectProject.Name : "").Append("\r\n");
                    sbContnet.AppendFormat("金额:【{0}】→【{1}】", originalManualCostDeduct.Money, curManualCostDeduct.Money).Append("\r\n");
                    sbContnet.AppendFormat("备注:【{0}】→【{1}】", originalManualCostDeduct.Remark, curManualCostDeduct.Remark).Append("\r\n");
                    sbContnet.AppendFormat("支付方式:【{0}】→【{1}】", originalManualCostDeduct.PaymentMethodEnum.ToCnName(), curManualCostDeduct.PaymentMethodEnum.ToCnName()).Append("\r\n");
                }

                if (curCostDeduct.UsedConfirmId.HasValue)
                {
                    entityName = "CostDeduct";
                    entityCnName = "使用扣费记录";
                    var originalUsedConfirm = operateType == OperateType.New ? originalCostDeduct.UsedConfirmForLog : originalCostDeduct.GetUsedConfirm();
                    var curUsedConfirm = operateType != OperateType.Deleted ? curCostDeduct.UsedConfirmForLog : curCostDeduct.GetUsedConfirm();
                    sbContnet.AppendFormat("使用记录编码:【{0}】→【{1}】", originalUsedConfirm.Id, curUsedConfirm.Id).Append("\r\n");
                    sbContnet.AppendFormat("预约记录编码:【{0}】→【{1}】", originalUsedConfirm.AppointmentId, curUsedConfirm.AppointmentId).Append("\r\n");
                    sbContnet.AppendFormat("使用设备:【{0}】→【{1}】", originalUsedConfirm.Equipment.Name, curUsedConfirm.Equipment.Name).Append("\r\n");
                    sbContnet.AppendFormat("使用开始时间:【{0}】→【{1}】", originalUsedConfirm.BeginAt.HasValue ? originalUsedConfirm.BeginAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curUsedConfirm.BeginAt.HasValue ? curUsedConfirm.BeginAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                    sbContnet.AppendFormat("使用结束时间:【{0}】→【{1}】", originalUsedConfirm.EndAt.HasValue ? originalUsedConfirm.EndAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curUsedConfirm.EndAt.HasValue ? curUsedConfirm.EndAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                    sbContnet.AppendFormat("使用者:【{0}】→【{1}】", originalUsedConfirm.User.UserName, curUsedConfirm.User.UserName).Append("\r\n");
                    sbContnet.AppendFormat("课题组:【{0}】→【{1}】", originalUsedConfirm.Subject.Name, curUsedConfirm.Subject.Name).Append("\r\n");
                    sbContnet.AppendFormat("项目名称:【{0}】→【{1}】", originalUsedConfirm.SubjectProjectId.HasValue ? originalUsedConfirm.SubjectProject.Name : "", curUsedConfirm.SubjectProjectId.HasValue ? curUsedConfirm.SubjectProject.Name : "").Append("\r\n");
                    sbContnet.AppendFormat("金额:【{0}】→【{1}】", originalUsedConfirm.RealFee.HasValue ? originalUsedConfirm.RealFee.Value.ToString("0.00") : "0", curUsedConfirm.RealFee.HasValue ? curUsedConfirm.RealFee.Value.ToString("0.00") : "0").Append("\r\n");
                    sbContnet.AppendFormat("使用备注:【{0}】→【{1}】", originalUsedConfirm.Remark, curUsedConfirm.Remark).Append("\r\n");
                    sbContnet.AppendFormat("支付方式:【{0}】→【{1}】", originalUsedConfirm.PaymentMethodEnum.ToCnName(), curUsedConfirm.PaymentMethodEnum.ToCnName()).Append("\r\n");
                    sbContnet.AppendFormat("计费时长:【{0}】→【{1}】", originalUsedConfirm.CalcDuration, curUsedConfirm.CalcDuration).Append("\r\n");
                    sbContnet.AppendFormat("计费方式:【{0}】→【{1}】", originalUsedConfirm.ChargeTypeEnum.ToCnName(), curUsedConfirm.ChargeTypeEnum.ToCnName()).Append("\r\n");
                    sbContnet.AppendFormat("实际时长:【{0}】→【{1}】", originalUsedConfirm.Duration, curUsedConfirm.Duration).Append("\r\n");
                    sbContnet.AppendFormat("计算费用:【{0}】→【{1}】", originalUsedConfirm.CalcFee, curUsedConfirm.CalcFee).Append("\r\n");
                    sbContnet.AppendFormat("实际费用:【{0}】→【{1}】", originalUsedConfirm.RealFee.HasValue ? originalUsedConfirm.RealFee.Value.ToString("0.00") : "0", curUsedConfirm.RealFee.HasValue ? curUsedConfirm.RealFee.Value.ToString("0.00") : "0").Append("\r\n");
                    sbContnet.AppendFormat("取整因子:【{0}】→【{1}】", originalUsedConfirm.RoundFator, curUsedConfirm.RoundFator).Append("\r\n");
                    sbContnet.AppendFormat("小数位数:【{0}】→【{1}】", originalUsedConfirm.RoundDigits, curUsedConfirm.RoundDigits).Append("\r\n");
                    sbContnet.AppendFormat("计费表达式:【{0}】→【{1}】", originalUsedConfirm.Expression, curUsedConfirm.Expression).Append("\r\n");
                }


                if (curCostDeduct.AppointmentId.HasValue && curCostDeduct.UsedConfirmId == null)
                {
                    var originalAppointment = operateType == OperateType.New ? originalCostDeduct.AppointmentForLog : originalCostDeduct.Appointment;
                    var curAppointment = operateType != OperateType.Deleted ? curCostDeduct.AppointmentForLog : curCostDeduct.Appointment;
                    if (originalAppointment != null && originalAppointment != null)
                    {
                        sbContnet.AppendFormat("预约记录编码:【{0}】→【{1}】", originalAppointment.Id, curAppointment.Id).Append("\r\n");
                        sbContnet.AppendFormat("预约设备:【{0}】→【{1}】", originalAppointment.Equipment.Name, curAppointment.Equipment.Name).Append("\r\n");
                        sbContnet.AppendFormat("预约开始时间:【{0}】→【{1}】", originalAppointment.BeginTime.HasValue ? originalAppointment.BeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curAppointment.BeginTime.HasValue ? curAppointment.BeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                        sbContnet.AppendFormat("预约结束时间:【{0}】→【{1}】", originalAppointment.EndTime.HasValue ? originalAppointment.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curAppointment.EndTime.HasValue ? curAppointment.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                        sbContnet.AppendFormat("使用者:【{0}】→【{1}】", originalAppointment.User.UserName, curAppointment.User.UserName).Append("\r\n");
                        sbContnet.AppendFormat("课题组:【{0}】→【{1}】", originalAppointment.Subject.Name, curAppointment.Subject.Name).Append("\r\n");
                        sbContnet.AppendFormat("项目名称:【{0}】→【{1}】", originalAppointment.SubjectProjectId.HasValue ? originalAppointment.SubjectProject.Name : "", curAppointment.SubjectProjectId.HasValue ? curAppointment.SubjectProject.Name : "").Append("\r\n");
                        sbContnet.AppendFormat("金额:【{0}】→【{1}】", originalAppointment.Fee.HasValue ? originalAppointment.Fee.Value.ToString("0.00") : "0", curAppointment.Fee.HasValue ? curAppointment.Fee.Value.ToString("0.00") : "0").Append("\r\n");
                        sbContnet.AppendFormat("申请人:【{0}】→【{1}】", originalAppointment.Creator != null ? originalAppointment.Creator.UserName : "", curAppointment.Creator != null ? curAppointment.Creator.UserName : "").Append("\r\n");
                        sbContnet.AppendFormat("申请时间:【{0}】→【{1}】", originalAppointment.ApplyTime.HasValue ? originalAppointment.ApplyTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curAppointment.ApplyTime.HasValue ? curAppointment.ApplyTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                        sbContnet.AppendFormat("改约人:【{0}】→【{1}】", originalAppointment.Changer != null ? originalAppointment.Changer.UserName : "", curAppointment.Changer != null ? curAppointment.Changer.UserName : "").Append("\r\n");
                        sbContnet.AppendFormat("改约时间:【{0}】→【{1}】", originalAppointment.ChangedTime.HasValue ? originalAppointment.ChangedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curAppointment.ChangedTime.HasValue ? curAppointment.ChangedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                        sbContnet.AppendFormat("预约备注:【{0}】→【{1}】", originalAppointment.WhyUnuseable, curAppointment.WhyUnuseable).Append("\r\n");
                        sbContnet.AppendFormat("预约状态:【{0}】→【{1}】", originalAppointment.StatusEnum.ToCnName(), curAppointment.StatusEnum.ToCnName()).Append("\r\n");
                        sbContnet.AppendFormat("支付方式:【{0}】→【{1}】", originalAppointment.PaymentMethodEnum.ToCnName(), curAppointment.PaymentMethodEnum.ToCnName()).Append("\r\n");
                        sbContnet.AppendFormat("计费时长:【{0}】→【{1}】", originalAppointment.CalDuration, curAppointment.CalDuration).Append("\r\n");
                        sbContnet.AppendFormat("实际时长:【{0}】→【{1}】", originalAppointment.Duration, curAppointment.Duration).Append("\r\n");
                        sbContnet.AppendFormat("备注选项:【{0}】→【{1}】", originalAppointment.DesOptions, curAppointment.DesOptions).Append("\r\n");
                        sbContnet.AppendFormat("审核人:【{0}】→【{1}】", originalAppointment.Auditor != null ? originalAppointment.Auditor.Name : "", curAppointment.Auditor != null ? curAppointment.Auditor.Name : "").Append("\r\n");
                        sbContnet.AppendFormat("审核状态:【{0}】→【{1}】", originalAppointment.AuditStatusEnum.ToCnName(), curAppointment.AuditStatusEnum.ToCnName()).Append("\r\n");
                        sbContnet.AppendFormat("审核备注:【{0}】→【{1}】", originalAppointment.AuditingNoPassWhy, curAppointment.AuditingNoPassWhy).Append("\r\n");
                        entityName = "CostDeduct";
                        entityCnName = "预约预扣费记录";
                    }
                }
                if (curCostDeduct.SampleApplyId.HasValue)
                {
                    entityName = "CostDeduct";
                    entityCnName = "送样扣费记录";
                    var originalSampleApply = operateType == OperateType.New ? originalCostDeduct.SampleApplyForLog : originalCostDeduct.SampleApply;
                    var curSampleApply = operateType != OperateType.Deleted ? curCostDeduct.SampleApplyForLog : curCostDeduct.SampleApply;
                    sbContnet.AppendFormat("送样记录编码:【{0}】→【{1}】", originalSampleApply.Id, curSampleApply.Id).Append("\r\n");
                    sbContnet.AppendFormat("样品编号:【{0}】→【{1}】", originalSampleApply.SampleNo + "~" + originalSampleApply.RowNo, curSampleApply.SampleNo + "~" + curSampleApply.RowNo).Append("\r\n");
                    sbContnet.AppendFormat("送样设备:【{0}】→【{1}】", originalSampleApply.SampleItem.GetEquipment().Name, curSampleApply.SampleItem.GetEquipment().Name).Append("\r\n");
                    sbContnet.AppendFormat("送样项目:【{0}】→【{1}】", originalSampleApply.SampleItem.Name, curSampleApply.SampleItem.Name).Append("\r\n");
                    sbContnet.AppendFormat("样品数:【{0}】→【{1}】", originalSampleApply.Quatity, curSampleApply.Quatity).Append("\r\n");
                    sbContnet.AppendFormat("实际样品数:【{0}】→【{1}】", originalSampleApply.RealQuatity, curSampleApply.RealQuatity).Append("\r\n");
                    sbContnet.AppendFormat("送样时间:【{0}】→【{1}】", originalSampleApply.ExpectSendDate.HasValue ? originalSampleApply.ExpectSendDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", curSampleApply.ExpectSendDate.HasValue ? curSampleApply.ExpectSendDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("\r\n");
                    sbContnet.AppendFormat("送样人:【{0}】→【{1}】", originalSampleApply.Applicant.UserName, curSampleApply.Applicant.UserName).Append("\r\n");
                    sbContnet.AppendFormat("课题组:【{0}】→【{1}】", originalSampleApply.Subject.Name, curSampleApply.Subject.Name).Append("\r\n");
                    sbContnet.AppendFormat("金额:【{0}】→【{1}】", originalSampleApply.UnitPrice, curSampleApply.UnitPrice).Append("\r\n");
                    sbContnet.AppendFormat("使用备注:【{0}】→【{1}】", originalSampleApply.Remark, curSampleApply.Remark).Append("\r\n");
                    sbContnet.AppendFormat("支付方式:【{0}】→【{1}】", originalSampleApply.PaymentMethodEnum.ToCnName(), curSampleApply.PaymentMethodEnum.ToCnName()).Append("\r\n");
                    sbContnet.AppendFormat("计费方式:【{0}】→【{1}】", originalSampleApply.ChargeCategoryEnum.ToCnName(), curSampleApply.ChargeCategoryEnum.ToCnName()).Append("\r\n");
                    sbContnet.AppendFormat("状态:【{0}】→【{1}】", originalSampleApply.StatusEnum.ToCnName(), curSampleApply.StatusEnum.ToCnName()).Append("\r\n");
                    sbContnet.AppendFormat("收费状态:【{0}】→【{1}】", originalSampleApply.ChargeStatusEnum.ToCnName(), curSampleApply.ChargeStatusEnum.ToCnName()).Append("\r\n");
                }

                if (curCostDeduct.WaterControlCostDeduct != null)
                {
                    entityName = "WaterControlCostDeduct";
                    entityCnName = "水控扣费记录";
                    var originalWaterControlCostDeduct = operateType == OperateType.New ? originalCostDeduct.WaterControlCostDeductForLog : originalCostDeduct.WaterControlCostDeduct;
                    var curWaterControlCostDeduct = operateType != OperateType.Deleted ? curCostDeduct.WaterControlCostDeductForLog : curCostDeduct.WaterControlCostDeduct;
                    sbContnet.AppendFormat("水控扣费编码:【{0}】→【{1}】", originalWaterControlCostDeduct.Id, curWaterControlCostDeduct.Id).Append("\r\n");
                    sbContnet.AppendFormat("名目:【{0}】→【{1}】", originalWaterControlCostDeduct.Name, curWaterControlCostDeduct.Name).Append("\r\n");
                    sbContnet.AppendFormat("使用者:【{0}】→【{1}】", originalWaterControlCostDeduct.User.UserName, curWaterControlCostDeduct.User.UserName).Append("\r\n");
                    sbContnet.AppendFormat("课题组:【{0}】→【{1}】", originalWaterControlCostDeduct.Subject.Name, curWaterControlCostDeduct.Subject.Name).Append("\r\n");
                    sbContnet.AppendFormat("项目名称:【{0}】→【{1}】", originalWaterControlCostDeduct.SubjectProjectId.HasValue ? curWaterControlCostDeduct.SubjectProject.Name : "", curWaterControlCostDeduct.SubjectProjectId.HasValue ? curWaterControlCostDeduct.SubjectProject.Name : "").Append("\r\n");
                    sbContnet.AppendFormat("金额:【{0}】→【{1}】", originalWaterControlCostDeduct.Money, curWaterControlCostDeduct.Money).Append("\r\n");
                    sbContnet.AppendFormat("备注:【{0}】→【{1}】", originalWaterControlCostDeduct.Remark, curWaterControlCostDeduct.Remark).Append("\r\n");
                    sbContnet.AppendFormat("支付方式:【{0}】→【{1}】", originalWaterControlCostDeduct.PaymentMethodEnum.ToCnName(), curWaterControlCostDeduct.PaymentMethodEnum.ToCnName()).Append("\r\n");
                }
                //if (curCostDeduct.AnimalCostDeductId.HasValue)
                //{
                //    sbContnet.AppendFormat("动物扣费编码:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.Id, curCostDeduct.AnimalCostDeduct.Id).Append("\r\n");
                //    sbContnet.AppendFormat("名目:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.Name, curCostDeduct.AnimalCostDeduct.Name).Append("\r\n");
                //    sbContnet.AppendFormat("动物:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.Animal.Name, curCostDeduct.AnimalCostDeduct.Animal.Name).Append("\r\n");
                //    sbContnet.AppendFormat("动物品系:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.Animal.AnimalCategory.Name, curCostDeduct.AnimalCostDeduct.Animal.AnimalCategory.Name).Append("\r\n");
                //    sbContnet.AppendFormat("扣费开始时间:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.BeginDate.ToString("yyyy-MM-dd HH:mm:ss"), curCostDeduct.AnimalCostDeduct.BeginDate.ToString("yyyy-MM-dd HH:mm:ss")).Append("\r\n");
                //    sbContnet.AppendFormat("扣费结束时间:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.EndDate.ToString("yyyy-MM-dd HH:mm:ss"), curCostDeduct.AnimalCostDeduct.EndDate.ToString("yyyy-MM-dd HH:mm:ss")).Append("\r\n");
                //    sbContnet.AppendFormat("使用者:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.User.UserName, curCostDeduct.AnimalCostDeduct.User.UserName).Append("\r\n");
                //    sbContnet.AppendFormat("课题组:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.Subject.Name, curCostDeduct.AnimalCostDeduct.Subject.Name).Append("\r\n");
                //    sbContnet.AppendFormat("项目名称:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.SubjectProjectId.HasValue ? originalCostDeduct.AnimalCostDeduct.SubjectProject.Name : "", curCostDeduct.ManualCostDeduct.SubjectProjectId.HasValue ? curCostDeduct.ManualCostDeduct.SubjectProject.Name : "").Append("\r\n");
                //    sbContnet.AppendFormat("金额:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.Money, curCostDeduct.AnimalCostDeduct.Money).Append("\r\n");
                //    sbContnet.AppendFormat("支付方式:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.PaymentMethodEnum.ToCnName(), curCostDeduct.AnimalCostDeduct.PaymentMethodEnum.ToCnName()).Append("\r\n");
                //    sbContnet.AppendFormat("备注:【{0}】→【{1}】", originalCostDeduct.AnimalCostDeduct.Remark, curCostDeduct.AnimalCostDeduct.Remark).Append("\r\n");
                //}
            }
            catch (Exception ex) { content = ""; contentHTML = ""; }
            content = sbContnet.ToString();
            contentHTML = content.Replace("\r\n", "<br />");
        }
        private void GenerateCostDeductLog(OperateType operateType, Guid id, Guid? operatorId, string operateIP, ref XTransaction tran, bool isSupress)
        {
            var entity = GetEntityById(id);
            GenerateCostDeductLog(operateType, entity, entity, operatorId, operateIP, ref tran, isSupress);
        }
        private void GenerateCostDeductLog(OperateType operateType, CostDeduct originalEntity, CostDeduct curEntity, Guid? operatorId, string operateIP, ref XTransaction tran, bool isSupress)
        {
            string content = "", contentHTML = "", entityName = "CostDeduct", entityCnName = "扣费记录";
            GenerateCostDeductLogData(operateType, originalEntity, curEntity, out content, out contentHTML, out entityName, out entityCnName);
            if (!string.IsNullOrWhiteSpace(content))
            {
                SystemLog systemLog = new SystemLog()
                {
                    BusinessId = curEntity.Id,
                    Id = Guid.NewGuid(),
                    OperateEntityName = entityName,
                    OperateEntityCnName = entityCnName,
                    OperateTime = DateTime.Now,
                    OperateTypeEnum = operateType,
                    OperateIP = operateIP,
                    LogContent = content,
                    LogContentHTML = contentHTML,
                    OperatorId = operatorId
                };
                __systemLogBLL.Add(new SystemLog[] { systemLog }, ref tran, isSupress);
            }
        }
        private void DelCostDeductForStatistics(Guid[] ids, ref XTransaction tran)
        {
            var costDeductForStatisticsList = __costDeductForStatisticsBLL.GetEntitiesByExpression(ids.ToFormatStr("CostDeductId"));
            if (costDeductForStatisticsList != null && costDeductForStatisticsList.Count > 0)
            {
                __costDeductForStatisticsBLL.Delete(costDeductForStatisticsList.Select(p => p.Id), ref tran, true);
            }
        }
        private void GenerateCostDeductForStatistics(CostDeduct costDeduct, ref XTransaction tran)
        {
            var costDeductForStatistics = new CostDeductForStatistics()
            {
                Id = Guid.NewGuid(),
                CostDeductId = costDeduct.Id,
                DeductAt = costDeduct.DeductAt,
                HasClossingAccount = costDeduct.HasClossingAccount,
                VirtualCurrency = costDeduct.VirtualCurrency.Value,
                RealCurrency = costDeduct.RealCurrency.Value,
                UserAccountId = costDeduct.UserAccountId,
                CreaterId = costDeduct.CreaterId,
                CostDeductType = costDeduct.CostDeductType,
                Duration = costDeduct.Duration,
                CalcDuration = costDeduct.CalcDuration,
                BalanceAccountItemId = costDeduct.BalanceAccountItemId,
                Status = costDeduct.Status,
                IsOpenFundCostDeduct = costDeduct.IsOpenFundCostDeduct,
                OpenFundDiscount = costDeduct.OpenFundDiscount,
                OpenFundCurrency = costDeduct.OpenFundCurrency,
                TotalCurrency = costDeduct.RealCurrency.Value + costDeduct.VirtualCurrency.Value
            };
            if (costDeduct.CostDeductTypeEnum == CostDeductType.Animal && costDeduct.AnimalCostDeduct != null)
            {
                costDeductForStatistics.PaymentMethod = costDeduct.AnimalCostDeduct.PaymentMethod;
                costDeductForStatistics.Title = costDeduct.AnimalCostDeduct.Name;
                costDeductForStatistics.SubjectProjectId = costDeduct.AnimalCostDeduct.SubjectProjectId;
                costDeductForStatistics.Remark = costDeduct.AnimalCostDeduct.Remark;
                costDeductForStatistics.SubjectId = costDeduct.AnimalCostDeduct.SubjectId;
                costDeductForStatistics.AnimalId = costDeduct.AnimalCostDeduct.AnimalId;
                costDeductForStatistics.UserId = costDeduct.AnimalCostDeduct.UserId;
                costDeductForStatistics.BusinessId = costDeduct.AnimalCostDeduct.Id;
            }
            if (costDeduct.CostDeductTypeEnum == CostDeductType.AppointmentPredictCostDeduct && costDeduct.AppointmentForLog != null)
            {
                costDeductForStatistics.PaymentMethod = costDeduct.AppointmentForLog.PaymentMethod;
                costDeductForStatistics.SubjectProjectId = costDeduct.AppointmentForLog.SubjectProjectId;
                costDeductForStatistics.SubjectId = costDeduct.AppointmentForLog.SubjectId;
                costDeductForStatistics.UserId = costDeduct.AppointmentForLog.UserId;
                costDeductForStatistics.BusinessId = costDeduct.AppointmentForLog.Id;
                costDeductForStatistics.EquipmentId = costDeduct.AppointmentForLog.EquipmentId;
                costDeductForStatistics.SampleCount = costDeduct.AppointmentForLog.SampleCount;
                costDeductForStatistics.BeginAt = costDeduct.AppointmentForLog.BeginTime;
                costDeductForStatistics.EndAt = costDeduct.AppointmentForLog.EndTime;
            }
            if (costDeduct.CostDeductTypeEnum == CostDeductType.ManualCostDeduct && costDeduct.ManualCostDeductForLog != null)
            {
                costDeductForStatistics.PaymentMethod = costDeduct.ManualCostDeductForLog.PaymentMethod;
                costDeductForStatistics.Title = costDeduct.ManualCostDeductForLog.Name;
                costDeductForStatistics.SubjectProjectId = costDeduct.ManualCostDeductForLog.SubjectProjectId;
                costDeductForStatistics.Remark = costDeduct.ManualCostDeductForLog.Remark;
                costDeductForStatistics.SubjectId = costDeduct.ManualCostDeductForLog.SubjectId;
                costDeductForStatistics.UserId = costDeduct.ManualCostDeductForLog.UserId;
                costDeductForStatistics.BusinessId = costDeduct.ManualCostDeductForLog.Id;
            }
            if (costDeduct.CostDeductTypeEnum == CostDeductType.MaterialCostDeduct && costDeduct.MaterialOutput != null)
            {
                costDeductForStatistics.PaymentMethod = (int)PaymentMethod.SubjectDirectorPay;
                costDeductForStatistics.Title = costDeduct.MaterialOutput.OutputNum;
                costDeductForStatistics.SubjectProjectId = costDeduct.MaterialOutput.SubjectProjectId;
                costDeductForStatistics.Remark = costDeduct.MaterialOutput.Remark;
                costDeductForStatistics.SubjectId = costDeduct.MaterialOutput.SubjectId;
                costDeductForStatistics.UserId = costDeduct.MaterialOutput.OutputUserId;
                costDeductForStatistics.BusinessId = costDeduct.MaterialOutput.Id;
            }
            if (costDeduct.CostDeductTypeEnum == CostDeductType.SampleCostDeduct && costDeduct.SampleApplyForLog != null)
            {
                costDeductForStatistics.PaymentMethod = costDeduct.SampleApplyForLog.PaymentMethod;
                costDeductForStatistics.SubjectProjectId = costDeduct.SampleApplyForLog.SubjectProjectId;
                costDeductForStatistics.SubjectId = costDeduct.SampleApplyForLog.SubjectId;
                costDeductForStatistics.UserId = costDeduct.SampleApplyForLog.ApplicantId;
                costDeductForStatistics.BusinessId = costDeduct.SampleApplyForLog.Id;
                costDeductForStatistics.EquipmentId = costDeduct.SampleApplyForLog.SampleItem.EquipmentId;
                costDeductForStatistics.SampleItemId = costDeduct.SampleApplyForLog.SampleItemId;
                costDeductForStatistics.UnitPrice = costDeduct.SampleApplyForLog.UnitPrice;
                costDeductForStatistics.SampleCount = costDeduct.SampleApplyForLog.Quatity;
            }
            if (costDeduct.CostDeductTypeEnum == CostDeductType.UsedCostDeduct && costDeduct.UsedConfirmForLog != null)
            {
                costDeductForStatistics.PaymentMethod = costDeduct.UsedConfirmForLog.PaymentMethod;
                costDeductForStatistics.SubjectProjectId = costDeduct.UsedConfirmForLog.SubjectProjectId;
                costDeductForStatistics.SubjectId = costDeduct.UsedConfirmForLog.SubjectId;
                costDeductForStatistics.UserId = costDeduct.UsedConfirmForLog.UserId;
                costDeductForStatistics.BusinessId = costDeduct.UsedConfirmForLog.Id;
                costDeductForStatistics.EquipmentId = costDeduct.UsedConfirmForLog.EquipmentId;
                costDeductForStatistics.UnitPrice = costDeduct.UsedConfirmForLog.UnitPrice;
                costDeductForStatistics.RealFee = costDeduct.UsedConfirmForLog.RealFee;
                costDeductForStatistics.CalcFee = costDeduct.UsedConfirmForLog.CalcFee;
                costDeductForStatistics.BeginAt = costDeduct.UsedConfirmForLog.BeginAt;
                costDeductForStatistics.EndAt = costDeduct.UsedConfirmForLog.EndAt;
                costDeductForStatistics.ChargeType = costDeduct.UsedConfirmForLog.ChargeType;
            }
            if (costDeduct.CostDeductTypeEnum == CostDeductType.UsedCostDeduct && costDeduct.WaterControlCostDeductForLog != null)
            {
                costDeductForStatistics.PaymentMethod = costDeduct.WaterControlCostDeductForLog.PaymentMethod;
                costDeductForStatistics.SubjectProjectId = costDeduct.WaterControlCostDeductForLog.SubjectProjectId;
                costDeductForStatistics.SubjectId = costDeduct.WaterControlCostDeductForLog.SubjectId;
                costDeductForStatistics.UserId = costDeduct.WaterControlCostDeductForLog.UserId;
                costDeductForStatistics.BusinessId = costDeduct.WaterControlCostDeductForLog.Id;
                costDeductForStatistics.Remark = costDeduct.WaterControlCostDeductForLog.Remark;
            }
            __costDeductForStatisticsBLL.Add(new CostDeductForStatistics[] { costDeductForStatistics }, ref tran, true);
        }

        /// <summary> 新建扣费
        /// 开放基金预扣费归还、生成扣费日志、生成扣费统计
        /// </summary>
        /// <param name="viewOpenFundDetailList"></param>
        /// <param name="entities"></param>
        /// <param name="preCostDeductEquipmentOpenFunds"></param>
        /// <param name="tran"></param>
        /// <param name="isSupress"></param>
        /// <param name="isSaveEquipmentOpenFunds"></param>
        /// <returns></returns>
        public bool Add(IList<ViewOpenFundDetail> viewOpenFundDetailList, IEnumerable<CostDeduct> entities, IEnumerable<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFunds, ref XTransaction tran, bool isSupress = false, bool isSaveEquipmentOpenFunds = true)
        {
            try
            {
                if (!isSupress && tran != null) tran = SessionManager.Instance.BeginTransaction();
                base.Add(entities, ref tran, isSupress);
                foreach (var entity in entities)
                {
                    entity.OpenFundCurrency = 0d;
                    /* 提取开放基金设备实例集合
                     * 将预扣费的开放基金归还至开放基金申请的剩余补贴
                     */
                    var quereyExpression = "";
                    var costDeductEquipmentOpenFunds = preCostDeductEquipmentOpenFunds == null
                        ? new List<CostDeductEquipmentOpenFund>()
                        : (preCostDeductEquipmentOpenFunds as IList<CostDeductEquipmentOpenFund> ?? preCostDeductEquipmentOpenFunds.ToList());
                    if (viewOpenFundDetailList == null || viewOpenFundDetailList.Count == 0)//处理预约预扣费，多个预约的情况
                    {
                        if (entity.CostDeductEquipmentOpenFunds != null && entity.CostDeductEquipmentOpenFunds.Count > 0)
                        {
                            quereyExpression = entity.CostDeductEquipmentOpenFunds.Select(p => p.OpenFundApplyId).ToFormatStr();
                        }
                        if (costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Any())
                        {
                            quereyExpression += (string.IsNullOrWhiteSpace(quereyExpression) ? "" : "*") + costDeductEquipmentOpenFunds.Select(p => p.OpenFundApplyId).ToFormatStr();
                        }
                    }
                    else
                    {
                        quereyExpression = viewOpenFundDetailList.Select(p => p.OpenFundApplyId).ToFormatStr();
                    }
                    IList<OpenFundApply> openFundApplyList = null;
                    if (!string.IsNullOrWhiteSpace(quereyExpression))
                    {
                        openFundApplyList = __openFundApplyBLL.GetEntitiesByExpression(quereyExpression);
                    }
                    if (costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Any())
                    {
                        foreach (var preCostDeductEquipmentOpenFund in costDeductEquipmentOpenFunds)
                        {
                            var openFundApply = openFundApplyList.FirstOrDefault(p => p.Id == preCostDeductEquipmentOpenFund.OpenFundApplyId);
                            if (openFundApply != null)
                            {
                                openFundApply.BalanceMoney += preCostDeductEquipmentOpenFund.CalFee;
                            }
                        }
                    }
                    if (entity.CostDeductEquipmentOpenFunds != null && entity.CostDeductEquipmentOpenFunds.Count > 0)
                    {
                        foreach (var costDeductEquipmentOpenFund in entity.CostDeductEquipmentOpenFunds)
                        {
                            costDeductEquipmentOpenFund.CostDeductId = entity.Id;
                            if (costDeductEquipmentOpenFunds == null
                                || !costDeductEquipmentOpenFunds.Any()
                                || costDeductEquipmentOpenFunds.All(p => p.Id != costDeductEquipmentOpenFund.Id))
                            {

                                var openFundApply = openFundApplyList.FirstOrDefault(p => p.Id == costDeductEquipmentOpenFund.OpenFundApplyId);
                                if (openFundApply != null)
                                {
                                    openFundApply.BalanceMoney -= costDeductEquipmentOpenFund.CalFee;
                                    entity.OpenFundCurrency += costDeductEquipmentOpenFund.CalFee;
                                }
                            }
                        }
                        __costDeductEquipmentOpenFundBLL.Add(entity.CostDeductEquipmentOpenFunds, ref tran, isSupress);
                    }
                    if (isSaveEquipmentOpenFunds && openFundApplyList != null && openFundApplyList.Count > 0)
                        __openFundApplyBLL.Save(openFundApplyList, ref tran, isSupress);
                    GenerateCostDeductLog(OperateType.New, entity, entity, entity.CreaterId, entity.OperateIP, ref tran, true);
                    GenerateCostDeductForStatistics(entity, ref tran);
                    CostDeductSyn(entity, ref tran, isSupress);
                }
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
        public bool Add(IEnumerable<CostDeduct> entities, ref XTransaction tran, bool isSupress = false, bool isSaveEquipmentOpenFunds = true)
        {
            return Add(null, entities, null, ref tran, isSupress);
        }
        public override bool Add(IEnumerable<CostDeduct> entities, ref XTransaction tran, bool isSupress = false)
        {
            try
            {
                if (!isSupress && tran != null) tran = SessionManager.Instance.BeginTransaction();
                base.Add(entities, ref tran, isSupress);
                foreach (var entity in entities)
                {
                    entity.OpenFundCurrency = 0d;
                    if (entity.CostDeductEquipmentOpenFunds != null && entity.CostDeductEquipmentOpenFunds.Count > 0)
                    {
                        foreach (var costDeductEquipmentOpenFund in entity.CostDeductEquipmentOpenFunds)
                        {
                            costDeductEquipmentOpenFund.CostDeductId = entity.Id;
                            entity.OpenFundCurrency += costDeductEquipmentOpenFund.CalFee;
                        }
                        __costDeductEquipmentOpenFundBLL.Add(entity.CostDeductEquipmentOpenFunds, ref tran, isSupress);
                    }
                    GenerateCostDeductLog(OperateType.New, entity, entity, entity.CreaterId, entity.OperateIP, ref tran, isSupress);
                    GenerateCostDeductForStatistics(entity, ref tran);
                    CostDeductSyn(entity, ref tran, isSupress);
                }
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

        public bool Save(IEnumerable<CostDeduct> entities, ref XTransaction tran, bool isSupress = false, bool isSaveEquipmentOpenFunds = true)
        {
            try
            {
                if (!isSupress && tran != null)
                    tran = SessionManager.Instance.BeginTransaction();
                foreach (var entity in entities)
                {
                    entity.OpenFundCurrency = 0d;
                    if (entity.CostDeductEquipmentOpenFunds != null && entity.CostDeductEquipmentOpenFunds.Count > 0 && isSaveEquipmentOpenFunds)
                    {
                        var openFundApplyList = entity.CostDeductEquipmentOpenFunds.Distinct(new CostDeductEquipmentOpenFundEqual()).Select(p => p.OpenFundApply).ToList();
                        //var openFundApplyList = __openFundApplyBLL.GetEntitiesByExpression(entity.CostDeductEquipmentOpenFunds.Select(p => p.OpenFundApplyId).ToFormatStr());
                        foreach (var costDeductEquipmentOpenFund in entity.CostDeductEquipmentOpenFunds)
                        {
                            var openFundApply = openFundApplyList.First(p => p.Id == costDeductEquipmentOpenFund.OpenFundApplyId);
                            costDeductEquipmentOpenFund.CostDeductId = entity.Id;
                            if (costDeductEquipmentOpenFund.State == ObjectState.Deleted)
                            {
                                openFundApply.BalanceMoney += costDeductEquipmentOpenFund.CalFee;
                                __costDeductEquipmentOpenFundBLL.Delete(new Guid[] { costDeductEquipmentOpenFund.Id }, ref tran, isSupress);
                            }
                            else
                            {
                                entity.OpenFundCurrency += costDeductEquipmentOpenFund.CalFee;
                                if (!entity.HasClossingAccount) openFundApply.BalanceMoney -= costDeductEquipmentOpenFund.CalFee;
                                var costDeductEquipmentOpenFundFind = __costDeductEquipmentOpenFundBLL.GetEntityById(costDeductEquipmentOpenFund.Id);
                                if (costDeductEquipmentOpenFundFind == null)
                                    __costDeductEquipmentOpenFundBLL.Add(new CostDeductEquipmentOpenFund[] { costDeductEquipmentOpenFund }, ref tran, isSupress);
                                else
                                    __costDeductEquipmentOpenFundBLL.Save(new CostDeductEquipmentOpenFund[] { costDeductEquipmentOpenFund }, ref tran, isSupress);
                            }
                        }
                        if (!entity.HasClossingAccount) __openFundApplyBLL.Save(openFundApplyList, ref tran, isSupress);
                    }
                    GenerateCostDeductLog(OperateType.Modified, GetEntityById(entity.Id), entity, entity.CreaterId, entity.OperateIP, ref tran, true);
                    DelCostDeductForStatistics(new Guid[] { entity.Id }, ref tran);
                    GenerateCostDeductForStatistics(entity, ref tran);
                    CostDeductSyn(entity, ref tran, isSupress);
                }
                base.Save(entities, ref tran, isSupress);
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
        public override bool Save(IEnumerable<CostDeduct> entities, ref XTransaction tran, bool isSupress = false)
        {
            try
            {
                if (!isSupress && tran != null)
                    tran = SessionManager.Instance.BeginTransaction();
                foreach (var entity in entities)
                {
                    entity.OpenFundCurrency = 0d;
                    if (entity.CostDeductEquipmentOpenFunds != null && entity.CostDeductEquipmentOpenFunds.Count > 0)
                    {
                        var openFundApplyList = entity.CostDeductEquipmentOpenFunds.Distinct(new CostDeductEquipmentOpenFundEqual()).Select(p => p.OpenFundApply).ToList();
                        //var openFundApplyList = __openFundApplyBLL.GetEntitiesByExpression(entity.CostDeductEquipmentOpenFunds.Select(p => p.OpenFundApplyId).ToFormatStr());
                        foreach (var costDeductEquipmentOpenFund in entity.CostDeductEquipmentOpenFunds)
                        {
                            var openFundApply = openFundApplyList.First(p => p.Id == costDeductEquipmentOpenFund.OpenFundApplyId);
                            costDeductEquipmentOpenFund.CostDeductId = entity.Id;
                            if (costDeductEquipmentOpenFund.State == ObjectState.Deleted)
                            {
                                openFundApply.BalanceMoney += costDeductEquipmentOpenFund.CalFee;
                                __costDeductEquipmentOpenFundBLL.Delete(new Guid[] { costDeductEquipmentOpenFund.Id }, ref tran, isSupress);
                            }
                            else
                            {
                                entity.OpenFundCurrency += costDeductEquipmentOpenFund.CalFee;
                                if (!entity.HasClossingAccount) openFundApply.BalanceMoney -= costDeductEquipmentOpenFund.CalFee;
                                var costDeductEquipmentOpenFundFind = __costDeductEquipmentOpenFundBLL.GetEntityById(costDeductEquipmentOpenFund.Id);
                                if (costDeductEquipmentOpenFundFind == null)
                                    __costDeductEquipmentOpenFundBLL.Add(new CostDeductEquipmentOpenFund[] { costDeductEquipmentOpenFund }, ref tran, isSupress);
                                else
                                    __costDeductEquipmentOpenFundBLL.Save(new CostDeductEquipmentOpenFund[] { costDeductEquipmentOpenFund }, ref tran, isSupress);
                            }
                        }
                        if (!entity.HasClossingAccount) __openFundApplyBLL.Save(openFundApplyList, ref tran, isSupress);
                    }
                    GenerateCostDeductLog(OperateType.Modified, GetEntityById(entity.Id), entity, entity.CreaterId, entity.OperateIP, ref tran, true);
                    DelCostDeductForStatistics(new Guid[] { entity.Id }, ref tran);
                    GenerateCostDeductForStatistics(entity, ref tran);
                    CostDeductSyn(entity, ref tran, isSupress);
                }
                base.Save(entities, ref tran, isSupress);
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

        public class CostDeductEquipmentOpenFundEqual : IEqualityComparer<CostDeductEquipmentOpenFund>
        {
            public bool Equals(CostDeductEquipmentOpenFund x, CostDeductEquipmentOpenFund y)
            {
                return x.OpenFundApplyId == y.OpenFundApplyId;
            }

            public int GetHashCode(CostDeductEquipmentOpenFund obj)
            {
                return obj.OpenFundApplyId.GetHashCode();
            }
        }
        /// <summary> 删除扣费记录
        /// 生成扣费系统日志、删除扣费统计记录、删除扣费记录、开放基金预扣费归还至对应开放基金申请剩余补贴、删除开放基金设备扣费记录</summary>
        /// <param name="ids"></param>
        /// <param name="tran"></param>
        /// <param name="operatorId"></param>
        /// <param name="operateIP"></param>
        /// <param name="isSupress"></param>
        /// <param name="isSaveEquipmentOpenFunds"></param>
        /// <returns></returns>
        public bool Delete(IEnumerable<Guid> ids, ref XTransaction tran, Guid? operatorId, string operateIP, bool isSupress = false, bool isSaveEquipmentOpenFunds = true)
        {
            try
            {
                if (!isSupress && tran != null)
                {
                    tran = SessionManager.Instance.BeginTransaction();
                }
                foreach (var id in ids)
                {
                    // 生成扣费系统日志
                    GenerateCostDeductLog(OperateType.Deleted, id, operatorId, operateIP, ref tran, true);
                    // 删除扣费统计记录
                    DelCostDeductForStatistics(new Guid[] { id }, ref tran);
                }
                // 删除扣费日志记录
                var costDeductLogList = __costDeductLogBLL.GetEntitiesByExpression(ids.ToFormatStr("CostDeductId"));
                if (costDeductLogList != null && costDeductLogList.Count > 0)
                    __costDeductLogBLL.Delete(costDeductLogList.Select(p => p.Id), ref tran, isSupress);
                // 开放基金
                var costDeductEquipmentOpenFundList = __costDeductEquipmentOpenFundBLL.GetEntitiesByExpression(ids.ToFormatStr("CostDeductId"));
                if (costDeductEquipmentOpenFundList != null && costDeductEquipmentOpenFundList.Count > 0)
                {
                    var openFundApplyFundList = __openFundApplyBLL.GetEntitiesByExpression(costDeductEquipmentOpenFundList.Select(p => p.OpenFundApplyId).ToFormatStr());
                    foreach (var costDeductEquipmentOpenFund in costDeductEquipmentOpenFundList)
                    {
                        var openFundApply = openFundApplyFundList.First(p => p.Id == costDeductEquipmentOpenFund.OpenFundApplyId);
                        openFundApply.BalanceMoney += costDeductEquipmentOpenFund.CalFee;
                    }
                    // 删除开放基金设备扣费
                    __costDeductEquipmentOpenFundBLL.Delete(costDeductEquipmentOpenFundList.Select(p => p.Id), ref tran, isSupress);
                    // 开放基金预扣费归还至对应开放基金申请剩余补贴
                    if (isSaveEquipmentOpenFunds) __openFundApplyBLL.Save(openFundApplyFundList, ref tran, isSupress);
                }
                // 删除扣费
                base.Delete(ids, ref tran, isSupress);
                DeleteSyn(ids, ref tran, isSupress);
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
        public bool Delete(IEnumerable<Guid> ids, ref XTransaction tran, Guid? operatorId, string operateIP, bool isSupress = false)
        {
            return Delete(ids, ref tran, operatorId, operateIP, isSupress, true);
        }
        public override bool Delete(IEnumerable<Guid> ids, ref XTransaction tran, bool isSupress = false)
        {
            DeleteSyn(ids, ref tran, isSupress);
            return base.Delete(ids, ref tran, isSupress);
        }

        /// <summary>
        /// 创建或更新同步记录
        /// </summary>
        /// <param name="costDeduct"></param>
        /// <param name="tran"> </param>
        /// <param name="isSupress"> </param>
        private void CostDeductSyn(CostDeduct costDeduct, ref XTransaction tran, bool isSupress)
        {
            var usnUseable = costDeduct.UserAccount.UnUseable ?? costDeduct.UserAccount.CreditLimit.UnUseable;
            var unAppointment = costDeduct.UserAccount.UnAppointment ?? costDeduct.UserAccount.CreditLimit.UnAppointment;
            var money = (costDeduct.RealCurrency + costDeduct.RealCurrency);

            var __dataSync = __dataSyncBLL.GetDataSyncSetting();
            if ((costDeduct.UserAccount.TotalCurrency - money < usnUseable) || (costDeduct.UserAccount.TotalCurrency - money < unAppointment))
            {
                CostDeductSyn deduct = null;
                var deducts = __costDeductSynBLL.GetEntitiesByExpression(string.Format("CostDeductId=\"{0}\"", costDeduct.Id));
                if (deducts != null && deducts.Count > 0)
                {
                    deduct = deducts[0];
                    deduct.IsSyned = false;
                    deduct.PayerId = costDeduct.UserAccount == null ? Guid.Empty : costDeduct.UserAccount.UserId;
                    deduct.AccountId = costDeduct.UserAccountId;
                    deduct.PayerName = costDeduct.UserAccount == null ? string.Empty : costDeduct.UserAccount.GetUser().UserName;
                    deduct.CostDeductType = costDeduct.CostDeductTypeEnum.ToCnName();
                    deduct.UserName = GetUserName(costDeduct);
                    if (costDeduct.UsedConfirmId.HasValue)
                    {
                        deduct.EquipmentId = costDeduct.GetUsedConfirm().EquipmentId;
                    }
                    if (costDeduct.Appointment != null)
                    {
                        deduct.EquipmentId = costDeduct.Appointment.EquipmentId;
                    }
                    __costDeductSynBLL.Save(new[] { deduct }, ref tran, isSupress);
                }
                else if (__dataSync.IsAutoCostDeductSync)
                {
                    deduct = new CostDeductSyn();
                    deduct.Id = Guid.NewGuid();
                    deduct.CreateTime = DateTime.Now;
                    deduct.CostDeductId = costDeduct.Id;
                    deduct.AccountId = costDeduct.UserAccountId;
                    deduct.PayerId = costDeduct.UserAccount == null ? Guid.Empty : costDeduct.UserAccount.UserId;
                    deduct.PayerName = costDeduct.UserAccount == null ? string.Empty : costDeduct.UserAccount.GetUser().UserName;
                    deduct.CostDeductType = costDeduct.CostDeductTypeEnum.ToCnName();
                    deduct.UserName = GetUserName(costDeduct);

                    if (costDeduct.UsedConfirmForLog != null)
                    {
                        deduct.EquipmentId = costDeduct.UsedConfirmForLog.EquipmentId;
                    }
                    else if (costDeduct.UsedConfirmId.HasValue)
                    {
                        deduct.EquipmentId = costDeduct.GetUsedConfirm().EquipmentId;
                    }
                    else if (costDeduct.Appointment != null)
                    {
                        deduct.EquipmentId = costDeduct.Appointment.EquipmentId;
                    }
                    __costDeductSynBLL.Add(new[] { deduct }, ref tran, isSupress);
                }
            }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public string GetUserName(CostDeduct costDeduct)
        {

            if (costDeduct != null)
            {
                switch (costDeduct.CostDeductTypeEnum)
                {
                    case CostDeductType.Animal:
                        if (costDeduct.AnimalCostDeduct != null)
                        {
                            return costDeduct.AnimalCostDeduct.User.UserName;
                        }
                        break;
                    case CostDeductType.AppointmentPredictCostDeduct:
                        if (costDeduct.Appointment != null)
                        {
                            return costDeduct.Appointment.User.UserName;
                        }
                        break;
                    case CostDeductType.ManualCostDeduct:
                        if (costDeduct.ManualCostDeduct != null)
                        {
                            return costDeduct.ManualCostDeduct.User.UserName;
                        }
                        break;
                    case CostDeductType.MaterialCostDeduct:
                        if (costDeduct.MaterialOutput != null)
                        {
                            return costDeduct.MaterialOutput.OutputUser.UserName;
                        }
                        break;
                    case CostDeductType.NMPUsedCostDeduct:
                        return string.Empty;
                    case CostDeductType.SampleCostDeduct:
                        if (costDeduct.SampleApply != null)
                        {
                            return costDeduct.SampleApply.Applicant.UserName;
                        }
                        break;
                    case CostDeductType.UsedCostDeduct:
                        if (costDeduct.UsedConfirmForLog != null)
                        {
                            return costDeduct.UsedConfirmForLog.User.UserName;
                        }
                        else if (costDeduct.UsedConfirmId.HasValue)
                        {
                            return costDeduct.GetUsedConfirm().User.UserName;
                        }
                        break;
                    case CostDeductType.WaterControlCostDeduct:
                        if (costDeduct.WaterControlCostDeduct != null)
                        {
                            return costDeduct.WaterControlCostDeduct.User.UserName;
                        }
                        break;
                    default:
                        return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 从校级平台删除扣费记录
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tran"> </param>
        /// <param name="isSupress"> </param>
        private void DeleteSyn(IEnumerable<Guid> ids, ref XTransaction tran, bool isSupress)
        {
            var updateDeducts = new List<CostDeductSyn>();
            var delIds = new List<Guid>();
            foreach (var id in ids)
            {
                CostDeductSyn deduct = null;
                var deducts = __costDeductSynBLL.GetEntitiesByExpression(string.Format("CostDeductId=\"{0}\"", id));
                if (deducts != null && deducts.Count > 0)
                {
                    deduct = deducts[0];
                    //没有做过同步的记录，直接删除
                    if (string.IsNullOrEmpty(deduct.DeductId))
                    {
                        __costDeductSynBLL.Delete(new[] { deduct.Id }, ref tran, isSupress);
                    }
                    //否则标记为删除
                    else
                    {
                        deduct.IsDeleted = true;
                        deduct.IsSyned = false;
                        updateDeducts.Add(deduct);
                    }
                }
            }
            if (updateDeducts.Count > 0)
            {
                __costDeductSynBLL.Save(updateDeducts, ref tran, isSupress);
            }
        }
    }
}