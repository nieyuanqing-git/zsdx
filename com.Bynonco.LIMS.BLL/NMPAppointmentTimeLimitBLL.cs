using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public  class NMPAppointmentTimeLimitBLL: BLLBase<NMPAppointmentTimeLimit>, INMPAppointmentTimeLimitBLL
    {
        private INMPAppointmentTimeLimitUserBLL __nmpAppointmentTimeLimitUserBLL;
        public NMPAppointmentTimeLimitBLL()
        {
            __nmpAppointmentTimeLimitUserBLL = BLLFactory.CreateNMPAppointmentTimeLimitUserBLL();
        }

        public IList<NMPAppointmentTimeLimit> GetUserNMPAppointmentTimeLimitList(Guid nmpId, Guid userId)
        {
            IList<NMPAppointmentTimeLimit> lstAppointmentTimeLimit = new List<NMPAppointmentTimeLimit>();
            var appointmentTimeLimitList = GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", nmpId.ToString()));
            if (appointmentTimeLimitList == null || appointmentTimeLimitList.Count == 0) return null;
            var appointmentTimeLimitUserList = __nmpAppointmentTimeLimitUserBLL.GetEntitiesByExpression(appointmentTimeLimitList.Select(p => p.Id).ToFormatStr("NMPAppointmentTimeLimitId"));
            foreach (var appointmentTimeLimit in appointmentTimeLimitList)
            {
                if (appointmentTimeLimitUserList == null || appointmentTimeLimitUserList.Count == 0 || !appointmentTimeLimitUserList.Any(p => p.NMPAppointmentTimeLimitId == appointmentTimeLimit.Id))
                {                    
                    lstAppointmentTimeLimit.Add(appointmentTimeLimit);
                    continue;
                }
                if (appointmentTimeLimitUserList.Any(p => p.UserId == userId && p.NMPAppointmentTimeLimitId == appointmentTimeLimit.Id))
                {
                    appointmentTimeLimit.IsHasUsers = true;
                    lstAppointmentTimeLimit.Add(appointmentTimeLimit);
                }
            }
            if (lstAppointmentTimeLimit != null && lstAppointmentTimeLimit.Count > 0)
            {
                lstAppointmentTimeLimit = lstAppointmentTimeLimit.OrderByDescending(p => p.IsHasUsers).ToList();
            }
            return lstAppointmentTimeLimit;
        }
    }
}
