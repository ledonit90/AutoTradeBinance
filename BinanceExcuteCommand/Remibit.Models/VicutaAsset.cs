using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemibitPrices.Models
{
    public class VicutaAsset
    {
        public int id { get; set; }
        public string assetName { get; set; }
        public string assetNameLong { get; set; }
        public double change24h { get; set; }
        public double volume24h { get; set; }
        public double buyPrice { get; set; }
        public double sellPrice { get; set; }
        public bool isActive { get; set; }
    }
}
