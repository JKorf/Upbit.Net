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
    internal class UpbitPingSubscription : SystemSubscription
    {
        /// <summary>
        /// ctor
        /// </summary>
        public UpbitPingSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<object>("status");
        }
    }
}
