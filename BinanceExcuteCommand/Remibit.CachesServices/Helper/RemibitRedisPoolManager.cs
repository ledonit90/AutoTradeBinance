using RemibitPrices.Models;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace Remibit.CachesServices.Helper
{
    public class RemibitRedisPoolManager : IRemibitRedisPoolManager
    {
        private readonly List<IRedisClientsManager> _pools;
        static readonly ILog Log = LogManager.GetLogger(typeof(RemibitRedisPoolManager));
        public static IRedisClientsManager GetClientsManager(RemibitRedisConfig config)
        {
            try
            {
                if (config == null) return null;
                var maxpoolsize = config.MaxPoolSize > 0 ? config.MaxPoolSize : 40;
                var pooltimeout = config.MaxPoolTimeout > 0 ? config.MaxPoolTimeout : 10;
                if (config.IsSentinel)
                {
                    IEnumerable<string> servers = config.Servers.Split(';');

                    var sentinel = new RedisSentinel(servers, config.SentinelMasterName)
                    {
                        RedisManagerFactory = (master, slaves) => new RedisManagerPool(master, new RedisPoolConfig { MaxPoolSize = maxpoolsize })
                    };
                    if (config.IsStrictPool)
                    {
                        sentinel.RedisManagerFactory = (master, slaves) =>
                            new PooledRedisClientManager(master, master, null, config.DbNumber, maxpoolsize,
                                null);
                    }
                    sentinel.OnFailover += x => Log.Error($"Redis fail over to {sentinel.GetMaster()}");
                    sentinel.HostFilter = host => $"{config.AuthPass}@{host}?db={config.DbNumber}&RetryTimeout=100";
                    var redisClient = sentinel.Start();
                    return redisClient;
                }
                var redisConnection = $"{config.AuthPass}@{config.Servers}";
                return new PooledRedisClientManager(new[] { redisConnection }, new[] { redisConnection }, null, config.DbNumber, maxpoolsize, pooltimeout);

            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
            }

            return null;
        }


        public RemibitRedisPoolManager(IAppSettings settings)
        {

            _pools = new List<IRedisClientsManager>();
            var ls = settings.Get<List<RemibitRedisConfig>>("redis.connections");
            try
            {
                foreach (var config in ls)
                {
                    _pools.Add(GetClientsManager(config));
                }
            }
            catch (Exception e)
            {

                Log.Error(e.Message, e);
            }

        }
        public List<IRedisClientsManager> GetPools()
        {
            return _pools;
        }
    }
}