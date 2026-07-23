using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace Pionex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Pionex Spot API endpoints
    /// </summary>
    public interface IPionexRestClientSpotApi : IRestApiClient<PionexCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IPionexRestClientSpotApiAccount" />
        public IPionexRestClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IPionexRestClientSpotApiExchangeData" />
        public IPionexRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IPionexRestClientSpotApiTrading" />
        public IPionexRestClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IPionexRestClientSpotApiShared SharedClient { get; }
    }
}
