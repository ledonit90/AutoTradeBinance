using Newtonsoft.Json;
using System.Collections.Generic;

namespace Binances.Helper.Models.WebSocket
{
    public class AccountUpdatedMessage
    {
        [JsonProperty("e")]
        public string EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("u")]
        public long TimeOfLastAccountUpdate { get; set; }
        [JsonProperty("B")]
        public IEnumerable<Balance> Balances { get; set; }
    }
    public class BalanceUpdatedMessage
    {
        [JsonProperty("e")]
        public string EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("a")]
        public string Asset { get; set; }
        [JsonProperty("d")]
        public decimal BalanceDelta { get; set; }
        [JsonProperty("T")]
        public long ClearTime { get; set; }
    }
    public class Balance
    {
        [JsonProperty("a")]
        public string Asset { get; set; }
        [JsonProperty("f")]
        public decimal Free { get; set; }
        [JsonProperty("l")]
        public decimal Locked { get; set; }
    }
}
