using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    /// <summary>
    /// 预约类型
    /// </summary>
    public enum AppointmentBusinessType
    {
        /// <summary>
        /// 预约
        /// </summary>
        Appointment,
        /// <summary>
        /// 取消预约
        /// </summary>
        CancelAppointment,
        /// <summary>
        /// 改约
        /// </summary>
        ChangeAppointment
    }
}
