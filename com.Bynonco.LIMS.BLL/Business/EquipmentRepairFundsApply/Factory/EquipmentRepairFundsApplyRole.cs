using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary>
    /// 设备维修基金申请角色
    /// </summary>
    public enum EquipmentRepairFundsApplyRole
    {
        /// <summary>
        /// 申请人
        /// </summary>
        Applicant,
        /// <summary>
        /// 实验室审核人
        /// </summary>
        LabRoomAuditor,
        /// <summary>
        /// 学院审核人
        /// </summary>
        CollegeAuditor,
        /// <summary>
        /// 设备处审核人
        /// </summary>
        EquipmentDeptAuditor,
        /// <summary>
        /// 仪器共享平台审核人
        /// </summary>
        ShareEAuditor,
        /// <summary>
        /// 校长
        /// </summary>
        SchoolMaster
    }
}
