using Newtonsoft.Json;
using Remibit.Models.Remitano;
using Remibit.Utility.Helper;
using Remibit.Utility.Redis;
using RemibitPrices.Models.VCCEx;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace VietnameseExchange.Helper
{
    public class VCCHelper
    {
        CallWebAPI api = new CallWebAPI();
        private RedisHelper redis;
        public VCCHelper()
        {
            redis = new RedisHelper();
        }
        #region Calculator Price
        public async Task<T> getCoinPrice<T>(string section, string tradepare, string queryString = "")
        {
            var strRq = ConstantVarURL.VCCEX_BASE_URL + section + "/" + tradepare ;
            strRq = string.IsNullOrEmpty(queryString) ? strRq : strRq + "?" + queryString;
            var strResponse = await api.CallAPIGet(strRq);
            return JsonConvert.DeserializeObject<T>(strResponse);
        }
        // Save to Redis Database
        public async Task PriceOnTime(string tradePair, int time)
        {
            try
            {
                var askBids = await getCoinPrice<OfferLevel1>(VCC_Section.orderbook, tradePair, "level=1");
                var historys = await getCoinPrice<VccTradeHistory>(VCC_Section.trades, tradePair, "count=4");
                double TB1 = (double.Parse(askBids.data.asks[0][0]) + double.Parse(askBids.data.bids[0][0])) / 2;
                var buyList = historys.data.Where(x=>x.type == "buy").Take(4);
                var sellList = historys.data.Where(x=>x.type == "sell").Take(4);
                double buyPrice = buyList.Count() > 0 ? buyList.Sum(x => double.Parse(x.price))/ buyList.Count() : 0;
                double sellPrice = sellList.Count() > 0 ? sellList.Sum(x => double.Parse(x.price))/ sellList.Count() : 0;
                double TB2 = sellPrice >0 && buyPrice > 0 ? (buyPrice + sellList.Sum(x => double.Parse(x.price))/sellList.Count())/2 : (sellPrice > 0 ? sellPrice: buyPrice);
                // luu vao redis
                double VNDRate = TB2*0.4 + TB1*0.6;
                redis.SaveContentWithUnixtime<CoinPrice>("VccExchange:" + tradePair + "VNDRate", VNDRate, time);
            }catch(Exception ex)
            {
                Console.WriteLine("khong lay duoc vcc excchange");
            }
        }

        #endregion
    }
}