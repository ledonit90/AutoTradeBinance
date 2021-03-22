using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Remibit.Models.Binance;
using System.Text;
using System.Security.Cryptography;

namespace Binances.Helper
{
    public class Binance
    {
        private string apiKey = "";
        private string apiSecretKey = "";
        private string Header_APIKEY = "";
        private HashHelper hasher = new HashHelper();
        public Binance(string apiKey, string apiSecretKey, string Header_api = "X-MBX-APIKEY")
        {
            this.apiKey = apiKey;
            this.apiSecretKey = apiSecretKey;
            this.Header_APIKEY = Header_api;
        }

        public string GenerateSignature(string RequestBody)
        {
            var key = Encoding.UTF8.GetBytes(apiSecretKey);
            string stringHash;
            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(RequestBody));
                stringHash = BitConverter.ToString(hash).Replace("-", "");
            }

            return stringHash;
        }

        public async Task<bool> buyTokenAsync(RequestParameter rq)
        {
            string requestURL = await RequestURLAsync(rq.symbol, rq.timeInForce, rq.quantity, rq.price, rq.type, 5000);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(@"https://www.binance.com");

                using (var request = new HttpRequestMessage(HttpMethod.Post, requestURL))
                {
                    request.Headers.TryAddWithoutValidation("X-MBX-APIKEY", this.apiKey);
                    request.Headers.Accept
                    .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                }
            }
            return true;
        }

        public async Task<bool> SellTokenAsync(string symbol, string timeInForce, double quantity, double price, OrderType type = OrderType.LIMIT, int recvWindow = 2)
        {
            string requestURL = await RequestURLAsync(symbol,  timeInForce, quantity, price, type, recvWindow);
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), requestURL))
                {
                    request.Headers.TryAddWithoutValidation("X-MBX-APIKEY", this.apiKey);

                    var response = await httpClient.SendAsync(request);
                }
            }

            return true;
        }

        public async Task<string> RequestURLAsync(string symbol, string timeInForce, double quantity, double price, OrderType type = OrderType.LIMIT, int recvWindow = 1000)
        {
            var tempURL = LinkConfig.New_order;
            DateTime now = DateTime.UtcNow;
            var unixTime = GetServerTimeAsync(DateTime.Now.ToUniversalTime());
            string signature = "";
            string MessageParameter = $"?symbol={symbol.ToUpper()}&side=BUY&type={type}&quantity={quantity}"
                + (type == OrderType.LIMIT ? $"&timeInForce=GTC" : "")
                + (type == OrderType.LIMIT ? $"&price={price}" : "")
                + $"&recvWindow={recvWindow}";
            MessageParameter +=  "&timestamp=" + unixTime;
            //get signature
            signature = GenerateSignature(MessageParameter);
            string url = tempURL + MessageParameter + "&signature=" + signature;
            return url;
        }

        public string GetServerTimeAsync(DateTime baseDateTime)
        {
            var dtOffset = new DateTimeOffset(baseDateTime);
            return dtOffset.ToUnixTimeMilliseconds().ToString();
        }

        public async Task<AccountInfo> AccountInfo()
        {
            var responseServerTime  = GetServerTimeAsync(DateTime.Now.ToUniversalTime());
            string requestURL = "recvWindow=5000&timestamp="+responseServerTime;

            string signature = GenerateSignature(requestURL);
            requestURL += "&signature=" + signature;
            requestURL = LinkConfig.Account_information + "?" + requestURL;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), requestURL))
                {
                    request.Headers.TryAddWithoutValidation("X-MBX-APIKEY", this.apiKey);
                    var response = await httpClient.SendAsync(request);
                    var resultInfo = response.Content.ReadAsStringAsync().Result;
                    var accountInfo = JsonConvert.DeserializeObject<AccountInfo>(resultInfo);
                    return accountInfo;
                }
            }
        }

        public async Task<DepositeAddressResponse> getDepositeAddress(string Coins)
        {
            var responseServerTime = GetServerTimeAsync(DateTime.Now.ToUniversalTime());
            string requestURL = "asset="+ Coins + "&status=true&" + "recvWindow=5000&timestamp=" + responseServerTime;

            string signature = GenerateSignature(requestURL);
            requestURL += "&signature=" + signature;
            requestURL = LinkConfig.DepositAddress + "?" + requestURL;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), requestURL))
                {
                    request.Headers.TryAddWithoutValidation("X-MBX-APIKEY", this.apiKey);
                    var response = await httpClient.SendAsync(request);
                    var resultInfo = response.Content.ReadAsStringAsync().Result;
                    var accountInfo = JsonConvert.DeserializeObject<DepositeAddressResponse>(resultInfo);
                    accountInfo.Description = resultInfo;
                    return accountInfo;
                }
            }
        }

        //public async Task getDepositeAddressAsync()
        //{
        //    using (SanGiaoDichEntities db = new SanGiaoDichEntities())
        //    {
        //        var listCoins = db.Coins.Where(x=>x.Active == true).ToList();
        //        foreach (var coin in listCoins)
        //        {
        //            // ngay xua toi da tung
        //            var xxx = await getDepositeAddress(coin.Code);
        //            MyAddress address = new MyAddress();
        //            address.account = "ledonit90@gmail.com";
        //            address.San = "Binance";
        //            address.Coin = coin;
        //            address.Address = xxx.address;
        //            address.Description = xxx.Description;
        //            db.MyAddresses.Add(address);
        //            db.SaveChanges();
        //        }
        //    }
        //}

        public class serverTimes
        {
            public long serverTime { get; set; }
        }
    }
}
