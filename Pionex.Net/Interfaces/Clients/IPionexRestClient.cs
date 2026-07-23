using CryptoExchange.Net.Interfaces.Clients;
using Pionex.Net.Interfaces.Clients.SpotApi;

namespace Pionex.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Pionex Rest API. 
    /// </summary>
    public interface IPionexRestClient : IRestClient<PionexCredentials>
    {
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="IPionexRestClientSpotApi"/>
        public IPionexRestClientSpotApi SpotApi { get; }

    }
}
