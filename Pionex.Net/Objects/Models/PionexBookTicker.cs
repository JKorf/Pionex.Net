using System;
using System.Text.Json.Serialization;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

internal record PionexBookTickerWrapper
{
    /// <summary>
    /// ["<c>tickers</c>"] Tickers
    /// </summary>
    [JsonPropertyName("tickers")]
    public PionexBookTicker[] Tickers { get; set; } = [];
}

/// <summary>
/// Book ticker
/// </summary>
public record PionexBookTicker
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>bidPrice</c>"] Best bid price
    /// </summary>
    [JsonPropertyName("bidPrice")]
    public decimal BidPrice { get; set; }
    /// <summary>
    /// ["<c>bidSize</c>"] Best bid quantity
    /// </summary>
    [JsonPropertyName("bidSize")]
    public decimal BidQuantity { get; set; }
    /// <summary>
    /// ["<c>askPrice</c>"] Best ask price
    /// </summary>
    [JsonPropertyName("askPrice")]
    public decimal AskPrice { get; set; }
    /// <summary>
    /// ["<c>askSize</c>"] Best ask quantity
    /// </summary>
    [JsonPropertyName("askSize")]
    public decimal AskQuantity { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

