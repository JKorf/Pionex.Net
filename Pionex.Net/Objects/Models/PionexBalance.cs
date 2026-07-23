using System;
using System.Text.Json.Serialization;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

internal record PionexBalanceWrapper
{
    /// <summary>
    /// ["<c>balances</c>"] Balances
    /// </summary>
    [JsonPropertyName("balances")]
    public PionexBalance[] Balances { get; set; } = [];
}

/// <summary>
/// Balance
/// </summary>
public record PionexBalance
{
    /// <summary>
    /// ["<c>coin</c>"] Asset
    /// </summary>
    [JsonPropertyName("coin")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>free</c>"] Free
    /// </summary>
    [JsonPropertyName("free")]
    public decimal Free { get; set; }
    /// <summary>
    /// ["<c>frozen</c>"] Frozen
    /// </summary>
    [JsonPropertyName("frozen")]
    public decimal Frozen { get; set; }
}

