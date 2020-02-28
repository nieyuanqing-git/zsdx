using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentTimeLimitBLL : BLLBase<AppointmentTimeLimit>, IAppointmentTimeLimitBLL
    {
        private IAppointmentTimeLimitUserBLL __appointmentTimeLimitUserBLL;
        public AppointmentTimeLimitBLL()
        {
            __appointmentTimeLimitUserBLL = BLLFactory.CreateAppointmentTimeLimitUserBLL();
        }

        public IList<AppointmentTimeLimit> GetUserAppointmentTimeLimitList(Guid equipmentId, Guid userId)
        {
            IList<AppointmentTimeLimit> lstAppointmentTimeLimit = new List<AppointmentTimeLimit>();
            var appointmentTimeLimitList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId.ToString()));
            if (appointmentTimeLimitList == null || appointmentTimeLimitList.Count == 0) return null;
            var appointmentTimeLimitUserList = __appointmentTimeLimitUserBLL.GetEntitiesByExpression(appointmentTimeLimitList.Select(p => p.Id).ToFormatStr("AppointmentTimeLimitId"));
            foreach (var appointmentTimeLimit in appointmentTimeLimitList)
            {
                if (appointmentTimeLimitUserList == null || appointmentTimeLimitUserList.Count == 0 || !appointmentTimeLimitUserList.Any(p => p.AppointmentTimeLimitId == appointmentTimeLimit.Id))
                {                    
                    lstAppointmentTimeLimit.Add(appointmentTimeLimit);
                    continue;
                }
                if (appointmentTimeLimitUserList.Any(p => p.UserId == userId && p.AppointmentTimeLimitId == appointmentTimeLimit.Id))
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