using StackExchange.Redis;
using System;

namespace Remibit.Utility.Redis
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

        public static IDatabase RedisCache(int IdDatabase) => Connection.GetDatabase(IdDatabase);
    }
}
