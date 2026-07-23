using System;
using System.Text.Json.Serialization;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

internal record PionexOrderWrapper
{
    /// <summary>
    /// ["<c>orders</c>"] Orders
    /// </summary>
    [JsonPropertyName("orders")]
    public PionexOrder[] Orders { get; set; } = [];
}

/// <summary>
/// Order info
/// </summary>
public record PionexOrder
{
    /// <summary>
    /// ["<c>orderId</c>"] Order id
    /// </summary>
    [JsonPropertyName("orderId")]
    public long OrderId { get; set; }
    /// <summary>
    /// ["<c>symbol</c>"] Symbol name
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>type</c>"] Type
    /// </summary>
    [JsonPropertyName("type")]
    public OrderType OrderType { get; set; }
    /// <summary>
    /// ["<c>side</c>"] Side
    /// </summary>
    [JsonPropertyName("side")]
    public OrderSide Side { get; set; }
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
    /// ["<c>amount</c>"] Quantity
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal QuoteQuantity { get; set; }
    /// <summary>
    /// ["<c>filledSize</c>"] Quantity filled
    /// </summary>
    [JsonPropertyName("filledSize")]
    public decimal QuantityFilled { get; set; }
    /// <summary>
    /// ["<c>filledAmount</c>"] Quantity filled
    /// </summary>
    [JsonPropertyName("filledAmount")]
    public decimal QuoteQuantityFilled { get; set; }
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
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public OrderStatus Status { get; set; }
    /// <summary>
    /// ["<c>IOC</c>"] Is IOC order
    /// </summary>
    [JsonPropertyName("IOC")]
    public bool IOC { get; set; }
    /// <summary>
    /// ["<c>clientOrderId</c>"] Client order id
    /// </summary>
    [JsonPropertyName("clientOrderId")]
    public string ClientOrderId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>source</c>"] Source
    /// </summary>
    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>createTime</c>"] Create time
    /// </summary>
    [JsonPropertyName("createTime")]
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// ["<c>updateTime</c>"] Update time
    /// </summary>
    [JsonPropertyName("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

