namespace Pionex.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class PionexApiAddresses
    {
        /// <summary>
        /// The address used by the PionexRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the PionexSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the Pionex API
        /// </summary>
        public static PionexApiAddresses Default = new PionexApiAddresses
        {
            RestClientAddress = "https://api.pionex.com",
            SocketClientAddress = "wss://ws.pionex.com"
        };
    }
}
