using CryptoExchange.Net.SharedApis;

namespace Upbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot socket API usage
    /// </summary>
    public interface IUpbitSocketClientSpotApiShared :
        ITickerSocketClient,
        ITradeSocketClient,
        IBookTickerSocketClient,
        IKlineSocketClient,
        IOrderBookSocketClient
    {
    }
}
