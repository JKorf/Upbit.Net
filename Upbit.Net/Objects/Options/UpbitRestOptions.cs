using CryptoExchange.Net.Objects.Options;

namespace Upbit.Net.Objects.Options
{
    /// <summary>
    /// Options for the UpbitRestClient
    /// </summary>
    public class UpbitRestOptions : RestExchangeOptions<UpbitEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static UpbitRestOptions Default { get; set; } = new UpbitRestOptions()
        {
            Environment = UpbitEnvironment.Live,
            AutoTimestamp = true
        };

        /// <summary>
        /// ctor
        /// </summary>
        public UpbitRestOptions()
        {
            Default?.Set(this);
        }

        
         /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();


        internal UpbitRestOptions Set(UpbitRestOptions targetOptions)
        {
            targetOptions = base.Set<UpbitRestOptions>(targetOptions);
            
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);

            return targetOptions;
        }
    }
}
