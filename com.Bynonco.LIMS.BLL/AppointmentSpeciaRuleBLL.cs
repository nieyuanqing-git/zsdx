using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentSpeciaRuleBLL : BLLBase<AppointmentSpeciaRule>, IAppointmentSpeciaRuleBLL
    {
        private IList<Guid> appointmentSpeciaRuleids = null;
        private bool isAppointmentValidate = false;
        public AppointmentSpeciaRuleBLL(IList<Guid> appointmentSpeciaRuleids)
        {
            this.appointmentSpeciaRuleids = appointmentSpeciaRuleids;
          isAppointmentValidate = true;
        }
        public AppointmentSpeciaRuleBLL(bool isAppointmentValidate = false)//bool isAppointmentValidate = false
        {
            this.isAppointmentValidate = isAppointmentValidate;
        }
       
        private object[] ExecLua(string luaExpression, DateTime beginTime, DateTime endTime, Guid equipmentId, Guid? userId, Guid roomId, Appointment appointment, Guid[] equipmentPartIds)
        {
            StringBuilder sbLuaCommand = new StringBuilder();
            sbLuaCommand.AppendLine("function specialAppointmentRule(beginTime,endTime,equipmentId,userId,roomId,appointment,equipmentPartIds) ");
            sbLuaCommand.AppendLine(luaExpression);
            sbLuaCommand.AppendLine(" end");
            try
            {
                
                LuaManager.Instance.ExecuteString(sbLuaCommand.ToString());
                var results = LuaManager.Instance.CallFunction("specialAppointmentRule", beginTime, endTime, equipmentId.ToString(), userId.HasValue ? userId.Value.ToString() : "", roomId.ToString(), appointment, equipmentPartIds);
                return results;
            }
            catch (System.Exception ex) { return null; }
        }
        public bool Validates(IList<AppointmentSpeciaRule> appointmentSpeciaRuleList, DateTime beginTime, DateTime endTime, Guid equipmentId, Guid? userId, Guid roomId, Appointment appointment,Guid[] equipmentPartIds, out string errorMessage, out string tipMessage, out bool isSingleSelect)
        {
            IAppointmentSpeciaRuleUserBLL appointmentSpeciaRuleUserBLL = BLLFactory.CreateAppointmentSpeciaRuleUserBLL();
            errorMessage = "";
            tipMessage = "";
            isSingleSelect = false;
            if (appointmentSpeciaRuleList == null || appointmentSpeciaRuleList.Count == 0)
                appointmentSpeciaRuleList = appointmentSpeciaRuleList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", equipmentId.ToString()));
            foreach (var appointmentSpeciaRule in appointmentSpeciaRuleList)
            {
                if (string.IsNullOrWhiteSpace(appointmentSpeciaRule.Expression)) continue;
                
                 if (isAppointmentValidate)
                 {
                     if (appointmentSpeciaRuleids != null && appointmentSpeciaRuleids.Contains(appointmentSpeciaRule.Id) && appointmentSpeciaRule.Users != null && appointmentSpeciaRule.Users.Count > 0)
                     {
                         if (!userId.HasValue) continue;
                         var appointmentSpeciaRuleUserList = appointmentSpeciaRuleUserBLL.GetEntitiesByExpression(string.Format("AppointmentSpecialRuleId=\"{0}\"*UserId=\"{1}\"", appointmentSpeciaRule.Id.ToString(), userId.Value.ToString()));
                         if (appointmentSpeciaRuleUserList == null || appointmentSpeciaRuleUserList.Count == 0) continue;
                     }
                 }
                 else {
                     if (appointmentSpeciaRule.Users != null && appointmentSpeciaRule.Users.Count > 0)
                     {
                         if (!userId.HasValue) continue;
                         var appointmentSpeciaRuleUserList = appointmentSpeciaRuleUserBLL.GetEntitiesByExpression(string.Format("AppointmentSpecialRuleId=\"{0}\"*UserId=\"{1}\"", appointmentSpeciaRule.Id.ToString(), userId.Value.ToString()));
                         if (appointmentSpeciaRuleUserList == null || appointmentSpeciaRuleUserList.Count == 0) continue;
                     }
                 }
                var results = ExecLua(appointmentSpeciaRule.Expression, beginTime, endTime, equipmentId, userId, roomId, appointment, equipmentPartIds);
                if (results == null || results.Length == 0) continue;
                var result = Convert.ToBoolean(results[0]);
                if(results.Length > 2 && results[2] != null)
                {
                    tipMessage = results[2].ToString();
                }
                if (results.Length > 3 && results[3] != null)
                {
                    isSingleSelect = (bool)results[3];
                }
                if (!result)
                {
                    errorMessage = results.Length > 1 ? results[1].ToString() : "";
                    return result;
                }
            }
            return true;
        }
        public bool Validates(DateTime beginTime, DateTime endTime, Guid equipmentId, Guid? userId, Guid roomId, Appointment appointment, Guid[] equipmentPartIds, out string errorMessage, out string tipMessage, out bool isSingleSelect)
        {
            return Validates(null, beginTime, endTime, equipmentId, userId, roomId, appointment, equipmentPartIds, out errorMessage, out tipMessage, out isSingleSelect);
        }
    }
}