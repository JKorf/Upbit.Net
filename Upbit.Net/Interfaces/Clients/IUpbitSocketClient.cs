using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using Upbit.Net.Interfaces.Clients.SpotApi;

namespace Upbit.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Upbit websocket API
    /// </summary>
    public interface IUpbitSocketClient : ISocketClient
    {        
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="IUpbitSocketClientSpotApi"/>
        public IUpbitSocketClientSpotApi SpotApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
