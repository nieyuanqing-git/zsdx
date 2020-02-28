using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPCalcFeeTimeRuleBLL : BLLBase<NMPCalcFeeTimeRule>, INMPCalcFeeTimeRuleBLL
    {
         private ICalcFeeTimeRuleRalativeBLL __calcFeeTimeRuleRalativeBLL;
         private const string __FUNCTIONNAME = "calNMPTimeRule";
         public NMPCalcFeeTimeRuleBLL()
        {
            __calcFeeTimeRuleRalativeBLL = new CalcFeeTimeRuleRalativeBLL();
        }
        public double CalcFeeTime(bool isDurationPrice, Guid nmpId, Guid? userId, DateTime? usedBeginTime, DateTime? usedEndTime, DateTime? appointmentBeginTime, DateTime? appointmentEndTime, string luaExpression, int roundFator = 0, int roundDigits = 2)
        {
            IDictionary<string, object> ddParameters = new Dictionary<string, object>();
            ddParameters["usedBeginTime"] = usedBeginTime;
            ddParameters["usedEndTime"] = usedEndTime;
            ddParameters["appointmentBeginTime"] = appointmentBeginTime;
            ddParameters["appointmentEndTime"] = appointmentEndTime;
            ddParameters["isDurationPrice"] = isDurationPrice;
            ddParameters["userId"] = userId.HasValue ? userId.Value.ToString() : null;
            ddParameters["nmpId"] = nmpId.ToString();
            return __calcFeeTimeRuleRalativeBLL.CalcFeeTime(__FUNCTIONNAME, ddParameters, luaExpression, roundFator, roundDigits);
        }

        public bool ValidateCalcFeeTimeExpression(string luaExpression, out string errorMessage)
        {
            errorMessage = "";
            return __calcFeeTimeRuleRalativeBLL.ValidateCalcFeeTimeExpression(__FUNCTIONNAME, luaExpression, new string[] { "t1", "t2", "t3", "t4", "appointmentBeginTime", "appointmentEndTime", "usedBeginTime", "usedEndTime", "isDurationPrice", "userId", "nmpId" }, out errorMessage);
        }
    }
}
