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
using System;
using static Remibit.Utility.BunchOfEnum;
using Remibit.Utility;

namespace Remibit.PriceServices
{
    public class ServiceProcessor : Service
    {
        ICacheClient CacheClient;
        RedisHelper redis;
        RemitanoHelper remitanoHelper;
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
            remitanoHelper = new RemitanoHelper();
        }
        #region Handle MQ
        public async Task<object> Any(PriceCoin req)
        {
            return null;
        }
        #endregion

        #region function getprice
        public void getRateRemitanoAsync()
        {
            DateTimeHelper.getATimer(getETHPrice);
            DateTimeHelper.getATimer(getBTCPrice);
        }

        private async void getETHPrice(Object source, ElapsedEventArgs e)
        {
            var timeStamp = DateTimeHelper.CurrentUnixTimeStamp();
            await remitanoHelper.PriceOnTime(Remitano_coin.eth.GetDescription(), timeStamp);
            //redis.SaveContentWithUnixtime<int>("Remitano:VNDETHRATE", timeStamp, timeStamp);
        }

        private async void getBTCPrice(Object source, ElapsedEventArgs e)
        {
            var unixTime = DateTimeHelper.CurrentUnixTimeStamp();
            await remitanoHelper.PriceOnTime(Remitano_coin.btc.GetDescription(), unixTime);
            //redis.SaveContentWithUnixtime<int>("Remitano:VNDBTCRATE", test.ToString() + "btc", test);
        }
        #endregion

        public object Any(Ping req)
        {
            return "pong" + (req != null && req.IncludeCmitSha ? (" - 0373812d852e39d0cbcfda14d58823de4908cbad") : string.Empty);
        }
    }
}
