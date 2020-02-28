using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class CancelAppointmentValidateManager: AppointmentValidateManagerBase
    {
        public CancelAppointmentValidateManager(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator)
            :base(equipmentId,userId,appointmentMidiator)
        {
            _appointmentMidiator = appointmentMidiator;
        }
        public override bool Validate(out string errorMessage)
        {
            if (_appointmentMidiator.CanceledAppointment == null)
            {
                errorMessage = "取消预约记录为空";
                return false;
            }
            return base.Validate(out errorMessage);
        }
        protected override IList<AppointmentValidateBase> CreateAppointmentValidates()
        {
            List<AppointmentValidateBase> lstAppointmentValidate = new List<AppointmentValidateBase>()
            {
               new EquipmentCalcFeeRuleValidate(_equipmentId,_userId),
               new EquipmentChargeStandardValidate(_equipmentId,_userId),
               new AppointmentAuditStatusValidate(_equipmentId,_userId,_appointmentMidiator),
               new UserStatusValidate(_equipmentId,_userId),
               
            };
            if (!_appointmentMidiator.IsAutoCancel) 
                lstAppointmentValidate.Add(new OutOfTimeValidate(_equipmentId, _userId, _appointmentMidiator));
            return lstAppointmentValidate;
        }
    }
}
