using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary>
    /// 设备入网申请角色
    /// </summary>
    public enum EquipmentApplyRole
    {
        /// <summary>
        /// 申请人
        /// </summary>
        Applicant,
        /// <summary>
        /// 重点实验室负责人
        /// </summary>
        LabRoomDirector,
        /// <summary>
        /// 学校负责人
        /// </summary>
        SchoolDirector,
        /// <summary>
        /// 仪器共享平台负责人
        /// </summary>
        ShareEDirector
    }
}
