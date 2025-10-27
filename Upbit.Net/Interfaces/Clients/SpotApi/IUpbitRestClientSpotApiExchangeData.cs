using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Upbit.Net.Enums;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Upbit Spot exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IUpbitRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Get list of supported symbols
        /// <para><a href="https://docs.upbit.com/kr/reference/list-trading-pairs" /></para>
        /// </summary>
        /// <param name="includeNotifications">Whether to include events</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitSymbol[]>> GetSymbolsAsync(bool includeNotifications, CancellationToken ct = default);

        /// <summary>
        /// Get trade history
        /// <para><a href="https://docs.upbit.com/kr/reference/list-pair-trades" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `USDT-ETH`</param>
        /// <param name="endTime">Filter by endTime</param>
        /// <param name="limit">Max number of results, max 500</param>
        /// <param name="cursor">Page cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitTrade[]>> GetTradeHistoryAsync(string symbol, DateTime? endTime = null, int? limit = null, string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker info
        /// <para><a href="https://docs.upbit.com/kr/reference/list-tickers" /></para>
        /// </summary>
        /// <param name="symbol">The symbols, for example `USDT-ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker info
        /// <para><a href="https://docs.upbit.com/kr/reference/list-tickers" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `USDT-ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitTicker[]>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker info
        /// <para><a href="https://docs.upbit.com/kr/reference/list-tickers" /></para>
        /// </summary>
        /// <param name="quoteAssets">The quote assets, for example `KRW`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitTicker[]>> GetTickersByQuoteAssetsAsync(IEnumerable<string> quoteAssets, CancellationToken ct = default);

        /// <summary>
        /// Get order book info
        /// <para><a href="https://docs.upbit.com/kr/reference/list-orderbooks" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `KRW-ETH`</param>
        /// <param name="levels">Number of rows</param>
        /// <param name="aggregation">Aggregation level</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitOrderBook>> GetOrderBookAsync(string symbol, int? levels = null, decimal? aggregation = null, CancellationToken ct = default);

        /// <summary>
        /// Get order book info
        /// <para><a href="https://docs.upbit.com/kr/reference/list-orderbooks" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `KRW-ETH`</param>
        /// <param name="levels">Number of rows</param>
        /// <param name="aggregation">Aggregation level</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitOrderBook[]>> GetOrderBooksAsync(IEnumerable<string> symbols, int? levels = null, decimal? aggregation = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline/candlestick data. Note that entries might be missing if there is no data for an entry
        /// <para><a href="https://docs.upbit.com/kr/reference/list-candles-seconds" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `USDT-ETH`</param>
        /// <param name="interval">Interval</param>
        /// <param name="endTime">End time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime ? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get symbol config
        /// <para><a href="https://docs.upbit.com/kr/reference/list-orderbook-instruments" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `USDT-ETH`, comma separated</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitSymbolConfig[]>> GetSymbolConfigAsync(string symbols, CancellationToken ct = default);

    }
}
