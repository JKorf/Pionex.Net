// 05-error-handling.cs
//
// Demonstrates: HttpResult checks, error metadata, and transient-only retries.
//
// Setup: dotnet add package Pionex.Net

using CryptoExchange.Net.Objects;
using Pionex.Net.Clients;
using Pionex.Net.Enums;

var client = new PionexRestClient();

var tickerResult = await client.SpotApi.ExchangeData.GetTickersAsync(
    "BTC_USDT",
    SymbolType.Spot);

if (tickerResult.Success)
{
    Console.WriteLine($"Close price: {tickerResult.Data.Single().ClosePrice}");
}
else
{
    LogError("ticker", tickerResult);
}

var klinesResult = await WithTransientRetryAsync(
    () => client.SpotApi.ExchangeData.GetKlinesAsync(
        "BTC_USDT",
        KlineInterval.OneMinute,
        limit: 100));

if (!klinesResult.Success)
{
    LogError("klines", klinesResult);
    return;
}

Console.WriteLine($"Received {klinesResult.Data.Length} klines");

static void LogError<T>(string operation, HttpResult<T> result)
{
    Console.WriteLine($"{operation} failed");
    Console.WriteLine($"Code: {result.Error?.Code}");
    Console.WriteLine($"Message: {result.Error?.Message}");
    Console.WriteLine($"Type: {result.Error?.ErrorType}");
    Console.WriteLine($"Transient: {result.Error?.IsTransient}");
}

static async Task<HttpResult<T>> WithTransientRetryAsync<T>(
    Func<Task<HttpResult<T>>> action,
    int maxAttempts = 3)
{
    HttpResult<T> result = default!;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        result = await action();
        if (result.Success || result.Error?.IsTransient != true)
            return result;

        await Task.Delay(TimeSpan.FromMilliseconds(250 * Math.Pow(2, attempt - 1)));
    }

    return result;
}

// Normal API failures are returned in result.Error rather than thrown.
// Retry only transient failures. Invalid requests, bad credentials, and
// insufficient balances should be fixed or surfaced to the caller.
