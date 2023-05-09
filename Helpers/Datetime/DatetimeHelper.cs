using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Datetime
{
    public static class DatetimeHelper
    {
        public static string FormatDatetimeInPST(this DateTime date)
        {
            TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            DateTime pstTime = TimeZoneInfo.ConvertTimeFromUtc(date, pstZone);
            return pstTime.ToString("MM/dd/yyyy HH:mm:ss");
        }
        public static string FormatDateInPST(this DateTime date)
        {
            TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            DateTime pstTime = TimeZoneInfo.ConvertTimeFromUtc(date, pstZone);
            return pstTime.ToString("MM/dd/yyyy");
        }
    }
}
