using Pionex.Net.Interfaces.Clients.SpotApi;

namespace Pionex.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of Pionex
    /// </summary>
    public interface IPionexSharedApiClient
    {
        /// <summary>
        /// REST shared API implementations
        /// </summary>
        IPionexRestClientSpotSharedApi SpotRest { get; }

        /// <summary>
        /// WebSocket shared API implementations
        /// </summary>
        IPionexSocketClientSpotSharedApi SpotSocket { get; }
    }
}
