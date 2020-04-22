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
using VietnameseExchange.Helper;

namespace Remibit.PriceServices
{
    public class ServiceProcessor : Service
    {
        ICacheClient CacheClient;
        RedisHelper redis;
        RemitanoHelper remitanoHelper;
        VCCHelper vccHelper;
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
            vccHelper = new VCCHelper();
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
            //foreach(var coin in ConstantVarURL.Remitano_Listcoin)
            //{
            //    DateTimeHelper.getATimer(getThePriceRemitano, 4000, coin);
            //}

            foreach(var pair in ConstantVarURL.VccTradingPairs)
            {
                DateTimeHelper.getATimer(getThePriceVCC, 4000, pair);
            }
        }

        private async void getThePriceRemitano(Object source, ElapsedEventArgs e)
        {
            var timeStamp = DateTimeHelper.CurrentUnixTimeStamp();
            string coin = ((CustomTimer)source).Data;
            await remitanoHelper.PriceOnTime(coin, timeStamp);
        }

        private async void getThePriceVCC(Object source, ElapsedEventArgs e)
        {
            var timeStamp = DateTimeHelper.CurrentUnixTimeStamp();
            string pair = ((CustomTimer)source).Data;
            await vccHelper.PriceOnTime(pair, timeStamp);
        }
        #endregion

        public object Any(Ping req)
        {
            return "pong" + (req != null && req.IncludeCmitSha ? (" - 0373812d852e39d0cbcfda14d58823de4908cbad") : string.Empty);
        }
    }
}
