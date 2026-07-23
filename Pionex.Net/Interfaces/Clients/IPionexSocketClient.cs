using CryptoExchange.Net.Interfaces.Clients;
using Pionex.Net.Interfaces.Clients.SpotApi;

namespace Pionex.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Pionex websocket API
    /// </summary>
    public interface IPionexSocketClient : ISocketClient<PionexCredentials>
    {
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="IPionexSocketClientSpotApi"/>
        public IPionexSocketClientSpotApi SpotApi { get; }
    }
}
