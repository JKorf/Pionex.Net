using System;
using System.Text.Json.Serialization;
using Pionex.Net.Enums;

namespace Pionex.Net.Objects.Models;

internal record PionexSymbolWrapper
{
    /// <summary>
    /// ["<c>symbols</c>"] Symbols
    /// </summary>
    [JsonPropertyName("symbols")]
    public PionexSymbol[] Symbols { get; set; } = [];
}

/// <summary>
/// Symbol information
/// </summary>
public record PionexSymbol
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>name</c>"] Name (perps only)
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    /// <summary>
    /// ["<c>type</c>"] Symbol type
    /// </summary>
    [JsonPropertyName("type")]
    public SymbolType SymbolType { get; set; }
    /// <summary>
    /// ["<c>baseCurrency</c>"] Base asset
    /// </summary>
    [JsonPropertyName("baseCurrency")]
    public string BaseAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>quoteCurrency</c>"] Quote asset
    /// </summary>
    [JsonPropertyName("quoteCurrency")]
    public string QuoteAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>basePrecision</c>"] Base asset precision
    /// </summary>
    [JsonPropertyName("basePrecision")]
    public int BasePrecision { get; set; }
    /// <summary>
    /// ["<c>quotePrecision</c>"] Quote asset precision
    /// </summary>
    [JsonPropertyName("quotePrecision")]
    public int QuotePrecision { get; set; }
    /// <summary>
    /// ["<c>amountPrecision</c>"] Quantity precision
    /// </summary>
    [JsonPropertyName("amountPrecision")]
    public int QuantityPrecision { get; set; }
    /// <summary>
    /// ["<c>minNotional</c>"] Min perp notional value
    /// </summary>
    [JsonPropertyName("minNotional")]
    public decimal? MinPerpNotional { get; set; }
    /// <summary>
    /// ["<c>minAmount</c>"] Min spot order quantity
    /// </summary>
    [JsonPropertyName("minAmount")]
    public decimal? MinSpotQuantity { get; set; }
    /// <summary>
    /// ["<c>minTradeSize</c>"] Min trade quantity
    /// </summary>
    [JsonPropertyName("minTradeSize")]
    public decimal MinTradeQuantity { get; set; }
    /// <summary>
    /// ["<c>maxTradeSize</c>"] Max trade quantity
    /// </summary>
    [JsonPropertyName("maxTradeSize")]
    public decimal MaxTradeQuantity { get; set; }
    /// <summary>
    /// ["<c>minTradeDumping</c>"] Min market order sell quantity
    /// </summary>
    [JsonPropertyName("minTradeDumping")]
    public decimal MinMarketOrderSellQuantity { get; set; }
    /// <summary>
    /// ["<c>maxTradeDumping</c>"] Max market order sell quantity
    /// </summary>
    [JsonPropertyName("maxTradeDumping")]
    public decimal MaxMarketOrderSellQuantity { get; set; }
    /// <summary>
    /// ["<c>buyCeiling</c>"] Maximum buy price multiplier ratio
    /// </summary>
    [JsonPropertyName("buyCeiling")]
    public decimal BuyCeiling { get; set; }
    /// <summary>
    /// ["<c>sellFloor</c>"] Minimum sell price multiplier ratio
    /// </summary>
    [JsonPropertyName("sellFloor")]
    public decimal SellFloor { get; set; }
    /// <summary>
    /// ["<c>enable</c>"] Enabled
    /// </summary>
    [JsonPropertyName("enable")]
    public bool Enable { get; set; }
    /// <summary>
    /// ["<c>maxImpactMarket</c>"] Max impact market
    /// </summary>
    [JsonPropertyName("maxImpactMarket")]
    public decimal? MaxImpactMarket { get; set; }
    /// <summary>
    /// ["<c>liquidationFeeRate</c>"] Liquidation fee rate
    /// </summary>
    [JsonPropertyName("liquidationFeeRate")]
    public decimal? LiquidationFeeRate { get; set; }
}

