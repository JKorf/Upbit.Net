using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Upbit.Net.Interfaces.Clients.SpotApi;

namespace Upbit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class UpbitRestClientSpotApiTrading : IUpbitRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly UpbitRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal UpbitRestClientSpotApiTrading(ILogger logger, UpbitRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }
    }
}
