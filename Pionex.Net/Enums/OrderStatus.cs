using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Pionex.Net.Enums;

/// <summary>
/// Order status
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderStatus>))]
public enum OrderStatus
{
    /// <summary>
    /// ["<c>OPEN</c>"] Open
    /// </summary>
    [Map("OPEN")]
    Open,
    /// <summary>
    /// ["<c>CLOSED</c>"] Closed
    /// </summary>
    [Map("CLOSED")]
    Closed,
}
