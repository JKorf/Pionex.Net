using Pionex.Net.Interfaces.Clients;
using Pionex.Net.Interfaces.Clients.SpotApi;

namespace Pionex.Net.Clients
{
    /// <inheritdoc />
    public class PionexSharedApiClient : IPionexSharedApiClient
    {
        /// <inheritdoc />
        public IPionexRestClientSpotSharedApi SpotRest { get; }
        /// <inheritdoc />
        public IPionexSocketClientSpotSharedApi SpotSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public PionexSharedApiClient(
            IPionexRestClient restClient,
            IPionexSocketClient socketClient)
        {
            SpotRest = restClient.SpotApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
        }
    }
}
