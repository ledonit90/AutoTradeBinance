using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemibitPrices.Models.VCCEx
{
    public class VccTrade
    {
        public int trade_id { get; set; }
        public string price { get; set; }
        public string base_volume { get; set; }
        public string quote_volume { get; set; }
        public object trade_timestamp { get; set; }
        public string type { get; set; }
    }

    public class VccTradeHistory
    {
        public object message { get; set; }
        public string dataVersion { get; set; }
        public List<VccTrade> data { get; set; }
    }
}
