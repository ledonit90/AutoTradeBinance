using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Models
{
    public class DepositeAddressResponse
    {
        public string address { get; set; }
        public bool success { get; set; }
        public string addressTag { get; set; }
        public string asset { get; set; }
        public string Description { get; set; }
    }
}
