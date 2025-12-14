using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Upbit.Net.Clients.MessageHandlers;
using Upbit.Net.Enums;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Objects.Models;
using Upbit.Net.Objects.Options;
using Upbit.Net.Objects.Sockets.Subscriptions;

namespace Upbit.Net.Clients.SpotApi
{
    /// <summary>
    /// Client providing access to the Upbit Spot websocket Api
    /// </summary>
    internal partial class UpbitSocketClientSpotApi : SocketApiClient, IUpbitSocketClientSpotApi
    {
        #region fields
        private static readonly MessagePath _typePath = MessagePath.Get().Property("type");
        private static readonly MessagePath _symbolPath = MessagePath.Get().Property("code");
        private static readonly MessagePath _errorPath = MessagePath.Get().Property("error").Property("name");
        private static readonly MessagePath _statusPath = MessagePath.Get().Property("status");

        private readonly TimeSpan _waitForErrorTimeout;

        protected override ErrorMapping ErrorMapping => UpbitErrors.Errors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal UpbitSocketClientSpotApi(ILogger logger, UpbitSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.SpotOptions)
        {
            _waitForErrorTimeout = options.SubscribeMaxWaitForError;

            RateLimiter = UpbitExchange.RateLimiter.Socket;

            AddSystemSubscription(new UpbitPingSubscription(_logger));
        }
        #endregion

        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor(WebSocketMessageType type) => new SystemTextJsonByteMessageAccessor(UpbitExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(UpbitExchange._serializerContext);

        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new UpbitSocketMessageHandler();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new UpbitAuthenticationProvider(credentials);


        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<UpbitTradeUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<UpbitTradeUpdate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, UpbitTradeUpdate>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<UpbitTradeUpdate>(UpbitExchange.ExchangeName, data, receiveTime, originalData)
                        .WithUpdateType(data.StreamType == StreamType.Snapshot ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithDataTimestamp(data.Timestamp)
                        .WithStreamId("trade")
                        .WithSymbol(data.Symbol)
                    );
            });

            var subscription = new UpbitSubscription<UpbitTradeUpdate>(_logger, this, "trade", symbols.ToArray(), null, null, internalHandler, false, _waitForErrorTimeout);
            return await SubscribeAsync(BaseAddress.AppendPath("websocket/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<UpbitTickerUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToTickerUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<UpbitTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, UpbitTickerUpdate>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<UpbitTickerUpdate>(UpbitExchange.ExchangeName, data, receiveTime, originalData)
                        .WithUpdateType(data.StreamType == StreamType.Snapshot ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithDataTimestamp(data.Timestamp)
                        .WithStreamId("ticker")
                        .WithSymbol(data.Symbol)
                    );
            });

            var subscription = new UpbitSubscription<UpbitTickerUpdate>(_logger, this, "ticker", symbols.ToArray(), null, null, internalHandler, false, _waitForErrorTimeout);
            return await SubscribeAsync(BaseAddress.AppendPath("websocket/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int levels, Action<DataEvent<UpbitOrderBookUpdate>> onMessage, decimal? aggregation = null, CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync([symbol], levels, onMessage, aggregation, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, Action<DataEvent<UpbitOrderBookUpdate>> onMessage, decimal? aggregation = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, UpbitOrderBookUpdate>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<UpbitOrderBookUpdate>(UpbitExchange.ExchangeName, data, receiveTime, originalData)
                        .WithUpdateType(data.StreamType == StreamType.Snapshot ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithDataTimestamp(data.Timestamp)
                        .WithStreamId("orderbook")
                        .WithSymbol(data.Symbol)
                    );
            });

            var subscription = new UpbitSubscription<UpbitOrderBookUpdate>(_logger, this, "orderbook", symbols.ToArray(), symbols.Select(x => x + "." + levels).ToArray(), aggregation, internalHandler, false, _waitForErrorTimeout);
            return await SubscribeAsync(BaseAddress.AppendPath("websocket/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<UpbitKlineUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync([symbol], interval, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<UpbitKlineUpdate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, UpbitKlineUpdate>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<UpbitKlineUpdate>(UpbitExchange.ExchangeName, data, receiveTime, originalData)
                        .WithUpdateType(data.StreamType == StreamType.Snapshot ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithDataTimestamp(data.Timestamp)
                        .WithStreamId("candle." + EnumConverter.GetString(interval))
                        .WithSymbol(data.Symbol)
                    );
            });

            var subscription = new UpbitSubscription<UpbitKlineUpdate>(_logger, this, "candle." + EnumConverter.GetString(interval), symbols.ToArray(), null, null, internalHandler, false, _waitForErrorTimeout);
            return await SubscribeAsync(BaseAddress.AppendPath("websocket/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            var type = message.GetValue<string>(_typePath);
            var symbol = message.GetValue<string>(_symbolPath);

            if (type == null)
            {
                var status = message.GetValue<string>(_statusPath);
                if (status != null)
                    return "status";

                var error = message.GetValue<string>(_errorPath);
                if (error != null)
                    return "error";
            }

            return type + symbol;
        }

        /// <inheritdoc />
        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection) => Task.FromResult<Query?>(null);

        /// <inheritdoc />
        public IUpbitSocketClientSpotApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => UpbitExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
