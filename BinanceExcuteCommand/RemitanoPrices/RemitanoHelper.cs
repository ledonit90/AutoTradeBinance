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
        public Offers GetCoinOffers(RequestOffers rq)
        {
            var queryString = ParameterHelper.ObjectToQueryString(rq);
            var res = api.CallAPIGet(queryString);

            if (!string.IsNullOrWhiteSpace(res)) return JsonConvert.DeserializeObject<Offers>(res);
            return null;
        }


        public Task<CoinPrice> getCoinPrice(string coin)
        {
            RequestOffers rq = new RequestOffers(coin);
            var strRq = ParameterHelper.ObjectToQueryString(rq);

        }

        public Task<double> AveragePrice()
        {

        }
        #endregion
    }
}
