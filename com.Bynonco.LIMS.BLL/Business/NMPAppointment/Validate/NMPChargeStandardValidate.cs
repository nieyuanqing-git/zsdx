using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPChargeStandardValidate: NMPAppointmentValidateBase
    {
        public NMPChargeStandardValidate(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments)
            : base(nmpEquipmentId, nmpId, userId, nmpAppointments) { }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            if (_nmp.ChargeStandard == null)
            {
                errorMessage = "未定义工位的计费标准";
            }
            return true;
        }
    }
}
