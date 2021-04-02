using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Models.Market.TradingRules
{
    public class TradingRules
    {
        [JsonProperty("timezone")]
        public string timezone { get; set; }
        [JsonProperty("serverTime")]
        public long serverTime { get; set; }
        [JsonProperty("rateLimits")]
        public List<RateLimit> rateLimits { get; set; }
        [JsonProperty("exchangeFilters")]
        public List<object> exchangeFilters { get; set; }
        [JsonProperty("symbols")]
        public List<Symbol> symbols { get; set; }
    }
}
