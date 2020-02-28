using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule
{
    public class NMPUnAppointmentPeriodTimeAppointmentRule:NMPAppointmentRuleBase
    {
        private INMPUnAppointmentPeriodTimeBLL __unAppointmentPeriodTimeBLL;
        private Guid __nmpId;
        private Guid __nmpEquipmentId;
        private IList<NMPUnAppointmentPeriodTime> __unAppointmentPeriodTimeList;
        public NMPUnAppointmentPeriodTimeAppointmentRule(EquipmentAppointmentTimes equipmentAppoitmentTimes, NMPAppointmentRuleBase next, Guid nmpId,Guid nmpEquipmentId, NMP nmp,NMPEquipment nmpEquipment, User user)
            : base(equipmentAppoitmentTimes, next, nmp,nmpEquipment, user)
        {
            __unAppointmentPeriodTimeBLL = BLLFactory.CreateNMPUnAppointmentPeriodTimeBLL();
            __nmpId = nmpId;
            __nmpEquipmentId = nmpEquipmentId;
            __unAppointmentPeriodTimeList = __unAppointmentPeriodTimeBLL.GetValidateNMPUnAppointmentPeriodTimeListByNMPId(nmpId);
        }
        protected override void Prepare() { }

        public override void Superimposing(EquipmentAppointmentTime appointmentTime)
        {
            string errorMessage = "";
            if (appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid || __unAppointmentPeriodTimeList == null || __unAppointmentPeriodTimeList.Count == 0) return;
            __unAppointmentPeriodTimeBLL.Validates(out errorMessage, appointmentTime, __unAppointmentPeriodTimeList);
        }
    }
}
