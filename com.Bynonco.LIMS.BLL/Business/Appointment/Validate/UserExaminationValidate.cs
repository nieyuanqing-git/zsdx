using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class UserExaminationValidate:AppointmentValidateBase
    {
        private IUserExaminationBLL __userExaminationBLL;
        private TrainningExaminationBusinessType __trainningExaminationBusinessType;
        public UserExaminationValidate(Guid equipmentId, Guid userId, TrainningExaminationBusinessType trainningExaminationBusinessType)
            : base(equipmentId, userId)
        {
            __trainningExaminationBusinessType = trainningExaminationBusinessType;
            __userExaminationBLL = BLLFactory.CreateUserExaminationBLL();
        }
        public override bool Validates(out string errorMessage)
        {
            errorMessage=  "";
            return __userExaminationBLL.Validates(_equipmentId, _userId, __trainningExaminationBusinessType, out errorMessage);
        }
    }
}
