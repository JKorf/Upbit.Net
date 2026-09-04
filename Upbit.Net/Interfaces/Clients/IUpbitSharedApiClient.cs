using Upbit.Net.Interfaces.Clients.SpotApi;

namespace Upbit.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of Upbit
    /// </summary>
    public interface IUpbitSharedApiClient
    {
        /// <summary>
        /// Spot REST shared API implementations
        /// </summary>
        IUpbitRestClientSpotSharedApi SpotRest { get; }

        /// <summary>
        /// Spot WebSocket shared API implementations
        /// </summary>
        IUpbitSocketClientSpotSharedApi SpotSocket { get; }
    }
}
