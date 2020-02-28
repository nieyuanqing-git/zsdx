using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPTagEquipmentFunAppointmentRule : NMPAppointmentRuleBase
    {
        private INMPTagEquipmentFunBLL __tagEquipmentFunBLL;
        private IList<NMPTagEquipmentFun> __tagEquipmentFunList;
        private Guid __nmpId;
        private Guid __nmpEquipmentId;
        private Guid? __userId;
        public NMPTagEquipmentFunAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, Guid nmpId, Guid nmpEquipmentId, Guid? userId, NMP nmp, NMPEquipment nmpEquipment, User user)
            : base(equipmentAppoitmentTimes, next, nmp, nmpEquipment,user)
        {
            __tagEquipmentFunBLL = BLLFactory.CreateNMPTagEquipmentFunBLL();
            this.__userId = userId;
            __nmpId = nmpId;
            __nmpEquipmentId = nmpEquipmentId;
        }
        protected override void Prepare()
        {
            if(__userId.HasValue)
                __tagEquipmentFunList = __tagEquipmentFunBLL.GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", __nmpId));
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid || __tagEquipmentFunList == null || __tagEquipmentFunList.Count == 0 || !__userId.HasValue || !_user.TagId.HasValue) return;
            foreach (var tagEquipmentFun in __tagEquipmentFunList)
            {
                if (_user.TagId.Value != tagEquipmentFun.TagId) continue;
                var timeScopers = string.IsNullOrWhiteSpace(tagEquipmentFun.WorkHours) ? null :
                    TimeUtility.GetTimeScopersByTimeFormatStr(tagEquipmentFun.WorkHours, _nmp.AppointmentTimeStep, appointmentTime.BeginTime.Date);
                if (timeScopers == null || !timeScopers.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
                {
                    appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                    appointmentTime.Remark = string.Format("您的身份【{0}】不能预约该时间段", _user.Tag.Name);
                }
            }
        }
    }
}
