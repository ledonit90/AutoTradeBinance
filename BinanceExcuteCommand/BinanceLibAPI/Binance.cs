using System;
using System.Threading.Tasks;
using System.Net.Http;
using BinanceLibAPI.Models;
using Newtonsoft.Json;
using System.Linq;

namespace BinanceLibAPI
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
            var result = hasher.HmacSha256(apiSecretKey, RequestBody);
            return result;
        }

        public async Task<bool> buyTokenAsync(RequestParameter rq)
        {
            string requestURL = await RequestURLAsync(rq.symbol, rq.type, rq.timeInForce, rq.quantity, rq.price, rq.recvWindow);
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), requestURL))
                {
                    request.Headers.TryAddWithoutValidation("X-MBX-APIKEY", this.apiKey);

                    var response = await httpClient.SendAsync(request);
                    var resultInfo = response.Content.ReadAsStringAsync().Result;
                }
            }
            return true;
        }

        public async Task<bool> SellTokenAsync(string symbol, string type, string timeInForce, double quantity, double price, int recvWindow = 2)
        {
            string requestURL = await RequestURLAsync(symbol, type, timeInForce, quantity, price, recvWindow);
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

        public async Task<string> RequestURLAsync(string symbol, string type, string timeInForce, double quantity, double price, int recvWindow = 2)
        {
            string url = "";
            var tempURL = "https://api.binance.com/api/v3/order?";
            DateTime now = DateTime.UtcNow;
            var unixTime = await GetServerTimeAsync();
            string signature = "";
            string MessageParameter = "symbol=" + symbol + "&side=BUY&type=" + type + "&timeInForce=GTC&quantity=" + quantity;
            MessageParameter += "&price=" + price + "&recvWindow=" + recvWindow + "&timestamp=" + unixTime;
            //get signature
            signature = GenerateSignature(MessageParameter);
            url = tempURL + MessageParameter + "&signature=" + signature;
            return url;
        }

        public async Task<long> GetServerTimeAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), LinkConfig.serverTime))
                {
                    var response = await httpClient.SendAsync(request);
                    var exchangeInfo = response.Content.ReadAsStringAsync().Result;
                    // chang can phai lam gi ca
                    var exInfoObject = JsonConvert.DeserializeObject<serverTimes>(exchangeInfo);
                    return exInfoObject.serverTime;
                }
            }
        }

        public async Task<AccountInfo> AccountInfo()
        {
            var responseServerTime  = await GetServerTimeAsync();
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
            var responseServerTime = await GetServerTimeAsync();
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
