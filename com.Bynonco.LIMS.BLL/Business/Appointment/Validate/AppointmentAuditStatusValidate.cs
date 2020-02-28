using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentAuditStatusValidate: AppointmentValidateBase
    {
        public AppointmentAuditStatusValidate(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator)
            : base(equipmentId, userId, appointmentMidiator) { }

        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            if (!_equipment.IsEnableCancelCheckedAppointment)
            {
                //取消预约校验
                if (_appointmentMidiator.CanceledAppointment != null && _appointmentMidiator.CanceledAppointment.AuditStatusEnum != Model.Enum.AppointmentAuditStatus.UnAudit)
                {
                    errorMessage = "该预约已经审核,不能进行取消预约操作";
                    return false;
                }
                //改约校验
                if (_appointmentMidiator.ChangeOldAppointment != null && _appointmentMidiator.ChangeOldAppointment.AuditStatusEnum != Model.Enum.AppointmentAuditStatus.UnAudit)
                {
                    errorMessage = "该预约已经审核,不能进行改约操作";
                    return false;
                }
            }
            return true;
        }
    }
}
