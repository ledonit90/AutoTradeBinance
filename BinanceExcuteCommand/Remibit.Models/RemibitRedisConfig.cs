using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemibitPrices.Models
{
    public class RemibitRedisConfig
    {
        public string Servers { get; set; }
        public string SentinelMasterName { get; set; }
        public long DbNumber { get; set; }
        public string AuthPass { get; set; }
        public bool IsSentinel { get; set; }
        public bool IsStrictPool { get; set; }
        public int MaxPoolSize { get; set; }
        /// <summary>
        /// Second
        /// </summary>
        public int MaxPoolTimeout { get; set; }
    }
}
