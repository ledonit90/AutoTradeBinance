using Newtonsoft.Json;
using RemitanoPrices.Helper;
using RemitanoPrices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemitanoPrices
{
    public class RemitanoHelper
    {
        CallWebAPI api = new CallWebAPI();
        #region Calculator Price
        public async Task<Offers> GetCoinOffersAsync(RequestOffers rq)
        {
            var queryString = ParameterHelper.ObjectToQueryString(rq);
            var res = await api.CallAPIGet(queryString);

            if (!string.IsNullOrWhiteSpace(res)) return JsonConvert.DeserializeObject<Offers>(res);
            return null;
        }


        public async Task<CoinPrice> getCoinSellPrice(string coin)
        {
            RequestOffers rq = new RequestOffers(coin);
            var strRq = ParameterHelper.ObjectToQueryString(rq);
            var strResponse = await api.CallAPIGet(strRq);
            return JsonConvert.DeserializeObject<CoinPrice>(strResponse);
        }
        
        public async Task<CoinPrice> 
        #endregion
    }
}
