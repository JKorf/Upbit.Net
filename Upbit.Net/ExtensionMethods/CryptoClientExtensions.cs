using CryptoExchange.Net.Interfaces.Clients;
using Upbit.Net.Clients;
using Upbit.Net.Interfaces.Clients;

namespace CryptoExchange.Net.Interfaces
{
    /// <summary>
    /// Extensions for the ICryptoRestClient and ICryptoSocketClient interfaces
    /// </summary>
    public static class CryptoClientExtensions
    {
        /// <summary>
        /// Get the Upbit REST Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IUpbitRestClient Upbit(this ICryptoRestClient baseClient) => baseClient.TryGet<IUpbitRestClient>(() => new UpbitRestClient());

        /// <summary>
        /// Get the Upbit Websocket Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IUpbitSocketClient Upbit(this ICryptoSocketClient baseClient) => baseClient.TryGet<IUpbitSocketClient>(() => new UpbitSocketClient());
    }
}
