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
    public class CalcFeeTimeRuleBLL : BLLBase<CalcFeeTimeRule>, ICalcFeeTimeRuleBLL
    {
        private CalcFeeTimeRuleRalativeBLL __calcFeeTimeRuleRalativeBLL;
        private const string __FUNCTIONNAME = "calTimeRule";
        public CalcFeeTimeRuleBLL()
        {
            __calcFeeTimeRuleRalativeBLL = new CalcFeeTimeRuleRalativeBLL();
        }
        public double CalcFeeTime(bool isDurationPrice, Guid equipmentId,Guid? userId,DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, string luaExpression, int roundFator = 0, int roundDigits = 2)
        {
            #region original
            //if (string.IsNullOrWhiteSpace(luaExpression)) throw new Exception("lua表达式为空");
            //if (!usedBeginTime.HasValue && !usedEndTime.HasValue && !appointmentBeginTime.HasValue && !appointmentEndTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            //if (!usedBeginTime.HasValue && !usedEndTime.HasValue && !appointmentBeginTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            //if (!usedBeginTime.HasValue && !usedEndTime.HasValue && !appointmentEndTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            //if (!appointmentBeginTime.HasValue && !appointmentEndTime.HasValue && !usedBeginTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            //if (!appointmentBeginTime.HasValue && !appointmentEndTime.HasValue && !usedEndTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            //if (usedEndTime.HasValue && !usedBeginTime.HasValue) throw new Exception("使用开始时间为空"); 
            //if (usedBeginTime.HasValue && !usedEndTime.HasValue) throw new Exception("使用结束时间为空");
            //if (appointmentEndTime.HasValue && !appointmentBeginTime.HasValue) throw new Exception("预约开始时间为空");
            //if (appointmentBeginTime.HasValue && !appointmentEndTime.HasValue) throw new Exception("预约结束时间为空"); 
            //var now = DateTime.Now;
            //if (!usedBeginTime.HasValue) usedBeginTime = now;
            //if (!usedEndTime.HasValue) usedEndTime = now;
            //if (!appointmentBeginTime.HasValue) appointmentBeginTime = now;
            //if (!appointmentEndTime.HasValue) appointmentEndTime = now;
            //StringBuilder sbLuaCommand = new StringBuilder();
            //sbLuaCommand.AppendLine("function calFeeTime(t1,t2,t3,t4,isDurationPrice,appointmentBeginTime,appointmentEndTime,usedBeginTime,usedEndTime,userId,equipmentId) ");
            //sbLuaCommand.AppendLine(luaExpression);
            //sbLuaCommand.AppendLine(" end");
            //LuaManager.Instance.ExecuteString(sbLuaCommand.ToString());
            //var results = LuaManager.Instance.CallFunction("calFeeTime", 
            //        (usedBeginTime.Value - DateTime.MinValue).TotalMinutes, 
            //        (usedEndTime.Value - DateTime.MinValue).TotalMinutes, 
            //        (appointmentBeginTime.Value - DateTime.MinValue).TotalMinutes, 
            //        (appointmentEndTime.Value - DateTime.MinValue).TotalMinutes, 
            //        isDurationPrice,
            //        appointmentBeginTime,
            //        appointmentEndTime,
            //        usedBeginTime,
            //        usedEndTime,
            //        userId.HasValue ? userId.Value.ToString() : "",
            //        equipmentId.ToString()
            //    );
            //var minute = double.Parse(results[0].ToString());
            //double hour = 0d;
            //if (minute - roundFator <= 0)
            //{
            //    hour = roundFator / 60.00000000000000;
            //}
            //else
            //{
            //    hour = roundFator == 0 ? minute / 60.00000000000000 : ((minute + (minute % roundFator != 0 ? (roundFator - minute % roundFator) : 0)) / 60.00000000000000);
            //}
            //return double.Parse(hour.ToString(string.Format("0.{0}", "".PadRight(roundDigits, '0'))));
            #endregion

            IDictionary<string, object> ddParameters = new Dictionary<string, object>();
            ddParameters["appointmentBeginTime"] = appointmentBeginTime;
            ddParameters["appointmentEndTime"] = appointmentEndTime;
            ddParameters["usedBeginTime"] = usedBeginTime;
            ddParameters["usedEndTime"] = usedEndTime;
            ddParameters["isDurationPrice"] = isDurationPrice;
            ddParameters["userId"] = userId.HasValue ? userId.Value.ToString() : null;
            ddParameters["equipmentId"] = equipmentId.ToString();
            return __calcFeeTimeRuleRalativeBLL.CalcFeeTime(__FUNCTIONNAME, ddParameters, luaExpression, roundFator, roundDigits);
        }


        public bool ValidateCalcFeeTimeExpression(string luaExpression, out string errorMessage)
        {
            #region original
            //errorMessage = "";
            //try
            //{
            //    StringBuilder sbLuaCommand = new StringBuilder();
            //    sbLuaCommand.AppendLine("function calFeeTime(t1,t2,t3,t4,isDurationPrice,appointmentBeginTime,appointmentEndTime,usedBeginTime,usedEndTime,userId,equipmentId) ");
            //    sbLuaCommand.AppendLine(luaExpression);
            //    sbLuaCommand.AppendLine(" end");
            //    return LuaManager.Instance.ExecuteString(sbLuaCommand.ToString());
            //}
            //catch (Exception ex)
            //{
            //    errorMessage = ex.Message;
            //    return false;
            //}
            #endregion

            errorMessage = "";
            return __calcFeeTimeRuleRalativeBLL.ValidateCalcFeeTimeExpression(__FUNCTIONNAME, luaExpression, new string[] { "t1", "t2", "t3", "t4", "appointmentBeginTime", "appointmentEndTime", "usedBeginTime", "usedEndTime", "isDurationPrice", "userId", "equipmentId" }, out errorMessage);
        }
    }
}