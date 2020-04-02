using System;
using Remibit.Utility.Helper;
using Remibit.Models.Remitano;
using System.Threading.Tasks;

namespace BinanceExcuteCommand
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //RequestParameter reRQ = new RequestParameter();
            //reRQ.symbol = "BTCUSDT";
            //reRQ.type = "LIMIT";
            //reRQ.price = 9000.92;
            //reRQ.quantity = 0.03;
            //reRQ.recvWindow = 5000;
            //reRQ.timeInForce = "GTC";
            //Binance binance = new Binance(ApiKey, ApiSecretKey);
            ////var m = binance.getDepositeAddressAsync();
            ////m.Wait();
            //BinanceStream stream = new BinanceStream();

            //var xxx = stream.getDataStream();
            //xxx.Wait();
            // test getset API
            //CallWebAPI api = new CallWebAPI();
            HttpRequestHelper remitanoSite = new HttpRequestHelper();
            var x = await remitanoSite.GetTAsync<REMI_RATE>("https://remitano.com/api/v1/rates/exchange");
            Console.WriteLine(x.bfx_BCHUSD);

            //api.CallAPIGet(ConstantVarURL.REMI_API_BTC );
            Console.WriteLine("thanh cong is ");
            Console.ReadLine();
        }
    }
}
