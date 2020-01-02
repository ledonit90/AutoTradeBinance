using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemitanoPrices.Helper
{
    public class DateTimeHelper
    {
        public static string GetCurrentDateStr(string format)
        {
            return DateTime.Now.ToString(format).Replace("/", "");
        }

        public static string GetCurrentTimeStr(string format)
        {
            return DateTime.Now.ToString(format).Replace(":", "");
        }

        public static string GetCurrentDateTimeStr(string format)
        {
            return DateTime.Now.ToString(format).Replace("/", "");
        }
    }
}
