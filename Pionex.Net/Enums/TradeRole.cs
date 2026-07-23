using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Pionex.Net.Enums;

/// <summary>
/// Trade role
/// </summary>
[JsonConverter(typeof(EnumConverter<TradeRole>))]
public enum TradeRole
{
    /// <summary>
    /// ["<c>TAKER</c>"] Taker
    /// </summary>
    [Map("TAKER")]
    Taker,
    /// <summary>
    /// ["<c>MAKER</c>"] Maker
    /// </summary>
    [Map("MAKER")]
    Maker,
}
