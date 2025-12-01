using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Sockets;
using Upbit.Net.Objects.Models;
using System.Collections.Generic;
using Upbit.Net.Enums;

namespace Upbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Upbit Spot streams
    /// </summary>
    public interface IUpbitSocketClientSpotApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribe to live trade updates
        /// <para><a href="https://global-docs.upbit.com/reference/websocket-trade" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `KRW-ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<UpbitTradeUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live trade updates
        /// <para><a href="https://global-docs.upbit.com/reference/websocket-trade" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `KRW-ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<UpbitTradeUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live symbol ticker updates
        /// <para><a href="https://global-docs.upbit.com/reference/websocket-ticker" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `KRW-ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<UpbitTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live symbol ticker updates
        /// <para><a href="https://global-docs.upbit.com/reference/websocket-ticker" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `KRW-ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<UpbitTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live order book updates
        /// <para><a href="https://docs.upbit.com/kr/reference/websocket-orderbook" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `KRW-ETH`</param>
        /// <param name="levels">Order book levels to push, 1, 5, 15 or 30</param>
        /// <param name="aggregation">Aggregation level</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int levels, Action<DataEvent<UpbitOrderBookUpdate>> onMessage, decimal? aggregation = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live order book updates
        /// <para><a href="https://docs.upbit.com/kr/reference/websocket-orderbook" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `KRW-ETH`</param>
        /// <param name="levels">Order book levels to push, 1, 5, 15 or 30</param>
        /// <param name="aggregation">Aggregation level</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, Action<DataEvent<UpbitOrderBookUpdate>> onMessage, decimal? aggregation = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live kline/candlestick updates
        /// <para><a href="https://global-docs.upbit.com/reference/websocket-candle" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `KRW-ETH`</param>
        /// <param name="interval">Interval (max 4 hours)</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<UpbitKlineUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live kline/candlestick updates
        /// <para><a href="https://global-docs.upbit.com/reference/websocket-candle" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `KRW-ETH`</param>
        /// <param name="interval">Interval (max 4 hours)</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<UpbitKlineUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IUpbitSocketClientSpotApiShared SharedClient { get; }
    }
}
