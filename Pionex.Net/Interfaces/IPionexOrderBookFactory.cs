using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using Pionex.Net.Objects.Options;

namespace Pionex.Net.Interfaces
{
    /// <summary>
    /// Pionex local order book factory
    /// </summary>
    public interface IPionexOrderBookFactory
    {
        /// <summary>
        /// Spot order book factory methods
        /// </summary>
        IOrderBookFactory<PionexOrderBookOptions> Spot { get; }

        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<PionexOrderBookOptions>? options = null);

        /// <summary>
        /// Create a new Spot local order book instance
        /// </summary>
        ISymbolOrderBook CreateSpot(string symbol, Action<PionexOrderBookOptions>? options = null);

    }
}