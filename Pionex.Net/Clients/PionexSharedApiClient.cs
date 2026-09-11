using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;
using Pionex.Net.Interfaces.Clients;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Options;

namespace Pionex.Net.Clients
{
    /// <inheritdoc />
    public class PionexSharedApiClient : SharedApiClientBase, IPionexSharedApiClient
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
            IPionexSocketClient socketClient,
            IOptions<PionexOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                  restClient.SpotApi.SharedApi,
                  socketClient.SpotApi.SharedApi
                  )
        {
            SpotRest = restClient.SpotApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
        }
    }
}
