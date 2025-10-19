using CryptoExchange.Net.Objects;
using Upbit.Net.Clients.SpotApi;
using Upbit.Net.Interfaces.Clients.SpotApi;

namespace Upbit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class UpbitRestClientSpotApiAccount : IUpbitRestClientSpotApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly UpbitRestClientSpotApi _baseClient;

        internal UpbitRestClientSpotApiAccount(UpbitRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }
    }
}
