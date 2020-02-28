using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPAppointmentPriorityBLL : BLLBase<NMPAppointmentPriority>, INMPAppointmentPriorityBLL
    {
        public IList<NMPAppointmentPriority> GetValidateNMPAppointmentPriorityListByNMPId(Guid nmpId)
        {
            return GetEntitiesByExpression(string.Format("(EndAt>\"{0}\"+EndAt=null)*NMPId=\"{1}\"", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), nmpId));
        }
    }
}
