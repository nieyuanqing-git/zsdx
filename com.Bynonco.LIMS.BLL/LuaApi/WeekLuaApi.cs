using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.LuaApi
{
    public class WeekLuaApi
    {
        [LuaFunction("GetWeekDayArabNo")]
        public string GetWeekDayArabNo(DateTime curDate)
        {
            return WeekDayUtility.GetWeekDayArabNo(curDate.DayOfWeek);
        }
        [LuaFunction("GetWeekDayEnName")]
        public string GetWeekDayEnName(DateTime curDate)
        {
            return WeekDayUtility.GetWeekDayEnName(curDate.DayOfWeek);
        }
        [LuaFunction("GetWeekDayCnName")]
        public string GetWeekDayCnName(DateTime curDate)
        {
            return WeekDayUtility.GetWeekDayCnName(curDate.DayOfWeek);
        }
        [LuaFunction("GetWeekDayCnNo")]
        public string GetWeekDayCnNo(DateTime curDate)
        {
            return WeekDayUtility.GetWeekDayCnNo(curDate.DayOfWeek);
        }



        [LuaFunction("GetCurWeekDayArabNo")]
        public string GetCurWeekDayArabNo()
        {
            return WeekDayUtility.GetWeekDayArabNo(DateTime.Now.DayOfWeek);
        }
        [LuaFunction("GetCurWeekDayEnName")]
        public string GetCurWeekDayEnName()
        {
            return WeekDayUtility.GetWeekDayEnName(DateTime.Now.DayOfWeek);
        }
        [LuaFunction("GetCurWeekDayCnName")]
        public string GetCurWeekDayCnName(DateTime curDate)
        {
            return WeekDayUtility.GetWeekDayCnName(DateTime.Now.DayOfWeek);
        }
        [LuaFunction("GetCurWeekDayCnNo")]
        public string GetCurWeekDayCnNo(DateTime curDate)
        {
            return WeekDayUtility.GetWeekDayCnNo(DateTime.Now.DayOfWeek);
        }
    }
}
