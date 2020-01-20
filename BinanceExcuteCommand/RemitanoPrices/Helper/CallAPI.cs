using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RemitanoPrices.Helper
{
    public class ResponseData
    {
        public int code { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    public class CallWebAPI
    {
        //private static string ApiUrlAppSetting = "API_URI";

        public string _accessToken { set; get; }
        public string _currentUserId { set; get; }

        private Dictionary<string, int> _cachedURLs = new Dictionary<string, int>();

        public CallWebAPI(string accessToken)
        {
            this._accessToken = accessToken;
            Init();
        }

        public CallWebAPI()
        {
            Init();
        }

        private void Init()
        {
            //_cachedURLs.Add("Account_GetPersonCommonInfo_", 4 * 60);
            //_cachedURLs.Add("Account_GetPersonCommonInfo",  4 * 60);
            //_cachedURLs.Add("Employee_GetProfile",          4 * 60);
            //_cachedURLs.Add("Employee_CurriculumVitae",     4 * 60);

            //_cachedURLs.Add("Account_GetAvatar", 24 * 60);
            ////_cachedURLs.Add("HomePage_GetLeftMenu", 24 * 60);
            //_cachedURLs.Add("HomePage_GetHomePageInfo", 24 * 60);
            //_cachedURLs.Add("Employee_GetProfile", 24 * 60);
            //_cachedURLs.Add("Employee_CurriculumVitae", 24 * 60);
            ////_cachedURLs.Add("Employee_GetProfileList", 24 * 60);
            //_cachedURLs.Add("Employee_GetAbsenceCommonInfo", 24 * 60);
            //_cachedURLs.Add("Employee_GetOverTimeInfo", 24 * 60);
            //_cachedURLs.Add("Employee_GetCTDTInfo", 24 * 60);
            //_cachedURLs.Add("Employee_GetInfoBaKyCongTruoc", 24 * 60);
            //_cachedURLs.Add("Employee_ProfileListViewMemberProfile", 24 * 60);
            //_cachedURLs.Add("Employee_ProfileListCurriculumVitaeByPerNo", 24 * 60);
        }

        private string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        private object GetFromCache(string url, string param = "")
        {
            //var temp = url.Split('?');
            //var baseUrl = temp[0].Replace(" ", "").Replace("/", "_");
            //if (_cachedURLs.ContainsKey(baseUrl))
            //{
            //    return RedisConnectorHelper.GetValue(url + "_" + RemoveSpecialCharacters(param));
            //}
            return null;
        }

        private object SetIntoCache(string url, string param, object value)
        {
            //var temp = url.Split('?');
            //var baseUrl = temp[0].Replace(" ", "").Replace("/", "_");
            //if (_cachedURLs.ContainsKey(baseUrl))
            //{
            //    int expireTime = _cachedURLs[baseUrl];
            //    return RedisConnectorHelper.SetValue(url + "_" + RemoveSpecialCharacters(param),
            //        value, expireTime);
            //}
            return null;
        }

        public string CallAPI(string url, JObject JObj)
        {
            // Check cache
            string retVal = "";
            //var resultFromCache = GetFromCache(url, JObj.ToString());

            //if (resultFromCache != null)
            //{
            //    retVal = resultFromCache.ToString();
            //    return retVal;
            //}

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(ConstantVarURL.BASEURL);
            ////client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpContent content = CreateHttpContent(JObj);
            try
            {
                var res = client.PostAsync(string.Format(url), content).Result;
                //response
                if (res.IsSuccessStatusCode)
                {
                    retVal = res.Content.ReadAsStringAsync().Result;
                    //SetIntoCache(url, JObj.ToString(), retVal);
                    LoggingHelper.LogInfo("Res message = " + retVal);
                }
                else
                {
                    LoggingHelper.LogException("Error message = " + res.ReasonPhrase);
                }
            }
            catch (Exception err)
            {
                LoggingHelper.LogException(err.ToString());
            }

            return retVal;
        }

        public HttpResponseMessage PostAPI(string url, JObject JObj)
        {
            HttpClient client = new HttpClient();
            ////client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CommonConstants.API_URI]);
            ////client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Send request to server
            LoggingHelper.LogDebug("PostAPI Sending POST " + client.BaseAddress + url + " == params = " + JObj.ToString());

            HttpContent content = CreateHttpContent(JObj);
            try
            {
                return client.PostAsync(string.Format(url), content).Result;

            }
            catch (Exception err)
            {
                LoggingHelper.LogException(err.ToString());
                return null;
            }
        }

        public Task<string> CallAPIGet(string url)
        {
            Task<string> _result = null;
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = new TimeSpan(0, 3, 0);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                LoggingHelper.LogDebug("CallAPIGet Sending GET" + client.BaseAddress + url + " == parans = ");

                //Send request to server
                var res = client.GetAsync(string.Format(url)).Result;

                //response
                if (res.IsSuccessStatusCode)
                {
                    _result = res.Content.ReadAsStringAsync();
                    //SetIntoCache(url, "", _result);
                    LoggingHelper.LogDebug("Res message = " + _result);
                }
                else
                {
                    LoggingHelper.LogDebug("Error message = " + res.ReasonPhrase);
                }
            }
            catch (Exception err)
            {
                LoggingHelper.LogException(err.ToString());
            }
            return _result;
        }

        public HttpContent CreateHttpContent(JObject obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public object RestApiGet(string url, Dictionary<string, string> dctParams = null)
        {

            ResponseData obj = new ResponseData();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CommonConstants.API_URI]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                BuildUrlParams(ref url, dctParams);

                var response = client.GetAsync(url).Result;
                LoggingHelper.LogDebug("RestApiGet Sending GET " + client.BaseAddress + url + " == parans = ");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    LoggingHelper.LogDebug("Res message = " + responseString);
                    obj = JsonConvert.DeserializeObject<ResponseData>(responseString);
                }
            }

            return obj;
        }
        public object RestApiGet(string url, Dictionary<string, string> dctParams, TimeSpan overrideTimeout)
        {

            ResponseData obj = new ResponseData();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CommonConstants.API_URI]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                LoggingHelper.LogDebug("RestApiGet List Sending GET " + client.BaseAddress + url + " == parans = ");

                if (overrideTimeout != null)
                {
                    client.Timeout = overrideTimeout;
                }

                BuildUrlParams(ref url, dctParams);

                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    obj = JsonConvert.DeserializeObject<ResponseData>(responseString);
                }
            }

            return obj;
        }
        public object RestApiPost(string url, object obj = null, TimeSpan? overrideTimeOut = null)
        {

            ResponseData objResponse = new ResponseData();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CommonConstants.API_URI]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                if (overrideTimeOut != null && overrideTimeOut.HasValue)
                {
                    client.Timeout = overrideTimeOut.Value;
                }

                var jsonRequest = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                LoggingHelper.LogDebug("RestApiPost Sending POST " + client.BaseAddress + url + " == parans = " + jsonRequest.ToString());

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseData>(responseString);
                }
            }

            return objResponse;
        }
        public object RestApiPost(string url, Dictionary<string, string> dctParams, object obj = null)
        {

            ResponseData objResponse = new ResponseData();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CommonConstants.API_URI]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                BuildUrlParams(ref url, dctParams);

                StringContent content = null;
                if (obj != null)
                {
                    var jsonRequest = JsonConvert.SerializeObject(obj);
                    content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
                }
                LoggingHelper.LogDebug("RestApiPost List Sending POST " + client.BaseAddress + url + " == parans = " + dctParams.ToString());

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseData>(responseString);
                }
            }

            return objResponse;
        }
        public object RestApiPut(string url, Dictionary<string, string> dctParams)
        {

            ResponseData objResponse = new ResponseData();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CommonConstants.API_URI]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                BuildUrlParams(ref url, dctParams);

                LoggingHelper.LogDebug("RestApiPost List Sending PUT " + client.BaseAddress + url + " == parans = " + dctParams.ToString());

                var response = client.PutAsync(url, null).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    objResponse = JsonConvert.DeserializeObject<ResponseData>(responseString);
                }
            }

            return objResponse;
        }
        private static void BuildUrlParams(ref string url, Dictionary<string, string> dctParams)
        {
            int index = 0;
            if (dctParams != null && dctParams.Count > 0)
            {
                foreach (var item in dctParams)
                {
                    if (index == 0)
                    {
                        url += "?" + item.Key + "=" + item.Value;
                    }
                    else
                    {
                        url += "&" + item.Key + "=" + item.Value;
                    }
                    index += 1;
                }
            }
        }

        public T RestApiGet<T>(string apiUrl, Dictionary<string, string> dicCriterial = null)
        {
            var obj = (ResponseData)RestApiGet(apiUrl, dicCriterial);
            T result = default(T);

            if (obj.code == HttpStatusCode.OK.GetHashCode() && obj.data != null)
            {
                result = JsonConvert.DeserializeObject<T>(Convert.ToString(obj.data));
            }

            return result;
        }
    }
    public enum SortOrderEnum
    {
        ASC = 0,
        DESC = 1
    }
}
