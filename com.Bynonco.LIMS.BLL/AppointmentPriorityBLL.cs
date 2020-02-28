using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentPriorityBLL : BLLBase<AppointmentPriority>, IAppointmentPriorityBLL
    {
        public IList<AppointmentPriority> GetValidateAppointmentPriorityListByEquipmentId(Guid equipmentId)
        {
            return GetEntitiesByExpression(string.Format("(EndAt>\"{0}\"+EndAt=null)*EquipmentId=\"{1}\"", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), equipmentId.ToString()));
        }
    }
}
