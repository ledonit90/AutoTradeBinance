using Newtonsoft.Json;
using Remitano.Helper.Helper;
using StackExchange.Redis;
using System.Threading.Tasks;
using Remibit.Models.Remitano;

namespace Remibit.Utility.Redis
{
    public class RemitanoHelper
    {
        CallWebAPI api = new CallWebAPI();
        private IDatabase db;
        #region Calculator Price
        public RemitanoHelper()
        {
            db = RedisConnectorHelper.RedisCache(2);
        }
        public async Task<Offers> GetCoinOffersAsync(RequestOffers rq)
        {
            var queryString = ParameterHelper.ObjectToQueryString(rq);
            var res = await api.CallAPIGet(queryString);

            if (!string.IsNullOrWhiteSpace(res)) return JsonConvert.DeserializeObject<Offers>(res);
            return null;
        }
        public async Task<CoinPrice> getCoinPrice(string coin,bool isSell)
        {
            RequestOffers rq = new RequestOffers(coin);
            rq.offer_type = isSell ? "sell" : "buy";
            var strRq = ParameterHelper.ObjectToQueryString(rq);
            var strResponse = await api.CallAPIGet(strRq);
            return JsonConvert.DeserializeObject<CoinPrice>(strResponse);
        }
        // Save to Redis Database
        public async Task PriceOnTime(string coin, int time)
        {
            var taskSell = getCoinPrice(coin, true);
            var taskBuy = getCoinPrice(coin, false);
            await Task.WhenAll(taskSell, taskBuy);
            // xu ly ket qua

            // luu vao csdl
            var strCurMessage1m = JsonConvert.SerializeObject(taskSell.Result);
            HashEntry[] redisBookHash =
            {
                new HashEntry(time, strCurMessage1m)
            };
            db.HashSet(time.ToString(), redisBookHash);
        }
        #endregion
    }
}
