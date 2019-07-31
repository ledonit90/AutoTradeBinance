using StackExchange.Redis;
using System;

namespace RedisProcessing
{
    public class RedisConnectorHelper
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisConnectorHelper()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { "127.0.0.1:6379" }
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase RedisCache => Connection.GetDatabase();
    }
}
