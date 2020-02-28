using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class NMPCalcFeeRuleValidate : NMPAppointmentValidateBase
    {
        public NMPCalcFeeRuleValidate(Guid nmpEquipmentId, Guid nmpId, Guid userId, IEnumerable<NMPAppointment> nmpAppointments)
            : base(nmpEquipmentId, nmpId, userId, nmpAppointments) { }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            if (_nmp.CalcFeeTimeRule == null)
            {
                errorMessage = "未定义工位的计费时间规则";
            }
            return true;
        }
    }
}
