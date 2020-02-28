using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class CalcFeeTimeRuleRalativeBLL : ICalcFeeTimeRuleRalativeBLL
    {
        object _lockLua = new object();
        public double CalcFeeTime(string functionName,IDictionary<string, object> ddParameters, string luaExpression, int roundFator = 0, int roundDigits = 2)
        {
            var usedBeginTime = (DateTime?)ddParameters["usedBeginTime"];
            var usedEndTime = (DateTime?)ddParameters["usedEndTime"];
            var appointmentBeginTime = (DateTime?)ddParameters["appointmentBeginTime"];
            var appointmentEndTime = (DateTime?)ddParameters["appointmentEndTime"];
            if (string.IsNullOrWhiteSpace(luaExpression)) throw new Exception("lua表达式为空");
            if (!usedBeginTime.HasValue && !usedEndTime.HasValue && !appointmentBeginTime.HasValue && !appointmentEndTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            if (!usedBeginTime.HasValue && !usedEndTime.HasValue && !appointmentBeginTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            if (!usedBeginTime.HasValue && !usedEndTime.HasValue && !appointmentEndTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            if (!appointmentBeginTime.HasValue && !appointmentEndTime.HasValue && !usedBeginTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            if (!appointmentBeginTime.HasValue && !appointmentEndTime.HasValue && !usedEndTime.HasValue) throw new Exception("使用时间和预约时间不能同时为空");
            if (usedEndTime.HasValue && !usedBeginTime.HasValue) throw new Exception("使用开始时间为空");
            if (usedBeginTime.HasValue && !usedEndTime.HasValue) throw new Exception("使用结束时间为空");
            if (appointmentEndTime.HasValue && !appointmentBeginTime.HasValue) throw new Exception("预约开始时间为空");
            if (appointmentBeginTime.HasValue && !appointmentEndTime.HasValue) throw new Exception("预约结束时间为空");
            var now = DateTime.Now;
            if (!usedBeginTime.HasValue) usedBeginTime = now;
            if (!usedEndTime.HasValue) usedEndTime = now;
            if (!appointmentBeginTime.HasValue) appointmentBeginTime = now;
            if (!appointmentEndTime.HasValue) appointmentEndTime = now;
            var t1 = (usedBeginTime.Value - DateTime.MinValue).TotalMinutes;
            var t2 = (usedEndTime.Value - DateTime.MinValue).TotalMinutes;
            var t3 = (appointmentBeginTime.Value - DateTime.MinValue).TotalMinutes;
            var t4 = (appointmentEndTime.Value - DateTime.MinValue).TotalMinutes;
            IList<object> lstParameterValues = new List<object>();
            lstParameterValues.Add(t1);
            lstParameterValues.Add(t2);
            lstParameterValues.Add(t3);
            lstParameterValues.Add(t4);
            lstParameterValues.Add(appointmentBeginTime);
            lstParameterValues.Add(appointmentEndTime);
            lstParameterValues.Add(usedBeginTime);
            lstParameterValues.Add(usedEndTime);
            var ddParameterTemp =  ddParameters.Where(p=>p.Key!="t1" && p.Key!="t2" && p.Key!="t3" && p.Key!="t4" && p.Key!="appointmentBeginTime" && p.Key!="appointmentEndTime" && p.Key!="usedBeginTime" && p.Key!="usedEndTime");
            var keys = ddParameterTemp != null ? ddParameterTemp.Select(p => p.Key) : null;
            var values = ddParameterTemp != null ? ddParameterTemp.Select(p => p.Value) : null;
            foreach (var value in values) lstParameterValues.Add(value);
            StringBuilder sbLuaCommand = new StringBuilder();
            sbLuaCommand.AppendFormat("function {0}(t1,t2,t3,t4,appointmentBeginTime,appointmentEndTime,usedBeginTime,usedEndTime{1}) ", functionName, keys != null && keys.Count() > 0 ? ("," + string.Join(",", keys)) : "").AppendLine();
            sbLuaCommand.AppendLine(luaExpression);
            sbLuaCommand.AppendLine(" end");
             object[] results = null;
            //lxm_2017-08-30 添加锁，当程序刚启动时，如果同时调用检查余额，将会出现外部程序异常
            lock (_lockLua)
            {               
                LuaManager.Instance.ExecuteString(sbLuaCommand.ToString());
               results = LuaManager.Instance.CallFunction(functionName, lstParameterValues.ToArray<object>());
            }
            
            var minute = double.Parse(results[0].ToString());
            double hour = 0d;
            if (minute - roundFator <= 0)
            {
                hour = roundFator / 60.00000000000000;
            }
            else
            {
                hour = roundFator == 0 ? minute / 60.00000000000000 : ((minute + (minute % roundFator != 0 ? (roundFator - minute % roundFator) : 0)) / 60.00000000000000);
            }
            return double.Parse(hour.ToString(string.Format("0.{0}", "".PadRight(roundDigits, '0'))));
        }
        public bool ValidateCalcFeeTimeExpression(string functionName,string luaExpression, IEnumerable<string> parameterNames, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                StringBuilder sbLuaCommand = new StringBuilder();
                sbLuaCommand.AppendFormat("function {0}({1}) ", functionName,string.Join(",", parameterNames)).AppendLine();
                sbLuaCommand.AppendLine(luaExpression);
                sbLuaCommand.AppendLine(" end");
                return LuaManager.Instance.ExecuteString(sbLuaCommand.ToString());
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }      
    }
}
