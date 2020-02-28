using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPBookedAppointmentRule : NMPAppointmentRuleBase
    {
        private INMPAppointmentBLL __nmpAppointmentBLL;
        private IList<NMPAppointment> __nmpApointmentList;
        private Guid __nmpId;
        private Guid __nmpEquipmentId;
        private Guid? __userId;
        public NMPBookedAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, Guid nmpId, Guid nmpEquipmentId, Guid? userId, NMP nmp, NMPEquipment nmpEquipment, User user)
            : base(equipmentAppoitmentTimes, next, nmp, nmpEquipment, user)
        {
            this.__userId = userId;
            __nmpAppointmentBLL = BLLFactory.CreateNMPAppointmentBLL();
            __nmpId = nmpId;
            __nmpEquipmentId = nmpEquipmentId;
        }
        protected override void Prepare()
        {
            __nmpApointmentList = __nmpAppointmentBLL.GetPeriodValidateNMPAppointmentList(__nmpEquipmentId, _equipmentAppoitmentTimes.BeginDate.Value, _equipmentAppoitmentTimes.EndDate.Value);
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (__nmpApointmentList == null || __nmpApointmentList.Count == 0) return;
            foreach (var nmpAppointment in __nmpApointmentList)
            {
                if (DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, nmpAppointment.BeginTime.Value, nmpAppointment.EndTime.Value))
                {
                    appointmentTime.Status = !__userId.HasValue || __userId.Value != nmpAppointment.UserId.Value ? EquipmentAppointmentTimeStatus.OtherPersonAppointmented : EquipmentAppointmentTimeStatus.SelfAppointmented;
                    StringBuilder sbRemark = new StringBuilder();
                    sbRemark.AppendFormat("该时间段被【{0}】预约了", nmpAppointment.User.UserName).AppendLine();
                    sbRemark.AppendFormat("联系电话:{0}", string.IsNullOrWhiteSpace(nmpAppointment.User.PhoneNumber) ? nmpAppointment.User.FixedPhone : nmpAppointment.User.PhoneNumber).AppendLine();
                    sbRemark.AppendFormat("电子邮箱:{0}", nmpAppointment.User.Email).AppendLine();
                    if (nmpAppointment.User.Tutor != null)
                    {
                        sbRemark.AppendFormat("导师:{0}", nmpAppointment.User.Tutor.UserName).AppendLine();
                    }
                    appointmentTime.CurNMPAppointment = nmpAppointment;
                    appointmentTime.Remark = sbRemark.ToString();
                }
            }
        }
    }
}
