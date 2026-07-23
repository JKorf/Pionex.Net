using System;
using System.Text.Json.Serialization;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

internal record PionexKlineWrapper
{
    /// <summary>
    /// ["<c>klines</c>"] Klines
    /// </summary>
    [JsonPropertyName("klines")]
    public PionexKline[] Klines { get; set; } = [];
}

/// <summary>
/// Kline info
/// </summary>
public record PionexKline
{
    /// <summary>
    /// ["<c>time</c>"] Open timestamp
    /// </summary>
    [JsonPropertyName("time")]
    public DateTime OpenTime { get; set; }
    /// <summary>
    /// ["<c>open</c>"] Open
    /// </summary>
    [JsonPropertyName("open")]
    public decimal OpenPrice { get; set; }
    /// <summary>
    /// ["<c>close</c>"] Close
    /// </summary>
    [JsonPropertyName("close")]
    public decimal ClosePrice { get; set; }
    /// <summary>
    /// ["<c>high</c>"] High
    /// </summary>
    [JsonPropertyName("high")]
    public decimal HighPrice { get; set; }
    /// <summary>
    /// ["<c>low</c>"] Low
    /// </summary>
    [JsonPropertyName("low")]
    public decimal LowPrice { get; set; }
    /// <summary>
    /// ["<c>volume</c>"] Volume
    /// </summary>
    [JsonPropertyName("volume")]
    public decimal Volume { get; set; }
}

