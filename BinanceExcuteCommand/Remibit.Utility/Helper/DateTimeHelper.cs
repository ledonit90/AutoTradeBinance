using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Remibit.Utility.Helper
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

        public static int CurrentUnixTimeStamp()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
