using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

/// <summary>
/// Balances details
/// </summary>
public record PionexBalanceDetails
{
    /// <summary>
    /// ["<c>prices</c>"] Prices
    /// </summary>
    [JsonPropertyName("prices")]
    public Dictionary<string, PionexBalanceDetailsAsset> Prices { get; set; } = new();
    /// <summary>
    /// ["<c>totalInUsdt</c>"] Total value in usdt
    /// </summary>
    [JsonPropertyName("totalInUsdt")]
    public decimal TotalInUsdt { get; set; }
    /// <summary>
    /// ["<c>totalInBtc</c>"] Total value in btc
    /// </summary>
    [JsonPropertyName("totalInBtc")]
    public decimal TotalInBtc { get; set; }
    /// <summary>
    /// ["<c>botAccount</c>"] Bot account info
    /// </summary>
    [JsonPropertyName("botAccount")]
    public PionexBalanceDetailsBot BotAccount { get; set; } = null!;
    /// <summary>
    /// ["<c>traderAccount</c>"] Trader account info
    /// </summary>
    [JsonPropertyName("traderAccount")]
    public PionexBalanceDetailsTrader TraderAccount { get; set; } = null!;
}

/// <summary>
/// Asset balances
/// </summary>
public record PionexBalanceDetailsAsset
{
    /// <summary>
    /// ["<c>priceInUsd</c>"] Price in usd
    /// </summary>
    [JsonPropertyName("priceInUsd")]
    public decimal PriceInUsd { get; set; }
    /// <summary>
    /// ["<c>priceInBtc</c>"] Price in btc
    /// </summary>
    [JsonPropertyName("priceInBtc")]
    public decimal PriceInBtc { get; set; }
    /// <summary>
    /// ["<c>change24hInUsd</c>"] Change 24h in usd
    /// </summary>
    [JsonPropertyName("change24hInUsd")]
    public decimal Change24hInUsd { get; set; }
    /// <summary>
    /// ["<c>change24hInBtc</c>"] Change 24h in btc
    /// </summary>
    [JsonPropertyName("change24hInBtc")]
    public decimal Change24hInBtc { get; set; }
    /// <summary>
    /// ["<c>fullName</c>"] Full name
    /// </summary>
    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;
}

/// <summary>
/// Bot info
/// </summary>
public record PionexBalanceDetailsBot
{
    /// <summary>
    /// ["<c>totalInUsdt</c>"] Total in usdt
    /// </summary>
    [JsonPropertyName("totalInUsdt")]
    public decimal TotalInUsdt { get; set; }
    /// <summary>
    /// ["<c>detail</c>"] Details
    /// </summary>
    [JsonPropertyName("detail")]
    public PionexBalanceDetailsBotAccount[] Details { get; set; } = [];
}

/// <summary>
/// Account details
/// </summary>
public record PionexBalanceDetailsBotAccount
{
    /// <summary>
    /// ["<c>type</c>"] Type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>title</c>"] Title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>totalInUsdt</c>"] Total in usdt
    /// </summary>
    [JsonPropertyName("totalInUsdt")]
    public decimal TotalInUsdt { get; set; }
    /// <summary>
    /// ["<c>hasMore</c>"] Has more
    /// </summary>
    [JsonPropertyName("hasMore")]
    public bool HasMore { get; set; }
    /// <summary>
    /// ["<c>count</c>"] Count
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; set; }
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>list</c>"] List
    /// </summary>
    [JsonPropertyName("list")]
    public PionexBalanceDetailsBotAccountBalance[] List { get; set; } = [];
}

/// <summary>
/// Account balance
/// </summary>
public record PionexBalanceDetailsBotAccountBalance
{
    /// <summary>
    /// ["<c>displayName</c>"] Display name
    /// </summary>
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>coin</c>"] Asset
    /// </summary>
    [JsonPropertyName("coin")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>assets</c>"] Assets
    /// </summary>
    [JsonPropertyName("assets")]
    public decimal Assets { get; set; }
    /// <summary>
    /// ["<c>free</c>"] Free
    /// </summary>
    [JsonPropertyName("free")]
    public decimal Free { get; set; }
    /// <summary>
    /// ["<c>loan</c>"] Loan
    /// </summary>
    [JsonPropertyName("loan")]
    public decimal? Loan { get; set; }
    /// <summary>
    /// ["<c>frozenByOrder</c>"] Frozen by order
    /// </summary>
    [JsonPropertyName("frozenByOrder")]
    public decimal FrozenByOrder { get; set; }
    /// <summary>
    /// ["<c>interest</c>"] Interest
    /// </summary>
    [JsonPropertyName("interest")]
    public decimal? Interest { get; set; }
    /// <summary>
    /// ["<c>frozenByCoinout</c>"] Frozen by coinout
    /// </summary>
    [JsonPropertyName("frozenByCoinout")]
    public decimal FrozenByCoinout { get; set; }
    /// <summary>
    /// ["<c>frozen</c>"] Frozen
    /// </summary>
    [JsonPropertyName("frozen")]
    public decimal Frozen { get; set; }
    /// <summary>
    /// ["<c>debts</c>"] Debts
    /// </summary>
    [JsonPropertyName("debts")]
    public decimal? Debts { get; set; }
    /// <summary>
    /// ["<c>payable</c>"] Payable
    /// </summary>
    [JsonPropertyName("payable")]
    public decimal? Payable { get; set; }
    /// <summary>
    /// ["<c>frozenByMoveout</c>"] Frozen by moveout
    /// </summary>
    [JsonPropertyName("frozenByMoveout")]
    public decimal FrozenByMoveout { get; set; }
}

/// <summary>
/// Trader info
/// </summary>
public record PionexBalanceDetailsTrader
{
    /// <summary>
    /// ["<c>totalInUsdt</c>"] Total in usdt
    /// </summary>
    [JsonPropertyName("totalInUsdt")]
    public decimal TotalInUsdt { get; set; }
    /// <summary>
    /// ["<c>title</c>"] Title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>detail</c>"] Details
    /// </summary>
    [JsonPropertyName("detail")]
    public PionexBalanceDetailsTraderDetails[] Details { get; set; } = [];
}

/// <summary>
/// Balance details
/// </summary>
public record PionexBalanceDetailsTraderDetails
{
    /// <summary>
    /// ["<c>type</c>"] Type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>title</c>"] Title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>totalInUsdt</c>"] Total in usdt
    /// </summary>
    [JsonPropertyName("totalInUsdt")]
    public decimal TotalInUsdt { get; set; }

    // Additional properties to be added when futures trading enabled
}
