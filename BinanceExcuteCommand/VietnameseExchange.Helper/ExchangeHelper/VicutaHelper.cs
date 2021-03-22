using Newtonsoft.Json;
using Remibit.Utility.Helper;
using Remibit.Utility.Redis;
using RemibitPrices.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace VietnameseExchange.Helper
{
    public class VicutaHelper
    {
        CallWebAPI api = new CallWebAPI();
        private RedisHelper redis;
        CancellationTokenSource cts;

        public VicutaHelper()
        {
            redis = new RedisHelper();
        }

        public async Task<List<VicutaAsset>> getCoinPrice()
        {
            var strResponse = await api.CallAPIGet(ConstantVarURL.VICUTA_MARKET_URL);
            return JsonConvert.DeserializeObject<List<VicutaAsset>>(strResponse);
        }
        public async Task PriceOnTime(int time)
        {
            try
            {
                cts = new CancellationTokenSource();
                cts.CancelAfter(3800);
                var data = await getCoinPrice();
                var listLaygia = new List<string>() { "usdt", "btc", "eth", "bch", "ltc", "ada", "bnb"};
                redis.SaveContentWithUnixtime<List<VicutaAsset>>("Vicuta:" + "BTC" + "VNDRate", data.Where(x => listLaygia.Contains(x.assetName.ToLower())).ToList(), time);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogException(ex.ToString());
                Console.WriteLine("khong lay duoc vicuta" + ex.ToString());
            }
        }
    }
}
