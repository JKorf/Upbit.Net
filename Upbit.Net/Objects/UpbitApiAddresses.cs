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
        /// The default addresses to connect to the Upbit API
        /// </summary>
        public static UpbitApiAddresses Default = new UpbitApiAddresses
        {
            RestClientAddress = "https://api.upbit.com/",
            SocketClientAddress = "wss://api.upbit.com/"
        };
    }
}
