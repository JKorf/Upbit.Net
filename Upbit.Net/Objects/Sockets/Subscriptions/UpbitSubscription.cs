using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Upbit.Net.Objects.Internal;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class UpbitSubscription<T> : Subscription where T : UpbitSocketUpdate
    {
        private readonly TimeSpan _waitForErrorTimeout;
        private readonly SocketApiClient _client;
        private readonly Action<DateTime, string?, T> _handler;
        private readonly string _topic;
        private readonly string[] _symbols;
        private readonly string[] _codes;
        private readonly decimal? _level;

        /// <summary>
        /// ctor
        /// </summary>
        public UpbitSubscription(
            ILogger logger,
            SocketApiClient client,
            string topic,
            string[] symbols,
            string[]? codes, 
            decimal? level, 
            Action<DateTime, string?, T> handler,
            bool auth,
            TimeSpan waitForErrorTimeout) : base(logger, auth)
        {
            _client = client;
            _handler = handler;
            _topic = topic;
            _symbols = symbols;
            _codes = codes ?? symbols;
            _level = level;
            _waitForErrorTimeout = waitForErrorTimeout;

            MessageMatcher = MessageMatcher.Create<T>(_symbols.Select(x => _topic + x).ToArray(), DoHandleMessage);
            MessageRouter = MessageRouter.CreateWithTopicFilters<T>(_topic, _symbols, DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new UpbitQuery(_client, [new SocketTicket(ExchangeHelpers.NextId()), new SocketRequest
            {
                Topic = _topic,
                Codes = _codes,
                Level = _level,
            }], false, _waitForErrorTimeout);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            //_handler.Invoke(message.As(message.Data!, _topic, null, message.Data.StreamType == Enums.StreamType.Snapshot ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
