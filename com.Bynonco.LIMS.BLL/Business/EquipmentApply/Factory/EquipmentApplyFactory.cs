using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{

    public class EquipmentApplyFactory
    {
        public static EquipmentApplyBase CreateEquipmentApplyManager(Guid operatorId, EquipmentApplyRole equipmentApplyRole)
        {
            switch (equipmentApplyRole)
            {
                case EquipmentApplyRole.Applicant:
                    return new EquipmentApplicant(operatorId);
                case EquipmentApplyRole.LabRoomDirector:
                    return new EquipmentLabRoomDirectorAudit(operatorId);
                case EquipmentApplyRole.SchoolDirector:
                    return new EquipmentSchoolDirectorAudit(operatorId);
                case EquipmentApplyRole.ShareEDirector:
                    return new EquipmentShareEAudit(operatorId);
            }
            throw new Exception("设备入网申请用户角色错误,创建对象失败");
        }
    }
}
