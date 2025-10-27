using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using Upbit.Net.Interfaces;
using Upbit.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
using System;
using Upbit.Net.Clients;
using Microsoft.Extensions.Logging;

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

        /// <inheritdoc />
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                return false;

            var client = _serviceProvider?.GetRequiredService<IUpbitSocketClient>() ?? new UpbitSocketClient();
            var klineOptions = client.SpotApi.SharedClient.SubscribeKlineOptions;
            return klineOptions.IsSupported(interval); 
        }

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol)
        {
            return symbol.TradingMode != TradingMode.Spot;
        }

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IUpbitRestClient>() ?? new UpbitRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IUpbitSocketClient>() ?? new UpbitSocketClient();

            var sharedRestClient = restClient.SpotApi.SharedClient;
            var sharedSocketClient = socketClient.SpotApi.SharedClient;

            return new KlineTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                sharedSocketClient,
                symbol,
                interval,
                limit,
                period
                );
        }

        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IUpbitRestClient>() ?? new UpbitRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IUpbitSocketClient>() ?? new UpbitSocketClient();

            var sharedRestClient = restClient.SpotApi.SharedClient;
            var sharedSocketClient = socketClient.SpotApi.SharedClient;

            return new TradeTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                sharedRestClient,
                sharedSocketClient,
                symbol,
                limit,
                period
                );
        }
    }
}
