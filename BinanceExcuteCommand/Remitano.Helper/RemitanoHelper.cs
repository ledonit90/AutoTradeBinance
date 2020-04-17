using Newtonsoft.Json;
using Remibit.Utility.Helper;
using StackExchange.Redis;
using System.Threading.Tasks;
using Remibit.Models.Remitano;
using System.Collections.Generic;

namespace Remibit.Utility.Redis
{
    public class RemitanoHelper
    {
        CallWebAPI api = new CallWebAPI();
        private RedisHelper redis;
        #region Calculator Price
        public RemitanoHelper()
        {
            redis = new RedisHelper();
        }
        public async Task<Offers> GetCoinOffersAsync(RequestOffers rq)
        {
            var queryString = ParameterHelper.ObjectToQueryString(rq);
            var res = await api.CallAPIGet(queryString);

            if (!string.IsNullOrWhiteSpace(res)) return JsonConvert.DeserializeObject<Offers>(res);
            return null;
        }
        public async Task<Offers> getCoinPrice(string coin,bool isSell)
        {
            RequestOffers rq = new RequestOffers(coin);
            rq.offer_type = isSell ? "sell" : "buy";
            var strRq = ParameterHelper.ObjectToQueryString(rq);
            var strResponse = await api.CallAPIGet(strRq);
            return JsonConvert.DeserializeObject<Offers>(strResponse);
        }
        // Save to Redis Database
        public async Task PriceOnTime(string coin, int time)
        {
            var resultSell = await getCoinPrice(coin, true);
            var resultBuy = await getCoinPrice(coin, true);
            double VNDRate = (resultSell.offers[0].price + resultBuy.offers[0].price + resultSell.offers[1].price + resultBuy.offers[1].price) / 4;
            // luu vao redis
            redis.SaveContentWithUnixtime<CoinPrice>("Remitano:" + coin.ToUpper() + "VNDRate", VNDRate, time);
        }
        #endregion
    }
}
