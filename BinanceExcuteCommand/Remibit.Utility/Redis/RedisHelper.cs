using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;
using Remibit.Models.Remitano;
using ServiceStack;

namespace Remibit.Utility.Redis
{
    public class RedisHelper
    {
        private IDatabaseAsync db; 
        public RedisHelper()
        {
            db = RedisConnectorHelper.RedisCache(3);
        }
        public void SaveContentWithUnixtime<T>(string key,object price, int unixTime)
        {
            string content = price.ToSafeJson();
            db.HashSetAsync(key, new HashEntry[] { new HashEntry(unixTime, content) });
        }

        public T GetObjectWithUnixTime<T>(string key, int unixTime)
        {
            var content = db.HashGetAsync(key,unixTime);
            return JsonConvert.DeserializeObject<T>(content.Result);
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
