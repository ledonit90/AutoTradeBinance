using Newtonsoft.Json;
using Remibit.Utility.Helper;
using System.Threading.Tasks;
using Remibit.Models.Remitano;
using System.Linq;
using Remibit.Utility.Redis;
using System;

namespace VietnameseExchange.Helper
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
        public async Task<Offers> getCoinPrice(string coin,bool isSell)
        {
            RequestOffers rq = new RequestOffers(coin);
            rq.offer_type = isSell ? "sell" : "buy";
            var strRq = ConstantVarURL.REMI_BASE_URL + "?" + ParameterHelper.ObjectToQueryString(rq);
            var strResponse = await api.CallAPIGet(strRq);
            return JsonConvert.DeserializeObject<Offers>(strResponse);
        }
        // Save to Redis Database
        public async Task PriceOnTime(string coin, int time)
        {
            try
            {
                var resultSell = await getCoinPrice(coin, true);
                var resultBuy = await getCoinPrice(coin, false);
                var AVGWeight = (resultSell.offers.Sum(x => x.max_amount) + resultBuy.offers.Sum(x => x.max_amount)) / 10;
                var totalWeight = resultSell.offers.Sum(x => x.max_amount >= AVGWeight ? AVGWeight : x.max_amount) + resultBuy.offers.Sum(x => x.max_amount >= AVGWeight ? AVGWeight : x.max_amount);
                double VNDRate = (
                    resultSell.offers.Sum(x => x.price * (x.max_amount >= AVGWeight ? AVGWeight : x.max_amount)) +
                    resultBuy.offers.Sum(x => x.price * (x.max_amount >= AVGWeight ? AVGWeight : x.max_amount))
                    )
                    / totalWeight;
                // luu vao redis
                redis.SaveContentWithUnixtime<CoinPrice>("Remitano:" + coin.ToUpper() + "VNDRate", VNDRate, time);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Loi " + coin + ":" + ex.ToString());
            }
        }
        #endregion
    }
}
