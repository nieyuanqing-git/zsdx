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
    public class SampleSendTimeHolidayValidate: SampleSendTimeValidateBase
    {
        private IPublicHolidaysBLL __publicHolidayBLL;
        private IList<PublicHolidays> __publicHolidaysList;
        public SampleSendTimeHolidayValidate(Equipment equipment)
            : base(equipment) 
        {
            __publicHolidayBLL = BLLFactory.CreatePublicHolidaysBLL();
            __publicHolidaysList = __publicHolidayBLL.GetValidatePublicHolidaysList(true);
        }

        protected override bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime)
        {
            __publicHolidayBLL.Validate(_equipment.Id, true, equipmentAppointmentTime, __publicHolidaysList);
            errorMessage = equipmentAppointmentTime.Remark;
            return equipmentAppointmentTime.Status == EquipmentAppointmentTimeStatus.Valid;
        }
    }
}
