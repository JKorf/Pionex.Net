using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Pionex.Net.Enums;

/// <summary>
/// Symbol type
/// </summary>
[JsonConverter(typeof(EnumConverter<SymbolType>))]
public enum SymbolType
{
    /// <summary>
    /// ["<c>SPOT</c>"] Spot symbol
    /// </summary>
    [Map("SPOT")]
    Spot,
    /// <summary>
    /// ["<c>PERP</c>"] Perp futures symbol
    /// </summary>
    [Map("PERP")]
    Perp,
}
