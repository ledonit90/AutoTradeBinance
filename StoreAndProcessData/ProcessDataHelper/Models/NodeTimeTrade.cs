using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDataHelper.Models
{
    public class NodeTimeTrade
    {
        // 100ms 400ms 700ms 1s 2s 5s 10s 20s 40s 
        public double MaxPrice { get; set; }
        public double MinPrice { get; set; }
        public long serverTime { get; set; }
    }
}
