using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public class SpeciaAppointmentRule:AppointmentRuleBase
    {
        private IAppointmentSpeciaRuleBLL __appointmentSpeciaRuleBLL;
        private IAppointmentSpeciaRuleUserBLL __appointmentSpeciaRuleUserBLL;
        private IList<AppointmentSpeciaRule> __appointmentSpeciaRuleList;
        private Appointment __appointment;
        private Guid __equipmentId;
        private Guid? __userId;
        private Guid[] __selectEquipmentPartIds;
        public SpeciaAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, AppointmentRuleBase next, Guid equipmentId, Guid? userId, Equipment equipment, User user, Appointment appointment,Guid[] selectEquipmentPartIds)
            : base(equipmentAppoitmentTimes, next, equipment, user)
        {
            this.__userId = userId;
            
            __appointmentSpeciaRuleUserBLL = BLLFactory.CreateAppointmentSpeciaRuleUserBLL();
          var appointmentSpeciaRuleUsers =  __appointmentSpeciaRuleUserBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.ToString()));
            if (appointmentSpeciaRuleUsers != null && appointmentSpeciaRuleUsers.Count > 0)
            {
                IList<Guid> appointmentSpeciaRuleids = new List<Guid>();
                foreach (var appointmentSpeciaRuleUser in appointmentSpeciaRuleUsers) {
                    appointmentSpeciaRuleids.Add(appointmentSpeciaRuleUser.AppointmentSpecialRuleId);
                }
                __appointmentSpeciaRuleBLL = new AppointmentSpeciaRuleBLL(appointmentSpeciaRuleids);
            }
            else {

                 __appointmentSpeciaRuleBLL = new AppointmentSpeciaRuleBLL(true);
               
            }
            __equipmentId = equipmentId;
            __appointment = appointment;
            __selectEquipmentPartIds = selectEquipmentPartIds;
        }
        protected override void Prepare()
        {
            __appointmentSpeciaRuleList = __appointmentSpeciaRuleBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", __equipmentId.ToString()));
        }
       
        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            string errorMessage = "", tipMessage = "";
            bool isSingleSelect = false;
            if (appointmentTime.Status != EquipmentAppointmentTimeStatus.Valid) return;
            if (__appointmentSpeciaRuleList == null || __appointmentSpeciaRuleList.Count == 0 || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid) return;
            var result = __appointmentSpeciaRuleBLL.Validates(__appointmentSpeciaRuleList, appointmentTime.BeginTime, appointmentTime.EndTime, __equipmentId, __userId, _equipment.RoomId.Value, __appointment, __selectEquipmentPartIds, out errorMessage, out tipMessage, out isSingleSelect);
            if (!result)
            {
                appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                appointmentTime.Remark = errorMessage;
            }
            appointmentTime.TipRemark = tipMessage;
            appointmentTime.IsSingleSelect = isSingleSelect;
        }
    }
}
