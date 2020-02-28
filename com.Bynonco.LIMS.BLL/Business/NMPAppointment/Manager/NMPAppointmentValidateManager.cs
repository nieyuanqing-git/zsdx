using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.BLL.Business.AppointmentRule;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Business.NMPAppointmentRule;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPAppointmentValidateManager : NMPAppointmentValidateManagerBase
    {
        public NMPAppointmentValidateManager(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments)
            : base(nmpEquipmentId, nmpId, userId, nmpAppointments)
        {
            _nmpAppointments = nmpAppointments;
        }

        public override bool Validate(out string errorMessage)
        {
            errorMessage = "";
            if (_nmpAppointments == null || _nmpAppointments == null || _nmpAppointments.Count() == 0)
            {
                errorMessage = "预约记录为空";
                return false;
            }
            var appointmentValidates = CreateNMPAppointmentValidates();
            if (appointmentValidates == null) return true;
            foreach (var appointmentValidate in appointmentValidates)
            {
                if (!appointmentValidate.Validates(out errorMessage)) return false;
            }
            var nmpAppointment = _nmpAppointments.First();
            var nmp = _nmpBLL.GetEntityById(_nmpId);
            var appointmentTimes = _nmpAppointments.Select(p => new EquipmentAppointmentTime() { BeginTime = p.BeginTime.Value, EndTime = p.EndTime.Value, Remark = "", Status = EquipmentAppointmentTimeStatus.Valid }).ToList();
            EquipmentAppointmentTimes equipmentAppointmentTimes = new EquipmentAppointmentTimes(nmp.OpenAdvanceTime, nmp.MaxAppointmentAdvanceTime, nmp.MinAppointmentCancelTime, nmp.AppointmentTimeStep, 0, 0, appointmentTimes);
            INMPAppointmentTimesManager nmpAppointmentRuleManager = new NMPAppointmentTimesManager(Model.Enum.AppointmentTimeBehaviorOfUser.Book, _nmpId, _nmpEquipmentId,nmpAppointment.UserId.Value,equipmentAppointmentTimes, nmpAppointment, null);
            if (!nmpAppointmentRuleManager.Validate(out errorMessage)) return false;
            return true;
        }

        protected override IList<NMPAppointmentValidateBase> CreateNMPAppointmentValidates()
        {
            return new List<NMPAppointmentValidateBase>()
            {
               new NMPCalcFeeRuleValidate(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments),
               new NMPChargeStandardValidate(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments),
               new NMPUserStatusValidate(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments),
               new NMPUserLimitValidate(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments),
               new NMPSubjectLimitValidate(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments),
               new NMPAppointmentMoneyValidate(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments)
            };
        }
    }
}
