using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPDelinquencyValidate : NMPAppointmentValidateBase
    {
        private IPunishActionBLL __punishActionBLL;
        public NMPDelinquencyValidate(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments)
            : base(nmpEquipmentId, nmpId, userId, nmpAppointments)
        {
            __punishActionBLL = BLLFactory.CreatePunishActionBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage=  "";
            return __punishActionBLL.Validates(_userId, out errorMessage);
        }
    }
}
