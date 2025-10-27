using CryptoExchange.Net.Objects.Options;
using System;

namespace Upbit.Net.Objects.Options
{
    /// <summary>
    /// Options for the UpbitSocketClient
    /// </summary>
    public class UpbitSocketOptions : SocketExchangeOptions<UpbitEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static UpbitSocketOptions Default { get; set; } = new UpbitSocketOptions()
        {
            Environment = UpbitEnvironment.Live,
            // Since there is no unsubscribe functionality only allow a single subscription per connection so we can close the connection to unsub
            SocketSubscriptionsCombineTarget = 1 
        };

        /// <summary>
        /// ctor
        /// </summary>
        public UpbitSocketOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// The server only replies with a message when there is an error in the subscription, not when it's successful. This timeout determines how
        /// long to wait at max for an error message before the subscription is assumed successful. Note that when data is received on the subscription
        /// before this timeout it is also deemed successful
        /// </summary>
        public TimeSpan SubscribeMaxWaitForError { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Spot API options
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions();

        internal UpbitSocketOptions Set(UpbitSocketOptions targetOptions)
        {
            targetOptions = base.Set<UpbitSocketOptions>(targetOptions);
            
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);

            return targetOptions;
        }
    }
}
