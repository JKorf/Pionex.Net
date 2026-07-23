using CryptoExchange.Net.Objects.Options;

namespace Pionex.Net.Objects.Options
{
    /// <summary>
    /// Options for the PionexRestClient
    /// </summary>
    public class PionexRestOptions : RestExchangeOptions<PionexEnvironment, PionexCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static PionexRestOptions Default { get; set; } = new PionexRestOptions()
        {
            Environment = PionexEnvironment.Live,
            AutoTimestamp = true
        };

        /// <summary>
        /// ctor
        /// </summary>
        public PionexRestOptions()
        {
            Default?.Set(this);
        }

                 /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();


        internal PionexRestOptions Set(PionexRestOptions targetOptions)
        {
            targetOptions = base.Set<PionexRestOptions>(targetOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);

            return targetOptions;
        }
    }
}
