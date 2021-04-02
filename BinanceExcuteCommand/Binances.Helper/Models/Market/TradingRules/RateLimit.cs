using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Models.Market.TradingRules
{
   public class RateLimit
    {
        [JsonProperty("rateLimitType")]
        public string rateLimitType { get; set; }
        [JsonProperty("interval")]
        public string interval { get; set; }
        [JsonProperty("intervalNum")]
        public int intervalNum { get; set; }
        [JsonProperty("limit")]
        public int limit { get; set; }
    }
}
