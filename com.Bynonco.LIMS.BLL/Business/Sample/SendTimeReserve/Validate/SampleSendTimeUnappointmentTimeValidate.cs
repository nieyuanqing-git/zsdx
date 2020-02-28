using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.Sample
{
    public class SampleSendTimeUnappointmentTimeValidate: SampleSendTimeValidateBase
    {
        private IList<UnAppointmentPeriodTime> __unAppointmentPeriodTimes;
        private IUnAppointmentPeriodTimeBLL __unAppointmentPeriodTimeBLL;
        public SampleSendTimeUnappointmentTimeValidate(Equipment equipment)
            : base(equipment) 
        {
            __unAppointmentPeriodTimeBLL = BLLFactory.CreateUnAppointmentPeriodTimeBLL();
            __unAppointmentPeriodTimes = __unAppointmentPeriodTimeBLL.GetValidateUnAppointmentPeriodTimeListByEquipmentId(_equipment.Id, true);
        }
        protected override bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime)
        {
            return __unAppointmentPeriodTimeBLL.Validates(out errorMessage, equipmentAppointmentTime, __unAppointmentPeriodTimes);
        }
    }
}
