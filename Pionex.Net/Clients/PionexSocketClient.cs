using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Pionex.Net.Interfaces.Clients;
using Pionex.Net.Objects.Options;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Clients.SpotApi;

namespace Pionex.Net.Clients
{
    /// <inheritdoc cref="IPionexSocketClient" />
    public class PionexSocketClient : BaseSocketClient<PionexEnvironment, PionexCredentials>, IPionexSocketClient
    {
        #region Api clients

        /// <inheritdoc />
        public IPionexSocketClientSpotApi SpotApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of PionexSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public PionexSocketClient(Action<PionexSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of PionexSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public PionexSocketClient(IOptions<PionexSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Pionex")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new PionexSocketClientSpotApi(loggerFactory, options.Value));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<PionexSocketOptions> optionsDelegate)
        {
            PionexSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
