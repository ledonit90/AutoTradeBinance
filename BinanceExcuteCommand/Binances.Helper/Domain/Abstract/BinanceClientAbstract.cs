using Binances.Helper.Domain.Interface;
using Binances.Helper.Models.Market.TradingRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Domain.Abstract
{
    public abstract class BinanceClientAbstract
    {
        /// <summary>
        /// Secret used to authenticate within the API.
        /// </summary>
        public TradingRules _tradingRules;

        /// <summary>
        /// Client to be used to call the API.
        /// </summary>
        public readonly IApiClient _apiClient;

        /// <summary>
        /// Defines the constructor of the Binance client.
        /// </summary>
        /// <param name="apiClient">API client to be used for API calls.</param>
        public BinanceClientAbstract(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
    }
}
