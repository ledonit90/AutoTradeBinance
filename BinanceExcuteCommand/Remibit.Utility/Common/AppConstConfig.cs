using System.Configuration;
namespace Remibit.Utility.Common
{
    public partial class AppConstConfig
    {
        public static string RabbitPort => ConfigurationSettings.AppSettings["RabbitPort"];
        public static string RedisPort => ConfigurationSettings.AppSettings["RedisPort"];
        public static string RedisIdAddress => ConfigurationSettings.AppSettings["RedisIdAddress"];
        public static string RabbitIpAddress => ConfigurationSettings.AppSettings["RabbitIpAddress"];

        public static string RemitanoUrl => ConfigurationSettings.AppSettings["RemitanoUrl"];
        public static string BinanceUrl => ConfigurationSettings.AppSettings["BinanceUrl"];
    }
}
