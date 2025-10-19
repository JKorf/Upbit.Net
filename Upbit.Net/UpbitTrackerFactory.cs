using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using Upbit.Net.Interfaces;
using Upbit.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Upbit.Net.Clients;

namespace Upbit.Net
{
    /// <inheritdoc />
    public class UpbitTrackerFactory : IUpbitTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public UpbitTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public UpbitTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval)
        {
            var client = _serviceProvider?.GetRequiredService<IUpbitSocketClient>() ?? new UpbitSocketClient();
#warning TODO
            SubscribeKlineOptions klineOptions = new SubscribeKlineOptions(true);
            return klineOptions.IsSupported(interval); 
        }

        public bool CanCreateTradeTracker(SharedSymbol symbol) => true;

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IUpbitRestClient>() ?? new UpbitRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IUpbitSocketClient>() ?? new UpbitSocketClient();

#warning todo
            throw new NotImplementedException();
            //IKlineRestClient sharedRestClient;
            //IKlineSocketClient sharedSocketClient;
            //if (symbol.TradingMode == TradingMode.Spot)
            //{
            //    sharedRestClient = restClient.SpotApi.SharedClient;
            //    sharedSocketClient = socketClient.SpotApi.SharedClient;
            //}
            //else
            //{
            //    sharedRestClient = restClient.FuturesApi.SharedClient;
            //    sharedSocketClient = socketClient.FuturesApi.SharedClient;
            //}

            //return new KlineTracker(
            //    _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
            //    sharedRestClient,
            //    sharedSocketClient,
            //    symbol,
            //    interval,
            //    limit,
            //    period
            //    );
        }
        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IUpbitRestClient>() ?? new UpbitRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IUpbitSocketClient>() ?? new UpbitSocketClient();

#warning todo
            throw new NotImplementedException();

            //IRecentTradeRestClient? sharedRestClient;
            //ITradeSocketClient sharedSocketClient;
            //if (symbol.TradingMode == TradingMode.Spot)
            //{
            //    sharedRestClient = restClient.SpotApi.SharedClient;
            //    sharedSocketClient = socketClient.SpotApi.SharedClient;
            //}
            //else
            //{
            //    sharedRestClient = restClient.FuturesApi.SharedClient;
            //    sharedSocketClient = socketClient.FuturesApi.SharedClient;
            //}

            //return new TradeTracker(
            //    _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
            //    sharedRestClient,
            //    null,
            //    sharedSocketClient,
            //    symbol,
            //    limit,
            //    period
            //    );
        }
    }
}
