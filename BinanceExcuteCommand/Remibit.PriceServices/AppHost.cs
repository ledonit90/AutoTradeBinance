using System;
using System.IO;
using System.Threading;
using Autofac;
using Funq;
using MongoDB.Driver;
using RemibitPrices.Models;
using Serilog;
using Serilog.Formatting.Json;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Logging.Serilog;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using ServiceStack.Text;
using Remibit.CachesServices.Helper;
using BackEndAPI.DataEntities;
using Microsoft.EntityFrameworkCore;
using Remibit.Utility.Common;
using Remibit.PriceServices.Models;
using System.Configuration;

namespace Remibit.PriceServices
{
    public class AppHost : AppSelfHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost() : base("Remibit.PriceServices", typeof(ServiceProcessor).Assembly) { }
        private ILog Log { get; set; }
        private ServiceProcessor registerTask;
        private RedisMqServer _mqHost;
        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            AppSettings = new AppSettings();
            var redisConfigCache = AppSettings.Get<RemibitRedisConfig>("redis.cache");
            var redisCache = RemibitRedisPoolManager.GetClientsManager(redisConfigCache);
            container.Register(redisCache);
            container.Register(c =>
            {
                try
                {
                    using (var redis = redisCache.GetClient())
                    {
                        if (redis.Info != null)
                            return redisCache.GetCacheClient();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                }
                return new MemoryCacheClient();
            });
            var redisConfig = AppSettings.Get<RemibitRedisConfig>("redis.messagequeue");
            var redisMq = RemibitRedisPoolManager.GetClientsManager(redisConfig);
            container.Register<IMessageFactory>(c => new RedisMessageFactory(redisMq));

            var dbconStr = ConfigurationManager.ConnectionStrings["SGDConnection"].ConnectionString;
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<SGDDataContext>().UseLazyLoadingProxies().UseSqlServer(dbconStr);
            var mb = new ContainerBuilder();
            Action<ContainerBuilder> configAction = ((builder) =>
            {
                builder.Register(d => new SGDDataContext(dbContextOptionsBuilder.Options));
                // Mongo
                builder.Register(d =>
                {
                    var client = new MongoClient(AppConstConfig.MongoDbConnection);
                    return client.GetDatabase(AppConstConfig.MonDataSet);
                });
                builder.Register(d => AppSettings);
                //var assembly = Assembly.GetAssembly(typeof(DeliveryClient.Impls.DeliveryClient));
                //builder.RegisterAssemblyTypes(assembly)
                //    .Where(x => x.Namespace != null && x.Namespace.EndsWith("DeliveryClient.Impls"))
                //    .AsImplementedInterfaces();
            });
            configAction.Invoke(mb);

            var autoFacContainer = mb.Build();
            var adapter = new AutofacIocAdapter(autoFacContainer, container) { ConfigAction = configAction };
            container.Adapter = adapter;
            ConfigJsonSerializer();
            ConfigLogger();
            ConfigMqRedis(redisMq, AppSettings);
            container.Register<IMessageService>(_mqHost);
            //Plugins.Add(new LogMonitorFeature());
            this.AfterInitCallbacks.Add(t =>
            {
                Thread.Sleep(2000);
                _mqHost?.Start(); //Starts listening for messages
            });
        }

        private void ConfigJsonSerializer()
        {
            JsConfig.DateHandler = DateHandler.ISO8601;
            SetConfig(new HostConfig
            {
                HandlerFactoryPath = "api",
                DefaultContentType = MimeTypes.Json
            });
        }
        private void ConfigLogger()
        {
            //Switch To SeriLogs
            var logPath = AppSettings.GetString("serilog:write-to:RollingFile.pathlog");
            var buffersize = AppSettings.Get<int>("serilog:write-to:RollingFile.bufferSize");
            buffersize = buffersize > 0 ? buffersize : 5000;

            //Configue 
            var logger = new LoggerConfiguration()
                .ReadFrom.AppSettings();
            
            if (string.IsNullOrEmpty(logPath))
            {
                logPath = "~\\Logging\\log-{Date}.json";
            }

            if (!Path.IsPathRooted(logPath))
            {
                logPath = logPath.MapServerPath();
            }
            
            if (!string.IsNullOrEmpty(logPath))
            {
                logger = logger.WriteTo.Async(a => a.RollingFile(new JsonFormatter(), logPath), buffersize);

            }
            Serilog.Log.Logger = logger.Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithMemoryUsage()
                .Enrich.WithThreadId()
                .CreateLogger();

            LogManager.LogFactory = new SerilogFactory();

            Log = LogManager.GetLogger(GetType());
            Log.Info("KiotViet.Shipping.Service is starting...");
        }

        private void ConfigMqRedis(IRedisClientsManager redisConfig, IAppSettings appSettings)
        {
            _mqHost = new RedisMqServer(redisConfig, appSettings.Get("Remibit:CreatePriceCoin", 0));

            // Create Multi Shipping Order
            _mqHost.RegisterHandler<PriceCoin>(m =>
            {
                m.Options = (int)MessageOption.None;
                var result = ExecuteMessage(m);
                return result;
            }, appSettings.Get<int>("Remibit:CreatePriceCoin"));
        }

        #region Start and Stop
        public override ServiceStackHost Start(string urlBase)
        {
            registerTask = new ServiceProcessor();
            registerTask.getRateRemitanoAsync();
            return base.Start(urlBase);
        }
        #endregion
    }
}
