using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPAppointmentAuditStatusValidate : NMPAppointmentValidateBase
    {
        private NMPAppointment __nmpAppointment;
        public NMPAppointmentAuditStatusValidate(Guid nmpEquipmentId, Guid nmpId, Guid userId, NMPAppointment nmpAppointment)
            : base(nmpEquipmentId, nmpId, userId, new List<NMPAppointment> { nmpAppointment }) 
        {
            __nmpAppointment = nmpAppointment;
        }

        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            if (!_nmp.IsEnableCancelCheckedAppointment)
            {
                if (__nmpAppointment != null && __nmpAppointment.AuditStatusEnum != Model.Enum.AppointmentAuditStatus.UnAudit)
                {
                    errorMessage = "该预约已经审核,不能进行取消预约操作";
                    return false;
                }
            }
            return true;
        }
    }
}
