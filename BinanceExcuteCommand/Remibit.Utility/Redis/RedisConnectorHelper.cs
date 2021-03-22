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
                EndPoints = { "34.221.95.44:6398" }
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase RedisCache(int IdDatabase) => Connection.GetDatabase(IdDatabase);
    }
}
