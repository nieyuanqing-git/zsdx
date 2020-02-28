using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.BLL.Business.AppointmentRule;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class AppointmentValidateManager : AppointmentValidateManagerBase
    {
        protected IUserBLL _userBLL =BLLFactory.CreateUserBLL();
        protected User _user;
        public AppointmentValidateManager(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator)
            : base(equipmentId, userId, appointmentMidiator) 
        {
            this._appointmentMidiator = appointmentMidiator;
        }

      
        public override bool Validate(out string errorMessage)
        {
            errorMessage = "";
            if (_appointmentMidiator == null || _appointmentMidiator.NewAppointments == null || _appointmentMidiator.NewAppointments.Count() == 0)
            {
                errorMessage = "预约记录为空";
                return false;
            }
            var appointmentValidates = CreateAppointmentValidates();
            if (appointmentValidates == null) return true;
            foreach (var appointmentValidate in appointmentValidates)
            {
                if (!appointmentValidate.Validates(out errorMessage)) return false;
            }
            var appointment = _appointmentMidiator.NewAppointments.First();
            var equipment = _equipmentBLL.GetEntityById(appointment.EquipmentId.Value);
            var appointmentTimes = _appointmentMidiator.NewAppointments.Select(p => new EquipmentAppointmentTime() { BeginTime = p.BeginTime.Value, EndTime = p.EndTime.Value, Remark = "", Status = EquipmentAppointmentTimeStatus.Valid }).ToList();
            EquipmentAppointmentTimes equipmentAppointmentTimes = new EquipmentAppointmentTimes(equipment.OpenAdvanceTime, equipment.MaxAppointmentAdvanceTime.Value, equipment.MinAppointmentCancelTime.Value, equipment.AppointmentTimeStep.Value, 0, 0, appointmentTimes);
            IAppointmentTimesManager appointmentRuleManager = new AppointmentTimesManager(Model.Enum.AppointmentTimeBehaviorOfUser.Book, appointment.EquipmentId.Value, appointment.UserId.Value, equipmentAppointmentTimes, appointment, appointment.AppointmentEquipmentParts != null && appointment.AppointmentEquipmentParts.Count > 0 ? appointment.AppointmentEquipmentParts.Select(p => p.EquipmentPartId).ToArray() : null, _equipmentTimeAppointmemtMode);
            if (!appointmentRuleManager.Validate(out errorMessage)) return false;
            return true;
        }
       
        public override bool ValidateAppointmentTime(out string errorMessage)
        {
            var appointment = _appointmentMidiator.NewAppointments.First();
            var equipment = _equipmentBLL.GetEntityById(appointment.EquipmentId.Value);
            var appointmentTimes = _appointmentMidiator.NewAppointments.Select(p => new EquipmentAppointmentTime() { BeginTime = p.BeginTime.Value, EndTime = p.EndTime.Value, Remark = "", Status = EquipmentAppointmentTimeStatus.Valid }).ToList();
            EquipmentAppointmentTimes equipmentAppointmentTimes = new EquipmentAppointmentTimes(equipment.OpenAdvanceTime, equipment.MaxAppointmentAdvanceTime.Value, equipment.MinAppointmentCancelTime.Value, equipment.AppointmentTimeStep.Value, 0, 0, appointmentTimes);
            if (appointment.UserId.HasValue) _user = _userBLL.GetEntityById(appointment.UserId.Value);
            errorMessage = "";
            if (equipmentAppointmentTimes == null || equipmentAppointmentTimes.AppoitmentTimes == null || equipmentAppointmentTimes.AppoitmentTimes.Count == 0)
            {
                errorMessage = "预约信息为空";
                return false;
            }
            AppointmentRuleBase bookedAppointmentRule = new BookedAppointmentRule(equipmentAppointmentTimes, null, appointment.EquipmentId.Value, appointment.UserId.Value, equipment, _user);
            if (bookedAppointmentRule == null)
            {
                errorMessage = "预约规则为空";
                return false;
            }
            foreach (var appointmentTime in equipmentAppointmentTimes.AppoitmentTimes)
            {
                var appointmentRule = bookedAppointmentRule;
                do
                {
                    appointmentRule.DoPrepare();
                    appointmentRule.Superimposing(appointmentTime);
                    if (appointmentTime.Status != EquipmentAppointmentTimeStatus.Valid)
                    {
                        errorMessage = appointmentTime.Remark;
                        return false;
                    }
                    appointmentRule = appointmentRule.Next;
                }
                while (appointmentRule != null);
            }
            return true;
        }
        protected override IList<AppointmentValidateBase> CreateAppointmentValidates()
        {
            return new List<AppointmentValidateBase>()
            {
               new EquipmentCalcFeeRuleValidate(_equipmentId,_userId),
               new EquipmentChargeStandardValidate(_equipmentId,_userId),
               new UserStatusValidate(_equipmentId,_userId),
               new EquipmentUserLimitValidate(_equipmentId,_userId,_appointmentMidiator),
               new EquipmentSubjectLimitValidate(_equipmentId,_userId,_appointmentMidiator),
               new UserExaminationValidate(_equipmentId,_userId,TrainningExaminationBusinessType.Equipment),
               new UserExaminationValidate(_equipmentId,_userId,TrainningExaminationBusinessType.LabOrganization),
               new AppointmentMoneyValidate(_equipmentId,_userId,_appointmentMidiator)
            };
        }
    }
}
