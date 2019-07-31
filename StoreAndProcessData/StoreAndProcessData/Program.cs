using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDataHelper;

namespace StoreAndProcessData
{
    class Program
    {
        static void Main(string[] args)
        {
            Subscriber test = new Subscriber("localhost", 5672,"getData","usdtbtc");
            test.SubcribeAChannel();
            Console.WriteLine("----------------------------------------------------------");
        }
    }
}
