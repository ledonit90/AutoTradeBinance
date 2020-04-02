using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;

namespace Remibit.PriceServices
{
    public class ServiceProcessor
    {
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

    }
}
