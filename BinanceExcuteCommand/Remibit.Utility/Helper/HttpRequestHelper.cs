using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Net.Http;
namespace Remibit.Utility.Helper
{
    public class HttpRequestHelper
    {
        public string url { get; set; }
        public  Dictionary<string, string> lsHeader { get; set; }
        public  Dictionary<string, string> dic_querystring { get; set; }
        public  object obj_querystring { get; set; }
        public  string str_querystring { get; set; }
        public HttpClient iweb { get; set; }

        public HttpClient Builder()
        {
            AddHeader();
            return iweb;
        }

        private void AddHeader()
        {
            if(lsHeader != null && lsHeader.Count()!=0)
            {
                foreach(var t in lsHeader)
                {
                    iweb.DefaultRequestHeaders.Add(t.Key, t.Value);
                }
            }
        }
        private void UrlBuilder()
        {
            if(!string.IsNullOrEmpty(str_querystring))
            {
                url += str_querystring;
            }else if(obj_querystring != null)
            {
                url += ParameterHelper.ObjectToQueryString(obj_querystring);
            } else if(dic_querystring != null && dic_querystring.Count() != 0)
            {
                url += ParameterHelper.ObjectToQueryString(obj_querystring);
            }

        }
    }
}
