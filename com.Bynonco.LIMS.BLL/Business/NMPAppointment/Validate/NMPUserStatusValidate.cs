using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPUserStatusValidate: NMPAppointmentValidateBase
    {
        public NMPUserStatusValidate(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments)
            : base(nmpEquipmentId, nmpId, userId, nmpAppointments) { }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            return _userBLL.CheckUserStatus(_userId, out errorMessage);
        }
    }
}
