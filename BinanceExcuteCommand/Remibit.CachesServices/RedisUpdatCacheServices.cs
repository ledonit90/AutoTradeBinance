using Remibit.CachesServices.Helper;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Redis;
using System.Collections.Generic;
using System.Linq;


namespace Remibit.CachesServices
{
    public class RedisUpdatCacheServices : Service
    {
        private readonly IRemibitRedisPoolManager _kvRedisPoolManager;
        public IAppSettings AppSettings { get; set; }
        public RedisUpdatCacheServices(IRemibitRedisPoolManager kvRedisPoolManager)
        {
            _kvRedisPoolManager = kvRedisPoolManager;
        }

        private void RemoveCache(IRedisClientsManager redis, List<string> keys)
        {
            using (var cli = redis.GetClient())
            {
                if (keys.Any(t => !t.Contains("*")))
                {
                    cli.RemoveAll(keys.SafeWhere(t => !t.Contains("*")));
                }
                foreach (var key in keys.SafeWhere(t => t.Contains("*")))
                {
                    var lskey = cli.ScanAllKeys(key);
                    cli.RemoveAll(lskey);
                }
            }
        }
    }
}