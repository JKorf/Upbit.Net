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
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.upbit.com/kr/reference/list-trading-pairs" /><br />
        /// Endpoint:<br />
        /// GET /v1/market/all
        /// </para>
        /// </summary>
        /// <param name="includeNotifications">["<c>is_details</c>"] Whether to include events</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitSymbol[]>> GetSymbolsAsync(bool includeNotifications, CancellationToken ct = default);

        /// <summary>
        /// Get trade history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.upbit.com/kr/reference/list-pair-trades" /><br />
        /// Endpoint:<br />
        /// GET /v1/trades/ticks
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] The symbol, for example `USDT-ETH`</param>
        /// <param name="endTime">["<c>to</c>", "<c>days_ago</c>"] Filter by endTime</param>
        /// <param name="limit">["<c>count</c>"] Max number of results, max 500</param>
        /// <param name="cursor">["<c>cursor</c>"] Page cursor</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitTrade[]>> GetTradeHistoryAsync(string symbol, DateTime? endTime = null, int? limit = null, string? cursor = null, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker info
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.upbit.com/kr/reference/list-tickers" /><br />
        /// Endpoint:<br />
        /// GET /v1/ticker
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>markets</c>"] The symbols, for example `USDT-ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker info
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.upbit.com/kr/reference/list-tickers" /><br />
        /// Endpoint:<br />
        /// GET /v1/ticker
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>markets</c>"] The symbols, for example `USDT-ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitTicker[]>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker info
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.upbit.com/kr/reference/list-tickers" /><br />
        /// Endpoint:<br />
        /// GET /v1/ticker/all
        /// </para>
        /// </summary>
        /// <param name="quoteAssets">["<c>quote_currencies</c>"] The quote assets, for example `KRW`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitTicker[]>> GetTickersByQuoteAssetsAsync(IEnumerable<string> quoteAssets, CancellationToken ct = default);

        /// <summary>
        /// Get order book info
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.upbit.com/kr/reference/list-orderbooks" /><br />
        /// Endpoint:<br />
        /// GET /v1/orderbook
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>markets</c>"] The symbol, for example `KRW-ETH`</param>
        /// <param name="levels">["<c>count</c>"] Number of rows</param>
        /// <param name="aggregation">["<c>level</c>"] Aggregation level</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitOrderBook>> GetOrderBookAsync(string symbol, int? levels = null, decimal? aggregation = null, CancellationToken ct = default);

        /// <summary>
        /// Get order book info
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.upbit.com/kr/reference/list-orderbooks" /><br />
        /// Endpoint:<br />
        /// GET /v1/orderbook
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>markets</c>"] The symbols, for example `KRW-ETH`</param>
        /// <param name="levels">["<c>count</c>"] Number of rows</param>
        /// <param name="aggregation">["<c>level</c>"] Aggregation level</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitOrderBook[]>> GetOrderBooksAsync(IEnumerable<string> symbols, int? levels = null, decimal? aggregation = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline/candlestick data. Note that entries might be missing if there is no data for an entry
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.upbit.com/kr/reference/list-candles-seconds" /><br />
        /// Endpoint:<br />
        /// GET /v1/candles/[seconds|minutes/{{unit}}|days|weeks|months|years]
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol, for example `USDT-ETH`</param>
        /// <param name="interval">Interval</param>
        /// <param name="endTime">["<c>to</c>"] End time</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime ? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get symbol config
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.upbit.com/kr/reference/list-orderbook-instruments" /><br />
        /// Endpoint:<br />
        /// GET /v1/orderbook/instruments
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>markets</c>"] The symbols, for example `USDT-ETH`, comma separated</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<UpbitSymbolConfig[]>> GetSymbolConfigAsync(string symbols, CancellationToken ct = default);

    }
}
