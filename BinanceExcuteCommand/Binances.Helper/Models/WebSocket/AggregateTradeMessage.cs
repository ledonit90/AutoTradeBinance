using Newtonsoft.Json;

namespace Binances.Helper.Models.WebSocket
{
    public class AggregateTradeMessage
    {
        [JsonProperty("e")]
        public string EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("t")]
        public int AggregatedTradeId { get; set; }
        [JsonProperty("p")]
        public decimal Price { get; set; }
        [JsonProperty("q")]
        public decimal Quantity { get; set; }
        [JsonProperty("b")]
        public int BuyerOrderID { get; set; }
        [JsonProperty("a")]
        public int SellerOrderID { get; set; }
        [JsonProperty("T")]
        public long TradeTime { get; set; }
        [JsonProperty("m")]
        public bool BuyerIsMaker { get; set; }
        [JsonProperty("M")]
        public bool Ignore { get; set; }
    }
}
