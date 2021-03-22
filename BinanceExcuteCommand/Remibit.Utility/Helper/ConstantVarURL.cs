using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Remibit.Utility.Helper
{
    public class ConstantVarURL
    {

        public const string REMI_BASE_URL = "https://remitano.com/api/v1/offers";
        public const string VCCEX_BASE_URL = "https://vcc.exchange/api/v2/";
        public const string VICUTA_MARKET_URL = "https://vicuta.com/api/getMarket";
        public const string REMI_API_ETH = "";
        public const string REMI_RATE_EXCHANGE = "https://remitano.com/api/v1/rates/exchange";
        public const string REMI_API_BTC = "";
        public const string REMI_API_USDT = "";
        public static List<string> Remitano_Listcoin = new List<string> { "btc", "eth", "usdt", "bch", "ltc", "xrp" };
        public static List<string> VccTradingPairs = new List<string> { "BTC_VND", "eth_vnd", "usdt_vnd" };

        public class RequestHeader
        {
            public const string JSON = "application/json";

            public const string TEXT = "html\text";
        }
    }

    public class VCC_Trade_Pairs
    {
        public const string BTC_VND = "BTC_VND";
        public const string ETH_VND = "eth_vnd";
        public const string USDT_VND = "usdt_vnd";
    }
    public class VCC_Section
    {
        public const string trades = "trades"; // History Trade
        public const string orderbook = "orderbook"; // List Orders
        public const string summary = "summary"; // Summary
        public const string assets = "assets"; // assets
    }
}
