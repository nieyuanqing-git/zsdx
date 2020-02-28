using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL.LuaApi
{
    public class DateTimeLuaApi
    {
        [LuaFunction("ToDateTime")]
        public DateTime ToDateTime(string dateTimeStr)
        {
            return Convert.ToDateTime(dateTimeStr);
        }
        [LuaFunction("GetDateStr")]
        public string GetDateStr(DateTime curDate)
        {
            return curDate.ToString("yyyy-MM-dd");
        }
        [LuaFunction("GetTimeStr")]
        public string GetTimeStr(DateTime curDate)
        {
            return curDate.ToString("HH:mm:ss");
        }
        [LuaFunction("GetYear")]
        public int GetYear(DateTime curDate)
        {
            return curDate.Year;
        }
        [LuaFunction("GetMonth")]
        public int GetMonth(DateTime curDate)
        {
            return curDate.Month;
        }
        [LuaFunction("GetDay")]
        public int GetDay(DateTime curDate)
        {
            return curDate.Day;
        }
        [LuaFunction("GetHour")]
        public int GetHour(DateTime curDate)
        {
            return curDate.Hour;
        }
        [LuaFunction("GetMinute")]
        public int GetMinute(DateTime curDate)
        {
            return curDate.Minute;
        }
        [LuaFunction("GetSecond")]
        public int GetSecond(DateTime curDate)
        {
            return curDate.Second;
        }
        [LuaFunction("GetMillionSencond")]
        public int GetMillionSencond(DateTime curDate)
        {
            return curDate.Millisecond;
        }
        [LuaFunction("IsDatetTimeEual")]
        public bool IsDatetTimeEual(DateTime dt1,DateTime dt2)
        {
            return dt2.Equals(dt1);
        }
        [LuaFunction("IsDateTimeLarge")]
        public bool IsDateTimeLarge(DateTime dt1, DateTime dt2)
        {
            return dt1 > dt2;
        }
        [LuaFunction("IsDatetTimeLargeEqual")]
        public bool IsDatetTimeLargeEqual(DateTime dt1, DateTime dt2)
        {
            return dt1 >= dt2;
        }
        [LuaFunction("IsDatetTimeLittle")]
        public bool IsDatetTimeLittle(DateTime dt1, DateTime dt2)
        {
            return dt1 < dt2;
        }
        [LuaFunction("IsDatetTimeLittleEqual")]
        public bool IsDatetTimeLittleEqual(DateTime dt1, DateTime dt2)
        {
            return dt1 <= dt2;
        }
        [LuaFunction("IsTimeLarge")]
        public bool IsTimeLarge(string tm1,string tm2)
        {
            var now = DateTime.Now.Date;
            var dt1 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm1));
            var dt2 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm2));
            return dt1 > dt2;
        }
        [LuaFunction("IsTimeEqual")]
        public bool IsTimeEqual(string tm1, string tm2)
        {
            var now = DateTime.Now.Date;
            var dt1 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm1));
            var dt2 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm2));
            return dt1 == dt2;
        }
        [LuaFunction("IsTimeLargeEqual")]
        public bool IsTimeLargeEqual(string tm1, string tm2)
        {
            var now = DateTime.Now.Date;
            var dt1 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm1));
            var dt2 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm2));
            return dt1 >= dt2;
        }
        [LuaFunction("IsTimeLittleEqual")]
        public bool IsTimeLittleEqual(string tm1, string tm2)
        {
            var now = DateTime.Now.Date;
            var dt1 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm1));
            var dt2 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm2));
            return dt1 <= dt2;
        }
        [LuaFunction("IsTimeLittle")]
        public bool IsTimeLittle(string tm1, string tm2)
        {
            var now = DateTime.Now.Date;
            var dt1 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm1));
            var dt2 = Convert.ToDateTime(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), tm2));
            return dt1 < dt2;
        }
        [LuaFunction("Now")]
        public DateTime Now()
        {
            return DateTime.Now;
        }
        [LuaFunction("GetDateTimeDayDiff")]
        public double GetDateTimeDayDiff(DateTime dt1, DateTime dt2)
        {
            return (dt2 - dt1).TotalDays;
        }
        [LuaFunction("GetDateTimeMuniteDiff")]
        public double GetDateTimeMuniteDiff(DateTime dt1, DateTime dt2)
        {
            return (dt2 - dt1).TotalMinutes;
        }
        [LuaFunction("GetDatetTimeSecondDiff")]
        public double GetDatetTimeSecondDiff(DateTime dt1, DateTime dt2)
        {
            return (dt2 - dt1).TotalSeconds;
        }
        [LuaFunction("GetDateTimeMillisecondsDiff")]
        public double GetDateTimeMillisecondsDiff(DateTime dt1, DateTime dt2)
        {
            return (dt2 - dt1).TotalMilliseconds;
        }
    }
}
