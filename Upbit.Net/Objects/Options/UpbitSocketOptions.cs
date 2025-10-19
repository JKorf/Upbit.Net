using CryptoExchange.Net.Objects.Options;

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
            SocketSubscriptionsCombineTarget = 10
        };


        /// <summary>
        /// ctor
        /// </summary>
        public UpbitSocketOptions()
        {
            Default?.Set(this);
        }


        
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
