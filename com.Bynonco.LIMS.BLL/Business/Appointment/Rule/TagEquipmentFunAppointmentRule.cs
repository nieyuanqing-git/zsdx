using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class TagEquipmentFunAppointmentRule : AppointmentRuleBase
    {
        private ITagEquipmentFunBLL __tagEquipmentFunBLL;
        private IList<TagEquipmentFun> __tagEquipmentFunList;
        private Guid __equipmentId;
        private Guid? __userId;
        public TagEquipmentFunAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Guid? userId, Equipment equipment, User user)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
             __tagEquipmentFunBLL = BLLFactory.CreateTagEquipmentFunBLL();
            this.__userId = userId;
            __equipmentId = equipmentId;
        }
        protected override void Prepare()
        {
            if(__userId.HasValue)
                 __tagEquipmentFunList = __tagEquipmentFunBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", __equipmentId.ToString()));
        }

        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            if (appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid || __tagEquipmentFunList == null || __tagEquipmentFunList.Count == 0 || !__userId.HasValue || !_user.TagId.HasValue) return;
            foreach (var tagEquipmentFun in __tagEquipmentFunList)
            {
                if (_user.TagId.Value != tagEquipmentFun.TagId) continue;
                var timeScopers = string.IsNullOrWhiteSpace(tagEquipmentFun.WorkHours) ? null :
                    TimeUtility.GetTimeScopersByTimeFormatStr(tagEquipmentFun.WorkHours, _equipment.AppointmentTimeStep.Value, appointmentTime.BeginTime.Date);
                if (timeScopers == null || !timeScopers.Any(p => DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, p.BeginTime, p.EndTime)))
                {
                    appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                    appointmentTime.Remark = string.Format("您的身份【{0}】不能预约该时间段", _user.Tag.Name);
                }
            }
        }
    }
}
