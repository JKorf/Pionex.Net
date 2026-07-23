using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pionex.Net.Interfaces.Clients;
using Pionex.Net.Objects.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace Pionex.Net.Clients
{
    /// <inheritdoc />
    public class PionexUserClientProvider : UserClientProvider<
        IPionexRestClient,
        IPionexSocketClient,
        PionexRestOptions,
        PionexSocketOptions,
        PionexCredentials,
        PionexEnvironment
        >, IPionexUserClientProvider
    {
        /// <inheritdoc />
        public override string ExchangeName => PionexExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public PionexUserClientProvider(Action<PionexOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public PionexUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<PionexRestOptions> restOptions,
            IOptions<PionexSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        protected override IPionexRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<PionexRestOptions> options)
            => new PionexRestClient(client, loggerFactory, options);
        /// <inheritdoc />
        protected override IPionexSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<PionexSocketOptions> options)
            => new PionexSocketClient(options, loggerFactory);
    }
}
