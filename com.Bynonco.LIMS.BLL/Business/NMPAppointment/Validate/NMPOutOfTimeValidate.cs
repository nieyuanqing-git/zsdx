using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPOutOfTimeValidate: NMPAppointmentValidateBase
    {
       private NMPAppointment __nmpAppointment;
       public NMPOutOfTimeValidate(Guid nmpEquipmentId, Guid nmpId, Guid userId, NMPAppointment nmpAppointment)
            : base(nmpEquipmentId, nmpId, userId, new List<NMPAppointment> { nmpAppointment }) 
        {
            __nmpAppointment = nmpAppointment;
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage = "";
            if (!_nmp.IsEnableCancelNotOverAppointment && __nmpAppointment.EndTime.Value > DateTime.Now)
            {
                if (__nmpAppointment.BeginTime.Value < DateTime.Now.AddMinutes(_nmp.MinAppointmentCancelTime))
                {
                    errorMessage = "过了提前取消的时间,不能取消预约";
                    return false;
                }
            }
            return true;
        }
    }
}
