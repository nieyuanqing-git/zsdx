using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.LuaApi
{
    
    public class AppointmentLuaApi
    {
        private IAppointmentBLL __appointmentBLL;
        private IEquipmentBLL __equipmentBLL;
        private IEquipmentPartBLL __equipmentPartBLL;
        public AppointmentLuaApi()
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __appointmentBLL = BLLFactory.CreateAppointmentBLL();
            __equipmentPartBLL = BLLFactory.CreateEquipmentPartBLL();
        }
        [LuaFunction("IsCurUserAppoitmentTheEquipmentPart")]
        public bool IsCurUserAppoitmentTheEquipmentPart(Guid[] equipmentPartIds, string equipmentPartName)
        {
            return __appointmentBLL.JudgeIsExistTheEquipmentPartName(equipmentPartIds, equipmentPartName);
        }
        [LuaFunction("IsSelectTheEquipmentPart")]
        public bool IsSelectTheEquipmentPart(Guid[] equipmentPartIds, string equipmentPartName)
        {
            if (equipmentPartIds == null || equipmentPartIds.Length == 0) return false;
            var equipmentPartList = __equipmentPartBLL.GetEntitiesByExpression(equipmentPartIds.ToFormatStr());
            if (equipmentPartList == null || equipmentPartList.Count == 0) return false;
            return equipmentPartList.Any(p => p.Name == equipmentPartName);
        }
        [LuaFunction("IsCurUserEnableAppoitmentByPX")]
        public string IsCurUserEnableAppoitmentByPX(string equipmentId,string roomId, DateTime beginTime, DateTime endTime, Appointment appointment)
        {
            string errorMessage = "";
            var equipment = __equipmentBLL.GetEntityById(new Guid(equipmentId));
            var result = __appointmentBLL.JudgeIsEnableAppoitmentByPX(equipment, beginTime, endTime, new Guid(roomId), appointment, out errorMessage);
            return errorMessage;
        }
    }
}
