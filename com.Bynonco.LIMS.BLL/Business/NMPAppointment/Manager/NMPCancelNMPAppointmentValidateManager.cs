using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPCancelAppointmentValidateManager : NMPAppointmentValidateManagerBase
    {
        public NMPCancelAppointmentValidateManager(Guid nmpEquipmentId, Guid nmpId, Guid userId, NMPAppointment nmpAppointment)
            : base(nmpEquipmentId, nmpId, userId, new List<NMPAppointment>() { nmpAppointment })
        {
            _nmpAppointments = new List<NMPAppointment>() { nmpAppointment };
        }
        public override bool Validate(out string errorMessage)
        {
            if (_nmpAppointments == null)
            {
                errorMessage = "取消预约记录为空";
                return false;
            }
            return base.Validate(out errorMessage);
        }
        protected override IList<NMPAppointmentValidateBase> CreateNMPAppointmentValidates()
        {
            List<NMPAppointmentValidateBase> lstNMPAppointmentValidate = new List<NMPAppointmentValidateBase>()
            {
               new NMPCalcFeeRuleValidate(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments),
               new  NMPChargeStandardValidate(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments),
               new  NMPAppointmentAuditStatusValidate(_nmpEquipmentId,_nmpId,_userId,_nmpAppointments.First()),
               new  NMPUserStatusValidate(_nmpEquipmentId, _nmpId, _userId, _nmpAppointments)
            };
            return lstNMPAppointmentValidate;
        }
    }
}
