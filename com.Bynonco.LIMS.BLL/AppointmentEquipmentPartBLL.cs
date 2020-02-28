using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentEquipmentPartBLL : BLLBase<AppointmentEquipmentPart>, IAppointmentEquipmentPartBLL
    {
        public IList<AppointmentEquipmentPart> GetAppointmentEquipmentPartList(Guid? appointmentId)
        {
            if (!appointmentId.HasValue) return null;
            return GetEntitiesByExpression(string.Format("AppointmentId=\"{0}\"", appointmentId.Value.ToString()));
        }
    }
}
