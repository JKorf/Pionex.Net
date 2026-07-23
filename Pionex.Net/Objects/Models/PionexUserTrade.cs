using System;
using System.Text.Json.Serialization;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

internal record PionexUserTradeWrapper
{
    /// <summary>
    /// ["<c>fills</c>"] Fills
    /// </summary>
    [JsonPropertyName("fills")]
    public PionexUserTrade[] Fills { get; set; } = [];
}

/// <summary>
/// User trade info
/// </summary>
public record PionexUserTrade
{
    /// <summary>
    /// ["<c>id</c>"] Id
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }
    /// <summary>
    /// ["<c>orderId</c>"] Order id
    /// </summary>
    [JsonPropertyName("orderId")]
    public long OrderId { get; set; }
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>side</c>"] Side
    /// </summary>
    [JsonPropertyName("side")]
    public OrderSide Side { get; set; }
    /// <summary>
    /// ["<c>role</c>"] Role
    /// </summary>
    [JsonPropertyName("role")]
    public TradeRole Role { get; set; }
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    /// <summary>
    /// ["<c>size</c>"] Quantity
    /// </summary>
    [JsonPropertyName("size")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>fee</c>"] Fee
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }
    /// <summary>
    /// ["<c>feeCoin</c>"] Fee asset
    /// </summary>
    [JsonPropertyName("feeCoin")]
    public string FeeAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

