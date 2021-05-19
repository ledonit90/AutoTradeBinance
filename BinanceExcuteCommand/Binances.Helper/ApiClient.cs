using Binances.Helper.Domain.Abstract;
using Binances.Helper.Domain.Interface;
using Binances.Helper.Models.Enums;
using Binances.Helper.Models.WebSocket;
using Binances.Helper.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceStack.Caching;
using Binances.Helper.Models.General;
using ServiceStack;

namespace Binances.Helper
{
    public class ApiClient : ApiClientAbstract, IApiClient
    {
        public ICacheClient CacheClient { get; set; }
        public string serverTimeKey = "serverTimeCached";

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="apiKey">Key used to authenticate within the API.</param>
        /// <param name="apiSecret">API secret used to signed API calls.</param>
        /// <param name="apiUrl">API base url.</param>
        public ApiClient(string apiKey, string apiSecret, string apiUrl = @"https://www.binance.com", string webSocketEndpoint = @"wss://stream.binance.com:9443/stream?streams=", bool addDefaultHeaders = true) : base(apiKey, apiSecret, apiUrl, webSocketEndpoint, addDefaultHeaders)
        {
            CacheClient = HostContext.TryResolve<ICacheClient>();
        }

        #region Basic Info
        public async Task<ServerInfo> GetServerTime()
        {
            var result = await CallAsync<ServerInfo>(ApiMethod.GET, EndPoints.CheckServerTime, false);

            return result;
        }
        public async Task<long> GetServerTimeWithCached()
        {
            var Cachedtime = CacheClient.Get<long>(serverTimeKey);
            var unixTimeNow = Utilities.GenerateTimeStamp(DateTime.Now.ToUniversalTime());
            if (Cachedtime == default || Cachedtime <= 0)
            {
                var serverTime = (await GetServerTime()).ServerTime;
                CacheClient.Set(serverTimeKey, unixTimeNow - serverTime);
                return serverTime;
            }

            return unixTimeNow - Cachedtime;
        }
        #endregion

        /// <summary>
        /// Calls API Methods.
        /// </summary>
        /// <typeparam name="T">Type to which the response content will be converted.</typeparam>
        /// <param name="method">HTTPMethod (POST-GET-PUT-DELETE)</param>
        /// <param name="endpoint">Url endpoing.</param>
        /// <param name="isSigned">Specifies if the request needs a signature.</param>
        /// <param name="parameters">Request parameters.</param>
        /// <returns></returns>
        public async Task<T> CallAsync<T>(ApiMethod method, string endpoint, bool isSigned = false, string parameters = null)
        {
            var finalEndpoint = endpoint + (string.IsNullOrWhiteSpace(parameters) ? "" : $"?{parameters}");

            if (isSigned)
            {
                // Joining provided parameters
                parameters += (!string.IsNullOrWhiteSpace(parameters) ? "&timestamp=" : "timestamp=") + (await GetServerTimeWithCached()).ToString();

                // Creating request signature
                var signature = Utilities.GenerateSignature(_apiSecret, parameters);
                finalEndpoint = $"{endpoint}?{parameters}&signature={signature}";
            }

            var request = new HttpRequestMessage(Utilities.CreateHttpMethod(method.ToString()), finalEndpoint);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                // Api return is OK
                response.EnsureSuccessStatusCode();

                // Get the result
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // Serialize and return result
                return JsonConvert.DeserializeObject<T>(result);
            }

            // We received an error
            if (response.StatusCode == HttpStatusCode.GatewayTimeout)
            {
                throw new Exception("Api Request Timeout.");
            }

            // Get te error code and message
            var e = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Error Values
            var eCode = 0;
            string eMsg = "";
            if (e.IsValidJson())
            {
                try
                {
                    var i = JObject.Parse(e);

                    eCode = i["code"]?.Value<int>() ?? 0;
                    eMsg = i["msg"]?.Value<string>();
                }
                catch { }
            }

            throw new Exception(string.Format("Api Error Code: {0} Message: {1}", eCode, eMsg));
        }

        /// <summary>
        /// Connects to a Websocket endpoint.
        /// </summary>
        /// <typeparam name="T">Type used to parsed the response message.</typeparam>
        /// <param name="parameters">Paremeters to send to the Websocket.</param>
        /// <param name="messageDelegate">Deletage to callback after receive a message.</param>
        /// <param name="useCustomParser">Specifies if needs to use a custom parser for the response message.</param>
        public void ConnectToWebSocket<T>(string parameters, MessageHandler<T> messageHandler, bool useCustomParser = false)
        {
            var finalEndpoint = _webSocketEndpoint + parameters;

            var ws = new WebSocket(finalEndpoint);

            ws.OnMessage += (sender, e) =>
            {
                dynamic eventData;

                if (useCustomParser)
                {
                    var customParser = new CustomParser();
                    eventData = customParser.GetParsedDepthMessage(JsonConvert.DeserializeObject<dynamic>(e.Data));
                }
                else
                {
                    eventData = JsonConvert.DeserializeObject<T>(e.Data);
                }

                int t = messageHandler(eventData);
                if (t == 10) ws.CloseAsync();
            };

            ws.OnClose += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.OnError += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.Connect();
            _openSockets.Add(ws);
        }

        /// <summary>
        /// Connects to a UserData Websocket endpoint.
        /// </summary>
        /// <param name="parameters">Paremeters to send to the Websocket.</param>
        /// <param name="accountHandler">Deletage to callback after receive a account info message.</param>
        /// <param name="tradeHandler">Deletage to callback after receive a trade message.</param>
        /// <param name="orderHandler">Deletage to callback after receive a order message.</param>
        public void ConnectToUserDataWebSocket(string parameters, MessageHandler<StreamMessage<AccountUpdatedMessage>> accountInfoHandler, MessageHandler<StreamMessage<OrderOrTradeUpdatedMessage>> tradesHandler, MessageHandler<StreamMessage<OrderOrTradeUpdatedMessage>> ordersHandler)
        {
            var finalEndpoint = _webSocketEndpoint + parameters;

            var ws = new WebSocket(finalEndpoint);

            ws.OnMessage += async (sender, e) =>
            {
                var eventData = JsonConvert.DeserializeObject<StreamMessage<dynamic>>(e.Data);
                int t = 0;
                JValue excuteType = (JValue)eventData.data["e"];
                switch (excuteType.ToString())
                {
                    case "outboundAccountPosition":
                        t = await accountInfoHandler(JsonConvert.DeserializeObject<StreamMessage<AccountUpdatedMessage>>(e.Data));
                        break;
                    case "executionReport":
                        var message = JsonConvert.DeserializeObject<StreamMessage<OrderOrTradeUpdatedMessage>>(e.Data);
                        var isTrade = message.data.ExecutionType.ToLower() == "trade";

                        if (isTrade)
                        {
                            t = await tradesHandler(message);
                        }
                        else
                        {
                            t = await ordersHandler(message);
                        }
                        break;
                    case "balanceUpdate":
                        Console.WriteLine("test ti thoi");
                        break;
                }

                if (t == 10) ws.CloseAsync();
            };

            ws.OnClose += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.OnError += (sender, e) =>
            {
                _openSockets.Remove(ws);
            };

            ws.Connect();
            _openSockets.Add(ws);
        }
    }
}
