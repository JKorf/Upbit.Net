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

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IUpbitRestClientSpotSharedApi :
        IGetKlinesRest,
        IGetOrderBookRest,
        IGetRecentTradesRest,
        IGetSpotSymbolsRest,
        IGetSpotTickerRest,
        IGetAllSpotTickersRest,
        IGetTradeHistoryRest,
        IGetBookTickerRest
    {
    }
}
