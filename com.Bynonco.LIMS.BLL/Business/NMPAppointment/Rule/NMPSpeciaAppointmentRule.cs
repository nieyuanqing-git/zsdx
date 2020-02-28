using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPSpeciaAppointmentRule:NMPAppointmentRuleBase
    {
        private INMPAppointmentSpeciaRuleBLL __nmpAppointmentSpeciaRuleBLL;
        private INMPAppointmentSpeciaRuleUserBLL __nmpAppointmentSpeciaRuleUserBLL;
        private IList<NMPAppointmentSpeciaRule> __appointmentSpeciaRuleList;
        private NMPAppointment __nmpAppointment;
        private Guid __nmpId;
        private Guid __nmpEquipmentId;
        private Guid? __userId;
        private Guid[] __selectNMPPartIds;
        public NMPSpeciaAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, Guid nmpId, Guid nmpEquipmentId, Guid? userId, NMP nmp, User user, NMPAppointment nmpAppointment, NMPEquipment nmpEquipment, Guid[] selectNMPPartIds)
            : base(equipmentAppoitmentTimes, next, nmp, nmpEquipment,user)
        {
            this.__userId = userId;
            __nmpAppointmentSpeciaRuleBLL = BLLFactory.CreateNMPAppointmentSpeciaRuleBLL();
            __nmpAppointmentSpeciaRuleUserBLL = BLLFactory.CreateNMPAppointmentSpeciaRuleUserBLL();
            __nmpId = nmpId;
            __nmpEquipmentId = nmpEquipmentId;
            __nmpAppointment = nmpAppointment;
            __selectNMPPartIds = selectNMPPartIds;
        }
        protected override void Prepare()
        {
            __appointmentSpeciaRuleList = __nmpAppointmentSpeciaRuleBLL.GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", __nmpId));
        }
       
        public override void Superimposing(Model.Business.EquipmentAppointmentTime appointmentTime)
        {
            string errorMessage = "", tipMessage = "";
            bool isSingleSelect = false;
            if (appointmentTime.Status != EquipmentAppointmentTimeStatus.Valid) return;
            if (__appointmentSpeciaRuleList == null || __appointmentSpeciaRuleList.Count == 0 || appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid) return;
            var result = __nmpAppointmentSpeciaRuleBLL.Validates(__appointmentSpeciaRuleList, appointmentTime.BeginTime, appointmentTime.EndTime, __nmpId, __userId, _nmp.RoomId.Value, __nmpAppointment, __selectNMPPartIds, out errorMessage, out tipMessage, out isSingleSelect);
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
