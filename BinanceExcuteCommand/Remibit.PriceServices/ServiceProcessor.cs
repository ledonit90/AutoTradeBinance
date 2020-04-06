using Remibit.PriceServices.Models;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using System.Threading.Tasks;
using ServiceStack.Caching;
using System.Timers;
using Remibit.Utility.Redis;
using Remibit.Utility.Helper;
using Remibit.PriceServices.RequestDTO;

namespace Remibit.PriceServices
{
    public class ServiceProcessor : Service
    {
        ICacheClient CacheClient;
        RedisHelper redis;
        #region Dependency
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ServiceProcessor));
        public ILog Log => Logger;
        private IAppSettings _appSettings { get; set; }
        public IAppSettings AppPriceSettings
        {
            get
            {
                _appSettings = _appSettings ?? HostContext.Resolve<IAppSettings>();
                return _appSettings;
            }
        }
        #endregion
        public ServiceProcessor()
        {
            CacheClient = HostContext.TryResolve<ICacheClient>();
            redis = new RedisHelper();
        }
        #region Handle MQ
        public async Task<object> Any(PriceCoin req)
        {
            return null;
        }
        #endregion

        #region function getprice
        public void getRateRemitano()
        {
            Timer aTimer;
            aTimer = new Timer();
            aTimer.Interval = 1000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (source, e) =>
            {
                var test = DateTimeHelper.CurrentUnixTimeStamp();
                redis.SaveContentWithUnixtime<int>("Remitano:VNDBTCRATE", test, test);
            };
        }
        #endregion

        public object Any(Ping req)
        {
            return "pong" + (req != null && req.IncludeCmitSha ? (" - 0373812d852e39d0cbcfda14d58823de4908cbad") : string.Empty);
        }
    }
}
