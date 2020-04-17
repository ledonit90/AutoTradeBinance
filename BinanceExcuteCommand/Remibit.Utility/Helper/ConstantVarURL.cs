using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Remibit.Utility.Helper
{
    public class ConstantVarURL
    {

        public const string REMI_BASE_URL = "https://remitano.com/api/v1/offers";
        public const string REMI_API_ETH = "";
        public const string REMI_RATE_EXCHANGE = "https://remitano.com/api/v1/rates/exchange";
        public const string REMI_API_BTC = "";
        public const string REMI_API_USDT = "";

        public class RequestHeader
        {
            public const string JSON = "application/json";

            public const string TEXT = "html\text";
        }
    }

    public class TradingPairs
    {
        public const string BTC_ETH = "BTC_ETH";

        public const string BTC_BNB = "BTC_BNB";
    }
}
