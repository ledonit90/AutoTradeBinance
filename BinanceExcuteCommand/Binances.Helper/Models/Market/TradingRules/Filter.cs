using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Models.Market.TradingRules
{
    public class Filter
    {
        [JsonProperty("filterType")]
        public string filterType { get; set; }
        [JsonProperty("minPrice")]
        public decimal minPrice { get; set; }
        [JsonProperty("maxPrice")]
        public decimal maxPrice { get; set; }
        [JsonProperty("tickSize")]
        public decimal tickSize { get; set; }
        [JsonProperty("multiplierUp")]
        public decimal multiplierUp { get; set; }
        [JsonProperty("multiplierDown")]
        public decimal multiplierDown { get; set; }
        [JsonProperty("avgPriceMins")]
        public int? avgPriceMins { get; set; }
        [JsonProperty("minQty")]
        public decimal minQty { get; set; }
        [JsonProperty("maxQty")]
        public decimal maxQty { get; set; }
        [JsonProperty("stepSize")]
        public decimal stepSize { get; set; }
        [JsonProperty("minNotional")]
        public decimal minNotional { get; set; }
        [JsonProperty("applyToMarket")]
        public bool? applyToMarket { get; set; }
        [JsonProperty("limit")]
        public int? limit { get; set; }
        [JsonProperty("maxNumOrders")]
        public int? maxNumOrders { get; set; }
        [JsonProperty("maxNumAlgoOrders")]
        public int? maxNumAlgoOrders { get; set; }
    }
}
