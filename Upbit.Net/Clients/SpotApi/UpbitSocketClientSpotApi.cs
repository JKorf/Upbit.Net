using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
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
        private static readonly MessagePath _statusPath = MessagePath.Get().Property("status");

        protected override ErrorMapping ErrorMapping => UpbitErrors.Errors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal UpbitSocketClientSpotApi(ILogger logger, UpbitSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.SpotOptions)
        {
            AddSystemSubscription(new UpbitPingSubscription(_logger));
        }
        #endregion

        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor(WebSocketMessageType type) => new SystemTextJsonByteMessageAccessor(UpbitExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(UpbitExchange._serializerContext);

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new UpbitAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<UpbitTradeUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new UpbitSubscription<UpbitTradeUpdate>(_logger, "trade", new [] { symbol }, onMessage, false);
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
