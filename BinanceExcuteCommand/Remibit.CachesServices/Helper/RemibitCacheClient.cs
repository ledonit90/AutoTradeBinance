using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Remibit.CachesServices.Helper
{
    public class RemibitCacheClient : ICacheClientExtended
    {
        private readonly IRemibitMemoryCache _memoryCache;
        private readonly IRedisClientsManager _redisClients;
        private static readonly ILog Log = LogManager.GetLogger(typeof(RemibitCacheClient));
        private const string KeyCahce = "cache:memoryCache:usingredis";
        private readonly TimeSpan _timeCheckCache = TimeSpan.FromMinutes(5);
        private static int _usingResource;
        private readonly bool _usingSlave;
        private static readonly object LockChange = new object();
        private static readonly object Lockobj = new object();

        private bool UseRedis
        {
            get
            {
                lock (Lockobj)
                {
                    var check = _memoryCache.Get(KeyCahce);
                    if (check != null)
                    {
                        return (bool)check;
                    }
                }
                try
                {
                    if (_redisClients != null && 0 == Interlocked.Exchange(ref _usingResource, 1))
                    {
                        var retval = CheckStableRedis();
                        if (retval)
                        {
                            SwitchCache(true);
                        }
                        Interlocked.Exchange(ref _usingResource, 0);
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Interlocked.Exchange(ref _usingResource, 0);
                    Error(e);
                }
                return false;
            }
        }

        private bool CheckStableRedis()
        {
            var useRedis = _memoryCache.Get<bool>(KeyCahce);

            var ping = PingRedis();
            if (useRedis && ping)
            {
                return true;
            }

            var stable = false;
            for (var i = 0; i < 3; i++)
            {
                if (PingRedis())
                {
                    Thread.Sleep(100);
                    stable = true;
                }
                else
                {
                    stable = false;
                    break;
                }
            }
            return stable;
        }

        private bool PingRedis()
        {
            try
            {
                using (var cli = (IRedisNativeClient)_redisClients.GetClient())
                {
                    return cli.Ping();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                SwitchCache(false);
            }
            return false;
        }
        private void SwitchCache(bool useRedis)
        {
            lock (LockChange)
            {
                if (useRedis)
                {
                    Log.Error("Using Redis Cache");
                    _memoryCache.FlushAll();
                }
                else
                {
                    Log.Error("Using Memory Cache");
                }
                _memoryCache.Set(KeyCahce, useRedis, _timeCheckCache);
            }
        }

        private void Error(Exception e)
        {
            Log.Error(e.Message, e);
            SwitchCache(false);
        }
        public RemibitCacheClient(IRedisClientsManager redisClients, IAppSettings settings, IRemibitMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _redisClients = redisClients;

        }
        public void Dispose()
        {
            _memoryCache.Dispose();
        }

        private IRedisClient GetReadClient()
        {
            try
            {
                if (_usingSlave)
                {
                    return _redisClients.GetReadOnlyClient();
                }
            }
            catch (Exception e)
            {
                Log.Error("Can't get slave" + e.Message, e);
            }

            return _redisClients.GetClient();
        }
        public bool Remove(string key)
        {
            try
            {
                if (UseRedis)
                {
                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Remove(key);
                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }

            return _memoryCache.Remove(key);
        }
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null) return;
            var enumerable = keys.ToList();
            try
            {
                if (UseRedis)
                {
                    using (var cli = _redisClients.GetClient())
                    {
                        cli.RemoveAll(enumerable);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            _memoryCache.RemoveAll(enumerable);
        }
        public T Get<T>(string key)
        {
            try
            {
                if (UseRedis)
                {

                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Get<T>(key);
                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Get<T>(key);
        }
        public long Increment(string key, uint amount)
        {
            try
            {
                if (UseRedis)
                {

                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Increment(key, amount);
                    }

                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Increment(key, amount);
        }
        public long Decrement(string key, uint amount)
        {
            try
            {
                if (UseRedis)
                {

                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Decrement(key, amount);
                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Decrement(key, amount);

        }
        public bool Add<T>(string key, T value)
        {
            try
            {
                if (UseRedis)
                {
                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Add(key, value);
                    }

                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Add(key, value);
        }
        public bool Set<T>(string key, T value)
        {
            try
            {
                if (UseRedis)
                {

                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Set(key, value);
                    }

                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Set(key, value);
        }
        public bool Replace<T>(string key, T value)
        {
            try
            {
                if (UseRedis)
                {
                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Replace(key, value);
                    }

                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Replace(key, value);

        }
        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            try
            {
                if (UseRedis)
                {

                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Add(key, value, expiresAt);
                    }

                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Add(key, value, expiresAt);
        }
        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            try
            {
                if (UseRedis)
                {

                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Set(key, value, expiresAt);
                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Set(key, value, expiresAt);

        }
        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            try
            {
                if (UseRedis)
                {

                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Replace(key, value, expiresAt);
                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Replace(key, value, expiresAt);
        }
        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            try
            {
                if (UseRedis)
                {

                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Add(key, value, expiresIn);
                    }

                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Add(key, value, expiresIn);
        }
        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            try
            {
                if (UseRedis)
                {
                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Set(key, value, expiresIn);
                    }

                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Set(key, value, expiresIn);
        }
        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            try
            {
                if (UseRedis)
                {
                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.Replace(key, value, expiresIn);
                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.Replace(key, value, expiresIn);
        }
        public void FlushAll()
        {
            try
            {
                if (UseRedis)
                {
                    using (var cli = _redisClients.GetClient())
                    {
                        cli.FlushAll();
                        return;

                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            _memoryCache.FlushAll();

        }
        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            var enumerable = keys.ToList();
            try
            {
                if (UseRedis)
                {
                    using (var cli = _redisClients.GetClient())
                    {
                        return cli.GetAll<T>(enumerable);
                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            return _memoryCache.GetAll<T>(enumerable);
        }
        public void SetAll<T>(IDictionary<string, T> values)
        {
            try
            {
                if (UseRedis)
                {
                    using (var cli = _redisClients.GetClient())
                    {
                        cli.SetAll(values);
                        return;

                    }
                }
            }
            catch (Exception e)
            {
                Error(e);
            }
            _memoryCache.SetAll(values);
        }
        public TimeSpan? GetTimeToLive(string key)
        {
            if (!UseRedis) return _memoryCache.GetTimeToLive(key);
            using (var cli = _redisClients.GetClient())
            {
                return cli.GetTimeToLive(key);
            }

            return null;
        }
        public IEnumerable<string> GetKeysByPattern(string pattern)
        {
            if (UseRedis)
            {
                using (var cli = _redisClients.GetClient())
                {
                    return cli.GetKeysByPattern(pattern);
                }
            }

            return null;
        }

        public Dictionary<string, string> GetByPipeLine(List<string> keys)
        {
            var dic = new Dictionary<string, string>();
            if (UseRedis)
            {
                using (var cli = GetReadClient())
                {
                    using (var pipe = cli.CreatePipeline())
                    {
                        foreach (var key in keys)
                        {
                            pipe.QueueCommand(t => t.Get<string>(key), c => dic.Add(key, c));
                            //    dic.Add(key, cli.Get<string>(key));
                        }
                        pipe.Flush();
                    }

                }

            }
            else
            {
                foreach (var key in keys)
                {
                    dic.Add(key, _memoryCache.Get(key).ToJson());
                }

            }

            return dic;
        }
    }
}
