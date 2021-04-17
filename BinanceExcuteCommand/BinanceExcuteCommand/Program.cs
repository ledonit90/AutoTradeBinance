using System;
using Remibit.Utility.Helper;
using Remibit.Models.Remitano;
using System.Threading.Tasks;
using Binances.Helper;
using Remibit.Models.Binance;

namespace BinanceExcuteCommand
{
    class Program
    {
        private static string ApiKey = "GnbABAsKMQYLOgCczlLFMPgOuxCVcJQok3FpUB1V8AvJPHwNLiPV4Ofnj4HRXBx4";
        private static string ApiSecretKey = "9Pvuxyt4rKvAg5fhPkbfibpW7FnT75x64pZqDCa5bVc0Duy0XQL3p8aTa5UjWKZy";
        static async Task Main(string[] args)
        {
            Binance binance = new Binance(ApiKey, ApiSecretKey);
            var m = await binance.buyTokenAsync(new RequestParameter
            {
                symbol = "BNBUSDT",
                quantity = 0.042,
                price = 250,
                type = OrderType.LIMIT
            });

            //api.CallAPIGet(ConstantVarURL.REMI_API_BTC );
            Console.WriteLine("thanh cong is ");
            Console.ReadLine();
        }
    }
}
