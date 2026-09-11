using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace Upbit.Net.Objects.Options
{
    /// <summary>
    /// Upbit options
    /// </summary>
    public class UpbitOptions : LibraryOptions<UpbitRestOptions, UpbitSocketOptions, UpbitEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}
