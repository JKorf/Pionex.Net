using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Pionex.Net.Enums;

/// <summary>
/// Kline interval
/// </summary>
[JsonConverter(typeof(EnumConverter<KlineInterval>))]
public enum KlineInterval
{
    /// <summary>
    /// ["<c>1M</c>"] One minute
    /// </summary>
    [Map("1M")]
    OneMinute = 60,
    /// <summary>
    /// ["<c>5M</c>"] Five minutes
    /// </summary>
    [Map("5M")]
    FiveMinutes = 60 * 5,
    /// <summary>
    /// ["<c>15M</c>"] Fifteen minutes
    /// </summary>
    [Map("15M")]
    FifteenMinutes = 60 * 15,
    /// <summary>
    /// ["<c>30M</c>"] Thirty minutes
    /// </summary>
    [Map("30M")]
    ThirtyMinutes = 60 * 30,
    /// <summary>
    /// ["<c>60M</c>"] One hour
    /// </summary>
    [Map("60M")]
    OneHour = 60 * 60,
    /// <summary>
    /// ["<c>4H</c>"] Four hours
    /// </summary>
    [Map("4H")]
    FourHours = 60 * 60 * 4,
    /// <summary>
    /// ["<c>8H</c>"] Eight hours
    /// </summary>
    [Map("8H")]
    EightHours = 60 * 60 * 8,
    /// <summary>
    /// ["<c>12H</c>"] Twelve hours
    /// </summary>
    [Map("12H")]
    TwelveHours = 60 * 60 * 12,
    /// <summary>
    /// ["<c>1D</c>"] One day
    /// </summary>
    [Map("1D")]
    OneDay = 60 * 60 * 24,
}
