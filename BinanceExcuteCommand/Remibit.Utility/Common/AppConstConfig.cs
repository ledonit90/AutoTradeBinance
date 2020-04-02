using System;
using System.Configuration;
namespace Remibit.Utility.Common
{
    public static partial class AppConstConfig
    {
        public static string REDIS_IP => ConfigurationManager.AppSettings["REDIS_IP"];
        public static int REDIS_PORT => int.Parse(ConfigurationManager.AppSettings["REDIS_PORT"]);

        public static string RABBITMQ_IP => ConfigurationManager.AppSettings["RABBITMQ_IP"];
        public static int RABBIT_PORT => int.Parse(ConfigurationManager.AppSettings["RABBIT_PORT"]);
        public static string RABBITMQ_USERNAME => ConfigurationManager.AppSettings["RABBITMQ_USERNAME"];
        public static string RABBITMQ_PASSWORD => ConfigurationManager.AppSettings["RABBITMQ_PASSWORD"];
        public static string PRICE_QEXCHANGE => ConfigurationManager.AppSettings["PRICE_QEXCHANGE"];
        public static string PRICE_QNAME => ConfigurationManager.AppSettings["PRICE_QNAME"];
        public static string PRICE_EXCHANGE_TYPE => ConfigurationManager.AppSettings["PRICE_EXCHANGE_TYPE"];

        public static string REMITANO_URL => ConfigurationManager.AppSettings["REMITANO_URL"];
        public static string REMITANO_API => ConfigurationManager.AppSettings["REMITANO_API"];


        public static string BINANCE_URL => ConfigurationManager.AppSettings["BINANCE_URL"];
        public static string BINANCE_API => ConfigurationManager.AppSettings["BINANCE_API"];
        public static string API_KEY => ConfigurationManager.AppSettings["API_KEY"];
        public static string API_SECRET_KEY => ConfigurationManager.AppSettings["API_SECRET_KEY"];

        public static string MongoDbConnection => ConfigurationManager.AppSettings["MongoDbConnection"];
        public static string MonDataSet => ConfigurationManager.AppSettings["MonDataSet"];

    }
}
