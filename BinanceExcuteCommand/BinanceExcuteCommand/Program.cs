using System;
using Binances.Helper;

namespace BinanceExcuteCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ApiKey = "m6I3N7jxf6gTrPxEDTG3RnTbn5uDemTCu6Ge7AWRFkFbJEvjw0Pi8ZZRhCl9vl91";
            const string ApiSecretKey = "HAimeHvPpeqzgFebEUwcpMQy4QYY82Wef4u0rHNIFXNo1lXz8y8ig204dpXYnVtE";
            //RequestParameter reRQ = new RequestParameter();
            //reRQ.symbol = "BTCUSDT";
            //reRQ.type = "LIMIT";
            //reRQ.price = 9000.92;
            //reRQ.quantity = 0.03;
            //reRQ.recvWindow = 5000;
            //reRQ.timeInForce = "GTC";
            Binance binance = new Binance(ApiKey, ApiSecretKey);
            //var m = binance.getDepositeAddressAsync();
            //m.Wait();
            BinanceStream stream = new BinanceStream();

            var xxx = stream.getDataStream();
            xxx.Wait();
            Console.WriteLine("thanh cong is " + xxx.Status);
            Console.ReadLine();
        }
    }
}
