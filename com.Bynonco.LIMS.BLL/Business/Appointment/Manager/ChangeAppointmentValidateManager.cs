using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.BLL.Business.AppointmentRule;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class ChangeAppointmentValidateManager: AppointmentValidateManagerBase
    {
        public ChangeAppointmentValidateManager(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator)
            : base(equipmentId, userId, appointmentMidiator) 
        {
            this._appointmentMidiator = appointmentMidiator;
        }
        public override bool Validate(out string errorMessage)
        {
            errorMessage = "";
            if (_appointmentMidiator.ChangeOldAppointment == null)
            {
                errorMessage = "改约记录为空";
                return false;
            }
            if (_appointmentMidiator.ChangeNewAppointment == null)
            {
                errorMessage = "新的预约记录为空";
                return false;
            }
            var appointmentValidates = CreateAppointmentValidates();
            if (appointmentValidates == null) return true;
            foreach (var appointmentValidate in appointmentValidates)
            {
                if (!appointmentValidate.Validates(out errorMessage)) return false;
            }
            var equipment = _equipmentBLL.GetEntityById(_appointmentMidiator.ChangeNewAppointment.EquipmentId.Value);
            var appointmentTimes = new List<Appointment>() { _appointmentMidiator.ChangeNewAppointment }.Select(p => new EquipmentAppointmentTime() { BeginTime = p.BeginTime.Value, EndTime = p.EndTime.Value, Remark = "", Status = EquipmentAppointmentTimeStatus.Valid }).ToList();
            EquipmentAppointmentTimes equipmentAppointmentTimes = new EquipmentAppointmentTimes(equipment.OpenAdvanceTime, equipment.MaxAppointmentAdvanceTime.Value, equipment.MinAppointmentCancelTime.Value, equipment.AppointmentTimeStep.Value, 0, 0, appointmentTimes);
            IAppointmentTimesManager appointmentRuleManager = new AppointmentTimesManager(Model.Enum.AppointmentTimeBehaviorOfUser.Book, _appointmentMidiator.ChangeNewAppointment.EquipmentId.Value, _appointmentMidiator.ChangeNewAppointment.UserId.Value, equipmentAppointmentTimes, _appointmentMidiator.ChangeNewAppointment, _appointmentMidiator.ChangeNewAppointment.AppointmentEquipmentParts != null && _appointmentMidiator.ChangeNewAppointment.AppointmentEquipmentParts.Count > 0 ? _appointmentMidiator.ChangeNewAppointment.AppointmentEquipmentParts.Select(p => p.EquipmentPartId).ToArray() : null, _equipmentTimeAppointmemtMode);
            if (!appointmentRuleManager.Validate(out errorMessage)) return false;
            return true;
        }

        protected override IList<AppointmentValidateBase> CreateAppointmentValidates()
        {
            return new List<AppointmentValidateBase> 
            {
               new OutOfTimeValidate(_equipmentId,_userId,_appointmentMidiator),
               new EquipmentCalcFeeRuleValidate(_equipmentId,_userId),
               new EquipmentChargeStandardValidate(_equipmentId,_userId),
               new AppointmentAuditStatusValidate(_equipmentId,_userId,_appointmentMidiator),
               new UserExaminationValidate(_equipmentId,_userId,TrainningExaminationBusinessType.Equipment), 
               new UserExaminationValidate(_equipmentId,_userId,TrainningExaminationBusinessType.LabOrganization), 
               new UserStatusValidate(_equipmentId,_userId),
               new EquipmentUserLimitValidate(_equipmentId,_userId,_appointmentMidiator),
               new AppointmentMoneyValidate(_equipmentId,_userId,_appointmentMidiator)
            };
        }
    }
}
