using CryptoExchange.Net.Objects;
using Upbit.Net.Objects;

namespace Upbit.Net
{
    /// <summary>
    /// Upbit environments
    /// </summary>
    public class UpbitEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Rest API address
        /// </summary>
        public string RestClientAddress { get; }

        /// <summary>
        /// Socket API address
        /// </summary>
        public string SocketClientAddress { get; }

        internal UpbitEnvironment(
            string name,
            string restAddress,
            string streamAddress) :
            base(name)
        {
            RestClientAddress = restAddress;
            SocketClientAddress = streamAddress;
        }

        /// <summary>
        /// ctor for DI, use <see cref="CreateCustom"/> for creating a custom environment
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public UpbitEnvironment() : base(TradeEnvironmentNames.Live)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        { }

        /// <summary>
        /// Get the Upbit environment by name
        /// </summary>
        public static UpbitEnvironment? GetEnvironmentByName(string? name)
         => name switch
         {
             TradeEnvironmentNames.Live => Live,
             "live-singapore" => Singapore,
             "live-indonesia" => Indonesia,
             "live-thailand" => Thailand,
             "" => Live,
             null => Live,
             _ => default
         };

        /// <summary>
        /// Available environment names
        /// </summary>
        /// <returns></returns>
        public static string[] All => [Live.Name, Singapore.Name, Indonesia.Name, Thailand.Name];

        /// <summary>
        /// Live South Korea environment
        /// </summary>
        public static UpbitEnvironment Live { get; }
            = new UpbitEnvironment(TradeEnvironmentNames.Live,
                                     UpbitApiAddresses.Default.RestClientAddress,
                                     UpbitApiAddresses.Default.SocketClientAddress);

        /// <summary>
        /// Live Singapore environment
        /// </summary>
        public static UpbitEnvironment Singapore { get; }
            = new UpbitEnvironment("live-singapore",
                                     UpbitApiAddresses.Singapore.RestClientAddress,
                                     UpbitApiAddresses.Singapore.SocketClientAddress);

        /// <summary>
        /// Live Indonesia environment
        /// </summary>
        public static UpbitEnvironment Indonesia { get; }
            = new UpbitEnvironment("live-indonesia",
                                     UpbitApiAddresses.Indonesia.RestClientAddress,
                                     UpbitApiAddresses.Indonesia.SocketClientAddress);

        /// <summary>
        /// Live Thailand environment
        /// </summary>
        public static UpbitEnvironment Thailand { get; }
            = new UpbitEnvironment("live-thailand",
                                     UpbitApiAddresses.Thailand.RestClientAddress,
                                     UpbitApiAddresses.Thailand.SocketClientAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spotRestAddress"></param>
        /// <param name="spotSocketStreamsAddress"></param>
        /// <returns></returns>
        public static UpbitEnvironment CreateCustom(
                        string name,
                        string spotRestAddress,
                        string spotSocketStreamsAddress)
            => new UpbitEnvironment(name, spotRestAddress, spotSocketStreamsAddress);
    }
}
