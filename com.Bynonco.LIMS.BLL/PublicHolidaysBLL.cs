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
    public class PublicHolidaysBLL : BLLBase<PublicHolidays>, IPublicHolidaysBLL
    {
        IList<HolidaysExclude> excludeLists = null;
        IList<HolidaysInclude> includeLists = null;

        public PublicHolidaysBLL(IList<HolidaysExclude> excludeLists, IList<HolidaysInclude> includeLists)
        {
            this.excludeLists = excludeLists;
            this.includeLists = includeLists;
        }
        public PublicHolidaysBLL()
        {
          
        }
        public IList<IPublicHolidays> GetValidatePublicHolidaysListForAppointment(bool isEffectOnSample)
        {
            var publicHolidaysList = GetValidatePublicHolidaysList(isEffectOnSample);
            if (publicHolidaysList == null || publicHolidaysList.Count == 0) return null;
            IList<IPublicHolidays> lstPublicHolidays = new List<IPublicHolidays>();
            foreach (var publicHolidays in publicHolidaysList) lstPublicHolidays.Add(publicHolidays);
            return lstPublicHolidays;
        }
        public void ValidateForAppointment(Guid equipmentId, bool isEffectOnSample, EquipmentAppointmentTime appointmentTime, IList<IPublicHolidays> publicHolidaysList)
        {
            IList<PublicHolidays> lstPublicHolidays = new List<PublicHolidays>();
            if (publicHolidaysList != null && publicHolidaysList.Count > 0)
            {
                foreach (var publicHolidays in publicHolidaysList) lstPublicHolidays.Add((PublicHolidays)publicHolidays);
            }
            Validate(equipmentId, isEffectOnSample, appointmentTime, lstPublicHolidays);
        }

        public IList<PublicHolidays> GetValidatePublicHolidaysList(bool isEffectOnSample)
        {
            var queryExpression = string.Format("EndAt>\"{0}\"", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (isEffectOnSample) queryExpression += "*IsEffectOnSample=true";
            return GetEntitiesByExpression(queryExpression);
        }
        
        public void Validate(Guid equipmentId, bool isEffectOnSample, EquipmentAppointmentTime appointmentTime, IList<PublicHolidays> publicHolidaysList)
        { 
            IHolidaysExcludeBLL holidaysExcludeBLL = BLLFactory.CreateHolidaysExcludeBLL();
            IHolidaysIncludeBLL holidaysIncludeBLL = BLLFactory.CreateHolidaysIncludeBLL();
            if (publicHolidaysList == null || publicHolidaysList.Count == 0) return;
            if (excludeLists == null) {
             excludeLists  = holidaysExcludeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId));
            }

            if (includeLists == null)
            {
                 includeLists = holidaysIncludeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId));
            }
        
            if (includeLists != null && includeLists.Count() > 0) { 
                foreach (var publicHoliday in publicHolidaysList)
                    {
                      
                    if (excludeLists != null && excludeLists.Count > 0) {
                      var exclude =   excludeLists.Where(p => p.HolidaysId == publicHoliday.Id);
                        if (exclude != null && exclude.Count() != 0) {
                            continue;
                        }
                    }
                      
                    var includeList = includeLists.Where(p => p.HolidaysId == publicHoliday.Id);
                   
                    if (includeList == null || includeList.Count() == 0)

                    {
                        continue;
                    }
                    //if ((includeList == null || includeList.Count == 0) && DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, publicHoliday.StartAt, publicHoliday.EndAt))
                    //{
                    //    appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                    //    appointmentTime.Remark = publicHoliday.Description;
                    //}

                    if (appointmentTime.Status == EquipmentAppointmentTimeStatus.Valid)
                        {
                            
                                if (DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, publicHoliday.StartAt, publicHoliday.EndAt))
                                {
                                    appointmentTime.Status = EquipmentAppointmentTimeStatus.Invalid;
                                    appointmentTime.Remark = publicHoliday.Description;
                                }
                            
                        }
                        if (appointmentTime.Status == EquipmentAppointmentTimeStatus.Invalid && publicHoliday.ExchangeWorkDays != null && publicHoliday.ExchangeWorkDays.Count > 0)
                        {
                            foreach (var exchangeWorkDay in publicHoliday.ExchangeWorkDays)
                            {
                                if (DateTimeUtility.IsContain(appointmentTime.BeginTime, appointmentTime.EndTime, exchangeWorkDay.BeginTime, exchangeWorkDay.EndTime))
                                {
                                    appointmentTime.Status = EquipmentAppointmentTimeStatus.Valid;
                                    appointmentTime.GenerateDefaultRemark();
                                }
                            }
                        }
                    }
            }
        }
    }
}