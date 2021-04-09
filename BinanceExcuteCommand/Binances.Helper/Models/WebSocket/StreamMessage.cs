using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Models.WebSocket
{
    public class StreamMessage
    {
        public string stream { get; set; }
        public AggregateTradeMessage data { get; set; }
    }
}
