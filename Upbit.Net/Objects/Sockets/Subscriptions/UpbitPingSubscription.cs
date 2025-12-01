using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;

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
            MessageRouter = MessageRouter.CreateWithoutHandler<object>("status");
        }
    }
}
