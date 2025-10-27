using CryptoExchange.Net.SharedApis;

namespace Upbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot rest API usage
    /// </summary>
    public interface IUpbitRestClientSpotApiShared :
        IKlineRestClient,
        IOrderBookRestClient,
        IRecentTradeRestClient,
        ISpotSymbolRestClient,
        ISpotTickerRestClient,
        ITradeHistoryRestClient,
        IBookTickerRestClient
    {
    }
}
