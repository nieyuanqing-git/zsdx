using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.LuaApi;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class LuaRegiserBLL : ILuaRegiserBLL
    {

        public void RegisterAll()
        {
            DateTimeLuaApi dateTimeLuaApi = new DateTimeLuaApi();
            WeekLuaApi weekLuaApi = new WeekLuaApi();
            EquipmentLuaApi equipmentLuaApi = new EquipmentLuaApi();
            AppointmentLuaApi appointmentLuaApi = new AppointmentLuaApi();
            LuaManager.Instance.BindLuaAPIClass(dateTimeLuaApi);
            LuaManager.Instance.BindLuaAPIClass(weekLuaApi);
            LuaManager.Instance.BindLuaAPIClass(equipmentLuaApi);
            LuaManager.Instance.BindLuaAPIClass(appointmentLuaApi);
        }
    }
}
