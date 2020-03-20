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
        public HttpClient iweb { get; set; }


        public void AddHeader()
        {
            if(lsHeader != null && lsHeader.Count()!=0)
            {
                foreach(var t in lsHeader)
                {
                    iweb.DefaultRequestHeaders.Add(t.Key, t.Value);
                }
            }
        }
    }
}
