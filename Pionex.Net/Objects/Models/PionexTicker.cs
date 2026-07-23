using System;
using System.Text.Json.Serialization;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

internal record PionexTickerWrapper
{
    /// <summary>
    /// ["<c>tickers</c>"] Tickers
    /// </summary>
    [JsonPropertyName("tickers")]
    public PionexTicker[] Tickers { get; set; } = [];
}

/// <summary>
/// Price ticker
/// </summary>
public record PionexTicker
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>time</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("time")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>open</c>"] Open price
    /// </summary>
    [JsonPropertyName("open")]
    public decimal OpenPrice { get; set; }
    /// <summary>
    /// ["<c>close</c>"] Close price
    /// </summary>
    [JsonPropertyName("close")]
    public decimal ClosePrice { get; set; }
    /// <summary>
    /// ["<c>high</c>"] High price
    /// </summary>
    [JsonPropertyName("high")]
    public decimal HighPrice { get; set; }
    /// <summary>
    /// ["<c>low</c>"] Low price
    /// </summary>
    [JsonPropertyName("low")]
    public decimal LowPrice { get; set; }
    /// <summary>
    /// ["<c>volume</c>"] Quantity in base asset
    /// </summary>
    [JsonPropertyName("volume")]
    public decimal Volume { get; set; }
    /// <summary>
    /// ["<c>amount</c>"] Quantity in quote asset
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal VolumeQuote { get; set; }
    /// <summary>
    /// ["<c>count</c>"] Trade count
    /// </summary>
    [JsonPropertyName("count")]
    public long TradeCount { get; set; }
}

