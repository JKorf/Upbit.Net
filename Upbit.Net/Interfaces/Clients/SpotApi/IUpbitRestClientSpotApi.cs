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
        /// Get the shared rest requests client. For new implementations prefer <see cref="SharedApi"/>
        /// </summary>
        public IUpbitRestClientSpotApiShared SharedClient { get; }

        /// <summary>
        /// Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public IUpbitRestClientSpotSharedApi SharedApi { get; }
    }
}
