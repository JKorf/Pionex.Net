using CryptoExchange.Net.Objects.Options;
using System;

namespace Pionex.Net.Objects.Options
{
    /// <summary>
    /// Options for the PionexSocketClient
    /// </summary>
    public class PionexSocketOptions : SocketExchangeOptions<PionexEnvironment, PionexCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static PionexSocketOptions Default { get; set; } = new PionexSocketOptions()
        {
            Environment = PionexEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10,
            MaxSocketConnections = 10,
            SocketNoDataTimeout = TimeSpan.FromSeconds(25) // Heartbeat is normally received every 15 seconds
        };

        /// <summary>
        /// ctor
        /// </summary>
        public PionexSocketOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// Spot API options
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions();

        internal PionexSocketOptions Set(PionexSocketOptions targetOptions)
        {
            targetOptions = base.Set<PionexSocketOptions>(targetOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);

            return targetOptions;
        }
    }
}
