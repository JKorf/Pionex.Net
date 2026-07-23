// 04-multi-exchange.cs
//
// Demonstrates: exchange-agnostic market data through CryptoExchange.Net
// SharedApis. The same request model works with other supported libraries.
//
// Setup: dotnet add package Pionex.Net

using CryptoExchange.Net.SharedApis;
using Pionex.Net.Clients;

ISpotTickerRestClient pionex = new PionexRestClient().SpotApi.SharedClient;
var symbol = new SharedSymbol(TradingMode.Spot, "BTC", "USDT");

// Discover reports which shared interfaces and request options are supported.
var capabilities = pionex.Discover();
Console.WriteLine($"Shared features: {capabilities.Features.Count(x => x.Supported)}");

await PrintTickerAsync(pionex, symbol);

static async Task PrintTickerAsync(
    ISpotTickerRestClient client,
    SharedSymbol symbol)
{
    var result = await client.GetSpotTickerAsync(new GetTickerRequest(symbol));
    if (!result.Success)
    {
        Console.WriteLine($"[{client.Exchange}] Ticker failed: {result.Error}");
        return;
    }

    Console.WriteLine(
        $"[{client.Exchange}] {result.Data.Symbol}: {result.Data.LastPrice}");
}

// Swap only the concrete client to query another exchange:
// ISpotTickerRestClient binance =
//     new BinanceRestClient().SpotApi.SharedClient;
// await PrintTickerAsync(binance, symbol);
