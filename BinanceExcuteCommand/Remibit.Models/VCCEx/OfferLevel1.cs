using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemibitPrices.Models.VCCEx
{
    public class VCCOffers
    {
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }
        public string timestamp { get; set; }
    }

    public class OfferLevel1
    {
        public object message { get; set; }
        public string dataVersion { get; set; }
        public VCCOffers data { get; set; }
    }
}
