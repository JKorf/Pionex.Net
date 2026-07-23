using CryptoExchange.Net.Objects;
using Pionex.Net.Objects.Internal;
using Pionex.Net.Objects.Models;
using System;
using System.Text.Json.Serialization;

namespace Pionex.Net.Converters
{
    [JsonSerializable(typeof(PionexResult<PionexSymbolWrapper>))]
    [JsonSerializable(typeof(PionexResult<PionexTradeWrapper>))]
    [JsonSerializable(typeof(PionexResult<PionexOrderBook>))]
    [JsonSerializable(typeof(PionexResult<PionexTickerWrapper>))]
    [JsonSerializable(typeof(PionexResult<PionexBookTickerWrapper>))]
    [JsonSerializable(typeof(PionexResult<PionexKlineWrapper>))]
    [JsonSerializable(typeof(PionexResult<PionexBalanceWrapper>))]
    [JsonSerializable(typeof(PionexResult<PionexOrderResult>))]
    [JsonSerializable(typeof(PionexResult<PionexOrder>))]
    [JsonSerializable(typeof(PionexResult<PionexOrderWrapper>))]
    [JsonSerializable(typeof(PionexResult<PionexUserTradeWrapper>))]
    [JsonSerializable(typeof(PionexResult<PionexBalanceDetails>))]
    [JsonSerializable(typeof(PionexResult))]

    [JsonSerializable(typeof(PionexSocketUpdate<PionexBalanceWrapper>))]
    [JsonSerializable(typeof(PionexSocketUpdate<PionexUserTrade>))]
    [JsonSerializable(typeof(PionexSocketUpdate<PionexOrder>))]
    [JsonSerializable(typeof(PionexSocketUpdate<PionexTrade[]>))]
    [JsonSerializable(typeof(PionexSocketUpdate<PionexOrderBook>))]
    [JsonSerializable(typeof(PionexSocketPingMessage))]
    [JsonSerializable(typeof(PionexSocketRequest))]
    [JsonSerializable(typeof(PionexSocketResponse))]

    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    [JsonSerializable(typeof(Parameters))]
    internal partial class PionexSourceGenerationContext : JsonSerializerContext
    {
    }
}
