using CryptoExchange.Net.Objects.Options;
using System;

namespace Pionex.Net.Objects.Options
{
    /// <summary>
    /// Options for the Pionex SymbolOrderBook
    /// </summary>
    public class PionexOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static PionexOrderBookOptions Default { get; set; } = new PionexOrderBookOptions();

        /// <summary>
        /// The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal PionexOrderBookOptions Copy()
        {
            var result = Copy<PionexOrderBookOptions>();
            result.Limit = Limit;
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}
