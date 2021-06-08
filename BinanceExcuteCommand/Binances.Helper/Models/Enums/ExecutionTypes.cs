using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Models.Enums
{
    public static class ExecutionType
    {
        public const string NEW = "NEW";
        public const string CANCELED = "CANCELED";
        public const string REPLACED = "REPLACED";
        public const string REJECTED = "REJECTED";
        public const string TRADE = "TRADE";
        public const string EXPIRED = "EXPIRED";
    }
}
