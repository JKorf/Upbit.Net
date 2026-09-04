using Upbit.Net.Interfaces.Clients;
using Upbit.Net.Interfaces.Clients.SpotApi;

namespace Upbit.Net.Clients
{
    /// <inheritdoc />
    public class UpbitSharedApiClient : IUpbitSharedApiClient
    {
        /// <inheritdoc />
        public IUpbitRestClientSpotSharedApi SpotRest { get; }
        /// <inheritdoc />
        public IUpbitSocketClientSpotSharedApi SpotSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public UpbitSharedApiClient(
            IUpbitRestClient restClient,
            IUpbitSocketClient socketClient)
        {
            SpotRest = restClient.SpotApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
        }
    }
}
