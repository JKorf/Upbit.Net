using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Options;
using Upbit.Net.Interfaces.Clients.SpotApi;

namespace Upbit.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Upbit Rest API. 
    /// </summary>
    public interface IUpbitRestClient : IRestClient
    {        
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="IUpbitRestClientSpotApi"/>
        public IUpbitRestClientSpotApi SpotApi { get; }
    }
}
