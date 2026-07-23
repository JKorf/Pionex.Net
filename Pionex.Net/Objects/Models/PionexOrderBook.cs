using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

/// <summary>
/// Order book snapshot
/// </summary>
public record PionexOrderBook
{
    /// <summary>
    /// ["<c>updateTime</c>"] Update time
    /// </summary>
    [JsonPropertyName("updateTime")]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// ["<c>asks</c>"] Asks list
    /// </summary>
    [JsonPropertyName("asks")]
    public PionexOrderBookEntry[] Asks { get; set; } = [];
    /// <summary>
    /// ["<c>bids</c>"] Bids list
    /// </summary>
    [JsonPropertyName("bids")]
    public PionexOrderBookEntry[] Bids { get; set; } = [];
}

/// <summary>
/// Order book entry
/// </summary>
[JsonConverter(typeof(ArrayConverter<PionexOrderBookEntry>))]
public record PionexOrderBookEntry : ISymbolOrderBookEntry
{
    /// <summary>
    /// Price
    /// </summary>
    [ArrayProperty(0)]
    public decimal Price { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    [ArrayProperty(1)]
    public decimal Quantity { get; set; }

}