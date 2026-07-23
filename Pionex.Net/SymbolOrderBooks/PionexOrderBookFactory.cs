using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Pionex.Net.Interfaces;
using Pionex.Net.Interfaces.Clients;
using Pionex.Net.Objects.Options;

namespace Pionex.Net.SymbolOrderBooks
{
    /// <summary>
    /// Pionex order book factory
    /// </summary>
    public class PionexOrderBookFactory : IPionexOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public PionexOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            Spot = new OrderBookFactory<PionexOrderBookOptions>(CreateSpot, Create);
        }

        /// <inheritdoc />
        public IOrderBookFactory<PionexOrderBookOptions> Spot { get; }

        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<PionexOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(PionexExchange.FormatSymbol);
            return CreateSpot(symbolName, options);
        }

                 /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<PionexOrderBookOptions>? options = null)
            => new PionexSpotSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IPionexSocketClient>());


    }
}
