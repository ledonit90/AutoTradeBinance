using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Models.Market.TradingRules
{
    public class Symbol
    {
        [JsonProperty("symbol")]
        public string symbol { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("baseAsset")]
        public string baseAsset { get; set; }
        [JsonProperty("baseAssetPrecision")]
        public int baseAssetPrecision { get; set; }
        [JsonProperty("quoteAsset")]
        public string quoteAsset { get; set; }
        [JsonProperty("quotePrecision")]
        public int quotePrecision { get; set; }
        [JsonProperty("quoteAssetPrecision")]
        public int quoteAssetPrecision { get; set; }
        [JsonProperty("baseCommissionPrecision")]
        public int baseCommissionPrecision { get; set; }
        [JsonProperty("quoteCommissionPrecision")]
        public int quoteCommissionPrecision { get; set; }
        [JsonProperty("orderTypes")]
        public List<string> orderTypes { get; set; }
        [JsonProperty("icebergAllowed")]
        public bool icebergAllowed { get; set; }
        [JsonProperty("ocoAllowed")]
        public bool ocoAllowed { get; set; }
        [JsonProperty("quoteOrderQtyMarketAllowed")]
        public bool quoteOrderQtyMarketAllowed { get; set; }
        [JsonProperty("isSpotTradingAllowed")]
        public bool isSpotTradingAllowed { get; set; }
        [JsonProperty("isMarginTradingAllowed")]
        public bool isMarginTradingAllowed { get; set; }
        [JsonProperty("filters")]
        public List<Filter> filters { get; set; }
        [JsonProperty("permissions")]
        public List<string> permissions { get; set; }
    }
}
