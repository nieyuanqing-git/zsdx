using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Sample
{
    public class SampleSendTimeTimeReserveValidate : SampleSendTimeValidateBase
    {
        private IViewSampleApplyBLL __viewSampleApplyBLL;
        private DateTime __beginDate = DateTime.MinValue;
        private DateTime __endDate = DateTime.MaxValue;
        private Guid __applicantId;
        private bool __hook = false;
        private IList<ViewSampleApply> __lstviewSampleApplies;
        public SampleSendTimeTimeReserveValidate(Equipment equipment, DateTime beginDate, DateTime endDate,Guid applicantId)
            : base(equipment)
        {
            __viewSampleApplyBLL = BLLFactory.CreateViewSampleApplyBLL();
            __beginDate = beginDate;
            __endDate = endDate;
            __applicantId = applicantId;
        }

        protected override bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime)
        {
            errorMessage = "";
            if (!__hook)
            {
                __lstviewSampleApplies = __viewSampleApplyBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*ExpectSendDate>\"{1}\"*ExpectSendDate<\"{2}\"*Status=-{3}*Status=-{4}",
                                                                                   _equipment.Id, __beginDate.ToString("yyyy-MM-dd HH:mm:ss"), __endDate.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), (int)SampleApplyStatus.Canceled, (int)SampleApplyStatus.RefuseAccept),
                                                                                    null, "", true, false, true, new string[] { "EnableOperatorId" });
                __hook = true;
            }
            if (__lstviewSampleApplies != null && __lstviewSampleApplies.Count > 0)
            {
                foreach (var viewSampleApply in __lstviewSampleApplies)
                {
                    if (DateTimeUtility.IsContain(equipmentAppointmentTime.BeginTime,
                                                  equipmentAppointmentTime.BeginTime.AddMinutes(_equipment.SampleSendTimeInerval),
                                                  viewSampleApply.ExpectSendDate.Value,
                                                  viewSampleApply.ExpectSendDate.Value.AddMinutes(_equipment.SampleSendTimeInerval)
                                                 ))
                    {
                        equipmentAppointmentTime.Status = __applicantId != viewSampleApply.ApplicantId ? EquipmentAppointmentTimeStatus.OtherPersonAppointmented : EquipmentAppointmentTimeStatus.SelfAppointmented;
                        StringBuilder sbRemark = new StringBuilder();
                        sbRemark.AppendFormat("该时间段被【{0}】预约了", viewSampleApply.ApplicantName).AppendLine();
                        sbRemark.AppendFormat("所属机构:{0}", viewSampleApply.OrganizationName).AppendLine();
                        sbRemark.AppendFormat("联系电话:{0}", viewSampleApply.TelephoneNo).AppendLine();
                        sbRemark.AppendFormat("电子邮箱:{0}", viewSampleApply.Email);
                        equipmentAppointmentTime.Remark = sbRemark.ToString();
                        return equipmentAppointmentTime.Status == EquipmentAppointmentTimeStatus.SelfAppointmented;
                    }
                }
                
            }
            return true;
        }
    }
}
