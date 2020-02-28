using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL.Sample
{
    public class SampleSendTimeValidateManager : ISampleSendTimeValidateManager
    {
        private IEquipmentBLL __equipmentBLL;
        private Equipment __equipment;
        private Guid __applicantId;
        private DateTime __beginDate = DateTime.MinValue;
        private DateTime __endDate = DateTime.MaxValue;
        private IList<SampleSendTimeValidateBase> __lstSampleSendTimeValidates;
        public SampleSendTimeValidateManager(Guid equipmentId,Guid applicantId,DateTime beginDate,DateTime endDate)
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __equipment = __equipmentBLL.GetEntityById(equipmentId);
            this.__applicantId = applicantId;
            this.__beginDate = beginDate;
            this.__endDate = endDate;
            __lstSampleSendTimeValidates = new List<SampleSendTimeValidateBase>()
            {
                new SampleSendTimeHolidayValidate(__equipment),
                new SampleSendTimeTimeReserveValidate(__equipment,__beginDate,__endDate,__applicantId),
                new SampleSendTimeTimeOutValidate(__equipment),
                new SampleSendTimeWorkDaysValidate(__equipment),
                new SampleSendTimeOutOfEnableReserveDaysValidate(__equipment),
                new SampleSendTimeExistValidate(__equipment),
                new SampleSendTimeUnappointmentTimeValidate(__equipment)
            };
        }
        public bool Validates(out string errorMessage, EquipmentAppointmentTime equipmentAppointmentTime)
        {
            errorMessage = "";
            foreach (var sampleSendTimeValidate in __lstSampleSendTimeValidates)
            {
                if(!sampleSendTimeValidate.DoValidates(out errorMessage,equipmentAppointmentTime))
                {
                    errorMessage = equipmentAppointmentTime.Remark;
                    return false;
                }
            }
            return true;
        }

        public Model.Business.EquipmentAppointmentTimes GetEquipmentSampleSendTimes()
        {
            string errorMessage="";
            var equipmentSampleSendTimes = __equipmentBLL.GetSampleSendTimes(__applicantId,__equipment.Id, __beginDate, __endDate);
            if (equipmentSampleSendTimes == null) return null;
            foreach (var equipmentSampleSendTime in equipmentSampleSendTimes.AppoitmentTimes)
            {
                foreach (var sampleSendTimeValidate in __lstSampleSendTimeValidates)
                    sampleSendTimeValidate.DoValidates(out errorMessage, equipmentSampleSendTime);
            }
            return equipmentSampleSendTimes; 
        }
    }
}
