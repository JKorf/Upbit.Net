namespace Upbit.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class UpbitApiAddresses
    {
        /// <summary>
        /// The address used by the UpbitRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the UpbitSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the Upbit South Korea API
        /// </summary>
        public static UpbitApiAddresses Default = new UpbitApiAddresses
        {
            RestClientAddress = "https://api.upbit.com/",
            SocketClientAddress = "wss://api.upbit.com/"
        };

        /// <summary>
        /// The default addresses to connect to the Upbit Singapore API
        /// </summary>
        public static UpbitApiAddresses Singapore = new UpbitApiAddresses
        {
            RestClientAddress = "https://sg-api.upbit.com/",
            SocketClientAddress = "wss://sg-api.upbit.com/"
        };

        /// <summary>
        /// The default addresses to connect to the Upbit Indonesia API
        /// </summary>
        public static UpbitApiAddresses Indonesia = new UpbitApiAddresses
        {
            RestClientAddress = "https://id-api.upbit.com/",
            SocketClientAddress = "wss://id-api.upbit.com/"
        };

        /// <summary>
        /// The default addresses to connect to the Upbit Thailand API
        /// </summary>
        public static UpbitApiAddresses Thailand = new UpbitApiAddresses
        {
            RestClientAddress = "https://th-api.upbit.com/",
            SocketClientAddress = "wss://th-api.upbit.com/"
        };
    }
}
