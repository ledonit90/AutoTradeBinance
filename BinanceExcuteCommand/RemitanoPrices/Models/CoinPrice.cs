using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemitanoPrices.Models
{
    public class CoinPrice
    {
        public double VNDRate { get; set; }
        public double EURORate { get; set; }
        public double USDPrice { get; set; }
        public string CodeCoin { get; set; }
    }
}
