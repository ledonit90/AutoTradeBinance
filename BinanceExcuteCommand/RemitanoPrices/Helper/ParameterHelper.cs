using System;
using System.Linq;
using System.Web;

namespace Remitano.Helper.Helper
{
    public class ParameterHelper
    {
        public static string ObjectToQueryString(object T)
        {
            var properties = from p in T.GetType().GetProperties()
                             where p.GetValue(T, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(T, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
