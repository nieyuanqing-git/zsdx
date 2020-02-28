using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL.Business
{
   public class BeforeAppointmentValidateManager:AppointmentValidateManagerBase
    {
       public BeforeAppointmentValidateManager(Guid equipmentId, Guid userId, AppointmentMidiator appointmentMidiator=null)
           : base(equipmentId, userId, appointmentMidiator) { }

        protected override IList<AppointmentValidateBase> CreateAppointmentValidates()
        {
            return new List<AppointmentValidateBase>()
            { 
                new UserStatusValidate(_equipmentId,_userId),
                new DelinquencyValidate(_equipmentId,_userId),
                new EquipmentBlackListValidate(_equipmentId,_userId),
                new EquipmentNoticValidate(_equipmentId,_userId),
                new TranningValidate(_equipmentId,_userId),
                new UserEquipmentValidate(_equipmentId,_userId),
                new UsedConfirmFeedBackValidate(_equipmentId,_userId),
                new UserExaminationValidate(_equipmentId,_userId,TrainningExaminationBusinessType.Equipment),
                new UserExaminationValidate(_equipmentId,_userId,TrainningExaminationBusinessType.LabOrganization)
            };
        }
    }
}
