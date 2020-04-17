using System;
using System.ComponentModel;
using static Remibit.Utility.BunchOfEnum;

namespace Remibit.Utility
{
    public static class MyEnumExtensions
    {
        public static string GetDescription(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
    public class BunchOfEnum
    {
        public enum Remitano_coin
        {
            [Description("btc")]
            btc = 1,
            [Description("eth")]
            eth = 2,
            [Description("usdt")]
            usdt = 3,
            [Description("bch")]
            bch = 4,
            [Description("ltc")]
            ltc = 5,
            [Description("xrp")]
            xrp = 6

        }
    }
}
