using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace Pionex.Net.Objects.Options
{
    /// <summary>
    /// Pionex options
    /// </summary>
    public class PionexOptions : LibraryOptions<PionexRestOptions, PionexSocketOptions, PionexCredentials, PionexEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}
