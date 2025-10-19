using CryptoExchange.Net.Interfaces;
using System;

namespace Upbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Upbit Spot API endpoints
    /// </summary>
    public interface IUpbitRestClientSpotApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IUpbitRestClientSpotApiAccount" />
        public IUpbitRestClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IUpbitRestClientSpotApiExchangeData" />
        public IUpbitRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IUpbitRestClientSpotApiTrading" />
        public IUpbitRestClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IUpbitRestClientSpotApiShared SharedClient { get; }
    }
}
