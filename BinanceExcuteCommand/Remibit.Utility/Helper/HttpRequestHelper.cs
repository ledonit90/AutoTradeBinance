using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Remibit.Utility.Helper
{
    public class HttpRequestHelper
    {

        private void AddHeader(HttpClient iweb, Dictionary<string, string> lsHeader = null)
        {
            if(lsHeader != null && lsHeader.Count()!=0)
            {
                foreach(var t in lsHeader)
                {
                    iweb.DefaultRequestHeaders.Add(t.Key, t.Value);
                }
            }
        }

        public async Task<T> GetTAsync<T>(string url,object uriResource = null,Dictionary<string, string> lsHeader = null)
        {
            HttpClient iweb = new HttpClient();
            url = Urlbuilder(url, uriResource);
            iweb.DefaultRequestHeaders.Clear();
            AddHeader(iweb,lsHeader);
            var httpRes= await iweb.GetAsync(url);
            T t = JsonConvert.DeserializeObject<T>(httpRes.Content.ReadAsStringAsync().Result);
            return t;
        }

        public async Task<T> PostTAsync<T>(string url, JObject req, Dictionary<string, string> lsHeader = null)
        {
            HttpClient iweb = new HttpClient();
            AddHeader(iweb, lsHeader);
            HttpContent content = CreateHttpContent(req);
            var httpRes = await iweb.PostAsync(url, content);
            T t = JsonConvert.DeserializeObject<T>(httpRes.Content.ReadAsStringAsync().Result);
            return t;
        }

        public HttpContent CreateHttpContent(JObject obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, ConstantVarURL.RequestHeader.JSON);
        }
        private string Urlbuilder(string url, object UriResource)
        {
            if(UriResource is string)
            {
                url += "?" + UriResource;
            }
            else 
            {
                url += ParameterHelper.ObjectToQueryString(UriResource);
            }
            return url;
        }
    }
}
