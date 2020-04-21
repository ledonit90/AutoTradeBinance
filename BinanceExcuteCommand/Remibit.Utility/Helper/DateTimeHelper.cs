using System;
using System.Timers;

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

        public static Timer getATimer(ElapsedEventHandler a,double interval, string coin)
        {
            CustomTimer aTimer = new CustomTimer()
            {
                Interval = 3000,
                Data = coin
            };
            ;
            aTimer.Elapsed += a;
            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;

            return aTimer;
        }
    }

    public class CustomTimer : Timer
    {
        public string Data;
    }
}
