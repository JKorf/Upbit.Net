using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;
using Upbit.Net.Interfaces.Clients;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Objects.Options;

namespace Upbit.Net.Clients
{
    /// <inheritdoc />
    public class UpbitSharedApiClient : SharedApiClientBase, IUpbitSharedApiClient
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
            IUpbitSocketClient socketClient,
            IOptions<UpbitOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                restClient.SpotApi.SharedApi,
                socketClient.SpotApi.SharedApi)
        {
            SpotRest = restClient.SpotApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
        }
    }
}
