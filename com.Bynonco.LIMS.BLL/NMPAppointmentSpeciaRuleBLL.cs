using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPAppointmentSpeciaRuleBLL : BLLBase<NMPAppointmentSpeciaRule>, INMPAppointmentSpeciaRuleBLL
    {
        private object[] ExecLua(string luaExpression, DateTime beginTime, DateTime endTime, Guid nmpId, Guid? userId, Guid roomId, NMPAppointment nmpAppointment, Guid[] nmpPartIds)
        {
            StringBuilder sbLuaCommand = new StringBuilder();
            sbLuaCommand.AppendLine("function specialNMPAppointmentRule(beginTime,endTime,equipmentId,userId,roomId,nmpAppointment,nmpPartIds) ");
            sbLuaCommand.AppendLine(luaExpression);
            sbLuaCommand.AppendLine(" end");
            try
            {

                LuaManager.Instance.ExecuteString(sbLuaCommand.ToString());
                var results = LuaManager.Instance.CallFunction("specialNMPAppointmentRule", beginTime, endTime, nmpId.ToString(), userId.HasValue ? userId.Value.ToString() : "", roomId.ToString(), nmpAppointment, nmpPartIds);
                return results;
            }
            catch (System.Exception ex) { return null; }
        }
        public bool Validates(IList<NMPAppointmentSpeciaRule> appointmentSpeciaRuleList, DateTime beginTime, DateTime endTime, Guid nmpId, Guid? userId, Guid roomId, NMPAppointment nmpAppointment, Guid[] nmpPartIds, out string errorMessage, out string tipMessage, out bool isSingleSelect)
        {
            INMPAppointmentSpeciaRuleUserBLL nmpAppointmentSpeciaRuleUserBLL = BLLFactory.CreateNMPAppointmentSpeciaRuleUserBLL();
            errorMessage = "";
            tipMessage = "";
            isSingleSelect = false;
            if (appointmentSpeciaRuleList == null || appointmentSpeciaRuleList.Count == 0)
                appointmentSpeciaRuleList = appointmentSpeciaRuleList = GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", nmpId.ToString()));
            foreach (var appointmentSpeciaRule in appointmentSpeciaRuleList)
            {
                if (string.IsNullOrWhiteSpace(appointmentSpeciaRule.Expression)) continue;
                if (appointmentSpeciaRule.Users != null && appointmentSpeciaRule.Users.Count > 0)
                {
                    if (!userId.HasValue) continue;
                    var appointmentSpeciaRuleUserList = nmpAppointmentSpeciaRuleUserBLL.GetEntitiesByExpression(string.Format("NMPAppointmentSpecialRuleId=\"{0}\"*UserId=\"{1}\"", appointmentSpeciaRule.Id.ToString(), userId.Value.ToString()));
                    if (appointmentSpeciaRuleUserList == null || appointmentSpeciaRuleUserList.Count == 0) continue;
                }
                var results = ExecLua(appointmentSpeciaRule.Expression, beginTime, endTime, nmpId, userId, roomId, nmpAppointment, nmpPartIds);
                if (results == null || results.Length == 0) continue;
                var result = Convert.ToBoolean(results[0]);
                if (results.Length > 2 && results[2] != null)
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
        public bool Validates(DateTime beginTime, DateTime endTime, Guid nmpId, Guid? userId, Guid roomId, NMPAppointment nmpAppointment, Guid[] nmpPartIds, out string errorMessage, out string tipMessage, out bool isSingleSelect)
        {
            return Validates(null, beginTime, endTime, nmpId, userId, roomId, nmpAppointment, nmpPartIds, out errorMessage, out tipMessage, out isSingleSelect);
        }
    }
}
