using System;
using System.Text.Json.Serialization;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

/// <summary>
/// Order result
/// </summary>
public record PionexOrderResult
{
    /// <summary>
    /// ["<c>orderId</c>"] Order id
    /// </summary>
    [JsonPropertyName("orderId")]
    public long OrderId { get; set; }
    /// <summary>
    /// ["<c>clientOrderId</c>"] Client order id
    /// </summary>
    [JsonPropertyName("clientOrderId")]
    public string? ClientOrderId { get; set; }
}

