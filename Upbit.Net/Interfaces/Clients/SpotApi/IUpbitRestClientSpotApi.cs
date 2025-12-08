using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace Upbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Upbit Spot API endpoints
    /// </summary>
    public interface IUpbitRestClientSpotApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IUpbitRestClientSpotApiExchangeData" />
        public IUpbitRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IUpbitRestClientSpotApiShared SharedClient { get; }
    }
}
