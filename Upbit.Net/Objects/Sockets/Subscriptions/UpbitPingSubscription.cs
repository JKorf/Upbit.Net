using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
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
            MessageRouter = MessageRouter.CreateWithoutHandler<object>("status");
        }
    }
}
