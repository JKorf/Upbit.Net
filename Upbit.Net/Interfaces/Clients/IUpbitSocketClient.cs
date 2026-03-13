using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
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
    }
}
