using Newtonsoft.Json;
using RemitanoPrices.Models;
using StackExchange.Redis;
using System.Threading.Tasks;
namespace RedisProcessing
{
    public class RedisCRUD
    {
        private IDatabaseAsync db; 
        public RedisCRUD()
        {
            db = RedisConnectorHelper.RedisCache(3);
        }
        public async Task savePriceAsync(CoinPrice price, int unixTime)
        {
            string content = JsonConvert.SerializeObject(price);
            string key = "price_" + price.CodeCoin;
            await db.HashSetAsync(key, new HashEntry[] { new HashEntry(unixTime, content) });
        }

        public async Task RecylebinPriceAsync(string key,int beforeUnixTime, int distance, int space)
        {
            if (distance == 0 || space == 0 || beforeUnixTime == 0 || string.IsNullOrWhiteSpace(key)) return;
            int Round = distance / space;
            for(int i = 1; i <= Round; i++)
            {
                int name = beforeUnixTime + i * space;
                await db.HashDeleteAsync(key, name);
            }
        }
    }
}
