using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL.Sample
{
    public interface ISampleSendTimeValidateManager
    {
        bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime);
        EquipmentAppointmentTimes GetEquipmentSampleSendTimes();
    }
}
