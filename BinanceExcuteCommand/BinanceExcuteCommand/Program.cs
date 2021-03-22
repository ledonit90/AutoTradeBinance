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
            //RequestParameter reRQ = new RequestParameter();
            //reRQ.symbol = "BTCUSDT";
            //reRQ.type = "LIMIT";
            //reRQ.price = 9000.92;
            //reRQ.quantity = 0.03;
            //reRQ.recvWindow = 5000;
            //reRQ.timeInForce = "GTC";
            Binance binance = new Binance(ApiKey, ApiSecretKey);
            var m = await binance.buyTokenAsync(new RequestParameter
            {
                symbol = "BNBUSDT",
                quantity = 0.042,
                price = 250,
                type = OrderType.LIMIT
            });
            //BinanceStream stream = new BinanceStream();

            //var xxx = stream.getDataStream();
            //xxx.Wait();
            // test getset API
            //CallWebAPI api = new CallWebAPI();
            //HttpRequestHelper remitanoSite = new HttpRequestHelper();
            //var x = await remitanoSite.GetTAsync<REMI_RATE>("https://remitano.com/api/v1/rates/exchange");
            //Console.WriteLine(x.bfx_BCHUSD);

            //api.CallAPIGet(ConstantVarURL.REMI_API_BTC );
            Console.WriteLine("thanh cong is ");
            Console.ReadLine();
        }
    }
}
