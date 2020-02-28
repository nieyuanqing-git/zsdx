using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{

    public class EquipmentRepairFundsApplyFactory
    {
        public static EquipmentRepairFundsApplyBase CreateEquipmentRepairFundsApplyManager(Guid operatorId, EquipmentRepairFundsApplyRole equipmentRepairFundsApplyRole)
        {
            switch (equipmentRepairFundsApplyRole)
            {
                case EquipmentRepairFundsApplyRole.Applicant:
                    return new EquipmentRepairFundsApplicant(operatorId);
                case EquipmentRepairFundsApplyRole.LabRoomAuditor:
                    return new EquipmentRepairFundsLabRoomAudit(operatorId);
                case EquipmentRepairFundsApplyRole.CollegeAuditor:
                    return new EquipmentRepairFundsCollegeAudit(operatorId);
                case EquipmentRepairFundsApplyRole.EquipmentDeptAuditor:
                    return new EquipmentRepairFundsEquipmentDeptAudit(operatorId);
                case EquipmentRepairFundsApplyRole.ShareEAuditor:
                    return new EquipmentRepairFundsShareEAudit(operatorId);
                case EquipmentRepairFundsApplyRole.SchoolMaster:
                    return new EquipmentRepairFundsSchoolEAudit(operatorId);
            }
            throw new Exception("设备维修基金申请用户角色错误,创建对象失败");
        }
    }
}
