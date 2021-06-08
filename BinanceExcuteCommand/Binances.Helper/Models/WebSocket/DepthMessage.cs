using Binances.Helper.Models.Market;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Binances.Helper.Models.WebSocket
{
    public class DepthMessage
    {
        [JsonProperty("e")]
        public string EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("U")]
        public int FistUpdateId { get; set; }
        [JsonProperty("u")]
        public int LastUpdateId { get; set; }
        public List<OrderBookOffer> Bids { get; set; }
        public List<OrderBookOffer> Asks { get; set; }
    }
}
