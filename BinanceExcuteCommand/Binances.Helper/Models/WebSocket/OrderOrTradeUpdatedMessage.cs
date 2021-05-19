using Newtonsoft.Json;

namespace Binances.Helper.Models.WebSocket
{
    public class OrderOrTradeUpdatedMessage
    {
        [JsonProperty("e")]
        public string OrderOrTradeReport { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("c")]
        public string NewClientOrderId { get; set; }
        [JsonProperty("S")]
        public string Side { get; set; }
        [JsonProperty("o")]
        public string Type { get; set; }
        [JsonProperty("f")]
        public string TimeInForce { get; set; }
        [JsonProperty("q")]
        public decimal OriginalQuantity { get; set; }
        [JsonProperty("p")]
        public decimal Price { get; set; }
        [JsonProperty("P")]
        public decimal StopPrice { get; set; }
        [JsonProperty("F")]
        public decimal IcebergQuantity { get; set; }
        [JsonProperty("g")]
        public long OrderListId { get; set; }
        [JsonProperty("C")]
        public string OriginalOrderIdCancelled { get; set; }
        [JsonProperty("x")]
        public string ExecutionType { get; set; }
        [JsonProperty("X")]
        public string Status { get; set; }
        [JsonProperty("r")]
        public string RejectReason { get; set; }
        [JsonProperty("i")]
        public int Orderid { get; set; }
        [JsonProperty("l")]
        public decimal LastFilledTradeQuantity { get; set; }
        [JsonProperty("z")]
        public decimal FilledTradesAccumulatedQuantity { get; set; }
        [JsonProperty("L")]
        public decimal LastFilledTradePrice { get; set; }
        [JsonProperty("n")]
        public decimal Commission { get; set; }
        [JsonProperty("N")]
        public string CommissionAsset { get; set; }
        [JsonProperty("T")]
        public long TradeTime { get; set; }
        [JsonProperty("t")]
        public int TradeId { get; set; }
        [JsonProperty("I")]
        public int Ignore { get; set; }
        [JsonProperty("w")]
        public bool IsOrderOnTheBook { get; set; }
        [JsonProperty("m")]
        public bool BuyerIsMaker { get; set; }
        [JsonProperty("M")]
        public bool Ignore2 { get; set; }
        [JsonProperty("O")]
        public long OrderCreationTime { get; set; }
        [JsonProperty("Z")]
        public decimal TransactedQuantity { get; set; }// Cumulative Quote Asset Transacted Quantity
        [JsonProperty("Y")]
        public decimal LastTransactedQuantity { get; set; } // Last quote asset transacted quantity (i.e. lastPrice * lastQty)
        [JsonProperty("Q")]
        public decimal QuoteOrderQty { get; set; } // Quote Order Qty

    }
}
