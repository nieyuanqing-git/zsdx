﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL.Business.AppointmentRule
{
    public interface IAppointmentRule
    {
        void Superimposing(EquipmentAppointmentTime appointmentTime);
    }
}
