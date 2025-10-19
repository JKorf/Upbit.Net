using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Upbit.Net.Objects.Internal;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class UpbitSubscription<T> : Subscription<object, object> where T : UpbitSocketUpdate
    {
        private readonly Action<DataEvent<T>> _handler;
        private readonly string _topic;
        private readonly string[] _symbols;

        /// <summary>
        /// ctor
        /// </summary>
        public UpbitSubscription(ILogger logger, string topic, string[] symbols, Action<DataEvent<T>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _topic = topic;
            _symbols = symbols;

            MessageMatcher = MessageMatcher.Create<T>(_symbols.Select(x => _topic + x).ToArray(), DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new UpbitQuery([new SocketTicket(ExchangeHelpers.NextId()), new SocketRequest
            {
                Topic = _topic,
                Codes = _symbols
            }], false);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<T> message)
        {
            _handler.Invoke(message.As(message.Data!, _topic, null, message.Data.StreamType == Enums.StreamType.Snapshot ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
