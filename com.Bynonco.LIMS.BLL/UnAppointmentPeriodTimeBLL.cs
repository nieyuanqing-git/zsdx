using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class UnAppointmentPeriodTimeBLL : BLLBase<UnAppointmentPeriodTime>, IUnAppointmentPeriodTimeBLL
    {
        public IList<UnAppointmentPeriodTime> GetValidateUnAppointmentPeriodTimeListByEquipmentId(Guid equipmentId, bool isEffectOnSample)
        {
            string queryExpression = string.Format("EndTime>\"{0}\"*EquipmentId=\"{1}\"*CancelTime=null", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), equipmentId.ToString());
            if (isEffectOnSample) queryExpression += "*IsEffectOnSample=true";
            return GetEntitiesByExpression(queryExpression);
        }
        public bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime, IList<UnAppointmentPeriodTime> unAppointmentPeriodTimes)
        {
            errorMessage = "";
            if (unAppointmentPeriodTimes != null && unAppointmentPeriodTimes.Count > 0)
            {
                foreach (var unAppointmentPeriodTime in unAppointmentPeriodTimes)
                {
                    if (DateTimeUtility.IsContain(equipmentAppointmentTime.BeginTime, equipmentAppointmentTime.EndTime, unAppointmentPeriodTime.BeginTime, unAppointmentPeriodTime.EndTime))
                    {
                        equipmentAppointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                        equipmentAppointmentTime.Remark = unAppointmentPeriodTime.Description;
                        equipmentAppointmentTime.IsRepareTime = true;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}