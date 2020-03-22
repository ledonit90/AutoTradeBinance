using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remibit.Models.Binance
{
    public class RequestParameter
    {
        public string symbol { get; set; }
        public string type { get; set; }
        public string timeInForce { get; set; }
        public double quantity { get; set; }
        public double price { get; set; }
        public int recvWindow { get; set; }
    }
}
